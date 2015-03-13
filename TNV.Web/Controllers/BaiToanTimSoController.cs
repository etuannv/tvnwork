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
    public class BaiToanTimSoController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public SystemManagerService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public BaiToanDaySoService ToolBaiToanDaySo { get; set; }
        public BaiToanTimSoService ToolBaiToanTimSo { get; set; }
        public BaiToanThoiGianService ToolBaiToanThoiGian { get; set; }
        public BaiToanDemHinhService ToolBaiToanDemHinh { get; set; }


        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new SystemManagerClass(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolSystemManager == null) { ToolSystemManager = new SystemManagerClass(); }
            if (ToolBaiToanDaySo == null) { ToolBaiToanDaySo = new BaiToanDaySoClass(); }
            if (ToolBaiToanTimSo == null) { ToolBaiToanTimSo = new BaiToanTimSoClass(); }
            if (ToolBaiToanThoiGian == null) { ToolBaiToanThoiGian = new BaiToanThoiGianClass(); }
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
        /// <summary>
        /// Hiển thị danh sách dãy số
        /// </summary>
        /// <param name="memvar1">Thuộc khối lớp</param>
        /// <param name="memvar2">Phạm vi phép toán</param>
        /// <param name="memvar3">Phân loại dãy số</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [Authorize]
        public ActionResult DanhSachBaiToanTimSo(string memvar1, string memvar2, string memvar3)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["PhamViPhepToan"] = memvar2;
                ViewData["PhanLoaiBaiToan"] = memvar3;

                //Đọc danh sách các dãy số
                List<BaiToanTimSoModel> DanhSachBaiToanTimSo = ToolBaiToanTimSo.DanhSachBaiToanTimSo(memvar1, memvar2, memvar3);

                //Khởi tạo trang
                int Demo = ToolBaiToanTimSo.SoBanGhiTrenMotTrang;
                int step = ToolBaiToanTimSo.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachBaiToanTimSo.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachBaiToanTimSo.Count;
                    StartNumOfRecordInPage = DanhSachBaiToanTimSo.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachBaiToanTimSo.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachBaiToanTimSo.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "BaiToanTimSo";
                OnPage.Action = "DanhSachBaiToanTimSo";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = memvar3;
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiToanTimSo.Count, NumOfRecordInPage);

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

                return View("DanhSachBaiToanTimSo", DanhSachBaiToanTimSo.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
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
        public ActionResult TaoTuDongCacBaiToanTimSo()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<BaiToanTimSoModel> DSBaiToanTimSo = new List<BaiToanTimSoModel>();

                Random rd = new Random();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string PhanLoaiBaiToan = Request.Form["PhanLoaiBaiToan"];

                #region Sinh ngẫu nhiên các bài toán tìm số

                #region Bài toán ba số

                #region Phạm vi 10

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 10; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(1, 10, d, 10, 3);
                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }

                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangHai")
                {
                    #region Tổng hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 4; SoPhanTich <= 9; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 9)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 1, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 1, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 1, SoCongThem);

                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item3 in BoThuBa)
                                        {
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "HinhTronBaSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongTamGiac";
                                            }

                                            DSBaiToanTimSo.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBa")
                {
                    #region Tổng hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 4; SoPhanTich <= 9; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 9; SoTruBot++)
                        {

                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 1, SoTruBot);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 1, SoTruBot);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 1, SoTruBot);
                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTich - SoTruBot > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item3 in BoThuBa)
                                        {
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "HinhTronBaSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongTamGiac";
                                            }
                                            DSBaiToanTimSo.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBon")
                {
                    #region Hiệu hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 4; SoPhanTich <= 9; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 9)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 2, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 2, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 2, SoCongThem);

                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item3 in BoThuBa)
                                        {
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "HinhTronBaSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongTamGiac";
                                            }

                                            DSBaiToanTimSo.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangNam")
                {
                    #region Hiệu hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 9; SoTruBot++)
                        {

                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 2, SoTruBot);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 2, SoTruBot);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 2, SoTruBot);
                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item3 in BoThuBa)
                                        {
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "HinhTronBaSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongTamGiac";
                                            }
                                            DSBaiToanTimSo.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 20

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 20; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = new List<CapSoCongModel>();
                        if (d < 4)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(10, 20, d, 20, 3);
                        }
                        else
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(1, 20, d, 20, 3);
                        }

                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }

                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangHai")
                {
                    #region Tổng hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 19)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 1, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 1, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 1, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);

                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "HinhTronBaSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongTamGiac";
                                            }

                                            DSBaiToanTimSo.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBa")
                {
                    #region Tổng hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 19; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 1, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 1, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 1, SoTruBot);

                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBon")
                {
                    #region Hiệu hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 19)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 2, SoCongThem);

                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 2, SoCongThem);

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 2, SoCongThem);

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "HinhTronBaSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongTamGiac";
                                        }

                                        DSBaiToanTimSo.Add(NewItem);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangNam")
                {
                    #region hiệu hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 19; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 2, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 2, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 2, SoTruBot);
                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();

                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 30

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 30; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = new List<CapSoCongModel>();
                        if (d < 5)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(20, 30, d, 30, 3);
                        }
                        else
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(10, 30, d, 30, 3);
                        }

                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }

                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangHai")
                {
                    #region Tổng hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 29)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 1, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 1, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 1, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);

                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "HinhTronBaSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongTamGiac";
                                        }

                                        DSBaiToanTimSo.Add(NewItem);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBa")
                {
                    #region Tổng hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 29; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 1, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 1, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 1, SoTruBot);

                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 15 && SoPhanTichHai - SoTruBot > 15 && SoPhanTichBa - SoTruBot > 15)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBon")
                {
                    #region Hiệu hai số và cộng thêm số không đổi bằng số thứ 3
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem <= 29)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 2, SoCongThem);

                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 2, SoCongThem);

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 2, SoCongThem);

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "HinhTronBaSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongTamGiac";
                                        }

                                        DSBaiToanTimSo.Add(NewItem);
                                    }
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangNam")
                {
                    #region hiệu hai số và trừ số không đổi bằng số thứ 3
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot <= 29; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 2, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 2, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 2, SoTruBot);
                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 15 && SoPhanTichHai - SoTruBot > 15 && SoPhanTichBa - SoTruBot > 15)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();

                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }

                #endregion

                #region Phạm vi 30 đến 100

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 100; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = new List<CapSoCongModel>();
                        if (d < 10)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(80, 100, d, 100, 3);
                        }
                        else if (d < 20)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(60, 100, d, 100, 3);
                        }
                        else if (d < 40)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(50, 100, d, 100, 3);
                        }
                        else
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(40, 100, d, 100, 3);
                        }

                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }

                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangHai")
                {
                    #region Tổng hai số và cộng thêm số không đổi bằng số thứ 3
                    List<BaiToanTimSoModel> DanhSachTamThoi4 = new List<BaiToanTimSoModel>();
                    for (int SoPhanTich = 30; SoPhanTich < 100; SoPhanTich++)
                    {
                        for (int SoCongThem = 30; SoCongThem < 100; SoCongThem++)
                        {
                            if (SoPhanTich + SoCongThem < 100)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 1, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 1, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 1, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);

                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "HinhTronBaSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongTamGiac";
                                        }

                                        DanhSachTamThoi4.Add(NewItem);
                                    }
                                }
                            }
                        }
                    }
                    if (DanhSachTamThoi4.Count < 500)
                    {
                        DSBaiToanTimSo.AddRange(DanhSachTamThoi4);
                    }
                    else
                    {
                        //Lấy 1000 bản ghi
                        List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi4.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                        List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(500).ToList<BaiToanTimSoModel>(); ;
                        DSBaiToanTimSo.AddRange(DSSelect);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBa")
                {
                    #region Tổng hai số và trừ số không đổi bằng số thứ 3
                    List<BaiToanTimSoModel> DanhSachTamThoi3 = new List<BaiToanTimSoModel>();
                    for (int SoPhanTich = 30; SoPhanTich < 100; SoPhanTich++)
                    {
                        for (int SoTruBot = 10; SoTruBot < 100; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 1, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 1, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 1, SoTruBot);

                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 50 && SoPhanTichBa - SoTruBot > 50 && SoPhanTichHai - SoTruBot > 50)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DanhSachTamThoi3.Add(NewItem);
                                }
                            }
                        }
                    }
                    if (DanhSachTamThoi3.Count < 500)
                    {
                        DSBaiToanTimSo.AddRange(DanhSachTamThoi3);
                    }
                    else
                    {
                        //Lấy 1000 bản ghi
                        List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi3.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                        List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(500).ToList<BaiToanTimSoModel>(); ;
                        DSBaiToanTimSo.AddRange(DSSelect);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangBon")
                {
                    #region Hiệu hai số và cộng thêm số không đổi bằng số thứ 3

                    List<BaiToanTimSoModel> DanhSachTamThoi = new List<BaiToanTimSoModel>();

                    for (int SoPhanTich = 30; SoPhanTich < 100; SoPhanTich++)
                    {
                        for (int SoCongThem = 10; SoCongThem < 100; SoCongThem++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 2, SoCongThem);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 2, SoCongThem);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 2, SoCongThem);

                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 100 && SoPhanTichHai + SoCongThem <= 100 && SoPhanTichBa + SoCongThem <= 100)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }

                                    DanhSachTamThoi.Add(NewItem);
                                }
                            }
                        }
                    }
                    if (DanhSachTamThoi.Count < 500)
                    {
                        DSBaiToanTimSo.AddRange(DanhSachTamThoi);
                    }
                    else
                    {
                        //Lấy 1000 bản ghi
                        List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                        List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(500).ToList<BaiToanTimSoModel>(); ;
                        DSBaiToanTimSo.AddRange(DSSelect);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaSoDangNam")
                {
                    #region hiệu hai số và trừ số không đổi bằng số thứ 3

                    List<BaiToanTimSoModel> DanhSachTamThoi1 = new List<BaiToanTimSoModel>();

                    for (int SoPhanTich = 30; SoPhanTich < 100; SoPhanTich++)
                    {
                        for (int SoTruBot = 0; SoTruBot < 100; SoTruBot++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 2, SoTruBot);

                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 2, SoTruBot);

                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 2, SoTruBot);

                            if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTichHai - SoTruBot > 70 && SoPhanTichBa - SoTruBot > 70)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();

                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "HinhTronBaSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongTamGiac";
                                    }
                                    DanhSachTamThoi1.Add(NewItem);
                                }
                            }
                        }
                    }

                    if (DanhSachTamThoi1.Count < 500)
                    {
                        DSBaiToanTimSo.AddRange(DanhSachTamThoi1);
                    }
                    else
                    {
                        //Lấy 1000 bản ghi
                        List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi1.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                        List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(500).ToList<BaiToanTimSoModel>(); ;
                        DSBaiToanTimSo.AddRange(DSSelect);
                    }
                    #endregion
                }

                #endregion

                #endregion

                #region Bài toán bốn số

                #region Phạm vi 10

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 10; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(1, 10, d, 10, 4);
                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 6 == 0)
                                    {
                                        NewItem.UserControlName = "BonSoHinhVuong";
                                    }
                                    else if (NewItem.ThuTuSapXep % 6 == 1)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 6 == 2)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                    }
                                    else if (NewItem.ThuTuSapXep % 6 == 3)
                                    {
                                        NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 6 == 4)
                                    {
                                        NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "BonSoHinhVuong1";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangHai")
                {
                    #region Tổng ba số và cộng thêm số không đổi bằng số thứ 4
                    int sl1 = 300;
                    List<BaiToanTimSoModel> DSPhamVi1 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi1.Count < sl1)
                    {
                        if (DSPhamVi1.Count < sl1)
                        {
                            DSPhamVi1.RemoveRange(0, DSPhamVi1.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi2 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 3, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 3, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 3, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 9 && SoPhanTichHai + SoCongThem <= 9 && SoPhanTichBa + SoCongThem <= 9)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi2.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi2.Count <= sl1)
                        {
                            DSPhamVi1.AddRange(DanhSachTamThoi2);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi2.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl1).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi1.AddRange(DSSelect);
                        }

                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi1);

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBa")
                {
                    #region Tổng ba số và trừ số không đổi bằng số thứ 4
                    int sl2 = 300;
                    List<BaiToanTimSoModel> DSPhamVi2 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi2.Count < sl2)
                    {
                        if (DSPhamVi2.Count < sl2)
                        {
                            DSPhamVi2.RemoveRange(0, DSPhamVi2.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi1 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 3, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 3, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 3, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 6 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 6 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 6 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 6 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 6 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong1";
                                            }
                                            DanhSachTamThoi1.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        if (DanhSachTamThoi1.Count < sl2)
                        {
                            DSPhamVi2.AddRange(DanhSachTamThoi1);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi1.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl2).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi2.AddRange(DSSelect);
                        }
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi2);
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBon")
                {
                    #region Tổng hai số trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4
                    int sl3 = 300;
                    List<BaiToanTimSoModel> DSPhamVi3 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi3.Count < sl3)
                    {
                        if (DSPhamVi3.Count < sl3)
                        {
                            DSPhamVi3.RemoveRange(0, DSPhamVi3.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi3 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 4, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 4, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 4, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 9 && SoPhanTichHai + SoCongThem <= 9 && SoPhanTichBa + SoCongThem <= 9)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi3.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi3.Count < sl3)
                        {
                            DSPhamVi3.AddRange(DanhSachTamThoi3);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi3.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl3).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi3.AddRange(DSSelect);
                        }

                        DSBaiToanTimSo.AddRange(DSPhamVi3);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangNam")
                {
                    #region Tổng hai số trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4
                    int sl4 = 300;
                    List<BaiToanTimSoModel> DSPhamVi4 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi4.Count < sl4)
                    {
                        if (DSPhamVi4.Count < sl4)
                        {
                            DSPhamVi4.RemoveRange(0, DSPhamVi4.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi4 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 4, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 4, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 4, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi4.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi4.Count < sl4)
                        {
                            DSPhamVi4.AddRange(DanhSachTamThoi4);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi4.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl4).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi4.AddRange(DSSelect);
                        }


                        DSBaiToanTimSo.AddRange(DSPhamVi4);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangSau")
                {
                    #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và cộng thêm số không đổi bằng số thứ 4
                    int sl5 = 300;
                    List<BaiToanTimSoModel> DSPhamVi5 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi5.Count < sl5)
                    {
                        if (DSPhamVi5.Count < sl5)
                        {
                            DSPhamVi5.RemoveRange(0, DSPhamVi5.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi5 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 5, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 5, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 5, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 9 && SoPhanTichHai + SoCongThem <= 9 && SoPhanTichBa + SoCongThem <= 9)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi5.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi5.Count < sl5)
                        {
                            DSPhamVi5.AddRange(DanhSachTamThoi5);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi5.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl5).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi5.AddRange(DSSelect);
                        }


                        DSBaiToanTimSo.AddRange(DSPhamVi5);
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBay")
                {
                    #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và trừ bớt số không đổi bằng số thứ 4
                    int sl6 = 300;
                    List<BaiToanTimSoModel> DSPhamVi6 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi6.Count < sl6)
                    {
                        if (DSPhamVi6.Count < sl6)
                        {
                            DSPhamVi6.RemoveRange(0, DSPhamVi6.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi6 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 5, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 5, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 5, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi6.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi6.Count < sl6)
                        {
                            DSPhamVi6.AddRange(DanhSachTamThoi6);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi6.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl6).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi6.AddRange(DSSelect);
                        }


                        DSBaiToanTimSo.AddRange(DSPhamVi6);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangTam")
                {
                    #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4
                    int sl7 = 300;
                    List<BaiToanTimSoModel> DSPhamVi7 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi7.Count < sl7)
                    {
                        if (DSPhamVi7.Count < sl7)
                        {
                            DSPhamVi7.RemoveRange(0, DSPhamVi7.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi7 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 6, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 6, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 6, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 9 && SoPhanTichHai + SoCongThem <= 9 && SoPhanTichBa + SoCongThem <= 9)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi7.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi7.Count < sl7)
                        {
                            DSPhamVi7.AddRange(DanhSachTamThoi7);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi7.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl7).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi7.AddRange(DSSelect);
                        }


                        DSBaiToanTimSo.AddRange(DSPhamVi7);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangChin")
                {
                    #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4
                    int sl8 = 300;
                    List<BaiToanTimSoModel> DSPhamVi8 = new List<BaiToanTimSoModel>();
                    while (DSPhamVi8.Count < sl8)
                    {
                        if (DSPhamVi8.Count < sl8)
                        {
                            DSPhamVi8.RemoveRange(0, DSPhamVi8.Count);
                        }
                        List<BaiToanTimSoModel> DanhSachTamThoi8 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 3; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 6, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 6, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(3, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 6, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item3 in BoThuHai)
                                            {
                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item3);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item3, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item3.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 6 == 0)
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 1)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 2)
                                                {
                                                    NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 3)
                                                {
                                                    NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                                }
                                                else if (NewItem.ThuTuSapXep % 6 == 4)
                                                {
                                                    NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "BonSoHinhVuong1";
                                                }

                                                DanhSachTamThoi8.Add(NewItem);
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi8.Count < sl8)
                        {
                            DSPhamVi8.AddRange(DanhSachTamThoi8);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi8.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl8).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi8.AddRange(DSSelect);
                        }


                        DSBaiToanTimSo.AddRange(DSPhamVi8);
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoi")
                {
                    #region Tổng hai số ở hai vị trí đối diện bằng nhau
                    for (int SoPhanTich = 6; SoPhanTich <= 9; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(6, 9, SoPhanTich.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(6, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện là một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 2 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }
                            else
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiMot")
                {
                    #region Tổng hai số ở nửa phía trên và phía dưới bằng nhau
                    for (int SoPhanTich = 6; SoPhanTich <= 9; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(6, 9, SoPhanTich.ToString().Trim());

                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(6, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở nửa trên và tổng hai số ở nửa dưới của mỗi hình là một số không đổi</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 3 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong";
                            }
                            else if (NewItem.ThuTuSapXep % 3 == 1)
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }
                            else
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiHai")
                {
                    #region Hai số đối diện hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa1 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 1; SoThuNhat <= 9; SoThuNhat++)
                    {
                        for (int SoThuHai = 1; SoThuHai <= 9; SoThuHai++)
                        {
                            for (int SoKhongDoi = 1; SoKhongDoi <= 9; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 9 && SoThuHai + SoKhongDoi <= 9)
                                {
                                    BoThuNhat1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 1));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai >= 9 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai >= 9)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 1));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa >= 9 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa >= 9)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 1));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat2 = BoThuNhat1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhatChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem1 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat2)
                    {
                        Dem1++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem1;
                        BoThuNhatChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai2 = BoThuHai1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHaiChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem2 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai2)
                    {
                        Dem2++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem2;
                        BoThuHaiChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa2 = BoThuBa1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBaChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem3 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa2)
                    {
                        Dem3++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem3;
                        BoThuBaChuanTT.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhatChuanTT)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHaiChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHaiChuanTT, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBaChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBaChuanTT, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở các vị trí đối diện hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 2 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }
                        else
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }

                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiBa")
                {
                    #region Hai số nửa phía trên và phía dưới hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa11 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 1; SoThuNhat <= 9; SoThuNhat++)
                    {
                        for (int SoThuHai = 1; SoThuHai <= 9; SoThuHai++)
                        {
                            for (int SoKhongDoi = 1; SoKhongDoi <= 9; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 9 && SoThuHai + SoKhongDoi <= 9)
                                {
                                    BoThuNhat11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 2));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai >= 9 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai >= 9)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 2));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa >= 9 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa >= 9)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 9, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 2));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat22 = BoThuNhat11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhat23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem11 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat22)
                    {
                        Dem11++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem11;
                        BoThuNhat23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai22 = BoThuHai11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem22 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai22)
                    {
                        Dem22++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem22;
                        BoThuHai23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa22 = BoThuBa11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem33 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa22)
                    {
                        Dem33++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem33;
                        BoThuBa23.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat23)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai23, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa23, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở nửa trên và nửa dưới mỗi hình hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 3 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong";
                        }
                        else if (NewItem.ThuTuSapXep % 3 == 1)
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }
                        else
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }


                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }

                #endregion

                #region Phạm vi 20

                #region Cấp số cộng
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();

                    for (int d = 1; d <= 20; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(10, 20, d, 20, 4);
                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 5 == 0)
                                    {
                                        NewItem.UserControlName = "BonSoHinhVuong";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 1)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 2)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 3)
                                    {
                                        NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 4)
                                    {
                                        NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                    }
                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }
                #endregion

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng ba số và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi2 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 3, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 3, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 3, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi2.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi2.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi2);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi2.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng ba số và trừ số không đổi bằng số thứ 4
                        List<BaiToanTimSoModel> DanhSachTamThoi1 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 3, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 3, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 3, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }
                                            DanhSachTamThoi1.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        if (DanhSachTamThoi1.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi1);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi1.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi3 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 4, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 4, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 4, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi3.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi3.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi3);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi3.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi4 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 4, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 4, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 4, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi4.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi4.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi4);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi4.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi5 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 5, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 5, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 5, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi5.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi5.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi5);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi5.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi6 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 5, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 5, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 5, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi6.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi6.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi6);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi6.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi8 = new List<BaiToanTimSoModel>();
                    int sl8 = 300;
                    while (DSPhamVi8.Count < sl8)
                    {
                        if (DSPhamVi8.Count < sl8)
                        {
                            DSPhamVi8.RemoveRange(0, DSPhamVi8.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi7 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 6, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 6, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 6, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi7.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi7.Count < sl8)
                        {
                            DSPhamVi8.AddRange(DanhSachTamThoi7);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi7.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl8).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi8.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi8);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi9 = new List<BaiToanTimSoModel>();
                    int sl9 = 300;
                    while (DSPhamVi9.Count < sl9)
                    {
                        if (DSPhamVi9.Count < sl9)
                        {
                            DSPhamVi9.RemoveRange(0, DSPhamVi9.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi8 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 6, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 6, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 6, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 10 && SoPhanTichHai - SoTruBot > 10 && SoPhanTichBa - SoTruBot > 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi8.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi8.Count < sl9)
                        {
                            DSPhamVi9.AddRange(DanhSachTamThoi8);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi8.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(sl9).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi9.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi9);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoi")
                {
                    #region Tổng hai số ở hai vị trí đối diện bằng nhau
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1, 3);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1, 3);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1, 3);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện là một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 2 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }
                            else
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiMot")
                {
                    #region Tổng hai số ở nửa phía trên và phía dưới bằng nhau
                    for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2, 3);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());

                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2, 3);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2, 3);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở nửa trên và tổng hai số ở nửa dưới của mỗi hình là một số không đổi</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 3 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong";
                            }
                            else if (NewItem.ThuTuSapXep % 3 == 1)
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }
                            else
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiHai")
                {
                    #region Hai số đối diện hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa1 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 10; SoThuNhat <= 19; SoThuNhat++)
                    {
                        for (int SoThuHai = 10; SoThuHai <= 19; SoThuHai++)
                        {
                            for (int SoKhongDoi = 1; SoKhongDoi <= 19; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 19 && SoThuHai + SoKhongDoi <= 19)
                                {
                                    BoThuNhat1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 1));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 19 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 19)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 1));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 19 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 19)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 1));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat2 = BoThuNhat1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhatChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem1 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat2)
                    {
                        Dem1++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem1;
                        BoThuNhatChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai2 = BoThuHai1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHaiChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem2 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai2)
                    {
                        Dem2++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem2;
                        BoThuHaiChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa2 = BoThuBa1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBaChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem3 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa2)
                    {
                        Dem3++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem3;
                        BoThuBaChuanTT.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhatChuanTT)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHaiChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHaiChuanTT, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBaChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBaChuanTT, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở các vị trí đối diện hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 2 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }
                        else
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }

                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiBa")
                {
                    #region Hai số nửa phía trên và phía dưới hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa11 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 10; SoThuNhat <= 19; SoThuNhat++)
                    {
                        for (int SoThuHai = 10; SoThuHai <= 19; SoThuHai++)
                        {
                            for (int SoKhongDoi = 1; SoKhongDoi <= 19; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 19 && SoThuHai + SoKhongDoi <= 19)
                                {
                                    BoThuNhat11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 2));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 19 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 19)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 2));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 19 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 19)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(10, 19, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 19, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 2));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat22 = BoThuNhat11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhat23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem11 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat22)
                    {
                        Dem11++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem11;
                        BoThuNhat23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai22 = BoThuHai11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem22 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai22)
                    {
                        Dem22++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem22;
                        BoThuHai23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa22 = BoThuBa11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem33 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa22)
                    {
                        Dem33++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem33;
                        BoThuBa23.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat23)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai23, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa23, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở nửa trên và nửa dưới mỗi hình hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 3 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong";
                        }
                        else if (NewItem.ThuTuSapXep % 3 == 1)
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }
                        else
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }


                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }

                #endregion

                #region Phạm vi 30

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d <= 30; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(20, 30, d, 30, 4);
                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 5 == 0)
                                    {
                                        NewItem.UserControlName = "BonSoHinhVuong";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 1)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 2)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 3)
                                    {
                                        NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 4)
                                    {
                                        NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng ba số và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi2 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 3, SoCongThem, 7);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 3, SoCongThem, 7);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 3, SoCongThem, 7);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi2.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi2.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi2);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi2.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng ba số và trừ số không đổi bằng số thứ 4
                        List<BaiToanTimSoModel> DanhSachTamThoi1 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 3, SoTruBot, 7);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 3, SoTruBot, 7);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 3, SoTruBot, 7);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 20 && SoPhanTichHai - SoTruBot > 20 && SoPhanTichBa - SoTruBot > 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }
                                            DanhSachTamThoi1.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        if (DanhSachTamThoi1.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi1);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi1.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi3 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 4, SoCongThem, 2);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 4, SoCongThem, 2);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 4, SoCongThem, 2);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi3.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi3.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi3);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi3.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi4 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 4, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 4, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 4, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 20 && SoPhanTichHai - SoTruBot > 20 && SoPhanTichBa - SoTruBot > 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi4.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi4.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi4);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi4.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi5 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 5, SoCongThem, 2);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 5, SoCongThem, 2);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 5, SoCongThem, 2);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);
                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi5.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi5.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi5);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi5.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi6 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 5, SoTruBot, 3);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 5, SoTruBot, 3);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 5, SoTruBot, 3);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 20 && SoPhanTichHai - SoTruBot > 20 && SoPhanTichBa - SoTruBot > 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi6.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi6.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi6);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi6.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi7 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 6, SoCongThem);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 6, SoCongThem);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 6, SoCongThem);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi7.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi7.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi7);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi7.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi8 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 6, SoTruBot);
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 6, SoTruBot);
                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 6, SoTruBot);
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 20 && SoPhanTichHai - SoTruBot > 20 && SoPhanTichBa - SoTruBot > 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item2 in BoThuHai)
                                        {
                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + item2.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + item2.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 5 == 0)
                                            {
                                                NewItem.UserControlName = "BonSoHinhVuong";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 1)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 2)
                                            {
                                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 3)
                                            {
                                                NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                            }
                                            else if (NewItem.ThuTuSapXep % 5 == 4)
                                            {
                                                NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                            }

                                            DanhSachTamThoi8.Add(NewItem);
                                        }
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi8.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi8);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi8.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoi")
                {
                    #region Tổng hai số ở hai vị trí đối diện bằng nhau
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1, 7);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1, 7);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1, 7);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện là một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 2 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }
                            else
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiMot")
                {
                    #region Tổng hai số ở nửa phía trên và phía dưới bằng nhau
                    for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2, 7);
                        int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());

                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2, 7);
                        int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2, 7);

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở nửa trên và tổng hai số ở nửa dưới của mỗi hình là một số không đổi</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 3 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong";
                            }
                            else if (NewItem.ThuTuSapXep % 3 == 1)
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }
                            else
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiHai")
                {
                    #region Hai số đối diện hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa1 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 20; SoThuNhat <= 29; SoThuNhat++)
                    {
                        for (int SoThuHai = 20; SoThuHai <= 29; SoThuHai++)
                        {
                            for (int SoKhongDoi = 5; SoKhongDoi <= 29; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 29 && SoThuHai + SoKhongDoi <= 29)
                                {
                                    BoThuNhat1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 1));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 29 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 29)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 1));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 29 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 29)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 1));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat2 = BoThuNhat1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhatChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem1 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat2)
                    {
                        Dem1++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem1;
                        BoThuNhatChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai2 = BoThuHai1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHaiChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem2 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai2)
                    {
                        Dem2++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem2;
                        BoThuHaiChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa2 = BoThuBa1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBaChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem3 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa2)
                    {
                        Dem3++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem3;
                        BoThuBaChuanTT.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhatChuanTT)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHaiChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHaiChuanTT, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBaChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBaChuanTT, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở các vị trí đối diện hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 2 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }
                        else
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }

                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiBa")
                {
                    #region Hai số nửa phía trên và phía dưới hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai11 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa11 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 20; SoThuNhat <= 29; SoThuNhat++)
                    {
                        for (int SoThuHai = 20; SoThuHai <= 29; SoThuHai++)
                        {
                            for (int SoKhongDoi = 5; SoKhongDoi <= 29; SoKhongDoi++)
                            {
                                if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 29 && SoThuHai + SoKhongDoi <= 29)
                                {
                                    BoThuNhat11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 2));

                                    //Sinh ngẫu nhiên bộ thứ 2
                                    int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 219, SoKhongDoi.ToString().Trim());
                                    while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 29 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 29)
                                    {
                                        SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                        SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                        SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim());
                                    }
                                    BoThuHai11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 2));

                                    //Sinh ngẫu nhiên bộ thứ 3
                                    int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 29 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 29)
                                    {
                                        SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                        SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(20, 29, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                        SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(1, 29, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                    }
                                    BoThuBa11.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 2));
                                }
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat22 = BoThuNhat11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhat23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem11 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat22)
                    {
                        Dem11++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem11;
                        BoThuNhat23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai22 = BoThuHai11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem22 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai22)
                    {
                        Dem22++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem22;
                        BoThuHai23.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa22 = BoThuBa11.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa23 = new List<DanhSachBieuThucTimSoModel>();
                    int Dem33 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa22)
                    {
                        Dem33++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem33;
                        BoThuBa23.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat23)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai23, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa23.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa23, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở nửa trên và nửa dưới mỗi hình hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 3 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong";
                        }
                        else if (NewItem.ThuTuSapXep % 3 == 1)
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }
                        else
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }


                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }

                #endregion

                #region Phạm vi 30 đến 100

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMot")
                {
                    #region Cấp số cộng
                    for (int d = 1; d < 100; d++)
                    {
                        List<CapSoCongModel> CacCapSoCong = new List<CapSoCongModel>();
                        if (d < 5)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(90, 99, d, 99, 4);
                        }
                        else if (d < 10)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(70, 99, d, 99, 4);
                        }
                        else if (d < 20)
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(50, 99, d, 99, 4);
                        }
                        else
                        {
                            CacCapSoCong = ToolBaiToanTimSo.DanhSachCSC(30, 99, d, 99, 4);
                        }


                        if (CacCapSoCong.Count >= 3)
                        {
                            for (int SoThuCSC = 1; SoThuCSC <= CacCapSoCong.Count; SoThuCSC++)
                            {
                                CapSoCongModel BoThuNhat = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuCSC);
                                int SoThuTuBoThuHai = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim());
                                CapSoCongModel BoThuHai = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuHai);
                                int SoThuTuBoThuBa = AllToolShare.LayMaNgauNhien(1, CacCapSoCong.Count, SoThuCSC.ToString().Trim() + "$" + SoThuTuBoThuHai.ToString().Trim());
                                CapSoCongModel BoThuBa = ToolBaiToanTimSo.LayMotCapSoCong(CacCapSoCong, SoThuTuBoThuBa);
                                List<CapSoCongModel> CacHoanVi = ToolBaiToanTimSo.SinhHoanVi(BoThuBa);
                                foreach (CapSoCongModel Item in CacHoanVi)
                                {
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = BoThuNhat.CapSoCong + "$" + BoThuHai.CapSoCong + "$" + Item.CapSoCong;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(BoThuBa, Item);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + BoThuNhat.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + BoThuHai.LoiGiai + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + BoThuBa.LoiGiai + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 5 == 0)
                                    {
                                        NewItem.UserControlName = "BonSoHinhVuong";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 1)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 2)
                                    {
                                        NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 3)
                                    {
                                        NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                    }
                                    else if (NewItem.ThuTuSapXep % 5 == 4)
                                    {
                                        NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                    }
                                    DSBaiToanTimSo.Add(NewItem);
                                }
                            }
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Tổng ba số và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi2 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 3, SoCongThem, 7);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 3, SoCongThem, 7);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 3, SoCongThem, 7);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 3, SoCongThem, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 3, SoCongThem, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 3, SoCongThem, 10);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 3, SoCongThem, 15);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 3, SoCongThem, 15);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 3, SoCongThem, 15);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");

                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi2.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi2.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi2);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi2.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Tổng ba số và trừ số không đổi bằng số thứ 4
                        List<BaiToanTimSoModel> DanhSachTamThoi1 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 40)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 40, 3, SoTruBot, 5);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 40, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 40, 3, SoTruBot, 5);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 40, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 40, 3, SoTruBot, 5);
                                }
                                else if (SoPhanTich < 60)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 60, 3, SoTruBot, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(40, 60, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 60, 3, SoTruBot, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(40, 60, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 60, 3, SoTruBot, 10);
                                }
                                else if (SoPhanTich < 80)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 80, 3, SoTruBot, 15);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(60, 80, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 80, 3, SoTruBot, 15);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(60, 80, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 80, 3, SoTruBot, 15);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 3, SoTruBot, 20);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(80, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 3, SoTruBot, 20);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(80, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 3, SoTruBot, 20);
                                }
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 30 && SoPhanTichHai - SoTruBot > 30 && SoPhanTichBa - SoTruBot > 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }
                                        DanhSachTamThoi1.Add(NewItem);
                                    }
                                }
                            }
                        }
                        if (DanhSachTamThoi1.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi1);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi1.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Tổng hai số trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi3 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 4, SoCongThem, 7);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 4, SoCongThem, 7);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 4, SoCongThem, 7);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 4, SoCongThem, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 4, SoCongThem, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 4, SoCongThem, 10);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 4, SoCongThem, 15);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 4, SoCongThem, 15);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 4, SoCongThem, 15);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi3.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi3.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi3);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi3.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Tổng hai số trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi4 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 40)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 40, 4, SoTruBot, 4);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 40, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 40, 4, SoTruBot, 4);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 40, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 40, 4, SoTruBot, 4);
                                }
                                else if (SoPhanTich < 60)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 60, 4, SoTruBot, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(40, 60, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 60, 4, SoTruBot, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(40, 60, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 60, 4, SoTruBot, 10);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 4, SoTruBot, 15);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(60, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 4, SoTruBot, 15);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(60, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 4, SoTruBot, 15);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 30 && SoPhanTichHai - SoTruBot > 30 && SoPhanTichBa - SoTruBot > 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi4.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi4.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi4);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi4.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi5 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 5, SoCongThem, 7);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 5, SoCongThem, 7);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 5, SoCongThem, 7);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 5, SoCongThem, 12);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 5, SoCongThem, 12);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 5, SoCongThem, 12);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 5, SoCongThem, 15);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 5, SoCongThem, 15);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 5, SoCongThem, 15);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi5.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi5.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi5);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi5.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Số thứ nhất trừ số thứ 2 cộng số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi6 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 50, 5, SoTruBot, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 50, 5, SoTruBot, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 50, 5, SoTruBot, 10);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 5, SoTruBot, 13);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 5, SoTruBot, 13);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 5, SoTruBot, 13);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 5, SoTruBot, 16);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 5, SoTruBot, 16);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 5, SoTruBot, 16);
                                }
                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 30 && SoPhanTichHai - SoTruBot > 30 && SoPhanTichBa - SoTruBot > 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }
                                        DanhSachTamThoi6.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi6.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi6);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi6.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và cộng thêm số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi7 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 6, SoCongThem, 4);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 6, SoCongThem, 4);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 6, SoCongThem, 4);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 6, SoCongThem, 7);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 6, SoCongThem, 7);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 6, SoCongThem, 7);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 6, SoCongThem, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 6, SoCongThem, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 6, SoCongThem, 10);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi7.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi7.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi7);
                        }
                        else
                        {
                            //Lấy 300 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi7.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    while (DSPhamVi.Count < 300)
                    {
                        if (DSPhamVi.Count < 300)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }
                        #region Số thứ nhất trừ số thứ 2 và trừ số thứ 3 và trừ bớt số không đổi bằng số thứ 4

                        List<BaiToanTimSoModel> DanhSachTamThoi8 = new List<BaiToanTimSoModel>();
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                if (SoPhanTich < 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 50, 6, SoTruBot, 4);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 50, 6, SoTruBot, 4);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 50, 6, SoTruBot, 4);
                                }
                                else if (SoPhanTich < 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 6, SoTruBot, 7);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 6, SoTruBot, 7);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 6, SoTruBot, 7);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 6, SoTruBot, 10);
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 6, SoTruBot, 10);
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 6, SoTruBot, 10);
                                }

                                if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 30 && SoPhanTichHai - SoTruBot > 30 && SoPhanTichBa - SoTruBot > 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(MotBoThuBa);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = item1.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(MotBoThuBa, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 5 == 0)
                                        {
                                            NewItem.UserControlName = "BonSoHinhVuong";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 1)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 2)
                                        {
                                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 3)
                                        {
                                            NewItem.UserControlName = "TamGiacHinhTronBonSo";
                                        }
                                        else if (NewItem.ThuTuSapXep % 5 == 4)
                                        {
                                            NewItem.UserControlName = "TamGiacNoiTiepBonSo";
                                        }

                                        DanhSachTamThoi8.Add(NewItem);
                                    }

                                }
                            }
                        }
                        if (DanhSachTamThoi8.Count < 300)
                        {
                            DSPhamVi.AddRange(DanhSachTamThoi8);
                        }
                        else
                        {
                            //Lấy 1000 bản ghi
                            List<BaiToanTimSoModel> DSAllSelect = DanhSachTamThoi8.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                            List<BaiToanTimSoModel> DSSelect = DSAllSelect.Skip(0).Take(300).ToList<BaiToanTimSoModel>(); ;
                            DSPhamVi.AddRange(DSSelect);
                        }
                        #endregion
                    }
                    DSBaiToanTimSo.AddRange(DSPhamVi);
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoi")
                {
                    #region Tổng hai số ở hai vị trí đối diện bằng nhau
                    for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                        List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                        List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                        if (SoPhanTich < 50)
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1, 10);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1, 10);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1, 10);
                        }
                        else if (SoPhanTich < 70)
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1, 15);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1, 15);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1, 15);
                        }
                        else
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 1, 20);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 1, 20);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 1, 20);
                        }
                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện là một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 2 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }
                            else
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiMot")
                {
                    #region Tổng hai số ở nửa phía trên và phía dưới bằng nhau
                    for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                    {
                        List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                        List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                        List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                        if (SoPhanTich < 50)
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2, 10);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2, 10);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2, 10);
                        }
                        else if (SoPhanTich < 70)
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2, 15);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2, 15);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2, 15);
                        }
                        else
                        {
                            BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTich, 2, 20);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                            BoThuHai = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichHai, 2, 20);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            BoThuBa = ToolBaiToanTimSo.PhanTichMotSoThanhTongHieuHaiSo(SoPhanTichBa, 2, 20);
                        }

                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                        {
                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                            NewItem.MaCauHoi = Guid.NewGuid();
                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở nửa trên và tổng hai số ở nửa dưới của mỗi hình là một số không đổi</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (NewItem.ThuTuSapXep % 3 == 0)
                            {
                                NewItem.UserControlName = "BonSoHinhVuong";
                            }
                            else if (NewItem.ThuTuSapXep % 3 == 1)
                            {
                                NewItem.UserControlName = "HinhTronBonSoChiaTu";
                            }
                            else
                            {
                                NewItem.UserControlName = "BonSoHinhVuong1";
                            }

                            DSBaiToanTimSo.Add(NewItem);
                        }
                    }
                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiHai")
                {
                    #region Hai số đối diện hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa1 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 30; SoThuNhat <= 99; SoThuNhat++)
                    {
                        int SoThuHai = SoThuNhat + rd.Next(1, 99 - SoThuNhat + 1);
                        int SoKhongDoi_start = 0;
                        int SoKhongDoi_End = 0;
                        int Start = 0; int End = 0;
                        if (SoThuNhat < 50)
                        {
                            SoKhongDoi_start = 10;
                            SoKhongDoi_End = 50;
                            Start = 30; End = 50;
                        }
                        else if (SoThuNhat < 70)
                        {
                            SoKhongDoi_start = 15;
                            SoKhongDoi_End = 55;
                            Start = 50; End = 70;
                        }
                        else
                        {
                            SoKhongDoi_start = 20;
                            SoKhongDoi_End = 99;
                            Start = 70; End = 99;
                        }
                        for (int SoKhongDoi = SoKhongDoi_start; SoKhongDoi <= SoKhongDoi_End; SoKhongDoi++)
                        {
                            if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 99 && SoThuHai + SoKhongDoi <= 99)
                            {
                                BoThuNhat1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 1));

                                //Sinh ngẫu nhiên bộ thứ 2
                                int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim());
                                while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 99 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 99)
                                {
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim());
                                }
                                BoThuHai1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 1));

                                //Sinh ngẫu nhiên bộ thứ 3
                                int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 99 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 99)
                                {
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                }
                                BoThuBa1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 1));
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat2 = BoThuNhat1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhatChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem1 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat2)
                    {
                        Dem1++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem1;
                        BoThuNhatChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai2 = BoThuHai1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHaiChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem2 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai2)
                    {
                        Dem2++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem2;
                        BoThuHaiChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa2 = BoThuBa1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBaChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem3 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa2)
                    {
                        Dem3++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem3;
                        BoThuBaChuanTT.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhatChuanTT)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHaiChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHaiChuanTT, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBaChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBaChuanTT, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở các vị trí đối diện hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 2 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }
                        else
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }

                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBonSoDangMuoiBa")
                {
                    #region Hai số nửa phía trên và phía dưới hơn kém nhau một số không đổi
                    List<DanhSachBieuThucTimSoModel> BoThuNhat1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHai1 = new List<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBa1 = new List<DanhSachBieuThucTimSoModel>();
                    for (int SoThuNhat = 30; SoThuNhat <= 99; SoThuNhat++)
                    {
                        int SoThuHai = SoThuNhat + rd.Next(1, 99 - SoThuNhat + 1);
                        int SoKhongDoi_start = 0;
                        int SoKhongDoi_End = 0;
                        int Start = 0; int End = 0;
                        if (SoThuNhat < 50)
                        {
                            SoKhongDoi_start = 10;
                            SoKhongDoi_End = 50;
                            Start = 30; End = 50;
                        }
                        else if (SoThuNhat < 70)
                        {
                            SoKhongDoi_start = 15;
                            SoKhongDoi_End = 55;
                            Start = 50; End = 70;
                        }
                        else
                        {
                            SoKhongDoi_start = 20;
                            SoKhongDoi_End = 99;
                            Start = 70; End = 99;
                        }
                        for (int SoKhongDoi = SoKhongDoi_start; SoKhongDoi <= SoKhongDoi_End; SoKhongDoi++)
                        {
                            if (SoThuNhat != SoThuHai && SoThuNhat + SoKhongDoi <= 99 && SoThuHai + SoKhongDoi <= 99)
                            {
                                BoThuNhat1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat, SoThuHai, SoKhongDoi, 2));

                                //Sinh ngẫu nhiên bộ thứ 2
                                int SoThuNhat_BoThuHai = 0; int SoThuHai_BoThuHai = 0; int SoKhongDoi_BoThuHai = 0;
                                SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim());
                                while (SoThuNhat_BoThuHai + SoKhongDoi_BoThuHai > 99 || SoThuHai_BoThuHai + SoKhongDoi_BoThuHai > 99)
                                {
                                    SoThuNhat_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim());
                                    SoThuHai_BoThuHai = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString().Trim());
                                    SoKhongDoi_BoThuHai = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim());
                                }
                                BoThuHai1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuHai, SoThuHai_BoThuHai, SoKhongDoi_BoThuHai, 2));

                                //Sinh ngẫu nhiên bộ thứ 3
                                int SoThuNhat_BoThuBa = 0; int SoThuHai_BoThuBa = 0; int SoKhongDoi_BoThuBa = 0;
                                SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                while (SoThuNhat_BoThuBa + SoKhongDoi_BoThuBa > 99 || SoThuHai_BoThuBa + SoKhongDoi_BoThuBa > 99)
                                {
                                    SoThuNhat_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString());
                                    SoThuHai_BoThuBa = AllToolShare.LayMaNgauNhien(Start, End, SoThuNhat.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuNhat_BoThuHai.ToString() + "$" + SoThuHai_BoThuHai.ToString() + "$" + SoThuNhat_BoThuBa.ToString());
                                    SoKhongDoi_BoThuBa = AllToolShare.LayMaNgauNhien(SoKhongDoi_start, SoKhongDoi_End, SoKhongDoi.ToString().Trim() + "$" + SoKhongDoi_BoThuHai.ToString().Trim());
                                }
                                BoThuBa1.Add(ToolBaiToanTimSo.PhanTichHaiSo(SoThuNhat_BoThuBa, SoThuHai_BoThuBa, SoKhongDoi_BoThuBa, 2));
                            }
                        }
                    }
                    List<DanhSachBieuThucTimSoModel> BoThuNhat2 = BoThuNhat1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuNhatChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem1 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuNhat2)
                    {
                        Dem1++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem1;
                        BoThuNhatChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuHai2 = BoThuHai1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuHaiChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem2 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuHai2)
                    {
                        Dem2++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem2;
                        BoThuHaiChuanTT.Add(NewItem);
                    }

                    List<DanhSachBieuThucTimSoModel> BoThuBa2 = BoThuBa1.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
                    List<DanhSachBieuThucTimSoModel> BoThuBaChuanTT = new List<DanhSachBieuThucTimSoModel>();
                    int Dem3 = 0;
                    foreach (DanhSachBieuThucTimSoModel item in BoThuBa2)
                    {
                        Dem3++;
                        DanhSachBieuThucTimSoModel NewItem = item; NewItem.ThuTuSapXep = Dem3;
                        BoThuBaChuanTT.Add(NewItem);
                    }

                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhatChuanTT)
                    {
                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHaiChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHaiChuanTT, STTBoThuHai);

                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBaChuanTT.Count, "");
                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBaChuanTT, STTBoThuBa);

                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                        NewItem.MaCauHoi = Guid.NewGuid();
                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                        NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Các số ở nửa trên và nửa dưới mỗi hình hơn kém nhau một số không đổi</b></i>." +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                      "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";

                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (NewItem.ThuTuSapXep % 2 == 0)
                        {
                            NewItem.UserControlName = "BonSoHinhVuong1";
                        }
                        else
                        {
                            NewItem.UserControlName = "HinhTronBonSoChiaTu";
                        }

                        DSBaiToanTimSo.Add(NewItem);
                    }

                    #endregion
                }

                #endregion

                #endregion

                #region Bài toán năm số

                #region Phạm vi 10
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 200;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và cộng thêm số không đổi bằng số thứ 5

                        for (int SoPhanTich = 4; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                if (9 - SoCongThem > 5)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(4, 9 - SoCongThem, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(4, 9 - SoCongThem, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 9)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 7, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 7, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 7, SoCongThem);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và trừ bớt số không đổi bằng số thứ 5

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 7, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim());
                                BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 7, SoTruBot);
                                while (BoThuHai.Count == 0)
                                {
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 7, SoTruBot);
                                }

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 7, SoTruBot);
                                while (BoThuBa.Count == 0)
                                {
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 7, SoTruBot);
                                }

                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "ElipNamSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongNamSo";
                                    }
                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E+k

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                if (9 - SoCongThem > 3)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 - SoCongThem, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 - SoCongThem, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 9)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 8, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 8, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 8, SoCongThem);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E-k

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 8, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 8, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 8, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E+k

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                if (9 - SoCongThem > 3)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 - SoCongThem, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 - SoCongThem, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 9)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 9, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 9, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 9, SoCongThem);

                                        if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E-k

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 9, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 9, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 9, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 200;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E+k

                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 9; SoCongThem++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 9, 10, SoCongThem);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 9, 10, SoCongThem);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 9, 10, SoCongThem);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 9 && SoPhanTichHai + SoCongThem <= 9 && SoPhanTichBa + SoCongThem <= 9)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 70;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E-k

                        for (int SoTruBot = 1; SoTruBot <= 9; SoTruBot++)
                        {
                            for (int SoPhanTich = 1; SoPhanTich <= 9 + SoTruBot; SoPhanTich++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 9, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 9, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 9, 10, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot > 0 && SoPhanTichHai - SoTruBot > 0 && SoPhanTichBa - SoTruBot > 0)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 13;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 4; SoPhanTich <= 9; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 1);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(4, 9, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 1);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(4, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 1);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {
                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "ElipNamSo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "HinhVuongNamSo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMuoi")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 33;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 1; SoPhanTich <= 9; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 2);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(1, 9, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 2);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(1, 9, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 2);

                            if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "ElipNamSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongNamSo";
                                    }

                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }
                #endregion

                #region Phạm vi 20
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và cộng thêm số không đổi bằng số thứ 5

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                if (19 - SoCongThem > 10)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 7, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 7, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 7, SoCongThem);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và trừ bớt số không đổi bằng số thứ 5

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 7, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim());
                                BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 7, SoTruBot);
                                while (BoThuHai.Count == 0)
                                {
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 7, SoTruBot);
                                }

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 7, SoTruBot);
                                while (BoThuBa.Count == 0)
                                {
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 7, SoTruBot);
                                }
                                if (SoPhanTich - SoTruBot >= 10 && SoPhanTichHai - SoTruBot >= 10 && SoPhanTichBa - SoTruBot >= 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E+k

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                if (19 - SoCongThem >= 10)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 8, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 8, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 8, SoCongThem);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E-k

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 8, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 8, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 8, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 10 && SoPhanTichHai - SoTruBot >= 10 && SoPhanTichBa - SoTruBot >= 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E+k

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                if (19 - SoCongThem >= 10)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 9, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 9, SoCongThem);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 9, SoCongThem);

                                        if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E-k

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 9, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 9, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 9, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 10 && SoPhanTichHai - SoTruBot >= 10 && SoPhanTichBa - SoTruBot >= 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 200;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E+k

                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 19; SoCongThem++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 19, 10, SoCongThem);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 19, 10, SoCongThem);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 19, 10, SoCongThem);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 19 && SoPhanTichHai + SoCongThem <= 19 && SoPhanTichBa + SoCongThem <= 19)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 70;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E-k

                        for (int SoTruBot = 1; SoTruBot <= 19; SoTruBot++)
                        {
                            for (int SoPhanTich = 10; SoPhanTich <= 19 + SoTruBot; SoPhanTich++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 19, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 19, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 19, 10, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 10 && SoPhanTichHai - SoTruBot >= 10 && SoPhanTichBa - SoTruBot >= 10)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 13;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 1);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 1);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 1);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {
                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "ElipNamSo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "HinhVuongNamSo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMuoi")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 33;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 2, 1, 19);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 2, 1, 19);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(10, 19, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 2, 1, 19);

                            if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "ElipNamSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongNamSo";
                                    }

                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }
                #endregion

                #region Phạm vi 30
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và cộng thêm số không đổi bằng số thứ 5

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                if (29 - SoCongThem >= 20)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 7, SoCongThem, 4);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 7, SoCongThem, 4);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 7, SoCongThem, 4);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và trừ bớt số không đổi bằng số thứ 5

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 7, SoTruBot, 4);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim());
                                BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 7, SoTruBot, 4);
                                while (BoThuHai.Count == 0)
                                {
                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 7, SoTruBot, 4);
                                }

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 7, SoTruBot, 4);
                                while (BoThuBa.Count == 0)
                                {
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 7, SoTruBot, 4);
                                }
                                if (SoPhanTich - SoTruBot >= 20 && SoPhanTichHai - SoTruBot >= 20 && SoPhanTichBa - SoTruBot >= 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E+k

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                if (29 - SoCongThem >= 20)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 8, SoCongThem, 5);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 8, SoCongThem, 5);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 8, SoCongThem, 5);

                                        if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E-k

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 8, SoTruBot, 5);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 8, SoTruBot, 5);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 8, SoTruBot, 5);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 20 && SoPhanTichHai - SoTruBot >= 20 && SoPhanTichBa - SoTruBot >= 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E+k

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                if (29 - SoCongThem >= 20)
                                {
                                    int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                                    int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                    if (SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                    {
                                        List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 9, SoCongThem, 7);

                                        List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 9, SoCongThem, 7);

                                        List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 9, SoCongThem, 7);

                                        if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                        {
                                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                            {
                                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                                NewItem.MaCauHoi = Guid.NewGuid();
                                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                                NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                                if (NewItem.ThuTuSapXep % 2 == 0)
                                                {
                                                    NewItem.UserControlName = "ElipNamSo";
                                                }
                                                else
                                                {
                                                    NewItem.UserControlName = "HinhVuongNamSo";
                                                }
                                                DSPhamVi.Add(NewItem);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 300;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E-k

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 9, SoTruBot, 5);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 9, SoTruBot, 5);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 9, SoTruBot, 5);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 20 && SoPhanTichHai - SoTruBot >= 20 && SoPhanTichBa - SoTruBot >= 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 200;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E+k

                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 29; SoCongThem++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 29, 10, SoCongThem, 2);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 29, 10, SoCongThem, 2);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 29, 10, SoCongThem, 2);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 29 && SoPhanTichHai + SoCongThem <= 29 && SoPhanTichBa + SoCongThem <= 29)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 70;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E-k

                        for (int SoTruBot = 1; SoTruBot <= 29; SoTruBot++)
                        {
                            for (int SoPhanTich = 20; SoPhanTich <= 29 + SoTruBot; SoPhanTich++)
                            {
                                int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim());

                                int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29 + SoTruBot, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());

                                List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 29, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 29, 10, SoTruBot);

                                List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 29, 10, SoTruBot);

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 20 && SoPhanTichHai - SoTruBot >= 20 && SoPhanTichBa - SoTruBot >= 20)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 13;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 1);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 1);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 1);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {
                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "ElipNamSo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "HinhVuongNamSo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMuoi")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 42;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 2, 1, 29);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 2, 1, 29);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(20, 29, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 2, 1, 29);

                            if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "ElipNamSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongNamSo";
                                    }

                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }
                #endregion

                #region Phạm vi 30
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và cộng thêm số không đổi bằng số thứ 5

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                if (99 - SoCongThem >= 30)
                                {
                                    
                                    int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                    List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                    if (SoPhanTich <= 50)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 7, SoCongThem, 10);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 7, SoCongThem, 10);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 7, SoCongThem, 10);
                                    }
                                    else if (SoPhanTich <= 70)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 7, SoCongThem, 15);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 7, SoCongThem, 15);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 7, SoCongThem, 15);
                                    }
                                    else
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 7, SoCongThem, 20);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 7, SoCongThem, 20);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 7, SoCongThem, 20);
                                    }

                                    if (BoThuHai.Count > 0 && BoThuBa.Count > 0)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                        {
                                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                            if (SoCongThem == 0)
                                            {
                                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Số ở giữa bằng tổng tất cả các số còn lại</b></i>.<br/>" +
                                                                          "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            }
                                            else
                                            {
                                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Số ở giữa bằng tổng các số còn lại và cộng thêm số " + SoCongThem.ToString().Trim() + "</b></i>.<br/>" +
                                                                          "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            }
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "ElipNamSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongNamSo";
                                            }
                                            DSPhamVi.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }
                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng bốn số và trừ bớt số không đổi bằng số thứ 5

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>(); 

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;

                                if (SoPhanTich <= 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 50, 7, SoTruBot, 10);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 50, 7, SoTruBot, 10);
                                   
                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 50, 7, SoTruBot, 10);
                                }
                                else if (SoPhanTich <= 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 7, SoTruBot, 15);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 7, SoTruBot, 15);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 7, SoTruBot, 15);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 7, SoTruBot, 20);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 7, SoTruBot, 20);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 7, SoTruBot, 20);
                                }

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 30 && SoPhanTichHai - SoTruBot >= 30 && SoPhanTichBa - SoTruBot >= 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        if (SoTruBot == 0)
                                        {
                                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Số ở giữa bằng tổng tất cả các số còn lại</b></i>.<br/>" +
                                                                      "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        }
                                        else
                                        {
                                            NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Số ở giữa bằng tổng các số còn lại và trừ đi số " + SoTruBot.ToString().Trim() + "</b></i>.<br/>" +
                                                                      "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        }
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBa")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E+k

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                if (99 - SoCongThem >= 30)
                                {
                                    int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                    List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                    if (SoPhanTich <= 50)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 8, SoCongThem, 10);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 8, SoCongThem, 10);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 8, SoCongThem, 10);
                                    }
                                    else if (SoPhanTich <= 70)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 8, SoCongThem, 15);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 8, SoCongThem, 15);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 8, SoCongThem, 15);
                                    }
                                    else
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 8, SoCongThem, 20);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 8, SoCongThem, 20);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 8, SoCongThem, 20);
                                    }


                                    if (BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                        {
                                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "ElipNamSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongNamSo";
                                            }
                                            DSPhamVi.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBon")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C+D-E-k

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;

                                if (SoPhanTich <= 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 50, 8, SoTruBot, 10);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 50, 8, SoTruBot, 10);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 50, 8, SoTruBot, 10);
                                }
                                else if (SoPhanTich <= 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 8, SoTruBot, 15);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 8, SoTruBot, 15);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 8, SoTruBot, 15);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 8, SoTruBot, 20);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 8, SoTruBot, 20);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 8, SoTruBot, 20);
                                }

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 30 && SoPhanTichHai - SoTruBot >= 30 && SoPhanTichBa - SoTruBot >= 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangNam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E+k

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                if (99 - SoCongThem >= 30)
                                {
                                    int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                    List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                    if (SoPhanTich <= 50)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 50, 9, SoCongThem, 10);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 50, 9, SoCongThem, 10);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 50, 9, SoCongThem, 10);
                                    }
                                    else if (SoPhanTich <= 70)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 9, SoCongThem, 15);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 9, SoCongThem, 15);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 9, SoCongThem, 15);
                                    }
                                    else
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 9, SoCongThem, 20);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 9, SoCongThem, 20);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 9, SoCongThem, 20);
                                    }

                                    if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                        {
                                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "ElipNamSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongNamSo";
                                            }
                                            DSPhamVi.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangSau")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B+C-D-E-k

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;

                                if (SoPhanTich <= 50)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 50, 9, SoTruBot, 10);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 50, 9, SoTruBot, 10);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 50, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 50, 9, SoTruBot, 10);
                                }
                                else if (SoPhanTich <= 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 9, SoTruBot, 15);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 9, SoTruBot, 15);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(50, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 9, SoTruBot, 15);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 9, SoTruBot, 20);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 9, SoTruBot, 20);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 9, SoTruBot, 20);
                                }

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 30 && SoPhanTichHai - SoTruBot >= 30 && SoPhanTichBa - SoTruBot >= 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }

                    //Lấy sl bản ghi
                    List<BaiToanTimSoModel> DSAllSelect = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                    DSBaiToanTimSo = DSAllSelect.Skip(0).Take(sl).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangBay")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 200;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E+k

                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            for (int SoCongThem = 0; SoCongThem <= 99; SoCongThem++)
                            {
                                if (99 - SoCongThem >= 30)
                                {
                                    int SoPhanTichHai = 0; int SoPhanTichBa = 0;
                                    List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();
                                    List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();
                                    if (SoPhanTich <= 70)
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 70, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 70, 10, SoCongThem, 10);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 70, 10, SoCongThem, 10);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 70, 10, SoCongThem, 10);
                                    }
                                    else
                                    {
                                        SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                        SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                        BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTich, 99, 10, SoCongThem, 12);
                                        BoThuHai = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichHai, 99, 10, SoCongThem, 12);
                                        BoThuBa = ToolBaiToanTimSo.PhanTichMotSoCongThem(SoPhanTichBa, 99, 10, SoCongThem, 12);
                                    }

                                    if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich + SoCongThem <= 99 && SoPhanTichHai + SoCongThem <= 99 && SoPhanTichBa + SoCongThem <= 99)
                                    {
                                        foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                        {
                                            int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                            int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                            DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                            List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                            int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                            DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                            BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                            NewItem.MaCauHoi = Guid.NewGuid();
                                            NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                            NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                            NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                      "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                            NewItem.PhamViPhepToan = PhamViPhepToan;
                                            NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                            NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                            NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                            if (NewItem.ThuTuSapXep % 2 == 0)
                                            {
                                                NewItem.UserControlName = "ElipNamSo";
                                            }
                                            else
                                            {
                                                NewItem.UserControlName = "HinhVuongNamSo";
                                            }
                                            DSPhamVi.Add(NewItem);
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangTam")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 70;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Dạng A=B-C-D-E-k

                        for (int SoTruBot = 1; SoTruBot <= 99; SoTruBot++)
                        {
                            for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                            {
                                List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuHai = new List<DanhSachBieuThucTimSoModel>();

                                List<DanhSachBieuThucTimSoModel> BoThuBa = new List<DanhSachBieuThucTimSoModel>();

                                int SoPhanTichHai = 0; int SoPhanTichBa = 0;

                                if (SoPhanTich <= 70)
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 70, 10, SoTruBot, 8);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 70, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 70, 10, SoTruBot, 8);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 70, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 70, 10, SoTruBot, 8);
                                }
                                else
                                {
                                    BoThuNhat = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTich, 99, 10, SoTruBot, 10);

                                    SoPhanTichHai = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim());
                                    BoThuHai = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichHai, 99, 10, SoTruBot, 10);

                                    SoPhanTichBa = AllToolShare.LayMaNgauNhien(70, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                                    BoThuBa = ToolBaiToanTimSo.PhanTichMotSoTruBot(SoPhanTichBa, 99, 10, SoTruBot, 10);
                                }

                                if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0 && SoPhanTich - SoTruBot >= 30 && SoPhanTichHai - SoTruBot >= 30 && SoPhanTichBa - SoTruBot >= 30)
                                {
                                    foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                    {
                                        int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                        int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                        DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                        List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                        int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                        DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                        BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                        NewItem.MaCauHoi = Guid.NewGuid();
                                        NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                        NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                        NewItem.LoiGiaiBaiToan = "<b> Trong hình thứ nhất: </b>" + MotBoThuBa.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ hai: </b>" + MotBoThuHai.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Trong hình thứ ba: </b>" + item1.LoiGiaiBaiToan + "<br/>" +
                                                                  "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                        NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                        NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                        NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                        NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                        if (NewItem.ThuTuSapXep % 2 == 0)
                                        {
                                            NewItem.UserControlName = "ElipNamSo";
                                        }
                                        else
                                        {
                                            NewItem.UserControlName = "HinhVuongNamSo";
                                        }
                                        DSPhamVi.Add(NewItem);
                                    }
                                }
                            }
                        }

                        #endregion

                    }
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangChin")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 1400;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 1, 13);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 1, 13);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 1, 13);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {
                                int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "ElipNamSo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "HinhVuongNamSo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanNamSoDangMuoi")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 400;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTich, 2, 13, 99);
                            int SoPhanTichHai = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuHai = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichHai, 2, 13, 99);
                            int SoPhanTichBa = AllToolShare.LayMaNgauNhien(30, 99, SoPhanTich.ToString().Trim() + "$" + SoPhanTichHai.ToString().Trim());
                            List<DanhSachBieuThucTimSoModel> BoThuBa = ToolBaiToanTimSo.PhanTichTongHieu(SoPhanTichBa, 2, 13, 99);

                            if (BoThuNhat.Count > 0 && BoThuHai.Count > 0 && BoThuBa.Count > 0)
                            {
                                foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                                {
                                    int STTBoThuHai = AllToolShare.LayMaNgauNhien(1, BoThuHai.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuHai = ToolBaiToanTimSo.LayMotHoanVi(BoThuHai, STTBoThuHai);

                                    int STTBoThuBa = AllToolShare.LayMaNgauNhien(1, BoThuBa.Count, "");
                                    DanhSachBieuThucTimSoModel MotBoThuBa = ToolBaiToanTimSo.LayMotHoanVi(BoThuBa, STTBoThuBa);

                                    List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                    int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                    DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                    BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                    NewItem.MaCauHoi = Guid.NewGuid();
                                    NewItem.ChuoiSoHienThi = MotBoThuBa.BieuThuc + "$" + MotBoThuHai.BieuThuc + "$" + MotHoanVi.BieuThuc;
                                    NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                    NewItem.LoiGiaiBaiToan = "Ta thấy trong các hình trên <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>." +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;<br/><i><b>Thật vậy:</b></i><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ nhất: <b>" + MotBoThuBa.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ hai:&#160;&#160; <b>" + MotBoThuHai.LoiGiaiBaiToan + "</b><br/>" +
                                                              "&#160;&#160;&#160;&#160;&#160;&#160;+ Trong hình thứ ba:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                              "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                    NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                    NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                    NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                    NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (NewItem.ThuTuSapXep % 2 == 0)
                                    {
                                        NewItem.UserControlName = "ElipNamSo";
                                    }
                                    else
                                    {
                                        NewItem.UserControlName = "HinhVuongNamSo";
                                    }

                                    DSPhamVi.Add(NewItem);
                                }
                            }
                        }
                        #endregion

                    }

                    DSBaiToanTimSo.AddRange(DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>());
                }
                #endregion


                #endregion

                #region Bài toán bảy số

                #region Phạm vi 10
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 1;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 6; SoPhanTich <= 10; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 33;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Hiệu hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 1; SoPhanTich <= 10; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 2);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }
                #endregion

                #region Phạm vi 20
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 70;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 33;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Hiệu hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 10; SoPhanTich <= 19; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 2, 2, 19);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }
                #endregion

                #region Phạm vi 30
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 80;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {

                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1, 5);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 39;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Hiệu hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 20; SoPhanTich <= 29; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 2, 1, 29);

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }
                #endregion

                #region Phạm vi 30 đến 100
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangMot")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 1220;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Tổng hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat = new List<DanhSachBieuThucTimSoModel>();
                            if (SoPhanTich <= 50)
                            {
                                BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1, 10);
                            }
                            else if (SoPhanTich <= 70)
                            {
                                BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1, 15);
                            }
                            else
                            {
                                BoThuNhat = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 1, 20);
                            }

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Tổng hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }

                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30Den100" && PhanLoaiBaiToan.Trim() == "BaiToanBaySoDangHai")
                {
                    List<BaiToanTimSoModel> DSPhamVi = new List<BaiToanTimSoModel>();
                    int sl = 1500;
                    while (DSPhamVi.Count < sl)
                    {
                        if (DSPhamVi.Count < sl)
                        {
                            DSPhamVi.RemoveRange(0, DSPhamVi.Count);
                        }

                        #region Hiệu hai số ở hai vị trí đối diện bằng số ở vị trí giữa
                        for (int SoPhanTich = 30; SoPhanTich <= 99; SoPhanTich++)
                        {
                            List<DanhSachBieuThucTimSoModel> BoThuNhat  = ToolBaiToanTimSo.PhanTichTongHieuBaCap(SoPhanTich, 2, 10, 99);
                            

                            foreach (DanhSachBieuThucTimSoModel item1 in BoThuNhat)
                            {

                                List<DanhSachBieuThucTimSoModel> DSHoanVi = ToolBaiToanTimSo.SinhHoanVi(item1);
                                int STTHoanVi = AllToolShare.LayMaNgauNhien(1, DSHoanVi.Count, "");
                                DanhSachBieuThucTimSoModel MotHoanVi = ToolBaiToanTimSo.LayMotHoanVi(DSHoanVi, STTHoanVi);
                                BaiToanTimSoModel NewItem = new BaiToanTimSoModel();
                                NewItem.MaCauHoi = Guid.NewGuid();
                                NewItem.ChuoiSoHienThi = MotHoanVi.BieuThuc;
                                NewItem.DapAn = ToolBaiToanTimSo.LayDapAn(item1, MotHoanVi);
                                NewItem.LoiGiaiBaiToan = "Trong hình trên ta thấy <i><b>Hiệu hai số ở hai vị trí đối diện bằng số ở giữa</b></i>.<br/>" +
                                                          "&#160;&#160;&#160;&#160;&#160;&#160;Thật vậy:&#160;&#160; <b>" + item1.LoiGiaiBaiToan + "</b><br/>" +
                                                          "<b> Đáp án:</b><i> x = " + NewItem.DapAn + "</i>";
                                NewItem.PhamViPhepToan = PhamViPhepToan; ;
                                NewItem.PhanLoaiBaiToan = PhanLoaiBaiToan;
                                NewItem.ThuocKhoiLop = ThuocKhoiLop;
                                NewItem.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (NewItem.ThuTuSapXep % 2 == 0)
                                {
                                    NewItem.UserControlName = "VongTronLucGiacBaySo";
                                }
                                else
                                {
                                    NewItem.UserControlName = "LucGiacBaySo";
                                }

                                DSPhamVi.Add(NewItem);
                            }
                        }
                        #endregion

                    }

                    //Lấy sl bản ghi
                    DSBaiToanTimSo = DSPhamVi.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();
                }
                #endregion

                #endregion

                #endregion

                List<BaiToanTimSoModel> BaiToanTimSoMoi = DSBaiToanTimSo.OrderBy(m => m.ThuTuSapXep).ToList<BaiToanTimSoModel>();

                //Lưu các bài toán trên vào bảng
                foreach (BaiToanTimSoModel item in BaiToanTimSoMoi)
                {
                    ToolBaiToanTimSo.ThemMoiMotBaiToanTimSo(item);
                }

                return RedirectToAction("DanhSachBaiToanTimSo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + PhanLoaiBaiToan, "BaiToanTimSo");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Xóa tất cả các bài toán tìm số
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacBaiToanTimSo()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string PhanLoaiBaiToan = Request.Form["PhanLoaiBaiToan"];

                if (String.IsNullOrEmpty(ToolBaiToanTimSo.XoaNhieuBaiToanTimSo(ThuocKhoiLop, PhamViPhepToan, PhanLoaiBaiToan)))
                {
                    return RedirectToAction("DanhSachBaiToanTimSo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + PhanLoaiBaiToan, "BaiToanTimSo");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachDaySo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + PhanLoaiBaiToan, "Home");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
    }
}