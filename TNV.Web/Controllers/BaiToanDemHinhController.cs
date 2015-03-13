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
    public class BaiToanDemHinhController : Controller
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
        public BaiToanDemHinhService ToolBaiToanDemHinh { get; set; }

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
            if (ToolBaiToanDemHinh == null) { ToolBaiToanDemHinh = new BaiToanDemHinhClass(); }

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
        public ActionResult DanhSachBaiToanDemHinh(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["PhanLoaiBaiToan"] = memvar2;
                //Đọc danh sách các bài toán đếm hình
                List<BaiToanDemHinhModel> DanhSachBaiToan = ToolBaiToanDemHinh.DanhSachBaiToanDemHinh(memvar1, memvar2);

                //Khởi tạo trang
                int Demo = ToolPhepToanBaSoHang.SoBanGhiTrenMotTrang;
                int step = ToolPhepToanBaSoHang.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachBaiToan.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachBaiToan.Count;
                    StartNumOfRecordInPage = DanhSachBaiToan.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachBaiToan.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachBaiToan.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "DoiTuongHonKemNhau";
                OnPage.Action = "DanhSachCauHoi";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiToan.Count, NumOfRecordInPage);

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
                return View("DanhSachBaiToanDemHinh", DanhSachBaiToan.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Thêm mới một phép bài toán đếm hình
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phan loại bài toán</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult ThemBaiToanDemHinh()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhanLoaiBaiToan = Request.Form["PhanLoaiBaiToan"];
                string PageCurent = Request.Form["PageCurent"];
                ViewData["ThuocKhoiLop"] = ThuocKhoiLop;
                ViewData["PhanLoaiBaiToan"] = PhanLoaiBaiToan;
                ViewData["PageCurent"] = PageCurent;

                return View();
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Sửa một bài toán đếm hình
        /// </summary>
        /// <param name="memvar1">Mã bài toán cần sửa</param>
        /// <returns></returns>
        public ActionResult SuaBaiToanDemHinh(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                BaiToanDemHinhModel BaiToanCanSua = ToolBaiToanDemHinh.DocMotBaiToanDemHinh(memvar1);
                string ThuocKhoiLop = BaiToanCanSua.ThuocKhoiLop;
                string PhanLoaiBaiToan = BaiToanCanSua.PhanLoaiBaiToan;
                ViewData["ThuocKhoiLop"] = ThuocKhoiLop;
                ViewData["PhanLoaiBaiToan"] = PhanLoaiBaiToan;
                ViewData["MaBaiToan"] = BaiToanCanSua.MaBaiToan;
                return View(BaiToanCanSua);
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Lưu mới bài toán đếm hình
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult LuuMoiBaiToanDemHinh(BaiToanDemHinhModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string PageCurent = Request.Form["PageCurent"];
                model.MaBaiToan = Guid.NewGuid();
                model.PhanLoaiBaiToan = model.PhanLoaiBaiToan;
                model.ThuocKhoiLop = model.ThuocKhoiLop;
                if (ModelState.IsValid)
                {
                    string Mes = ToolBaiToanDemHinh.ThemMoiBaiToanDemHinh(model);
                    if (Mes == "")
                    {
                        return RedirectToAction("/DanhSachBaiToanDemHinh/" + model.ThuocKhoiLop + "/" + model.PhanLoaiBaiToan, "BaiToanDemHinh");
                    }
                    else
                    {
                        //Đọc tất cả danh sách các lớp học
                        List<ClassListModel> DanhSachKhoiLop = ToolSystemManager.GetClassList();

                        //Tạo Combobox lựa chọn lớp học
                        ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;
                        ViewData["PhanLoaiBaiToan"] = model.PhanLoaiBaiToan;
                        ViewData["PageCurent"] = PageCurent;

                        return View("ThemBaiToanDemHinh", model);

                    }
                }
                else
                {
                    //Tạo Combobox lựa chọn lớp học
                    ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;
                    ViewData["PhanLoaiBaiToan"] = model.PhanLoaiBaiToan;
                    ViewData["PageCurent"] = PageCurent;

                    return View("ThemBaiToanDemHinh", model);
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
        /// <summary>
        /// Lưu sửa bài toán đếm hình
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult LuuSuaBaiToanDemHinh(BaiToanDemHinhModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string PageCurent = Request.Form["PageCurent"];
                if (ModelState.IsValid)
                {
                    string Mes = ToolBaiToanDemHinh.SuaBaiToanDemHinh(model);
                    if (Mes == "")
                    {
                        return RedirectToAction("/DanhSachBaiToanDemHinh/" + model.ThuocKhoiLop + "/" + model.PhanLoaiBaiToan, "BaiToanDemHinh");
                    }
                    else
                    {
                        //Đọc tất cả danh sách các lớp học
                        List<ClassListModel> DanhSachKhoiLop = ToolSystemManager.GetClassList();

                        //Tạo Combobox lựa chọn lớp học
                        ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;
                        ViewData["PhanLoaiBaiToan"] = model.PhanLoaiBaiToan;
                        ViewData["PageCurent"] = PageCurent;

                        return View("SuaBaiToanDemHinh", model);

                    }
                }
                else
                {
                    //Tạo Combobox lựa chọn lớp học
                    ViewData["ThuocKhoiLop"] = model.ThuocKhoiLop;
                    ViewData["PhanLoaiBaiToan"] = model.PhanLoaiBaiToan;
                    ViewData["PageCurent"] = PageCurent;

                    return View("SuaBaiToanDemHinh", model);
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Xóa bài toán đếm hình
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        public ActionResult XoaBaiToanDemHinh(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                BaiToanDemHinhModel BaiToanCanXoa = ToolBaiToanDemHinh.DocMotBaiToanDemHinh(memvar1);
                ToolBaiToanDemHinh.XoaBaiToanDemHinh(memvar1);
                return RedirectToAction("/DanhSachBaiToanDemHinh/" + BaiToanCanXoa.ThuocKhoiLop + "/" + BaiToanCanXoa.PhanLoaiBaiToan, "BaiToanDemHinh");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
    }
}
