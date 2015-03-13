using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Linq;
namespace TNV.Web.Models
{
    #region Models

    public class RolesModel
    {
        [DisplayName("Mã chức năng:")]
        public Guid RoleId { get; set; }

        [DisplayName("Tên chức năng:")]
        public string RoleName { get; set; }

        [DisplayName("Mô tả chức năng:")]
        public string Description { get; set; }
         
    }

    public class UserPropertyModel
    {
        [DisplayName("Tên đăng nhập:")]
        public string UserName { get; set; }

        [DisplayName("Tên đầy đủ:")]
        public string UserFullName { get; set; }

        [DisplayName("Mã tỉnh, thành phố:")]
        public string MaTinh { get; set; }

        [DisplayName("Mã huyện, thành phố, thị xã:")]
        public string MaHuyen { get; set; }

        [DisplayName("Ngày sinh thành viên:")]
        public DateTime NgaySinh { get; set; }

        [DisplayName("Số lần đăng nhập:")]
        public int LoginNumber { get; set; }

        [DisplayName("Ngày khởi tạo tài khoản:")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Ngày tham gia tính phí:")]
        public DateTime StartDate { get; set; }

        [DisplayName("Ngày hết hạn tính phí:")]
        public DateTime ExpiredDate { get; set; }

        [DisplayName("Ngăn chặn người dùng:")]
        public int Prevent { get; set; }

        [DisplayName("Học sinh của trường:")]
        public string SchoolId { get; set; }

        [DisplayName("Điện thoại liên hệ:")]
        public string PhoneNumber{ get; set; }

    }

    [PropertiesMustMatch("NewPassword", "ConfirmPassword", ErrorMessage = "The new password and confirmation password do not match.")]
    public class ChangePasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Current password")]
        public string OldPassword { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("New password")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm new password")]
        public string NewConfirmPassword { get; set; }
    }

    [PropertiesMustMatch("PassWord", "ConfirmPassWord", ErrorMessage = "The new password and confirmation password do not match.")]
    public class UserModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Nhập sai tên đăng nhập")]
        [DisplayName("Tên đăng nhập:")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nhập sai họ và tên")]
        [DisplayName("Họ và tên thành viên:")]
        public string FullName { get; set; }

        public string OldUserName { get; set; }

        [Required]
        [ValidatePasswordLength]
        [DataType(DataType.Password)]
        [DisplayName("Mật khẩu đăng nhập:")]
        public string PassWord { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Nhập lại mật khẩu đăng nhập:")]
        public string ConfirmPassWord { get; set; }
        
        [Required(ErrorMessage = "Nhập sai địa chỉ thư điện tử!")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Địa chỉ Email:")]
        public string Email { get; set; }

        [DataType(DataType.PhoneNumber)]
        [DisplayName("Điện thoại liên hệ:")]
        public string MobileAlias { get; set; }

        [DisplayName("Tên chức năng của người dùng:")]
        public string RoleNames { get; set; }

        [DisplayName("Chọn chức năng người dùng:")]
        public Guid RoleId { get; set; }

        [DisplayName("Mã chức năng người dùng:")]
        public string RoleDescription { get; set; }

        [DisplayName("Ngày khởi tạo tài khoản:")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Ngày đăng nhập cuối cùng:")]
        public DateTime LastLoginDate { get; set; }

        [DisplayName("Tỉnh, thành phố:")]
        public string MaTinh { get; set; }

        [DisplayName("Huyện, thành phố, thị xã:")]
        public string MaHuyen { get; set; }

        [DisplayName("Tên tỉnh, thành phố:")]
        public string TenTinh { get; set; }

        [DisplayName("Tên huyện, thành phố, thị xã:")]
        public string TenHuyen { get; set; }

        [DisplayName("Ngày sinh:")]
        public DateTime NgaySinh { get; set; }

        [DisplayName("Ngày tham gia tính phí:")]
        public DateTime StartDate { get; set; }

        [DisplayName("Ngày hết hạn tính phí:")]
        public DateTime ExpiredDate { get; set; }

        [DisplayName("Số lần đăng nhập:")]
        public int LoginNumber { get; set; }

        [DisplayName("Ngăn chặn người dùng:")]
        public int Prevent { get; set; }

        [DisplayName("Học sinh của trường:")]
        public string SchoolId { get; set; }

        [DisplayName("Tên trường học:")]
        public string SchoolName { get; set; }
    }

    public class LogOnModel
    {
        [Required]
        [DisplayName("User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Nhập sai tên đăng nhập")]
        [DisplayName("Tên đăng nhập:")]
        public string UserNames { get; set; }

        [Required(ErrorMessage = "Nhập sai họ và tên")]
        [DisplayName("Họ và tên thành viên:")]
        public string FullNames { get; set; }

        public string OldUserName { get; set; }

        [Required(ErrorMessage = "Nhập mật khẩu đăng nhập")]
        [DisplayName("Mật khẩu đăng nhập:")]
        public string PassWords { get; set; }

        [Required(ErrorMessage = "Nhập lại mật khẩu đăng nhập")]
        [DisplayName("Nhập lại mật khẩu đăng nhập:")]
        public string ConfirmPassWords { get; set; }

        [Required(ErrorMessage = "Nhập sai địa chỉ thư điện tử!")]
        [DisplayName("Địa chỉ Email:")]
        public string Email { get; set; }

        [DisplayName("Điện thoại liên hệ:")]
        public string MobileAlias { get; set; }

        [DisplayName("Tên chức năng của người dùng:")]
        public string RoleNames { get; set; }

        [DisplayName("Chọn chức năng người dùng:")]
        public Guid RoleId { get; set; }

        [DisplayName("Mã chức năng người dùng:")]
        public string RoleDescription { get; set; }

        [DisplayName("Ngày khởi tạo tài khoản:")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Ngày đăng nhập cuối cùng:")]
        public DateTime LastLoginDate { get; set; }

        [DisplayName("Tỉnh, thành phố:")]
        public string MaTinh { get; set; }

        [DisplayName("Huyện, thành phố, thị xã:")]
        public string MaHuyen { get; set; }

        [DisplayName("Tên tỉnh, thành phố:")]
        public string TenTinh { get; set; }

        [DisplayName("Tên huyện, thành phố, thị xã:")]
        public string TenHuyen { get; set; }

        [DisplayName("Ngày sinh:")]
        public DateTime NgaySinh { get; set; }

        [DisplayName("Ngày đăng ký thành viên:")]
        public DateTime RegistryDate { get; set; }

        [DisplayName("Ngày tham gia tính phí:")]
        public DateTime StartDate { get; set; }

        [DisplayName("Ngày hết hạn tính phí:")]
        public DateTime ExpiredDate { get; set; }

        [DisplayName("Số lần đăng nhập:")]
        public int LoginNumber { get; set; }

        [DisplayName("Ngăn chặn người dùng:")]
        public int Prevent { get; set; }

        [DisplayName("Học sinh của trường:")]
        public string SchoolId { get; set; }
    }
    #endregion

    #region Services
    // The FormsAuthentication type is sealed and contains static members, so it is difficult to
    // unit test code that calls its members. The interface and helper class below demonstrate
    // how to create an abstract wrapper around such a type in order to make the AccountController
    // code unit testable.

    public interface IMembershipService
    {
        int MinPasswordLength { get; }

        bool ValidateUser(string userName, string password);
        MembershipCreateStatus CreateUser(string userName, string password, string email);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        UserModel GetOneUserByUserName(string UserNameLogon);
        List<UserModel> GetAllUser();
        List<UserModel> GetAllUserByRolesName(string RolesName);
        List<UserModel> GetAllUserByCreatDate(string CreatDate);
        List<UserModel> GetAllUserByStartDate(string StartDate);
        List<UserModel> GetAllUserByExpiredDate(string ExpiredDate);
        List<UserModel> GetAllUserByPrevent(int Prevent);
        void CapNhatSoLanDangNhap(string UserNameLogon);
        List<RolesModel> DanhSachChucNang();
        RolesModel LayChucNangDauTien();
        RolesModel LayChucNang(string RoleName);
        void DelUserProperty(string id);
        void SaveEditUserProperty(UserPropertyModel model);
        void SaveNewUserProperty(UserPropertyModel model);
        void DelUser(Guid ID);
        UserModel GetOneUserByUserId(Guid UserId);
        void SaveEditMemberShip(Guid UserId, string Email);
        void SaveEditPreventUser(string UserName, string Khoa);
    }

    public class AccountMembershipService : IMembershipService
    {
        ToanThongMinhDataContext LinqContext = new ToanThongMinhDataContext();

        private readonly MembershipProvider _provider;

        public AccountMembershipService()
            : this(null)
        {
        }

        public AccountMembershipService(MembershipProvider provider)
        {
            _provider = provider ?? Membership.Provider;
        }

        public int MinPasswordLength
        {
            get
            {
                return _provider.MinRequiredPasswordLength;
            }
        }

        public bool ValidateUser(string userName, string password)
        {
           return _provider.ValidateUser(userName, password);
        }

        public MembershipCreateStatus CreateUser(string userName, string password, string email)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(password)) throw new ArgumentException("Value cannot be null or empty.", "password");
            if (String.IsNullOrEmpty(email)) throw new ArgumentException("Value cannot be null or empty.", "email");

            MembershipCreateStatus status;
            _provider.CreateUser(userName, password, email, null, null, true, null, out status);
            return status;
        }

        /// <summary>
        /// Đọc danh sách chức năng
        /// </summary>
        /// <returns></returns>
        public List<RolesModel> DanhSachChucNang()
        {
            List<RolesModel> DanhSachChucNang = (from DSChucNang in LinqContext.aspnet_Roles
                                                 select new RolesModel
                                                 {
                                                     RoleId = DSChucNang.RoleId,
                                                     RoleName = DSChucNang.RoleName,
                                                     Description = DSChucNang.Description
                                                 }).ToList<RolesModel>();
            return DanhSachChucNang;
        }
        /// <summary>
        /// Lấy chức năng người dùng đầu tiên
        /// </summary>
        /// <returns></returns>
        public RolesModel LayChucNangDauTien()
        {
            RolesModel ChucNang = (from DSChucNang in LinqContext.aspnet_Roles
                                                 select new RolesModel
                                                 {
                                                     RoleId = DSChucNang.RoleId,
                                                     RoleName = DSChucNang.RoleName,
                                                     Description = DSChucNang.Description
                                                 }).First<RolesModel>();
            return ChucNang;
        }
        /// <summary>
        /// Lấy chức năng bởi Tên của nó
        /// </summary>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        public RolesModel LayChucNang(string RoleName)
        {
            RolesModel ChucNang = (from DSChucNang in LinqContext.aspnet_Roles
                                   where DSChucNang.RoleName == RoleName
                                   select new RolesModel
                                   {
                                       RoleId = DSChucNang.RoleId,
                                       RoleName = DSChucNang.RoleName,
                                       Description = DSChucNang.Description
                                   }).Single<RolesModel>();
            return ChucNang;
        }
        /// <summary>
        /// Lấy thông tin một người dùng bởi Tên đăng nhập của nó
        /// </summary>
        /// <param name="UserNameLogon"> Tên đăng nhập</param>
        /// <returns></returns>RolesModel
        public UserModel GetOneUserByUserName(string UserNameLogon)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";
            
            UserModel ParentUser = (from UserList in LinqContext.aspnet_Users
                                    from InRole in LinqContext.aspnet_UsersInRoles
                                    from Role in LinqContext.aspnet_Roles
                                    from Memberships in LinqContext.aspnet_Memberships
                                    from UserPropertyList in LinqContext.UserProperties
                                    from DSHuyen in LinqContext.HuyenThanhThis
                                    from DSTinh in LinqContext.TinhThanhPhos
                                    where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && UserList.UserName == UserNameLogon && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId
                                    select new UserModel
                                    {
                                        UserId = UserList.UserId,
                                        UserName = UserList.UserName,
                                        OldUserName = UserList.UserName,
                                        MobileAlias = UserPropertyList.PhoneNumber,
                                        Email = Memberships.Email,
                                        RoleNames = Role.RoleName,
                                        RoleId = Role.RoleId,
                                        RoleDescription = Role.Description,
                                        CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                        ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                        FullName = UserPropertyList.UserFullName,
                                        LastLoginDate = Memberships.LastLoginDate,
                                        LoginNumber = (int)UserPropertyList.LoginNumber,
                                        MaHuyen = UserPropertyList.MaHuyen,
                                        MaTinh = UserPropertyList.MaTinh,
                                        TenTinh=DSTinh.TenTinhTP,
                                        TenHuyen=DSHuyen.TenHuyenThi,
                                        NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                        StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                        Prevent =(int) UserPropertyList.Prevent,
                                        SchoolId = UserPropertyList.SchoolId,
                                    }).SingleOrDefault<UserModel>();

            return ParentUser;
        }

        /// <summary>
        /// Lấy một thàn viên bởi Id của nó
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public UserModel GetOneUserByUserId(Guid UserId)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";

            UserModel ParentUser = (from UserList in LinqContext.aspnet_Users
                                    from InRole in LinqContext.aspnet_UsersInRoles
                                    from Role in LinqContext.aspnet_Roles
                                    from Memberships in LinqContext.aspnet_Memberships
                                    from UserPropertyList in LinqContext.UserProperties
                                    from DSHuyen in LinqContext.HuyenThanhThis
                                    from DSTinh in LinqContext.TinhThanhPhos
                                    where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && UserList.UserId == UserId && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId
                                    select new UserModel
                                    {
                                        UserId = UserList.UserId,
                                        UserName = UserList.UserName,
                                        OldUserName = UserList.UserName,
                                        MobileAlias = UserPropertyList.PhoneNumber,
                                        Email = Memberships.Email,
                                        RoleNames = Role.RoleName,
                                        RoleId = Role.RoleId,
                                        RoleDescription = Role.Description,
                                        CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                        ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                        FullName = UserPropertyList.UserFullName,
                                        LastLoginDate = Memberships.LastLoginDate,
                                        LoginNumber = (int)UserPropertyList.LoginNumber,
                                        MaHuyen = UserPropertyList.MaHuyen,
                                        MaTinh = UserPropertyList.MaTinh,
                                        TenTinh = DSTinh.TenTinhTP,
                                        TenHuyen = DSHuyen.TenHuyenThi,
                                        NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                        StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                        Prevent = (int)UserPropertyList.Prevent,
                                        SchoolId = UserPropertyList.SchoolId,
                                    }).SingleOrDefault<UserModel>();

            return ParentUser;
        }

        /// <summary>
        /// Đọc danh sách tất cả các thành viên
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUser()
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";

            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                    from InRole in LinqContext.aspnet_UsersInRoles
                                    from Role in LinqContext.aspnet_Roles
                                    from Memberships in LinqContext.aspnet_Memberships
                                    from UserPropertyList in LinqContext.UserProperties
                                    from DSHuyen in LinqContext.HuyenThanhThis
                                    from DSTinh in LinqContext.TinhThanhPhos
                                    where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId
                                    select new UserModel
                                    {
                                        UserId = UserList.UserId,
                                        UserName = UserList.UserName,
                                        OldUserName = UserList.UserName,
                                        MobileAlias = UserPropertyList.PhoneNumber,
                                        Email = Memberships.Email,
                                        RoleNames = Role.RoleName,
                                        RoleId = Role.RoleId,
                                        RoleDescription = Role.Description,
                                        CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                        ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                        FullName = UserPropertyList.UserFullName,
                                        LastLoginDate = Memberships.LastLoginDate,
                                        LoginNumber = (int)UserPropertyList.LoginNumber,
                                        MaHuyen = UserPropertyList.MaHuyen,
                                        MaTinh = UserPropertyList.MaTinh,
                                        TenTinh = DSTinh.TenTinhTP,
                                        TenHuyen = DSHuyen.TenHuyenThi,
                                        NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                        StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                        Prevent = (int)UserPropertyList.Prevent,
                                        SchoolId = UserPropertyList.SchoolId,
                                    }).ToList<UserModel>();

            return ListUser;
        }

        /// <summary>
        /// Đọc danh sách tất cả các thành viên của một Roles
        /// </summary>
        /// <returns></returns>
        public List<UserModel> GetAllUserByRolesName(string RolesName)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";
            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                        from InRole in LinqContext.aspnet_UsersInRoles
                                        from Role in LinqContext.aspnet_Roles
                                        from Memberships in LinqContext.aspnet_Memberships
                                        from UserPropertyList in LinqContext.UserProperties
                                        from DSHuyen in LinqContext.HuyenThanhThis
                                        from DSTinh in LinqContext.TinhThanhPhos
                                        where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId && Role.RoleName.ToLower().Trim() == RolesName.ToLower().Trim()
                                        select new UserModel
                                        {
                                            UserId = UserList.UserId,
                                            UserName = UserList.UserName,
                                            OldUserName = UserList.UserName,
                                            MobileAlias = UserPropertyList.PhoneNumber,
                                            Email = Memberships.Email,
                                            RoleNames = Role.RoleName,
                                            RoleId = Role.RoleId,
                                            RoleDescription = Role.Description,
                                            CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                            ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                            FullName = UserPropertyList.UserFullName,
                                            LastLoginDate = Memberships.LastLoginDate,
                                            LoginNumber = (int)UserPropertyList.LoginNumber,
                                            MaHuyen = UserPropertyList.MaHuyen,
                                            MaTinh = UserPropertyList.MaTinh,
                                            TenTinh = DSTinh.TenTinhTP,
                                            TenHuyen = DSHuyen.TenHuyenThi,
                                            NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                            StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                            Prevent = (int)UserPropertyList.Prevent,
                                            SchoolId = UserPropertyList.SchoolId,
                                        }).ToList<UserModel>();

            return ListUser;
        }

        /// <summary>
        /// Danh sách thành viên đăng ký của một ngày nào đó
        /// </summary>
        /// <param name="CreatDate"></param>
        /// <returns></returns>
        public List<UserModel> GetAllUserByCreatDate(string CreatDate)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";
            DateTime CreatedDate = Convert.ToDateTime(CreatDate, DateFormat);
            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                        from InRole in LinqContext.aspnet_UsersInRoles
                                        from Role in LinqContext.aspnet_Roles
                                        from Memberships in LinqContext.aspnet_Memberships
                                        from UserPropertyList in LinqContext.UserProperties
                                        from DSHuyen in LinqContext.HuyenThanhThis
                                        from DSTinh in LinqContext.TinhThanhPhos
                                        where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId && UserPropertyList.CreateDate==CreatedDate
                                        select new UserModel
                                        {
                                            UserId = UserList.UserId,
                                            UserName = UserList.UserName,
                                            OldUserName = UserList.UserName,
                                            MobileAlias = UserPropertyList.PhoneNumber,
                                            Email = Memberships.Email,
                                            RoleNames = Role.RoleName,
                                            RoleId = Role.RoleId,
                                            RoleDescription = Role.Description,
                                            CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                            ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                            FullName = UserPropertyList.UserFullName,
                                            LastLoginDate = Memberships.LastLoginDate,
                                            LoginNumber = (int)UserPropertyList.LoginNumber,
                                            MaHuyen = UserPropertyList.MaHuyen,
                                            MaTinh = UserPropertyList.MaTinh,
                                            TenTinh = DSTinh.TenTinhTP,
                                            TenHuyen = DSHuyen.TenHuyenThi,
                                            NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                            StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                            Prevent = (int)UserPropertyList.Prevent,
                                            SchoolId = UserPropertyList.SchoolId,
                                        }).ToList<UserModel>();

            return ListUser;
        }

        /// <summary>
        /// Danh sách người dùng theo ngày bắt đầu tính phí
        /// </summary>
        /// <param name="StartDate"></param>
        /// <returns></returns>
        public List<UserModel> GetAllUserByStartDate(string StartDate)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";
            DateTime DateStart = Convert.ToDateTime(StartDate, DateFormat);
            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                        from InRole in LinqContext.aspnet_UsersInRoles
                                        from Role in LinqContext.aspnet_Roles
                                        from Memberships in LinqContext.aspnet_Memberships
                                        from UserPropertyList in LinqContext.UserProperties
                                        from DSHuyen in LinqContext.HuyenThanhThis
                                        from DSTinh in LinqContext.TinhThanhPhos
                                        where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId && UserPropertyList.StartDate == DateStart
                                        select new UserModel
                                        {
                                            UserId = UserList.UserId,
                                            UserName = UserList.UserName,
                                            OldUserName = UserList.UserName,
                                            MobileAlias = UserPropertyList.PhoneNumber,
                                            Email = Memberships.Email,
                                            RoleNames = Role.RoleName,
                                            RoleId = Role.RoleId,
                                            RoleDescription = Role.Description,
                                            CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                            ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                            FullName = UserPropertyList.UserFullName,
                                            LastLoginDate = Memberships.LastLoginDate,
                                            LoginNumber = (int)UserPropertyList.LoginNumber,
                                            MaHuyen = UserPropertyList.MaHuyen,
                                            MaTinh = UserPropertyList.MaTinh,
                                            TenTinh = DSTinh.TenTinhTP,
                                            TenHuyen = DSHuyen.TenHuyenThi,
                                            NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                            StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                            Prevent = (int)UserPropertyList.Prevent,
                                            SchoolId = UserPropertyList.SchoolId,
                                        }).ToList<UserModel>();

            return ListUser;
        }

        /// <summary>
        /// Danh sách thành viên theo ngày hết hạn tính phí
        /// </summary>
        /// <param name="ExpiredDate"></param>
        /// <returns></returns>
        public List<UserModel> GetAllUserByExpiredDate(string ExpiredDate)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";
            DateTime DateExpired = Convert.ToDateTime(ExpiredDate, DateFormat);
            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                        from InRole in LinqContext.aspnet_UsersInRoles
                                        from Role in LinqContext.aspnet_Roles
                                        from Memberships in LinqContext.aspnet_Memberships
                                        from UserPropertyList in LinqContext.UserProperties
                                        from DSHuyen in LinqContext.HuyenThanhThis
                                        from DSTinh in LinqContext.TinhThanhPhos
                                        where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId && UserPropertyList.ExpiredDate == DateExpired
                                        select new UserModel
                                        {
                                            UserId = UserList.UserId,
                                            UserName = UserList.UserName,
                                            OldUserName = UserList.UserName,
                                            MobileAlias = UserPropertyList.PhoneNumber,
                                            Email = Memberships.Email,
                                            RoleNames = Role.RoleName,
                                            RoleId = Role.RoleId,
                                            RoleDescription = Role.Description,
                                            CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                            ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                            FullName = UserPropertyList.UserFullName,
                                            LastLoginDate = Memberships.LastLoginDate,
                                            LoginNumber = (int)UserPropertyList.LoginNumber,
                                            MaHuyen = UserPropertyList.MaHuyen,
                                            MaTinh = UserPropertyList.MaTinh,
                                            TenTinh = DSTinh.TenTinhTP,
                                            TenHuyen = DSHuyen.TenHuyenThi,
                                            NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                            StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                            Prevent = (int)UserPropertyList.Prevent,
                                            SchoolId = UserPropertyList.SchoolId,
                                        }).ToList<UserModel>();

            return ListUser;
        }

        /// <summary>
        /// Danh sách thành viên theo trạng thái bị chặn
        /// </summary>
        /// <param name="Prevent">Prevent - 0: Chặn; Prevent - 1: Hết chặn</param>
        /// <returns></returns>
        public List<UserModel> GetAllUserByPrevent(int Prevent)
        {
            System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
            DateFormat.ShortDatePattern = "dd/MM/yyyy";

            List<UserModel> ListUser = (from UserList in LinqContext.aspnet_Users
                                        from InRole in LinqContext.aspnet_UsersInRoles
                                        from Role in LinqContext.aspnet_Roles
                                        from Memberships in LinqContext.aspnet_Memberships
                                        from UserPropertyList in LinqContext.UserProperties
                                        from DSHuyen in LinqContext.HuyenThanhThis
                                        from DSTinh in LinqContext.TinhThanhPhos
                                        where UserPropertyList.MaTinh == DSTinh.MaTinhTP && UserPropertyList.MaHuyen == DSHuyen.MaHuyenThi && UserPropertyList.UserName == UserList.UserName && InRole.RoleId == Role.RoleId && InRole.UserId == UserList.UserId && Memberships.UserId == UserList.UserId && UserPropertyList.Prevent == Prevent
                                        select new UserModel
                                        {
                                            UserId = UserList.UserId,
                                            UserName = UserList.UserName,
                                            OldUserName = UserList.UserName,
                                            MobileAlias = UserPropertyList.PhoneNumber,
                                            Email = Memberships.Email,
                                            RoleNames = Role.RoleName,
                                            RoleId = Role.RoleId,
                                            RoleDescription = Role.Description,
                                            CreateDate = Convert.ToDateTime(UserPropertyList.CreateDate, DateFormat),
                                            ExpiredDate = Convert.ToDateTime(UserPropertyList.ExpiredDate, DateFormat),
                                            FullName = UserPropertyList.UserFullName,
                                            LastLoginDate = Memberships.LastLoginDate,
                                            LoginNumber = (int)UserPropertyList.LoginNumber,
                                            MaHuyen = UserPropertyList.MaHuyen,
                                            MaTinh = UserPropertyList.MaTinh,
                                            TenTinh = DSTinh.TenTinhTP,
                                            TenHuyen = DSHuyen.TenHuyenThi,
                                            NgaySinh = Convert.ToDateTime(UserPropertyList.NgaySinh, DateFormat),
                                            StartDate = Convert.ToDateTime(UserPropertyList.StartDate, DateFormat),
                                            Prevent = (int)UserPropertyList.Prevent,
                                            SchoolId = UserPropertyList.SchoolId,
                                        }).ToList<UserModel>();

            return ListUser;
        }


        #region Cập nhật các thuộc tính bổ sung của thành viên
        /// <summary>
        /// Cập nhật số lần đăng nhập
        /// </summary>
        /// <param name="model"></param>
        public void CapNhatSoLanDangNhap(string UserNameLogon)
        {
            UserModel ThanhVienDangNhap= GetOneUserByUserName(UserNameLogon);
            var ThanhVien = LinqContext.UserProperties.Single(p => p.UserName == UserNameLogon);
            ThanhVien.LoginNumber = (int)ThanhVien.LoginNumber+1;
            LinqContext.SubmitChanges();
        }
        
        /// <summary>
        /// Lưu mới thông tin người dùng
        /// </summary>
        /// <param name="model"></param>
        public void SaveNewUserProperty(UserPropertyModel model)
        {
            Table<UserProperty> NewUserProperty = LinqContext.GetTable<UserProperty>();
            UserProperty OneUserProperty = new UserProperty();
            OneUserProperty.ExpiredDate = model.ExpiredDate;
            OneUserProperty.LoginNumber = model.LoginNumber;
            OneUserProperty.MaHuyen = model.MaHuyen;
            OneUserProperty.MaTinh = model.MaTinh;
            OneUserProperty.CreateDate = model.CreateDate;
            OneUserProperty.NgaySinh = model.NgaySinh;
            OneUserProperty.Prevent = model.Prevent;
            OneUserProperty.StartDate = model.StartDate;
            OneUserProperty.UserFullName = model.UserFullName;
            OneUserProperty.UserName = model.UserName;
            OneUserProperty.SchoolId = model.SchoolId;
            OneUserProperty.PhoneNumber = model.PhoneNumber;
            NewUserProperty.InsertOnSubmit(OneUserProperty);
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Sửa thông tin người dùng
        /// </summary>
        /// <param name="model"></param>
        public void SaveEditUserProperty(UserPropertyModel model) 
        {
            var OneUserProperty = LinqContext.UserProperties.Single(p => p.UserName == model.UserName);
            OneUserProperty.MaHuyen = model.MaHuyen;
            OneUserProperty.MaTinh = model.MaTinh;
            OneUserProperty.NgaySinh = model.NgaySinh;
            OneUserProperty.StartDate = model.StartDate;
            OneUserProperty.ExpiredDate = model.ExpiredDate;
            OneUserProperty.UserFullName = model.UserFullName;
            OneUserProperty.PhoneNumber = model.PhoneNumber;
            OneUserProperty.SchoolId = model.SchoolId;
            LinqContext.SubmitChanges();
        }
        /// <summary>
        /// Khóa, mở khóa tài khoản người dùng
        /// </summary>
        /// <param name="UserName"></param>
        public void SaveEditPreventUser(string UserName, string Khoa)
        {
            var OneUserProperty = LinqContext.UserProperties.Single(p => p.UserName == UserName);
            if (Khoa == "Khoa")
            {
                OneUserProperty.Prevent = 0;
            }
            else
            {
                OneUserProperty.Prevent = 1;
            }
            LinqContext.SubmitChanges();
        }
        /// <summary>
        /// Chỉnh sửa địa chỉ Email
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Email"></param>
        public void SaveEditMemberShip(Guid UserId, string Email)
        {
            var MemberShip = LinqContext.aspnet_Memberships.Single(p => p.UserId == UserId);
            MemberShip.Email = Email;
            MemberShip.LoweredEmail = Email.ToLower();
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Xóa thông tin một người dùng
        /// </summary>
        /// <param name="id"></param>
        public void DelUserProperty(string id)
        {
            var OneUserProperty = from UserPropertyOne in LinqContext.UserProperties
                                  where UserPropertyOne.UserName == id
                                  select UserPropertyOne;
            LinqContext.UserProperties.DeleteAllOnSubmit(OneUserProperty);
            LinqContext.SubmitChanges();
        }

        /// <summary>
        /// Xóa một user
        /// </summary>
        /// <param name="ID"></param>
        public void DelUser(Guid ID)
        {
            var OneUser = from UserOne in LinqContext.aspnet_UsersInRoles
                          where UserOne.UserId == ID
                          select UserOne;
            LinqContext.aspnet_UsersInRoles.DeleteAllOnSubmit(OneUser);
            LinqContext.SubmitChanges();

            var OneUser1 = from UserOne1 in LinqContext.aspnet_Profiles
                          where UserOne1.UserId == ID
                          select UserOne1;
            LinqContext.aspnet_Profiles.DeleteAllOnSubmit(OneUser1);
            LinqContext.SubmitChanges();

            var OneUser2 = from UserOne2 in LinqContext.aspnet_Memberships
                           where UserOne2.UserId == ID
                           select UserOne2;
            LinqContext.aspnet_Memberships.DeleteAllOnSubmit(OneUser2);
            LinqContext.SubmitChanges();

            var OneUser3 = from UserOne3 in LinqContext.aspnet_PersonalizationPerUsers
                           where UserOne3.UserId == ID
                           select UserOne3;
            LinqContext.aspnet_PersonalizationPerUsers.DeleteAllOnSubmit(OneUser3);
            LinqContext.SubmitChanges();

            var OneUser4 = from UserOne4 in LinqContext.aspnet_Users
                           where UserOne4.UserId == ID
                           select UserOne4;
            LinqContext.aspnet_Users.DeleteAllOnSubmit(OneUser4);
            LinqContext.SubmitChanges();
        }

        #endregion

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");
            if (String.IsNullOrEmpty(oldPassword)) throw new ArgumentException("Value cannot be null or empty.", "oldPassword");
            if (String.IsNullOrEmpty(newPassword)) throw new ArgumentException("Value cannot be null or empty.", "newPassword");

            // The underlying ChangePassword() will throw an exception rather
            // than return false in certain failure scenarios.
            try
            {
                MembershipUser currentUser = _provider.GetUser(userName, true /* userIsOnline */);
                return currentUser.ChangePassword(oldPassword, newPassword);
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
        }
    }

    public interface IFormsAuthenticationService
    {
        void SignIn(string userName, bool createPersistentCookie);
        void SignOut();
    }

    public class FormsAuthenticationService : IFormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
        }

        public void SignOut()
        {
            FormsAuthentication.SignOut();
        }
    }

    #region CreatByUser
    
    #endregion

    #endregion

    #region Validation
    public static class AccountValidation
    {
        public static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class PropertiesMustMatchAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' and '{1}' do not match.";
        private readonly object _typeId = new object();

        public PropertiesMustMatchAttribute(string originalProperty, string confirmProperty)
            : base(_defaultErrorMessage)
        {
            OriginalProperty = originalProperty;
            ConfirmProperty = confirmProperty;
        }

        public string ConfirmProperty { get; private set; }
        public string OriginalProperty { get; private set; }

        public override object TypeId
        {
            get
            {
                return _typeId;
            }
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                OriginalProperty, ConfirmProperty);
        }

        public override bool IsValid(object value)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(value);
            object originalValue = properties.Find(OriginalProperty, true /* ignoreCase */).GetValue(value);
            object confirmValue = properties.Find(ConfirmProperty, true /* ignoreCase */).GetValue(value);
            return Object.Equals(originalValue, confirmValue);
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class ValidatePasswordLengthAttribute : ValidationAttribute
    {
        private const string _defaultErrorMessage = "'{0}' must be at least {1} characters long.";
        private readonly int _minCharacters = Membership.Provider.MinRequiredPasswordLength;

        public ValidatePasswordLengthAttribute()
            : base(_defaultErrorMessage)
        {
        }

        public override string FormatErrorMessage(string name)
        {
            return String.Format(CultureInfo.CurrentUICulture, ErrorMessageString,
                name, _minCharacters);
        }

        public override bool IsValid(object value)
        {
            string valueAsString = value as string;
            return (valueAsString != null && valueAsString.Length >= _minCharacters);
        }
    }
    #endregion

}
