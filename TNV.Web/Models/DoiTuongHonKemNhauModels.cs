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
    public class DoiTuongHonKemNhauModel
    {
        [DisplayName("Mã câu hỏi:")]
        public Guid MaCauHoi { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }

        [DisplayName("Loại câu hỏi:")]
        public string LoaiCauHoi { get; set; }

        [DisplayName("Nội dung câu hỏi:")]
        [Required(ErrorMessage = "Phải nhập nội dung câu hỏi!")]
        public string NoiDungCauHoi { get; set; }

        [DisplayName("Thành phần câu hỏi:")]
        public string ThanhPhanCauHoi { get; set; }

        [DisplayName("Số lượng đáp án của câu hỏi:")]
        [Required(ErrorMessage = "Số lượng đáp án của câu hỏi!")]
        public int SoLuongDapAn { get; set; }

        [DisplayName("Đáp án của câu hỏi:")]
        [Required(ErrorMessage = "Đáp án của câu hỏi!")]
        public string DapAnCauHoi { get; set; }

        [DisplayName("Lời giải của câu hỏi:")]
        [Required(ErrorMessage = "Bạn phải nhập lời giải câu hỏi!")]
        public string LoiGiaiCauHoi { get; set; }

        [DisplayName("Kết luận của câu hỏi:")]
        [Required(ErrorMessage = "Bạn phải nhập kết luận câu hỏi!")]
        public string KetLuanCauHoi { get; set; }

        [DisplayName("Số lượng đối tượng:")]
        [Required(ErrorMessage = "Bạn phải nhập số lượng đối tượng!")]
        public int SoLuongDoiTuong { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int SapXepThuTu { get; set; }

    }
    public class HoTenModel
    {
        [DisplayName("Mã họ tên:")]
        public int MaHoTen { get; set; }

        [DisplayName("Tên:")]
        public string Ten { get; set; }
    }
    public class DoiTuongModel
    {
        [DisplayName("Mã đối tượng:")]
        public int MaDoiTuong { get; set; }

        [DisplayName("Tên đối tượng:")]
        public string TenDoiTuong { get; set; }

        [DisplayName("Đơn vị tính:")]
        public string DonViTinh { get; set; }

        [DisplayName("Hình ảnh đại diện:")]
        public string HinhAnh { get; set; }

        [DisplayName("Thao tác đối tượng:")]
        public string ThaoTacDoiTuong { get; set; }

        [DisplayName("Tiền tố chủ ngữ:")]
        public string TienToChuNgu { get; set; }

        [DisplayName("Thuộc về tự nhiên:")]
        public bool ThuocVeTuNhien { get; set; }

        [DisplayName("Sở hữu:")]
        public string SoHuu { get; set; }
    }
    public interface DoiTuongHonKemNhauService
    {
        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }

        #region Quản trị danh sách các câu hỏi đối tượng hơn kém nhau
        List<DoiTuongHonKemNhauModel> DanhSachCauHoi(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi);
        DoiTuongHonKemNhauModel DocCauHoiDauTien(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi);
        DoiTuongHonKemNhauModel GetOneBaiToanThemBot(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi);
        DoiTuongHonKemNhauModel DocMotCauHoi(string MaCauHoi);
        string ThemMoiMotCauHoi(DoiTuongHonKemNhauModel model);
        string SuaCauHoi(DoiTuongHonKemNhauModel model);
        string XoaCauHoi(string MaCauHoi);
        string XoaCauHoiMotLop(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi);
        string XoaCacTen(); 
        string ThemMoiMotTen(HoTenModel model); 
        List<HoTenModel> DanhSachTen();
        HoTenModel DocMotTen(int MaHoTen);
        DoiTuongModel MotDoiTuong(int MaDoiTuong);
        List<DoiTuongModel> DanhSachDoiTuong();
        #endregion
    }

    public class DoiTuongHonKemNhauClass : DoiTuongHonKemNhauService
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

        #region Quản trị danh sách các câu hỏi tính toán chứa hai phép toán 
        /// <summary>
        /// Đọc danh sách các câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <param name="PhamViPhepToan"></param>
        /// <returns></returns>
        public List<DoiTuongHonKemNhauModel> DanhSachCauHoi(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi)
        {
            List<DoiTuongHonKemNhauModel> TatCaDanhSachCauHoi = (from DanhSachCauHoi in LinqContext.DoiTuongHonKemNhaus
                                                                 where DanhSachCauHoi.ThuocKhoiLop == ThuocKhoiLop && DanhSachCauHoi.SoLuongDoiTuong == SoLuongDoiTuong && DanhSachCauHoi.PhamViPhepToan == PhamViPhepToan && DanhSachCauHoi.LoaiCauHoi == LoaiCauHoi
                                                                 orderby DanhSachCauHoi.SapXepThuTu descending
                                                                 select new DoiTuongHonKemNhauModel
                                                                {
                                                                    MaCauHoi = DanhSachCauHoi.MaCauHoi,
                                                                    NoiDungCauHoi = DanhSachCauHoi.NoiDungCauHoi,
                                                                    DapAnCauHoi = DanhSachCauHoi.DapAnCauHoi,
                                                                    LoiGiaiCauHoi = DanhSachCauHoi.LoiGiaiCauHoi,
                                                                    SoLuongDapAn=DanhSachCauHoi.SoLuongDapAn!=null?(int)DanhSachCauHoi.SoLuongDapAn:0,
                                                                    SoLuongDoiTuong = DanhSachCauHoi.SoLuongDoiTuong != null ? (int)DanhSachCauHoi.SoLuongDoiTuong : 0,
                                                                    KetLuanCauHoi=DanhSachCauHoi.KetLuanCauHoi,
                                                                    SapXepThuTu = DanhSachCauHoi.SapXepThuTu != null ? (int)DanhSachCauHoi.SapXepThuTu : 0,
                                                                    ThuocKhoiLop = DanhSachCauHoi.ThuocKhoiLop,
                                                                    ThanhPhanCauHoi = DanhSachCauHoi.ThanhPhanCauHoi
                                                                }).ToList<DoiTuongHonKemNhauModel>();
            return TatCaDanhSachCauHoi;
        }


        
        /// <summary>
        /// Đọc câu hỏi đối tượng hơn kém
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public DoiTuongHonKemNhauModel GetOneBaiToanThemBot(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi)
        {
            IEnumerable<DoiTuongHonKemNhauModel> ResultList = (from CauHoiDauTien in LinqContext.DoiTuongHonKemNhaus
                                                        where CauHoiDauTien.ThuocKhoiLop == ThuocKhoiLop && CauHoiDauTien.SoLuongDoiTuong == SoLuongDoiTuong && CauHoiDauTien.PhamViPhepToan == PhamViPhepToan && CauHoiDauTien.LoaiCauHoi == LoaiCauHoi
                                                         orderby CauHoiDauTien.SapXepThuTu ascending
                                                         select new DoiTuongHonKemNhauModel
                                                         {
                                                             MaCauHoi = CauHoiDauTien.MaCauHoi,
                                                             NoiDungCauHoi = CauHoiDauTien.NoiDungCauHoi,
                                                             DapAnCauHoi = CauHoiDauTien.DapAnCauHoi,
                                                             LoiGiaiCauHoi = CauHoiDauTien.LoiGiaiCauHoi,
                                                             SoLuongDapAn = CauHoiDauTien.SoLuongDapAn != null ? (int)CauHoiDauTien.SoLuongDapAn : 0,
                                                             SoLuongDoiTuong = CauHoiDauTien.SoLuongDoiTuong != null ? (int)CauHoiDauTien.SoLuongDoiTuong : 0,
                                                             KetLuanCauHoi = CauHoiDauTien.KetLuanCauHoi,
                                                             SapXepThuTu = CauHoiDauTien.SapXepThuTu != null ? (int)CauHoiDauTien.SapXepThuTu : 0,
                                                             ThuocKhoiLop = CauHoiDauTien.ThuocKhoiLop,
                                                             ThanhPhanCauHoi = CauHoiDauTien.ThanhPhanCauHoi
                                                         });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Đọc câu hỏi đối tượng hơn kém
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public DoiTuongHonKemNhauModel DocCauHoiDauTien(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi)
        {
            DoiTuongHonKemNhauModel MotCauHoiDauTien = (from CauHoiDauTien in LinqContext.DoiTuongHonKemNhaus
                                                        where CauHoiDauTien.ThuocKhoiLop == ThuocKhoiLop && CauHoiDauTien.SoLuongDoiTuong == SoLuongDoiTuong && CauHoiDauTien.PhamViPhepToan == PhamViPhepToan && CauHoiDauTien.LoaiCauHoi == LoaiCauHoi
                                                         orderby CauHoiDauTien.SapXepThuTu ascending
                                                         select new DoiTuongHonKemNhauModel
                                                         {
                                                             MaCauHoi = CauHoiDauTien.MaCauHoi,
                                                             NoiDungCauHoi = CauHoiDauTien.NoiDungCauHoi,
                                                             DapAnCauHoi = CauHoiDauTien.DapAnCauHoi,
                                                             LoiGiaiCauHoi = CauHoiDauTien.LoiGiaiCauHoi,
                                                             SoLuongDapAn = CauHoiDauTien.SoLuongDapAn != null ? (int)CauHoiDauTien.SoLuongDapAn : 0,
                                                             SoLuongDoiTuong = CauHoiDauTien.SoLuongDoiTuong != null ? (int)CauHoiDauTien.SoLuongDoiTuong : 0,
                                                             KetLuanCauHoi = CauHoiDauTien.KetLuanCauHoi,
                                                             SapXepThuTu = CauHoiDauTien.SapXepThuTu != null ? (int)CauHoiDauTien.SapXepThuTu : 0,
                                                             ThuocKhoiLop = CauHoiDauTien.ThuocKhoiLop,
                                                             ThanhPhanCauHoi = CauHoiDauTien.ThanhPhanCauHoi
                                                         }).FirstOrDefault<DoiTuongHonKemNhauModel>();
            return MotCauHoiDauTien;
        }

        /// <summary>
        /// Đọc một câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// <param name="MaCauHoi"></param>
        /// <returns></returns>
        public DoiTuongHonKemNhauModel DocMotCauHoi(string MaCauHoi)
        {
            Guid MaCauHoiDoc = new Guid(MaCauHoi);
            DoiTuongHonKemNhauModel MotCauHoi = (from CauHoi in LinqContext.DoiTuongHonKemNhaus
                                                    where CauHoi.MaCauHoi == MaCauHoiDoc
                                                    select new DoiTuongHonKemNhauModel
                                                    {
                                                        MaCauHoi = CauHoi.MaCauHoi,
                                                        NoiDungCauHoi = CauHoi.NoiDungCauHoi,
                                                        DapAnCauHoi = CauHoi.DapAnCauHoi,
                                                        LoiGiaiCauHoi = CauHoi.LoiGiaiCauHoi,
                                                        SoLuongDapAn = CauHoi.SoLuongDapAn != null ? (int)CauHoi.SoLuongDapAn : 0,
                                                        SoLuongDoiTuong = CauHoi.SoLuongDoiTuong != null ? (int)CauHoi.SoLuongDoiTuong : 0,
                                                        KetLuanCauHoi = CauHoi.KetLuanCauHoi,
                                                        SapXepThuTu = CauHoi.SapXepThuTu != null ? (int)CauHoi.SapXepThuTu : 0,
                                                        ThuocKhoiLop = CauHoi.ThuocKhoiLop,
                                                        ThanhPhanCauHoi = CauHoi.ThanhPhanCauHoi
                                                    }).SingleOrDefault<DoiTuongHonKemNhauModel>();
            return MotCauHoi;
        }
       
        /// <summary>
        /// Thêm mới một câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotCauHoi(DoiTuongHonKemNhauModel model)
        {
            try
            {
                Table<DoiTuongHonKemNhau> BangCauHoi = LinqContext.GetTable<DoiTuongHonKemNhau>();
                DoiTuongHonKemNhau CauHoiMoi = new DoiTuongHonKemNhau();
                CauHoiMoi.MaCauHoi = model.MaCauHoi;
                CauHoiMoi.NoiDungCauHoi = model.NoiDungCauHoi;
                CauHoiMoi.DapAnCauHoi = model.DapAnCauHoi;
                CauHoiMoi.LoiGiaiCauHoi = model.LoiGiaiCauHoi;
                CauHoiMoi.KetLuanCauHoi = model.KetLuanCauHoi;
                CauHoiMoi.SoLuongDapAn = model.SoLuongDapAn;
                CauHoiMoi.SoLuongDoiTuong = model.SoLuongDoiTuong;
                CauHoiMoi.SapXepThuTu = model.SapXepThuTu;
                CauHoiMoi.ThuocKhoiLop = model.ThuocKhoiLop;
                CauHoiMoi.PhamViPhepToan = model.PhamViPhepToan;
                CauHoiMoi.LoaiCauHoi = model.LoaiCauHoi;
                CauHoiMoi.ThanhPhanCauHoi = model.ThanhPhanCauHoi;
                BangCauHoi.InsertOnSubmit(CauHoiMoi);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được câu hỏi này!";
            }
        }

        /// <summary>
        /// Sửa một câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// <param name="model"></param>
        public string SuaCauHoi(DoiTuongHonKemNhauModel model)
        {
            try
            {
                var CauHoiSua = LinqContext.DoiTuongHonKemNhaus.Single(m => m.MaCauHoi == model.MaCauHoi);
                CauHoiSua.MaCauHoi = model.MaCauHoi;
                CauHoiSua.NoiDungCauHoi = model.NoiDungCauHoi;
                CauHoiSua.DapAnCauHoi = model.DapAnCauHoi;
                CauHoiSua.LoiGiaiCauHoi = model.LoiGiaiCauHoi;
                CauHoiSua.KetLuanCauHoi = model.KetLuanCauHoi;
                CauHoiSua.SoLuongDoiTuong = model.SoLuongDoiTuong;
                CauHoiSua.SoLuongDapAn = model.SoLuongDapAn;
                CauHoiSua.SapXepThuTu = model.SapXepThuTu;
                CauHoiSua.ThuocKhoiLop = model.ThuocKhoiLop;
                CauHoiSua.PhamViPhepToan = model.PhamViPhepToan;
                CauHoiSua.LoaiCauHoi = model.LoaiCauHoi;
                CauHoiSua.ThanhPhanCauHoi = model.ThanhPhanCauHoi;
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được câu hỏi này!";
            }
        }

        /// <summary>
        /// Xóa một câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// <param name="id"></param>
        public string XoaCauHoi(string MaCauHoi)
        {
            Guid MaCauHoiXoa = new Guid(MaCauHoi);
            try
            {
                var CauHoiCanXoa = LinqContext.DoiTuongHonKemNhaus.Where(m => m.MaCauHoi == MaCauHoiXoa);
                LinqContext.DoiTuongHonKemNhaus.DeleteAllOnSubmit(CauHoiCanXoa);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được câu hỏi này!";
            }
        }

        /// <summary>
        /// Xóa tất cả các câu hỏi một lớp
        /// </summary>
        /// <param name="id"></param>
        public string XoaCauHoiMotLop(string ThuocKhoiLop, int SoLuongDoiTuong, string PhamViPhepToan, string LoaiCauHoi)
        {
            try
            {
                var CacCauHoi = LinqContext.DoiTuongHonKemNhaus.Where(m => m.ThuocKhoiLop == ThuocKhoiLop).Where(m => m.SoLuongDoiTuong == SoLuongDoiTuong).Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m=>m.LoaiCauHoi==LoaiCauHoi);
                LinqContext.DoiTuongHonKemNhaus.DeleteAllOnSubmit(CacCauHoi);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các câu hỏi này!";
            }
        }
        #endregion

        #region Quản lý danh sách tên
        /// <summary>
        /// Đọc danh sách tất cả các tên
        /// </summary>
        /// <returns></returns>
        public List<HoTenModel> DanhSachTen()
        {
            List<HoTenModel> TatCaDanhSachTen = (from DanhSachTen in LinqContext.DanhSachTens
                                                 select new HoTenModel
                                                {
                                                    MaHoTen=DanhSachTen.MaHoTen,
                                                    Ten=DanhSachTen.Ten
                                                }).ToList<HoTenModel>();
            return TatCaDanhSachTen;
        }
        /// <summary>
        /// Lấy một tên bởi mã của nó
        /// </summary>
        /// <param name="MaHoTen"></param>
        /// <returns></returns>
        public HoTenModel DocMotTen(int MaHoTen)
        {
            HoTenModel MotTen = (from LayMotTen in LinqContext.DanhSachTens
                                 where LayMotTen.MaHoTen == MaHoTen
                                select new HoTenModel
                                {
                                    MaHoTen = LayMotTen.MaHoTen,
                                    Ten = LayMotTen.Ten
                                }).Single<HoTenModel>();
            return MotTen;
        }
        /// <summary>
        /// Thêm mới một tên
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotTen(HoTenModel model)
        {
            try
            {
                Table<DanhSachTen> BangTen = LinqContext.GetTable<DanhSachTen>();
                DanhSachTen MotTen = new DanhSachTen();
                MotTen.Ten = model.Ten;
                BangTen.InsertOnSubmit(MotTen);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới tên này!";
            }
        }

        /// <summary>
        /// Xóa tất cả các câu hỏi một lớp
        /// </summary>
        /// <param name="id"></param>
        public string XoaCacTen()
        {
            try
            {
                var CacTen = LinqContext.DanhSachTens.Where(m => m.MaHoTen > 0);
                LinqContext.DanhSachTens.DeleteAllOnSubmit(CacTen);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các tên này!";
            }
        }
        #endregion

        #region Quản lý danh sách đối tượng
        /// <summary>
        /// Đọc danh sách tất cả các đối tượng
        /// </summary>
        /// <returns></returns>
        public List<DoiTuongModel> DanhSachDoiTuong()
        {
            List<DoiTuongModel> TatCaDanhSachDoiTuong = (from DoiTuongItem in LinqContext.DanhSachDoiTuongs
                                                         select new DoiTuongModel
                                                         {
                                                             MaDoiTuong = DoiTuongItem.MaDoiTuong,
                                                             TenDoiTuong = DoiTuongItem.TenDoiTuong,
                                                             DonViTinh=DoiTuongItem.DonViTinh,
                                                             HinhAnh=DoiTuongItem.HinhAnh,
                                                             SoHuu=DoiTuongItem.SoHuu,
                                                             ThaoTacDoiTuong=DoiTuongItem.ThaoTacDoiTuong,
                                                             ThuocVeTuNhien=DoiTuongItem.ThuocVeTuNhien!=null?(bool)DoiTuongItem.ThuocVeTuNhien:false,
                                                             TienToChuNgu=DoiTuongItem.TienToChuNgu,
                                                         }).ToList<DoiTuongModel>();
            return TatCaDanhSachDoiTuong;
        }

        /// <summary>
        /// Đọc một đối tượng bởi mã của nó
        /// </summary>
        /// <returns></returns>
        public DoiTuongModel MotDoiTuong(int MaDoiTuong) 
        {
            DoiTuongModel DocDoiTuong = (from DoiTuongItem in LinqContext.DanhSachDoiTuongs
                                         where DoiTuongItem.MaDoiTuong == MaDoiTuong
                                        select new DoiTuongModel
                                        {
                                            MaDoiTuong = DoiTuongItem.MaDoiTuong,
                                            TenDoiTuong = DoiTuongItem.TenDoiTuong,
                                            DonViTinh = DoiTuongItem.DonViTinh,
                                            HinhAnh = DoiTuongItem.HinhAnh,
                                            SoHuu = DoiTuongItem.SoHuu,
                                            ThaoTacDoiTuong = DoiTuongItem.ThaoTacDoiTuong,
                                            ThuocVeTuNhien = DoiTuongItem.ThuocVeTuNhien != null ? (bool)DoiTuongItem.ThuocVeTuNhien : false,
                                            TienToChuNgu = DoiTuongItem.TienToChuNgu,
                                        }).Single<DoiTuongModel>();
            return DocDoiTuong;
        }

        
        #endregion

    }
}