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
    public class ToanCoBanLopMotController : Controller
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
        /// Quản trị môn toán lớp 1
        /// </summary>
        /// <returns></returns>
        public ActionResult QuanTriToanCoBanLopMot()
        {
            return View("QuanTriToanCoBanLopMot");
        }

        
    }
}
