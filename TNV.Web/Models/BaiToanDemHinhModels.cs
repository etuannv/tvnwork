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
    public class BaiToanDemHinhModel
    {
        [DisplayName("Mã câu hỏi:")]
        public Guid MaBaiToan { get; set; }

        [DisplayName("Nội dung bài toán:")]
        public string NoiDungBaiToan { get; set; }

        [DisplayName("Lời giải bài toán:")]
        public string LoiGiaiBaiToan { get; set; }

        [DisplayName("Đáp án bài toán:")]
        [Required(ErrorMessage = "Phải nhập đáp án bài toán!")]
        public string DapAnBaiToan { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Phân loại bài toán:")]
        public string PhanLoaiBaiToan { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int SapXepThuTu { get; set; }

    }
   
    public interface BaiToanDemHinhService
    {
        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }

        #region Quản trị bài toán đếm hình
        List<BaiToanDemHinhModel> DanhSachBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan);
        BaiToanDemHinhModel BaiToanDemHinhDauTien(string ThuocKhoiLop, string PhanLoaiBaiToan);
        BaiToanDemHinhModel GetOneBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan);
        BaiToanDemHinhModel DocMotBaiToanDemHinh(string MaBaiToan);
        string ThemMoiBaiToanDemHinh(BaiToanDemHinhModel BaiToan);
        string SuaBaiToanDemHinh(BaiToanDemHinhModel BaiToan);
        string XoaBaiToanDemHinh(string MaBaiToan);
        string XoaNhieuBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan);
        #endregion
    }

    public class BaiToanDemHinhClass : BaiToanDemHinhService
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

        ToanThongMinhDataContext LinqContext = new ToanThongMinhDataContext();
        /// <summary>
        /// Đọc danh sách bài toán đếm hình
        /// </summary>
        /// <param name="ThuocKhoiLop">Thuộc khối lớp</param>
        /// <param name="PhanLoaiBaiToan">Phân loại bài toán đếm hình</param>
        /// <returns></returns>
        public List<BaiToanDemHinhModel> DanhSachBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan)
        {
            List<BaiToanDemHinhModel> DSBaiToanDemHinh=(from BaiToanDemhinhItem in LinqContext.BaiToanDemHinhs
                                                        where BaiToanDemhinhItem.ThuocKhoiLop==ThuocKhoiLop && BaiToanDemhinhItem.PhanLoaiBaiToan==PhanLoaiBaiToan
                                                        orderby BaiToanDemhinhItem.SapXepThuTu ascending
                                                        select new BaiToanDemHinhModel
                                                        {
                                                            MaBaiToan=BaiToanDemhinhItem.MaBaiToan,
                                                            NoiDungBaiToan=BaiToanDemhinhItem.NoiDungBaiToan,
                                                            LoiGiaiBaiToan=BaiToanDemhinhItem.LoiGiaiBaiToan,
                                                            DapAnBaiToan=BaiToanDemhinhItem.DapAnBaiToan,
                                                            ThuocKhoiLop=BaiToanDemhinhItem.ThuocKhoiLop,
                                                            SapXepThuTu=BaiToanDemhinhItem.SapXepThuTu,
                                                            PhanLoaiBaiToan=BaiToanDemhinhItem.PhanLoaiBaiToan
                                                        }).ToList<BaiToanDemHinhModel>();
            return DSBaiToanDemHinh;
        }

        /// <summary>
        /// Bài toán đếm hình đầu tiên
        /// </summary>
        /// <param name="ThuocKhoiLop">Thuộc khối lớp</param>
        /// <param name="PhanLoaiBaiToan">Phân loại bài toán</param>
        /// <returns></returns>
        public BaiToanDemHinhModel BaiToanDemHinhDauTien(string ThuocKhoiLop, string PhanLoaiBaiToan)
        {
            BaiToanDemHinhModel BaiToanDemHinhDauTien=(from BaiToanDemhinhItem in LinqContext.BaiToanDemHinhs
                                                        where BaiToanDemhinhItem.ThuocKhoiLop==ThuocKhoiLop && BaiToanDemhinhItem.PhanLoaiBaiToan==PhanLoaiBaiToan
                                                        orderby BaiToanDemhinhItem.SapXepThuTu ascending
                                                        select new BaiToanDemHinhModel
                                                        {
                                                            MaBaiToan=BaiToanDemhinhItem.MaBaiToan,
                                                            NoiDungBaiToan=BaiToanDemhinhItem.NoiDungBaiToan,
                                                            LoiGiaiBaiToan=BaiToanDemhinhItem.LoiGiaiBaiToan,
                                                            DapAnBaiToan=BaiToanDemhinhItem.DapAnBaiToan,
                                                            ThuocKhoiLop=BaiToanDemhinhItem.ThuocKhoiLop,
                                                            SapXepThuTu=BaiToanDemhinhItem.SapXepThuTu,
                                                            PhanLoaiBaiToan=BaiToanDemhinhItem.PhanLoaiBaiToan
                                                        }).First<BaiToanDemHinhModel>();
            return BaiToanDemHinhDauTien;
        }


        /// <summary>
        /// Bài toán đếm hình bat ky
        /// </summary>
        /// <param name="ThuocKhoiLop">Thuộc khối lớp</param>
        /// <param name="PhanLoaiBaiToan">Phân loại bài toán</param>
        /// <returns></returns>
        public BaiToanDemHinhModel GetOneBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan)
        {
            IEnumerable<BaiToanDemHinhModel> ResultList = (from BaiToanDemhinhItem in LinqContext.BaiToanDemHinhs
                                                         where BaiToanDemhinhItem.ThuocKhoiLop == ThuocKhoiLop && BaiToanDemhinhItem.PhanLoaiBaiToan == PhanLoaiBaiToan
                                                         orderby BaiToanDemhinhItem.SapXepThuTu ascending
                                                         select new BaiToanDemHinhModel
                                                         {
                                                             MaBaiToan = BaiToanDemhinhItem.MaBaiToan,
                                                             NoiDungBaiToan = BaiToanDemhinhItem.NoiDungBaiToan,
                                                             LoiGiaiBaiToan = BaiToanDemhinhItem.LoiGiaiBaiToan,
                                                             DapAnBaiToan = BaiToanDemhinhItem.DapAnBaiToan,
                                                             ThuocKhoiLop = BaiToanDemhinhItem.ThuocKhoiLop,
                                                             SapXepThuTu = BaiToanDemhinhItem.SapXepThuTu,
                                                             PhanLoaiBaiToan = BaiToanDemhinhItem.PhanLoaiBaiToan
                                                         });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Đọc một bài toán đếm hình
        /// </summary>
        /// <param name="MaBaiToan">Mã bài toán cần đọc</param>
        /// <returns></returns>
        public BaiToanDemHinhModel DocMotBaiToanDemHinh(string MaBaiToan) 
        {
            Guid MaBaiToanDoc= new Guid(MaBaiToan);
            BaiToanDemHinhModel BaiToanCanDoc=(from BaiToanDemhinhItem in LinqContext.BaiToanDemHinhs
                                                        where BaiToanDemhinhItem.MaBaiToan==MaBaiToanDoc
                                                        select new BaiToanDemHinhModel
                                                        {
                                                            MaBaiToan=BaiToanDemhinhItem.MaBaiToan,
                                                            NoiDungBaiToan=BaiToanDemhinhItem.NoiDungBaiToan,
                                                            LoiGiaiBaiToan=BaiToanDemhinhItem.LoiGiaiBaiToan,
                                                            DapAnBaiToan=BaiToanDemhinhItem.DapAnBaiToan,
                                                            ThuocKhoiLop=BaiToanDemhinhItem.ThuocKhoiLop,
                                                            SapXepThuTu=BaiToanDemhinhItem.SapXepThuTu,
                                                            PhanLoaiBaiToan=BaiToanDemhinhItem.PhanLoaiBaiToan
                                                        }).Single<BaiToanDemHinhModel>();
            return BaiToanCanDoc;
        }

        /// <summary>
        /// Thêm mới một bài toán đếm hình
        /// </summary>
        /// <param name="BaiToan"></param>
        /// <returns></returns>
        public string ThemMoiBaiToanDemHinh(BaiToanDemHinhModel BaiToan) 
        {
            try
            {
                Table<BaiToanDemHinh> BangBaiToan = LinqContext.GetTable<BaiToanDemHinh>();
                BaiToanDemHinh BaiToanDemHinhItem = new BaiToanDemHinh();
                BaiToanDemHinhItem.MaBaiToan = BaiToan.MaBaiToan;
                BaiToanDemHinhItem.NoiDungBaiToan = BaiToan.NoiDungBaiToan;
                BaiToanDemHinhItem.LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan;
                BaiToanDemHinhItem.DapAnBaiToan = BaiToan.DapAnBaiToan;
                BaiToanDemHinhItem.PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan;
                BaiToanDemHinhItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BangBaiToan.InsertOnSubmit(BaiToanDemHinhItem);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được bài toán này!";
            }
        }

        /// <summary>
        /// Sửa một bài toán đếm hình
        /// </summary>
        /// <param name="BaiToan">Bài toán cần sửa</param>
        /// <returns></returns>
        public string SuaBaiToanDemHinh(BaiToanDemHinhModel BaiToan) 
        {
            try
            {
                var BaiToanDemHinhItem = LinqContext.BaiToanDemHinhs.Single(m => m.MaBaiToan == BaiToan.MaBaiToan);
                BaiToanDemHinhItem.NoiDungBaiToan = BaiToan.NoiDungBaiToan;
                BaiToanDemHinhItem.LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan;
                BaiToanDemHinhItem.DapAnBaiToan = BaiToan.DapAnBaiToan;
                BaiToanDemHinhItem.PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan;
                BaiToanDemHinhItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được bài toán này!";
            }
        }

        /// <summary>
        /// Xóa một bài toán đếm hình
        /// </summary>
        /// <param name="MaBaiToan"></param>
        /// <returns></returns>
        public string XoaBaiToanDemHinh(string MaBaiToan) 
        {
            Guid MaBaiToanXoa = new Guid(MaBaiToan);
            try
            {
                var BaiToanCanXoa = LinqContext.BaiToanDemHinhs.Where(m => m.MaBaiToan == MaBaiToanXoa);
                LinqContext.BaiToanDemHinhs.DeleteAllOnSubmit(BaiToanCanXoa);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được bài toán này!";
            }
        }

        /// <summary>
        /// Xóa nhiều bài toán đếm hình
        /// </summary>
        /// <param name="ThuocKhoiLop">Mã khối lớp cần xóa</param>
        /// <param name="PhanLoaiBaiToan">Phân loại bài toán cần xóa</param>
        /// <returns></returns>
        public string XoaNhieuBaiToanDemHinh(string ThuocKhoiLop, string PhanLoaiBaiToan)
        {
            try
            {
                var CacBaiToanCanXoa = LinqContext.BaiToanDemHinhs.Where(m => m.ThuocKhoiLop == ThuocKhoiLop).Where(m => m.PhanLoaiBaiToan == PhanLoaiBaiToan);
                LinqContext.BaiToanDemHinhs.DeleteAllOnSubmit(CacBaiToanCanXoa);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các bài toán này!";
            }
        }
    }
}