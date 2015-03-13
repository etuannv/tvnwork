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
    public class PhepToanBaSoHangController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }
        public PhepToanHaiSoHangService ToolPhepToanHaiSoHang { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public PhepToanBaSoHangService ToolPhepToanBaSoHang { get; set; }

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
        /// Hiển thị danh sách phép toán
        /// </summary>
        /// <param name="memvar1">Phạm vi phép toán</param>
        /// <param name="memvar2">Thuộc khối lớp</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult PhepToanBaSoHang(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Gán thuộc khối lớp sang view
                ViewData["ThuocKhoiLop"] = memvar2;

                //Đọc danh sách các phép toán hai số hạng
                List<PhepToanBaSoHangModel> DanhSachPhepToan = ToolPhepToanBaSoHang.ListQuesTwoOperator(memvar2, memvar1);

                //Khởi tạo trang
                int Demo = ToolPhepToanBaSoHang.SoBanGhiTrenMotTrang;
                int step = ToolPhepToanBaSoHang.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachPhepToan.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachPhepToan.Count;
                    StartNumOfRecordInPage = DanhSachPhepToan.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachPhepToan.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachPhepToan.Count;

                ViewData["PhamViPhepToan"] = memvar1;
                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "PhepToanBaSoHang";
                OnPage.Action = "PhepToanBaSoHang";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachPhepToan.Count, NumOfRecordInPage);

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

                return View("PhepToanBaSoHang", DanhSachPhepToan.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
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
        public ActionResult TaoTuDongPhepToanBaSoHang(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Tạo danh sách phép toán
                List<PhepToanBaSoHangModel> DanhSachPhepToan = new List<PhepToanBaSoHangModel>();
                Random rd = new Random();

                if (memvar1.Trim().ToLower() == "congtruphamvi10")
                {
                    #region Khởi tạo phép toán ba số hạng trong phạm vi 10
                    for (int i = 1; i < 10; i++)
                    {
                        for (int j = i; j < 10; j++)
                        {
                            for (int k = j; k < 10; k++)
                            {
                                if (i + j + k <= 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    int rad = rd.Next(1, 2500);
                                    PhepToan.SapXepThuTu = rad;
                                    if (rad % 6 == 0)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 1)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    else if (rad % 6 == 2)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    else if (rad % 6 == 3)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 4)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    if (rad % 6 == 5)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j + k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (i + j - k >= 0 && i + j - k <= 10 && i + j <= 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = k.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j - k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - j + i <= 10 && i + k <= 10 && k - j + i >=0)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = i.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - j + i).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i + j <= 10 && j + k <= 10 && k - i + j >=0)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i + j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i - j >=0 && k - i - j <= 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i - j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congtruphamvi20")
                {
                    #region Khởi tạo phép toán ba số hạng trong phạm vi 20
                    for (int i = 1; i < 20; i++)
                    {
                        for (int j = i; j < 20; j++)
                        {
                            for (int k = j; k < 20; k++)
                            {
                                if (i + j + k <= 20 && i + j + k > 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    int rad = rd.Next(1, 2500);
                                    PhepToan.SapXepThuTu = rad;
                                    if (rad % 6 == 0)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 1)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    else if (rad % 6 == 2)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    else if (rad % 6 == 3)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 4)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    if (rad % 6 == 5)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j + k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (i + j - k > 10 && i + j - k <= 20 && i + j <= 20)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = k.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j - k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - j + i <= 20 && i + k <= 20 && k - j + i > 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = i.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - j + i).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i + j <= 20 && j + k <= 20 && k - i + j > 10)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i + j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i - j > 10 && k - i - j <= 20)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i - j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congtruphamvi30")
                {
                    #region Khởi tạo phép toán ba số hạng trong phạm vi 30
                    for (int i = 1; i < 30; i++)
                    {
                        for (int j = i; j < 30; j++)
                        {
                            for (int k = j; k < 30; k++)
                            {
                                if (i + j + k <= 30 && i + j + k > 20)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    int rad = rd.Next(1, 2500);
                                    PhepToan.SapXepThuTu = rad;
                                    if (rad % 6 == 0)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 1)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    else if (rad % 6 == 2)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    else if (rad % 6 == 3)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 4)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    if (rad % 6 == 5)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j + k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (i + j - k > 20 && i + j - k <= 30 && i + j <= 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = k.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j - k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - j + i <= 30 && i + k <= 30 && k - j + i > 20)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = i.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - j + i).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i + j <= 30 && j + k <= 30 && k - i + j > 20)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i + j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i - j > 20 && k - i - j <= 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i - j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congtruphamvi30den100")
                {
                    #region Khởi tạo phép toán ba số hạng trong phạm vi 30 đến 100
                    for (int i = 1; i < 100; i++)
                    {
                        for (int j = i+1; j < 100; j++)
                        {
                            for (int k = j+1; k < 100; k++)
                            {
                                if (i + j + k <= 100 && i + j + k > 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    int rad = rd.Next(1, 2500);
                                    PhepToan.SapXepThuTu = rad;
                                    if (rad % 6 == 0)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 1)
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    else if (rad % 6 == 2)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = k.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    else if (rad % 6 == 3)
                                    {
                                        PhepToan.SoHangThuNhat = j.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = k.ToString().Trim();
                                    }
                                    else if (rad % 6 == 4)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = i.ToString().Trim();
                                        PhepToan.SoHangThuBa = j.ToString().Trim();
                                    }
                                    if (rad % 6 == 5)
                                    {
                                        PhepToan.SoHangThuNhat = k.ToString().Trim();
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.SoHangThuBa = i.ToString().Trim();
                                    }
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j + k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30Den100";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (i + j - k > 30 && i + j - k <= 100 && i + j <= 100 && i>=30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = k.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (i + j - k).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "+";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30Den100";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - j + i <= 100 && i + k <= 100 && k - j + i > 30 && i >= 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = i.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - j + i).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30Den100";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i + j <= 100 && j + k <= 100 && k - i + j > 30 && i >= 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i + j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "+";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30Den100";
                                    DanhSachPhepToan.Add(PhepToan);
                                }

                                if (k - i - j > 30 && k - i - j <= 100 && i >= 30)
                                {
                                    PhepToanBaSoHangModel PhepToan = new PhepToanBaSoHangModel();
                                    PhepToan.SoHangThuNhat = k.ToString().Trim();
                                    PhepToan.PhepToanThuNhat = "?";
                                    PhepToan.SoHangThuHai = i.ToString().Trim();
                                    PhepToan.PhepToanThuHai = "?";
                                    PhepToan.SoHangThuBa = j.ToString().Trim();
                                    PhepToan.QuanHePhepToan = "=";
                                    PhepToan.KetQuaPhepToan = (k - i - j).ToString().Trim();
                                    PhepToan.DapAnThuNhat = "-";
                                    PhepToan.DapAnThuHai = "-";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongTruPhamVi30Den100";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                List<PhepToanBaSoHangModel> DanhSachSapXep = DanhSachPhepToan.OrderBy(m => m.SapXepThuTu).ToList<PhepToanBaSoHangModel>();

                //Lưu danh sách vào cơ sở dữ liệu
                foreach (PhepToanBaSoHangModel item in DanhSachSapXep)
                {
                    PhepToanBaSoHangModel AddItem = item;
                    AddItem.MaCauHoi = Guid.NewGuid();
                    AddItem.ThuocKhoiLop = memvar2;
                    ToolPhepToanBaSoHang.SaveAddQuesTwoOperator(AddItem);
                }
                return RedirectToAction("PhepToanBaSoHang/" + memvar1 + "/" + memvar2, "PhepToanBaSoHang");
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
        public ActionResult XoaTatCacPhepToanBaSoHang(string memvar2, string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (String.IsNullOrEmpty(ToolPhepToanBaSoHang.DelAllQuesTwoOperator(memvar2, memvar1)))
                {
                    return RedirectToAction("PhepToanBaSoHang/" + memvar1 + "/" + memvar2, "PhepToanBaSoHang");
                }
                else
                {
                    return RedirectToAction("ViewError/PhepToanBaSoHang/" + memvar1 + "/MenuQuanTriToanLopMot", "Home");
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
