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
    [HandleError]
    public class DoiTuongHonKemNhauController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }
        public PhepToanHaiSoHangService ToolPhepToanHaiSoHang { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public PhepToanBaSoHangService ToolPhepToanBaSoHang { get; set; }
        public DoiTuongHonKemNhauService ToolDoiTuongHonKemNhau { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new NewsCategoryClass(); }
            if (ToolPhepToanHaiSoHang == null) { ToolPhepToanHaiSoHang = new PhepToanHaiSoHangClass(); }
            if (ToolSystemManager == null) { ToolSystemManager = new SystemManagerClass(); }
            if (ToolPhepToanBaSoHang == null) { ToolPhepToanBaSoHang = new PhepToanBaSoHangClass(); }
            if (ToolDoiTuongHonKemNhau == null) { ToolDoiTuongHonKemNhau = new DoiTuongHonKemNhauClass(); }

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

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Hiển thị danh sách câu hỏi 
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult DanhSachCauHoi(string memvar1, string memvar2, string memvar3, string memvar4)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["SoLuongDoiTuong"] = memvar2;
                ViewData["PhamViPhepToan"] = memvar3;
                ViewData["LoaiCauHoi"] = memvar4;
                //Đọc danh sách các phép toán hai số hạng
                int SoLuongDoiTuong = Convert.ToInt32(memvar2);
                List<DoiTuongHonKemNhauModel> DanhSachCauHoi = ToolDoiTuongHonKemNhau.DanhSachCauHoi(memvar1, SoLuongDoiTuong, memvar3, memvar4);

                //Khởi tạo trang
                int Demo = ToolPhepToanBaSoHang.SoBanGhiTrenMotTrang;
                int step = ToolPhepToanBaSoHang.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachCauHoi.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachCauHoi.Count;
                    StartNumOfRecordInPage = DanhSachCauHoi.Count; ;
                }
                string NumRecPerPages = Request.Form["NumRecPerPages"];
                if (!String.IsNullOrEmpty(NumRecPerPages))
                {
                    try
                    {
                        if (NumRecPerPages.Trim() != "0")
                        {
                            NumOfRecordInPage = Convert.ToInt32(NumRecPerPages);
                        }
                        else
                        {
                            NumOfRecordInPage = Demo;
                        }
                    }
                    catch
                    {
                        NumOfRecordInPage = Demo;
                    }
                }

                //Tạo lựa chọn số bản ghi trên một trang
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachCauHoi.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachCauHoi.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "DoiTuongHonKemNhau";
                OnPage.Action = "DanhSachCauHoi";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = memvar3;
                OnPage.memvar5 = memvar4;
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachCauHoi.Count, NumOfRecordInPage);

                string PageCurent = Request.Form["PageCurent"];
                if (String.IsNullOrEmpty(PageCurent))
                {
                    OnPage.CurentPage = 1;
                }
                else
                {
                    OnPage.CurentPage = Convert.ToInt32(PageCurent);
                }

                ViewData["Page"] = OnPage;
                ViewData["PageCurent"] = OnPage.CurentPage;
                ViewData["StartOrder"] = (OnPage.CurentPage - 1) * NumOfRecordInPage;
                if (OnPage.NumberPages > 1)
                {
                    ViewData["LoadNumberPage"] = "1";
                }
                else
                {
                    ViewData["LoadNumberPage"] = "0";
                }
                return View("DanhSachCauHoi", DanhSachCauHoi.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }


        /// <summary>
        /// Khởi tạo tự động các phép toán
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult TaoTuDongCacCauHoi()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<DoiTuongHonKemNhauModel> ListItem = new List<DoiTuongHonKemNhauModel>();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string SoLuongDoiTuong = Request.Form["SoLuongDoiTuong"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string LoaiCauHoi = Request.Form["LoaiCauHoi"];

                #region Bài toán thêm, bớt

                #region Tạo các bài toán thêm số lượng đối tượng phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "ThemSoLuongDoiTuong")
                {
                    for (int SoBanDau = 1; SoBanDau <= 10; SoBanDau++)
                    {
                        for (int SoDenThem = 1; SoDenThem <= 10; SoDenThem++)
                        {
                            if (SoBanDau + SoDenThem <= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " + " + SoDenThem.ToString().Trim() + " = " + (SoBanDau + SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau + SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau + SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[0] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán bớt số lượng đối tượng phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BotSoLuongDoiTuong")
                {
                    for (int SoBanDau = 1; SoBanDau <= 10; SoBanDau++)
                    {
                        for (int SoBotDi = 1; SoBotDi <= 10; SoBotDi++)
                        {
                            if (SoBanDau - SoBotDi > 0)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " - " + SoBotDi.ToString().Trim() + " = " + (SoBanDau - SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau - SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau - SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[1] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangMot")
                {
                    for (int SoDoiTuongSauKhiThem = 1; SoDoiTuongSauKhiThem <= 10; SoDoiTuongSauKhiThem++)
                    {
                        for (int SoDenThem = 1; SoDenThem <= 10; SoDenThem++)
                        {
                            if (SoDoiTuongSauKhiThem - SoDenThem > 0)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiThem.ToString() + " - " + SoDenThem.ToString().Trim() + " = " + (SoDoiTuongSauKhiThem - SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangHai")
                {
                    for (int SoDoiTuongSauKhiBot = 1; SoDoiTuongSauKhiBot <= 10; SoDoiTuongSauKhiBot++)
                    {
                        for (int SoBotDi = 1; SoBotDi <= 10; SoBotDi++)
                        {
                            if (SoDoiTuongSauKhiBot + SoBotDi <= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiBot.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiBot.ToString() + " + " + SoBotDi.ToString().Trim() + " = " + (SoDoiTuongSauKhiBot + SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion




                #region Tạo các bài toán thêm số lượng đối tượng phạm vi 20 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "ThemSoLuongDoiTuong")
                {
                    for (int SoBanDau = 3; SoBanDau <= 20; SoBanDau++)
                    {
                        for (int SoDenThem = 5; SoDenThem <= 20; SoDenThem++)
                        {
                            if (SoBanDau + SoDenThem <= 20 && SoBanDau + SoDenThem > 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " + " + SoDenThem.ToString().Trim() + " = " + (SoBanDau + SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau + SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau + SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[0] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán bớt số lượng đối tượng phạm vi 20 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BotSoLuongDoiTuong")
                {
                    for (int SoBanDau = 3; SoBanDau <= 20; SoBanDau++)
                    {
                        for (int SoBotDi = 1; SoBotDi <= 20; SoBotDi++)
                        {
                            if (SoBanDau - SoBotDi > 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " - " + SoBotDi.ToString().Trim() + " = " + (SoBanDau - SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau - SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau - SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[1] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 20 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangMot")
                {
                    for (int SoDoiTuongSauKhiThem = 10; SoDoiTuongSauKhiThem <= 20; SoDoiTuongSauKhiThem++)
                    {
                        for (int SoDenThem = 5; SoDenThem <= 20; SoDenThem++)
                        {
                            if (SoDoiTuongSauKhiThem - SoDenThem > 5)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiThem.ToString() + " - " + SoDenThem.ToString().Trim() + " = " + (SoDoiTuongSauKhiThem - SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 20 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangHai")
                {
                    for (int SoDoiTuongSauKhiBot = 7; SoDoiTuongSauKhiBot <= 20; SoDoiTuongSauKhiBot++)
                    {
                        for (int SoBotDi = 4; SoBotDi <= 20; SoBotDi++)
                        {
                            if (SoDoiTuongSauKhiBot + SoBotDi <= 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiBot.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiBot.ToString() + " + " + SoBotDi.ToString().Trim() + " = " + (SoDoiTuongSauKhiBot + SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion



                #region Tạo các bài toán thêm số lượng đối tượng phạm vi 30 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "ThemSoLuongDoiTuong")
                {
                    for (int SoBanDau = 3; SoBanDau <= 30; SoBanDau++)
                    {
                        for (int SoDenThem = 5; SoDenThem <= 30; SoDenThem++)
                        {
                            if (SoBanDau + SoDenThem <= 30 && SoBanDau + SoDenThem > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " + " + SoDenThem.ToString().Trim() + " = " + (SoBanDau + SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau + SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau + SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[0] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán bớt số lượng đối tượng phạm vi 30 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BotSoLuongDoiTuong")
                {
                    for (int SoBanDau = 3; SoBanDau <= 30; SoBanDau++)
                    {
                        for (int SoBotDi = 1; SoBotDi <= 30; SoBotDi++)
                        {
                            if (SoBanDau - SoBotDi > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " - " + SoBotDi.ToString().Trim() + " = " + (SoBanDau - SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau - SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau - SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[1] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 30 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangMot")
                {
                    for (int SoDoiTuongSauKhiThem = 20; SoDoiTuongSauKhiThem <= 30; SoDoiTuongSauKhiThem++)
                    {
                        for (int SoDenThem = 10; SoDenThem <= 30; SoDenThem++)
                        {
                            if (SoDoiTuongSauKhiThem - SoDenThem > 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiThem.ToString() + " - " + SoDenThem.ToString().Trim() + " = " + (SoDoiTuongSauKhiThem - SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 30 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangHai")
                {
                    for (int SoDoiTuongSauKhiBot = 12; SoDoiTuongSauKhiBot <= 30; SoDoiTuongSauKhiBot++)
                    {
                        for (int SoBotDi = 7; SoBotDi <= 30; SoBotDi++)
                        {
                            if (SoDoiTuongSauKhiBot + SoBotDi <= 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiBot.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiBot.ToString() + " + " + SoBotDi.ToString().Trim() + " = " + (SoDoiTuongSauKhiBot + SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion


                #region Tạo các bài toán thêm số lượng đối tượng phạm vi 30 đến 100 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "ThemSoLuongDoiTuong")
                {
                    for (int SoBanDau = 30; SoBanDau <= 100; SoBanDau++)
                    {
                        for (int SoDenThem = 10; SoDenThem <= 100; SoDenThem++)
                        {
                            if (SoBanDau + SoDenThem <= 100 && SoBanDau + SoDenThem >30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " + " + SoDenThem.ToString().Trim() + " = " + (SoBanDau + SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau + SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau + SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[0] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán bớt số lượng đối tượng phạm vi 30 đến 100 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BotSoLuongDoiTuong")
                {
                    for (int SoBanDau = 35; SoBanDau <= 100; SoBanDau++)
                    {
                        for (int SoBotDi = 10; SoBotDi < SoBanDau; SoBotDi++)
                        {
                            if (SoBanDau - SoBotDi > 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " " + SoBanDau.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh
                                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoBanDau.ToString() + " - " + SoBotDi.ToString().Trim() + " = " + (SoBanDau - SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoBanDau - SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoBanDau - SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = ThaoTacDoiTuong[1] + " " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 30 đến 100 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangMot")
                {
                    for (int SoDoiTuongSauKhiThem = 30; SoDoiTuongSauKhiThem < 100; SoDoiTuongSauKhiThem++)
                    {
                        for (int SoDenThem = 20; SoDenThem < 100; SoDenThem++)
                        {
                            if (SoDoiTuongSauKhiThem - SoDenThem > 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[0] + " " + SoDenThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiThem.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiThem.ToString() + " - " + SoDenThem.ToString().Trim() + " = " + (SoDoiTuongSauKhiThem - SoDenThem).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiThem - SoDenThem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số lượng đối tượng ban đầu, biết số lượng đối tượng đã thêm và tổng số sau khi thêm phạm vi 30 đến 100 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "1" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TimSoDoiTuongBanDauDangHai")
                {
                    for (int SoDoiTuongSauKhiBot = 30; SoDoiTuongSauKhiBot < 100; SoDoiTuongSauKhiBot++)
                    {
                        for (int SoBotDi = 20; SoBotDi < 100; SoBotDi++)
                        {
                            if (SoDoiTuongSauKhiBot + SoBotDi < 100)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == false)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                string[] ThaoTacDoiTuong = MotDoiTuong.ThaoTacDoiTuong.Split('$');

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". " + ThaoTacDoiTuong[1] + " " + SoBotDi.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " " + SoDoiTuongSauKhiBot.ToString().Trim() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoDoiTuongSauKhiBot.ToString() + " + " + SoBotDi.ToString().Trim() + " = " + (SoDoiTuongSauKhiBot + SoBotDi).ToString().Trim() + "</b>";
                                OneItem.DapAnCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString();
                                OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiBot + SoBotDi).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region Bài toán tính tuổi

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 10 khối lớp 1 (Dạng 1)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangMot")
                {
                    for (int TongSoTuoi = 4; TongSoTuoi <= 10; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 2; SoNamSauDo <= 9; SoNamSauDo++)
                        {
                            if (TongSoTuoi + 2 * SoNamSauDo <= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hiện tại tổng số tuổi của hai bạn " + TenNguoiMot.Ten.Trim() + " và  " + TenNguoiHai.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi"+
                                    ". Hỏi sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai bạn là bao nhiêu?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi bạn đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là: <b>" + TongSoTuoi.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi + 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 20 khối lớp 1 (Dạng 1)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangMot")
                {
                    for (int TongSoTuoi = 6; TongSoTuoi <= 20; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 1; SoNamSauDo <= 20; SoNamSauDo++)
                        {
                            if (TongSoTuoi + 2 * SoNamSauDo <= 20 && TongSoTuoi + 2 * SoNamSauDo >10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hiện tại tổng số tuổi của hai bạn " + TenNguoiMot.Ten.Trim() + " và  " + TenNguoiHai.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi" +
                                    ". Hỏi sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai bạn là bao nhiêu?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi bạn đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là: <b>" + TongSoTuoi.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi + 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 30 khối lớp 1 (Dạng 1)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangMot")
                {
                    for (int TongSoTuoi = 15; TongSoTuoi <= 30; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 3; SoNamSauDo <= 30; SoNamSauDo++)
                        {
                            if (TongSoTuoi + 2 * SoNamSauDo <= 30 && TongSoTuoi + 2 * SoNamSauDo > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hiện tại tổng số tuổi của hai bạn " + TenNguoiMot.Ten.Trim() + " và  " + TenNguoiHai.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi" +
                                    ". Hỏi sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai bạn là bao nhiêu?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi bạn đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là: <b>" + TongSoTuoi.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi + 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả hai bạn là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 30 đến 100 khối lớp 1 (Dạng 1)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangMot")
                {
                    for (int TongSoTuoi = 30; TongSoTuoi <= 75; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 10; SoNamSauDo <= 20; SoNamSauDo++)
                        {
                            if (TongSoTuoi + 2 * SoNamSauDo <= 90 && TongSoTuoi + 2 * SoNamSauDo > 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                string DaiTu = "";
                                if (TongSoTuoi <= 35)
                                {
                                    DaiTu = "hai anh em ";
                                }
                                else if (TongSoTuoi <= 60)
                                {
                                    DaiTu = "hai bố con ";
                                }
                                else if (TongSoTuoi <= 75)
                                {
                                    DaiTu = "hai ông cháu ";
                                }

                                    
                                OneItem.NoiDungCauHoi = "Hiện tại tổng số tuổi của "+DaiTu + TenNguoiMot.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi" +
                                    ". Hỏi sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của " + DaiTu + TenNguoiMot.Ten.Trim() + " là bao nhiêu?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi người đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả "+ DaiTu + TenNguoiMot.Ten.Trim() +" là: <b>" + TongSoTuoi.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " + " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi + 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi + 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Sau " + SoNamSauDo.ToString().Trim() + " thì tổng số tuổi của cả " + DaiTu + TenNguoiMot.Ten.Trim() + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 10 khối lớp 1 (Dạng 2)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangHai")
                {
                    for (int TongSoTuoi = 7; TongSoTuoi <= 10; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 2; SoNamSauDo <= 9; SoNamSauDo++)
                        {
                            if (TongSoTuoi - 2 * SoNamSauDo >= 4)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Tính tổng số tuổi hiện tại của hai anh em " + TenNguoiMot.Ten.Trim() +
                                    ". Biết rằng sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi người đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() +" là: <b>" + TongSoTuoi.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi - 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 20 khối lớp 1 (Dạng 2)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangHai")
                {
                    for (int TongSoTuoi = 10; TongSoTuoi <= 20; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 2; SoNamSauDo <= 20; SoNamSauDo++)
                        {
                            if (TongSoTuoi - 2 * SoNamSauDo >= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Tính tổng số tuổi hiện tại của hai anh em " + TenNguoiMot.Ten.Trim() +
                                    ". Biết rằng sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi người đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là: <b>" + TongSoTuoi.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi - 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 30 khối lớp 1 (Dạng 2)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangHai")
                {
                    for (int TongSoTuoi = 20; TongSoTuoi <= 30; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 2; SoNamSauDo <= 30; SoNamSauDo++)
                        {
                            if (TongSoTuoi - 2 * SoNamSauDo >= 15)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Tính tổng số tuổi hiện tại của hai anh em " + TenNguoiMot.Ten.Trim() +
                                    ". Biết rằng sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi người đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là: <b>" + TongSoTuoi.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi - 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Hiện tại tổng số tuổi của hai anh em " + TenNguoiMot.Ten.Trim() + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tính tuổi hai đối tượng phạm vi 30 khối lớp 1 (Dạng 2)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaiToanTinhTuoiDangHai")
                {
                    for (int TongSoTuoi = 30; TongSoTuoi <= 95; TongSoTuoi++)
                    {
                        for (int SoNamSauDo = 7; SoNamSauDo <= 30; SoNamSauDo++)
                        {
                            if (TongSoTuoi - 2 * SoNamSauDo >= 30 && TongSoTuoi - 2 * SoNamSauDo <= 75)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                string DaiTu = "";
                                if (TongSoTuoi - 2 * SoNamSauDo <= 35)
                                {
                                    DaiTu = "hai anh em ";
                                }
                                else if (TongSoTuoi - 2 * SoNamSauDo <= 60)
                                {
                                    DaiTu = "hai bố con ";
                                }
                                else if (TongSoTuoi - 2 * SoNamSauDo <= 75)
                                {
                                    DaiTu = "hai ông cháu ";
                                }

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Tính tổng số tuổi hiện tại của " + DaiTu + TenNguoiMot.Ten.Trim() +
                                    ". Biết rằng sau " + SoNamSauDo.ToString().Trim() + " năm thì tổng số tuổi của " + DaiTu + TenNguoiMot.Ten.Trim() + " là " + TongSoTuoi.ToString().Trim() + " tuổi";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = " Sau <b>" + SoNamSauDo.ToString().Trim() + "</b> năm thì mỗi người đều thêm <b>" + SoNamSauDo.ToString().Trim() + "</b> (tuổi).<br/>"
                                    + " Vậy hiện tại tổng số tuổi của " + DaiTu + TenNguoiMot.Ten.Trim() + " là: <b>" + TongSoTuoi.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " - " + SoNamSauDo.ToString().Trim() + " = " + (TongSoTuoi - 2 * SoNamSauDo).ToString().Trim() + "</b> (tuổi).";
                                OneItem.DapAnCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString();
                                OneItem.KetLuanCauHoi = (TongSoTuoi - 2 * SoNamSauDo).ToString() + "(tuổi)";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Hiện tại tổng số tuổi của" + DaiTu + TenNguoiMot.Ten.Trim() + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #endregion

                #region Hai đối tượng

                #region Tạo các bài toán hai đối tượng hơn kém nhau phạm vi 10 khối lớp 1 (CLS1847290691)

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongHaiDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 9; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 9; SoHangThuHai++)
                        {
                            if (SoHangThuNhat + SoHangThuHai <= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi  cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                    + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 10 khối lớp 1 dạng 2
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangHai")
                {
                    for (int SoHangThuNhatThem = 1; SoHangThuNhatThem <= 9; SoHangThuNhatThem++)
                    {
                        for (int SoHangThuHaiThem = 1; SoHangThuHaiThem <= 9; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuNhatThem + SoHangThuHaiThem + 1; TongHaiDoiTuongSauKhiThem <= 9; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem -SoHangThuNhatThem - SoHangThuHaiThem <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuNhatThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu +" thêm là: <b>" + SoHangThuNhatThem.ToString().Trim() + " + " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 10 khối lớp 1 dạng 3
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBa")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 9; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = SoHangThuNhatGiam + 1; SoHangThuHaiThem <= 9; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuHaiThem - SoHangThuNhatGiam + 1; TongHaiDoiTuongSauKhiThem <= 9; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " tăng thêm là: <b>" + SoHangThuHaiThem.ToString().Trim() + " - " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 10 khối lớp 1 dạng 4
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBon")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 9; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiGiam = 1; SoHangThuHaiGiam <= 9; SoHangThuHaiGiam++)
                        {
                            for (int TongHaiDoiTuongSauKhiGiam = 1; TongHaiDoiTuongSauKhiGiam <= 9; TongHaiDoiTuongSauKhiGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " giảm bớt " + SoHangThuHaiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm bớt là: <b>" + SoHangThuHaiGiam.ToString().Trim() + " + " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiGiam.ToString().Trim() + " + " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 10 khối lớp 1 dạng 5
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangNam")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 9; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = 1; SoHangThuHaiThem <= 9; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThemGiam = 1; TongHaiDoiTuongSauKhiThemGiam <= 9; TongHaiDoiTuongSauKhiThemGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem <= 10 && SoHangThuHaiThem < SoHangThuNhatGiam)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThemGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm so với ban đầu là: <b>" + SoHangThuNhatGiam.ToString().Trim() + " - " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThemGiam.ToString().Trim() + " + " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion
                
                #region Đối tượng 2 nhiều hơn đối tượng một phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanHon = 1; PhanHon <= 10 - SoHangThuNhat; PhanHon++)
                        {
                            if (SoHangThuNhat + PhanHon <= 10 && 2*SoHangThuNhat + PhanHon<=10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHon.ToString()+ " = "+(SoHangThuNhat + PhanHon).ToString() + "</b>"+" (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; 
                                OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHon).ToString() + "$" + (2*SoHangThuNhat + PhanHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat + PhanHon).ToString() + " và " + (2 * SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 ít hơn đối tượng một phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanItHon = 1; PhanItHon < SoHangThuNhat; PhanItHon++)
                        {
                            if (2 * SoHangThuNhat - PhanItHon <= 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanItHon.ToString() + " = " + (SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * SoHangThuNhat - PhanItHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat - PhanItHon).ToString() + " và " + (2 * SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangMot")
                {
                    for (int TongHaiDoiTuong = 1; TongHaiDoiTuong  <=10; TongHaiDoiTuong++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat <TongHaiDoiTuong; SoHangThuNhat++)
                        {
                            //Khởi tạo câu hỏi
                            DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                            //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                            int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                            {
                                MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            }
                            DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                            //Lấy hai tên người quan hệ hơn kém
                            int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                            int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                            HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                            HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                            //Tạo nội dung câu hỏi
                            OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                            OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                            OneItem.SoLuongDapAn = 1;
                            OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                            OneItem.ThuocKhoiLop = ThuocKhoiLop;
                            OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiDoiTuong.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiDoiTuong - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.DapAnCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString();
                            OneItem.KetLuanCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString() +"(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.PhamViPhepToan = PhamViPhepToan;
                            OneItem.LoaiCauHoi = LoaiCauHoi;
                            OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                            ListItem.Add(OneItem);
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangHai")
                {
                    for (int TongHaiDoiTuong = 1; TongHaiDoiTuong <= 10; TongHaiDoiTuong++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiDoiTuong; PhanNhieuHon++)
                        {
                            if ((TongHaiDoiTuong - PhanNhieuHon) % 2 == 0)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>"+ ((TongHaiDoiTuong - PhanNhieuHon)/2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")<br/>"
                                    + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "$" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString();
                                OneItem.KetLuanCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + " và " + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$"+MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #endregion

                #region Tạo các bài toán hai đối tượng hơn kém nhau phạm vi 20 khối lớp 1 (CLS1847290691)

                #region Tạo các bài toán tổng hai đối tượng phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongHaiDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 20; SoHangThuHai++)
                        {
                            if (SoHangThuNhat + SoHangThuHai <= 20 && SoHangThuNhat + SoHangThuHai > 10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi  cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                    + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 20 khối lớp 1 dạng 2
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangHai")
                {
                    for (int SoHangThuNhatThem = 1; SoHangThuNhatThem <= 20; SoHangThuNhatThem++)
                    {
                        for (int SoHangThuHaiThem = 1; SoHangThuHaiThem <= 20; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuNhatThem + SoHangThuHaiThem + 1; TongHaiDoiTuongSauKhiThem <= 20; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem <= 20 && TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem >10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuNhatThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm là: <b>" + SoHangThuNhatThem.ToString().Trim() + " + " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 20 khối lớp 1 dạng 3
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBa")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 20; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = SoHangThuNhatGiam + 1; SoHangThuHaiThem <= 20; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuHaiThem - SoHangThuNhatGiam + 1; TongHaiDoiTuongSauKhiThem <= 20; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem <= 20 && TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem >= 10 && SoHangThuHaiThem- SoHangThuNhatGiam >=5)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " tăng thêm là: <b>" + SoHangThuHaiThem.ToString().Trim() + " - " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 20 khối lớp 1 dạng 4
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBon")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 20; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiGiam = 1; SoHangThuHaiGiam <= 20; SoHangThuHaiGiam++)
                        {
                            for (int TongHaiDoiTuongSauKhiGiam = 1; TongHaiDoiTuongSauKhiGiam <= 20; TongHaiDoiTuongSauKhiGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam <= 20 && SoHangThuNhatGiam + SoHangThuHaiGiam > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " giảm bớt " + SoHangThuHaiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm bớt là: <b>" + SoHangThuHaiGiam.ToString().Trim() + " + " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiGiam.ToString().Trim() + " + " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 20 khối lớp 1 dạng 5
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangNam")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 20; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = 1; SoHangThuHaiThem <= 20; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThemGiam = 1; TongHaiDoiTuongSauKhiThemGiam <=20; TongHaiDoiTuongSauKhiThemGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem <= 20 && SoHangThuHaiThem < SoHangThuNhatGiam && TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThemGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm so với ban đầu là: <b>" + SoHangThuNhatGiam.ToString().Trim() + " - " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThemGiam.ToString().Trim() + " + " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 nhiều hơn đối tượng một phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanHon = 1; PhanHon <= 20 - SoHangThuNhat; PhanHon++)
                        {
                            if (SoHangThuNhat + PhanHon <= 20 && 2 * SoHangThuNhat + PhanHon <= 20 && SoHangThuNhat + PhanHon >10 && 2 * SoHangThuNhat + PhanHon >10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHon).ToString() + "$" + (2 * SoHangThuNhat + PhanHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat + PhanHon).ToString() + " và " + (2 * SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 ít hơn đối tượng một phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanItHon = 1; PhanItHon < SoHangThuNhat; PhanItHon++)
                        {
                            if (2 * SoHangThuNhat - PhanItHon <= 20 && 2 * SoHangThuNhat - PhanItHon >10)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanItHon.ToString() + " = " + (SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * SoHangThuNhat - PhanItHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat - PhanItHon).ToString() + " và " + (2 * SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangMot")
                {
                    for (int TongHaiDoiTuong = 10; TongHaiDoiTuong <= 20; TongHaiDoiTuong++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiDoiTuong; SoHangThuNhat++)
                        {
                            //Khởi tạo câu hỏi
                            DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                            //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                            int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                            {
                                MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            }
                            DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                            //Lấy hai tên người quan hệ hơn kém
                            int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                            int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                            HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                            HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                            //Tạo nội dung câu hỏi
                            OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                            OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                            OneItem.SoLuongDapAn = 1;
                            OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                            OneItem.ThuocKhoiLop = ThuocKhoiLop;
                            OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiDoiTuong.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiDoiTuong - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.DapAnCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString();
                            OneItem.KetLuanCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.PhamViPhepToan = PhamViPhepToan;
                            OneItem.LoaiCauHoi = LoaiCauHoi;
                            OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                            ListItem.Add(OneItem);
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangHai")
                {
                    for (int TongHaiDoiTuong = 11; TongHaiDoiTuong <= 20; TongHaiDoiTuong++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiDoiTuong; PhanNhieuHon++)
                        {
                            if ((TongHaiDoiTuong - PhanNhieuHon) % 2 == 0 && (TongHaiDoiTuong - PhanNhieuHon) / 2>4)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")<br/>"
                                    + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "$" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString();
                                OneItem.KetLuanCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + " và " + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #endregion

                #region Tạo các bài toán hai đối tượng hơn kém nhau phạm vi 30 khối lớp 1 (CLS1847290691)

                #region Tạo các bài toán tổng hai đối tượng phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongHaiDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 30; SoHangThuHai++)
                        {
                            if (SoHangThuNhat + SoHangThuHai <= 30 && SoHangThuNhat + SoHangThuHai > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi  cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 khối lớp 1 dạng 2
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangHai")
                {
                    for (int SoHangThuNhatThem = 1; SoHangThuNhatThem <= 30; SoHangThuNhatThem++)
                    {
                        for (int SoHangThuHaiThem = 1; SoHangThuHaiThem <= 30; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuNhatThem + SoHangThuHaiThem + 1; TongHaiDoiTuongSauKhiThem <= 30; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem <= 30 && TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuNhatThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm là: <b>" + SoHangThuNhatThem.ToString().Trim() + " + " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 khối lớp 1 dạng 3
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBa")
                {
                    for (int SoHangThuNhatGiam = 1; SoHangThuNhatGiam <= 30; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = SoHangThuNhatGiam + 1; SoHangThuHaiThem <= 30; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuHaiThem - SoHangThuNhatGiam + 1; TongHaiDoiTuongSauKhiThem <= 30; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem <= 30 && TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem >= 20 && SoHangThuHaiThem - SoHangThuNhatGiam >= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " tăng thêm là: <b>" + SoHangThuHaiThem.ToString().Trim() + " - " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 khối lớp 1 dạng 4
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBon")
                {
                    for (int SoHangThuNhatGiam = 5; SoHangThuNhatGiam <= 30; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiGiam = 5; SoHangThuHaiGiam <= 30; SoHangThuHaiGiam++)
                        {
                            for (int TongHaiDoiTuongSauKhiGiam = 15; TongHaiDoiTuongSauKhiGiam <= 30; TongHaiDoiTuongSauKhiGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam <= 30 && TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam >20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " giảm bớt " + SoHangThuHaiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm bớt là: <b>" + SoHangThuHaiGiam.ToString().Trim() + " + " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiGiam.ToString().Trim() + " + " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 khối lớp 1 dạng 5
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangNam")
                {
                    for (int SoHangThuNhatGiam = 5; SoHangThuNhatGiam <= 30; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = 5; SoHangThuHaiThem <= 30; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThemGiam = 10; TongHaiDoiTuongSauKhiThemGiam <= 30; TongHaiDoiTuongSauKhiThemGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiThemGiam - SoHangThuHaiThem > 5 && TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem <= 30 && SoHangThuNhatGiam - SoHangThuHaiThem > 10 && TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThemGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm so với ban đầu là: <b>" + SoHangThuNhatGiam.ToString().Trim() + " - " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThemGiam.ToString().Trim() + " + " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 nhiều hơn đối tượng một phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanHon = 1; PhanHon <= 30 - SoHangThuNhat; PhanHon++)
                        {
                            if (SoHangThuNhat + PhanHon <= 30 && 2 * SoHangThuNhat + PhanHon <= 30 && SoHangThuNhat + PhanHon > 20 && 2 * SoHangThuNhat + PhanHon > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". " 
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHon).ToString() + "$" + (2 * SoHangThuNhat + PhanHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat + PhanHon).ToString() + " và " + (2 * SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 ít hơn đối tượng một phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanItHon = 1; PhanItHon < SoHangThuNhat; PhanItHon++)
                        {
                            if (2 * SoHangThuNhat - PhanItHon <= 30 && 2 * SoHangThuNhat - PhanItHon > 20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". " 
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong 
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanItHon.ToString() + " = " + (SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai " 
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * SoHangThuNhat - PhanItHon).ToString();
                                OneItem.KetLuanCauHoi =(SoHangThuNhat - PhanItHon).ToString() + " và " + (2 * SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangMot")
                {
                    for (int TongHaiDoiTuong = 20; TongHaiDoiTuong <= 30; TongHaiDoiTuong++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiDoiTuong; SoHangThuNhat++)
                        {
                            //Khởi tạo câu hỏi
                            DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                            //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                            int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                            {
                                MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                            }
                            DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                            //Lấy hai tên người quan hệ hơn kém
                            int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                            int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                            HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                            HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                            //Tạo nội dung câu hỏi
                            OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                            OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                            OneItem.SoLuongDapAn = 1;
                            OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                            OneItem.ThuocKhoiLop = ThuocKhoiLop;
                            OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiDoiTuong.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiDoiTuong - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.DapAnCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString();
                            OneItem.KetLuanCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                            OneItem.PhamViPhepToan = PhamViPhepToan;
                            OneItem.LoaiCauHoi = LoaiCauHoi;
                            OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                            ListItem.Add(OneItem);
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangHai")
                {
                    for (int TongHaiDoiTuong = 21; TongHaiDoiTuong <= 30; TongHaiDoiTuong++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiDoiTuong; PhanNhieuHon++)
                        {
                            if ((TongHaiDoiTuong - PhanNhieuHon) % 2 == 0 && (TongHaiDoiTuong - PhanNhieuHon) / 2 > 4)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")<br/>"
                                    + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "$" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString();
                                OneItem.KetLuanCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + " và " + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #endregion

                #region Tạo các bài toán hai đối tượng hơn kém nhau phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)

                #region Tạo các bài toán tổng hai đối tượng phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongHaiDoiTuong")
                {
                    for (int SoHangThuNhat = 20; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat+1; SoHangThuHai <= 100; SoHangThuHai++)
                        {
                            if (SoHangThuNhat + SoHangThuHai <= 100 && SoHangThuNhat + SoHangThuHai > 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi  cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 đến 100 khối lớp 1 dạng 2
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangHai")
                {
                    for (int SoHangThuNhatThem = 25; SoHangThuNhatThem <= 100; SoHangThuNhatThem++)
                    {
                        for (int SoHangThuHaiThem = 25; SoHangThuHaiThem <= 100; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThem = SoHangThuNhatThem + SoHangThuHaiThem + 1; TongHaiDoiTuongSauKhiThem <= 100; TongHaiDoiTuongSauKhiThem++)
                            {
                                if (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem <= 100 && TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem >= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuNhatThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm là: <b>" + SoHangThuNhatThem.ToString().Trim() + " + " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuNhatThem + SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem - SoHangThuNhatThem - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 đến 100 khối lớp 1 dạng 3
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBa")
                {
                    for (int SoHangThuNhatGiam = 10; SoHangThuNhatGiam < 100; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = 10; SoHangThuHaiThem < 100; SoHangThuHaiThem++)
                        {
                            if (SoHangThuHaiThem - SoHangThuNhatGiam > 15)
                            {
                                Random rd=new Random();
                                int TongHaiDoiTuongSauKhiThem = rd.Next(60, 100);
                                if (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem < 100 && TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem >= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " tăng thêm là: <b>" + SoHangThuHaiThem.ToString().Trim() + " - " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThem.ToString().Trim() + " - " + (SoHangThuHaiThem - SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThem + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                               
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 đến 100 khối lớp 1 dạng 4
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangBon")
                {
                    for (int SoHangThuNhatGiam = 5; SoHangThuNhatGiam <100; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiGiam = 5; SoHangThuHaiGiam <100; SoHangThuHaiGiam++)
                        {
                            Random rd = new Random();
                            int TongHaiDoiTuongSauKhiGiam = rd.Next(30, 70);
                            if (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam <= 100 && SoHangThuNhatGiam + SoHangThuHaiGiam>=20)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " giảm bớt " + SoHangThuHaiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm bớt là: <b>" + SoHangThuHaiGiam.ToString().Trim() + " + " + SoHangThuNhatGiam.ToString().Trim() + " = " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                    + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiGiam.ToString().Trim() + " + " + (SoHangThuHaiGiam + SoHangThuNhatGiam).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim();
                                OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiGiam + SoHangThuNhatGiam + SoHangThuHaiGiam).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán hai tổng hai đối tượng phạm vi 30 đến 100 khối lớp 1 dạng 5
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongHaiDoiTuongDangNam")
                {
                    for (int SoHangThuNhatGiam = 30; SoHangThuNhatGiam < 100; SoHangThuNhatGiam++)
                    {
                        for (int SoHangThuHaiThem = 30; SoHangThuHaiThem < 100; SoHangThuHaiThem++)
                        {
                            for (int TongHaiDoiTuongSauKhiThemGiam = 60; TongHaiDoiTuongSauKhiThemGiam < 100; TongHaiDoiTuongSauKhiThemGiam++)
                            {
                                if (TongHaiDoiTuongSauKhiThemGiam - SoHangThuHaiThem > 30 && TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem < 100 && SoHangThuNhatGiam - SoHangThuHaiThem > 20 && TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem > 45)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                        + " Nếu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " giảm bớt " + SoHangThuNhatGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " thêm " + SoHangThuHaiThem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " thì hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuongSauKhiThemGiam.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Số  " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " giảm so với ban đầu là: <b>" + SoHangThuNhatGiam.ToString().Trim() + " - " + SoHangThuHaiThem.ToString().Trim() + " = " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>"
                                        + "- Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b> " + TongHaiDoiTuongSauKhiThemGiam.ToString().Trim() + " + " + (SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + " = " + (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "</b>" + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")" + "<br/>";
                                    OneItem.DapAnCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim();
                                    OneItem.KetLuanCauHoi = (TongHaiDoiTuongSauKhiThemGiam + SoHangThuNhatGiam - SoHangThuHaiThem).ToString().Trim() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 nhiều hơn đối tượng một phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 25; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanHon = 8; PhanHon <= 100 - SoHangThuNhat; PhanHon++)
                        {
                            if (2 * SoHangThuNhat + PhanHon <= 100)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai "
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHon).ToString() + "$" + (2 * SoHangThuNhat + PhanHon).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHon).ToString() + " và " + (2 * SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }
                #endregion

                #region Đối tượng 2 ít hơn đối tượng một phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "HaiDoiTuongHonKemNhau")
                {
                    for (int SoHangThuNhat = 30; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanItHon = 5; PhanItHon < SoHangThuNhat; PhanItHon++)
                        {
                            if (2 * SoHangThuNhat - PhanItHon <= 100 && SoHangThuNhat - PhanItHon > 30)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                    + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + " và cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanItHon.ToString() + " = " + (SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả hai "
                                    + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * SoHangThuNhat - PhanItHon).ToString();
                                OneItem.KetLuanCauHoi = (SoHangThuNhat - PhanItHon).ToString() + " và " + (2 * SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangMot")
                {
                    for (int TongHaiDoiTuong = 50; TongHaiDoiTuong <= 100; TongHaiDoiTuong++)
                    {
                        for (int SoHangThuNhat = 30; SoHangThuNhat < TongHaiDoiTuong; SoHangThuNhat++)
                        {
                            if (TongHaiDoiTuong - SoHangThuNhat > 25)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 1;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiDoiTuong.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiDoiTuong - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.DapAnCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString();
                                OneItem.KetLuanCauHoi = (TongHaiDoiTuong - SoHangThuNhat).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #region Đối đối tượng một và tổng của hai đối tượng, tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "2" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BietTongHaiDoiTuongDangHai")
                {
                    for (int TongHaiDoiTuong = 31; TongHaiDoiTuong <= 100; TongHaiDoiTuong++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiDoiTuong; PhanNhieuHon++)
                        {
                            if ((TongHaiDoiTuong - PhanNhieuHon) % 2 == 0 && (TongHaiDoiTuong - PhanNhieuHon) / 2 > 15)
                            {
                                //Khởi tạo câu hỏi
                                DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                {
                                    MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                }
                                DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                //Lấy hai tên người quan hệ hơn kém
                                int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);

                                //Tạo nội dung câu hỏi
                                OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiDoiTuong.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                    + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                OneItem.SoLuongDapAn = 2;
                                OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")<br/>"
                                    + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                OneItem.DapAnCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "$" + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString();
                                OneItem.KetLuanCauHoi = ((TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + " và " + (TongHaiDoiTuong - (TongHaiDoiTuong - PhanNhieuHon) / 2).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                OneItem.PhamViPhepToan = PhamViPhepToan;
                                OneItem.LoaiCauHoi = LoaiCauHoi;
                                OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                ListItem.Add(OneItem);
                            }
                        }
                    }
                }

                #endregion

                #endregion


                #endregion


                #region Ba đối tượng

                #region Tạo các bài toán tổng ba đối tượng phạm vi 10 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "TongBaDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 9; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 9; SoHangThuHai++)
                        {
                            for (int SoHangThuBa = SoHangThuHai; SoHangThuBa <= 9; SoHangThuBa++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + SoHangThuBa <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". " 
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong +". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuBa.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                            + ". Hỏi cả ba " + MotDoiTuong.TienToChuNgu.ToLower() +" "+ MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                                            + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + SoHangThuBa.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString();
                                    OneItem.KetLuanCauHoi =(SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tổng ba đối tượng phạm vi 20 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "TongBaDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 20; SoHangThuHai++)
                        {
                            for (int SoHangThuBa = SoHangThuHai; SoHangThuBa <= 20; SoHangThuBa++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + SoHangThuBa <= 20 && SoHangThuNhat + SoHangThuHai + SoHangThuBa >10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuBa.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                            + ". Hỏi cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                                            + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + SoHangThuBa.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString();
                                    OneItem.KetLuanCauHoi =(SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tổng ba đối tượng phạm vi 30 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "TongBaDoiTuong")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat; SoHangThuHai <= 30; SoHangThuHai++)
                        {
                            for (int SoHangThuBa = SoHangThuHai; SoHangThuBa <= 30; SoHangThuBa++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + SoHangThuBa <= 30 && SoHangThuNhat + SoHangThuHai + SoHangThuBa > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuBa.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                            + ". Hỏi cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                                            + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + SoHangThuBa.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString();
                                    OneItem.KetLuanCauHoi =(SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán tổng ba đối tượng phạm vi 30 đến 100 khối lớp 1
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "TongBaDoiTuong")
                {
                    for (int SoHangThuNhat = 18; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = SoHangThuNhat+1; SoHangThuHai <= 100 - SoHangThuNhat; SoHangThuHai++)
                        {
                            for (int SoHangThuBa = SoHangThuHai+1; SoHangThuBa <= 100 - SoHangThuNhat - SoHangThuHai; SoHangThuBa++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + SoHangThuBa > 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ". "
                                                            + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuBa.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                                            + ". Hỏi cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> "
                                                            + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + SoHangThuBa.ToString().Trim() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")"; ;
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai + SoHangThuBa).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = "Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tạo các bài toán ba đối tượng hơn kém nhau phạm vi 10 khối lớp 1 (CLS1847290691)

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 2) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMot")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 10 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 10 - (SoHangThuNhat + PhanHonThuNhat); PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 10 && SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 10 && 3 * SoHangThuNhat + 2*PhanHonThuNhat + PhanHonThuHai <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " và " + TenNguoiBa.Ten.Trim()  +" "+ MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"+
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2*PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + 2*PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi =(SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + 2*PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$"+MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 1) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBon")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 10 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 10 - SoHangThuNhat; PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 10 && SoHangThuNhat + PhanHonThuHai <= 10 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " và " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 2) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangHai")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 10 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + PhanHonThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 10 && SoHangThuNhat + PhanHonThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " và " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 1) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBa")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 10 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 10 && SoHangThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim()  + " và " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 hơn tổng của hai đối tượng 1, 2) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangNam")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 10; SoHangThuHai++)
                        {
                            for (int PhanHon = 1; PhanHon <= 10; PhanHon++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + PhanHon <= 10 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon<=10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  +" "+ PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên "+ MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat +SoHangThuHai).ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString()+ "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 kém hơn tổng của hai đối tượng 1, 2) phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangSau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 10; SoHangThuHai++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + SoHangThuHai; PhanKem++)
                            {
                                if (SoHangThuNhat + SoHangThuHai - PhanKem > 0 && SoHangThuNhat + SoHangThuHai - PhanKem <= 10 && SoHangThuNhat + SoHangThuHai <= 10 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  +" "+ MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + " và " + TenNguoiHai.Ten.Trim()  + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng ba đối tượng và đối tượng thức nhất và đối tượng thứ 3. Tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBay")
                {
                    for (int TongBaSoHang = 1; TongBaSoHang <= 10; TongBaSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat <= 10; SoHangThuNhat++)
                        {
                            for (int SoHangThuHai = 1; SoHangThuHai <= 10; SoHangThuHai++)
                            {
                                if (TongBaSoHang - SoHangThuNhat - SoHangThuHai > 0 && TongBaSoHang - SoHangThuNhat - SoHangThuHai <= 10 )
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim()  + ", " + TenNguoiHai.Ten.Trim()  + " và " + TenNguoiBa.Ten.Trim()  +" "+ MotDoiTuong.SoHuu + " " + TongBaSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong+ "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu+ " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongBaSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " - "  + SoHangThuHai.ToString() + " = " + (TongBaSoHang- SoHangThuNhat - SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim()  + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangTam")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 10; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanHon = 1; PhanHon <= 10; PhanHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 10 && TongHaiSoHang - SoHangThuNhat + PhanHon <= 10 && 2*TongHaiSoHang - SoHangThuNhat + PhanHon <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " + " + PhanHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2*TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangChin")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 10; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 10; PhanItHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 10 && TongHaiSoHang - SoHangThuNhat - PhanItHon <= 10 && TongHaiSoHang - SoHangThuNhat - PhanItHon > 0 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon <= 10 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon >0)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " - " + PhanItHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoi")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 10; TongHaiSoHang++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat < TongHaiSoHang; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 10; PhanHonThuHai++)
                            {
                                if ((TongHaiSoHang - PhanHonThuNhat) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanHonThuNhat) / 2 + PhanHonThuNhat;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 + PhanHonThuHai;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So2 <= 10 && So3 <= 10 && Tong3So <= 10)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim()+ "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiMot")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 10; TongHaiSoHang++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiSoHang; PhanNhieuHon++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 10; PhanItHon++)
                            {
                                if ((TongHaiSoHang - PhanNhieuHon) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanNhieuHon) / 2 + PhanNhieuHon;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 - PhanItHon;
                                    int Tong3So = So1+So2+So3;
                                    if (So2 <= 10 && So3 <= 10 && So3>0 && Tong3So <= 10)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiHai")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 1; SoDoiTuongSauKhiChoNhan <= 10; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongChoThuNhat = 1; SoDoiTuongChoThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongChoThuNhat++)
                        {
                            for (int SoDoiTuongChoThuHai = 1; SoDoiTuongChoThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongChoThuNhat; SoDoiTuongChoThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai > 0 && SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongChoThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongChoThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " + " + SoDoiTuongChoThuNhat.ToString() + " + " + SoDoiTuongChoThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() +"(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBa")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 1; SoDoiTuongSauKhiChoNhan <= 10; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhan = 1; SoDoiTuongNhan < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhan++)
                        {
                            for (int SoDoiTuongCho = 1; SoDoiTuongCho < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan; SoDoiTuongCho++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho > 0 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongCho.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhan.ToString() + " + " + SoDoiTuongCho.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 10 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBon")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 1; SoDoiTuongSauKhiChoNhan <= 10; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhanThuNhat = 1; SoDoiTuongNhanThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhanThuNhat++)
                        {
                            for (int SoDoiTuongNhanThuHai = 1; SoDoiTuongNhanThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat; SoDoiTuongNhanThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai > 0 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai <= 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhanThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongNhanThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhanThuNhat.ToString() + " - " + SoDoiTuongNhanThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion 

                #region Tạo các bài toán ba đối tượng hơn kém nhau phạm vi 20 khối lớp 1 (CLS1847290691)

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 2) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMot")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 20 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 20 - (SoHangThuNhat + PhanHonThuNhat); PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 20 &&  SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 20 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai <= 20 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai >10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 1) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBon")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 20 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 20 - SoHangThuNhat; PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 20 && SoHangThuNhat + PhanHonThuHai <= 20 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 20 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 2) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangHai")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 20 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + PhanHonThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 20 && SoHangThuNhat + PhanHonThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem <= 20 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 1) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBa")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 20 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 20 && SoHangThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem <= 20 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 hơn tổng của hai đối tượng 1, 2) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangNam")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 20; SoHangThuHai++)
                        {
                            for (int PhanHon = 1; PhanHon <= 20; PhanHon++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + PhanHon <= 20 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon <= 20 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 kém hơn tổng của hai đối tượng 1, 2) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangSau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 20; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 20; SoHangThuHai++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + SoHangThuHai; PhanKem++)
                            {
                                if (SoHangThuNhat + SoHangThuHai - PhanKem > 0 && SoHangThuNhat + SoHangThuHai - PhanKem <= 20 && SoHangThuNhat + SoHangThuHai <= 20 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem <= 20 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem > 10)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng ba đối tượng và đối tượng thức nhất và đối tượng thứ 2. Tìm đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBay")
                {
                    for (int TongBaSoHang = 11; TongBaSoHang <= 20; TongBaSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongBaSoHang; SoHangThuNhat++)
                        {
                            for (int SoHangThuHai = 1; SoHangThuHai < TongBaSoHang - SoHangThuNhat; SoHangThuHai++)
                            {
                                if (TongBaSoHang - SoHangThuNhat - SoHangThuHai > 0 && TongBaSoHang - SoHangThuNhat - SoHangThuHai <= 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongBaSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongBaSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " - " + SoHangThuHai.ToString() + " = " + (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangTam")
                {
                    for (int TongHaiSoHang = 11; TongHaiSoHang <= 20; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanHon = 1; PhanHon <= 20; PhanHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 20 && TongHaiSoHang - SoHangThuNhat + PhanHon <= 20 && 2 * TongHaiSoHang - SoHangThuNhat + PhanHon <= 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " + " + PhanHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangChin")
                {
                    for (int TongHaiSoHang = 11; TongHaiSoHang <= 20; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 20; PhanItHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 20 && TongHaiSoHang - SoHangThuNhat - PhanItHon <= 20 && TongHaiSoHang - SoHangThuNhat - PhanItHon > 0 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon <= 20 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon > 0)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " - " + PhanItHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoi")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 20; TongHaiSoHang++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat < TongHaiSoHang; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 20; PhanHonThuHai++)
                            {
                                if ((TongHaiSoHang - PhanHonThuNhat) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanHonThuNhat) / 2 + PhanHonThuNhat;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 + PhanHonThuHai;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So2 <= 20 && So3 <= 20 && Tong3So <= 20 && Tong3So >= 10)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiMot")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 20; TongHaiSoHang++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiSoHang; PhanNhieuHon++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 20; PhanItHon++)
                            {
                                if ((TongHaiSoHang - PhanNhieuHon) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanNhieuHon) / 2 + PhanNhieuHon;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 - PhanItHon;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So2 <= 20 && So3 <= 20 && So3 > 0 && Tong3So <= 20 && Tong3So >= 10)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiHai")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 10; SoDoiTuongSauKhiChoNhan <= 20; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongChoThuNhat = 1; SoDoiTuongChoThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongChoThuNhat++)
                        {
                            for (int SoDoiTuongChoThuHai = 1; SoDoiTuongChoThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongChoThuNhat; SoDoiTuongChoThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai > 0 && SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai <= 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongChoThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongChoThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " + " + SoDoiTuongChoThuNhat.ToString() + " + " + SoDoiTuongChoThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBa")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 10; SoDoiTuongSauKhiChoNhan <= 20; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhan = 1; SoDoiTuongNhan < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhan++)
                        {
                            for (int SoDoiTuongCho = 1; SoDoiTuongCho < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan; SoDoiTuongCho++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho > 10 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho <= 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongCho.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhan.ToString() + " + " + SoDoiTuongCho.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBon")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 10; SoDoiTuongSauKhiChoNhan <= 20; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhanThuNhat = 1; SoDoiTuongNhanThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhanThuNhat++)
                        {
                            for (int SoDoiTuongNhanThuHai = 1; SoDoiTuongNhanThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat; SoDoiTuongNhanThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai > 0 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai <= 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhanThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongNhanThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhanThuNhat.ToString() + " - " + SoDoiTuongNhanThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion 

                #region Tạo các bài toán ba đối tượng hơn kém nhau phạm vi 30 khối lớp 1 (CLS1847290691)

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 2) phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMot")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 30 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 30 - (SoHangThuNhat + PhanHonThuNhat); PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 30 && SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 30 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai <= 30 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 1) phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBon")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 30 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 30 - SoHangThuNhat; PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 30 && SoHangThuNhat + PhanHonThuHai <= 30 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 30 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 2) phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangHai")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 30 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + PhanHonThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 30 && SoHangThuNhat + PhanHonThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem <= 30 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 1) phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBa")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat <= 30 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 30 && SoHangThuNhat - PhanKem > 0 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem <= 30 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 hơn tổng của hai đối tượng 1, 2) phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangNam")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 30; SoHangThuHai++)
                        {
                            for (int PhanHon = 1; PhanHon <= 30; PhanHon++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + PhanHon <= 30 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon <= 30 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 kém hơn tổng của hai đối tượng 1, 2) phạm vi 20 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangSau")
                {
                    for (int SoHangThuNhat = 1; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 1; SoHangThuHai <= 30; SoHangThuHai++)
                        {
                            for (int PhanKem = 1; PhanKem <= SoHangThuNhat + SoHangThuHai; PhanKem++)
                            {
                                if (SoHangThuNhat + SoHangThuHai - PhanKem > 0 && SoHangThuNhat + SoHangThuHai - PhanKem <= 30 && SoHangThuNhat + SoHangThuHai <= 30 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem <= 30 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem > 20)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() +" "+ MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng ba đối tượng và đối tượng thức nhất và đối tượng thứ 2. Tìm đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBay")
                {
                    for (int TongBaSoHang = 21; TongBaSoHang <= 30; TongBaSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongBaSoHang; SoHangThuNhat++)
                        {
                            for (int SoHangThuHai = 1; SoHangThuHai < TongBaSoHang - SoHangThuNhat; SoHangThuHai++)
                            {
                                if (TongBaSoHang - SoHangThuNhat - SoHangThuHai > 0 && TongBaSoHang - SoHangThuNhat - SoHangThuHai <= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongBaSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongBaSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " - " + SoHangThuHai.ToString() + " = " + (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangTam")
                {
                    for (int TongHaiSoHang = 21; TongHaiSoHang <= 30; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanHon = 1; PhanHon <= 30; PhanHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 30 && TongHaiSoHang - SoHangThuNhat + PhanHon <= 30 && 2 * TongHaiSoHang - SoHangThuNhat + PhanHon <= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " + " + PhanHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangChin")
                {
                    for (int TongHaiSoHang = 21; TongHaiSoHang <= 30; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 1; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 30; PhanItHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 0 && TongHaiSoHang - SoHangThuNhat <= 30 && TongHaiSoHang - SoHangThuNhat - PhanItHon <= 30 && TongHaiSoHang - SoHangThuNhat - PhanItHon > 0 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon <= 30 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon > 0)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " - " + PhanItHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoi")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 30; TongHaiSoHang++)
                    {
                        for (int PhanHonThuNhat = 1; PhanHonThuNhat < TongHaiSoHang; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 1; PhanHonThuHai <= 30; PhanHonThuHai++)
                            {
                                if ((TongHaiSoHang - PhanHonThuNhat) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanHonThuNhat) / 2 + PhanHonThuNhat;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 + PhanHonThuHai;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So2 <= 30 && So3 <= 30 && Tong3So <= 30 && Tong3So >= 20)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiMot")
                {
                    for (int TongHaiSoHang = 1; TongHaiSoHang <= 30; TongHaiSoHang++)
                    {
                        for (int PhanNhieuHon = 1; PhanNhieuHon < TongHaiSoHang; PhanNhieuHon++)
                        {
                            for (int PhanItHon = 1; PhanItHon <= 30; PhanItHon++)
                            {
                                if ((TongHaiSoHang - PhanNhieuHon) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanNhieuHon) / 2 + PhanNhieuHon;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 - PhanItHon;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So2 <= 30 && So3 <= 30 && So3 > 0 && Tong3So <= 30 && Tong3So >= 20)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiHai")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 20; SoDoiTuongSauKhiChoNhan <= 30; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongChoThuNhat = 1; SoDoiTuongChoThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongChoThuNhat++)
                        {
                            for (int SoDoiTuongChoThuHai = 1; SoDoiTuongChoThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongChoThuNhat; SoDoiTuongChoThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai > 0 && SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai <= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongChoThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongChoThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " + " + SoDoiTuongChoThuNhat.ToString() + " + " + SoDoiTuongChoThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBa")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 20; SoDoiTuongSauKhiChoNhan <= 30; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhan = 5; SoDoiTuongNhan < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhan++)
                        {
                            for (int SoDoiTuongCho = 5; SoDoiTuongCho < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan; SoDoiTuongCho++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho > 20 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho <= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongCho.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhan.ToString() + " + " + SoDoiTuongCho.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBon")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 20; SoDoiTuongSauKhiChoNhan <= 30; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhanThuNhat = 5; SoDoiTuongNhanThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhanThuNhat++)
                        {
                            for (int SoDoiTuongNhanThuHai = 5; SoDoiTuongNhanThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat; SoDoiTuongNhanThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai > 5 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai <= 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhanThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongNhanThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhanThuNhat.ToString() + " - " + SoDoiTuongNhanThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion 

                #region Tạo các bài toán ba đối tượng hơn kém nhau phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 2) phạm vi 30 đên 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMot")
                {
                    for (int SoHangThuNhat = 20; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 5; PhanHonThuNhat <= 100 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 5; PhanHonThuHai <= 100 - (SoHangThuNhat + PhanHonThuNhat); PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 100 && SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 100 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai <= 100 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai > 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 hơn đối tượng 1) phạm vi 30 đên 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBon")
                {
                    for (int SoHangThuNhat = 18; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 10; PhanHonThuNhat <= 100 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 10; PhanHonThuHai <= 100 - SoHangThuNhat; PhanHonThuHai++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 100 && SoHangThuNhat + PhanHonThuHai <= 100 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai <= 100 && 3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai > 30)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuHai.ToString() + " = " + (SoHangThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuHai).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuHai).ToString() + "$" + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuHai).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat + PhanHonThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 2) phạm vi 30 đên 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangHai")
                {
                    for (int SoHangThuNhat = 20; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat = 10; PhanHonThuNhat <= 100 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 10; PhanKem <= SoHangThuNhat + PhanHonThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat <= 100 && SoHangThuNhat + PhanHonThuNhat - PhanKem > 20 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem <= 100 && 3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem > 50)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + PhanHonThuNhat).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (dạng đối tượng 2 hơn đối tượng 1, đối tượng 3 kém đối tượng 1) phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBa")
                {
                    for (int SoHangThuNhat = 15; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int PhanHonThuNhat =10; PhanHonThuNhat <= 100 - SoHangThuNhat; PhanHonThuNhat++)
                        {
                            for (int PhanKem = 10; PhanKem <= SoHangThuNhat; PhanKem++)
                            {
                                if (SoHangThuNhat + PhanHonThuNhat >= 20 && SoHangThuNhat + PhanHonThuNhat <= 100 && SoHangThuNhat - PhanKem > 15 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem <= 100 && 3 * SoHangThuNhat + PhanHonThuNhat - PhanKem > 50)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + PhanHonThuNhat.ToString() + " = " + (SoHangThuNhat + PhanHonThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (SoHangThuNhat + PhanHonThuNhat).ToString().Trim() + " + " + (SoHangThuNhat - PhanKem).ToString().Trim() + " = " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "$" + (SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "$" + (3 * SoHangThuNhat + 2 * PhanHonThuNhat - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + PhanHonThuNhat).ToString() + "; " + (SoHangThuNhat - PhanKem).ToString() + "; " + (3 * SoHangThuNhat + PhanHonThuNhat - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 hơn tổng của hai đối tượng 1, 2) phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangNam")
                {
                    for (int SoHangThuNhat = 15; SoHangThuNhat <= 100; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 15; SoHangThuHai <= 100; SoHangThuHai++)
                        {
                            for (int PhanHon = 10; PhanHon <= 100; PhanHon++)
                            {
                                if (SoHangThuNhat + SoHangThuHai + PhanHon <= 100 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon <= 100 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon > 50)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " + " + PhanHon.ToString() + " = " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Ba đối tượng hơn kém nhau (Biết đối tượng 1, 2. Đối tượng 3 kém hơn tổng của hai đối tượng 1, 2) phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangSau")
                {
                    for (int SoHangThuNhat = 25; SoHangThuNhat <= 30; SoHangThuNhat++)
                    {
                        for (int SoHangThuHai = 25; SoHangThuHai <= 30; SoHangThuHai++)
                        {
                            for (int PhanKem = 15; PhanKem <= SoHangThuNhat + SoHangThuHai; PhanKem++)
                            {
                                if (SoHangThuNhat + SoHangThuHai - PhanKem > 20 && SoHangThuNhat + SoHangThuHai - PhanKem <= 100 && SoHangThuNhat + SoHangThuHai <= 100 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem <= 100 && SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem > 50)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 2;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- Cả hai " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoHangThuNhat.ToString() + " + " + SoHangThuHai.ToString() + " = " + (SoHangThuNhat + SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                        + "- Vì " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn của cả " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " và " + TenNguoiHai.Ten.Trim() + " " + PhanKem.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " nên " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (SoHangThuNhat + SoHangThuHai).ToString() + " - " + PhanKem.ToString() + " = " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + SoHangThuHai.ToString().Trim() + " + " + (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + " = " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "$" + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString();
                                    OneItem.KetLuanCauHoi = (SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "; " + (SoHangThuNhat + SoHangThuHai + SoHangThuNhat + SoHangThuHai - PhanKem).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng ba đối tượng và đối tượng thức nhất và đối tượng thứ 2. Tìm đối tượng còn lại phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangBay")
                {
                    for (int TongBaSoHang = 60; TongBaSoHang < 100; TongBaSoHang++)
                    {
                        for (int SoHangThuNhat = 20; SoHangThuNhat < TongBaSoHang; SoHangThuNhat++)
                        {
                            for (int SoHangThuHai = SoHangThuNhat +1; SoHangThuHai < TongBaSoHang - SoHangThuNhat; SoHangThuHai++)
                            {
                                if (TongBaSoHang - SoHangThuNhat - SoHangThuHai > 25)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongBaSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongBaSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " - " + SoHangThuHai.ToString() + " = " + (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (TongBaSoHang - SoHangThuNhat - SoHangThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangTam")
                {
                    for (int TongHaiSoHang = 50; TongHaiSoHang <= 100; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 20; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanHon = 10; PhanHon <= 100; PhanHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 20 && TongHaiSoHang - SoHangThuNhat <= 100 && TongHaiSoHang - SoHangThuNhat + PhanHon <= 100 && 2 * TongHaiSoHang - SoHangThuNhat + PhanHon <= 100)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " + " + PhanHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat + PhanHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai và biết đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm đối tượng còn lại phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangChin")
                {
                    for (int TongHaiSoHang = 50; TongHaiSoHang <= 100; TongHaiSoHang++)
                    {
                        for (int SoHangThuNhat = 20; SoHangThuNhat < TongHaiSoHang; SoHangThuNhat++)
                        {
                            for (int PhanItHon = 10; PhanItHon <= 100; PhanItHon++)
                            {
                                if (TongHaiSoHang - SoHangThuNhat > 20 && TongHaiSoHang - SoHangThuNhat <= 100 && TongHaiSoHang - SoHangThuNhat - PhanItHon <= 100 && TongHaiSoHang - SoHangThuNhat - PhanItHon > 20 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon <= 100 && 2 * TongHaiSoHang - SoHangThuNhat - PhanItHon > 50)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + SoHangThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                    OneItem.SoLuongDapAn = 3;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + TongHaiSoHang.ToString() + " - " + SoHangThuNhat.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>" +
                                        "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + (TongHaiSoHang - SoHangThuNhat).ToString() + " - " + PhanItHon.ToString() + " = " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                        + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + SoHangThuNhat.ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat).ToString().Trim() + " + " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString().Trim() + " = " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.DapAnCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "$" + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "$" + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString();
                                    OneItem.KetLuanCauHoi = (TongHaiSoHang - SoHangThuNhat).ToString() + "; " + (TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "; " + (2 * TongHaiSoHang - SoHangThuNhat - PhanItHon).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 nhiều hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoi")
                {
                    for (int TongHaiSoHang = 40; TongHaiSoHang <= 100; TongHaiSoHang++)
                    {
                        for (int PhanHonThuNhat = 8; PhanHonThuNhat < TongHaiSoHang; PhanHonThuNhat++)
                        {
                            for (int PhanHonThuHai = 8; PhanHonThuHai <= 100; PhanHonThuHai++)
                            {
                                if ((TongHaiSoHang - PhanHonThuNhat) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanHonThuNhat) / 2 + PhanHonThuNhat;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 + PhanHonThuHai;
                                    int Tong3So = So1 + So2 + So3;
                                    if ( So1>=15 && So1<=100 && So2 <= 100 && So2 >= 15 && So3 <= 100 && So3 >= 15 && Tong3So <= 100)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanHonThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanHonThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Biết tổng hai đối tượng thứ nhất và đối tượng thứ hai. Biết đối tượng thứ hai nhiều hơn đối tượng thứ nhất. Đối tượng thứ 3 ít hơn đối tượng thứ hai. Tìm các đối tượng và tổng các đối tượng còn lại phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiMot")
                {
                    for (int TongHaiSoHang = 50; TongHaiSoHang <= 100; TongHaiSoHang++)
                    {
                        for (int PhanNhieuHon = 10; PhanNhieuHon < TongHaiSoHang; PhanNhieuHon++)
                        {
                            for (int PhanItHon = 10; PhanItHon <= 100; PhanItHon++)
                            {
                                if ((TongHaiSoHang - PhanNhieuHon) % 2 == 0)
                                {
                                    int So2 = (TongHaiSoHang - PhanNhieuHon) / 2 + PhanNhieuHon;
                                    int So1 = TongHaiSoHang - So2;
                                    int So3 = So2 - PhanItHon;
                                    int Tong3So = So1 + So2 + So3;
                                    if (So1 >= 20 && So1 <= 100 && So2 >= 20 && So2 <= 100 && So3 <= 100 && So3 >= 20 && Tong3So <= 100)
                                    {
                                        //Khởi tạo câu hỏi
                                        DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                        //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                        int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                        {
                                            MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                        }
                                        DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                        //Lấy hai tên người quan hệ hơn kém
                                        int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                        int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                        int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                        HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                        HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                        HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                        //Tạo nội dung câu hỏi
                                        OneItem.NoiDungCauHoi = "Hai " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + TongHaiSoHang.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " nhiều hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " " + PhanNhieuHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " ít hơn " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + PhanItHon.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + ". Hỏi mỗi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + ", " + TenNguoiHai.Ten.Trim() + " và " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                            + " và cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.TenDoiTuong + "?";
                                        OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 19256, "");
                                        OneItem.SoLuongDapAn = 4;
                                        OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                        OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                        OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So1.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So2.ToString() + "</b>(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "). <br/>"
                                            + "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + So3.ToString() + "</b>( " + MotDoiTuong.TenDoiTuong + "). <br/> - Cả ba "
                                            + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:<b> " + Tong3So.ToString().Trim() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.DapAnCauHoi = So1.ToString() + "$" + So2.ToString() + "$" + So3.ToString() + "$" + Tong3So.ToString();
                                        OneItem.KetLuanCauHoi = So1.ToString() + "; " + So2.ToString() + "; " + So3.ToString() + "; " + Tong3So.ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                        OneItem.PhamViPhepToan = PhamViPhepToan;
                                        OneItem.LoaiCauHoi = LoaiCauHoi;
                                        OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiHai.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$" + MotDoiTuong.TienToChuNgu + " " + TenNguoiBa.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:$Cả ba " + MotDoiTuong.TienToChuNgu.ToLower() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                        ListItem.Add(OneItem);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiHai")
                {
                    for (int SoDoiTuongSauKhiChoNhan = 30; SoDoiTuongSauKhiChoNhan <= 100; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongChoThuNhat = 15; SoDoiTuongChoThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongChoThuNhat++)
                        {
                            for (int SoDoiTuongChoThuHai = 15; SoDoiTuongChoThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongChoThuNhat; SoDoiTuongChoThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai >80 && SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai <= 100)
                                {
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongChoThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongChoThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " + " + SoDoiTuongChoThuNhat.ToString() + " + " + SoDoiTuongChoThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan + SoDoiTuongChoThuNhat + SoDoiTuongChoThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBa")
                {
                    int Dem = 0;
                    for (int SoDoiTuongSauKhiChoNhan = 40; SoDoiTuongSauKhiChoNhan <= 100; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhan = 15; SoDoiTuongNhan < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhan++)
                        {
                            for (int SoDoiTuongCho = 15; SoDoiTuongCho < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan; SoDoiTuongCho++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho > 70 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho < 100 && Dem <= 2156)
                                {
                                    Dem++;
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và bớt cho " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongCho.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhan.ToString() + " + " + SoDoiTuongCho.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhan + SoDoiTuongCho).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Tìm số đối tượng ban đầu biết sau khi cho hoặc nhận thêm đối tượng phạm vi 30 đến 100 khối lớp 1 (CLS1847290691)
                if (SoLuongDoiTuong.Trim() == "3" && ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && LoaiCauHoi.Trim() == "BaDoiTuongHonKemNhauDangMuoiBon")
                {
                    int Dem = 0;
                    for (int SoDoiTuongSauKhiChoNhan = 30; SoDoiTuongSauKhiChoNhan <= 100; SoDoiTuongSauKhiChoNhan++)
                    {
                        for (int SoDoiTuongNhanThuNhat = 15; SoDoiTuongNhanThuNhat < SoDoiTuongSauKhiChoNhan; SoDoiTuongNhanThuNhat++)
                        {
                            for (int SoDoiTuongNhanThuHai = 15; SoDoiTuongNhanThuHai < SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat; SoDoiTuongNhanThuHai++)
                            {
                                if (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai > 30 && SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai <= 100 && Dem<=1546)
                                {
                                    Dem++;
                                    //Khởi tạo câu hỏi
                                    DoiTuongHonKemNhauModel OneItem = new DoiTuongHonKemNhauModel();

                                    //Lấy ngẫu nhiên một đối tượng không thuộc về tự nhiên
                                    int MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    while (ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong).ThuocVeTuNhien == true)
                                    {
                                        MaDoiTuong = AllToolShare.LayMaNgauNhien(1, 45, "");
                                    }
                                    DoiTuongModel MotDoiTuong = ToolDoiTuongHonKemNhau.MotDoiTuong(MaDoiTuong);

                                    //Lấy hai tên người quan hệ hơn kém
                                    int MaNguoiMot = AllToolShare.LayMaNgauNhien(1, 645, "");
                                    int MaNguoiHai = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim());
                                    int MaNguoiBa = AllToolShare.LayMaNgauNhien(1, 645, MaNguoiMot.ToString().Trim() + "$" + MaNguoiHai.ToString().Trim());
                                    HoTenModel TenNguoiMot = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiMot);
                                    HoTenModel TenNguoiHai = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiHai);
                                    HoTenModel TenNguoiBa = ToolDoiTuongHonKemNhau.DocMotTen(MaNguoiBa);

                                    //Tạo nội dung câu hỏi
                                    OneItem.NoiDungCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " một số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ".  Sau khi " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiHai.Ten.Trim() + " " + SoDoiTuongNhanThuNhat.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + " và nhận thêm từ " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiBa.Ten.Trim() + " " + SoDoiTuongNhanThuHai.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " thì có tổng số " + SoDoiTuongSauKhiChoNhan.ToString() + " " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong
                                        + ". Hỏi ban đầu " + MotDoiTuong.TienToChuNgu.ToLower() + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " bao nhiêu " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + "?";
                                    OneItem.SapXepThuTu = AllToolShare.LayMaNgauNhien(2566, 9256, "");
                                    OneItem.SoLuongDapAn = 1;
                                    OneItem.SoLuongDoiTuong = Convert.ToInt32(SoLuongDoiTuong);
                                    OneItem.ThuocKhoiLop = ThuocKhoiLop;
                                    OneItem.LoiGiaiCauHoi = "- " + MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là: <b>" + SoDoiTuongSauKhiChoNhan.ToString() + " - " + SoDoiTuongNhanThuNhat.ToString() + " - " + SoDoiTuongNhanThuHai.ToString() + " = " + (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "</b>" + " (" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ").";
                                    OneItem.DapAnCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString();
                                    OneItem.KetLuanCauHoi = (SoDoiTuongSauKhiChoNhan - SoDoiTuongNhanThuNhat - SoDoiTuongNhanThuHai).ToString() + "(" + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + ")";
                                    OneItem.PhamViPhepToan = PhamViPhepToan;
                                    OneItem.LoaiCauHoi = LoaiCauHoi;
                                    OneItem.ThanhPhanCauHoi = MotDoiTuong.TienToChuNgu + " " + TenNguoiMot.Ten.Trim() + " " + MotDoiTuong.SoHuu + " số " + MotDoiTuong.DonViTinh + " " + MotDoiTuong.TenDoiTuong + " là:";
                                    ListItem.Add(OneItem);
                                }
                            }
                        }
                    }
                }
                #endregion

                #endregion 

                #endregion

                List<DoiTuongHonKemNhauModel> OrderedList=ListItem.OrderBy(m=>m.SapXepThuTu).ToList<DoiTuongHonKemNhauModel>();

                //Lưu danh sách câu hỏi vào bảng dữ liệu
                foreach (DoiTuongHonKemNhauModel item in OrderedList)
                {
                    DoiTuongHonKemNhauModel FullItem = item;
                    FullItem.MaCauHoi = Guid.NewGuid();
                    ToolDoiTuongHonKemNhau.ThemMoiMotCauHoi(FullItem);
                }

                return RedirectToAction("DanhSachCauHoi/" + ThuocKhoiLop + "/" + SoLuongDoiTuong + "/" + PhamViPhepToan + "/" + LoaiCauHoi, "DoiTuongHonKemNhau");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Xóa tất cả các phép toán
        /// </summary>
        /// <param name="memvar1">Phạm vi của phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacCauHoi()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string SoLuongDoiTuong = Request.Form["SoLuongDoiTuong"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string LoaiCauHoi = Request.Form["LoaiCauHoi"];
                if (String.IsNullOrEmpty(ToolDoiTuongHonKemNhau.XoaCauHoiMotLop(ThuocKhoiLop, Convert.ToInt32(SoLuongDoiTuong), PhamViPhepToan, LoaiCauHoi)))
                {
                    return RedirectToAction("DanhSachCauHoi/" + ThuocKhoiLop + "/" + SoLuongDoiTuong + "/" + PhamViPhepToan + "/" + LoaiCauHoi, "DoiTuongHonKemNhau");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachCauHoi/" + ThuocKhoiLop + "/" + SoLuongDoiTuong + "/" + PhamViPhepToan + "/" + LoaiCauHoi, "Home");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Thêm mới một phép toán hai số hạng
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult ThemPhepToanBaSoHang(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Đọc tất cả danh sách các lớp học
                List<ClassListModel> DanhSachKhoiLop = ToolSystemManager.GetClassList();

                //Tạo Combobox lựa chọn lớp học
                var DSKhoiLop = new SelectList(DanhSachKhoiLop, "ClassListId", "ClassListName", memvar2);
                ViewData["DSKhoiLop"] = DSKhoiLop;

                //Chuyển phạm vi phép toán sang view
                ViewData["PhamViPhepToan"] = memvar1;

                //Chuyển khối lớp sang view
                ViewData["ThuocKhoiLop"] = memvar2;

                //Đọc trang hiện tại
                ViewData["PageCurent"] = Request.Form["PageCurent"];

                return View();
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
        /// <summary>
        /// Thêm mới một phép toán hai số hạng
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult LuuMoiPhepToanBaSoHang(PhepToanBaSoHangModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                model.MaCauHoi = Guid.NewGuid();
                if (ModelState.IsValid)
                {
                    string Mes = ToolPhepToanBaSoHang.SaveAddQuesTwoOperator(model);
                    if (Mes == "")
                    {
                        return RedirectToAction("PhepToanBaSoHang/" + model.PhamViPhepToan + "/" + model.ThuocKhoiLop, "PhepToanBaSoHang");
                    }
                    else
                    {
                        //Đọc tất cả danh sách các lớp học
                        List<ClassListModel> DanhSachKhoiLop = ToolSystemManager.GetClassList();

                        //Tạo Combobox lựa chọn lớp học
                        var DSKhoiLop = new SelectList(DanhSachKhoiLop, "ClassListId", "ClassListName", model.ThuocKhoiLop);
                        ViewData["DSKhoiLop"] = DSKhoiLop;

                        //Chuyển phạm vi phép toán sang view
                        ViewData["PhamViPhepToan"] = model.PhamViPhepToan;

                        //Chuyển khối lớp sang view
                        ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;

                        //Đọc trang hiện tại
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        return View("ThemPhepToanBaSoHang", model);

                    }
                }
                else
                {
                    //Đọc tất cả danh sách các lớp học
                    List<ClassListModel> DanhSachKhoiLop = ToolSystemManager.GetClassList();

                    //Tạo Combobox lựa chọn lớp học
                    var DSKhoiLop = new SelectList(DanhSachKhoiLop, "ClassListId", "ClassListName", model.ThuocKhoiLop);
                    ViewData["DSKhoiLop"] = DSKhoiLop;

                    //Chuyển phạm vi phép toán sang view
                    ViewData["PhamViPhepToan"] = model.PhamViPhepToan;

                    //Chuyển khối lớp sang view
                    ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;

                    //Đọc trang hiện tại
                    ViewData["PageCurent"] = Request.Form["PageCurent"];
                    return View("ThemPhepToanBaSoHang", model);
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

    }
}
