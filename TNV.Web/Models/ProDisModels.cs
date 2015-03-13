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
    public class TinhThanhPhoModel
    {
        [DisplayName("Mã tỉnh, thành phố:")]
        public string MaTinhTP { get; set; }

        [Required(ErrorMessage = "Phải nhập tên tỉnh, thành phố!")]
        [DisplayName("Tên tỉnh, thành phố:")]
        public string TenTinhTP { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int ThuTuSapXep { get; set; }

    }
    public class HuyenThiXaModel
    {
        [DisplayName("Mã huyện, thị xã:")]
        public string MaHuyenThi { get; set; }

        [Required(ErrorMessage = "Phải nhập Tên huyện, thị xã!")]
        [DisplayName("Tên huyện, thị xã:")]
        public string TenHuyenThi { get; set; }

        [DisplayName("Mã tỉnh, thành phố:")]
        public string MaTinhTP { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int ThuTuSapXep { get; set; }
    }

    public class SchoolModel
    {
        [DisplayName("Mã trường học:")]
        public string SchoolId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên trường học!")]
        [DisplayName("Tên trường học:")]
        public string SchoolName { get; set; }

        [DisplayName("Mã huyện. thành phố, thị xã:")]
        public string DistrictId { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int SchoolOrder { get; set; }
    }

    public interface TinhHuyenService
    {
        //Danh mục huyện thị xã
        List<HuyenThiXaModel> DanhSachHuyenThiXaTheoTinh(string MaTinhTP);
        List<HuyenThiXaModel> DanhSachHuyenThiXa();
        HuyenThiXaModel MotHuyenThiXa(string MaHuyenThi);
        bool KienTraTinhHuyen(string MaTinhTP);
        void LuuMoiHuyenThiXa(HuyenThiXaModel model);
        void XoaHuyenThiXa(string id);
        void LuuSuaHuyenThiXa(HuyenThiXaModel model);
        HuyenThiXaModel HuyenDauTienTrongTinh(string MaTinhTP);

        //Danh sách tỉnh thành phố
        void XoaMotTinhThanh(string id);
        void LuuSuaTinhThanhPho(TinhThanhPhoModel model);
        void LuuMoiTinhThanhPho(TinhThanhPhoModel model);
        TinhThanhPhoModel DocMotTinhThanh(string MaTinhTP);
        List<TinhThanhPhoModel> DanhSachTinhThanhPho();
        TinhThanhPhoModel LayMotTinhDauTien();

        //Quản trị danh sách trường học
        void DelSchool(string id);
        void SaveEditSchool(SchoolModel model);
        void SaveNewSchool(SchoolModel model);
        SchoolModel GetOneSchool(string SchoolId);
        List<SchoolModel> AllSchool();
        List<SchoolModel> AllSchool(string DistrictId);
        SchoolModel FirstSchool(string DistrictId);
        bool CheckSchoolInDistrict(string DistrictId);
    }

    public class TinhHuyenClass : TinhHuyenService
    {
        ToanThongMinhDataContext LinqContext = new ToanThongMinhDataContext();

        #region Quản trị danh sách trường học
        /// <summary>
        /// Đọc danh sách các trường theo huyện
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<SchoolModel> AllSchool(string DistrictId)
        {
            List<SchoolModel> SchoolList = (from School in LinqContext.SchoolLists
                                            where School.DistrictId == DistrictId
                                            orderby School.SchoolOrder ascending
                                            select new SchoolModel
                                              {
                                                  SchoolId = School.SchoolId,
                                                  SchoolName = School.SchoolName,
                                                  SchoolOrder = School.SchoolOrder != null ? (int)School.SchoolOrder : 0,
                                                  DistrictId = School.DistrictId
                                              }).ToList<SchoolModel>();
            return SchoolList;
        }
        /// <summary>
        /// Đọc trường học đầu tiên trong huyện
        /// </summary>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public SchoolModel FirstSchool(string DistrictId)
        {
            SchoolModel SchoolFirst = (from School in LinqContext.SchoolLists
                                       where School.DistrictId == DistrictId
                                       orderby School.SchoolOrder ascending
                                       select new SchoolModel
                                       {
                                           SchoolId = School.SchoolId,
                                           SchoolName = School.SchoolName,
                                           SchoolOrder = School.SchoolOrder != null ? (int)School.SchoolOrder : 0,
                                           DistrictId = School.DistrictId
                                       }).First<SchoolModel>();
            return SchoolFirst;
        }
        /// <summary>
        /// Kiểm tra xem một huyện đã có trường nào hay chưa
        /// </summary>
        /// <param name="DistrictId"></param>
        /// <returns></returns>
        public bool CheckSchoolInDistrict(string DistrictId)
        {
            List<SchoolModel> DanhSach = (from School in LinqContext.SchoolLists
                                          where School.DistrictId.Trim() == DistrictId.Trim()
                                          orderby School.SchoolOrder ascending
                                          select new SchoolModel
                                        {
                                            SchoolId = School.SchoolId,
                                            SchoolName = School.SchoolName,
                                            SchoolOrder = School.SchoolOrder != null ? (int)School.SchoolOrder : 0,
                                            DistrictId = School.DistrictId
                                        }).ToList<SchoolModel>();

            if (DanhSach.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Đọc danh sách tất cả các trường
        /// </summary>
        /// <returns></returns>
        public List<SchoolModel> AllSchool() 
        {
            List<SchoolModel> SchoolList = (from School in LinqContext.SchoolLists
                                            orderby School.SchoolOrder ascending
                                            select new SchoolModel
                                            {
                                                SchoolId = School.SchoolId,
                                                SchoolName = School.SchoolName,
                                                SchoolOrder = School.SchoolOrder != null ? (int)School.SchoolOrder : 0,
                                                DistrictId = School.DistrictId
                                            }).ToList<SchoolModel>();
            return SchoolList;
        }
       

        /// <summary>
        /// Lấy thông tin một trường bởi mã của nó
        /// </summary>
        /// <param name="MaHuyenThi"></param>
        /// <returns></returns>
        public SchoolModel GetOneSchool(string SchoolId) 
        {
            SchoolModel SchoolOne = (from OneSchool in LinqContext.SchoolLists
                                     where OneSchool.SchoolId.Trim() == SchoolId.Trim()
                                     orderby OneSchool.SchoolOrder ascending
                                     select new SchoolModel
                                         {
                                             SchoolId = OneSchool.SchoolId,
                                             SchoolName = OneSchool.SchoolName,
                                             SchoolOrder = OneSchool.SchoolOrder != null ? (int)OneSchool.SchoolOrder : 0,
                                             DistrictId = OneSchool.DistrictId
                                         }).Single<SchoolModel>();
            return SchoolOne;
        }
        /// <summary>
        /// Thêm mới trường học
        /// </summary>
        /// <param name="model"></param>
        public void SaveNewSchool(SchoolModel model) 
        {
            Table<SchoolList> NewSchool = LinqContext.GetTable<SchoolList>();
            SchoolList OneSchool = new SchoolList();
            OneSchool.SchoolId = model.SchoolId;
            OneSchool.SchoolName = model.SchoolName;
            OneSchool.SchoolOrder = model.SchoolOrder;
            OneSchool.DistrictId = model.DistrictId;
            NewSchool.InsertOnSubmit(OneSchool);
            LinqContext.SubmitChanges();
        }
        /// <summary>
        /// Sửa thông tin trường học
        /// </summary>
        /// <param name="model"></param>
        public void SaveEditSchool(SchoolModel model) 
        {
            var OneSchool = LinqContext.SchoolLists.Single(p => p.SchoolId == model.SchoolId);
            OneSchool.SchoolName = model.SchoolName;
            OneSchool.DistrictId = model.DistrictId;
            OneSchool.SchoolOrder = model.SchoolOrder;
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Xóa thông tin một trường học
        /// </summary>
        /// <param name="id"></param>
        public void DelSchool(string id) 
        {
            var OneSchool = from School in LinqContext.SchoolLists
                            where School.SchoolId == id
                            select School;
            LinqContext.SchoolLists.DeleteAllOnSubmit(OneSchool);
            LinqContext.SubmitChanges();
        }
        #endregion

        #region Quản trị danh sách huyện
        /// <summary>
        /// Đọc danh sách các huyện, thị xã theo tỉnh
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<HuyenThiXaModel> DanhSachHuyenThiXaTheoTinh(string MaTinhTP) 
        {
            List<HuyenThiXaModel> DanhSach = (from HuyenThi in LinqContext.HuyenThanhThis
                                              where HuyenThi.MaTinhTP == MaTinhTP.Trim()
                                              orderby HuyenThi.ThuTuSapXep ascending
                                              select new HuyenThiXaModel
                                               {
                                                   MaHuyenThi = HuyenThi.MaHuyenThi,
                                                   TenHuyenThi = HuyenThi.TenHuyenThi,
                                                   ThuTuSapXep = HuyenThi.ThuTuSapXep != null ? (int)HuyenThi.ThuTuSapXep : 0,
                                                   MaTinhTP = HuyenThi.MaTinhTP
                                               }).ToList<HuyenThiXaModel>();
            return DanhSach;
        }
        /// <summary>
        /// Lấy huyện đầu tiên trong một tỉnh
        /// </summary>
        /// <param name="MaTinhTP"></param>
        /// <returns></returns>
        public HuyenThiXaModel HuyenDauTienTrongTinh(string MaTinhTP)
        {
            HuyenThiXaModel MotHuyenThi = (from HuyenThi in LinqContext.HuyenThanhThis
                                           where HuyenThi.MaTinhTP == MaTinhTP.Trim()
                                           orderby HuyenThi.ThuTuSapXep ascending
                                           select new HuyenThiXaModel
                                           {
                                               MaHuyenThi = HuyenThi.MaHuyenThi,
                                               TenHuyenThi = HuyenThi.TenHuyenThi,
                                               ThuTuSapXep = HuyenThi.ThuTuSapXep!=null? (int)HuyenThi.ThuTuSapXep:0,
                                               MaTinhTP = HuyenThi.MaTinhTP
                                           }).First<HuyenThiXaModel>();
            return MotHuyenThi;
        }
        /// <summary>
        /// Đọc danh sách các huyện, thị xã
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<HuyenThiXaModel> DanhSachHuyenThiXa() 
        {
            List<HuyenThiXaModel> DanhSach = (from HuyenThi in LinqContext.HuyenThanhThis
                                              orderby HuyenThi.ThuTuSapXep ascending
                                              select new HuyenThiXaModel
                                              {
                                                  MaHuyenThi = HuyenThi.MaHuyenThi,
                                                  TenHuyenThi = HuyenThi.TenHuyenThi,
                                                  ThuTuSapXep = HuyenThi.ThuTuSapXep != null ? (int)HuyenThi.ThuTuSapXep : 0,
                                                  MaTinhTP = HuyenThi.MaTinhTP
                                              }).ToList<HuyenThiXaModel>();
            return DanhSach;
        }

        /// <summary>
        /// Lấy thông tin một huyện bởi mã của nó
        /// </summary>
        /// <param name="MaHuyenThi"></param>
        /// <returns></returns>
        public HuyenThiXaModel MotHuyenThiXa(string MaHuyenThi) 
        {
            HuyenThiXaModel HuyenThiXa = (from HuyenThi in LinqContext.HuyenThanhThis
                                          where HuyenThi.MaHuyenThi == MaHuyenThi
                                          orderby HuyenThi.ThuTuSapXep ascending
                                              select new HuyenThiXaModel
                                              {
                                                  MaHuyenThi = HuyenThi.MaHuyenThi,
                                                  TenHuyenThi = HuyenThi.TenHuyenThi,
                                                  ThuTuSapXep = HuyenThi.ThuTuSapXep != null ? (int)HuyenThi.ThuTuSapXep : 0,
                                                  MaTinhTP = HuyenThi.MaTinhTP
                                              }).Single<HuyenThiXaModel>();
            return HuyenThiXa;
        }
        
        /// <summary>
        /// Kiểm tra tỉnh đã có huyện hay chưa?
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public bool KienTraTinhHuyen(string MaTinhTP) 
        {
            List<HuyenThiXaModel> DanhSach = (from HuyenThi in LinqContext.HuyenThanhThis
                                              where HuyenThi.MaTinhTP == MaTinhTP.Trim()
                                              orderby HuyenThi.ThuTuSapXep ascending
                                              select new HuyenThiXaModel
                                               {
                                                   MaHuyenThi = HuyenThi.MaHuyenThi,
                                                   TenHuyenThi = HuyenThi.TenHuyenThi,
                                                   ThuTuSapXep = HuyenThi.ThuTuSapXep != null ? (int)HuyenThi.ThuTuSapXep : 0,
                                                   MaTinhTP = HuyenThi.MaTinhTP
                                               }).ToList<HuyenThiXaModel>();

            if (DanhSach.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Thêm mới huyện
        /// </summary>
        /// <param name="model"></param>
        public void LuuMoiHuyenThiXa(HuyenThiXaModel model) 
        {
            Table<HuyenThanhThi> BangHuyenThi = LinqContext.GetTable<HuyenThanhThi>();
            HuyenThanhThi BanGhiHuyenThi = new HuyenThanhThi();
            BanGhiHuyenThi.MaHuyenThi = model.MaHuyenThi;
            BanGhiHuyenThi.TenHuyenThi = model.TenHuyenThi;
            BanGhiHuyenThi.MaTinhTP = model.MaTinhTP;
            BanGhiHuyenThi.ThuTuSapXep = model.ThuTuSapXep;
            BangHuyenThi.InsertOnSubmit(BanGhiHuyenThi);
            LinqContext.SubmitChanges();
        }
        /// <summary>
        /// Sửa thông tin huyên
        /// </summary>
        /// <param name="model"></param>
        public void LuuSuaHuyenThiXa(HuyenThiXaModel model) 
        {
            var BanGhiHuyenThi = LinqContext.HuyenThanhThis.Single(p => p.MaHuyenThi == model.MaHuyenThi);
            BanGhiHuyenThi.TenHuyenThi = model.TenHuyenThi;
            BanGhiHuyenThi.MaTinhTP = model.MaTinhTP;
            BanGhiHuyenThi.ThuTuSapXep = model.ThuTuSapXep;
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Xóa thông tin một huyện
        /// </summary>
        /// <param name="id"></param>
        public void XoaHuyenThiXa(string id) 
        {
            var BanGhiHuyenThi = from HuyenThiXa in LinqContext.HuyenThanhThis
                                 where HuyenThiXa.MaHuyenThi == id
                                 select HuyenThiXa;
            LinqContext.HuyenThanhThis.DeleteAllOnSubmit(BanGhiHuyenThi);
            LinqContext.SubmitChanges();
        }
        #endregion

        #region Quản trị danh sách tỉnh thành
        /// <summary>
        /// Lấy tất cả các tỉnh thành phố
        /// </summary>
        /// <returns></returns>
        public List<TinhThanhPhoModel> DanhSachTinhThanhPho()
        {
            List<TinhThanhPhoModel> TinhThanhPho = (from TinhThanh in LinqContext.TinhThanhPhos
                                                    orderby TinhThanh.ThuTuSapXep ascending
                                                    select new TinhThanhPhoModel
                                                    {
                                                        MaTinhTP = TinhThanh.MaTinhTP,
                                                        TenTinhTP = TinhThanh.TenTinhTP,
                                                        ThuTuSapXep = TinhThanh.ThuTuSapXep != null ? (int)TinhThanh.ThuTuSapXep : 0,
                                                    }).ToList<TinhThanhPhoModel>();
            return TinhThanhPho;
        }
        /// <summary>
        /// Lấy một tỉnh thành đầu tiên
        /// </summary>
        /// <returns></returns>
        public TinhThanhPhoModel LayMotTinhDauTien()
        {
            TinhThanhPhoModel MotTinhThanh = (from TinhThanh in LinqContext.TinhThanhPhos
                                              orderby TinhThanh.ThuTuSapXep ascending
                                              select new TinhThanhPhoModel
                                              {
                                                  MaTinhTP = TinhThanh.MaTinhTP,
                                                  TenTinhTP = TinhThanh.TenTinhTP,
                                                  ThuTuSapXep = TinhThanh.ThuTuSapXep != null ? (int)TinhThanh.ThuTuSapXep : 0,
                                              }).First<TinhThanhPhoModel>();
            return MotTinhThanh;
        }
        /// <summary>
        /// Lấy thông tin của một tỉnh thành phố
        /// </summary>
        /// <returns></returns>
        public TinhThanhPhoModel DocMotTinhThanh(string MaTinhTP) 
        {
            TinhThanhPhoModel TinhThanhPho = (from TinhThanh in LinqContext.TinhThanhPhos
                                              where TinhThanh.MaTinhTP == MaTinhTP
                                              select new TinhThanhPhoModel
                                              {
                                                  MaTinhTP = TinhThanh.MaTinhTP,
                                                  TenTinhTP = TinhThanh.TenTinhTP,
                                                  ThuTuSapXep = TinhThanh.ThuTuSapXep != null ? (int)TinhThanh.ThuTuSapXep : 0,
                                              }).Single<TinhThanhPhoModel>();
            return TinhThanhPho;
        }
        /// <summary>
        /// Thêm mới một tỉnh thành
        /// </summary>
        /// <param name="model"></param>
        public void LuuMoiTinhThanhPho(TinhThanhPhoModel model) 
        {
            Table<TinhThanhPho> DanhSachTinhThanhPho = LinqContext.GetTable<TinhThanhPho>();
            TinhThanhPho TinhThanh = new TinhThanhPho();
            TinhThanh.MaTinhTP = model.MaTinhTP;
            TinhThanh.TenTinhTP = model.TenTinhTP;
            TinhThanh.ThuTuSapXep = model.ThuTuSapXep;
            DanhSachTinhThanhPho.InsertOnSubmit(TinhThanh);
            LinqContext.SubmitChanges();
        }
        /// <summary>
        /// Sửa thông tin của một tỉnh thành
        /// </summary>
        /// <param name="model"></param>
        public void LuuSuaTinhThanhPho(TinhThanhPhoModel model) 
        {
            var TinhThanh = LinqContext.TinhThanhPhos.Single(p => p.MaTinhTP == model.MaTinhTP);
            TinhThanh.TenTinhTP = model.TenTinhTP;
            TinhThanh.ThuTuSapXep = model.ThuTuSapXep;
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Xóa một chuyên mục tin
        /// </summary>
        /// <param name="id"></param>
        public void XoaMotTinhThanh(string id) 
        {
            var MotTinhThanh = from TinhThanh in LinqContext.TinhThanhPhos
                               where TinhThanh.MaTinhTP == id
                               select TinhThanh;
            LinqContext.TinhThanhPhos.DeleteAllOnSubmit(MotTinhThanh);
            LinqContext.SubmitChanges();
        }
        #endregion
    }
}