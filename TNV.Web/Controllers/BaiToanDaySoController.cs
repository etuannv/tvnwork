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
    public class BaiToanDaySoController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public SystemManagerService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }
        public BaiToanDaySoService ToolBaiToanDaySo { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new SystemManagerClass(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolSystemManager == null) { ToolSystemManager = new SystemManagerClass(); }
            if (ToolBaiToanDaySo == null) { ToolBaiToanDaySo = new BaiToanDaySoClass(); }
            
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
        public ActionResult DanhSachDaySo(string memvar1, string memvar2, string memvar3)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["ThuocKhoiLop"] = memvar1;
                ViewData["PhamViPhepToan"] = memvar2;
                ViewData["PhanLoaiDaySo"] = memvar3;
                //Đọc danh sách các dãy số
                List<BaiToanDaySoModel> DanhSachDaySo = ToolBaiToanDaySo.DanhSachDaySo(memvar1, memvar2, memvar3);

                //Khởi tạo trang
                int Demo = ToolBaiToanDaySo.SoBanGhiTrenMotTrang;
                int step = ToolBaiToanDaySo.BuocNhay;
                int NumOfRecordInPage = Demo;
                int StartNumOfRecordInPage = Demo;
                if (DanhSachDaySo.Count < Demo)
                {
                    NumOfRecordInPage = DanhSachDaySo.Count;
                    StartNumOfRecordInPage = DanhSachDaySo.Count; ;
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
                List<PagesSelect> ListModel = AllToolShare.CreateList(StartNumOfRecordInPage, DanhSachDaySo.Count, step);
                var SelectList = new SelectList(ListModel, "TitleActive", "Values", NumOfRecordInPage);
                ViewData["ListToSelect"] = SelectList;


                //Tổng số bản ghi
                ViewData["TongSo"] = DanhSachDaySo.Count;

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "BaiToanDaySo";
                OnPage.Action = "DanhSachDaySo";
                OnPage.memvar2 = memvar1;
                OnPage.memvar3 = memvar2;
                OnPage.memvar4 = memvar3;
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachDaySo.Count, NumOfRecordInPage);

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

                return View("DanhSachDaySo", DanhSachDaySo.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
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
        public ActionResult TaoTuDongCacDaySo()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<BaiToanDaySoModel> DSDaySo = new List<BaiToanDaySoModel>();
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string PhanLoaiDaySo = Request.Form["PhanLoaiDaySo"];

                #region Tạo các dãy bộ số
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi100" && PhanLoaiDaySo.Trim() == "DayBoSo2So")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 2, 2, 4, 4, 100, 1, 1, 1, ',', '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 2, 2, 5, 5, 100, 1, 1, 1, ',', '~', "12", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 2, 2, 6, 6, 100, 1, 1, 1, ',', '~', "23", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 2, 2, 7, 7, 100, 1, 1, 1, ',', '~', "234", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 2, 2, 8, 10, 100, 1, 1, 1, ',', '~', "456", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                }
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi100" && PhanLoaiDaySo.Trim() == "DayBoSo3So")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 3, 3, 4, 4, 100, 1, 1, 1, ',', '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 3, 3, 5, 5, 100, 1, 1, 1, ',', '~', "12", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 3, 3, 6, 6, 100, 1, 1, 1, ',', '~', "23", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 3, 3, 7, 7, 100, 1, 1, 1, ',', '~', "234", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacBoSo(0, 25, 0, 25, 1, 25, 3, 3, 8, 10, 100, 1, 1, 1, ',', '~', "456", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo));
                }
                
                #endregion

                #region Phạm vi 10
                //CLS1847290691/PhamVi10/DaySoDem
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiDaySo.Trim() == "DaySoDem")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(1, 5, 1, 1, 7, 9, 10, '~', "34567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                }
                //CLS1847290691/PhamVi10/CapSoCong
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi10" && PhanLoaiDaySo.Trim() == "CapSoCong")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(1, 5, 2, 3, 4, 10, 10, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                }
                
                #endregion

                #region Phạm vi 20
                //CLS1847290691/PhamVi10/DaySoDem
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiDaySo.Trim() == "DaySoDem")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(10, 15, 1, 1, 7, 9, 20, '~', "567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                }
                //CLS1847290691/PhamVi10/CapSoCong
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi20" && PhanLoaiDaySo.Trim() == "CapSoCong")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(8, 12, 2, 2, 4, 10, 20, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(2, 12, 3, 3, 4, 4, 20, '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(2, 12, 3, 3, 5, 5, 20, '~', "2", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(2, 12, 3, 3, 6, 10, 20, '~', "34567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(1, 12, 4, 4, 4, 10, 20, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(1, 12, 5, 6, 4, 10, 20, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, true, true));
                }
                #endregion

                #region Phạm vi 30
                //CLS1847290691/PhamVi30/DaySoDem
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiDaySo.Trim() == "DaySoDem")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(20, 25, 1, 1, 7, 9, 30, '~', "567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                }
                //CLS1847290691/PhamVi30/CapSoCong
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30" && PhanLoaiDaySo.Trim() == "CapSoCong")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(15, 20, 2, 2, 4, 10, 30, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(8, 20, 3, 3, 4, 4, 30, '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(8, 20, 3, 3, 5, 5, 30, '~', "2", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(10, 20, 3, 3, 6, 10, 30, '~', "34567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(10, 20, 4, 4, 4, 10, 30, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(1, 20, 5, 10, 5, 10, 30, '~', "1234567", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, true));
                }
                #endregion

                #region Phạm vi 30 đến 100
                //CLS1847290691/PhamVi30/DaySoDem
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30_100" && PhanLoaiDaySo.Trim() == "DaySoDem")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 4, 4, 100, '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 5, 5, 100, '~', "2", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 6, 6, 100, '~', "2", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 7, 7, 100, '~', "4", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 8, 8, 100, '~', "4", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 9, 9, 100, '~', "5", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 1, 1, 10, 10, 100, '~', "6", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                }
                //CLS1847290691/PhamVi30/CapSoCong
                if (ThuocKhoiLop.Trim() == "CLS1847290691" && PhamViPhepToan.Trim() == "PhamVi30_100" && PhanLoaiDaySo.Trim() == "CapSoCong")
                {
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 4, 4, 100, '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 5, 5, 100, '~', "1", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 6, 6, 100, '~', "2", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 7, 7, 100, '~', "3", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 8, 8, 100, '~', "4", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 9, 9, 100, '~', "5", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                    DSDaySo.AddRange(ToolBaiToanDaySo.TaoCacCapSoCong(30, 90, 2, 20, 10, 10, 100, '~', "6", ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo, false, false));
                }
                #endregion

                List<BaiToanDaySoModel> DaySoMoi = DSDaySo.OrderBy(m => m.SapXepThuTu).ToList<BaiToanDaySoModel>();
                //Lưu các dãy số trên vào bảng
                foreach (BaiToanDaySoModel item in DaySoMoi)
                {
                    ToolBaiToanDaySo.ThemMoiMotDaySo(item);
                }

                return RedirectToAction("DanhSachDaySo/" + ThuocKhoiLop  + "/" + PhamViPhepToan + "/" + PhanLoaiDaySo, "BaiToanDaySo");
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }

        /// <summary>
        /// Xóa tất cả các dãy số 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [Authorize]
        public ActionResult XoaTatCacDaySo()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string ThuocKhoiLop = Request.Form["ThuocKhoiLop"];
                string PhamViPhepToan = Request.Form["PhamViPhepToan"];
                string PhanLoaiDaySo = Request.Form["PhanLoaiDaySo"];
                if (String.IsNullOrEmpty(ToolBaiToanDaySo.XoaNhieuDaySo(ThuocKhoiLop, PhamViPhepToan, PhanLoaiDaySo)))
                {
                    return RedirectToAction("DanhSachDaySo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + PhanLoaiDaySo, "BaiToanDaySo");
                }
                else
                {
                    return RedirectToAction("ViewError/DanhSachDaySo/" + ThuocKhoiLop + "/" + PhamViPhepToan + "/" + PhanLoaiDaySo, "Home");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Home");
            }
        }
    }
}