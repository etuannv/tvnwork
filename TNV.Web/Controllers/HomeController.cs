using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNV.Web.Models;
using System.Web.Routing;
using System.Web.Security;
using System.Web.UI;

namespace TNV.Web.Controllers
{
    [HandleError]
    public class HomeController : Controller
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
                UserModel ThanhVien=MembershipService.GetOneUserByUserName(User.Identity.Name);
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
        /// Kết xuất ra MSWord theo nội dung chỉ định
        /// </summary>
        /// <param name="memvar1">Tên biến chứa tên file phần mềm Export</param>
        /// <param name="memvar2">Tên biến chưa nội dung cần Export</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult CusExportToWord(string memvar1, string memvar2)
        {
            //Lấy đường dẫn ảnh
            String strPathAndQuery = Request.Url.PathAndQuery;
            String strUrl = Request.Url.AbsoluteUri.Replace(strPathAndQuery, "/");
            strUrl = strUrl.Substring(0, strUrl.Length - 1);
            Response.Clear();
            string FileName = (string)Request.Form[memvar1]; ;
            Response.AddHeader("content-disposition", "attachment;filename=" + FileName + ".doc");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/doc";
            System.IO.StringWriter stringWrite = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter htmlWrite = new HtmlTextWriter(stringWrite);
            Response.Write("<html>");
            Response.Write("<head>");
            Response.Write("<META HTTP-EQUIV=\"Content-Type\" CONTENT=\"text/html; charset=UTF-8\">");
            Response.Write("<meta name=ProgId content=Word.Document>");
            Response.Write("<meta name=Generator content=\"Microsoft Word 9\">");
            Response.Write("<meta name=Originator content=\"Microsoft Word 9\">");
            Response.Write("</head>");
            Response.Write("<body>");
            string WordContent = Request.Form[memvar2].ToString().Replace("643px", "482pt");
            WordContent = WordContent.Replace("px;", "pt;");
            WordContent = WordContent.Replace("0px", "0pt");
            Response.Write(WordContent);
            Response.Write("</body>");
            Response.Write("</html>");
            Response.Flush();
            Response.End();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Index(string memvar1)
        {
            bool LogInCheck = false;
            if (!String.IsNullOrEmpty(memvar1))
            {
                if (memvar1.Trim() == "LogOn")
                {
                    string UserName = Request.Form["UserNameLogin"];
                    string PassWord = Request.Form["PasswordLogin"];
                    bool Remember = true;
                    string RememberView = Request.Form["RememberMe"];
                    if (RememberView == "false")
                    {
                        Remember = false;
                    }
                    if (MembershipService.ValidateUser(UserName, PassWord))
                    {
                        UserModel OneUser = MembershipService.GetOneUserByUserName(UserName);
                        if (OneUser != null)
                        {
                            //Kiểm tra xem tài khoản này có bị khóa hay không? Khóa: 0, Không khoa: 1
                            if (OneUser.Prevent == 0)
                            {
                                ViewData["ThongBao"] = "Thông báo: Tài khoản của bạn đã bị khóa! <br /><br /> Bạn vui lòng liên hệ với ban quản trị để mở khóa tài khoản!";
                                return View("ThongBao");
                            }
                            else if (OneUser.RoleNames.Trim().ToLower() == "smartuser" || OneUser.RoleNames.Trim().ToLower() == "specialuser")
                            {
                                //Kiểm tra xem tài khoản đã hết hạn sử dụng hay chưa? 
                                if (!AllToolShare.CheckExpiredDate(OneUser.ExpiredDate))
                                {
                                    ViewData["ThongBao"] = "Thông báo: Tài khoản của bạn đã hết hạn sử dụng! <br /> Hệ thống sẽ tự động không cho bạn sử dụng các chức năng tính phí! <br />Bạn vui lòng nộp tiền để được dùng không giới hạn chức năng!";
                                    Roles.RemoveUserFromRole(OneUser.UserName, "SmartUser");
                                    Roles.AddUserToRole(OneUser.UserName, "NormalUser");
                                    FormsService.SignIn(UserName, Remember);
                                    MembershipService.CapNhatSoLanDangNhap(UserName);
                                    LogInCheck = true;
                                    return View("ThongBao");
                                }
                                else
                                {
                                    FormsService.SignIn(UserName, Remember);
                                    LogInCheck = true;
                                    //Tăng số lần đăng nhập lên 1 đơn vị
                                    MembershipService.CapNhatSoLanDangNhap(UserName);
                                }

                            }
                            else
                            {
                                FormsService.SignIn(UserName, Remember);
                                LogInCheck = true;
                                //Tăng số lần đăng nhập lên 1 đơn vị
                                MembershipService.CapNhatSoLanDangNhap(UserName);
                            }
                        }
                    }
                    else
                    {
                        ViewData["Error"] = "Sai mật khẩu hoặc tên đăng nhập!";
                        ViewData["UserName"] = UserName;
                        ViewData["Password"] = PassWord;
                    }
                }
                else
                {
                    ViewData["UserName"] = "";
                    ViewData["Password"] = "";
                    ViewData["Error"] = "";
                }
            }
            if (LogInCheck)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                //Đây là người dùng nặc danh, chuyển về trang chủ Website
                NewsContentModel FirstNewsItem = ToolNewsCategory.GetFirstNewsContent();
                ViewData["NewsTitle"] = FirstNewsItem.NewsTitle;
                ViewData["NewsId"] = FirstNewsItem.NewsId;
                ViewData["NewsImage"] = FirstNewsItem.PathNewsImage;
                ViewData["NewsNarration"] = FirstNewsItem.NewsNarration;
                List<NewsContentModel> ListNewsItem = ToolNewsCategory.GetListNewsContent(1, 5);
                ViewData["MainListNews"] = ListNewsItem;
                return View();
            }
        }
        /// <summary>
        /// Hiển thị chi tiết thông tin thành viên
        /// </summary>
        /// <returns></returns>
        public ActionResult Profile()
        {
            if (Request.IsAuthenticated)
            {
                UserModel ThanhVien = MembershipService.GetOneUserByUserName(User.Identity.Name);
                ThanhVien.SchoolName = ToolTinhHuyen.GetOneSchool(ThanhVien.SchoolId).SchoolName;
                ViewData["Profile"] = ThanhVien;
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
                return View("ProfileView");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Đăng ký thành viên tự do
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult RegistryFree()
        {
            //Thông tin đăng nhập(Tại Box đăng nhập)
            ViewData["UserName"] = "";
            ViewData["Password"] = "";
            ViewData["Error"] = "";


            //Khởi tạo đối tượng đăng ký 
            RegisterModel NewRegistry = new RegisterModel();

            //Lấy lại tên đăng nhập
            string UserNames = Request.Form["UserNames"];
            if (!String.IsNullOrEmpty(UserNames))
            {
                NewRegistry.UserNames = UserNames;
            }
            else
            {
                NewRegistry.UserNames = "";
            }

            //Lấy lại mật khẩu đăng nhập
            string PassWords = Request.Form["PassWords"];
            if (!String.IsNullOrEmpty(PassWords))
            {
                NewRegistry.PassWords = PassWords;
            }
            else
            {
                NewRegistry.PassWords = "";
            }

            //Lấy lại mật khẩu đăng nhập lần 2
            string ConfirmPassWords = Request.Form["ConfirmPassWords"];
            if (!String.IsNullOrEmpty(ConfirmPassWords))
            {
                NewRegistry.ConfirmPassWords = ConfirmPassWords;
            }
            else
            {
                NewRegistry.ConfirmPassWords = "";
            }

            //Lấy lại họ và tên
            string FullNames = Request.Form["FullNames"];
            if (!String.IsNullOrEmpty(FullNames))
            {
                NewRegistry.FullNames = FullNames;
            }
            else
            {
                NewRegistry.FullNames = "";
            }
           
            //Lấy lại ngày sinh thành viên            
            string NgaySinh=Request.Form["NgaySinh"];
            if (!String.IsNullOrEmpty(NgaySinh))
            {
                ViewData["NgaySinh"] = NgaySinh;
            }
            else
            {
               ViewData["NgaySinh"]= "01/01/2005";
            }

            //Đọc danh sách tỉnh, huyện
            string MaTinh = Request.Form["MaTinh"];
            string MaTinhCu = Request.Form["MaTinhCu"];
            string MaHuyen = Request.Form["MaHuyen"];
            string MaTruong = Request.Form["SchoolId"];
            if (!String.IsNullOrEmpty(MaTinh))
            {
                if (MaTinh != MaTinhCu)
                {
                    MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                    if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                    {
                        MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                    }
                    else
                    {
                        MaTruong = "";
                    }
                }
            }
            else
            {
                MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                {
                    MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                }
                else
                {
                    MaTruong = "";
                }
            }
            
            List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
            ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
            ViewData["MaTinhCu"] = MaTinh;

            List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
            ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

            List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
            ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);

            //Lấy lại địa chỉ Email
            string Email = Request.Form["Email"];
            if (!String.IsNullOrEmpty(Email))
            {
                NewRegistry.Email = Email;
            }
            else
            {
                NewRegistry.Email = "";
            }

            //Lấy lại địa chỉ số điện thoại
            string MobileAlias = Request.Form["MobileAlias"];
            if (!String.IsNullOrEmpty(MobileAlias))
            {
                NewRegistry.MobileAlias = MobileAlias;
            }
            else
            {
                NewRegistry.MobileAlias = "";
            }

            //Lấy lại tên trường học
            string SchoolName = Request.Form["SchoolName"];
            if (!String.IsNullOrEmpty(SchoolName))
            {
                ViewData["SchoolName"] = SchoolName;
            }
            else
            {
                ViewData["SchoolName"] = "";
            }

            //Lấy lại mã bảo vệ
            string Security = Request.Form["Security"];
            if (!String.IsNullOrEmpty(Security))
            {
                ViewData["Security"] = Security;
            }
            else
            {
                Random rd = new Random();
                int GiaTriNgauNhien = rd.Next(1000, 2000);
                ViewData["Security"] = GiaTriNgauNhien;
            }

            //Lấy lại mã bảo vệ người dùng nhập
            string SecurityValue = Request.Form["SecurityValue"];
            if (!String.IsNullOrEmpty(SecurityValue))
            {
                ViewData["SecurityValue"] = SecurityValue;
            }
            else
            {
                ViewData["SecurityValue"] = "";
            }

            //Lấy lại mã bảo vệ người dùng nhập
            string Checked = Request.Form["DongY"];
            if (!String.IsNullOrEmpty(Checked))
            {
                ViewData["Checked"] = "checked=\"Checked\"";
            }
            else
            {
                ViewData["Checked"] = "";
            }

            return View(NewRegistry);
        }
        /// <summary>
        /// Đăng ký thành viên tự do
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveRegistry(RegisterModel model)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";

            //Lấy ngày hiện tại làm ngày đăng ký
            model.CreateDate = Convert.ToDateTime(AllToolShare.GetDateNow(), DateFormat);

            //Chuyển ngày sinh sang dạng ngày của hệ thông
            model.NgaySinh = Convert.ToDateTime(Request.Form["NgaySinhTV"], DateFormat);

            //Mặc định ngày bắt đầu tính phí
            model.StartDate = Convert.ToDateTime("01/01/2000", DateFormat);

            //Mặc định ngày hết hạn tính phí
            model.ExpiredDate = Convert.ToDateTime("01/01/2000", DateFormat);

            if (ModelState.IsValid)
            {
                // Đăng ký người sử dụng
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserNames, model.PassWords, model.Email);
                if (createStatus == MembershipCreateStatus.Success) //Nếu đăng ký thành công
                {
                    
                    //Chuyển trạng thái người dùng về đã đăng nhập
                    FormsService.SignIn(model.UserNames, false /* createPersistentCookie */);

                    //Đăng ký các thuộc tính của người dùng
                    UserPropertyModel NewUserProperty = new UserPropertyModel();
                    NewUserProperty.UserName = model.UserNames;
                    NewUserProperty.UserFullName = model.FullNames;

                    NewUserProperty.NgaySinh = model.NgaySinh;
                    NewUserProperty.MaTinh = Request.Form["MaTinh"];
                    NewUserProperty.MaHuyen = Request.Form["MaHuyen"];

                    NewUserProperty.LoginNumber = 1;

                    NewUserProperty.PhoneNumber = model.MobileAlias;

                    NewUserProperty.Prevent = 1;

                    NewUserProperty.CreateDate = model.CreateDate;

                    //Kiểm tra xem người dùng có bổ sung thêm một trường học mới hay không, nếu có thì phải thêm mới tên trường đó 
                    string NewSchoolName = Request.Form["SchoolName"];
                    if (!String.IsNullOrEmpty(NewSchoolName))
                    {
                        SchoolModel NewSchool = new SchoolModel();
                        NewSchool.DistrictId = NewUserProperty.MaHuyen;
                        NewSchool.SchoolName = NewSchoolName;
                        //Sinh ngẫu nhiêm mã trường
                        List<SchoolModel> ListSchool = ToolTinhHuyen.AllSchool();
                        List<ListSearch> ListID = new List<ListSearch>();
                        foreach (SchoolModel ItemId in ListSchool)
                        {
                            ListSearch IdItem = new ListSearch();
                            IdItem.FieldSearch = ItemId.SchoolId;
                            ListID.Add(IdItem);
                        }
                        NewSchool.SchoolId = AllToolShare.GetRandomId(ListID, "TTH").ToString();
                        //Sinh số thứ tự
                        List<ListFindMax> ListOrder = new List<ListFindMax>();
                        foreach (SchoolModel Item in ListSchool)
                        {
                            ListFindMax OrderItem = new ListFindMax();
                            OrderItem.FieldFindMax = Item.SchoolOrder;
                            ListOrder.Add(OrderItem);
                        }
                        NewSchool.SchoolOrder = AllToolShare.GetMaxOrderby(ListOrder);
                        ToolTinhHuyen.SaveNewSchool(NewSchool);
                        //Gán mã trường học mới cho thành viên
                        NewUserProperty.SchoolId = NewSchool.SchoolId;
                    }
                    else
                    {
                        NewUserProperty.SchoolId = Request.Form["SchoolId"];
                    }

                    NewUserProperty.Prevent = 1;

                    NewUserProperty.StartDate = model.StartDate;

                    NewUserProperty.ExpiredDate = model.ExpiredDate;

                    MembershipService.SaveNewUserProperty(NewUserProperty);

                    //Đăng ký Roles cho người dùng - Là người dùng bình thường và không trả tiền
                    Roles.AddUserToRole(model.UserNames, "NormalUser");

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    string Mes = "";
                    if (createStatus == MembershipCreateStatus.DuplicateEmail || createStatus == MembershipCreateStatus.InvalidEmail)
                    {
                        Mes = "Địa chỉ Email này bị sai hoặc đã được sử dụng!";
                    }
                    if (createStatus == MembershipCreateStatus.DuplicateUserName || createStatus == MembershipCreateStatus.InvalidUserName)
                    {
                        if (String.IsNullOrEmpty(Mes)) Mes = "Tên đăng nhập này bị sai hoặc đã được sử dụng!"; else Mes = ", Tên đăng nhập này bị sai hoặc đã được sử dụng!";
                    }
                    ViewData["Mes"] ="Thông báo: Không thể đăng ký được thành viên này! "+ Mes;
                    //Đọc danh sách tỉnh, huyện
                    string MaTinh = Request.Form["MaTinh"];
                    string MaTinhCu = Request.Form["MaTinhCu"];
                    string MaHuyen = Request.Form["MaHuyen"];
                    string MaTruong = Request.Form["SchoolId"];
                    if (!String.IsNullOrEmpty(MaTinh))
                    {
                        if (MaTinh != MaTinhCu)
                        {
                            MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                            if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                            {
                                MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                            }
                            else
                            {
                                MaTruong = "";
                            }
                        }
                    }
                    else
                    {
                        MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                        MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                        if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                        {
                            MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                        }
                        else
                        {
                            MaTruong = "";
                        }
                    }

                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                    ViewData["MaTinhCu"] = MaTinh;

                    List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                    ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                    List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                    ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);

                    //Lấy lại tên trường học
                    string SchoolName = Request.Form["SchoolName"];
                    if (!String.IsNullOrEmpty(SchoolName))
                    {
                        ViewData["SchoolName"] = SchoolName;
                    }
                    else
                    {
                        ViewData["SchoolName"] = "";
                    }

                    //Lấy lại mã bảo vệ
                    string Security = Request.Form["Security"];
                    if (!String.IsNullOrEmpty(Security))
                    {
                        ViewData["Security"] = Security;
                    }
                    else
                    {
                        Random rd = new Random();
                        int GiaTriNgauNhien = rd.Next(1000, 2000);
                        ViewData["Security"] = GiaTriNgauNhien;
                    }

                    //Lấy lại mã bảo vệ người dùng nhập
                    string SecurityValue = Request.Form["SecurityValue"];
                    if (!String.IsNullOrEmpty(SecurityValue))
                    {
                        ViewData["SecurityValue"] = SecurityValue;
                    }
                    else
                    {
                        ViewData["SecurityValue"] = "";
                    }

                    //Lấy lại mã bảo vệ người dùng nhập
                    string Checked = Request.Form["DongY"];
                    if (!String.IsNullOrEmpty(Checked))
                    {
                        ViewData["Checked"] = "checked=\"Checked\"";
                    }
                    else
                    {
                        ViewData["Checked"] = "";
                    }

                    //Lấy lại ngày sinh thành viên            
                    string NgaySinh = Request.Form["NgaySinh"];
                    if (!String.IsNullOrEmpty(NgaySinh))
                    {
                        ViewData["NgaySinh"] = NgaySinh;
                    }
                    else
                    {
                        ViewData["NgaySinh"] = AllToolShare.GetDateNow();
                    }

                    return View("RegistryFree",model);
                }
            }
            else
            {
                //Không đăng ký được thành viên
                //Đọc danh sách tỉnh, huyện
                string MaTinh = Request.Form["MaTinh"];
                string MaTinhCu = Request.Form["MaTinhCu"];
                string MaHuyen = Request.Form["MaHuyen"];
                string MaTruong = Request.Form["SchoolId"];
                if (!String.IsNullOrEmpty(MaTinh))
                {
                    if (MaTinh != MaTinhCu)
                    {
                        MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                        if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                        {
                            MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                        }
                        else
                        {
                            MaTruong = "";
                        }
                    }
                }
                else
                {
                    MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                    MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                    if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                    {
                        MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                    }
                    else
                    {
                        MaTruong = "";
                    }
                }

                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                ViewData["MaTinhCu"] = MaTinh;

                List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);

                //Lấy lại tên trường học
                string SchoolName = Request.Form["SchoolName"];
                if (!String.IsNullOrEmpty(SchoolName))
                {
                    ViewData["SchoolName"] = SchoolName;
                }
                else
                {
                    ViewData["SchoolName"] = "";
                }

                //Lấy lại mã bảo vệ
                string Security = Request.Form["Security"];
                if (!String.IsNullOrEmpty(Security))
                {
                    ViewData["Security"] = Security;
                }
                else
                {
                    Random rd = new Random();
                    int GiaTriNgauNhien = rd.Next(1000, 2000);
                    ViewData["Security"] = GiaTriNgauNhien;
                }

                //Lấy lại mã bảo vệ người dùng nhập
                string SecurityValue = Request.Form["SecurityValue"];
                if (!String.IsNullOrEmpty(SecurityValue))
                {
                    ViewData["SecurityValue"] = SecurityValue;
                }
                else
                {
                    ViewData["SecurityValue"] = "";
                }

                //Lấy lại mã bảo vệ người dùng nhập
                string Checked = Request.Form["DongY"];
                if (!String.IsNullOrEmpty(Checked))
                {
                    ViewData["Checked"] = "checked=\"Checked\"";
                }
                else
                {
                    ViewData["Checked"] = "";
                }
                //Lấy lại ngày sinh thành viên            
                string NgaySinh = Request.Form["NgaySinh"];
                if (!String.IsNullOrEmpty(NgaySinh))
                {
                    ViewData["NgaySinh"] = NgaySinh;
                }
                else
                {
                    ViewData["NgaySinh"] = AllToolShare.GetDateNow();
                }


                return View("RegistryFree", model);
            }

        }

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ChangePassword()
        {
            return View();
        }

        /// <summary>
        /// Lưu thay đổi mật khẩu
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveNewPassword()
        {
            if (Request.IsAuthenticated)
            {
                string NewPassword = Request.Form["NewPassword"];
                MembershipProvider _provider = Membership.Provider;
                MembershipUser UserP = _provider.GetUser(User.Identity.Name, true);
                UserP.UnlockUser();
                MembershipService.ChangePassword(User.Identity.Name, UserP.ResetPassword(), NewPassword.Trim());
                return View("ChangePasswordSucces");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Quản lý danh sách thành viên
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult UserManager(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Khởi tạo trang
                int NumOfRecordInPage =20;

                //Lấy loại người dùng hiện tại
                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];
                if (String.IsNullOrEmpty(LoaiNguoiDung))
                {
                    if (String.IsNullOrEmpty(memvar2))
                    {
                        LoaiNguoiDung = MembershipService.LayChucNangDauTien().RoleName;
                    }
                    else
                    {
                        LoaiNguoiDung = memvar2;
                    }
                }
                ViewData["LoaiNguoiDungCu"] = LoaiNguoiDung;

                List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);

                List<UserModel> AllUserinARole = MembershipService.GetAllUserByRolesName(LoaiNguoiDung);

                //Thêm tên trường học vào các bản ghi
                List<UserModel> NewAllUser = new List<UserModel>();
                foreach (UserModel item in AllUserinARole)
                {
                    UserModel NewUser = item;

                    if (!String.IsNullOrEmpty(item.SchoolId))
                    {
                        NewUser.SchoolName = ToolTinhHuyen.GetOneSchool(item.SchoolId).SchoolName;
                    }
                    NewAllUser.Add(NewUser);
                }

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "Home";
                OnPage.Action = "UserManager";
                OnPage.memvar2 = "";
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(AllUserinARole.Count, NumOfRecordInPage);
                if (String.IsNullOrEmpty(memvar1))
                {
                    string PageCurent = Request.Form["PageCurent"];
                    string LoaiNguoiDungCu = Request.Form["LoaiNguoiDungCu"];
                    if (String.IsNullOrEmpty(LoaiNguoiDungCu))
                    {
                        OnPage.CurentPage = 1;
                    }
                    else if (LoaiNguoiDung.Trim().ToLower()!=LoaiNguoiDungCu.ToString().Trim().ToLower())
                    {
                        OnPage.CurentPage = 1;
                    }
                    else if (String.IsNullOrEmpty(PageCurent))
                    {
                        OnPage.CurentPage = 1;
                    }
                    else
                    {
                        OnPage.CurentPage = Convert.ToInt32(PageCurent);
                    }
                }
                else
                {
                    try
                    {
                        OnPage.CurentPage = Convert.ToInt32(memvar1);
                    }
                    catch
                    {
                        OnPage.CurentPage = 1;
                    }
                }
                ViewData["Page"] = OnPage;
                ViewData["PageCurent"] = OnPage.CurentPage;
                return View(NewAllUser.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        /// <summary>
        /// Xóa thông tin một thành viên
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelUser(Guid memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy mã tỉnh, thành phố
                string LoaiNguoiDung = (string)Request.Form["LoaiNguoiDung"];
                string PageCurent = (string)Request.Form["PageCurent"];
                MembershipService.DelUserProperty(MembershipService.GetOneUserByUserId(memvar1).UserName);
                MembershipService.DelUser(memvar1);
                return RedirectToAction("UserManager/" + PageCurent + "/" + LoaiNguoiDung, "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        
        /// <summary>
        /// Thêm mới một thành viên
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AddNewUser()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Thông tin đăng nhập(Tại Box đăng nhập)
                ViewData["UserName"] = "";
                ViewData["Password"] = "";
                ViewData["Error"] = "";

                ViewData["PageCurent"] = Request.Form["PageCurent"];

                //Khởi tạo đối tượng đăng ký 
                RegisterModel NewUser = new RegisterModel();

                //Lấy lại tên đăng nhập
                string UserNames = Request.Form["UserNames"];
                if (!String.IsNullOrEmpty(UserNames))
                {
                    NewUser.UserNames = UserNames;
                }
                else
                {
                    NewUser.UserNames = "";
                }

                //Lấy lại mật khẩu đăng nhập
                string PassWords = Request.Form["PassWords"];
                if (!String.IsNullOrEmpty(PassWords))
                {
                    NewUser.PassWords = PassWords;
                }
                else
                {
                    NewUser.PassWords = "";
                }

                //Lấy lại mật khẩu đăng nhập lần 2
                string ConfirmPassWords = Request.Form["ConfirmPassWords"];
                if (!String.IsNullOrEmpty(ConfirmPassWords))
                {
                    NewUser.ConfirmPassWords = ConfirmPassWords;
                }
                else
                {
                    NewUser.ConfirmPassWords = "";
                }

                //Lấy lại họ và tên
                string FullNames = Request.Form["FullNames"];
                if (!String.IsNullOrEmpty(FullNames))
                {
                    NewUser.FullNames = FullNames;
                }
                else
                {
                    NewUser.FullNames = "";
                }

                //Lấy lại ngày sinh thành viên            
                string NgaySinh = Request.Form["NgaySinh"];
                if (!String.IsNullOrEmpty(NgaySinh))
                {
                    ViewData["NgaySinh"] = NgaySinh;
                }
                else
                {
                    ViewData["NgaySinh"] = "01/01/2005";
                }

                //Lấy loại người dùng hiện tại
                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];
                if (String.IsNullOrEmpty(LoaiNguoiDung))
                {
                    LoaiNguoiDung = MembershipService.LayChucNangDauTien().RoleName;
                }

                List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);
                ViewData["LoaiNguoiDung"] = LoaiNguoiDung;

                //Đọc danh sách tỉnh, huyện
                string MaTinh = Request.Form["MaTinh"];
                string MaTinhCu = Request.Form["MaTinhCu"];
                string MaHuyen = Request.Form["MaHuyen"];
                string MaTruong = Request.Form["SchoolId"];
                if (!String.IsNullOrEmpty(MaTinh))
                {
                    if (MaTinh != MaTinhCu)
                    {
                        MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                        if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                        {
                            MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                        }
                        else
                        {
                            MaTruong = "";
                        }
                    }
                }
                else
                {
                    MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                    MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                    if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                    {
                        MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                    }
                    else
                    {
                        MaTruong = "";
                    }
                }

                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                ViewData["MaTinhCu"] = MaTinh;

                List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);
                //Lấy lại địa chỉ Email
                string Email = Request.Form["Email"];
                if (!String.IsNullOrEmpty(Email))
                {
                    NewUser.Email = Email;
                }
                else
                {
                    NewUser.Email = "";
                }

                //Lấy lại địa chỉ số điện thoại
                string MobileAlias = Request.Form["MobileAlias"];
                if (!String.IsNullOrEmpty(MobileAlias))
                {
                    NewUser.MobileAlias = MobileAlias;
                }
                else
                {
                    NewUser.MobileAlias = "";
                }

                return View("AddNewUser", NewUser);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu mới một người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveNewUser(RegisterModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
                DateFormat.ShortDatePattern = "dd/MM/yyyy";

                //Lấy ngày hiện tại làm ngày đăng ký
                model.CreateDate = Convert.ToDateTime(AllToolShare.GetDateNow(), DateFormat);

                //Chuyển ngày sinh sang dạng ngày của hệ thông
                model.NgaySinh = Convert.ToDateTime(Request.Form["NgaySinhTV"], DateFormat);

                //Mặc định ngày bắt đầu tính phí
                model.StartDate = Convert.ToDateTime("01/01/2000", DateFormat);

                //Mặc định ngày hết hạn tính phí
                model.ExpiredDate = Convert.ToDateTime("01/01/2000", DateFormat);

                if (ModelState.IsValid)
                {
                    // Đăng ký người sử dụng
                    MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserNames, model.PassWords, model.Email);
                    if (createStatus == MembershipCreateStatus.Success) //Nếu đăng ký thành công
                    {

                        //Đăng ký các thuộc tính của người dùng
                        UserPropertyModel NewUserProperty = new UserPropertyModel();
                        NewUserProperty.UserName = model.UserNames;
                        NewUserProperty.UserFullName =AllToolShare.ChuanHoaXauTiengViet(model.FullNames);

                        NewUserProperty.NgaySinh = model.NgaySinh;

                        string LoaiNguoiDung=Request.Form["LoaiNguoiDung"];

                        NewUserProperty.MaTinh = Request.Form["MaTinh"];
                        NewUserProperty.MaHuyen = Request.Form["MaHuyen"];

                        if (LoaiNguoiDung.Trim().ToLower() != "AdminOfSystem")
                        {
                            NewUserProperty.SchoolId = Request.Form["SchoolId"];
                        }
                        else
                        {
                            NewUserProperty.SchoolId = "";
                        }

                        NewUserProperty.LoginNumber = 1;

                        NewUserProperty.PhoneNumber = model.MobileAlias;

                        NewUserProperty.Prevent = 1;

                        NewUserProperty.CreateDate = model.CreateDate;

                        NewUserProperty.Prevent = 1;

                        NewUserProperty.StartDate = model.StartDate;

                        NewUserProperty.ExpiredDate = model.ExpiredDate;

                        MembershipService.SaveNewUserProperty(NewUserProperty);

                        //Đăng ký Roles cho người dùng - Là người dùng bình thường và không trả tiền
                        Roles.AddUserToRole(model.UserNames, LoaiNguoiDung);

                        return RedirectToAction("UserManager/" +Request.Form["PageCurent"].ToString().Trim()+ "/" + LoaiNguoiDung, "Home");
                    }
                    else
                    {
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        string Mes = "";
                        if (createStatus == MembershipCreateStatus.DuplicateEmail || createStatus == MembershipCreateStatus.InvalidEmail)
                        {
                            Mes = "Địa chỉ Email này bị sai hoặc đã được sử dụng!";
                        }
                        if (createStatus == MembershipCreateStatus.DuplicateUserName || createStatus == MembershipCreateStatus.InvalidUserName)
                        {
                            if (String.IsNullOrEmpty(Mes)) Mes = "Tên đăng nhập này bị sai hoặc đã được sử dụng!"; else Mes = ", Tên đăng nhập này bị sai hoặc đã được sử dụng!";
                        }
                        ViewData["Mes"] = "Thông báo: Không thể đăng ký được thành viên này! " + Mes;

                        string LoaiNguoiDung=Request.Form["LoaiNguoiDung"];
                        List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                        ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);
                        ViewData["LoaiNguoiDung"] = LoaiNguoiDung;

                        //Đọc danh sách tỉnh, huyện
                        string MaTinh = Request.Form["MaTinh"];
                        string MaTinhCu = Request.Form["MaTinhCu"];
                        string MaHuyen = Request.Form["MaHuyen"];
                        string MaTruong = Request.Form["SchoolId"];
                        if (!String.IsNullOrEmpty(MaTinh))
                        {
                            if (MaTinh != MaTinhCu)
                            {
                                MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                                if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                                {
                                    MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                                }
                                else
                                {
                                    MaTruong = "";
                                }
                            }
                        }
                        else
                        {
                            MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                            MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                            if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                            {
                                MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                            }
                            else
                            {
                                MaTruong = "";
                            }
                        }

                        List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                        ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                        ViewData["MaTinhCu"] = MaTinh;

                        List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                        ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                        List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                        ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);

                       
                        //Lấy lại ngày sinh thành viên            
                        string NgaySinh = Request.Form["NgaySinh"];
                        if (!String.IsNullOrEmpty(NgaySinh))
                        {
                            ViewData["NgaySinh"] = NgaySinh;
                        }
                        else
                        {
                            ViewData["NgaySinh"] = AllToolShare.GetDateNow();
                        }

                        return View("AddNewUser", model);
                    }
                }
                else
                {
                    //Không đăng ký được thành viên
                    ViewData["PageCurent"] = Request.Form["PageCurent"];
                      string LoaiNguoiDung=Request.Form["LoaiNguoiDung"];
                        List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                        ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);
                        ViewData["LoaiNguoiDung"] = LoaiNguoiDung;

                        //Đọc danh sách tỉnh, huyện
                        string MaTinh = Request.Form["MaTinh"];
                        string MaTinhCu = Request.Form["MaTinhCu"];
                        string MaHuyen = Request.Form["MaHuyen"];
                        string MaTruong = Request.Form["SchoolId"];
                        if (!String.IsNullOrEmpty(MaTinh))
                        {
                            if (MaTinh != MaTinhCu)
                            {
                                MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                                if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                                {
                                    MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                                }
                                else
                                {
                                    MaTruong = "";
                                }
                            }
                        }
                        else
                        {
                            MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                            MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                            if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                            {
                                MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                            }
                            else
                            {
                                MaTruong = "";
                            }
                        }

                        List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                        ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                        ViewData["MaTinhCu"] = MaTinh;

                        List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                        ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                        List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                        ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);
                   
                    //Lấy lại ngày sinh thành viên            
                    string NgaySinh = Request.Form["NgaySinh"];
                    if (!String.IsNullOrEmpty(NgaySinh))
                    {
                        ViewData["NgaySinh"] = NgaySinh;
                    }
                    else
                    {
                        ViewData["NgaySinh"] = AllToolShare.GetDateNow();
                    }


                    return View("AddNewUser", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Thêm mới một thành viên
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult EditUser(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Thông tin đăng nhập(Tại Box đăng nhập)
                ViewData["UserName"] = "";
                ViewData["Password"] = "";
                ViewData["Error"] = "";

                //Khởi tạo thông tin người dùng muốn sửa
                RegisterModel ItemModel = new RegisterModel();

                //Đọc các thông tin của người dùng ra cho phép sửa
                if (!String.IsNullOrEmpty(memvar1))
                {
                    Guid UserId = Guid.Empty;     
                    UserId = new Guid(memvar1);
                    //Đây là trường hợp đầu tiên load form sửa
                    UserModel ThongTinNguoiDung = MembershipService.GetOneUserByUserId(UserId);
                    ItemModel.UserNames = ThongTinNguoiDung.UserName;
                    ItemModel.FullNames = ThongTinNguoiDung.FullName;
                    ViewData["NgaySinh"] = AllToolShare.GetDateFormDateTime(ThongTinNguoiDung.NgaySinh);
                    ItemModel.Email = ThongTinNguoiDung.Email;
                    ItemModel.MobileAlias = ThongTinNguoiDung.MobileAlias;
                    ItemModel.RoleNames = ThongTinNguoiDung.RoleNames;
                    ViewData["LoaiNguoiDungCu"] = ItemModel.RoleNames; 
                    ItemModel.MaTinh = ThongTinNguoiDung.MaTinh;
                    ItemModel.MaHuyen = ThongTinNguoiDung.MaHuyen;
                    ItemModel.SchoolId = ThongTinNguoiDung.SchoolId;
                    if (ItemModel.RoleNames.ToLower() != "AdminOfSystem" && ItemModel.RoleNames.ToLower() != "normaluser")
                    {
                        ViewData["StartDateMoney"] = AllToolShare.GetDateFormDateTime(ThongTinNguoiDung.StartDate);
                        ViewData["ExpiredDateMoney"] = AllToolShare.GetDateFormDateTime(ThongTinNguoiDung.ExpiredDate);
                    }
                }
                else
                {
                    //Trường hợp load dữ liệu lần 2
                    //Lấy lại tên đăng nhập
                    string UserNames = Request.Form["UserNames"];
                    if (!String.IsNullOrEmpty(UserNames))
                    {
                        ItemModel.UserNames = UserNames;
                    }
                    else
                    {
                        ItemModel.UserNames = "";
                    }

                    //Lấy lại họ và tên
                    string FullNames = Request.Form["FullNames"];
                    if (!String.IsNullOrEmpty(FullNames))
                    {
                        ItemModel.FullNames = FullNames;
                    }
                    else
                    {
                        ItemModel.FullNames = "";
                    }

                    //Lấy lại ngày sinh thành viên            
                    string NgaySinh = Request.Form["NgaySinh"];
                    if (!String.IsNullOrEmpty(NgaySinh))
                    {
                        ViewData["NgaySinh"] = NgaySinh;
                    }
                    else
                    {
                        ViewData["NgaySinh"] = "01/01/2005";
                    }

                    //Lấy lại địa chỉ Email
                    string Email = Request.Form["Email"];
                    if (!String.IsNullOrEmpty(Email))
                    {
                        ItemModel.Email = Email;
                    }
                    else
                    {
                        ItemModel.Email = "";
                    }

                    //Lấy lại địa chỉ số điện thoại
                    string MobileAlias = Request.Form["MobileAlias"];
                    if (!String.IsNullOrEmpty(MobileAlias))
                    {
                        ItemModel.MobileAlias = MobileAlias;
                    }
                    else
                    {
                        ItemModel.MobileAlias = "";
                    }
                    ItemModel.RoleNames = Request.Form["LoaiNguoiDung"];
                    ViewData["LoaiNguoiDungCu"] = Request.Form["LoaiNguoiDungCu"]; ; 
                    ItemModel.MaTinh = Request.Form["MaTinh"];
                    ItemModel.MaHuyen = Request.Form["MaHuyen"];
                    ItemModel.SchoolId = Request.Form["SchoolId"];

                    if (ItemModel.RoleNames.ToLower() != "AdminOfSystem" && ItemModel.RoleNames.ToLower() != "normaluser")
                    {
                        //Lấy lại ngày tính phí của thành viên            
                        string StartDateMoney = Request.Form["StartDateMoney"];
                        if (!String.IsNullOrEmpty(StartDateMoney))
                        {
                            ViewData["StartDateMoney"] = StartDateMoney;
                        }
                        else
                        {
                            ViewData["StartDateMoney"] = AllToolShare.GetDateNow();
                        }

                        //Lấy lại ngày hết hạn tính phí của thành viên            
                        string ExpiredDateMoney = Request.Form["ExpiredDateMoney"];
                        if (!String.IsNullOrEmpty(ExpiredDateMoney))
                        {
                            ViewData["ExpiredDateMoney"] = ExpiredDateMoney;
                        }
                        else
                        {
                            ViewData["ExpiredDateMoney"] = AllToolShare.GetDateAddYear(DateTime.Now.Date, 1);
                        }
                    }
                }
                
                //Đọc lại trang hiện tại
                ViewData["PageCurent"] = Request.Form["PageCurent"];

                //Lấy loại người dùng hiện tại
                string LoaiNguoiDung = ItemModel.RoleNames;
                if (String.IsNullOrEmpty(LoaiNguoiDung))
                {
                    LoaiNguoiDung = MembershipService.LayChucNangDauTien().RoleName;
                }

                List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);
                ViewData["LoaiNguoiDung"] = LoaiNguoiDung;

                //Đọc danh sách tỉnh, huyện
                string MaTinh = ItemModel.MaTinh;
                string MaTinhCu = Request.Form["MaTinhCu"];
                string MaHuyen = ItemModel.MaHuyen;
                string MaTruong = ItemModel.SchoolId;
                if (!String.IsNullOrEmpty(MaTinh))
                {
                    if (!String.IsNullOrEmpty(MaTinhCu))
                    {
                        if (MaTinh != MaTinhCu)
                        {
                            MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                            if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                            {
                                MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                            }
                            else
                            {
                                MaTruong = "";
                            }
                        }
                    }
                }
                else
                {
                    MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                    MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                    if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                    {
                        MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                    }
                    else
                    {
                        MaTruong = "";
                    }
                }

                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                ViewData["MaTinhCu"] = MaTinh;

                List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);
               
                return View("EditUser", ItemModel);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu sửa một người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditUser(RegisterModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
                DateFormat.ShortDatePattern = "dd/MM/yyyy";

                //Chuyển ngày sinh sang dạng ngày của hệ thống
                model.NgaySinh = Convert.ToDateTime(Request.Form["NgaySinhTV"], DateFormat);

                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];
                if (LoaiNguoiDung.ToString().Trim().ToLower() != "AdminOfSystem" && LoaiNguoiDung.ToString().Trim().ToLower() != "normaluser")
                {
                    //Chuyển ngày bắt đầu tính phí sang dạng ngày của hệ thông
                    model.StartDate = Convert.ToDateTime(Request.Form["StartDateMoney"], DateFormat);

                    //Chuyển ngày hết hạn tính phí sang dạng ngày của hệ thông
                    model.ExpiredDate = Convert.ToDateTime(Request.Form["ExpiredDateMoney"], DateFormat);
                }
                else
                {
                    //Mặc định ngày bắt đầu tính phí
                    model.StartDate = Convert.ToDateTime("01/01/2000", DateFormat);

                    //Mặc định ngày hết hạn tính phí
                    model.ExpiredDate = Convert.ToDateTime("01/01/2000", DateFormat);
                }

                if (ModelState.IsValid)
                {
                    //Đăng ký các thuộc tính của người dùng
                    UserPropertyModel EditUserProperty = new UserPropertyModel();
                    EditUserProperty.UserName = model.UserNames;
                    EditUserProperty.UserFullName =AllToolShare.ChuanHoaXauTiengViet(model.FullNames);

                    EditUserProperty.NgaySinh = model.NgaySinh;

                    EditUserProperty.MaTinh = Request.Form["MaTinh"];
                    EditUserProperty.MaHuyen = Request.Form["MaHuyen"];

                    if (LoaiNguoiDung.Trim().ToLower() != "AdminOfSystem")
                    {
                        EditUserProperty.SchoolId = Request.Form["SchoolId"];
                    }
                    else
                    {
                        EditUserProperty.SchoolId = "";
                    }

                    EditUserProperty.PhoneNumber = model.MobileAlias;

                    EditUserProperty.CreateDate = model.CreateDate;

                    EditUserProperty.StartDate = model.StartDate;

                    EditUserProperty.ExpiredDate = model.ExpiredDate;

                    MembershipService.SaveEditUserProperty(EditUserProperty);

                    //Đăng ký Roles cho người dùng - Là người dùng bình thường và không trả tiền
                    string LoaiNguoiDungCu = Request.Form["LoaiNguoiDungCu"];
                    if (LoaiNguoiDungCu.Trim() != LoaiNguoiDung.Trim())
                    {
                        Roles.RemoveUserFromRole(model.UserNames, LoaiNguoiDungCu);
                        Roles.AddUserToRole(model.UserNames, LoaiNguoiDung);
                    }

                    //Chỉnh sửa địa chỉ mail
                    UserModel EditUser = MembershipService.GetOneUserByUserName(model.UserNames);
                    MembershipService.SaveEditMemberShip(EditUser.UserId, model.Email);
 
                    return RedirectToAction("UserManager/" + Request.Form["PageCurent"].ToString().Trim() + "/" + LoaiNguoiDung, "Home");
                }
                else
                {
                    //Không đăng ký được thành viên
                    ViewData["PageCurent"] = Request.Form["PageCurent"];
                    List<RolesModel> DanhSachChucNang = MembershipService.DanhSachChucNang();
                    ViewData["DSLoaiNguoiDung"] = new SelectList(DanhSachChucNang, "RoleName", "Description", LoaiNguoiDung);
                    ViewData["LoaiNguoiDung"] = LoaiNguoiDung;

                    //Đọc danh sách tỉnh, huyện
                    string MaTinh = Request.Form["MaTinh"];
                    string MaTinhCu = Request.Form["MaTinhCu"];
                    string MaHuyen = Request.Form["MaHuyen"];
                    string MaTruong = Request.Form["SchoolId"];
                    if (!String.IsNullOrEmpty(MaTinh))
                    {
                        if (MaTinh != MaTinhCu)
                        {
                            MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                            if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                            {
                                MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                            }
                            else
                            {
                                MaTruong = "";
                            }
                        }
                    }
                    else
                    {
                        MaTinh = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                        MaHuyen = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinh).MaHuyenThi;
                        if (ToolTinhHuyen.CheckSchoolInDistrict(MaHuyen))
                        {
                            MaTruong = ToolTinhHuyen.FirstSchool(MaHuyen).SchoolId;
                        }
                        else
                        {
                            MaTruong = "";
                        }
                    }

                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinh);
                    ViewData["MaTinhCu"] = MaTinh;

                    List<HuyenThiXaModel> DsHuyenThiXa = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinh);
                    ViewData["DSHuyen"] = new SelectList(DsHuyenThiXa, "MaHuyenThi", "TenHuyenThi", MaHuyen);

                    List<SchoolModel> DanhSachTruong = ToolTinhHuyen.AllSchool(MaHuyen);
                    ViewData["DSTruongHoc"] = new SelectList(DanhSachTruong, "SchoolId", "SchoolName", MaTruong);

                    //Lấy lại ngày sinh thành viên            
                    string NgaySinh = Request.Form["NgaySinh"];
                    if (!String.IsNullOrEmpty(NgaySinh))
                    {
                        ViewData["NgaySinh"] = NgaySinh;
                    }
                    else
                    {
                        ViewData["NgaySinh"] = AllToolShare.GetDateNow();
                    }
                    if (LoaiNguoiDung.ToLower() != "AdminOfSystem" && LoaiNguoiDung.ToLower() != "normaluser")
                    {
                        //Lấy lại ngày tính phí của thành viên            
                        string StartDateMoney = Request.Form["StartDateMoney"];
                        if (!String.IsNullOrEmpty(StartDateMoney))
                        {
                            ViewData["StartDateMoney"] = StartDateMoney;
                        }
                        else
                        {
                            ViewData["StartDateMoney"] = AllToolShare.GetDateNow();
                        }

                        //Lấy lại ngày hết hạn tính phí của thành viên            
                        string ExpiredDateMoney = Request.Form["ExpiredDateMoney"];
                        if (!String.IsNullOrEmpty(ExpiredDateMoney))
                        {
                            ViewData["ExpiredDateMoney"] = ExpiredDateMoney;
                        }
                        else
                        {
                            ViewData["ExpiredDateMoney"] = AllToolShare.GetDateAddYear(DateTime.Now.Date,1);
                        }
                    }

                    return View("EditUser", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Reset Pasword
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult RestPass(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string PageCurent= Request.Form["PageCurent"];
                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];

                if (!String.IsNullOrEmpty(memvar1))
                {
                    MembershipProvider _provider = Membership.Provider;
                    Guid UserId = Guid.Empty;
                    UserId = new Guid(memvar1);
                    UserModel OneUser = MembershipService.GetOneUserByUserId(UserId);
                    if (OneUser != null)
                    {
                        MembershipUser UserP = _provider.GetUser(OneUser.UserName, true);
                        UserP.UnlockUser();
                        MembershipService.ChangePassword(OneUser.UserName, UserP.ResetPassword(), "123456");
                    }
                }
                return RedirectToAction("UserManager/" + PageCurent.Trim() + "/" + LoaiNguoiDung, "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Khóa tài khoản người dùng
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult PreventUser(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string PageCurent = Request.Form["PageCurent"];
                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];

                if (!String.IsNullOrEmpty(memvar1))
                {
                    MembershipProvider _provider = Membership.Provider;
                    Guid UserId = Guid.Empty;
                    UserId = new Guid(memvar1);
                    UserModel OneUser = MembershipService.GetOneUserByUserId(UserId);
                    if (OneUser != null)
                    {
                        MembershipService.SaveEditPreventUser(OneUser.UserName, "Khoa");
                    }
                }
                return RedirectToAction("UserManager/" + PageCurent.Trim() + "/" + LoaiNguoiDung, "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Mở khóa người dùng
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult NotPreventUser(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string PageCurent = Request.Form["PageCurent"];
                string LoaiNguoiDung = Request.Form["LoaiNguoiDung"];

                if (!String.IsNullOrEmpty(memvar1))
                {
                    MembershipProvider _provider = Membership.Provider;
                    Guid UserId = Guid.Empty;
                    UserId = new Guid(memvar1);
                    UserModel OneUser = MembershipService.GetOneUserByUserId(UserId);
                    if (OneUser != null)
                    {
                        MembershipService.SaveEditPreventUser(OneUser.UserName, "MoKhoa");
                    }
                }
                return RedirectToAction("UserManager/" + PageCurent.Trim() + "/" + LoaiNguoiDung, "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Thông báo không có quyền sử dụng
        /// </summary>
        /// <returns></returns>
        public ActionResult Warning()
        {
            NoticeModel TheNotice = new NoticeModel();
            TheNotice.NoticeContent = "Bạn không được phép sử dụng chức năng này. Bạn liên hệ với quản trị để được cấp quyền sử dụng!";
            TheNotice.NoticeControler = "Home";
            TheNotice.NoticeAction = "index";
            return View("Notice", TheNotice);
        }

       /// <summary>
        /// Thông báo có lỗi xảy ra
       /// </summary>
       /// <param name="memvar1"></param>
       /// <param name="memvar2"></param>
       /// <param name="memvar3"></param>
       /// <returns></returns>
        public ActionResult ViewError(string memvar1, string memvar2, string memvar3)
        {
            ErrorModel TheError = new ErrorModel();
            TheError.ErrorContent = "Có lỗi xảy ra trong quá trình thực hiện. Hãy kiểm tra và thực hiện lại!";
            TheError.ErrorControler = memvar1;
            TheError.ErrorAction = memvar2;
            TheError.LeftMenuName = memvar3;
            return View("ViewError", TheError);
        }
    }
}
