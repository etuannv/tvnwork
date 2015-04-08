using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using TNV.Web.Models;
using System.Data;
using System.IO;
using System.Text;

namespace TNV.Web.Controllers
{
    public class BaiToanThoiGianController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public SystemManagerService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public BaiToanDaySoService ToolBaiToanDaySo { get; set; }
        public BaiToanThoiGianService ToolBaiToanThoiGian { get; set; }
        public static Random rnd = new Random();

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new SystemManagerClass(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolSystemManager == null) { ToolSystemManager = new SystemManagerClass(); }
            if (ToolBaiToanDaySo == null) { ToolBaiToanDaySo = new BaiToanDaySoClass(); }
            if (ToolBaiToanThoiGian == null) { ToolBaiToanThoiGian = new BaiToanThoiGianClass(); }

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
        /// <summary>
        /// Hiển thị danh sách dãy số
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Phân loại dãy số</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult DanhSachBaiToanThoiGian(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                //Đọc danh sách các dãy số
                List<BaiToanThoiGianModel> DanhSachBaiToanThoiGian = ToolBaiToanThoiGian.DanhSachBaiToanThoiGian(memvar1);

                //Khởi tạo trang
                int Demo = ToolBaiToanThoiGian.SoBanGhiTrenMotTrang;
                int step = ToolBaiToanThoiGian.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachBaiToanThoiGian.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachBaiToanThoiGian.Count;
                    StartNumOfRecordInPage = DanhSachBaiToanThoiGian.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachBaiToanThoiGian.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachBaiToanThoiGian.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "BaiToanThoiGian";
                OnPage.Action = "DanhSachBaiToanThoiGian";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiToanThoiGian.Count, NumOfRecordInPage);

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

                return View("DanhSachBaiToanThoiGian", DanhSachBaiToanThoiGian.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Tạo ngẫu nhiên các dãy số
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        [HttpPost]
        public ActionResult TaoTuDongCacBaiToanThoiGian()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<BaiToanThoiGianModel> DSBaiToanThoiGian = new List<BaiToanThoiGianModel>();
                Random rd = new Random();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                #region Sinh ngẫu nhiên các bài toán thời gian
                for (int i = 1; i <= 12; i++)
                {
                    for (int j = 0; j <= 59; j++)
                    {
                        BaiToanThoiGianModel BaiToanMoi = new BaiToanThoiGianModel();
                        BaiToanMoi.MaCauHoi = Guid.NewGuid();
                        BaiToanMoi.Gio = i;
                        BaiToanMoi.Phut = j;
                        BaiToanMoi.Giay = rd.Next(12, 48);
                        string DapAn = "";
                        if (j < 30)
                        {
                            if (i < 10)
                            {
                                DapAn += "0" + i.ToString().Trim() + " giờ ";
                            }
                            else
                            {
                                DapAn += i.ToString().Trim() + " giờ ";
                            }
                            if (j < 10)
                            {
                                DapAn += "0" + j.ToString().Trim() + " phút ";
                            }
                            else
                            {
                                DapAn += j.ToString().Trim() + " phút ";
                            }
                            if (BaiToanMoi.Giay < 10)
                            {
                                DapAn += "0" + BaiToanMoi.Giay.ToString().Trim() + " giây ";
                            }
                            else
                            {
                                DapAn += BaiToanMoi.Giay.ToString().Trim() + " giây ";
                            }
                        }
                        else
                        {
                            if (i + 1 < 10)
                            {
                                DapAn += "0" + (i + 1).ToString().Trim() + " giờ kém ";
                            }
                            else
                            {
                                if (i == 12)
                                {
                                    DapAn += " 01 giờ kém ";
                                }
                                else
                                {
                                    DapAn += (i + 1).ToString().Trim() + " giờ kém ";
                                }
                            }

                            if (60 - j < 10)
                            {
                                DapAn += "0" + (60 - j).ToString().Trim() + " phút kém ";
                            }
                            else
                            {
                                DapAn += (60 - j).ToString().Trim() + " phút kém ";
                            }

                            if (60 - BaiToanMoi.Giay < 10)
                            {
                                DapAn += "0" + (60 - BaiToanMoi.Giay).ToString().Trim() + " giây ";
                            }
                            else
                            {
                                DapAn += (60 - BaiToanMoi.Giay).ToString().Trim() + " giây ";
                            }
                        }
                        
                        BaiToanMoi.SoDapAn = 1;

                        BaiToanMoi.DapAn = DapAn;

                        BaiToanMoi.DapAnSai = LayDapSoSai(DapAn);

                        BaiToanMoi.ThuTuSapXep = rd.Next(2639, 92568);

                        BaiToanMoi.ThuocKhoiLop = ThuocKhoiLop;

                        DSBaiToanThoiGian.Add(BaiToanMoi);
                    }
                }
                #endregion
                List<BaiToanThoiGianModel> BaiToanThoiGianMoi = DSBaiToanThoiGian.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanThoiGianModel>();
                //Lưu các bài toán trên vào bảng
                foreach (BaiToanThoiGianModel item in BaiToanThoiGianMoi)
                {
                    ToolBaiToanThoiGian.ThemMoiMotBaiToanThoiGian(item);
                }

                return RedirectToAction("DanhSachBaiToanThoiGian/" + ThuocKhoiLop, "BaiToanThoiGian");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }



        /// <summary>
        /// Generate list dap an sai
        /// Cần sinh ra 3 đáp án sai, phân cách bởi dấu #
        /// </summary>
        /// <param name="p">dap an ung</param>
        /// <returns></returns>
        private string LayDapSoSai(string dapAnDung)
        {

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbKetQua1 = new StringBuilder();
            StringBuilder sbKetQua2 = new StringBuilder();
            StringBuilder sbKetQua3 = new StringBuilder();
            StringBuilder sbNumberTemp = new StringBuilder();
            bool IsGio = true;
            List<int> BaSoKhac;
            foreach (char c in dapAnDung)
            {
                if (Char.IsDigit(c))
                {
                    // Append so
                    sbNumberTemp.Append(c);
                }
                else
                {
                    // Neu vua het chuoi so thi append so
                    if (sbNumberTemp.Length > 0)
                    {

                        if (IsGio)
                        {
                            BaSoKhac = Generate3SoKhac(0, 13);
                            IsGio = false;
                        }
                        else
                        {
                            BaSoKhac = Generate3SoKhac(0, 30);
                        }
                        sbKetQua1.Append(BaSoKhac[0]);
                        sbKetQua2.Append(BaSoKhac[1]);
                        sbKetQua3.Append(BaSoKhac[2]);
                        sbNumberTemp.Clear();
                    }
                    // Append ki tu
                    sbKetQua1.Append(c);
                    sbKetQua2.Append(c);
                    sbKetQua3.Append(c);
                }
            }
            if (sbNumberTemp.Length > 0)
            {
                BaSoKhac = Generate3SoKhac(0, 30);
                sbKetQua1.Append(BaSoKhac[0]);
                sbKetQua2.Append(BaSoKhac[1]);
                sbKetQua3.Append(BaSoKhac[2]);
                sbNumberTemp.Clear();
            }

            return sbKetQua1.ToString() + "#" + sbKetQua2.ToString() + "#" + sbKetQua3.ToString();
        }


        /// <summary>
        /// Generate ra 3 so tu nhien khac so dau vao va khac nhau
        /// </summary>
        /// <param name="soDauVao"></param>
        /// <returns></returns>
        private List<int> Generate3SoKhac(int min, int max)
        {
            
            List<int> Result = new List<int>();
            int So1 = 0;
            int So2 = 0;
            int So3 = 0;

            // Generate so 1 - la gio nen phai lon hon 1 nho hon 12
            So1 = rnd.Next(min, max);

            // Generate so 2 - la phu nen phai lon hon 0 nho hon 30
            do
            {
                So2 = rnd.Next(min, max);
            }
            while (So2 == So1);

            // Generate so 3
            do
            {
                So3 = rnd.Next(min, max);
            }
            while (So3 == So1 || So3 == So2);

            Result.Add(So1);
            Result.Add(So2);
            Result.Add(So3);
            return Result;
        }


        /// <summary>
        /// Xóa tất cả các bài toán thời gian
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacBaiToanThoiGian()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                if (String.IsNullOrEmpty(ToolBaiToanThoiGian.XoaNhieuBaiToanThoiGian(ThuocKhoiLop)))
                {
                    return RedirectToAction("DanhSachBaiToanThoiGian/" + ThuocKhoiLop , "BaiToanThoiGian");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachDaySo/" + ThuocKhoiLop , "Home");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
    }
}