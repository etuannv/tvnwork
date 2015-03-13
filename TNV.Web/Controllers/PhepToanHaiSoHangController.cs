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
    public class PhepToanHaiSoHangController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }
        public PhepToanHaiSoHangService ToolPhepToanHaiSoHang { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new NewsCategoryClass(); }
            if (ToolPhepToanHaiSoHang == null) { ToolPhepToanHaiSoHang = new PhepToanHaiSoHangClass(); }
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
        public ActionResult PhepToanHaiSoHang(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Gán thuộc khối lớp sang view
                ViewData["ThuocKhoiLop"] = memvar2;

                //Đọc danh sách các phép toán hai số hạng
                List<PhepToanHaiSoHangModel> DanhSachPhepToan = ToolPhepToanHaiSoHang.ListQuesOneOperator(memvar2, memvar1);

                //Khởi tạo trang
                int Demo = ToolPhepToanHaiSoHang.SoBanGhiTrenMotTrang;
                int step = ToolPhepToanHaiSoHang.BuocNhay;
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
                OnPage.Controler = "PhepToanHaiSoHang";
                OnPage.Action = "PhepToanHaiSoHang";
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

                return View("PhepToanHaiSoHang", DanhSachPhepToan.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
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
        public ActionResult TaoTuDongPhepToanHaiSoHang(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Tạo danh sách phép toán
                List<PhepToanHaiSoHangModel> DanhSachPhepToan = new List<PhepToanHaiSoHangModel>();
                Random rd = new Random();
                if (memvar1.Trim().ToLower() == "congphamvi10")
                {
                    #region Khởi tạo phép cộng trong phạm vi 10
                    for (int i = 0; i <= 10; i++)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            if (i + j <= 10 && i + j != 0)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "+";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "+";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i + j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i + j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (memvar1.Trim().ToLower() == "truphamvi10")
                {
                    #region Khởi tạo phép trừ trong phạm vi 10
                    for (int i = 0; i <= 10; i++)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            if (i >= j)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "-";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "-";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i - j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i - j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruPhamVi10";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (memvar1.Trim().ToLower() == "congsosanhphamvi10")
                {
                    #region Khởi tạo phép cộng so sánh trong phạm vi 10
                    for (int i = 0; i <= 10; i++)
                    {
                        for (int j = 0; j <= 10; j++)
                        {
                            if (i + j != 0 && i + j > 5)
                            {
                                for (int k = 3; k <= 10; k++)
                                {
                                    if (i + j < k)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "+";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = "<";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "CongSoSanhPhamVi10";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                    if (i + j > k && i < k && j < k && i + j <= 10)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "+";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = ">";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "CongSoSanhPhamVi10";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }
                if (memvar1.Trim().ToLower() == "trusosanhphamvi10")
                {
                    #region Khởi tạo phép trừ so sánh trong phạm vi 10
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 10; j++)
                        {
                            if (i + j != 0 && i + j > 5)
                            {
                                for (int k = 5; k < 10; k++)
                                {
                                    if (i - j < k && i - j > 0)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "-";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = "<";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "TruSoSanhPhamVi10";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                    if (i - j > k)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "-";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = ">";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "TruSoSanhPhamVi10";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congphamvi20")
                {
                    #region Khởi tạo phép cộng trong phạm vi 20
                    for (int i = 1; i <= 20; i++)
                    {
                        for (int j = 1; j <= 20; j++)
                        {
                            if (i + j <= 20 && i + j >10)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "+";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "+";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i + j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i + j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "truphamvi20")
                {
                    #region Khởi tạo phép trừ trong phạm vi 20
                    for (int i = 0; i <= 20; i++)
                    {
                        for (int j = 0; j <= 20; j++)
                        {
                            if (i -j>=10)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "-";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "-";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i - j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i - j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congsosanhphamvi20")
                {
                    #region Khởi tạo phép cộng so sánh trong phạm vi 20
                    for (int i = 5; i <= 20; i++)
                    {
                        for (int j = 5; j <= 20; j++)
                        {
                            if (i + j > 10)
                            {
                                for (int k = 13; k <= 20; k++)
                                {
                                    if (i + j < k)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "+";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = "<";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "CongSoSanhPhamVi20";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                    if (i + j > k && i < k && j < k && i + j <= 20)
                                    {
                                        PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                        PhepToan.PhepToan = "+";
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                        PhepToan.DapAn = ">";
                                        PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                        PhepToan.PhamViPhepToan = "CongSoSanhPhamVi20";
                                        DanhSachPhepToan.Add(PhepToan);
                                    }
                                }
                            }
                        }

                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "trusosanhphamvi20")
                {
                    #region Khởi tạo phép trừ so sánh trong phạm vi 20
                    for (int i = 5; i < 20; i++)
                    {
                        for (int j = 5; j < 20; j++)
                        {
                            for (int k = 10; k < 20; k++)
                            {
                                if (i - j < k && i - j > 7)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "-";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = "<";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruSoSanhPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                                if (i - j > k)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "-";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = ">";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruSoSanhPhamVi20";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }

                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congphamvi30")
                {
                    #region Khởi tạo phép cộng trong phạm vi 30
                    for (int i = 10; i <= 30; i++)
                    {
                        for (int j = 10; j <= 30; j++)
                        {
                            if (i + j <= 30 && i + j >= 20)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "+";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "+";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i + j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i + j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "truphamvi30")
                {
                    #region Khởi tạo phép trừ trong phạm vi 30
                    for (int i = 20; i <= 30; i++)
                    {
                        for (int j = 20; j <= 30; j++)
                        {
                            if (i - j >= 0)
                            {
                                for (int k = 1; k <= 5; k++)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    if (k == 1)
                                    {
                                        PhepToan.SoHangThuNhat = "?";
                                        PhepToan.DapAn = i.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    }
                                    if (k == 2)
                                    {
                                        PhepToan.PhepToan = "?";
                                        PhepToan.DapAn = "-";
                                    }
                                    else
                                    {
                                        PhepToan.PhepToan = "-";
                                    }
                                    if (k == 3)
                                    {
                                        PhepToan.SoHangThuHai = "?";
                                        PhepToan.DapAn = j.ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.SoHangThuHai = j.ToString().Trim();
                                    }
                                    if (k == 4)
                                    {
                                        PhepToan.DauQuanHe = "?";
                                        PhepToan.DapAn = "=";
                                    }
                                    else
                                    {
                                        PhepToan.DauQuanHe = "=";
                                    }
                                    if (k == 5)
                                    {
                                        PhepToan.KetQuaPhepToan = "?";
                                        PhepToan.DapAn = (i - j).ToString().Trim();
                                    }
                                    else
                                    {
                                        PhepToan.KetQuaPhepToan = (i - j).ToString().Trim();
                                    }
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congsosanhphamvi30")
                {
                    #region Khởi tạo phép cộng so sánh trong phạm vi 30
                    for (int i = 10; i <= 30; i++)
                    {
                        for (int j = 10; j <= 30; j++)
                        {
                            for (int k = 20; k <= 30; k++)
                            {
                                if (i + j < k)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "+";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = "<";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongSoSanhPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                                if (i + j > k && i + j <= 30)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "+";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = ">";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "CongSoSanhPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }

                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "trusosanhphamvi30")
                {
                    #region Khởi tạo phép trừ so sánh trong phạm vi 30
                    for (int i = 20; i <= 30; i++)
                    {
                        for (int j = 10; j <= 30; j++)
                        {
                            for (int k = 10; k < 30; k++)
                            {
                                if (i - j < k && i - j >= 15)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "-";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = "<";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruSoSanhPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                                if (i - j > k)
                                {
                                    PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                    PhepToan.SoHangThuNhat = i.ToString().Trim();
                                    PhepToan.PhepToan = "-";
                                    PhepToan.SoHangThuHai = j.ToString().Trim();
                                    PhepToan.DauQuanHe = "?";
                                    PhepToan.KetQuaPhepToan = k.ToString().Trim();
                                    PhepToan.DapAn = ">";
                                    PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                    PhepToan.PhamViPhepToan = "TruSoSanhPhamVi30";
                                    DanhSachPhepToan.Add(PhepToan);
                                }
                            }
                        }

                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congkhongnho")
                {
                    #region Khởi tạo phép cộng không nhớ phạm vi nhỏ hơn 100
                    for (int i = 1; i <= 9; i++)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            for (int k = 1; k <= 9; k++)
                            {
                                for (int m = 1; m <= 9; m++)
                                {
                                    if (i + k < 10 && j + m < 10 && 10 * i + j + 10 * k + m <99)
                                    {
                                        int SoThuNhat = i * 10 + j;
                                        int SoThuHai = k * 10 + m;
                                        for (int p = 1; p <= 3; p++)
                                        {
                                            PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                            if (p == 1)
                                            {
                                                PhepToan.SoHangThuNhat = "?";
                                                PhepToan.DapAn = SoThuNhat.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuNhat = SoThuNhat.ToString().Trim();
                                            }
                                            
                                            if (p == 2)
                                            {
                                                PhepToan.SoHangThuHai = "?";
                                                PhepToan.DapAn = SoThuHai.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuHai = SoThuHai.ToString().Trim();
                                            }
                                            
                                            if (p == 3)
                                            {
                                                PhepToan.KetQuaPhepToan = "?";
                                                PhepToan.DapAn = (SoThuNhat+SoThuHai).ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.KetQuaPhepToan = (SoThuNhat + SoThuHai).ToString().Trim();
                                            }
                                            PhepToan.DauQuanHe = "=";
                                            PhepToan.PhepToan = "+";
                                            PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                            PhepToan.PhamViPhepToan = "CongKhongNho";
                                            DanhSachPhepToan.Add(PhepToan);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "trukhongnho")
                {
                    #region Khởi tạo phép cộng không nhớ phạm vi nhỏ hơn 100
                    for (int i = 1; i <= 9; i++)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            for (int k = 1; k <= 9; k++)
                            {
                                for (int m = 1; m <= 9; m++)
                                {
                                    if (i - k >= 0 && j - m >= 0 && 10 * i + j - 10 * k - m > 10)
                                    {
                                        int SoThuNhat = i * 10 + j;
                                        int SoThuHai = k * 10 + m;
                                        for (int p = 1; p <= 3; p++)
                                        {
                                            PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                            if (p == 1)
                                            {
                                                PhepToan.SoHangThuNhat = "?";
                                                PhepToan.DapAn = SoThuNhat.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuNhat = SoThuNhat.ToString().Trim();
                                            }

                                            if (p == 2)
                                            {
                                                PhepToan.SoHangThuHai = "?";
                                                PhepToan.DapAn = SoThuHai.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuHai = SoThuHai.ToString().Trim();
                                            }

                                            if (p == 3)
                                            {
                                                PhepToan.KetQuaPhepToan = "?";
                                                PhepToan.DapAn = (SoThuNhat - SoThuHai).ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.KetQuaPhepToan = (SoThuNhat - SoThuHai).ToString().Trim();
                                            }
                                            PhepToan.DauQuanHe = "=";
                                            PhepToan.PhepToan = "-";
                                            PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                            PhepToan.PhamViPhepToan = "TruKhongNho";
                                            DanhSachPhepToan.Add(PhepToan);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "congconho")
                {
                    #region Khởi tạo phép cộng có nhớ phạm vi nhỏ hơn 100
                    for (int i = 1; i <= 9; i++)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            for (int k = 1; k <= 9; k++)
                            {
                                for (int m = 1; m <= 9; m++)
                                {
                                    if (i + k < 10 && j + m > 10 && 10 * i + j + 10 * k + m < 99)
                                    {
                                        int SoThuNhat = i * 10 + j;
                                        int SoThuHai = k * 10 + m;
                                        for (int p = 1; p <= 3; p++)
                                        {
                                            PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                            if (p == 1)
                                            {
                                                PhepToan.SoHangThuNhat = "?";
                                                PhepToan.DapAn = SoThuNhat.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuNhat = SoThuNhat.ToString().Trim();
                                            }

                                            if (p == 2)
                                            {
                                                PhepToan.SoHangThuHai = "?";
                                                PhepToan.DapAn = SoThuHai.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuHai = SoThuHai.ToString().Trim();
                                            }

                                            if (p == 3)
                                            {
                                                PhepToan.KetQuaPhepToan = "?";
                                                PhepToan.DapAn = (SoThuNhat + SoThuHai).ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.KetQuaPhepToan = (SoThuNhat + SoThuHai).ToString().Trim();
                                            }
                                            PhepToan.DauQuanHe = "=";
                                            PhepToan.PhepToan = "+";
                                            PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                            PhepToan.PhamViPhepToan = "CongCoNho";
                                            DanhSachPhepToan.Add(PhepToan);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (memvar1.Trim().ToLower() == "truconho")
                {
                    #region Khởi tạo phép cộng có nhớ phạm vi nhỏ hơn 100
                    for (int i = 1; i <= 9; i++)
                    {
                        for (int j = 1; j <= 9; j++)
                        {
                            for (int k = 1; k <= 9; k++)
                            {
                                for (int m = 1; m <= 9; m++)
                                {
                                    if (i - k > 0 && j - m < 0 && 10 * i + j - 10 * k - m > 10)
                                    {
                                        int SoThuNhat = i * 10 + j;
                                        int SoThuHai = k * 10 + m;
                                        for (int p = 1; p <= 3; p++)
                                        {
                                            PhepToanHaiSoHangModel PhepToan = new PhepToanHaiSoHangModel();
                                            if (p == 1)
                                            {
                                                PhepToan.SoHangThuNhat = "?";
                                                PhepToan.DapAn = SoThuNhat.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuNhat = SoThuNhat.ToString().Trim();
                                            }

                                            if (p == 2)
                                            {
                                                PhepToan.SoHangThuHai = "?";
                                                PhepToan.DapAn = SoThuHai.ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.SoHangThuHai = SoThuHai.ToString().Trim();
                                            }

                                            if (p == 3)
                                            {
                                                PhepToan.KetQuaPhepToan = "?";
                                                PhepToan.DapAn = (SoThuNhat - SoThuHai).ToString().Trim();
                                            }
                                            else
                                            {
                                                PhepToan.KetQuaPhepToan = (SoThuNhat - SoThuHai).ToString().Trim();
                                            }
                                            PhepToan.DauQuanHe = "=";
                                            PhepToan.PhepToan = "-";
                                            PhepToan.SapXepThuTu = rd.Next(1, 2500);
                                            PhepToan.PhamViPhepToan = "TruCoNho";
                                            DanhSachPhepToan.Add(PhepToan);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                List<PhepToanHaiSoHangModel> DanhSachSapXep = DanhSachPhepToan.OrderBy(m => m.SapXepThuTu).ToList<PhepToanHaiSoHangModel>();

                //Lưu danh sách vào cơ sở dữ liệu
                foreach (PhepToanHaiSoHangModel item in DanhSachSapXep)
                {
                    PhepToanHaiSoHangModel AddItem = item;
                    AddItem.MaCauHoi = Guid.NewGuid();
                    AddItem.ThuocKhoiLop = memvar2;
                    ToolPhepToanHaiSoHang.SaveAddQuesOneOperator(AddItem);
                }
                return RedirectToAction("PhepToanHaiSoHang/" + memvar1 + "/" + memvar2, "PhepToanHaiSoHang");
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
        public ActionResult XoaTatCacPhepToanHaiSoHang(string memvar2, string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (String.IsNullOrEmpty(ToolPhepToanHaiSoHang.DelAllQuesOneOperator(memvar2, memvar1)))
                {
                    return RedirectToAction("PhepToanHaiSoHang/" + memvar1 + "/" + memvar2, "PhepToanHaiSoHang");
                }
                else
                {
                    return RedirectToAction("ViewError/PhepToanHaiSoHang/" + memvar1 + "/MenuQuanTriToanLopMot", "Home");
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
        public ActionResult ThemPhepToanHaiSoHang(string memvar1, string memvar2)
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
        public ActionResult LuuMoiPhepToanHaiSoHang(PhepToanHaiSoHangModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                model.MaCauHoi = Guid.NewGuid();
                if (ModelState.IsValid)
                {
                    string Mes = ToolPhepToanHaiSoHang.SaveAddQuesOneOperator(model);
                    if (Mes == "")
                    {
                        return RedirectToAction("PhepToanHaiSoHang/" + model.PhamViPhepToan + "/" + model.ThuocKhoiLop, "PhepToanHaiSoHang");
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
                        return View("ThemPhepToanHaiSoHang", model);

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
                    return View("ThemPhepToanHaiSoHang", model);
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

    }
}
