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
    public class SystemManagerController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public SystemManagerService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }
        public SystemManagerService ToolSystemManager { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new SystemManagerClass(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }
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

        #region Quản trị danh sách dạng toán
        /// <summary>
        /// Danh sách dạng toán
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        public ActionResult MathKindList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Khởi tạo trang
                int NumOfRecordInPage = 20;

                List<MathKindListModel> AllMathKindList = ToolSystemManager.GetMathKindList();

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "SystemManager";
                OnPage.Action = "MathKindList";
                OnPage.memvar2 = "";
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(AllMathKindList.Count, NumOfRecordInPage);

                if (String.IsNullOrEmpty(memvar1))
                {
                    string PageCurent = Request.Form["PageCurent"];
                    if (String.IsNullOrEmpty(PageCurent))
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
                ViewData["StartOrder"] = (OnPage.CurentPage - 1) * NumOfRecordInPage;
                return View("MathKindList", AllMathKindList.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Thêm mới một dạng toán
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AddMathKindList()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Khởi tạo đối tượng dạng toán
                MathKindListModel NewMathKindList = new MathKindListModel();

                //Đọc danh sách tất cả các dạng toán đã có
                List<MathKindListModel> AllMathKindList = ToolSystemManager.GetMathKindList();

                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (MathKindListModel Item in AllMathKindList)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = Item.MathKindListOrder;
                    ListOrder.Add(OrderItem);
                }
                NewMathKindList.MathKindListOrder = AllToolShare.GetMaxOrderby(ListOrder);

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (MathKindListModel ItemId in AllMathKindList)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.MathKindListId;
                    ListID.Add(IdItem);
                }
                NewMathKindList.MathKindListId = AllToolShare.GetRandomId(ListID, "DT").ToString();

                //Chuyển trang hiện tại để khi lưu xong dữ liệu có thể về trang cũ đang xem
                ViewData["PageCurent"] = Request.Form["PageCurent"];

                return View("AddMathKindList", NewMathKindList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu thêm mới một dạng toán
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddMathKindList(MathKindListModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    //Kiểm tra xem có lưu thành công hay không
                    string KetQua = ToolSystemManager.SaveNewMathKind(model);
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        string PageCurent = Request.Form["PageCurent"];
                        return RedirectToAction("MathKindList/" + PageCurent.Trim(), "SystemManager");
                    }
                    else
                    {
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        ViewData["Mes"] = KetQua;
                        return View("AddMathKindList", model);
                    }
                }
                else
                {
                    return View("AddMathKindList", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Sửa thông tin dạng toán
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditMathKindList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                MathKindListModel EditMathKindList = ToolSystemManager.GetOneMathKindList(memvar1);
                ViewData["PageCurent"] = Request.Form["PageCurent"];
                return View("EditMathKindList", EditMathKindList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu sửa khoảng thời gian
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditMathKindList(MathKindListModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    //Kiểm tra xem có lưu thành công hay không
                    string KetQua = ToolSystemManager.SaveEditMathKind(model);
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        string PageCurent = Request.Form["PageCurent"];
                        return RedirectToAction("MathKindList/" + PageCurent.Trim(), "SystemManager");
                    }
                    else
                    {
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        ViewData["Mes"] = KetQua;
                        return View("EditMathKindList", model);
                    }
                }
                else
                {
                    return View("EditMathKindList", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Xóa khoảng thời gian
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelMathKindList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string KetQua = ToolSystemManager.DelMathKind(memvar1);
                string PageCurent = Request.Form["PageCurent"];
                if (String.IsNullOrEmpty(KetQua))
                {
                    ViewData["Mes"] = "";
                }
                else
                {
                    ViewData["Mes"] = KetQua;
                }
                return RedirectToAction("MathKindList/" + PageCurent.Trim(), "SystemManager");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Quản trị danh sách khối lớp
        /// <summary>
        /// Danh sách khối lớp
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        public ActionResult ClassList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Khởi tạo trang
                int NumOfRecordInPage = 20;

                List<ClassListModel> AllClassList = ToolSystemManager.GetClassList();

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "SystemManager";
                OnPage.Action = "ClassList";
                OnPage.memvar2 = "";
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(AllClassList.Count, NumOfRecordInPage);

                if (String.IsNullOrEmpty(memvar1))
                {
                    string PageCurent = Request.Form["PageCurent"];
                    if (String.IsNullOrEmpty(PageCurent))
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
                ViewData["StartOrder"] = (OnPage.CurentPage - 1) * NumOfRecordInPage;
                return View("ClassList", AllClassList.Skip((OnPage.CurentPage - 1) * NumOfRecordInPage).Take(NumOfRecordInPage));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Thêm mới một khối lớp
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult AddClassList()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Khởi tạo đối tượng dạng toán
                ClassListModel NewClassList = new ClassListModel();

                //Đọc danh sách tất cả các dạng toán đã có
                List<ClassListModel> AllClassList = ToolSystemManager.GetClassList();

                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (ClassListModel Item in AllClassList)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = Item.ClassListOrder;
                    ListOrder.Add(OrderItem);
                }
                NewClassList.ClassListOrder = AllToolShare.GetMaxOrderby(ListOrder);

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (ClassListModel ItemId in AllClassList)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.ClassListId;
                    ListID.Add(IdItem);
                }
                NewClassList.ClassListId = AllToolShare.GetRandomId(ListID, "CLS").ToString();

                //Chuyển trang hiện tại để khi lưu xong dữ liệu có thể về trang cũ đang xem
                ViewData["PageCurent"] = Request.Form["PageCurent"];

                return View("AddClassList", NewClassList);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu thêm mới một khối lớp
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddClassList(ClassListModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    //Kiểm tra xem có lưu thành công hay không
                    string KetQua = ToolSystemManager.SaveNewClassList(model);
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        string PageCurent = Request.Form["PageCurent"];
                        return RedirectToAction("ClassList/" + PageCurent.Trim(), "SystemManager");
                    }
                    else
                    {
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        ViewData["Mes"] = KetQua;
                        return View("AddClassList", model);
                    }
                }
                else
                {
                    return View("AddClassList", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Sửa thông tin khối lớp
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditClassList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ClassListModel EditClassListItem = ToolSystemManager.GetOneClassList(memvar1);
                ViewData["PageCurent"] = Request.Form["PageCurent"];
                return View("EditClassList", EditClassListItem);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu sửa khoảng thời gian
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditClassList(ClassListModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    //Kiểm tra xem có lưu thành công hay không
                    string KetQua = ToolSystemManager.SaveEditClassList(model);
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        string PageCurent = Request.Form["PageCurent"];
                        return RedirectToAction("ClassList/" + PageCurent.Trim(), "SystemManager");
                    }
                    else
                    {
                        ViewData["PageCurent"] = Request.Form["PageCurent"];
                        ViewData["Mes"] = KetQua;
                        return View("EditClassList", model);
                    }
                }
                else
                {
                    return View("EditClassList", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Xóa khối lớp
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelClassList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string KetQua = ToolSystemManager.DelClassList(memvar1);
                string PageCurent = Request.Form["PageCurent"];
                if (String.IsNullOrEmpty(KetQua))
                {
                    ViewData["Mes"] = "";
                }
                else
                {
                    ViewData["Mes"] = KetQua;
                }
                return RedirectToAction("ClassList/" + PageCurent.Trim(), "SystemManager");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion
    }
}