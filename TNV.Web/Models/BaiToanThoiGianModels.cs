using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using System.Data.Linq;

namespace TNV.Web.Models
{

    #region Định nghĩa các Models

    public class BaiToanThoiGianModel
    {
        [DisplayName("Mã câu hỏi:")]
        public Guid MaCauHoi { get; set; }

        [DisplayName("Giá trị giờ:")]
        public int Gio { get; set; }

        [DisplayName("Giá trị phút:")]
        public int Phut { get; set; }

        [DisplayName("Giá trị giây:")]
        public int Giay { get; set; }

        [DisplayName("Nội dung đáp án:")]
        public string DapAn { get; set; }
        
        [DisplayName("Nội dung đáp án sai:")]
        public string DapAnSai { get; set; }

        [DisplayName("Số lượng đáp án:")]
        public int SoDapAn { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int ThuTuSapXep { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

    }
    #endregion

    public interface BaiToanThoiGianService
    {
        #region Quản lý bài toán thời gian
        List<BaiToanThoiGianModel> DanhSachBaiToanThoiGian(string ThuocKhoiLop);
        BaiToanThoiGianModel BaiToanThoiGianDauTien(string ThuocKhoiLop);
        BaiToanThoiGianModel GetOneBaiToanVeThoiGian(string ThuocKhoiLop);
        BaiToanThoiGianModel DocMotBaiToanThoiGian(string MaCauHoi);
        string ThemMoiMotBaiToanThoiGian(BaiToanThoiGianModel BaiToan);
        string SuaCauHoi(BaiToanThoiGianModel BaiToan);
        string XoaBaiToanThoiGian(string MaCauHoi);
        string XoaNhieuBaiToanThoiGian(string ThuocKhoiLop);
        #endregion

        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }
    }

    public class BaiToanThoiGianClass : BaiToanThoiGianService
    {
        public int SoBanGhiTrenMotTrang
        {
            get
            {
                return 30;
            }
        }
        public int BuocNhay
        {
            get
            {
                return 5;
            }
        }

        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext();

        #region Quản lý bài toán thời gian
        /// <summary>
        /// Đọc danh sách các bài toán về thời gian
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public List<BaiToanThoiGianModel> DanhSachBaiToanThoiGian(string ThuocKhoiLop)
        {
            List<BaiToanThoiGianModel> TatCaDanhSach = (from BaiToan in ListData.BaiToanThoiGians
                                                          where BaiToan.ThuocKhoiLop == ThuocKhoiLop
                                                          orderby BaiToan.ThuTuSapXep descending
                                                          select new BaiToanThoiGianModel
                                                        {
                                                            MaCauHoi = BaiToan.MaCauHoi,
                                                            Gio = BaiToan.Gio,
                                                            Phut = BaiToan.Phut,
                                                            Giay = BaiToan.Giay,
                                                            DapAn = BaiToan.DapAn,
                                                            SoDapAn = BaiToan.SoDapAn,
                                                            ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                            ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                        }).ToList<BaiToanThoiGianModel>();
            return TatCaDanhSach;
        }

        /// <summary>
        /// Đọc bài toán thời gian đầu tiên
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanThoiGianModel BaiToanThoiGianDauTien(string ThuocKhoiLop)
        {
            BaiToanThoiGianModel MotBaiToanThoiGianDauTien = (from BaiToan in ListData.BaiToanThoiGians
                                                              where BaiToan.ThuocKhoiLop == ThuocKhoiLop
                                                              orderby BaiToan.ThuTuSapXep descending
                                                              select new BaiToanThoiGianModel
                                                              {
                                                                  MaCauHoi = BaiToan.MaCauHoi,
                                                                  Gio = BaiToan.Gio,
                                                                  Phut = BaiToan.Phut,
                                                                  Giay = BaiToan.Giay,
                                                                  DapAn = BaiToan.DapAn,
                                                                  SoDapAn = BaiToan.SoDapAn,
                                                                  ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                                  ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                              }).First<BaiToanThoiGianModel>();
            return MotBaiToanThoiGianDauTien;
        }


        /// <summary>
        /// Doc random bai toan ve thoi gian
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanThoiGianModel GetOneBaiToanVeThoiGian(string ThuocKhoiLop)
        {
            IEnumerable<BaiToanThoiGianModel> ResultList = (from BaiToan in ListData.BaiToanThoiGians
                                                            where BaiToan.ThuocKhoiLop == ThuocKhoiLop
                                                            orderby BaiToan.ThuTuSapXep descending
                                                            select new BaiToanThoiGianModel
                                                            {
                                                                MaCauHoi = BaiToan.MaCauHoi,
                                                                Gio = BaiToan.Gio,
                                                                Phut = BaiToan.Phut,
                                                                Giay = BaiToan.Giay,
                                                                DapAn = BaiToan.DapAn,
                                                                DapAnSai = BaiToan.DapAnSai,
                                                                SoDapAn = BaiToan.SoDapAn,
                                                                ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                                ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                            });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Đọc một bài toán thời gian
        /// </summary>
        /// <param name="MaCauHoi"></param>
        /// <returns></returns>
        public BaiToanThoiGianModel DocMotBaiToanThoiGian(string MaCauHoi)
        {
            Guid MaCauHoiDoc = new Guid(MaCauHoi);
            BaiToanThoiGianModel MotCauHoiDoc = (from BaiToan in ListData.BaiToanThoiGians
                                                 where BaiToan.MaCauHoi == MaCauHoiDoc
                                                 select new BaiToanThoiGianModel
                                                   {
                                                       MaCauHoi = BaiToan.MaCauHoi,
                                                       Gio = BaiToan.Gio,
                                                       Phut = BaiToan.Phut,
                                                       Giay = BaiToan.Giay,
                                                       DapAn = BaiToan.DapAn,
                                                       SoDapAn = BaiToan.SoDapAn,
                                                       ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                       ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                   }).SingleOrDefault<BaiToanThoiGianModel>();
            return MotCauHoiDoc;
        }

        /// <summary>
        /// Thêm mới một bài toán thời gian
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotBaiToanThoiGian(BaiToanThoiGianModel BaiToan)
        {
            try
            {
                Table<BaiToanThoiGian> BangBaiToan = ListData.GetTable<BaiToanThoiGian>();
                BaiToanThoiGian BaiToanThoiGianItem = new BaiToanThoiGian();
                BaiToanThoiGianItem.MaCauHoi = BaiToan.MaCauHoi;
                BaiToanThoiGianItem.Gio = BaiToan.Gio;
                BaiToanThoiGianItem.Phut = BaiToan.Phut;
                BaiToanThoiGianItem.Giay = BaiToan.Giay;
                BaiToanThoiGianItem.DapAn = BaiToan.DapAn;
                BaiToanThoiGianItem.DapAnSai = BaiToan.DapAnSai;
                BaiToanThoiGianItem.SoDapAn = BaiToan.SoDapAn;
                BaiToanThoiGianItem.ThuTuSapXep = BaiToan.ThuTuSapXep;
                BaiToanThoiGianItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BangBaiToan.InsertOnSubmit(BaiToanThoiGianItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được bài toán này!";
            }
        }

        /// <summary>
        /// Sửa một dyax số
        /// </summary>
        /// <param name="model"></param>
        public string SuaCauHoi(BaiToanThoiGianModel BaiToan) 
        {
            try
            {
                var BaiToanThoiGianItem = ListData.BaiToanThoiGians.Single(m => m.MaCauHoi == BaiToan.MaCauHoi);
                BaiToanThoiGianItem.Gio = BaiToan.Gio;
                BaiToanThoiGianItem.Phut = BaiToan.Phut;
                BaiToanThoiGianItem.Giay = BaiToan.Giay;
                BaiToanThoiGianItem.DapAn = BaiToan.DapAn;
                BaiToanThoiGianItem.SoDapAn = BaiToan.SoDapAn;
                BaiToanThoiGianItem.ThuTuSapXep = BaiToan.ThuTuSapXep;
                BaiToanThoiGianItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được bài toán thời gian này!";
            }
        }

        /// <summary>
        /// Xóa một bài toán thời gian
        /// </summary>
        /// <param name="id"></param>
        public string XoaBaiToanThoiGian(string MaCauHoi) 
        {
            Guid MaBaiToanXoa = new Guid(MaCauHoi);
            try
            {
                var BaiToanCanXoa = ListData.BaiToanThoiGians.Where(m => m.MaCauHoi == MaBaiToanXoa);
                ListData.BaiToanThoiGians.DeleteAllOnSubmit(BaiToanCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được bài toán thời gian này!";
            }
        }

        /// <summary>
        /// Xóa nhiều bài toán thời gian
        /// </summary>
        /// <param name="id"></param>
        public string XoaNhieuBaiToanThoiGian(string ThuocKhoiLop)
        {
            try
            {
                var CacBaiToanThoiGianCanXoa = ListData.BaiToanThoiGians.Where(m => m.ThuocKhoiLop == ThuocKhoiLop);
                ListData.BaiToanThoiGians.DeleteAllOnSubmit(CacBaiToanThoiGianCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các bài toán thời gian này!";
            }
        }

        
        #endregion

    }
}