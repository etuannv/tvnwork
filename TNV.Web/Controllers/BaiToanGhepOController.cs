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

namespace TNV.Web.Controllers
{
    public class BaiToanGhepOController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public SystemManagerService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public BaiToanDaySoService ToolBaiToanDaySo { get; set; }
        public BaiToanGhepOService ToolBaiToanGhepO { get; set; }
        public BaiToanThoiGianService ToolBaiToanThoiGian { get; set; }

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
            if (ToolBaiToanGhepO == null) { ToolBaiToanGhepO = new BaiToanGhepOClass(); }

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

        #region Bài toán ghép Ô

        /// <summary>
        /// Hiển thị danh sách bài toán ghép ô
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult DanhSachBaiToanGhepO(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["PhamViPhepToan"] = memvar2;
                ViewData["ChieuNgang"] = memvar3;
                ViewData["ChieuDoc"] = memvar4;
                ViewData["LoaiBaiToan"] = memvar5;

                //Đọc danh sách các dãy số
                List<BaiToanGhepOModel> DanhSachBaiToanGhepO = ToolBaiToanGhepO.DanhSachBaiToanGhepO(memvar1, memvar2, Convert.ToInt32(memvar3), Convert.ToInt32(memvar4), memvar5);

                //Khởi tạo trang
                int Demo = ToolBaiToanGhepO.SoBanGhiTrenMotTrang;
                int step = ToolBaiToanGhepO.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachBaiToanGhepO.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachBaiToanGhepO.Count;
                    StartNumOfRecordInPage = DanhSachBaiToanGhepO.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachBaiToanGhepO.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachBaiToanGhepO.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "BaiToanGhepO";
                OnPage.Action = "DanhSachBaiToanGhepO";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = memvar3;
                OnPage.memvar5 = memvar4 + "/" + memvar5;
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiToanGhepO.Count, NumOfRecordInPage);

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

                return View("DanhSachBaiToanGhepO", DanhSachBaiToanGhepO.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
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
        public ActionResult TaoTuDongCacBaiToanGhepO()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<BaiToanGhepOModel> DSBaiToanGhepO = new List<BaiToanGhepOModel>();
                Random rd = new Random();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string ChieuNgang = Request.Form["ChieuNgang"];
                string ChieuDoc = Request.Form["ChieuDoc"];
                string LoaiBaiToan = Request.Form["LoaiBaiToan"];

                #region Sinh ngẫu nhiên các bài toán ghép ô

                #region Phạm vi 10

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4x3
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4x4
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9; int So7 = 2; int So8 = 3;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 10);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 10);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4x4
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9; int So7 = 10; int So8 = 3; int So9 = 1; int So10 = 2;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 10);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 10);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 10);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 10);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                        int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                        int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                        int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 20

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*3 
                    for (int GT = 10; GT <= 14; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        for (int k = 1; k <= 400; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*4
                    for (int GT = 10; GT <= 13; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                        for (int k = 1; k <= 500; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 5*4
                    for (int GT = 10; GT <= 11; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                        for (int k = 1; k <= 1000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*4

                    int So1 = 10; int So2 = 11; int So3 = 12; int So4 = 13; int So5 = 14; int So6 = 15; int So7 = 16; int So8 = 17; ; int So9 = 18; int So10 = 19; int So11 = 8; int So12 = 9;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 20);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 20);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                        int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                        int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                        int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                        int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                        int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                        int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                        int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*5

                    int So1 = 10; int So2 = 11; int So3 = 12; int So4 = 13; int So5 = 14; int So6 = 15; int So7 = 16; int So8 = 17; ; int So9 = 18; int So10 = 19; int So11 = 8; int So12 = 9; int So13 = 5; int So14 = 6; int So15 = 7;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 20);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 20);

                    List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 20);

                    List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 20);

                    List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 20);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                        int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                        int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                        int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                        int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                        int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                        int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                        int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                        int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                        int MaDS152 = AllToolShare.LayMaNgauNhien(1, DS15.Count, MaDS151.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS152));

                        int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                        int MaDS132 = AllToolShare.LayMaNgauNhien(1, DS13.Count, MaDS131.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS132));

                        int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                        int MaDS142 = AllToolShare.LayMaNgauNhien(1, DS14.Count, MaDS141.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS142));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }
                #endregion

                #region Phạm vi 30

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*3
                    for (int GT = 20; GT <= 24; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        for (int k = 1; k <= 400; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*4
                    for (int GT = 20; GT <= 23; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        for (int k = 1; k <= 500; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 5*4
                    for (int GT = 20; GT <= 21; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                        for (int k = 1; k <= 1000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*4

                    int So1 = 20; int So2 = 21; int So3 = 22; int So4 = 23; int So5 = 24; int So6 = 25; int So7 = 26; int So8 = 27; ; int So9 = 28; int So10 = 29; int So11 = 18; int So12 = 19;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                        int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                        int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                        int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                        int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                        int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                        int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                        int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*5

                    int So1 = 20; int So2 = 21; int So3 = 22; int So4 = 23; int So5 = 24; int So6 = 25; int So7 = 26; int So8 = 27 ; int So9 = 28; int So10 = 29; int So11 = 18; int So12 = 19; int So13 = 15; int So14 = 16; int So15 = 17;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                    List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 30);

                    List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 30);

                    List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 30);

                    for (int k = 1; k <= 2000; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                        int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                        int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                        int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                        int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                        int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                        int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                        int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                        int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                        int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                        int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                        int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                        int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                        int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                        int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                        int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                        int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                        int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                        int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                        int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                        int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                        int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                        int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                        int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                        int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                        int MaDS152 = AllToolShare.LayMaNgauNhien(1, DS15.Count, MaDS151.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS152));

                        int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                        int MaDS132 = AllToolShare.LayMaNgauNhien(1, DS13.Count, MaDS131.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS132));

                        int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                        int MaDS142 = AllToolShare.LayMaNgauNhien(1, DS14.Count, MaDS141.ToString().Trim());
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));
                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS142));

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "6" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*6
                    for (int GT = 12; GT <= 12; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14; int So16 = GT + 15; int So17 = GT + 16; int So18 = GT + 17;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 30);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 30);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 30);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(So16, 30);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(So17, 30);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(So18, 30);

                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            int MaDS132 = AllToolShare.LayMaNgauNhien(1, DS13.Count, MaDS131.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS132));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            int MaDS142 = AllToolShare.LayMaNgauNhien(1, DS14.Count, MaDS141.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS142));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            int MaDS152 = AllToolShare.LayMaNgauNhien(1, DS15.Count, MaDS151.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS152));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            int MaDS162 = AllToolShare.LayMaNgauNhien(1, DS16.Count, MaDS161.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS162));

                            int MaDS171 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                            int MaDS172 = AllToolShare.LayMaNgauNhien(1, DS17.Count, MaDS171.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS171));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS172));

                            int MaDS181 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                            int MaDS182 = AllToolShare.LayMaNgauNhien(1, DS18.Count, MaDS181.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS181));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS182));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                #endregion

                #region Phạm vi 30 đến 100

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*3
                    for (int GT = 31; GT <= 94; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        for (int k = 1; k <= 30; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 4*4
                    for (int GT = 31; GT <= 92; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        for (int k = 1; k <= 35; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 5*4
                    for (int GT = 31; GT <= 90; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        for (int k = 1; k <= 40; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*4
                    for (int GT = 31; GT <= 87; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        for (int k = 1; k <= 45; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*5
                    for (int GT = 31; GT <= 84; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 100);

                        for (int k = 1; k <= 50; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            int MaDS152 = AllToolShare.LayMaNgauNhien(1, DS15.Count, MaDS151.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS152));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            int MaDS132 = AllToolShare.LayMaNgauNhien(1, DS13.Count, MaDS131.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS132));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            int MaDS142 = AllToolShare.LayMaNgauNhien(1, DS14.Count, MaDS141.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS142));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "6" && LoaiBaiToan == "BaiToanGhepO")
                {
                    #region Kích thước 6*5
                    for (int GT = 31; GT <= 81; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14; int So16 = GT + 15; int So17 = GT + 16; int So18 = GT + 17;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 100);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(So16, 100);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(So17, 100);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(So18, 100);

                        for (int k = 1; k <= 50; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            int MaDS12 = AllToolShare.LayMaNgauNhien(1, DS1.Count, MaDS11.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS12));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            int MaDS22 = AllToolShare.LayMaNgauNhien(1, DS2.Count, MaDS21.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS22));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            int MaDS32 = AllToolShare.LayMaNgauNhien(1, DS3.Count, MaDS31.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS32));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            int MaDS42 = AllToolShare.LayMaNgauNhien(1, DS4.Count, MaDS41.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS42));


                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            int MaDS52 = AllToolShare.LayMaNgauNhien(1, DS5.Count, MaDS51.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS52));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            int MaDS62 = AllToolShare.LayMaNgauNhien(1, DS6.Count, MaDS61.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS62));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            int MaDS72 = AllToolShare.LayMaNgauNhien(1, DS7.Count, MaDS71.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS72));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            int MaDS82 = AllToolShare.LayMaNgauNhien(1, DS8.Count, MaDS81.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS82));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            int MaDS92 = AllToolShare.LayMaNgauNhien(1, DS9.Count, MaDS91.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS92));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            int MaDS102 = AllToolShare.LayMaNgauNhien(1, DS10.Count, MaDS101.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS102));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            int MaDS112 = AllToolShare.LayMaNgauNhien(1, DS11.Count, MaDS111.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS112));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            int MaDS122 = AllToolShare.LayMaNgauNhien(1, DS12.Count, MaDS121.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS122));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            int MaDS132 = AllToolShare.LayMaNgauNhien(1, DS13.Count, MaDS131.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS132));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            int MaDS142 = AllToolShare.LayMaNgauNhien(1, DS14.Count, MaDS141.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS142));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            int MaDS152 = AllToolShare.LayMaNgauNhien(1, DS15.Count, MaDS151.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS152));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            int MaDS162 = AllToolShare.LayMaNgauNhien(1, DS16.Count, MaDS161.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS162));

                            int MaDS171 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                            int MaDS172 = AllToolShare.LayMaNgauNhien(1, DS17.Count, MaDS171.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS171));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS172));

                            int MaDS181 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                            int MaDS182 = AllToolShare.LayMaNgauNhien(1, DS18.Count, MaDS181.ToString().Trim());
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS181));
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS182));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                    
                #endregion

                #endregion

                #region Sinh ngẫu nhiên bài toán đọc số

                #region Phạm vi 10

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4x3
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }
                        
                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4x4
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9; int So7 = 2; int So8 = 3;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 10);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 10);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 5x4
                    int So1 = 4; int So2 = 5; int So3 = 6; int So4 = 7; int So5 = 8; int So6 = 9; int So7 = 2; int So8 = 3; int So9 = 0; int So10 = 1;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 10);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 10);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 10);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 10);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 10);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 10);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 10);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 10);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 10);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 10);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }
                #endregion

                #region Phạm vi 20

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*3
                    for (int GT = 10; GT <= 14; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        for (int k = 1; k <= 150; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*4
                    for (int GT = 10; GT <= 13; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                        for (int k = 1; k <= 150; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 5*4
                    for (int GT = 10; GT <= 11; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                        for (int k = 1; k <= 300; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*4

                    int So1 = 10; int So2 = 11; int So3 = 12; int So4 = 13; int So5 = 14; int So6 = 15; int So7 = 16; int So8 = 17; ; int So9 = 18; int So10 = 19; int So11 = 8; int So12 = 9;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 20);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 20);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = AllToolShare.LayMaNgauNhien(1,569875,"");
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*5

                    int So1 = 10; int So2 = 11; int So3 = 12; int So4 = 13; int So5 = 14; int So6 = 15; int So7 = 16; int So8 = 17; ; int So9 = 18; int So10 = 19; int So11 = 8; int So12 = 9; int So13 = 5; int So14 = 6; int So15 = 7;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 20);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 20);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 20);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 20);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 20);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 20);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 20);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 20);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 20);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 20);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 20);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 20);

                    List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 20);

                    List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 20);

                    List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 20);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 30

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*3
                    for (int GT = 20; GT <= 24; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        for (int k = 1; k <= 100; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*4
                    for (int GT = 20; GT <= 23; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        for (int k = 1; k <= 150; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 5*4
                    for (int GT = 20; GT <= 21; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                        for (int k = 1; k <= 250; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*4

                    int So1 = 20; int So2 = 21; int So3 = 22; int So4 = 23; int So5 = 24; int So6 = 25; int So7 = 26; int So8 = 27; ; int So9 = 28; int So10 = 29; int So11 = 18; int So12 = 19;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*5

                    int So1 = 20; int So2 = 21; int So3 = 22; int So4 = 23; int So5 = 24; int So6 = 25; int So7 = 26; int So8 = 27; int So9 = 28; int So10 = 29; int So11 = 18; int So12 = 19; int So13 = 15; int So14 = 16; int So15 = 17;

                    List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                    List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                    List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                    List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                    List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                    List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                    List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                    List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                    List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                    List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                    List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                    List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                    List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 30);

                    List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 30);

                    List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 30);

                    for (int k = 1; k <= 500; k++)
                    {
                        List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                        int GTNN = 0;

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, false));
                        }

                        DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, true));
                        GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                        if (GTNN == 0)
                        {
                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS11));
                        }
                        else
                        {
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, false));
                        }

                        List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                        foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                        {
                            DanhSachBieuThucModel NewItem = Item;
                            NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                            DanhSachMoi.Add(NewItem);
                        }
                        List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                        //Tạo bản ghi để lưu vào Bảng
                        BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                        SaveItem.MaBaiToan = Guid.NewGuid();
                        SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                        SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                        SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                        SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                        string DSDapAn = "";
                        //Lấy danh sách đáp án
                        string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                        for (int h = 0; h <= DSDA.Length - 2; h++)
                        {
                            for (int n = h + 1; n <= DSDA.Length - 1; n++)
                            {
                                if (DSDA[h].Trim() == DSDA[n].Trim())
                                {
                                    if (String.IsNullOrEmpty(DSDapAn))
                                    {
                                        DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                    else
                                    {
                                        DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                    }
                                }
                            }
                        }
                        SaveItem.NoiDungDapAn = DSDapAn;
                        SaveItem.PhamViPhepToan = PhamViPhepToan;
                        SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                        SaveItem.LoaiBaiToan = LoaiBaiToan;
                        SaveItem.ThuTuSapXep = k;
                        ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "6" && ChieuDoc == "6" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*6
                    for (int GT = 12; GT <= 12; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14; int So16 = GT + 15; int So17 = GT + 16; int So18 = GT + 17;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 30);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 30);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 30);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 30);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 30);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 30);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(So16, 30);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(So17, 30);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(So18, 30);

                        for (int k = 1; k <= 500; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So16, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So16, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So17, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So17, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So18, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So18, false));
                            }
                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 30 đến 100

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*3
                    for (int GT = 31; GT <= 94; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        for (int k = 1; k <= 30; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                           List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 4*4
                    for (int GT = 31; GT <= 92; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        for (int k = 1; k <= 35; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 5*4
                    for (int GT = 31; GT <= 90; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        for (int k = 1; k <= 40; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*4
                    for (int GT = 31; GT <= 87; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        for (int k = 1; k <= 45; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                            }

                            

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*5
                    for (int GT = 31; GT <= 84; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 100);

                        for (int k = 1; k <= 50; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, false));
                            }

                            

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "6" && ChieuDoc == "6" && LoaiBaiToan == "BaiToanDocSo")
                {
                    #region Kích thước 6*5
                    for (int GT = 31; GT <= 81; GT++)
                    {
                        int So1 = GT; int So2 = GT + 1; int So3 = GT + 2; int So4 = GT + 3; int So5 = GT + 4; int So6 = GT + 5; int So7 = GT + 6; int So8 = GT + 7; ; int So9 = GT + 8; int So10 = GT + 9; int So11 = GT + 10; int So12 = GT + 11; int So13 = GT + 12; int So14 = GT + 13; int So15 = GT + 14; int So16 = GT + 15; int So17 = GT + 16; int So18 = GT + 17;

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(So1, 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(So2, 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(So3, 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(So4, 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(So5, 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(So6, 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(So7, 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(So8, 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(So9, 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(So10, 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(So11, 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(So12, 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(So13, 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(So14, 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(So15, 100);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(So16, 100);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(So17, 100);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(So18, 100);

                        for (int k = 1; k <= 50; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int GTNN = 0;

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So1, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So2, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So3, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So4, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So5, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So6, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So7, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So8, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So9, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So10, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So11, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So12, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So13, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So14, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So15, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So16, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So16, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So17, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So17, false));
                            }

                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So18, true));
                            GTNN = AllToolShare.LayMaNgauNhien(1, 25689, "") % 2;
                            if (GTNN == 0)
                            {
                                int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS11));
                            }
                            else
                            {
                                DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(So18, false));
                            }

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');
                            for (int h = 0; h <= DSDA.Length - 2; h++)
                            {
                                for (int n = h + 1; n <= DSDA.Length - 1; n++)
                                {
                                    if (DSDA[h].Trim() == DSDA[n].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim() + ";" + (n + 1).ToString().Trim();
                                        }
                                    }
                                }
                            }
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                #endregion


                #endregion

                return RedirectToAction("DanhSachBaiToanGhepO/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "BaiToanGhepO");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Xóa tất cả các bài toán ghép ô
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacBaiToanGhepO()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string ChieuNgang = Request.Form["ChieuNgang"];
                string ChieuDoc = Request.Form["ChieuDoc"];
                string LoaiBaiToan = Request.Form["LoaiBaiToan"];
                if (String.IsNullOrEmpty(ToolBaiToanGhepO.XoaNhieuBaiToanGhepO(ThuocKhoiLop, PhamViPhepToan, Convert.ToInt32(ChieuNgang), Convert.ToInt32(ChieuDoc), LoaiBaiToan)))
                {
                    return RedirectToAction("DanhSachBaiToanGhepO/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "BaiToanGhepO");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachDaySo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "Home");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        #endregion

        #region Bài toán sắp xếp trong bảng

        /// <summary>
        /// Hiển thị danh sách bài toán sắp xếp trong bảng
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Kích thước chiều ngang</param>
        /// <param name="memvar4">Kích thước chiều dọc</param>
        /// <param name="memvar5">Loại bài toán</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult DanhSachBaiToanSapXepBang(string memvar1, string memvar2, string memvar3, string memvar4, string memvar5)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["PhamViPhepToan"] = memvar2;
                ViewData["ChieuNgang"] = memvar3;
                ViewData["ChieuDoc"] = memvar4;
                ViewData["LoaiBaiToan"] = memvar5;

                //Đọc danh sách các dãy số
                List<BaiToanGhepOModel> DanhSachBaiToanGhepO = ToolBaiToanGhepO.DanhSachBaiToanGhepO(memvar1, memvar2, Convert.ToInt32(memvar3), Convert.ToInt32(memvar4), memvar5);

                //Khởi tạo trang
                int Demo = ToolBaiToanGhepO.SoBanGhiTrenMotTrang;
                int step = ToolBaiToanGhepO.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachBaiToanGhepO.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachBaiToanGhepO.Count;
                    StartNumOfRecordInPage = DanhSachBaiToanGhepO.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachBaiToanGhepO.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachBaiToanGhepO.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "BaiToanGhepO";
                OnPage.Action = "DanhSachBaiToanSapXepBang";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = memvar3;
                OnPage.memvar5 = memvar4 + "/" + memvar5;
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiToanGhepO.Count, NumOfRecordInPage);

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

                return View("DanhSachBaiToanSapXepBang", DanhSachBaiToanGhepO.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }

        }

        /// <summary>
        /// Xóa tất cả các bài toán sawps xếp bảng
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacBaiToanSapXepBang()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string ChieuNgang = Request.Form["ChieuNgang"];
                string ChieuDoc = Request.Form["ChieuDoc"];
                string LoaiBaiToan = Request.Form["LoaiBaiToan"];
                if (String.IsNullOrEmpty(ToolBaiToanGhepO.XoaNhieuBaiToanGhepO(ThuocKhoiLop, PhamViPhepToan, Convert.ToInt32(ChieuNgang), Convert.ToInt32(ChieuDoc), LoaiBaiToan)))
                {
                    return RedirectToAction("DanhSachBaiToanSapXepBang/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "BaiToanGhepO");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachDaySo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "Home");
                }
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
        public ActionResult TaoTuDongCacBaiToanSapXepBang()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<BaiToanGhepOModel> DSBaiToanGhepO = new List<BaiToanGhepOModel>();
                Random rd = new Random();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string ChieuNgang = Request.Form["ChieuNgang"];
                string ChieuDoc = Request.Form["ChieuDoc"];
                string LoaiBaiToan = Request.Form["LoaiBaiToan"];
                #region Sinh ngẫu nhiên các bài toán ghép ô

                #region Phạm vi 10

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && ChieuNgang == "3" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 3x3
                    for (int GT = 0; GT <= 1; GT++)
                    {
                        int[] GiaTri = new int[9];
                        for (int i = 0; i <= 8; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 10);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 10);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 10);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 10);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 10);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 10);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 10);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 10);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 10);

                        for (int k = 1; k <= 1000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }
                            
                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                
                #endregion

                #region Phạm vi 20

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "3" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 3*3
                    for (int GT = 10; GT <= 11; GT++)
                    {
                        int[] GiaTri = new int[9];
                        for (int i = 0; i <= 8; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 20);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 20);

                        for (int k = 1; k <= 1000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*3
                    for (int GT = 8; GT <= 8; GT++) // 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 11; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 20);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 20);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 20);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 20);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 20);

                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*4
                    for (int GT = 4; GT <= 4; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 15; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 20);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 20);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 20);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 20);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 20);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 20);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 20);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 20);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 20);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 20);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 20);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 20);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 20);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 20);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 20);

                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                
                #endregion

                #region Phạm vi 30

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "3" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 3*3
                    for (int GT = 20; GT <= 21; GT++)
                    {
                        int[] GiaTri = new int[9];
                        for (int i = 0; i <= 8; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 30);

                        for (int k = 1; k <= 1000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*3
                    for (int GT = 18; GT <= 18; GT++) // 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 11; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 30);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 30);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 30);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 30);

                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*4
                    for (int GT = 14; GT <= 14; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 15; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 30);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 30);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 30);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 30);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 30);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 30);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 30);

                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 5*4
                    for (int GT = 10; GT <= 10; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[20];
                        for (int i = 0; i <= 19; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 30);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 30);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 30);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 20);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 30);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 30);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 30);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 30);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 30);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 30);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 30);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 30);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 30);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 30);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 30);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 30);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[16], 30);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[17], 30);

                        List<DanhSachBieuThucModel> DS19 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[18], 30);

                        List<DanhSachBieuThucModel> DS20 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[19], 30);


                        for (int k = 1; k <= 2000; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            int MaDS171 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS171));

                            int MaDS181 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS181));

                            int MaDS191 = AllToolShare.LayMaNgauNhien(1, DS19.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS19, MaDS191));

                            int MaDS201 = AllToolShare.LayMaNgauNhien(1, DS20.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS20, MaDS201));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 30 đến 100

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "3" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 3*3
                    for (int GT = 30; GT <= 90; GT++)
                    {
                        int[] GiaTri = new int[9];
                        for (int i = 0; i <= 8; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 100);

                        for (int k = 1; k <= 35; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "3" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*3
                    for (int GT = 30; GT <= 88; GT++) // 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 11; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 100);

                        for (int k = 1; k <= 40; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "4" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 4*4
                    for (int GT = 30; GT <= 84; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[16];
                        for (int i = 0; i <= 15; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 100);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 100);

                        for (int k = 1; k <= 40; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "5" && ChieuDoc == "4" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 5*4
                    for (int GT = 30; GT <= 80; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[20];
                        for (int i = 0; i <= 19; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 100);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 100);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[16], 100);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[17], 100);

                        List<DanhSachBieuThucModel> DS19 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[18], 100);

                        List<DanhSachBieuThucModel> DS20 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[19], 100);


                        for (int k = 1; k <= 45; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            int MaDS171 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS171));

                            int MaDS181 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS181));

                            int MaDS191 = AllToolShare.LayMaNgauNhien(1, DS19.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS19, MaDS191));

                            int MaDS201 = AllToolShare.LayMaNgauNhien(1, DS20.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS20, MaDS201));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && ChieuNgang == "5" && ChieuDoc == "5" && LoaiBaiToan == "BaiToanSapXepBang")
                {
                    #region Kích thước 5*4
                    for (int GT = 30; GT <= 75; GT++) //4 5 6 7 8 9 10 11 12 13 14 15 16 17 18 19
                    {
                        int[] GiaTri = new int[25];
                        for (int i = 0; i <= 24; i++)
                        {
                            GiaTri[i] = GT + i;
                        }

                        List<DanhSachBieuThucModel> DS1 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[0], 100);

                        List<DanhSachBieuThucModel> DS2 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[1], 100);

                        List<DanhSachBieuThucModel> DS3 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[2], 100);

                        List<DanhSachBieuThucModel> DS4 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[3], 100);

                        List<DanhSachBieuThucModel> DS5 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[4], 100);

                        List<DanhSachBieuThucModel> DS6 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[5], 100);

                        List<DanhSachBieuThucModel> DS7 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[6], 100);

                        List<DanhSachBieuThucModel> DS8 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[7], 100);

                        List<DanhSachBieuThucModel> DS9 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[8], 100);

                        List<DanhSachBieuThucModel> DS10 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[9], 100);

                        List<DanhSachBieuThucModel> DS11 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[10], 100);

                        List<DanhSachBieuThucModel> DS12 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[11], 100);

                        List<DanhSachBieuThucModel> DS13 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[12], 100);

                        List<DanhSachBieuThucModel> DS14 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[13], 100);

                        List<DanhSachBieuThucModel> DS15 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[14], 100);

                        List<DanhSachBieuThucModel> DS16 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[15], 100);

                        List<DanhSachBieuThucModel> DS17 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[16], 100);

                        List<DanhSachBieuThucModel> DS18 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[17], 100);

                        List<DanhSachBieuThucModel> DS19 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[18], 100);

                        List<DanhSachBieuThucModel> DS20 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[19], 100);

                        List<DanhSachBieuThucModel> DS21 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[20], 100);

                        List<DanhSachBieuThucModel> DS22 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[21], 100);

                        List<DanhSachBieuThucModel> DS23 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[22], 100);

                        List<DanhSachBieuThucModel> DS24 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[23], 100);

                        List<DanhSachBieuThucModel> DS25 = ToolBaiToanGhepO.PhanTichMotSo(GiaTri[24], 100);


                        for (int k = 1; k <= 45; k++)
                        {
                            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();

                            int MaDS11 = AllToolShare.LayMaNgauNhien(1, DS1.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS1, MaDS11));

                            int MaDS21 = AllToolShare.LayMaNgauNhien(1, DS2.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS2, MaDS21));

                            int MaDS31 = AllToolShare.LayMaNgauNhien(1, DS3.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS3, MaDS31));

                            int MaDS41 = AllToolShare.LayMaNgauNhien(1, DS4.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS4, MaDS41));

                            int MaDS51 = AllToolShare.LayMaNgauNhien(1, DS5.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS5, MaDS51));

                            int MaDS61 = AllToolShare.LayMaNgauNhien(1, DS6.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS6, MaDS61));

                            int MaDS71 = AllToolShare.LayMaNgauNhien(1, DS7.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS7, MaDS71));

                            int MaDS81 = AllToolShare.LayMaNgauNhien(1, DS8.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS8, MaDS81));

                            int MaDS91 = AllToolShare.LayMaNgauNhien(1, DS9.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS9, MaDS91));

                            int MaDS101 = AllToolShare.LayMaNgauNhien(1, DS10.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS10, MaDS101));

                            int MaDS111 = AllToolShare.LayMaNgauNhien(1, DS11.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS11, MaDS111));

                            int MaDS121 = AllToolShare.LayMaNgauNhien(1, DS12.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS12, MaDS121));

                            int MaDS131 = AllToolShare.LayMaNgauNhien(1, DS13.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS13, MaDS131));

                            int MaDS141 = AllToolShare.LayMaNgauNhien(1, DS14.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS14, MaDS141));

                            int MaDS151 = AllToolShare.LayMaNgauNhien(1, DS15.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS15, MaDS151));

                            int MaDS161 = AllToolShare.LayMaNgauNhien(1, DS16.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS16, MaDS161));

                            int MaDS171 = AllToolShare.LayMaNgauNhien(1, DS17.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS17, MaDS171));

                            int MaDS181 = AllToolShare.LayMaNgauNhien(1, DS18.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS18, MaDS181));

                            int MaDS191 = AllToolShare.LayMaNgauNhien(1, DS19.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS19, MaDS191));

                            int MaDS201 = AllToolShare.LayMaNgauNhien(1, DS20.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS20, MaDS201));

                            int MaDS211 = AllToolShare.LayMaNgauNhien(1, DS21.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS21, MaDS211));

                            int MaDS221 = AllToolShare.LayMaNgauNhien(1, DS22.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS22, MaDS221));

                            int MaDS231 = AllToolShare.LayMaNgauNhien(1, DS23.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS23, MaDS231));

                            int MaDS241 = AllToolShare.LayMaNgauNhien(1, DS24.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS24, MaDS241));

                            int MaDS251 = AllToolShare.LayMaNgauNhien(1, DS25.Count, "");
                            DSBieuThuc.Add(ToolBaiToanGhepO.LayMotBieuThuc(DS25, MaDS251));

                            List<DanhSachBieuThucModel> DanhSachMoi = new List<DanhSachBieuThucModel>();
                            foreach (DanhSachBieuThucModel Item in DSBieuThuc)
                            {
                                DanhSachBieuThucModel NewItem = Item;
                                NewItem.ThuTuSapXep = rd.Next(25368, 99632);
                                DanhSachMoi.Add(NewItem);
                            }
                            List<DanhSachBieuThucModel> DanhSachSapXep = DanhSachMoi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

                            //Tạo bản ghi để lưu vào Bảng
                            BaiToanGhepOModel SaveItem = new BaiToanGhepOModel();
                            SaveItem.MaBaiToan = Guid.NewGuid();
                            SaveItem.ChieuDoc = Convert.ToInt32(ChieuDoc);
                            SaveItem.ChieuNgang = Convert.ToInt32(ChieuNgang);
                            SaveItem.NoiDungBaiToan = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, true);
                            SaveItem.NoiDungGiaTri = ToolBaiToanGhepO.DocDanhSachBieuThuc(DanhSachSapXep, false);
                            string DSDapAn = "";
                            //Lấy danh sách đáp án
                            string[] DSDA = SaveItem.NoiDungGiaTri.Split('$');

                            for (int i = 0; i <= DSDA.Length - 1; i++)
                            {
                                for (int h = 0; h <= DSDA.Length - 1; h++)
                                {
                                    if (GiaTri[i].ToString().Trim() == DSDA[h].Trim())
                                    {
                                        if (String.IsNullOrEmpty(DSDapAn))
                                        {
                                            DSDapAn += (h + 1).ToString().Trim();
                                        }
                                        else
                                        {
                                            DSDapAn += "$" + (h + 1).ToString().Trim();
                                        }
                                        break;
                                    }
                                }
                            }

                            SaveItem.NoiDungDapAn = DSDapAn;
                            SaveItem.PhamViPhepToan = PhamViPhepToan;
                            SaveItem.ThuocKhoiLop = ThuocKhoiLop;
                            SaveItem.LoaiBaiToan = LoaiBaiToan;
                            SaveItem.ThuTuSapXep = k;
                            ToolBaiToanGhepO.ThemMoiMotBaiToanGhepO(SaveItem);
                        }
                    }
                    #endregion
                }
                #endregion

                #endregion

                return RedirectToAction("DanhSachBaiToanSapXepBang/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + ChieuNgang + "/" + ChieuDoc + "/" + LoaiBaiToan, "BaiToanGhepO");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
        #endregion
    }
}