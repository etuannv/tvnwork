using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TNV.Web.Models;
using System.Web.Routing;

namespace TNV.Web.Controllers
{
    public class ProDisController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }
        public ShareService AllToolShare { get; set; }
        public TinhHuyenService ToolTinhHuyen { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }
            if (AllToolShare == null) { AllToolShare = new ToolShareService(); }
            if (ToolTinhHuyen == null) { ToolTinhHuyen = new TinhHuyenClass(); }

            base.Initialize(requestContext);

            if (Request.IsAuthenticated)
            {
                if (User.IsInRole("AdminOfSystem"))
                {
                    UserModel ThanhVien = MembershipService.GetOneUserByUserName(User.Identity.Name);
                    ViewData["TenThanhVien"] = ThanhVien.FullName;
                    ViewData["LoaiThanhVien"] = ThanhVien.RoleDescription;
                    ViewData["SoLanDangNhap"] = ThanhVien.LoginNumber;
                    ViewData["KindMenu"] = "1";
                    //Hiển thị thông tin đăng nhập của quản trị
                    ViewData["Role"] = "1";
                }
                else
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else
            {
                RedirectToAction("Index", "Home");
            }
        }

        #region Quản lý danh sách tỉnh và thành phố
        /// <summary>
        /// Hiển thị danh sách tỉnh thành phố
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        public ActionResult ProvinceList()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<TinhThanhPhoModel> ListProvince = ToolTinhHuyen.DanhSachTinhThanhPho();
                return View("ProvinceList", ListProvince);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Thêm mới tỉnh, thành phố
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddProDis()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<TinhThanhPhoModel> ListProvince = ToolTinhHuyen.DanhSachTinhThanhPho();
                TinhThanhPhoModel NewProDis = new TinhThanhPhoModel();
                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (TinhThanhPhoModel Item in ListProvince)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = Item.ThuTuSapXep;
                    ListOrder.Add(OrderItem);
                }
                NewProDis.ThuTuSapXep = AllToolShare.GetMaxOrderby(ListOrder);

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (TinhThanhPhoModel ItemId in ListProvince)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.MaTinhTP;
                    ListID.Add(IdItem);
                }
                NewProDis.MaTinhTP = AllToolShare.GetRandomId(ListID, "T").ToString();

                return View("AddProvince", NewProDis);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu thêm mới một tỉnh thành
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddProDis(TinhThanhPhoModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolTinhHuyen.LuuMoiTinhThanhPho(model);
                    return RedirectToAction("ProvinceList", "ProDis");
                }
                else
                {
                    return View("AddProvince", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Đọc danh sách các trường của một huyện
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DanhSachCacTruong(string memvar1)
        {
            return Json(ToolTinhHuyen.AllSchool(memvar1));
        }

        /// <summary>
        /// Đọc danh sách cấp huyện của một tỉnh
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DanhSachCapHuyen(string memvar1)
        {
            return Json(ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(memvar1));
        }
        /// <summary>
        /// Sửa thông tin tỉnh, thành phố
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditProDis(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                TinhThanhPhoModel EditProDis = ToolTinhHuyen.DocMotTinhThanh(memvar1);

                return View("EditProvince", EditProDis);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Sửa thông tin một tỉnh thành
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditProDis(TinhThanhPhoModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolTinhHuyen.LuuSuaTinhThanhPho(model);
                    return RedirectToAction("ProvinceList", "ProDis");
                }
                else
                {
                    return View("EditProvince", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Xóa thông tin một tỉnh thành
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelProDis(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<HuyenThiXaModel> DSHuyenThi = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(memvar1);
                if (DSHuyenThi.Count == 0)
                {
                    ToolTinhHuyen.XoaMotTinhThanh(memvar1);
                }
                return RedirectToAction("ProvinceList", "ProDis");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Quản lý danh sách huyện
        public ActionResult DistrictList(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy mã tỉnh, thành phố
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                if (String.IsNullOrEmpty(MaTinhTP))
                {
                    if (!String.IsNullOrEmpty(memvar1))
                    {
                        MaTinhTP = memvar1;
                    }
                    else
                    {
                        MaTinhTP = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                    }
                }

                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);

                List<HuyenThiXaModel> ListDistrict = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                return View("DistrictList", ListDistrict);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Thêm mới huyện, thành phố, thị xã
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddDistrict()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<HuyenThiXaModel> ListDistrict = ToolTinhHuyen.DanhSachHuyenThiXa();
                HuyenThiXaModel NewDistrict = new HuyenThiXaModel();
                //Lấy số thứ thự
                List<ListFindMax> ListOrder = new List<ListFindMax>();
                foreach (HuyenThiXaModel Item in ListDistrict)
                {
                    ListFindMax OrderItem = new ListFindMax();
                    OrderItem.FieldFindMax = Item.ThuTuSapXep;
                    ListOrder.Add(OrderItem);
                }
                NewDistrict.ThuTuSapXep = AllToolShare.GetMaxOrderby(ListOrder);

                //Sinh ngẫu nhiên mã
                List<ListSearch> ListID = new List<ListSearch>();
                foreach (HuyenThiXaModel ItemId in ListDistrict)
                {
                    ListSearch IdItem = new ListSearch();
                    IdItem.FieldSearch = ItemId.MaHuyenThi;
                    ListID.Add(IdItem);
                }
                NewDistrict.MaHuyenThi = AllToolShare.GetRandomId(ListID, "HTX").ToString();


                //Lấy mã tỉnh, thành phố
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                if (String.IsNullOrEmpty(MaTinhTP))
                {
                    MaTinhTP = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                }

                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);

                return View("AddDistrict", NewDistrict);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu thêm mới một tỉnh thành
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddDistrict(HuyenThiXaModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolTinhHuyen.LuuMoiHuyenThiXa(model);

                    return RedirectToAction("DistrictList/" + model.MaTinhTP, "ProDis");
                }
                else
                {
                    //Tạo dữ liệu combobox chọn tỉnh cần xem
                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", model.MaTinhTP);
                    return View("AddDistrict", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Xóa thông tin một huyện, thành, thị
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelDistrict(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy mã tỉnh, thành phố
                string MaTinhTP = ToolTinhHuyen.MotHuyenThiXa(memvar1).MaTinhTP;
                ToolTinhHuyen.XoaHuyenThiXa(memvar1);
                return RedirectToAction("DistrictList/" + MaTinhTP, "ProDis");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Sửa thông tin huyện, thành phố, thị xã
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditDistrict(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                HuyenThiXaModel OneDistrict = ToolTinhHuyen.MotHuyenThiXa(memvar1);
                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", OneDistrict.MaTinhTP);
                return View("EditDistrict", OneDistrict);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Sửa thông tin một tỉnh thành
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditDistrict(HuyenThiXaModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                if (ModelState.IsValid)
                {
                    ToolTinhHuyen.LuuSuaHuyenThiXa(model);
                    return RedirectToAction("DistrictList/" + model.MaTinhTP, "ProDis");
                }
                else
                {
                    //Tạo dữ liệu combobox chọn tỉnh cần xem
                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", model.MaTinhTP);
                    return View("EditDistrict", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

        #region Quản lý danh sách trường học
        public ActionResult SchoolList(string memvar1, string memvar2)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy mã tỉnh, thành phố
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaTinhTPCu = (string)Request.Form["MaTinhCu"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];
                if (String.IsNullOrEmpty(MaTinhTP))
                {
                    if (!String.IsNullOrEmpty(memvar1))
                    {
                        MaTinhTP = memvar1;
                        MaHuyenThiXa = memvar2;
                    }
                    else
                    {
                        MaTinhTP = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                        MaHuyenThiXa = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinhTP).MaHuyenThi;
                    }
                }
                else
                {
                    if (MaTinhTP != MaTinhTPCu)
                    {
                        MaHuyenThiXa = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinhTP).MaHuyenThi;
                    }
                }

                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);
                ViewData["MaTinhTP"] = MaTinhTP;

                //Tạo dữ liệu combobox chọn huyện cần xem
                List<HuyenThiXaModel> DanhSachHuyen = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                ViewData["DsHuyenThiXa"] = new SelectList(DanhSachHuyen, "MaHuyenThi", "TenHuyenThi", MaHuyenThiXa);

                List<SchoolModel> ListSchool = ToolTinhHuyen.AllSchool(MaHuyenThiXa);

                return View("SchoolList", ListSchool);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Thêm mới một trường học
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult AddSchool()
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                List<SchoolModel> ListSchool = ToolTinhHuyen.AllSchool();

                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaTinhTPCu = (string)Request.Form["MaTinhCu"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];
                if (String.IsNullOrEmpty(MaTinhTP))
                {
                    MaTinhTP = ToolTinhHuyen.LayMotTinhDauTien().MaTinhTP;
                    MaHuyenThiXa = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinhTP).MaHuyenThi;
                }
                else
                {
                    if (MaTinhTP != MaTinhTPCu)
                    {
                        MaHuyenThiXa = ToolTinhHuyen.HuyenDauTienTrongTinh(MaTinhTP).MaHuyenThi;
                    }
                }

                //Đọc lại các giá trị khi Submit
                SchoolModel NewSchool = new SchoolModel();
                
                //Đọc lại mã trường học
                string SchoolId = (string)Request.Form["SchoolId"];
                if (!String.IsNullOrEmpty(SchoolId))
                {
                    NewSchool.SchoolId = SchoolId;
                }
                else
                {
                    //Sinh ngẫu nhiên mã
                    List<ListSearch> ListID = new List<ListSearch>();
                    foreach (SchoolModel ItemId in ListSchool)
                    {
                        ListSearch IdItem = new ListSearch();
                        IdItem.FieldSearch = ItemId.SchoolId;
                        ListID.Add(IdItem);
                    }
                    NewSchool.SchoolId = AllToolShare.GetRandomId(ListID, "TTH").ToString();
                }

                //Đọc lại tên trường học
                string SchoolName = (string)Request.Form["SchoolName"];
                if (!String.IsNullOrEmpty(SchoolName))
                {
                    NewSchool.SchoolName = SchoolName;
                }

                //Đọc lại thứ tự trường học
                string SchoolOrder = (string)Request.Form["SchoolOrder"];
                if (!String.IsNullOrEmpty(SchoolOrder))
                {
                    try
                    {
                        NewSchool.SchoolOrder = Convert.ToInt32(SchoolOrder);
                    }
                    catch
                    {
                        //Lấy số thứ thự
                        List<ListFindMax> ListOrder = new List<ListFindMax>();
                        foreach (SchoolModel Item in ListSchool)
                        {
                            ListFindMax OrderItem = new ListFindMax();
                            OrderItem.FieldFindMax = Item.SchoolOrder;
                            ListOrder.Add(OrderItem);
                        }
                        NewSchool.SchoolOrder = AllToolShare.GetMaxOrderby(ListOrder);
                    }
                }
                else
                {
                    //Lấy số thứ thự
                    List<ListFindMax> ListOrder = new List<ListFindMax>();
                    foreach (SchoolModel Item in ListSchool)
                    {
                        ListFindMax OrderItem = new ListFindMax();
                        OrderItem.FieldFindMax = Item.SchoolOrder;
                        ListOrder.Add(OrderItem);
                    }
                    NewSchool.SchoolOrder = AllToolShare.GetMaxOrderby(ListOrder);
                }

                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);
                ViewData["MaTinhTP"] = MaTinhTP;

                //Tạo dữ liệu combobox chọn huyện cần xem
                List<HuyenThiXaModel> DanhSachHuyen = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                ViewData["DsHuyenThiXa"] = new SelectList(DanhSachHuyen, "MaHuyenThi", "TenHuyenThi", MaHuyenThiXa);

                //Chuẩn hóa tên trường
                NewSchool.SchoolName = AllToolShare.ChuanHoaXauTiengViet(NewSchool.SchoolName);

                return View("AddSchool", NewSchool);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu thêm mới một trường học
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveAddSchool(SchoolModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];
                model.SchoolName = AllToolShare.ChuanHoaXauTiengViet(model.SchoolName);
                if (ModelState.IsValid)
                {
                    //Vì đặt biến MaHuyenThi khác với biến DistrictId của model
                    model.DistrictId = MaHuyenThiXa;
                    ToolTinhHuyen.SaveNewSchool(model);
                    return RedirectToAction("SchoolList/" + MaTinhTP + "/" + MaHuyenThiXa, "ProDis");
                }
                else
                {
                    //Tạo dữ liệu combobox chọn tỉnh cần xem
                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);
                    ViewData["MaTinhTP"] = MaTinhTP;

                    //Tạo dữ liệu combobox chọn huyện cần xem
                    List<HuyenThiXaModel> DanhSachHuyen = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                    ViewData["DsHuyenThiXa"] = new SelectList(DanhSachHuyen, "MaHuyenThi", "TenHuyenThi", MaHuyenThiXa);

                    return View("AddSchool", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Xóa thông tin một trường
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult DelSchool(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                //Lấy mã tỉnh, thành phố
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];
                ToolTinhHuyen.DelSchool(memvar1);
                return RedirectToAction("SchoolList/" + MaTinhTP + "/" + MaHuyenThiXa, "ProDis");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        /// <summary>
        /// Sửa thông tin huyện, thành phố, thị xã
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult EditSchool(string memvar1)
        {
            if (User.IsInRole("AdminOfSystem"))
            {

                SchoolModel OneSchool = new SchoolModel();
                if (!String.IsNullOrEmpty(memvar1))
                {
                    //Lần đầu tiên load form sửa thông tin
                    OneSchool = ToolTinhHuyen.GetOneSchool(memvar1);
                }
                else
                {
                    //Trường hợp Submit khi sửa
                    //Đọc lại mã trường học
                    string SchoolId = (string)Request.Form["SchoolId"];
                    if (!String.IsNullOrEmpty(SchoolId))
                    {
                        OneSchool.SchoolId = SchoolId;
                    }

                    //Đọc lại tên trường học
                    string SchoolName = (string)Request.Form["SchoolName"];
                    if (!String.IsNullOrEmpty(SchoolName))
                    {
                        OneSchool.SchoolName = SchoolName;
                    }

                    //Đọc lại thứ tự trường học
                    string SchoolOrder = (string)Request.Form["SchoolOrder"];
                    if (!String.IsNullOrEmpty(SchoolOrder))
                    {
                        try
                        {
                            OneSchool.SchoolOrder = Convert.ToInt32(SchoolOrder);
                        }
                        catch
                        {
                            //Bạn nhập sai thứ tự
                            OneSchool.SchoolOrder = ToolTinhHuyen.GetOneSchool(OneSchool.SchoolId).SchoolOrder;
                        }
                    }
                }
                
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];

                //Tạo dữ liệu combobox chọn tỉnh cần xem
                List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);
                ViewData["MaTinhTP"] = MaTinhTP;

                //Tạo dữ liệu combobox chọn huyện cần xem
                List<HuyenThiXaModel> DanhSachHuyen = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                ViewData["DsHuyenThiXa"] = new SelectList(DanhSachHuyen, "MaHuyenThi", "TenHuyenThi", MaHuyenThiXa);

                OneSchool.SchoolName = AllToolShare.ChuanHoaXauTiengViet(OneSchool.SchoolName);

                return View("EditSchool", OneSchool);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        /// <summary>
        /// Lưu sửa một trường học
        /// </summary>
        /// <returns></returns>
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SaveEditSchool(SchoolModel model)
        {
            if (User.IsInRole("AdminOfSystem"))
            {
                string MaTinhTP = (string)Request.Form["MaTinhTP"];
                string MaHuyenThiXa = (string)Request.Form["MaHuyenThi"];
                //Chuản hóa lại tên trường
                model.SchoolName = AllToolShare.ChuanHoaXauTiengViet(model.SchoolName);
                if (ModelState.IsValid)
                {
                    //Vì đặt biến MaHuyenThi khác với biến DistrictId của model
                    model.DistrictId = MaHuyenThiXa;
                    ToolTinhHuyen.SaveEditSchool(model);
                    return RedirectToAction("SchoolList/" + MaTinhTP + "/" + MaHuyenThiXa, "ProDis");
                }
                else
                {
                    //Tạo dữ liệu combobox chọn tỉnh cần xem
                    List<TinhThanhPhoModel> DanhSachTinh = ToolTinhHuyen.DanhSachTinhThanhPho();
                    ViewData["DsTinhTP"] = new SelectList(DanhSachTinh, "MaTinhTP", "TenTinhTP", MaTinhTP);
                    ViewData["MaTinhTP"] = MaTinhTP;

                    //Tạo dữ liệu combobox chọn huyện cần xem
                    List<HuyenThiXaModel> DanhSachHuyen = ToolTinhHuyen.DanhSachHuyenThiXaTheoTinh(MaTinhTP);
                    ViewData["DsHuyenThiXa"] = new SelectList(DanhSachHuyen, "MaHuyenThi", "TenHuyenThi", MaHuyenThiXa);

                    return View("EditSchool", model);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        #endregion

    }
}
