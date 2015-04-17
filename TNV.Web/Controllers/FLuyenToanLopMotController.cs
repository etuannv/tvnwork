using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNV.Web.Models;
using System.Web.Routing;
using System.Web.Security;

namespace TNV.Web.Controllers
{
    public class FLuyenToanLopMotController : Controller
    {
        #region Initialize
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }
        public PhepToanHaiSoHangService ToolPhepToanHaiSoHang { get; set; }
        public PhepToanBaSoHangService ToolPhepToanBaSoHang { get; set; }
        public DoiTuongHonKemNhauService ToolBaiToanDoiTuongHonKemNhau { get; set; }
        public BaiToanDaySoService ToolBaiToanDaySo { get; set; }
        public BaiToanThoiGianService ToolBaiVeThoiGian { get; set; }
        public BaiToanGhepOService ToolBaiToanGhepO { get; set; }
        public BaiToanTimSoService ToolBaiToanTimSo { get; set; }
        public BaiToanDemHinhService ToolBaiToanDemHinh { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new NewsCategoryClass(); }
            if (ToolPhepToanHaiSoHang == null) { ToolPhepToanHaiSoHang = new PhepToanHaiSoHangClass(); }
            if (ToolPhepToanBaSoHang == null) { ToolPhepToanBaSoHang = new PhepToanBaSoHangClass(); }
            if (ToolBaiToanDoiTuongHonKemNhau == null) { ToolBaiToanDoiTuongHonKemNhau = new DoiTuongHonKemNhauClass(); }
            if (ToolBaiToanDaySo == null) { ToolBaiToanDaySo = new BaiToanDaySoClass(); }
            if (ToolBaiVeThoiGian == null) { ToolBaiVeThoiGian = new BaiToanThoiGianClass(); }
            if (ToolBaiToanGhepO == null) { ToolBaiToanGhepO = new BaiToanGhepOClass(); }
            if (ToolBaiToanTimSo == null) { ToolBaiToanTimSo = new BaiToanTimSoClass(); }
            if (ToolBaiToanDemHinh == null) { ToolBaiToanDemHinh = new BaiToanDemHinhClass(); }
            if (ToolSystemManager == null) { ToolSystemManager = new SystemManagerClass(); }


            base.Initialize(requestContext);

            if (Request.IsAuthenticated)
            {
                UserModel ThanhVien = MembershipService.GetOneUserByUserName(User.Identity.Name);
                ViewData["TenThanhVien"] = ThanhVien.FullName;
                ViewData["LoaiThanhVien"] = ThanhVien.RoleDescription;
                ViewData["SoLanDangNhap"] = ThanhVien.LoginNumber;
                if (User.IsInRole("AdminOfSystem"))
                {
                    ViewData["KindMenu"] = "1";
                    //Hiển thị thông tin đăng nhập của quản trị
                    ViewData["Role"] = "1";
                }
                else
                {
                    if (User.IsInRole("SmartUser") || User.IsInRole("SpecialUser"))
                    {
                        //Hiển thị thông tin đăng nhập của người dùng trả tiền
                        ViewData["Role"] = "2";
                        ViewData["NgayTinhPhi"] = AllToolShare.GetDateFormDateTime(ThanhVien.StartDate);
                        ViewData["NgayHetHan"] = AllToolShare.GetDateFormDateTime(ThanhVien.ExpiredDate);
                    }
                    else
                    {
                        //Hiển thị thông tin đăng nhập của người dùng bình thường không trả tiền
                        ViewData["Role"] = "3";
                        ViewData["NgayDangKy"] = AllToolShare.GetDateFormDateTime(ThanhVien.CreateDate);
                    }
                    ViewData["KindMenu"] = "0";
                }
            }
            else
            {
                ViewData["UserName"] = "";
                ViewData["Password"] = "";
                ViewData["KindMenu"] = "0";
            }
        }

        #endregion

        /// <summary>
        /// Hiển thị form chọn dạng toán
        /// </summary>
        /// <returns></returns>
        public ActionResult FDanhSachToanLopMot()
        {
            return View("FDanhSachToanLopMot");
        }

        #region Phep toan 2 so hang
        /// <summary>
        /// Hiển thị bài luyện tập phep toan 2 so hang
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        public ActionResult PhepToanHaiSoHang(string memvar1, string memvar2)
        {
            ViewData["Title"] = "Phép toán 2 số hạng";
            //Gán thuộc khối lớp sang view
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar1) ? "CongPhamVi10" : memvar1;
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar2) ? "CLS1847290691" : memvar2;
            

            //Đọc danh sách các phép toán hai số hạng
            PhepToanHaiSoHangModel PhepToan = ToolPhepToanHaiSoHang.FirstQuesOneOperator(memvar2, memvar1);

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập phep toan 2 so hang
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        public JsonResult GetOnePhepToan2SoHang(string memvar1, string memvar2)
        {
            PhepToanHaiSoHangModel BaiToan = ToolPhepToanHaiSoHang.RandomQuesOneOperator(memvar2, memvar1);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Phep toan ba so hang
        /// <summary>
        /// Hiển thị bài luyện tập phep toan 3 so hang
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        public ActionResult PhepToanBaSoHang(string memvar1, string memvar2)
        {
            ViewData["Title"] = "Phép toán 3 số hạng";
            //Gán thuộc khối lớp sang view
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar1) ? "CongTruPhamVi10" : memvar1;
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar2) ? "CLS1847290691" : memvar2;
            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập phep toan 2 so hang
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        public JsonResult GetOnePhepToan3SoHang(string memvar1, string memvar2)
        {
            PhepToanBaSoHangModel BaiToan = ToolPhepToanBaSoHang.RandomQuesTwoOperator(memvar2, memvar1);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bai Toan Them bot
        /// <summary>
        /// Hiển thị bài luyện tập phep toan 3 so hang
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public ActionResult BaiToanThemBot(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            ViewData["Title"] = "Phép toán 3 số hạng";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar1) ? "CLS1847290691" : memvar1;
            ViewData["SoLuongDoiTuong"] = string.IsNullOrEmpty(memvar2) ? "1": memvar2;
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar3) ? "PhamVi10" : memvar3;
            ViewData["LoaiCauHoi"] = string.IsNullOrEmpty(memvar4) ? "ThemSoLuongDoiTuong" : memvar4;
            
            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan them bot
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanThemBot(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            int SoLuongDoiTuong = int.Parse(memvar2);
            DoiTuongHonKemNhauModel BaiToan = ToolBaiToanDoiTuongHonKemNhau.GetOneBaiToanThemBot(memvar1, SoLuongDoiTuong, memvar3, memvar4);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bai Toan hai doi tuong
        /// <summary>
        /// Hiển thị bài luyện tap bai toan 3 doi tuong
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public ActionResult BaiToanHaiDoiTuong(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            ViewData["Title"] = "Phép toán 3 số hạng";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar1) ? "CLS1847290691" : memvar1;
            ViewData["SoLuongDoiTuong"] = string.IsNullOrEmpty(memvar2) ? "2" : memvar2;
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar3) ? "PhamVi10" : memvar3;
            ViewData["LoaiCauHoi"] = string.IsNullOrEmpty(memvar4) ? "TongHaiDoiTuong" : memvar4;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan them bot
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanHaiDoiTuong(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            int SoLuongDoiTuong = int.Parse(memvar2);
            DoiTuongHonKemNhauModel BaiToan = ToolBaiToanDoiTuongHonKemNhau.GetOneBaiToanThemBot(memvar1, SoLuongDoiTuong, memvar3, memvar4);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bai Toan ba doi tuong
        /// <summary>
        /// Hiển thị bài luyện tap bai toan 3 doi tuong
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public ActionResult BaiToanBaDoiTuong(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            ViewData["Title"] = "Phép toán 3 số hạng";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar1) ? "CLS1847290691" : memvar1;
            ViewData["SoLuongDoiTuong"] = string.IsNullOrEmpty(memvar2) ? "2" : memvar2;
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar3) ? "PhamVi10" : memvar3;
            ViewData["LoaiCauHoi"] = string.IsNullOrEmpty(memvar4) ? "TongHaiDoiTuong" : memvar4;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan ba đối tượng
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanBaDoiTuong(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            int SoLuongDoiTuong = int.Parse(memvar2);
            DoiTuongHonKemNhauModel BaiToan = ToolBaiToanDoiTuongHonKemNhau.GetOneBaiToanThemBot(memvar1, SoLuongDoiTuong, memvar3, memvar4);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Bai Toan dãy số
        /// <summary>
        /// Hiển thị bài luyện tap bai dãy số
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Phân loại dãy số</param>
        /// <returns></returns>
        public ActionResult BaiToanDaySo(string memvar1, string memvar2, string memvar3)
        {
            ViewData["Title"] = "Phép toán 3 số hạng";
            //Gán thuộc khối lớp sang view

            ViewData["ThuocKhoiLop"] = memvar1;
            ViewData["PhamVi"] = memvar2;
            ViewData["PhanLoaiDaySo"] = memvar3;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan ba đối tượng
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Phân loại dãy số</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanDaySo(string memvar1, string memvar2, string memvar3)
        {
            BaiToanDaySoModel BaiToan = ToolBaiToanDaySo.GetOneDaySo(memvar1, memvar2, memvar3);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BaiToanVeThoiGian
        /// <summary>
        /// Hiển thị bài luyện tap bai toan ve thoi gian
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public ActionResult BaiToanVeThoiGian(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            ViewData["Title"] = "Bài toán về thời gian";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = string.IsNullOrEmpty(memvar1) ? "CLS1847290691" : memvar1;
            ViewData["SoLuongDoiTuong"] = string.IsNullOrEmpty(memvar2) ? "2" : memvar2;
            ViewData["PhamVi"] = string.IsNullOrEmpty(memvar3) ? "PhamVi10" : memvar3;
            ViewData["LoaiCauHoi"] = string.IsNullOrEmpty(memvar4) ? "TongHaiDoiTuong" : memvar4;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan them bot
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Số lượng đối tượng</param>
        /// <param name="memvar3">Phạm vi phép toán</param>
        /// <param name="memvar4">Loại câu hỏi</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanVeThoiGian(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            BaiToanThoiGianModel BaiToan = ToolBaiVeThoiGian.GetOneBaiToanVeThoiGian(memvar1);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BaiToanGhepO
        /// <summary>
        /// Hiển thị bài luyện tap bai toan ghep o
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Chiều ngang</param>
        /// <param name="memvar4">Chiều dọc</param>
        /// <param name="memvar5">Loai bai toan</param>
        /// <returns></returns>
        public ActionResult BaiToanGhepO(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            ViewData["Title"] = "Bài toán về thời gian";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = memvar1;
            ViewData["PhamVi"] = memvar2;
            ViewData["ChieuNgang"] = memvar3;
            ViewData["ChieuDoc"] = memvar4;
            ViewData["LoaiBaiToan"] = memvar5;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan them bot
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Chiều ngang</param>
        /// <param name="memvar4">Chiều dọc</param>
        /// <param name="memvar5">Loai bai toan</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanGhepO(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            int ChieuNgang = int.Parse(memvar3);
            int ChieuDoc = int.Parse(memvar4);
            BaiToanGhepOModel BaiToan = ToolBaiToanGhepO.GetOneBaiToanGhepO(memvar1, memvar2, ChieuNgang, ChieuDoc, memvar5);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BaiToan Sap xep
        /// <summary>
        /// Hiển thị bài luyện tap bai toan sap xep
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Chiều ngang</param>
        /// <param name="memvar4">Chiều dọc</param>
        /// <param name="memvar5">Loai bai toan</param>
        /// <returns></returns>
        public ActionResult BaiToanSapXep(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            ViewData["Title"] = "Bài toán sắp xếp";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = memvar1;
            ViewData["PhamVi"] = memvar2;
            ViewData["ChieuNgang"] = memvar3;
            ViewData["ChieuDoc"] = memvar4;
            ViewData["LoaiBaiToan"] = memvar5;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan sap xep
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Chiều ngang</param>
        /// <param name="memvar4">Chiều dọc</param>
        /// <param name="memvar5">Loai bai toan</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanSapXep(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            int ChieuNgang = int.Parse(memvar3);
            int ChieuDoc = int.Parse(memvar4);
            BaiToanGhepOModel BaiToan = ToolBaiToanGhepO.GetOneBaiToanGhepO(memvar1, memvar2, ChieuNgang, ChieuDoc, memvar5);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region BaiToan Tim so
        /// <summary>
        /// Hiển thị bài luyện tap bai toan sap xep
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Loai Bai Toan</param>
        
        /// <returns></returns>
        public ActionResult BaiToanTimSo(string memvar1, string memvar2, string memvar3)
        {
            ViewData["Title"] = "Bài toán sắp xếp";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = memvar1;
            ViewData["PhamVi"] = memvar2;
            ViewData["PhanLoaiBaiToan"] = memvar3;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan sap xep
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Loai Bai Toan</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanTimSo(string memvar1, string memvar2, string memvar3)
        {
            
            BaiToanTimSoModel BaiToan = ToolBaiToanTimSo.GetOneBaiToanTimSo(memvar1, memvar2, memvar3);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region BaiToan Đếm hình
        /// <summary>
        /// Hiển thị bài luyện tap bai toan đếm hình
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar3">Loai Bai Toan</param>

        /// <returns></returns>
        public ActionResult BaiToanDemHinh(string memvar1, string memvar2)
        {
            ViewData["Title"] = "Bài toán đếm hình";
            //Gán thuộc khối lớp sang view
            ViewData["ThuocKhoiLop"] = memvar1;
            ViewData["PhanLoaiBaiToan"] = memvar2;

            return View();
        }

        /// <summary>
        /// Hiển thị 1 bài luyện tập bai toan sap xep
        /// </summary>
        /// <param name="memvar1">Khối lớp</param>
        /// <param name="memvar2">Loai Bai Toan</param>
        /// <returns></returns>
        public JsonResult GetOneBaiToanDemHinh(string memvar1, string memvar2)
        {

            BaiToanDemHinhModel BaiToan = ToolBaiToanDemHinh.GetOneBaiToanDemHinh(memvar1, memvar2);
            return Json(BaiToan, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
