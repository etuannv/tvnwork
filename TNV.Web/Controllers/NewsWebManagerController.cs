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
    public class NewsWebManagerController : Controller
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext(); 
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public NewsCategoryService ToolNewsCategory { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolNewsCategory == null) { ToolNewsCategory = new NewsCategoryClass(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }

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

        #region Quản lý danh sách chuyên mục
        /// <summary>
        /// Hiển thị danh sách các chuyên mục tin
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult NewsCategoryList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                int NumRecPerPage = 20;
                int page = 1;
                if (!String.IsNullOrEmpty(memvar1))
                {
                    page = int.Parse(memvar1);
                }
                else
                {
                    string pages = (string)Request.Form["PagesCurent"];
                    if (!String.IsNullOrEmpty(pages))
                    {
                        page = int.Parse(pages);
                    }
                    else
                    {
                        page = 1;
                    }
                }
                //Số thứ tự bắt đầu trong trang
                ViewData["RecordStart"] = (page - 1) * NumRecPerPage;

                //Lấy danh sách tất cả các chuyên mục tin từ cơ sở dữ liệu
                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();

                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "NewsWebManager";
                OnPage.Action = "NewsCategoryList";
                OnPage.memvar2 = "";
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachChuyenMuc.Count, NumRecPerPage);
                OnPage.CurentPage = page;
                ViewData["Pages"] = OnPage;
                ViewData["PagesCurent"] = OnPage.CurentPage;
                return View(DanhSachChuyenMuc.Skip((OnPage.CurentPage - 1) * NumRecPerPage).Take(NumRecPerPage));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
            
        }

        /// <summary>
        /// Thêm mới một chuyên mục tin
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddNewsCategory()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy danh sách các chuyên mục tin đã có
                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();

                //Chuyển trang hiện tại sang View
                ViewData["PagesCurent"] = (string)Request.Form["PagesCurent"];

                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (NewsCategoryModel ItemOder in DanhSachChuyenMuc)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = ItemOder.NewsCategoryOrder;
                    ListOrder.Add(OrderItem);
                }
                ViewData["NewsCategoryOrder"] = AllToolShare.GetMaxOrderby(ListOrder).ToString();

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (NewsCategoryModel ItemId in DanhSachChuyenMuc)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.NewsCategoryId;
                    ListID.Add(IdItem);
                }
                ViewData["NewsCategoryId"] = AllToolShare.GetRandomId(ListID, "Cate").ToString();

                return View("AddNewsCategory");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu mới một chuyên mục tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddNewsCategory(NewsCategoryModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolNewsCategory.SaveNewNewsCategory(model);
                    return RedirectToAction("NewsCategoryList/" + Request.Form["PagesCurent"].ToString().Trim(), "NewsWebManager");
                }
                else
                {
                    ViewData["Error"] = "Ghi chú: Không thể thêm mới chuyên mục này!";
                    ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                    ViewData["NewsCategoryId"] = Request.Form["NewsCategoryId"].ToString();
                    return View("AddNewsCategory", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Sửa một chuyên mục tin
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNewsCategory(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["Error"] = "";
                ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                return View(ToolNewsCategory.GetOneNewsCategory(memvar1));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu sửa một chuyên mục tin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SaveEditNewsCategory(NewsCategoryModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolNewsCategory.SaveEditNewsCategory(model);
                    return RedirectToAction("NewsCategoryList/" + Request.Form["PagesCurent"], "NewsWebManager");
                }
                else
                {
                    ViewData["Error"] = "Bạn không thể sửa chuyên mục này!";
                    ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                    return View("EditNewsCategory", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Xóa một chuyên mục tin
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DelNewsCategory(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Kiểm tra xem chuyên mục đã được sử dụng chưa, nếu chưa sử dụng thì xóa bỏ
                if (ToolNewsCategory.CategoryInUse(memvar1))
                {
                    ErrorsModel OneError = new ErrorsModel();
                    OneError.TextError = "Chuyên mục này vẫn còn chứa tin, bài nên không thể xóa!";
                    OneError.Controler = "NewsWebManager";
                    OneError.Action = "NewsCategoryList";
                    OneError.memvar1 = Request.Form["PagesCurent"];
                    OneError.memvar2 = "";
                    OneError.memvar3 = "";
                    OneError.memvar4 = "";
                    OneError.memvar5 = "";
                    return View("BaoLoiWebAdmin", OneError);
                }
                else
                {
                    ToolNewsCategory.DelNewsCategory(memvar1);
                    return RedirectToAction("NewsCategoryList/" + Request.Form["PagesCurent"], "NewsWebManager");
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion

        #region Quản lý danh sách bài viết

        [ValidateInput(false)]
        public ActionResult ListNews(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                int NumRecPerPage = 20;
                string pages = "";
                int page = 1;
                if (!String.IsNullOrEmpty(memvar1))
                {
                    page = int.Parse(memvar1);
                }
                else
                {
                    pages = (string)Request.Form["PagesCurent"];
                    if (!String.IsNullOrEmpty(pages))
                    {
                        page = int.Parse(pages);
                    }
                    else
                    {
                        page = 1;
                    }
                }
                //Số thứ tự bắt đầu trong trang
                ViewData["RecordStart"] = (page - 1) * NumRecPerPage;

                // Chuyên mục hiện tại theo phương thức POST hoặc lấy từ GET(Khi người dùng chọn trang hiển thị)
                string StartNewsCatId = Request.Form["NewsCatId"];
                if (String.IsNullOrEmpty(StartNewsCatId))
                {
                    if (!String.IsNullOrEmpty(memvar2))
                    {
                        StartNewsCatId = memvar2;
                    }
                }

                //Lấy danh sách tất cả các chuyên mục tin từ cơ sở dữ liệu
                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();

                if (String.IsNullOrEmpty(StartNewsCatId))
                {
                    foreach (NewsCategoryModel Item in DanhSachChuyenMuc)
                    {
                        StartNewsCatId = Item.NewsCategoryId;
                        break;
                    }
                }

                // Danh sách lựa chọn chuyên mục
                var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", StartNewsCatId);
                ViewData["NewsCategoryList"] = SelectList;

                //Đọc danh sách bài viết tương ứng với chuyên mục
                List<NewsContentModel> DanhSachBaiViet = ToolNewsCategory.GetListNewsContent(StartNewsCatId);

                //Lấy trang danh sách
                PagesModel OnPage = new PagesModel();
                OnPage.Controler = "NewsWebManager";
                OnPage.Action = "ListNews";
                OnPage.memvar2 = "";
                OnPage.memvar3 = "";
                OnPage.memvar4 = "";
                OnPage.memvar5 = "";
                OnPage.NumberPages = AllToolShare.GetNumberPages(DanhSachBaiViet.Count, NumRecPerPage);
                OnPage.CurentPage = page;
                ViewData["Pages"] = OnPage;
                ViewData["PagesCurent"] = OnPage.CurentPage;

                return View("ListNews", DanhSachBaiViet.Skip((page - 1) * NumRecPerPage).Take(NumRecPerPage));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        /// <summary>
        /// Thêm mới một tin bài
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddNewsItem()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy danh sách các chuyên mục tin đã có
                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();

                //Chuyển trang hiện tại sang View
                ViewData["PagesCurent"] = (string)Request.Form["PagesCurent"];

                //Lấy danh sách tin bài hiện tại 
                List<NewsContentModel> DanhSachTinBai = ToolNewsCategory.GetListNewsContent();

                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (NewsContentModel ItemOder in DanhSachTinBai)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = ItemOder.NewsOrder;
                    ListOrder.Add(OrderItem);
                }
                ViewData["NewsOrder"] = AllToolShare.GetMaxOrderby(ListOrder).ToString();

                // Chuyên mục hiện tại theo phương thức POST hoặc lấy từ GET(Khi người dùng chọn trang hiển thị)
                string StartNewsCatId = Request.Form["NewsCatId"];
                if (String.IsNullOrEmpty(StartNewsCatId))
                {
                    StartNewsCatId = Request.QueryString["NewsCatId"];
                }

                if (String.IsNullOrEmpty(StartNewsCatId))
                {
                    foreach (NewsCategoryModel Item in DanhSachChuyenMuc)
                    {
                        StartNewsCatId = Item.NewsCategoryId;
                        break;
                    }
                }

                // Danh sách lựa chọn chuyên mục
                var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", StartNewsCatId);
                ViewData["NewsCategoryList"] = SelectList;

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (NewsContentModel ItemId in DanhSachTinBai)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.NewsId;
                    ListID.Add(IdItem);
                }
                ViewData["NewsId"] = AllToolShare.GetRandomId(ListID, "News").ToString();

                return View("AddNewsItem");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu mới một tin bài
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddNewsItem(NewsContentModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    if (model.NewsImage != null)
                    {
                        if (model.NewsImage.ContentLength <= 209715200)
                        {
                            var fileName = Path.GetFileName(model.NewsImage.FileName);
                            var Extention = Path.GetExtension(model.NewsImage.FileName);
                            //Người dùng có chọn ảnh
                            if (Extention.ToLower() == ".png" || Extention.ToLower() == ".jpg" || Extention.ToLower() == ".gif")
                            {
                                //Chọn ảnh đúng định dạng cho phép
                                string ImageName = AllToolShare.GetRandomValue("News");
                                string path = Path.Combine(Server.MapPath("~/Content/UpLoads/ImageUploads/"), ImageName + Extention);
                                string Path1 = "/Content/UpLoads/ImageUploads/" + ImageName + Extention;
                                while (System.IO.File.Exists(path) == true)
                                {
                                    ImageName = AllToolShare.GetRandomValue("News");
                                    Path1 = "/Content/UpLoads/ImageUploads/" + ImageName + Extention;
                                    path = Path.Combine(Server.MapPath("~/Content/UpLoads/ImageUploads/"), ImageName);
                                }
                                model.NewsImage.SaveAs(path);
                                model.PathNewsImage = Path1;
                                ToolNewsCategory.SaveNewNewsContent(model);
                                return RedirectToAction("ListNews/" + Request.Form["PagesCurent"].ToString().Trim() + "/" + Request.Form["NewsCatId"], "NewsWebManager");
                            }
                            else
                            {
                                //Chọn ảnh không đúng định dạng cho phép
                                ViewData["Error"] = "Ghi chú: Chỉ chấp nhận các định dạng ảnh: .png, .jpg, .gif!";
                                ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                                //Lấy danh sách các chuyên mục tin đã có
                                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                                var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                                ViewData["NewsCategoryList"] = SelectList;
                                ViewData["NewsOrder"] = model.NewsOrder;
                                return View("AddNewsItem", model);
                            }
                        }
                        else
                        {
                            ViewData["Error"] = "Ghi chú: Dung lượng ảnh phải nhỏ hơn 201(MB)!";
                            ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                            ViewData["NewsId"] = Request.Form["NewsId"];
                            //Lấy danh sách các chuyên mục tin đã có
                            List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                            var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                            ViewData["NewsCategoryList"] = SelectList;
                            ViewData["NewsOrder"] = model.NewsOrder;
                            return View("AddNewsItem", model);
                        }
                    }
                    else
                    {
                        //Người dùng không chọn ảnh
                        model.PathNewsImage = "";
                        ToolNewsCategory.SaveNewNewsContent(model);
                        return RedirectToAction("ListNews/" + Request.Form["PagesCurent"].ToString().Trim() + "/" + Request.Form["NewsCatId"], "NewsWebManager");
                    }
                }
                else
                {
                    ViewData["Error"] = "Ghi chú: Không thể thêm mới tin, bài này!";
                    ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                    ViewData["NewsId"] = Request.Form["NewsId"];
                    //Lấy danh sách các chuyên mục tin đã có
                    List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                    var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                    ViewData["NewsCategoryList"] = SelectList;
                    ViewData["NewsOrder"] = model.NewsOrder;
                    return View("AddNewsItem", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Xóa một tin bài
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult DelNewsItem(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy file ảnh đại diện
                string Item = ToolNewsCategory.GetOneNewsContent(memvar1).PathNewsImage;
                if (!String.IsNullOrEmpty(Item))
                {
                    if (System.IO.File.Exists(Path.Combine(Server.MapPath("~" + Item))))
                    {
                        System.IO.File.Delete(Path.Combine(Server.MapPath("~" + Item)));
                    }
                }
                ToolNewsCategory.DelNewsContent(memvar1);
                return RedirectToAction("ListNews/" + Request.Form["PagesCurent"] + "/" + Request.Form["NewsCatId"], "NewsWebManager");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Sửa một chuyên mục tin
        /// </summary>
        /// <param name="memvar1"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditNewsItem(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                ViewData["Error"] = "";
                ViewData["PagesCurent"] = Request.Form["PagesCurent"];

                //Lấy tin bài cần sửa
                NewsContentModel EditItem = ToolNewsCategory.GetOneNewsContent(memvar1);
                ViewData["NewsImage"] = EditItem.PathNewsImage;
                //Chọn danh sách chuyên mục
                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", EditItem.NewsCatId);
                ViewData["NewsCategoryList"] = SelectList;

                return View(EditItem);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Lưu sửa một tin bài
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditNewsItem(NewsContentModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    if (model.NewsImage != null)
                    {
                        if (model.NewsImage.ContentLength <= 209715200)
                        {
                            var fileName = Path.GetFileName(model.NewsImage.FileName);
                            var Extention = Path.GetExtension(model.NewsImage.FileName);
                            //Người dùng có chọn ảnh
                            if (Extention.ToLower() == ".png" || Extention.ToLower() == ".jpg" || Extention.ToLower() == ".gif")
                            {
                                //Chọn ảnh đúng định dạng cho phép
                                string ImageName = AllToolShare.GetRandomValue("News");
                                string path = Path.Combine(Server.MapPath("~/Content/UpLoads/ImageUploads/"), ImageName + Extention);
                                string Path1 = "/Content/UpLoads/ImageUploads/" + ImageName + Extention;
                                while (System.IO.File.Exists(path) == true)
                                {
                                    ImageName = AllToolShare.GetRandomValue("News");
                                    Path1 = "/Content/UpLoads/ImageUploads/" + ImageName + Extention;
                                    path = Path.Combine(Server.MapPath("~/Content/UpLoads/ImageUploads/"), ImageName);
                                }
                                model.NewsImage.SaveAs(path);
                                model.PathNewsImage = Path1;
                                //Xóa bỏ ảnh cũ
                                string OldImage = Request.Form["NewsImageOld"];
                                if (!String.IsNullOrEmpty(OldImage))
                                {
                                    string pathImageOld = Path.Combine(Server.MapPath("~" + OldImage));
                                    if (System.IO.File.Exists(pathImageOld))
                                    {
                                        System.IO.File.Delete(pathImageOld);
                                    }
                                }
                                ToolNewsCategory.SaveEditNewsContent(model);
                                return RedirectToAction("ListNews/" + Request.Form["PagesCurent"].ToString().Trim() + "/" + Request.Form["NewsCatId"], "NewsWebManager");
                            }
                            else
                            {
                                //Chọn ảnh không đúng định dạng cho phép
                                ViewData["Error"] = "Ghi chú: Chỉ chấp nhận các định dạng ảnh: .png, .jpg, .gif!";
                                ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                                ViewData["NewsImage"] = Request.Form["NewsImageOld"];
                                //Lấy danh sách các chuyên mục tin đã có
                                List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                                var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                                ViewData["NewsCategoryList"] = SelectList;
                                return View("EditNewsItem", model);
                            }
                        }
                        else
                        {
                            ViewData["Error"] = "Ghi chú: Dung lượng ảnh phải nhỏ hơn 201(MB)";
                            ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                            //Lấy danh sách các chuyên mục tin đã có
                            List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                            var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                            ViewData["NewsCategoryList"] = SelectList;
                            ViewData["NewsImage"] = Request.Form["NewsImageOld"];
                            return View("EditNewsItem", model);

                        }
                    }
                    else
                    {
                        //Người dùng không chọn ảnh
                        model.PathNewsImage = Request.Form["NewsImageOld"];
                        ToolNewsCategory.SaveEditNewsContent(model);
                        return RedirectToAction("ListNews/" + Request.Form["PagesCurent"].ToString().Trim() + "/" + Request.Form["NewsCatId"], "NewsWebManager");
                    }
                }
                else
                {
                    ViewData["Error"] = "Ghi chú: Không thể sửa tin, bài này!";
                    ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                    //Lấy danh sách các chuyên mục tin đã có
                    List<NewsCategoryModel> DanhSachChuyenMuc = ToolNewsCategory.GetListNewsCategory();
                    var SelectList = new SelectList(DanhSachChuyenMuc, "NewsCategoryId", "NewsCategoryTitle", Request.Form["NewsCatId"]);
                    ViewData["NewsCategoryList"] = SelectList;
                    ViewData["NewsImage"] = Request.Form["NewsImageOld"];
                    return View("EditNewsItem", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Xem nội dung bài viết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ViewNewsItem(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy tin bài cần xem
                NewsContentModel ViewItem = ToolNewsCategory.GetOneNewsContent(memvar1);
                ViewData["PagesCurent"] = Request.Form["PagesCurent"];
                ViewData["NewsCatId"] = ViewItem.NewsCatId;
                ViewData["CatTitle"] = ToolNewsCategory.GetOneNewsCategory(ViewItem.NewsCatId).NewsCategoryTitle; ;

                return View(ViewItem);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}