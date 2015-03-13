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
using System.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace TNV.Web.Controllers
{
    public class BackupRetoreController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new NewsCategoryClass(); }

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
        /// Hiển thị giao diện thông tin sao lưu
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpGet]
        [Authorize] 
        public ActionResult DataBackup()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Chuyển ngày hiện tại sang View
                ViewData["BackupDate"] = AllToolShare.GetDateNow();

                return View("BackupData");
            }
            else
            {
                return RedirectToAction("Warning", "Account");
            }

        }

        /// <summary>
        /// Hiển thị danh sách các chuyên mục tin
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [Authorize] 
        public ActionResult MakeBackupData()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string BackupDate = (string)Request.Form["BackupDate"];
                string conn = ConfigurationManager.ConnectionStrings["ToanThongMinhConnectionString"].ConnectionString.ToString().Trim();
                SqlConnection cn = new SqlConnection(conn);
                cn.Open();
                string FileName = "Backup_ToanThongMinh_" + AllToolShare.GetRandomValue("") + "(" + BackupDate.Replace('/', '_') + ").bak";
                string path = Path.Combine(Server.MapPath("~/Content/Backup/"));
                while (System.IO.File.Exists(path + FileName) == true)
                {
                    FileName = "Backup_ToanThongMinh_" + AllToolShare.GetRandomValue("") + "(" + BackupDate.Replace('/', '_') + ").bak";
                    path = Path.Combine(Server.MapPath("~/Content/Backup/"));
                }
                string strSQLBackup = @"BACKUP DATABASE [ToanThongMinh] TO  DISK = N'" + path + FileName + "' WITH NOFORMAT, NOINIT,  NAME = N'ToanThongMinh-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
                SqlCommand cmdBackup = new SqlCommand(strSQLBackup, cn);
                cmdBackup.ExecuteNonQuery();
                cn.Close();
                return File(path + FileName, "application/bak", FileName);
            }
            else
            {
                return RedirectToAction("Warning", "Account");
            }
            
        }
        /// <summary>
        /// Khôi phục dữ liệu sao lưu
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpGet]
        [Authorize]
        public ActionResult DataRestore()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                return View("RetoreData");
            }
            else
            {
                return RedirectToAction("Warning", "Account");
            }

        }
        /// <summary>
        /// Khôi phục dữ liệu
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        [Authorize] 
        public ActionResult MakeRetoreData(FileUploadModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Nhận file backup
                if (ModelState.IsValid)
                {
                    if (model.FileUpload.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(model.FileUpload.FileName);
                        var Extention = Path.GetExtension(model.FileUpload.FileName);
                        if (Extention.ToString().Trim() == ".bak")
                        {
                            string Randomname = AllToolShare.GetRandomValue("Backup");
                            string path = Path.Combine(Server.MapPath("~/Content/Retore/"), Randomname + Extention);
                            while (System.IO.File.Exists(path) == true)
                            {
                                Randomname = AllToolShare.GetRandomValue("Backup");
                                path = Path.Combine(Server.MapPath("~/Content/Retore/"), Randomname + Extention);
                            }
                            model.FileUpload.SaveAs(path);
                            string conn = ConfigurationManager.ConnectionStrings["MasterConnectionString"].ConnectionString.ToString().Trim();
                            string strsqlres = @"RESTORE DATABASE [ToanThongMinh] FROM  DISK = N'" + path + "' WITH FILE = 1 , REPLACE";
                            string str1 = "ALTER DATABASE [ToanThongMinh] SET Single_User WITH Rollback Immediate";
                            string str2 = "ALTER DATABASE [ToanThongMinh] SET Multi_User";
                            SqlConnection cn = new SqlConnection(conn);
                            cn.Open();

                            SqlCommand comm1 = new SqlCommand("use master", cn);
                            comm1.ExecuteNonQuery();

                            SqlCommand comm2 = new SqlCommand(str1, cn);
                            comm2.ExecuteNonQuery();

                            SqlCommand comm3 = new SqlCommand(strsqlres, cn);
                            comm3.ExecuteNonQuery();

                            SqlCommand comm4 = new SqlCommand(str2, cn);
                            comm4.ExecuteNonQuery();

                            cn.Close();
                            ViewData["Error"] = "Kết xuất thành công!";
                            return View("RetoreData");
                        }
                        else
                        {
                            ViewData["Error"] = "Bạn phải chọn file sao lưu có phần định dạng: .bak";
                            return View("RetoreData");
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Bạn phải chưa chọn file sao lưu có phần định dạng: .bak";
                        return View("RetoreData");
                    }
                }
                else
                {
                    ViewData["Error"] = "Bạn phải chưa chọn file sao lưu có phần định dạng: .bak";
                    return View("RetoreData");
                }
            }
            else
            {
                return RedirectToAction("Warning", "Account");
            }

        }
    }
}