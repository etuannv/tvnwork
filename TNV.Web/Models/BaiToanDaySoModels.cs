using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;
using System.Web.Security;
using System.Data.Linq;
using System.Text;

namespace TNV.Web.Models
{

    #region Định nghĩa các Models

    public class BaiToanDaySoModel
    {
        [DisplayName("Mã dãy số:")]
        public Guid MaDaySo { get; set; }

        [DisplayName("Nội dung dãy số:")]
        public string NoiDungDaySo { get; set; }

        [DisplayName("Số lượng phần tử:")]
        public int SoLuongPhanTu { get; set; }

        [DisplayName("Nội dung đáp án:")]
        public string NoiDungDapAn { get; set; }

        [DisplayName("Nội dung đáp án sai:")]
        public string NoiDungDapAnSai { get; set; }

        [DisplayName("Số lượng đáp án:")]
        public int SoLuongDapAn { get; set; }

        [DisplayName("Phân loại dãy số:")]
        public string PhanLoaiDaySo { get; set; }
        
        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int SapXepThuTu { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }
        
        [DisplayName("Câu hỏi hiển thị:")]
        public string CauHoiHienThi { get; set; }

        [DisplayName("Lời giải bài toán:")]
        public string LoiGiaiCauHoi { get; set; }

        [DisplayName("Kết luận bài toán:")]
        public string KetLuanCauHoi { get; set; }

        [DisplayName("Gọi ý trả lời:")]
        public string GoiYTraLoi { get; set; }
    }

    public class CleverMathKindModel
    {
        
        [DisplayName("Mã dạng toán thông minh:")]
        public string CleverMathKindId { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ClassListId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên dạng toán thông minh!")]
        [DisplayName("Tên dạng toán thông minh:")]
        public string CleverMathKindName { get; set; }

        [DisplayName("Thông tin dạng toán thông minh:")]
        public string CleverMathKindInfor { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int CleverMathKindOrder { get; set; }

    }

    public class CleverExerKindModel
    {

        [DisplayName("Mã loại bài tập toán thông minh:")]
        public string CleverExerKindId { get; set; }

        [DisplayName("Thuộc dạng toán thông minh:")]
        public string CleverMathKindId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên loại bài tập toán thông minh!")]
        [DisplayName("Tên loại bài tập toán thông minh:")]
        public string CleverExerKindName { get; set; }

        [DisplayName("Thông tin loại bài tập toán thông minh:")]
        public string CleverExerKindInfor { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int CleverExerKindOrder { get; set; }

    }

    public class CleverRangeModel
    {
        [DisplayName("Mã dãy số:")]
        public string CleverRangeId { get; set; }

        [DisplayName("Thuộc loại toán thông minh:")]
        public string CleverExerKindId { get; set; }

        [Required(ErrorMessage = "Phải nhập đáp số!")]
        [DisplayName("Đáp số:")]
        public string CleverAnswers { get; set; }

        [DisplayName("Dãy số:")]
        [Required(ErrorMessage = "Phải nhập dãy số!")]
        public string CleverRangeValue { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập thứ tự hiển thị!")]
        public int CleverRangeOrder { get; set; }

        [DisplayName("Độ khó:")]
        public string CleverRangeLever { get; set; }

        [DisplayName("Hướng dẫn:")]
        public string CleverRangeHelp { get; set; }
      
    }

    #endregion

    public interface BaiToanDaySoService
    {
        #region Quản lý danh sách dạng toán thông minh
        List<CleverMathKindModel> GetListCleverMathKind(string ClassListId);
        CleverMathKindModel GetOneCleverMathKind(string CleverMathKindId);
        string SaveNewCleverMathKind(CleverMathKindModel model);
        string SaveEditCleverMathKind(CleverMathKindModel model);
        string DelCleverMathKind(string id);
        CleverMathKindModel GetFirstCleverMathKind(string ClassListId);
        #endregion

        #region Quản lý danh sách loại bài tập toán thông minh
        List<CleverExerKindModel> GetListCleverExerKind(string CleverMathKindId);
        CleverExerKindModel GetOneCleverExerKind(string CleverExerKindId);
        string SaveNewCleverExerKind(CleverExerKindModel model);
        string SaveEditCleverExerKind(CleverExerKindModel model);
        string DelCleverExerKind(string id);
        #endregion

        #region Quản lý Dãy số thông minh
        List<CleverRangeModel> GetListCleverRange(string CleverExerKindId);
        CleverRangeModel GetOneCleverRange(string CleverRangeId);
        CleverRangeModel GetFirstCleverRange(string CleverExerKindId);
        string SaveNewCleverRange(CleverRangeModel model);
        string SaveEditCleverRange(CleverRangeModel model);
        string DelCleverRange(string id);
        string Arithmetic(bool ascending, int RangeCount, int u1, int d, char Key);
        string Geomatric(bool ascending, int RangeCount, int u1, int q, char Key);
        string BoSoNgauNhien(bool SapXepPhanTu, int SoBoTrongDay, int SoPhanTuTrongMoiBo, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int PhanTuDauTien, int CongSai_CongBoiTrongMoiBo, int CongSai_CongBoiGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay);
        string DaySoTongVaTich(string DayPhanTuDauTien, int SoPhanTuCuaDaySo, char Key, int KindOperator, bool ThuTuSapXep);
        string DaySoSinhTuCapSoCongHoacCapSoNhan(int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan, char Key);
        string DaySoChinhPhuongTuDaySo(bool ThuTuSapXep, int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, char Key);
        void LuuDaySoVaoCSDL(int LoaiBaiTap, string CleverExerKindId, char Key);
        string LayViTri(int DoDaiCuaDay, string SoPhanTuAn, char Key1, char Key2);
        List<BaiToanDaySoModel> TaoCacCapSoCong(int u1_min, int u1_max, int d_min, int d_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, char Key, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo, bool PhamViSinh, bool TatCaToHopAn);
        List<BaiToanDaySoModel> TaoCacBoSo(int u1_min, int u1_max, int dq_min, int dq_max, int dq_in_min, int dq_in_max, int SoPhanTu_In_min, int SoPhanTu_In_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        List<BaiToanDaySoModel> TaoCacDaySoTuCSCVaCSN(int u1_min, int u1_max, int dq_min, int dq_max, int SoPhanTu_min, int SoPhanTu_max, int LoaiDaySo, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan_min, int SoPhanTuThamGiaPhepToan_Max, int GioiHanPhanTu, char Key, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        #endregion

        #region Dãy số mới
        List<BaiToanDaySoModel> DanhSachDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        BaiToanDaySoModel DocDaySoDauTien(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        BaiToanDaySoModel GetOneDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        BaiToanDaySoModel DocMotDaySo(string MaDaySo);
        string ThemMoiMotDaySo(BaiToanDaySoModel DaySo);
        string SuaCauHoi(BaiToanDaySoModel DaySo);
        string XoaDaySo(string MaDaySo);
        string XoaNhieuDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo);
        #endregion

        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }

        
    }

    public class BaiToanDaySoClass : BaiToanDaySoService
    {
        public int SoBanGhiTrenMotTrang
        {
            get
            {
                return 30;
            }
        }
        public int BuocNhay
        {
            get
            {
                return 5;
            }
        }

        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext();

        #region Quản trị danh sách dạng toán thông minh
        /// <summary>
        /// Lấy danh sách tất cả các dạng toán thông minh
        /// </summary>
        /// <returns></returns>
        public List<CleverMathKindModel> GetListCleverMathKind(string ClassListId) 
        {
            List<CleverMathKindModel> ListCleverMathKind = new List<CleverMathKindModel>();
            if (String.IsNullOrEmpty(ClassListId))
            {
                ListCleverMathKind = (from ListCleverMathKindItem in ListData.CleverMathKinds
                                      orderby ListCleverMathKindItem.CleverMathKindOrder ascending
                                      select new CleverMathKindModel
                                                         {
                                                             CleverMathKindId = ListCleverMathKindItem.CleverMathKindId,
                                                             CleverMathKindInfor = ListCleverMathKindItem.CleverMathKindInfor,
                                                             CleverMathKindName = ListCleverMathKindItem.CleverMathKindName,
                                                             CleverMathKindOrder = (int)ListCleverMathKindItem.CleverMathKindOrder,
                                                             ClassListId = ListCleverMathKindItem.ClassListId,
                                                         }).ToList<CleverMathKindModel>();
            }
            else
            {
                ListCleverMathKind = (from ListCleverMathKindItem in ListData.CleverMathKinds
                                      where ListCleverMathKindItem.ClassListId == ClassListId
                                      orderby ListCleverMathKindItem.CleverMathKindOrder ascending
                                      select new CleverMathKindModel
                                      {
                                          CleverMathKindId = ListCleverMathKindItem.CleverMathKindId,
                                          CleverMathKindInfor = ListCleverMathKindItem.CleverMathKindInfor,
                                          CleverMathKindName = ListCleverMathKindItem.CleverMathKindName,
                                          CleverMathKindOrder = (int)ListCleverMathKindItem.CleverMathKindOrder,
                                          ClassListId = ListCleverMathKindItem.ClassListId,
                                      }).ToList<CleverMathKindModel>();
            }
            return ListCleverMathKind;
        }
        /// <summary>
        /// Lấy thông tin của dạng toán thông minh
        /// </summary>
        /// <returns></returns>
        public CleverMathKindModel GetOneCleverMathKind(string CleverMathKindId) 
        {
            CleverMathKindModel CleverMathKindItem = (from ListCleverMathKindItem in ListData.CleverMathKinds
                                                      where ListCleverMathKindItem.CleverMathKindId == CleverMathKindId
                                                      select new CleverMathKindModel
                                                 {
                                                     CleverMathKindId = ListCleverMathKindItem.CleverMathKindId,
                                                     CleverMathKindInfor = ListCleverMathKindItem.CleverMathKindInfor,
                                                     CleverMathKindName = ListCleverMathKindItem.CleverMathKindName,
                                                     CleverMathKindOrder = (int)ListCleverMathKindItem.CleverMathKindOrder,
                                                     ClassListId = ListCleverMathKindItem.ClassListId,
                                                 }).Single<CleverMathKindModel>();
            return CleverMathKindItem;
        }

        /// <summary>
        /// Lấy dạng toán thông minh đầu tiên theo khối lớp
        /// </summary>
        /// <param name="ClassListId"></param>
        /// <returns></returns>
        public CleverMathKindModel GetFirstCleverMathKind(string ClassListId)
        {
            CleverMathKindModel CleverMathKindItem = (from ListCleverMathKindItem in ListData.CleverMathKinds
                                                      where ListCleverMathKindItem.ClassListId == ClassListId
                                                      select new CleverMathKindModel
                                                      {
                                                          CleverMathKindId = ListCleverMathKindItem.CleverMathKindId,
                                                          CleverMathKindInfor = ListCleverMathKindItem.CleverMathKindInfor,
                                                          CleverMathKindName = ListCleverMathKindItem.CleverMathKindName,
                                                          CleverMathKindOrder = (int)ListCleverMathKindItem.CleverMathKindOrder,
                                                          ClassListId = ListCleverMathKindItem.ClassListId,
                                                      }).First<CleverMathKindModel>();
            return CleverMathKindItem;
        }
        /// <summary>
        /// Thêm mới một dạng toán thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveNewCleverMathKind(CleverMathKindModel model) 
        {
            try
            {
                Table<CleverMathKind> CleverMathKindList = ListData.GetTable<CleverMathKind>();
                CleverMathKind CleverMathKindItem = new CleverMathKind();
                CleverMathKindItem.CleverMathKindId = model.CleverMathKindId;
                CleverMathKindItem.CleverMathKindInfor = model.CleverMathKindInfor;
                CleverMathKindItem.CleverMathKindName = model.CleverMathKindName;
                CleverMathKindItem.CleverMathKindOrder =model.CleverMathKindOrder;
                CleverMathKindItem.ClassListId = model.ClassListId;
                CleverMathKindList.InsertOnSubmit(CleverMathKindItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới dạng toán thông minh này!";
            }
        }
        /// <summary>
        /// Sửa thông tin của một dạng toán thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditCleverMathKind(CleverMathKindModel model) 
        {
            try
            {
                var CleverMathKindItem = ListData.CleverMathKinds.Single(p => p.CleverMathKindId == model.CleverMathKindId);
                CleverMathKindItem.CleverMathKindInfor = model.CleverMathKindInfor;
                CleverMathKindItem.CleverMathKindName = model.CleverMathKindName;
                CleverMathKindItem.CleverMathKindOrder = model.CleverMathKindOrder;
                CleverMathKindItem.ClassListId = model.ClassListId;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được thông tin dạng toán thông minh này";
            }
        }

        /// <summary>
        /// Xóa một dạng toán thông minh
        /// </summary>
        /// <param name="id"></param>
        public string DelCleverMathKind(string id)
        {
            try
            {
                var OneCleverMathKind = from CleverMathKindItem in ListData.CleverMathKinds
                                        where CleverMathKindItem.CleverMathKindId == id
                                        select CleverMathKindItem;
                ListData.CleverMathKinds.DeleteAllOnSubmit(OneCleverMathKind);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được dạng toán thông minh này!";
            }
        }
        #endregion

        #region Quản trị danh sách loại bài tập toán thông minh
        /// <summary>
        /// Lấy danh sách tất cả các loại bài tập toán thông minh
        /// </summary>
        /// <returns></returns>
        public List<CleverExerKindModel> GetListCleverExerKind(string CleverMathKindId) 
        {
            List<CleverExerKindModel> ListCleverExerKind = new List<CleverExerKindModel>();
            if (String.IsNullOrEmpty(CleverMathKindId))
            {
                ListCleverExerKind = (from ListCleverExerKindItem in ListData.CleverExerKinds
                                      orderby ListCleverExerKindItem.CleverExerKindOrder ascending
                                      select new CleverExerKindModel
                                      {
                                          CleverMathKindId = ListCleverExerKindItem.CleverMathKindId,
                                          CleverExerKindInfor = ListCleverExerKindItem.CleverExerKindInfor,
                                          CleverExerKindName = ListCleverExerKindItem.CleverExerKindName,
                                          CleverExerKindOrder = (int)ListCleverExerKindItem.CleverExerKindOrder,
                                          CleverExerKindId = ListCleverExerKindItem.CleverExerKindId,
                                      }).ToList<CleverExerKindModel>();
            }
            else
            {
                ListCleverExerKind = (from ListCleverExerKindItem in ListData.CleverExerKinds
                                      where ListCleverExerKindItem.CleverMathKindId == CleverMathKindId
                                      orderby ListCleverExerKindItem.CleverExerKindOrder ascending
                                      select new CleverExerKindModel
                                      {
                                          CleverMathKindId = ListCleverExerKindItem.CleverMathKindId,
                                          CleverExerKindInfor = ListCleverExerKindItem.CleverExerKindInfor,
                                          CleverExerKindName = ListCleverExerKindItem.CleverExerKindName,
                                          CleverExerKindOrder = (int)ListCleverExerKindItem.CleverExerKindOrder,
                                          CleverExerKindId = ListCleverExerKindItem.CleverExerKindId,
                                      }).ToList<CleverExerKindModel>();
            }
            return ListCleverExerKind;
        }
        /// <summary>
        /// Lấy thông tin của loại bài tập toán thông minh
        /// </summary>
        /// <returns></returns>
        public CleverExerKindModel GetOneCleverExerKind(string CleverExerKindId) 
        {
            CleverExerKindModel CleverMathKindItem = (from ListCleverExerKindItem in ListData.CleverExerKinds
                                                      where ListCleverExerKindItem.CleverExerKindId == CleverExerKindId
                                                      select new CleverExerKindModel
                                                      {
                                                          CleverMathKindId = ListCleverExerKindItem.CleverMathKindId,
                                                          CleverExerKindInfor = ListCleverExerKindItem.CleverExerKindInfor,
                                                          CleverExerKindName = ListCleverExerKindItem.CleverExerKindName,
                                                          CleverExerKindOrder = (int)ListCleverExerKindItem.CleverExerKindOrder,
                                                          CleverExerKindId = ListCleverExerKindItem.CleverExerKindId,
                                                      }).Single<CleverExerKindModel>();
            return CleverMathKindItem;
        }
        /// <summary>
        /// Thêm mới một loại bài tập toán thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveNewCleverExerKind(CleverExerKindModel model) 
        {
            try
            {
                Table<CleverExerKind> CleverExerKindList = ListData.GetTable<CleverExerKind>();
                CleverExerKind CleverMathKindItem = new CleverExerKind();
                CleverMathKindItem.CleverMathKindId = model.CleverMathKindId;
                CleverMathKindItem.CleverExerKindInfor = model.CleverExerKindInfor;
                CleverMathKindItem.CleverExerKindName = model.CleverExerKindName;
                CleverMathKindItem.CleverExerKindOrder = (int)model.CleverExerKindOrder;
                CleverMathKindItem.CleverExerKindId = model.CleverExerKindId;
                CleverExerKindList.InsertOnSubmit(CleverMathKindItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới loại bài tập toán thông minh này!";
            }
        }
        /// <summary>
        /// Sửa thông tin của một loại bài tập toán thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditCleverExerKind(CleverExerKindModel model) 
        {
            try
            {
                var CleverMathKindItem = ListData.CleverExerKinds.Single(p => p.CleverExerKindId == model.CleverExerKindId);
                CleverMathKindItem.CleverMathKindId = model.CleverMathKindId;
                CleverMathKindItem.CleverExerKindInfor = model.CleverExerKindInfor;
                CleverMathKindItem.CleverExerKindName = model.CleverExerKindName;
                CleverMathKindItem.CleverExerKindOrder = (int)model.CleverExerKindOrder;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được thông tin loại bài tập toán thông minh này";
            }
        }

        /// <summary>
        /// Xóa một loại bài tập toán thông minh
        /// </summary>
        /// <param name="id"></param>
        public string DelCleverExerKind(string id)
        {
            try
            {
                var OneCleverExerKind = from CleverExerKindItem in ListData.CleverExerKinds
                                        where CleverExerKindItem.CleverExerKindId == id
                                        select CleverExerKindItem;
                ListData.CleverExerKinds.DeleteAllOnSubmit(OneCleverExerKind);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được loại bài tập toán thông minh này!";
            }
        }
        #endregion  

        #region Quản trị danh sách dãy số thông minh

        /// <summary>
        /// Lấy danh sách tất cả các dãy số thông minh
        /// </summary>
        /// <returns></returns>
        public List<CleverRangeModel> GetListCleverRange(string CleverExerKindId) 
        {
            List<CleverRangeModel> ListCleverRange = new List<CleverRangeModel>();
            if (String.IsNullOrEmpty(CleverExerKindId))
            {
                ListCleverRange = (from ListCleverRangeItem in ListData.CleverRanges
                                   orderby ListCleverRangeItem.CleverRangeOrder ascending
                                   select new CleverRangeModel
                                   {
                                       CleverRangeId = ListCleverRangeItem.CleverRangeId,
                                       CleverRangeValue = ListCleverRangeItem.CleverRangeValue,
                                       CleverAnswers = ListCleverRangeItem.CleverAnswers,
                                       CleverRangeOrder = (int)ListCleverRangeItem.CleverRangeOrder,
                                       CleverExerKindId = ListCleverRangeItem.CleverExerKindId,
                                       CleverRangeLever = ListCleverRangeItem.CleverRangeLever,
                                       CleverRangeHelp = ListCleverRangeItem.CleverRangeHelp
                                   }).ToList<CleverRangeModel>();
            }
            else
            {
                ListCleverRange = (from ListCleverRangeItem in ListData.CleverRanges
                                   where ListCleverRangeItem.CleverExerKindId == CleverExerKindId
                                   orderby ListCleverRangeItem.CleverRangeOrder ascending
                                   select new CleverRangeModel
                                      {
                                          CleverRangeId = ListCleverRangeItem.CleverRangeId,
                                          CleverRangeValue = ListCleverRangeItem.CleverRangeValue,
                                          CleverAnswers = ListCleverRangeItem.CleverAnswers,
                                          CleverRangeOrder = (int)ListCleverRangeItem.CleverRangeOrder,
                                          CleverExerKindId = ListCleverRangeItem.CleverExerKindId,
                                          CleverRangeLever = ListCleverRangeItem.CleverRangeLever,
                                          CleverRangeHelp = ListCleverRangeItem.CleverRangeHelp
                                      }).ToList<CleverRangeModel>();
            }
            return ListCleverRange;
        }

        /// <summary>
        /// Lấy thông tin của một dãy số thông minh
        /// </summary>
        /// <returns></returns>
        public CleverRangeModel GetOneCleverRange(string CleverRangeId) 
        {
            CleverRangeModel CleverRangeItem = (from ListCleverRangeItem in ListData.CleverRanges
                                                where ListCleverRangeItem.CleverRangeId == CleverRangeId
                                                select new CleverRangeModel
                                                   {
                                                       CleverRangeId = ListCleverRangeItem.CleverRangeId,
                                                       CleverRangeValue = ListCleverRangeItem.CleverRangeValue,
                                                       CleverAnswers = ListCleverRangeItem.CleverAnswers,
                                                       CleverRangeOrder = (int)ListCleverRangeItem.CleverRangeOrder,
                                                       CleverExerKindId = ListCleverRangeItem.CleverExerKindId,
                                                       CleverRangeLever = ListCleverRangeItem.CleverRangeLever,
                                                       CleverRangeHelp = ListCleverRangeItem.CleverRangeHelp
                                                   }).Single<CleverRangeModel>();
            return CleverRangeItem;
        }

        /// <summary>
        /// Lấy dãy số đầu tiên theo loại bài tập toán thông minh
        /// </summary>
        /// <param name="ClassListId"></param>
        /// <returns></returns>
        public CleverRangeModel GetFirstCleverRange(string CleverExerKindId) 
        {
            CleverRangeModel CleverRangeItem = (from ListCleverRangeItem in ListData.CleverRanges
                                                where ListCleverRangeItem.CleverExerKindId == CleverExerKindId
                                                select new CleverRangeModel
                                                   {
                                                       CleverRangeId = ListCleverRangeItem.CleverRangeId,
                                                       CleverRangeValue = ListCleverRangeItem.CleverRangeValue,
                                                       CleverAnswers = ListCleverRangeItem.CleverAnswers,
                                                       CleverRangeOrder = (int)ListCleverRangeItem.CleverRangeOrder,
                                                       CleverExerKindId = ListCleverRangeItem.CleverExerKindId,
                                                       CleverRangeLever = ListCleverRangeItem.CleverRangeLever,
                                                       CleverRangeHelp = ListCleverRangeItem.CleverRangeHelp
                                                   }).First<CleverRangeModel>();
            return CleverRangeItem;
        }

        /// <summary>
        /// Thêm mới một dãy số thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveNewCleverRange(CleverRangeModel model) 
        {
            try
            {
                Table<CleverRange> CleverRangeList = ListData.GetTable<CleverRange>();
                CleverRange CleverRangeItem = new CleverRange();
                CleverRangeItem.CleverRangeId = model.CleverRangeId;
                CleverRangeItem.CleverRangeValue = model.CleverRangeValue;
                CleverRangeItem.CleverAnswers = model.CleverAnswers;
                CleverRangeItem.CleverRangeOrder = model.CleverRangeOrder;
                CleverRangeItem.CleverExerKindId = model.CleverExerKindId;
                CleverRangeItem.CleverRangeLever = model.CleverRangeLever;
                CleverRangeItem.CleverRangeHelp = model.CleverRangeHelp;
                CleverRangeList.InsertOnSubmit(CleverRangeItem);
                
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới dãy số thông minh này!";
            }
        }

        /// <summary>
        /// Sửa thông tin của một dãy số thông minh
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditCleverRange(CleverRangeModel model) 
        {
            try
            {
                var CleverRangeItem = ListData.CleverRanges.Single(p => p.CleverRangeId == model.CleverRangeId);
                CleverRangeItem.CleverRangeId = model.CleverRangeId;
                CleverRangeItem.CleverRangeValue = model.CleverRangeValue;
                CleverRangeItem.CleverAnswers = model.CleverAnswers;
                CleverRangeItem.CleverRangeOrder = model.CleverRangeOrder;
                CleverRangeItem.CleverExerKindId = model.CleverExerKindId;
                CleverRangeItem.CleverRangeLever = model.CleverRangeLever;
                CleverRangeItem.CleverRangeHelp = model.CleverRangeHelp;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được thông tin dãy số thông minh này";
            }
        }

        /// <summary>
        /// Xóa một dãy số thông minh
        /// </summary>
        /// <param name="id"></param>
        public string DelCleverRange(string id)
        {
            try
            {
                var OneCleverRange = from CleverRangeItem in ListData.CleverRanges
                                        where CleverRangeItem.CleverRangeId == id
                                        select CleverRangeItem;
                ListData.CleverRanges.DeleteAllOnSubmit(OneCleverRange);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được dãy số thông minh này!";
            }
        }

        /// <summary>
        /// Sinh ngẫu nhiên một dãy số theo cấp số cộng
        /// </summary>
        /// <param name="ascending">Dãy tăng hay dãy giảm</param>
        /// <param name="RangeCount">Số phần tử của dãy số</param>
        /// <param name="u1">Số hạng đầu tiên, Nếu muốn sinh ngẫu nhiên thòi để giá trị -1</param>
        /// <param name="d">Công sai, Nếu muốn sinh ngẫu nhiên thòi để giá trị -1</param>
        /// <param name="Key">Ký tự phân cách giữa cacxs phần tử</param>
        /// <returns></returns>
        public string Arithmetic(bool ascending, int RangeCount, int u1, int d, char Key) 
        {
            //Cấp số cộng u(n)=u1+(n-1)*d trong đó: u1 là số hạng đầu tiên, d là công sai, n thứ tự phần tử
            string ReturnValue = "";
            Random RandomTool = new Random();

            //Số hạng đầu tiên 
            int SoHangDauTien = u1;
            
            //Công sai d
            int CongSai = d;
            
            if (ascending)
            {
                for (int i = 1; i <= RangeCount; i++)
                {
                    if (i == 1)
                    {
                        ReturnValue = (SoHangDauTien + (i - 1) * CongSai).ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + (SoHangDauTien + (i - 1) * CongSai).ToString().Trim();
                    }
                }
            }
            else
            {
                for (int i = RangeCount; i >=1 ; i--)
                {
                    if (i == RangeCount)
                    {
                        ReturnValue = (SoHangDauTien + (i - 1) * CongSai).ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + (SoHangDauTien + (i - 1) * CongSai).ToString().Trim();
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Lấy hướng dẫn dãy số cấp số cộng
        /// </summary>
        /// <param name="ascending">Dãy tăng hay dãy giảm</param>
        /// <param name="RangeCount">Số phần tử của dãy số</param>
        /// <param name="u1">Số hạng đầu tiên, Nếu muốn sinh ngẫu nhiên thòi để giá trị -1</param>
        /// <param name="d">Công sai, Nếu muốn sinh ngẫu nhiên thòi để giá trị -1</param>
        /// <param name="Key">Ký tự phân cách giữa cacxs phần tử</param>
        /// <returns></returns>
        public string HelpArithmetic(bool ascending, int RangeCount, int u1, int d, char Key)
        {
            //Cấp số cộng u(n)=u1+(n-1)*d trong đó: u1 là số hạng đầu tiên, d là công sai, n thứ tự phần tử
           string ReturnValue="";

            //Số hạng đầu tiên 
            int SoHangDauTien = u1;

            //Công sai d
            int CongSai = d;

            if (ascending)
            {
                ReturnValue = " Các số đã cho có quy luật: <b>Số đứng sau bằng số ngay trước nó cộng với số " + d.ToString().Trim() + " </b>. <br/> <b>Tức là:</b>";
                for (int i = 1; i <= RangeCount; i++)
                {
                    if (i == 1)
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + i.ToString().Trim() + " là: <b>" + SoHangDauTien+"</b>";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + i.ToString().Trim() + " là: <b>" + (SoHangDauTien + (i - 1) * CongSai).ToString().Trim()+" = "+(SoHangDauTien + (i - 2) * CongSai).ToString().Trim() + " + " + CongSai.ToString().Trim() + "</b>";
                    }
                }
            }
            else
            {
                ReturnValue = " Các số đã cho ở trên có quy luật: <b>Số đứng sau bằng số ngay trước nó trừ với số " + d.ToString().Trim() + " </b>. <br/> <b>Tức là:</b>";

                for (int i = RangeCount; i >= 1; i--)
                {
                    if (i == RangeCount)
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + (RangeCount-i+1).ToString().Trim() + " là: <b>" + (SoHangDauTien + (i - 1) * CongSai).ToString().Trim() + "</b>";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + (RangeCount - i + 1).ToString().Trim() + " là: <b>" + (SoHangDauTien + (i - 1) * CongSai).ToString().Trim() + " = " + (SoHangDauTien + i * CongSai).ToString().Trim() + " - " + CongSai.ToString().Trim() + "</b>";
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Lấy gới ý trả lời cấp số cộng
        /// </summary>
        /// <param name="ascending">Dãy tăng hay giảm</param>
        /// <param name="d">Công sai, Nếu muốn sinh ngẫu nhiên thòi để giá trị -1</param>
        /// <returns></returns>
        public string GoiYTraLoiCapSoCong(bool ascending, int d)
        {
            string ReturnValue = "";
            
            //Công sai d
            int CongSai = d;

            if (ascending)
            {
                ReturnValue = " Các số đã cho có quy luật: <b>Số đứng sau bằng số ngay trước nó cộng với số " + d.ToString().Trim() + " </b>.";
            }
            else
            {
                ReturnValue = " Các số đã cho ở trên có quy luật: <b>Số đứng sau bằng số ngay trước nó trừ với số " + d.ToString().Trim() + " </b>.";
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sinh một cấp số nhân
        /// </summary>
        /// <param name="ascending">Dãy tăng hay giảm</param>
        /// <param name="RangeCount">Số phần tử của dãy cần sinh</param>
        /// <returns>Sinh ngẫu nhiên một cấp số nhân</returns>
        /// <param name="u1">Số hạng đầu, nếu để sinh ngẫu nhiên thì mặc định 0</param>
        /// <param name="q">Công bội, nếu để sinh ngẫu nhiên mặc định 0</param>
        /// <param name="Key">Ký tự phân cách giữa cacxs phần tử</param>
        /// <returns></returns>
        public string Geomatric(bool ascending, int RangeCount, int u1, int q, char Key)
        {
            //Cấp số nhân u(n)=u1*q^(n-1) trong đó: u1 là số hạng đầu tiên, q là công bội, n thứ tự phần tử
            string ReturnValue = "";
            Random RandomTool = new Random();

            //Số hạng đầu tiên 
            int SoHangDauTien = u1;
            

            //Công bội q 
            int CongBoi = q;
            
            if (ascending)
            {
                for (int i = 1; i <= RangeCount; i++)
                {
                    if (i == 1)
                    {
                        ReturnValue = (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim();
                    }
                }
            }
            else
            {
                for (int i = RangeCount; i >= 1; i--)
                {
                    if (i == RangeCount)
                    {
                        ReturnValue = (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim();
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sinh ngẫu nhiên một cấp số nhân
        /// </summary>
        /// <param name="ascending">Dãy tăng hay giảm</param>
        /// <param name="RangeCount">Số phần tử của dãy cần sinh</param>
        /// <returns>Sinh ngẫu nhiên một cấp số nhân</returns>
        /// <param name="u1">Số hạng đầu, nếu để sinh ngẫu nhiên thì mặc định 0</param>
        /// <param name="q">Công bội, nếu để sinh ngẫu nhiên mặc định 0</param>
        /// <param name="Key">Ký tự phân cách giữa cacxs phần tử</param>
        /// <returns></returns>
        public string HelpGeomatric(bool ascending, int RangeCount, int u1, int q, char Key)
        {
            //Cấp số nhân u(n)=u1*q^(n-1) trong đó: u1 là số hạng đầu tiên, q là công bội, n thứ tự phần tử
            string ReturnValue = "";

            if (ascending)
            {
                ReturnValue = "- Trong các số trên, <b>Số đứng sau bằng số đứng trước nhân với số " + q.ToString().Trim() + "</b>";
            }
            else
            {
                ReturnValue = "- Trong các số trên, <b>Số đứng sau bằng số đứng trước chia cho số " + q.ToString().Trim() + "</b>";
            }

            ReturnValue = ReturnValue + "<br/>Như vậy:";

            //Số hạng đầu tiên 
            int SoHangDauTien = u1;

            //Công bội q 
            int CongBoi = q;

            if (ascending)
            {
                for (int i = 1; i <= RangeCount; i++)
                {
                    if (i == 1)
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + i.ToString().Trim() + " là: <b>" + SoHangDauTien.ToString().Trim() + "</b>";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + i.ToString().Trim() + " là: <b> " + (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim() + " = " + (SoHangDauTien * Math.Pow(CongBoi, i - 2)).ToString().Trim() + " x " + CongBoi.ToString().Trim() + "</b>";
                    }
                }
            }
            else
            {
                for (int i = RangeCount; i >= 1; i--)
                {
                    if (i == RangeCount)
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + (RangeCount - i + 1).ToString().Trim() + " là: <b>" + (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim() + "</b>";
                    }
                    else
                    {
                        ReturnValue = ReturnValue + "<br/>- Số thứ " + (RangeCount - i + 1).ToString().Trim() + " là: <b> " + (SoHangDauTien * Math.Pow(CongBoi, i - 1)).ToString().Trim() + " = " + (SoHangDauTien * Math.Pow(CongBoi, i)).ToString().Trim() + " : " + CongBoi.ToString().Trim() + "</b>";
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Lấy gợi ý trả lời cấp số nhân
        /// </summary>
        /// <param name="ascending">Dãy tăng hay giảm</param>
        /// <param name="q">Công bội, nếu để sinh ngẫu nhiên mặc định 0</param>
        /// <returns></returns>
        public string GoiYTraLoiCapSoNhan(bool ascending, int q)
        {
            string ReturnValue = "";

            if (ascending)
            {
                ReturnValue = "- Trong các số trên, <b>Số đứng sau bằng số đứng trước nhân với số " + q.ToString().Trim() + "</b>";
            }
            else
            {
                ReturnValue = "- Trong các số trên, <b>Số đứng sau bằng số đứng trước chia cho số " + q.ToString().Trim() + "</b>";
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sinh ngẫu nhiên dãy gồm các bộ số có quy luật
        /// </summary>
        /// <param name="SapXepPhanTu">Thứ tự sắp xếp các bộ số: True- Tăng; False - Giảm</param>
        /// <param name="SoBoTrongDay">Số bộ trong dãy số</param>
        /// <param name="SoPhanTuTrongMoiBo">Số phần tử trong mỗi bộ số</param>
        /// <param name="QuLuatTrongMoiBo">Quy luật trong mỗi bộ số: 1 - Cấp số cộng; 2- Cấp số nhân</param>
        /// <param name="QuyLuatGiuaCacBo">Quy luật giữa các bộ số: 1- Cấp số cộng, 2- Cấp số nhân</param>
        /// <param name="QuyTacTinhPhanTuDauTien">Quy tắc tính phần tử đầu tiên: 1- Số đầu tiên của bộ tiếp theo là tổng các số trong bộ trước nó; 2- Số đầu tiên của bộ tiếp theo là tích các số trong bộ trước nó</param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của bộ số đầu tiên</param>
        /// <param name="CongSai_CongBoiTrongMoiBo">Công sai hoặc công bội trong mỗi bộ số</param>
        /// <param name="CongSai_CongBoiGiuaCacBo">Công sai hoặc công bội giữa các bộ số</param>
        /// <param name="Key">Ký tự dùng để ngăn cách các phần tử trong mỗi bộ số, hoặc giữa các bộ số</param>
        /// <returns></returns>
        public string BoSoNgauNhien(bool SapXepPhanTu, int SoBoTrongDay, int SoPhanTuTrongMoiBo, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int PhanTuDauTien, int CongSai_CongBoiTrongMoiBo, int CongSai_CongBoiGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay )
        {
            // Sinh ngẫu nhiên các bộ số dạng: 
            //     *8: Bộ hai số có tổng của bộ thứ nhất là phần tử đầu tiên của bộ thứ 2, các phần tử trong mỗi bộ có quy luật chung: (1,3), (4,6), (10,12)
            //     *9: Bộ ba số có tổng của bộ thứ nhất là phần tử đầu tiên của bộ thứ 2, các phần tử trong mỗi bộ có quy luật chung: (1, 3, 5), (9, 11, 13), (33, 35, 37)
            //     *10: Bộ hai số có tổng của bộ thứ nhất là phần tử đầu tiên của bộ thứ 2, các phần tử trong mỗi bộ có quy luật chung: (1,2), (3,6), (9, 18)
            //     *11: Bộ ba số có tích của bộ thứ nhất là phần tử đầu tiên của bộ thứ 2, các phần tử trong mỗi bộ có quy luật chung: (1, 2, 4), (7, 14, 28), (49, 98, 196)
            //     *12: Dãy gồm các bộ số có hai số, tổng của bộ thứ nhất và số hạng đầu của bộ thứ 2 có quy luật nhân với một số, các phần tử trong mỗi bộ có quy luật chung: (1,3), (8,10), (36, 38)
            //     *13: Dãy gồm các bộ số có ba số, tổng của bộ thứ nhất và số hạng đầu của bộ thứ 2 có quy luật nhân với một số, các phần tử trong mỗi bộ có quy luật chung: (1, 3, 5), (18, 20, 22), (120, 122, 124)
            string ReturnValue = "";

            int PhanTuDauTienTrongBoSoDauTien = PhanTuDauTien;

            string BoSoTruoc = "";

            //Vòng lặp để sinh ra các bộ số của dãy số
            for (int i = 1; i <= SoBoTrongDay; i++)
            {
                if (i == 1)
                {
                    if (QuLuatTrongMoiBo == 1) //Cấp số cộng trong mỗi bộ số
                    {
                        BoSoTruoc = Arithmetic(SapXepPhanTu, SoPhanTuTrongMoiBo, PhanTuDauTien, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                        ReturnValue = "(" + BoSoTruoc + ")";
                    }
                    else //Cấp số nhân trong mỗi bộ số
                    {
                        BoSoTruoc = Geomatric( SapXepPhanTu, SoPhanTuTrongMoiBo, PhanTuDauTien, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                        ReturnValue = "(" + Geomatric(SapXepPhanTu, SoPhanTuTrongMoiBo, PhanTuDauTien, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo) + ")";
                    }
                }
                else
                {
                    if (QuLuatTrongMoiBo == 1) //Cấp số cộng trong mỗi bộ số
                    {
                        if (QuyTacTinhPhanTuDauTien == 1) //1- Số đầu tiên của bộ tiếp theo là tổng các số trong bộ trước nó;
                        {
                            if (QuyLuatGiuaCacBo == 1) //Phần tử đầu tiên tính được đem cộng với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Arithmetic(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 1) + CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                            else //Phần tử đầu tiên tính được đem nhân với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Arithmetic(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 1) * CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                            
                        }
                        else //2- Số đầu tiên của bộ tiếp theo là tích các số trong bộ trước nó;
                        {
                            if (QuyLuatGiuaCacBo == 1) //Phần tử đầu tiên tính được đem cộng với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Arithmetic(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 2) + CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                            else //Phần tử đầu tiên tính được đem nhân với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Arithmetic( SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 2) * CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                        }
                        ReturnValue = ReturnValue + KeyTrongDay + "(" + BoSoTruoc + ")";
                    }
                    else //Cấp số nhân trong mỗi bộ số
                    {
                        if (QuyTacTinhPhanTuDauTien == 1) //1- Số đầu tiên của bộ tiếp theo là tổng các số trong bộ trước nó;
                        {
                            if (QuyLuatGiuaCacBo == 1) //Phần tử đầu tiên tính được đem cộng với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Geomatric(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 1) + CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                            else //Phần tử đầu tiên tính được đem nhân với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Geomatric(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 1) * CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                        }
                        else //2- Số đầu tiên của bộ tiếp theo là tích các số trong bộ trước nó;
                        {
                            if (QuyLuatGiuaCacBo == 1) //Phần tử đầu tiên tính được đem cộng với CongSai_CongBoiGiuaCacBo
                            {
                                BoSoTruoc = Geomatric(SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 2) + CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                            else
                            {
                                BoSoTruoc = Geomatric( SapXepPhanTu, SoPhanTuTrongMoiBo, TinhTongHoacTichCuaBoSo(BoSoTruoc, KeyTrongMoiBo, 2) * CongSai_CongBoiGiuaCacBo, CongSai_CongBoiTrongMoiBo, KeyTrongMoiBo);
                            }
                        }
                        ReturnValue = ReturnValue + KeyTrongDay + "(" + BoSoTruoc + ")";
                    }
                }
            }

            if (SapXepPhanTu)
            {
                return ReturnValue;
            }
            else
            {
                return DaoDay(ReturnValue, KeyTrongDay);
            }
        }

        /// <summary>
        /// Tạo hướng dẫn giải dãy gồm các bộ số có quy luật
        /// </summary>
        /// <param name="SapXepPhanTu">Thứ tự sắp xếp các bộ số: True- Tăng; False - Giảm</param>
        /// <param name="SoBoTrongDay">Số bộ trong dãy số</param>
        /// <param name="SoPhanTuTrongMoiBo">Số phần tử trong mỗi bộ số</param>
        /// <param name="QuLuatTrongMoiBo">Quy luật trong mỗi bộ số: 1 - Cấp số cộng; 2- Cấp số nhân</param>
        /// <param name="QuyLuatGiuaCacBo">Quy luật giữa các bộ số: 1- Cấp số cộng, 2- Cấp số nhân</param>
        /// <param name="QuyTacTinhPhanTuDauTien">Quy tắc tính phần tử đầu tiên: 1- Số đầu tiên của bộ tiếp theo là tổng các số trong bộ trước nó; 2- Số đầu tiên của bộ tiếp theo là tích các số trong bộ trước nó</param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của bộ số đầu tiên</param>
        /// <param name="CongSai_CongBoiTrongMoiBo">Công sai hoặc công bội trong mỗi bộ số</param>
        /// <param name="CongSai_CongBoiGiuaCacBo">Công sai hoặc công bội giữa các bộ số</param>
        /// <param name="Key">Ký tự dùng để ngăn cách các phần tử trong mỗi bộ số, hoặc giữa các bộ số</param>
        /// <returns></returns>
        public string HelpBoSoNgauNhien(bool SapXepPhanTu, int SoBoTrongDay, int SoPhanTuTrongMoiBo, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int PhanTuDauTien, int CongSai_CongBoiTrongMoiBo, int CongSai_CongBoiGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay)
        {
            int PhanTuDauTienTrongBoSoDauTien = PhanTuDauTien;

            //Vòng lặp để sinh ra các bộ số của dãy số
            string HelpReturn="";
            
            //Lấy quy luật giữa các phần tử của một bộ
            if (QuLuatTrongMoiBo == 1)
            {
                HelpReturn = "- Trong mỗi một bộ số, các số có quy luật: <b>Số đứng sau bằng số đứng ngay trước nó cộng với số " + CongSai_CongBoiTrongMoiBo.ToString().Trim() + " </b>.";
            }
            else
            {
                HelpReturn = "- Trong mỗi một bộ số, các số có quy luật: <b>Số đứng sau bằng số đứng ngay trước nó nhân với số " + CongSai_CongBoiTrongMoiBo.ToString().Trim() + " </b>.";
            }

            //Lấy quy luật giữa các bộ của dãy
            if (QuyLuatGiuaCacBo == 1) //Phần tử đầu tiên tính được đem cộng với CongSai_CongBoiGiuaCacBo
            {
                if (QuyTacTinhPhanTuDauTien == 1) //1- Số đầu tiên của bộ tiếp theo là tổng các số trong bộ trước nó;
                {
                    if (CongSai_CongBoiGiuaCacBo != 0)
                    {
                        HelpReturn =HelpReturn+ "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tổng các số trong bộ số đứng ngay trước nó cộng với số " + CongSai_CongBoiGiuaCacBo.ToString().Trim() + " </b>.";
                    }
                    else
                    {
                        HelpReturn =HelpReturn+ "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tổng các số trong bộ số đứng ngay trước nó</b>.";
                    }
                }
                else
                {
                    if (CongSai_CongBoiGiuaCacBo != 0)
                    {
                        HelpReturn =HelpReturn+ "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tích các số trong bộ số đứng ngay trước nó cộng với số " + CongSai_CongBoiGiuaCacBo.ToString().Trim() + " </b>.";
                    }
                    else
                    {
                        HelpReturn =HelpReturn+ "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tích các số trong bộ số đứng ngay trước nó</b>.";
                    }
                }
            }
            else //Phần tử đầu tiên tính được đem nhân với CongSai_CongBoiGiuaCacBo 
            {
                if (QuyTacTinhPhanTuDauTien == 1)  //2- Số đầu tiên của bộ tiếp theo là tích các số trong bộ trước nó;
                {
                    if (CongSai_CongBoiGiuaCacBo != 1)
                    {
                        HelpReturn = HelpReturn + "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tổng các số trong bộ số đứng ngay trước nó nhân với số " + CongSai_CongBoiGiuaCacBo.ToString().Trim() + " </b>.";
                    }
                    else
                    {
                        HelpReturn = HelpReturn + "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tổng các số trong bộ số đứng ngay trước nó</b>.";
                    }

                }
                else
                {
                    if (CongSai_CongBoiGiuaCacBo != 1)
                    {
                        HelpReturn = HelpReturn + "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tích các số trong bộ số đứng ngay trước nó nhân với số " + CongSai_CongBoiGiuaCacBo.ToString().Trim() + " </b>.";
                    }
                    else
                    {
                        HelpReturn = HelpReturn + "<br/>- Giữa các bộ số có quy luật: <b>Số đầu tiên trong bộ số đứng sau bằng tích các số trong bộ số đứng ngay trước nó</b>.";
                    }
                }
            }
            return HelpReturn;
        }

        /// <summary>
        /// Đảo chật tự một dãy các số theo thứ tự ngược lại 
        /// </summary>
        /// <param name="DaySoCanDao">Dãy số cần đảo</param>
        /// <param name="KeyPhanCachGiuaCacBo">Ký tự phân cách giữa các phần tử</param>
        /// <returns></returns>
        public string DaoDay(string DaySoCanDao, char KeyPhanCachGiuaCacBo)
        {
            string KetQua = "";
            string[] DaySo = DaySoCanDao.Split(KeyPhanCachGiuaCacBo);
            for (int i = DaySo.Length - 1; i >= 0; i--)
            {
                if (i == DaySo.Length-1)
                {
                    KetQua = DaySo[i].Trim();
                }
                else
                {
                    KetQua = KetQua + KeyPhanCachGiuaCacBo + DaySo[i].Trim();
                }
            }
            return KetQua;
        }

        /// <summary>
        /// Tính tổng hoặc tích của các số trong một bộ số
        /// </summary>
        /// <param name="BoSo">Bộ số cần tính tổng hoặc tích</param>
        /// <param name="Key"> \Ký tự phân cách giữa các phần tử của mỗi bộ số</param>
        /// <param name="KindOperator">Loại phép toán cần tính: +, -</param>
        /// <returns>Kết quả trả về tổng hoặc tích của các số trong một bộ số</returns>
        public int TinhTongHoacTichCuaBoSo(string BoSo, char Key, int KindOperator)
        {
            int KetQua;
            if (KindOperator == 1)
            {
                KetQua = 0;
            }
            else
            {
                KetQua = 1;
            }
            string[] DSPhanTu = BoSo.Trim().Split(Key);
            for (int i = 0; i <= DSPhanTu.Length - 1; i++)
            {
                if (KindOperator == 1)//Tổng các số của bộ số
                {
                    KetQua = KetQua + Convert.ToInt32(DSPhanTu[i].Trim());
                }
                else if (KindOperator == 2)//Tích các số của bộ số
                {
                    KetQua = KetQua * Convert.ToInt32(DSPhanTu[i].Trim());
                }
            }
            return KetQua;
        }

        /// <summary>
        /// Sinh ngẫu nhiên day số theo quy luật tổng hoặc tích các phần tử đúng trước bằng phần tử đứng sau
        /// </summary>
        /// <param name="DayPhanTuDauTien">Các phần tử đầu tiên của dãy</param>
        /// <param name="SoPhanTuLayTongHoacTich">Số phần tử lấy tổng hoặc tích bằng phần tử tiếp sau</param>
        /// <param name="SoPhanTuCuaDaySo">Số phần tử của dãy số</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử</param>
        /// <param name="KindOperator">Phép toán tổng hoặc tích: 1- Tổng, 2- Tích</param>
        /// <param name="ThuTuSapXep">THứ tự sắp xếp dãy số</param>
        public string DaySoTongVaTich(string DayPhanTuDauTien, int SoPhanTuCuaDaySo, char Key, int KindOperator, bool ThuTuSapXep)
        {
            //     *5: Tổng hai số trước bằng số tiếp sau: 1, 2, 3, 5, 8
            //     *6: Hiệu hai số trước bằng số tiếp sau: 8, 5, 3, 2, 1
            //     *7: Tổng ba số đầu bằng số tiếp theo: 1, 2, 5, 8, 15, 25, 48
            //     *6: Tích hai số trước bằng số tiếp sau: 1, 2, 2, 4, 8, 32
            //     *7: Thương ba số đầu bằng số tiếp theo: 32, 8, 4, 2, 2, 1

            string ReturnValue = DayPhanTuDauTien;
            Random RandomTool = new Random();
            string[] DanhSachPhanTu = ReturnValue.Split(Key);
            for (int i = 1; i <= SoPhanTuCuaDaySo - DanhSachPhanTu.Length; i++)
            {
                string[] DSPhanTu = ReturnValue.Split(Key);
                                
                int TongTich;
                if (KindOperator==1)//Tính tổng các số hạng đứng trước
                {
                    TongTich=0;
                }
                else//Tính tích các số hạng đứng trước
                {
                    TongTich=1;
                }
                int Dem = 0;
                for (int j = DSPhanTu.Length - 1; j >= 0; j--)
                {
                    Dem++;
                    if (KindOperator == 1)//Tính tổng các số hạng đứng trước
                    {
                        TongTich = TongTich + Convert.ToInt32(DSPhanTu[j].Trim());
                    }
                    else//Tính tích các số hạng đứng trước
                    {
                        TongTich = TongTich * Convert.ToInt32(DSPhanTu[j].Trim());
                    }
                    if (Dem >= DanhSachPhanTu.Length)
                    {
                        break;
                    }
                }
                ReturnValue = ReturnValue + Key + TongTich;
            }
            
            if (ThuTuSapXep)
            {
                return ReturnValue;
            }
            else
            {
                return DaoDay(ReturnValue, Key);
            }
        }

        /// <summary>
        /// Sinh một dãy số từ một dãy số cho trước là cấp số cộng hoặc cấp số nhân
        /// </summary>
        /// <param name="SoPhanTuDaySo">Số phần tử của dãy số muốn sinh</param>
        /// <param name="LoaiDaySo">Dãy số nguồn là cấp số cộng hay cấp số nhân: 1- Cấp số cộng; 2- Cấp số nhân </param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của dãy số nguồn</param>
        /// <param name="CongSaiCongBoi">Công sai hoặc công bội của dãy số nguồn</param>
        /// <param name="LoaiPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng các phần tử của dãy số nguồn: 1- Tổng; 2- Tích</param>
        /// <param name="SoPhanTuThamGiaPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng của SoPhanTuThamGiaPhepToan của dãy số nguồn</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử của dãy số sinh ra</param>
        /// <returns> Sinh một dãy số từ một dãy số cho trước là cấp số cộng hoặc cấp số nhân</returns>
        public string DaySoSinhTuCapSoCongHoacCapSoNhan(int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan, char Key)
        {
            //14: Mỗi số trong dãy là tích của hai phần tử có quy luật: 6=2x3, 12=3x4, 20=4x5, 30=5x6 sinh từ dãy 2, 3, 4, 5, 6 
            //(Hay) 3=1x3; 15=3x5; 35=5x7 sinh ra từ: 1, 3, 5, 7
            // (Hay) 1x3x5=15; 3x5x7= 105; 5x7x9=315  sinh ra từ: 1, 3, 5, 7, 9
            //(Hay)  1x3x5x7=! 3x5x7x9=! 5x7x9x11=!    sinh ra từ: 1, 3, 5, 7, 9, 11

            //Khởi tạo cấp số nguồn để sinh dãy số
            int SoPhanTuCuaCapSo = SoPhanTuDaySo + SoPhanTuThamGiaPhepToan - 1;
            int[] CapSo = new int[SoPhanTuCuaCapSo + 1];
            CapSo[1] = PhanTuDauTien;
            for (int i = 2; i <= SoPhanTuCuaCapSo; i++)
            {
                if (LoaiDaySo == 1) //Cấp số cộng
                {
                    CapSo[i] = CapSo[i - 1] + CongSaiCongBoi;
                }
                else // Cấp số nhân
                {
                    CapSo[i] = CapSo[i - 1] * CongSaiCongBoi;
                }
            }

            //Sinh dãy số từ cấp số nguồn
            int[] DaySoKetQua = new int[SoPhanTuDaySo + 1];
            for (int i = 1; i <= SoPhanTuDaySo; i++)
            {
                if (LoaiPhepToan == 1) //Phần tử của dãy số cần sinh là tổng của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                {
                    DaySoKetQua[i] = 0;
                }
                else //Phần tử của dãy số cần sinh là tích của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                {
                    DaySoKetQua[i] = 1;
                }

                for (int k = i; k <= i + SoPhanTuThamGiaPhepToan - 1; k++)
                {
                    if (LoaiPhepToan == 1) //Phần tử của dãy số cần sinh là tổng của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                    {
                        DaySoKetQua[i] = DaySoKetQua[i] + CapSo[k];
                    }
                    else //Phần tử của dãy số cần sinh là tích của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                    {
                        DaySoKetQua[i] = DaySoKetQua[i] * CapSo[k];
                    }
                }
            }

            //Chuyển dãy số cần tìm sang dạng xâu ký tự
            string ReturnValue = "";
            for (int i = 1; i <= SoPhanTuDaySo; i++)
            {
                if (i == 1)
                {
                    ReturnValue = DaySoKetQua[i].ToString().Trim();
                }
                else
                {
                    ReturnValue = ReturnValue + Key + DaySoKetQua[i].ToString().Trim();
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Sinh hướng dẫn giải một dãy số từ một dãy số cho trước là cấp số cộng hoặc cấp số nhân
        /// </summary>
        /// <param name="SoPhanTuDaySo">Số phần tử của dãy số muốn sinh</param>
        /// <param name="LoaiDaySo">Dãy số nguồn là cấp số cộng hay cấp số nhân: 1- Cấp số cộng; 2- Cấp số nhân </param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của dãy số nguồn</param>
        /// <param name="CongSaiCongBoi">Công sai hoặc công bội của dãy số nguồn</param>
        /// <param name="LoaiPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng các phần tử của dãy số nguồn: 1- Tổng; 2- Tích</param>
        /// <param name="SoPhanTuThamGiaPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng của SoPhanTuThamGiaPhepToan của dãy số nguồn</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử của dãy số sinh ra</param>
        /// <returns> Sinh một dãy số từ một dãy số cho trước là cấp số cộng hoặc cấp số nhân</returns>
        public string HuongDanDaySoSinhTuCapSoCongHoacCapSoNhan(int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan, char Key)
        {
            //14: Mỗi số trong dãy là tích của hai phần tử có quy luật: 6=2x3, 12=3x4, 20=4x5, 30=5x6 sinh từ dãy 2, 3, 4, 5, 6 
            //(Hay) 3=1x3; 15=3x5; 35=5x7 sinh ra từ: 1, 3, 5, 7
            // (Hay) 1x3x5=15; 3x5x7= 105; 5x7x9=315  sinh ra từ: 1, 3, 5, 7, 9
            //(Hay)  1x3x5x7=! 3x5x7x9=! 5x7x9x11=!    sinh ra từ: 1, 3, 5, 7, 9, 11

            //Khởi tạo cấp số nguồn để sinh dãy số

            string HuongDan = "Xét các số sau: <b>" + PhanTuDauTien.ToString().Trim();
            
            int SoPhanTuCuaCapSo = SoPhanTuDaySo + SoPhanTuThamGiaPhepToan - 1;
            int[] CapSo = new int[SoPhanTuCuaCapSo + 1];
            CapSo[1] = PhanTuDauTien;
            for (int i = 2; i <= SoPhanTuCuaCapSo; i++)
            {
                if (LoaiDaySo == 1) //Cấp số cộng
                {
                    CapSo[i] = CapSo[i - 1] + CongSaiCongBoi;
                    HuongDan = HuongDan + "  " + CapSo[i];
                }
                else // Cấp số nhân
                {
                    CapSo[i] = CapSo[i - 1] * CongSaiCongBoi;
                    HuongDan = HuongDan + "  " + CapSo[i];
                }
            }
            if (LoaiDaySo == 1) //Cấp số cộng
            {
                HuongDan = HuongDan + "</b>. <br/> Các số đó có quy luật: <b> Số đứng sau bằng số đứng ngay trước nó cộng với " + CongSaiCongBoi.ToString().Trim() + "</b>";
            }
            else // Cấp số nhân
            {
                HuongDan = HuongDan + "</b>. <br/> Các số đó có quy luật: <b> Số đứng sau bằng số đứng ngay trước nó nhân với " + CongSaiCongBoi.ToString().Trim() + "</b>";
            }

            HuongDan = HuongDan + "<br/>Từ các số trên ta có thể tính toán như sau để được các số trong bài toán:";
            //Sinh dãy số từ cấp số nguồn
            int[] DaySoKetQua = new int[SoPhanTuDaySo + 1];
            for (int i = 1; i <= SoPhanTuDaySo; i++)
            {
                if (LoaiPhepToan == 1) //Phần tử của dãy số cần sinh là tổng của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                {
                    DaySoKetQua[i] = 0;
                }
                else //Phần tử của dãy số cần sinh là tích của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                {
                    DaySoKetQua[i] = 1;
                }
                string XauPhepToan = "";
                int Dem = 0;
                for (int k = i; k <= i + SoPhanTuThamGiaPhepToan - 1; k++)
                {
                    Dem++;
                    if (LoaiPhepToan == 1) //Phần tử của dãy số cần sinh là tổng của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                    {
                        if (Dem == 1)
                        {
                            XauPhepToan = XauPhepToan+CapSo[k].ToString().Trim();
                        }
                        else
                        {
                            XauPhepToan = XauPhepToan + " + " + CapSo[k].ToString().Trim();
                        }
                        DaySoKetQua[i] = DaySoKetQua[i] + CapSo[k];
                    }
                    else //Phần tử của dãy số cần sinh là tích của SoPhanTuThamGiaPhepToan phần tử của dãy số nguồn
                    {
                        if (Dem == 1)
                        {
                            XauPhepToan =XauPhepToan+ CapSo[k].ToString().Trim();
                        }
                        else
                        {
                            XauPhepToan = XauPhepToan + " x " + CapSo[k].ToString().Trim();
                        }
                        DaySoKetQua[i] = DaySoKetQua[i] * CapSo[k];
                    }
                }
                HuongDan = HuongDan + "<br/> - Số thứ " + i.ToString().Trim() + " là: <b>" + DaySoKetQua[i].ToString().Trim() + " = " + XauPhepToan.Trim() + "</b>";
            }
            return HuongDan;
        }

        /// <summary>
        /// Sinh dãy chính phương từ một dãy số cấp số cộng hoặc cấp số nhân
        /// </summary>
        /// <param name="SoPhanTuDaySo">Số phần tử của dãy số muốn sinh</param>
        /// <param name="LoaiDaySo">Dãy số nguồn là cấp số cộng hay cấp số nhân: 1- Cấp số cộng; 2- Cấp số nhân </param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của dãy số nguồn</param>
        /// <param name="CongSaiCongBoi">Công sai hoặc công bội của dãy số nguồn</param>
        /// <param name="LoaiPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng các phần tử của dãy số nguồn: 1- Tổng; 2- Tích</param>
        /// <param name="SoPhanTuThamGiaPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng của SoPhanTuThamGiaPhepToan của dãy số nguồn</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử của dãy số sinh ra</param>
        /// <returns>Sinh dãy chính phương từ một dãy số cấp số cộng hoặc cấp số nhân</returns>
        public string DaySoChinhPhuongTuDaySo(bool ThuTuSapXep, int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, char Key)
        {
            //Dãy số chính phương: 1, 4, 9, 16

            //Khởi tạo cấp số nguồn để sinh dãy số
            int[] CapSo = new int[SoPhanTuDaySo + 1];
            CapSo[1] = PhanTuDauTien;
            for (int i = 2; i <= SoPhanTuDaySo; i++)
            {
                if (LoaiDaySo == 1) //Cấp số cộng
                {
                    CapSo[i] = CapSo[i - 1] + CongSaiCongBoi;
                }
                else // Cấp số nhân
                {
                    CapSo[i] = CapSo[i - 1] * CongSaiCongBoi;
                }
            }

            //Sinh dãy số từ dãy số nguồn
            int[] DaySoKetQua = new int[SoPhanTuDaySo + 1];
            for (int i = 1; i <= SoPhanTuDaySo; i++)
            {
                DaySoKetQua[i] = CapSo[i] * CapSo[i];
            }

            //Chuyển dãy số cần tìm sang dạng xâu ký tự
            string ReturnValue = "";
            if (ThuTuSapXep)
            {
                for (int i = 1; i <= SoPhanTuDaySo; i++)
                {
                    if (i == 1)
                    {
                        ReturnValue = DaySoKetQua[i].ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + DaySoKetQua[i].ToString().Trim();
                    }
                }
            }
            else
            {
                for (int i = SoPhanTuDaySo; i >=1 ; i--)
                {
                    if (i == SoPhanTuDaySo)
                    {
                        ReturnValue = DaySoKetQua[i].ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key + DaySoKetQua[i].ToString().Trim();
                    }
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Hướng dẫn giải dãy chính phương từ một dãy số cấp số cộng hoặc cấp số nhân
        /// </summary>
        /// <param name="SoPhanTuDaySo">Số phần tử của dãy số muốn sinh</param>
        /// <param name="LoaiDaySo">Dãy số nguồn là cấp số cộng hay cấp số nhân: 1- Cấp số cộng; 2- Cấp số nhân </param>
        /// <param name="PhanTuDauTien">Phần tử đầu tiên của dãy số nguồn</param>
        /// <param name="CongSaiCongBoi">Công sai hoặc công bội của dãy số nguồn</param>
        /// <param name="LoaiPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng các phần tử của dãy số nguồn: 1- Tổng; 2- Tích</param>
        /// <param name="SoPhanTuThamGiaPhepToan">Phần tử của dãy số được sinh ra bằng cách lấy tích hay tổng của SoPhanTuThamGiaPhepToan của dãy số nguồn</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử của dãy số sinh ra</param>
        /// <returns>Sinh dãy chính phương từ một dãy số cấp số cộng hoặc cấp số nhân</returns>
        public string HuongDanDaySoChinhPhuongTuDaySo(bool ThuTuSapXep, int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, char Key)
        {
            string HuongDan = "Xét các số: <b>";
            //Dãy số chính phương: 1, 4, 9, 16

            //Khởi tạo cấp số nguồn để sinh dãy số
            int[] CapSo1 = new int[SoPhanTuDaySo + 1];
            int[] CapSo = new int[SoPhanTuDaySo + 1];
            CapSo1[1] = PhanTuDauTien;
            for (int i = 2; i <= SoPhanTuDaySo; i++)
            {
                if (LoaiDaySo == 1) //Cấp số cộng
                {
                    CapSo1[i] = CapSo1[i - 1] + CongSaiCongBoi;
                }
                else // Cấp số nhân
                {
                    CapSo1[i] = CapSo1[i - 1] * CongSaiCongBoi;
                }
            }

            if (!ThuTuSapXep)
            {
                for (int i = 1; i <= SoPhanTuDaySo; i++)
                {
                    CapSo[i] = CapSo1[SoPhanTuDaySo + 1 - i];
                    HuongDan = HuongDan + " " + CapSo[i].ToString().Trim();
                }
            }
            else
            {
                for (int i = 1; i <= SoPhanTuDaySo; i++)
                {
                    CapSo[i] = CapSo1[i];
                    HuongDan = HuongDan + " " + CapSo[i].ToString().Trim();
                }
            }
            if (LoaiDaySo == 1) //Cấp số cộng
            {
                if (!ThuTuSapXep)
                {
                    HuongDan = HuongDan + "</b><br/> - Trong các số trên, số đứng sau bằng số đứng ngay trước nó trừ bớt số <b>" + CongSaiCongBoi.ToString().Trim() + "</b>";
                }
                else
                {
                    HuongDan = HuongDan + "</b><br/> - Trong các số trên, số đứng sau bằng số đứng ngay trước nó cộng với số <b>" + CongSaiCongBoi.ToString().Trim() + "</b>";
                }
            }
            else
            {
                if (!ThuTuSapXep)
                {
                    HuongDan = HuongDan + "</b><br/> - Trong các số trên, số đứng sau bằng số đứng ngay trước nó chia cho số <b>" + CongSaiCongBoi.ToString().Trim() + "</b>";
                }
                else
                {
                    HuongDan = HuongDan + "</b><br/> - Trong các số trên, số đứng sau bằng số đứng ngay trước nó nhân với số <b>" + CongSaiCongBoi.ToString().Trim() + "</b>";
                }
            }
            //Sinh dãy số từ dãy số nguồn
            int[] DaySoKetQua = new int[SoPhanTuDaySo + 1];
            HuongDan = HuongDan + "<br/> Từ các số trên ta có thể tạo ra các số trong đề bài toán bằng cách như sau:";
            for (int i = 1; i <= SoPhanTuDaySo; i++)
            {
                HuongDan = HuongDan + "<br/> - Số hạng thứ " + i.ToString().Trim() + " là: <b>" + (CapSo[i] * CapSo[i]).ToString().Trim() + " = " + CapSo[i].ToString().Trim() + " x " + CapSo[i].ToString().Trim() + "</b>";
            }
            return HuongDan;
        }

        /// <summary>
        /// Lấy danh sách các vị trí phần tử có thể ẩn trong dãy số
        /// </summary>
        /// <param name="DoDaiCuaDay">Dộ dài của dãy số</param>
        /// <param name="Key1">Ký tự phân cách giữa các bộ</param>
        /// <param name="Key2">Ký tự phân cách giữa các phần tử của một bộ</param>
        /// <returns></returns>
        public string  LayViTri(int DoDaiCuaDay, string SoPhanTuAn, char Key1, char Key2)
        {
            string ReturnValue = "";
            if (SoPhanTuAn.IndexOf("1") >= 0 && DoDaiCuaDay>=4)
            {
                //Ẩn 01 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    if (ReturnValue=="")
                    {
                        ReturnValue = i.ToString().Trim();
                    }
                    else
                    {
                        ReturnValue = ReturnValue + Key1 + i.ToString().Trim();
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("2") >= 0 && DoDaiCuaDay >= 5)
            {
                //Ẩn 02 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        if (ReturnValue == "")
                        {
                            ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim();
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim();
                        }
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("3") >= 0 && DoDaiCuaDay >= 6)
            {
                //Ẩn 03 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        for (int k = j + 1; k <= DoDaiCuaDay - 1; k++)
                        {
                            if (ReturnValue == "")
                            {
                                ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim();
                            }
                            else
                            {
                                ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim();
                            }
                        }
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("4") >= 0 && DoDaiCuaDay >= 7)
            {
                //Ẩn 04 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        for (int k = j + 1; k <= DoDaiCuaDay - 1; k++)
                        {
                            for (int m = k + 1; m <= DoDaiCuaDay - 1; m++)
                            {
                                if (ReturnValue == "")
                                {
                                    ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim();
                                }
                                else
                                {
                                    ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim();
                                }
                            }
                        }
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("5") >= 0 && DoDaiCuaDay >= 8)
            {
                //Ẩn 05 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        for (int k = j + 1; k <= DoDaiCuaDay - 1; k++)
                        {
                            for (int m = k + 1; m <= DoDaiCuaDay - 1; m++)
                            {
                                for (int n = m + 1; n <= DoDaiCuaDay - 1; n++)
                                {
                                    if (ReturnValue == "")
                                    {
                                        ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim();
                                    }
                                    else
                                    {
                                        ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("6") >= 0 && DoDaiCuaDay >= 9)
            {
                //Ẩn 06 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        for (int k = j + 1; k <= DoDaiCuaDay - 1; k++)
                        {
                            for (int m = k + 1; m <= DoDaiCuaDay - 1; m++)
                            {
                                for (int n = m + 1; n <= DoDaiCuaDay - 1; n++)
                                {
                                    for (int p = n + 1; p <= DoDaiCuaDay - 1; p++)
                                    {
                                        if (ReturnValue == "")
                                        {
                                            ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim() + Key2 + p.ToString().Trim();
                                        }
                                        else
                                        {
                                            ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim() + Key2 + p.ToString().Trim();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (SoPhanTuAn.IndexOf("7") >= 0 && DoDaiCuaDay >= 10)
            {
                //Ẩn 06 phần tử
                for (int i = 0; i <= DoDaiCuaDay - 1; i++)
                {
                    for (int j = i + 1; j <= DoDaiCuaDay - 1; j++)
                    {
                        for (int k = j + 1; k <= DoDaiCuaDay - 1; k++)
                        {
                            for (int m = k + 1; m <= DoDaiCuaDay - 1; m++)
                            {
                                for (int n = m + 1; n <= DoDaiCuaDay - 1; n++)
                                {
                                    for (int p = n + 1; p <= DoDaiCuaDay - 1; p++)
                                    {
                                        for (int q = p + 1; q <= DoDaiCuaDay - 1; q++)
                                        {
                                            if (ReturnValue == "")
                                            {
                                                ReturnValue = i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim() + Key2 + p.ToString().Trim() + Key2 + q.ToString().Trim();
                                            }
                                            else
                                            {
                                                ReturnValue = ReturnValue + Key1 + i.ToString().Trim() + Key2 + j.ToString().Trim() + Key2 + k.ToString().Trim() + Key2 + m.ToString().Trim() + Key2 + n.ToString().Trim() + Key2 + p.ToString().Trim() + Key2 + q.ToString().Trim();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Kiểm tra một vị trí có trong danh sách vị trí hay không
        /// </summary>
        /// <param name="ViTriAn">Danh sách vị trí</param>
        /// <param name="ViTri">Vị trí muốn kiểm tra</param>
        /// <returns></returns>
        public bool KiemTraViTri(string ViTriAn, string ViTri, char Key)
        {
            string[] DSViTri = ViTriAn.Split(Key);
            bool kiemtra = false;
            for (int i = 0; i <= DSViTri.Length - 1; i++)
            {
                if (ViTri.Trim() == DSViTri[i].Trim())
                {
                    kiemtra = true;
                    break;
                }
            }
            return kiemtra;
        }

        /// <summary>
        /// Tạo ra một dãy số đã khuyết các vị trí từ một dãy nguồn đầy đủ
        /// </summary>
        /// <param name="DaySo">Dãy số đầy đủ phần tử nguồn</param>
        /// <param name="ViTriAn">Các vị trí ẩn phần tử</param>
        /// <param name="ViTriAn">Ký tự phân cách các phần tử của dãy số</param>
        /// <param name="ViTriAn">Ký tự phân cách của các vị trí ẩn</param>
        public string BienDoiDaySo(string DaySo, string ViTriAn, char Key1, char Key2, bool ThuTuSapXep)
        {
            string ReturnValue = "";
            string[] DSPhanTu = DaySo.Split(Key1);
            if (ThuTuSapXep)
            {
                for (int i = 0; i <= DSPhanTu.Length - 1; i++)
                {
                    if (KiemTraViTri(ViTriAn.Trim(), i.ToString().Trim(), Key2))
                    {
                        if (i == 0)
                        {
                            ReturnValue = "...";
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + "...";
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            ReturnValue = DSPhanTu[i].Trim();
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + DSPhanTu[i].Trim();
                        }
                    }
                }
            }
            else
            {
                for (int i = DSPhanTu.Length - 1; i >=0 ; i--)
                {
                    if (KiemTraViTri(ViTriAn.Trim(), i.ToString().Trim(), Key2))
                    {
                        if (i == DSPhanTu.Length - 1)
                        {
                            ReturnValue = "...";
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + "...";
                        }
                    }
                    else
                    {
                        if (i == DSPhanTu.Length - 1)
                        {
                            ReturnValue = DSPhanTu[i].Trim();
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + DSPhanTu[i].Trim();
                        }
                    }
                }
            }
            return ReturnValue;
        }


        /// <summary>
        /// Generate list dap an sai
        /// Cần sinh ra 3 đáp án sai, phân cách bởi dấu #
        /// </summary>
        /// <param name="p">dap an ung</param>
        /// <returns></returns>
        private string LayDapSoSai(string dapAnDung)
        {

            StringBuilder sbResult = new StringBuilder();
            StringBuilder sbKetQua1 = new StringBuilder();
            StringBuilder sbKetQua2 = new StringBuilder();
            StringBuilder sbKetQua3 = new StringBuilder();
            StringBuilder sbNumberTemp = new StringBuilder();

            foreach (char c in dapAnDung)
            {
                if (Char.IsDigit(c))
                {
                    // Append so
                    sbNumberTemp.Append(c);
                }
                else
                {
                    // Neu vua het chuoi so thi append so
                    if (sbNumberTemp.Length > 0)
                    {
                        List<int> BaSoKhac = Generate3SoKhac(sbNumberTemp.ToString());
                        sbKetQua1.Append(BaSoKhac[0]);
                        sbKetQua2.Append(BaSoKhac[1]);
                        sbKetQua3.Append(BaSoKhac[2]);
                        sbNumberTemp.Clear();
                    }
                    // Append ki tu
                    sbKetQua1.Append(c);
                    sbKetQua2.Append(c);
                    sbKetQua3.Append(c);
                }
            }
            if (sbNumberTemp.Length > 0)
            {
                List<int> BaSoKhac = Generate3SoKhac(sbNumberTemp.ToString());
                sbKetQua1.Append(BaSoKhac[0]);
                sbKetQua2.Append(BaSoKhac[1]);
                sbKetQua3.Append(BaSoKhac[2]);
                sbNumberTemp.Clear();
            }

            return sbKetQua1.ToString() + "#" + sbKetQua2.ToString() + "#" + sbKetQua3.ToString();
        }


        /// <summary>
        /// Generate ra 3 so tu nhien khac so dau vao va khac nhau
        /// </summary>
        /// <param name="soDauVao"></param>
        /// <returns></returns>
        private List<int> Generate3SoKhac(string soDauVao)
        {
            int SoDauVao = int.Parse(soDauVao);
            List<int> Result = new List<int>();
            int So1 = 0;
            int So2 = 0;
            int So3 = 0;
            int MinRange = 0 - SoDauVao;
            int MaxRange = SoDauVao;
            Random rnd = new Random();
            int SoRandom;

            if (SoDauVao < 2) { MinRange = 0; MaxRange = 5; }
            if (2 < SoDauVao && SoDauVao > 30) { MinRange = 0 - SoDauVao; MaxRange = SoDauVao; }
            if (SoDauVao > 30) { MinRange = -30; MaxRange = 30; }

            SoRandom = rnd.Next(MinRange, MaxRange);

            // Generate so 1
            if (SoRandom == 0) SoRandom += 1;
            So1 = SoDauVao + SoRandom;

            // Generate so 1
            do
            {
                SoRandom = rnd.Next(MinRange, MaxRange);
                if (SoRandom == 0) SoRandom += 1;
                So2 = SoDauVao + SoRandom;
            }
            while (So2 == So1);

            // Generate so 3
            do
            {
                SoRandom = rnd.Next(MinRange, MaxRange);
                if (SoRandom == 0) SoRandom += 1;
                So3 = SoDauVao + SoRandom;
            }
            while (So3 == So1 || So3 == So2);

            Result.Add(So1);
            Result.Add(So2);
            Result.Add(So3);
            return Result;
        }




        /// <summary>
        /// Lấy đáp số của một dãy số 
        /// </summary>
        /// <param name="DaySo">Dãy số đầy đủ phần tử nguồn</param>
        /// <param name="ViTriAn">Các vị trí ẩn phần tử</param>
        /// <param name="ViTriAn">Ký tự phân cách các phần tử của dãy số</param>
        /// <param name="ViTriAn">Ký tự phân cách của các vị trí ẩn</param>
        /// <param name="ThuTuSapXep">Thứ tự sắp xếp phần tử</param>
        /// <returns></returns>
        public string LayDapSo(string DaySo, string ViTriAn, char Key1, char Key2, bool ThuTuSapXep)
        {
            string ReturnValue = "";
            string[] DSPhanTu = DaySo.Split(Key1);
            if (ThuTuSapXep)
            {
                int Dem = 0;
                for (int i = 0; i <= DSPhanTu.Length - 1; i++)
                {
                    if (KiemTraViTri(ViTriAn.Trim(), i.ToString().Trim(), Key2))
                    {
                        Dem++;
                        if (Dem == 1)
                        {
                            ReturnValue = DSPhanTu[i].Trim();
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + DSPhanTu[i].Trim();
                        }
                    }
                }
            }
            else
            {
                int Dem = 0;
                for (int i = DSPhanTu.Length-1; i >=0; i--)
                {
                    if (KiemTraViTri(ViTriAn.Trim(), i.ToString().Trim(), Key2))
                    {
                        Dem++;
                        if (Dem == 1)
                        {
                            ReturnValue = DSPhanTu[i].Trim();
                        }
                        else
                        {
                            ReturnValue = ReturnValue + Key1 + DSPhanTu[i].Trim();
                        }
                    }
                }
            }
            return ReturnValue;
        }

        /// <summary>
        /// Tạo danh sách các cấp số cộng
        /// </summary>
        /// <param name="u1_min">Phần tử đầu tiên bé nhất</param>
        /// <param name="u1_max">Phần tử đầu tiên lớn nhất</param>
        /// <param name="d_min">Công sai bé nhất</param>
        /// <param name="d_max">Công sai nhất</param>
        /// <param name="SoPhanTu_min">Số  phần tử của dãy số bé nhất</param>
        /// <param name="SoPhanTu_max">Số  phần tử của dãy số lớn nhất</param>
        /// <param name="GioiHanPhanTu">Giá trị lớn nhất của phần tử lớn nhất</param>
        /// <param name="Key">Ký tự phân cách của các phần tử</param>
        /// <param name="CleverExerKindId">Thuộc loại bài tập</param>
        /// <param name="SoPhanTuAn">Số phần tử ẩn của dãy số</param>
        public List<BaiToanDaySoModel> TaoCacCapSoCong(int u1_min, int u1_max, int d_min, int d_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, char Key, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo, bool PhamViSinh, bool TatCaToHopAn)
        {
            List<BaiToanDaySoModel> CacDaySoDuocTao = new List<BaiToanDaySoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();
            //Arithmetic(int MinValueItem, int MaxValueItem, bool ascending, int RangeCount, int u1, int d, char Key)
            for (int u1 = u1_min; u1 <= u1_max; u1++)
            {
                for (int d = d_min; d <= d_max; d++)
                {
                    for (int RangeCount = SoPhanTu_min; RangeCount <= SoPhanTu_max; RangeCount++)
                    {
                        //Sinh dãy số là cấp số cộng có phần tử đầu tiên u1, công sai d, ký tự phân cách giữa các phần tử là Key
                        string DaySo = Arithmetic(true, RangeCount, u1, d, Key);
                        string DaySoGiam = Arithmetic(false, RangeCount, u1, d, Key);
                        string HuongDanDaySoTang = HelpArithmetic(true, RangeCount, u1, d, Key);
                        string HuongDanHelpDaySoGiam = HelpArithmetic(false, RangeCount, u1, d, Key);
                        string GoiYTraLoiTang = GoiYTraLoiCapSoCong(true, d);
                        string GoiYTraLoiGiam = GoiYTraLoiCapSoCong(false, d);
                        string[] DSPhanTuDaySo = DaySo.Split(Key);
                        if (Convert.ToInt32(DSPhanTuDaySo[DSPhanTuDaySo.Length - 1]) <= GioiHanPhanTu)
                        {

                            //Lấy tổ hợp các vị trí có thể ẩn
                            string[] DSViTriAn = LayViTri(RangeCount, SoPhanTuAn, ',', '!').Split(',');

                            //TatCaToHopAn=false chỉ sinh một trường hợp ẩn cho 1 dẫy không lấy tất cả các tổ hợp ẩn
                            if (!TatCaToHopAn)
                            {
                                int s = rd.Next(0, DSViTriAn.Length - 1);
                                //Lấy số ngẫu nhiên
                                int SoNgauNhien = rd.Next(26598, 99598) % 2;

                                //Danh sách các vị trí ẩn
                                string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                #region Sinh dãy tăng
                                if (SoNgauNhien == 1 || PhamViSinh)
                                {
                                    //Khởi tạo một dãy số null
                                    BaiToanDaySoModel modelItem = new BaiToanDaySoModel();

                                    //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                    modelItem.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                    //Lấy đáp số của các phần tử bị ẩn
                                    modelItem.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                    // Lấy đáp số sai
                                    modelItem.NoiDungDapAnSai = LayDapSoSai(modelItem.NoiDungDapAn);

                                    //Lấy câu hỏi hiển thị
                                    modelItem.CauHoiHienThi = "Tìm các số thích hợp điền vào ô trống";

                                    //Lấy hướng dẫn giải
                                    modelItem.LoiGiaiCauHoi = HuongDanDaySoTang;

                                    //Lấy mã dãy số
                                    modelItem.MaDaySo = Guid.NewGuid();

                                    //Phạm vi phép toán
                                    modelItem.PhamViPhepToan = PhamViPhepToan;

                                    //Phạm vi phép toán
                                    modelItem.ThuocKhoiLop = ThuocKhoiLop;

                                    //Phân loại dãy số
                                    modelItem.PhanLoaiDaySo = PhanLoaiDaySo;

                                    //Lấy số thứ tự ngẫu nhiên
                                    modelItem.SapXepThuTu = rd.Next(25698, 95698);

                                    //Số lượng đáp án
                                    modelItem.SoLuongDapAn = DSVTAn.Length;

                                    //Gợi ý trả lời
                                    modelItem.GoiYTraLoi = GoiYTraLoiTang;

                                    //Số phần tử của dãy số
                                    modelItem.SoLuongPhanTu = RangeCount;

                                    modelItem.KetLuanCauHoi = "<br/>- Kết quả các số phải tìm là: <b>" + modelItem.NoiDungDapAn.Replace(Key, ' ') + "</b><br/> - Kết quả các số đầy đủ là: <b>" + DaySo.Replace(Key, ' ') + "</b>";

                                    //Thêm dãy số vào danh sách
                                    CacDaySoDuocTao.Add(modelItem);
                                }
                                #endregion

                                #region Sinh dãy giảm

                                if (SoNgauNhien == 0 || PhamViSinh)
                                {
                                    //Khởi tạo một dãy số null
                                    BaiToanDaySoModel modelItemDes = new BaiToanDaySoModel();

                                    //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                    modelItemDes.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), Key, '!', false);

                                    //Lấy đáp số của các phần tử bị ẩn
                                    modelItemDes.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), Key, '!', false);

                                    // Lấy đáp số sai
                                    modelItemDes.NoiDungDapAnSai = LayDapSoSai(modelItemDes.NoiDungDapAn);

                                    //Lấy câu hỏi hiển thị
                                    modelItemDes.CauHoiHienThi = "Tìm các số thích hợp điền vào ô trống";

                                    //Lấy hướng dẫn giải
                                    modelItemDes.LoiGiaiCauHoi = HuongDanHelpDaySoGiam;

                                    //Lấy mã dãy số
                                    modelItemDes.MaDaySo = Guid.NewGuid();

                                    //Phạm vi phép toán
                                    modelItemDes.PhamViPhepToan = PhamViPhepToan;

                                    //Phạm vi phép toán
                                    modelItemDes.ThuocKhoiLop = ThuocKhoiLop;

                                    //Phân loại dãy số
                                    modelItemDes.PhanLoaiDaySo = PhanLoaiDaySo;

                                    //Gợi ý trả lời
                                    modelItemDes.GoiYTraLoi = GoiYTraLoiGiam;

                                    //Lấy số thứ tự ngẫu nhiên
                                    modelItemDes.SapXepThuTu = rd.Next(25698, 95698);

                                    //Số lượng đáp án
                                    modelItemDes.SoLuongDapAn = DSVTAn.Length;

                                    //Số phần tử của dãy số
                                    modelItemDes.SoLuongPhanTu = RangeCount;

                                    modelItemDes.KetLuanCauHoi = "<br/>- Kết quả các số phải tìm là: <b>" + modelItemDes.NoiDungDapAn.Replace(Key, ' ') + "</b><br/> - Kết quả các số đầy đủ là: <b>" + DaySoGiam.Replace(Key, ' ') + "</b>";

                                    //Thêm dãy số vào danh sách
                                    CacDaySoDuocTao.Add(modelItemDes);
                                }
                                #endregion
                            }
                            else
                            {

                                //Lặp từng trường hợp ẩn vị trí để đưa ra các khả năng dãy số 
                                for (int s = 0; s <= DSViTriAn.Length - 1; s++)
                                {
                                    //Lấy số ngẫu nhiên

                                    int SoNgauNhien = rd.Next(26598, 99598) % 2;

                                    //Danh sách các vị trí ẩn
                                    string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                    #region Sinh dãy tăng
                                    if (SoNgauNhien == 1 || PhamViSinh)
                                    {
                                        //Khởi tạo một dãy số null
                                        BaiToanDaySoModel modelItem = new BaiToanDaySoModel();

                                        //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                        modelItem.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                        //Lấy đáp số của các phần tử bị ẩn
                                        modelItem.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                        // Lấy đáp số sai
                                        modelItem.NoiDungDapAnSai = LayDapSoSai(modelItem.NoiDungDapAn);

                                        //Lấy câu hỏi hiển thị
                                        modelItem.CauHoiHienThi = "Tìm các số thích hợp điền vào ô trống";

                                        //Lấy hướng dẫn giải
                                        modelItem.LoiGiaiCauHoi = HuongDanDaySoTang;

                                        //Lấy mã dãy số
                                        modelItem.MaDaySo = Guid.NewGuid();

                                        //Phạm vi phép toán
                                        modelItem.PhamViPhepToan = PhamViPhepToan;

                                        //Phạm vi phép toán
                                        modelItem.ThuocKhoiLop = ThuocKhoiLop;

                                        //Phân loại dãy số
                                        modelItem.PhanLoaiDaySo = PhanLoaiDaySo;

                                        //Lấy số thứ tự ngẫu nhiên
                                        modelItem.SapXepThuTu = rd.Next(25698, 95698);

                                        //Số lượng đáp án
                                        modelItem.SoLuongDapAn = DSVTAn.Length;

                                        //Gợi ý trả lời
                                        modelItem.GoiYTraLoi = GoiYTraLoiTang;

                                        //Số phần tử của dãy số
                                        modelItem.SoLuongPhanTu = RangeCount;

                                        modelItem.KetLuanCauHoi = "<br/>- Kết quả các số phải tìm là: <b>" + modelItem.NoiDungDapAn.Replace(Key, ' ') + "</b><br/> - Kết quả các số đầy đủ là: <b>" + DaySo.Replace(Key, ' ') + "</b>";

                                        //Thêm dãy số vào danh sách
                                        CacDaySoDuocTao.Add(modelItem);
                                    }
                                    #endregion

                                    #region Sinh dãy giảm

                                    if (SoNgauNhien == 0 || PhamViSinh)
                                    {
                                        //Khởi tạo một dãy số null
                                        BaiToanDaySoModel modelItemDes = new BaiToanDaySoModel();

                                        //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                        modelItemDes.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), Key, '!', false);

                                        //Lấy đáp số của các phần tử bị ẩn
                                        modelItemDes.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), Key, '!', false);

                                        // Lấy đáp số sai
                                        modelItemDes.NoiDungDapAnSai = LayDapSoSai(modelItemDes.NoiDungDapAn);

                                        //Lấy câu hỏi hiển thị
                                        modelItemDes.CauHoiHienThi = "Tìm các số thích hợp điền vào ô trống";

                                        //Lấy hướng dẫn giải
                                        modelItemDes.LoiGiaiCauHoi = HuongDanHelpDaySoGiam;

                                        //Lấy mã dãy số
                                        modelItemDes.MaDaySo = Guid.NewGuid();

                                        //Phạm vi phép toán
                                        modelItemDes.PhamViPhepToan = PhamViPhepToan;

                                        //Phạm vi phép toán
                                        modelItemDes.ThuocKhoiLop = ThuocKhoiLop;

                                        //Phân loại dãy số
                                        modelItemDes.PhanLoaiDaySo = PhanLoaiDaySo;

                                        //Gợi ý trả lời
                                        modelItemDes.GoiYTraLoi = GoiYTraLoiGiam;

                                        //Lấy số thứ tự ngẫu nhiên
                                        modelItemDes.SapXepThuTu = rd.Next(25698, 95698);

                                        //Số lượng đáp án
                                        modelItemDes.SoLuongDapAn = DSVTAn.Length;

                                        //Số phần tử của dãy số
                                        modelItemDes.SoLuongPhanTu = RangeCount;

                                        modelItemDes.KetLuanCauHoi = "<br/>- Kết quả các số phải tìm là: <b>" + modelItemDes.NoiDungDapAn.Replace(Key, ' ') + "</b><br/> - Kết quả các số đầy đủ là: <b>" + DaySoGiam.Replace(Key, ' ') + "</b>";

                                        //Thêm dãy số vào danh sách
                                        CacDaySoDuocTao.Add(modelItemDes);
                                    }
                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
            // Sắp xếp các dãy số theo thứ tự tăng dần
            return CacDaySoDuocTao.OrderBy(m=>m.SapXepThuTu).ToList<BaiToanDaySoModel>();
        }

        /// <summary>
        /// Tạo các bộ số có quy luật
        /// </summary>
        /// <param name="u1_min">Phần tử đầu tiên bé nhất</param>
        /// <param name="u1_max">Phần tử đầu tiên lớn nhất</param>
        /// <param name="dq_min">Công sai, công bội bé nhất của dãy</param>
        /// <param name="dq_max">Công sai công bội lớn nhất của dãy </param>
        /// <param name="dq_in_min">Công sai, công bội trong mỗi bộ bé nhất của dãy</param>
        /// <param name="dq_in_max">>Công sai, công bội trong mỗi bộ lớn nhất của dãy</param>
        /// <param name="SoPhanTu_min">Số bộ bé nhất trong dãy</param>
        /// <param name="SoPhanTu_max">Số bộ lớn nhất trong dãy</param>
        /// <param name="SoPhanTu_In_min">Số phần tử bé nhất trong mỗi bộ</param>
        /// <param name="SoPhanTu_In_max">Số phần tử lớn nhất trong mỗi bộ</param>
        /// <param name="GioiHanPhanTu">Giới hạn của phần tử lớn nhất</param>
        /// <param name="QuLuatTrongMoiBo">Quy luật trong mỗi bộ: 1- Cấp số cộng, 2- Cấp số nhân</param>
        /// <param name="QuyLuatGiuaCacBo">Quy luật giữa các bộ: 1- Cấp số cộng, 2- Cấp số nhân</param>
        /// <param name="QuyTacTinhPhanTuDauTien">Quy tắc tính phần tử đầu tiên: 1- Cộng, 2- Nhân</param>
        /// <param name="KeyTrongMoiBo">Ký tự phân cách phần tử trong mỗi bộ</param>
        /// <param name="KeyTrongDay">Ký tự phân cách mỗi bộ trong dãy</param>
        /// <param name="CleverExerKindId">Thuộc dạng bài tập toán thông minh</param>
        public List<BaiToanDaySoModel> TaoCacBoSo(int u1_min, int u1_max, int dq_min, int dq_max, int dq_in_min, int dq_in_max, int SoPhanTu_In_min, int SoPhanTu_In_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            ToolShareService AllToolShare = new ToolShareService();
            List<BaiToanDaySoModel> DanhSachDay = new List<BaiToanDaySoModel>();
            Random rd = new Random();
            //string BoSoNgauNhien(bool SapXepPhanTu, int SoBoTrongDay, int SoPhanTuTrongMoiBo, int QuLuatTrongMoiBo, int QuyLuatGiuaCacBo, int PhanTuDauTien, int CongSai_CongBoiTrongMoiBo, int CongSai_CongBoiGiuaCacBo, int QuyTacTinhPhanTuDauTien, char KeyTrongMoiBo, char KeyTrongDay )

            for (int u1 = u1_min; u1 <= u1_max; u1++)
            {
                for (int dq = dq_min; dq <= dq_max; dq++)
                {
                    for (int dq_in = dq_in_min; dq_in <= dq_in_max; dq_in++)
                    {
                        for (int SoPhanTu_In = SoPhanTu_In_min; SoPhanTu_In <= SoPhanTu_In_max; SoPhanTu_In++)
                        {
                            for (int SoPhanTu = SoPhanTu_min; SoPhanTu <= SoPhanTu_max; SoPhanTu++)
                            {
                                //Sinh một dãy các bộ số có quy luật
                                string DaySo = BoSoNgauNhien(true, SoPhanTu, SoPhanTu_In, QuLuatTrongMoiBo, QuyLuatGiuaCacBo, u1, dq_in, dq, QuyTacTinhPhanTuDauTien, KeyTrongMoiBo, KeyTrongDay);

                                //Lấy hướng dẫn giải
                                string HuongDan = HelpBoSoNgauNhien(true, SoPhanTu, SoPhanTu_In, QuLuatTrongMoiBo, QuyLuatGiuaCacBo, u1, dq_in, dq, QuyTacTinhPhanTuDauTien, KeyTrongMoiBo, KeyTrongDay);

                                //Lấy ra phần tử cuối cùng để kiểm tra giới hạn
                                string[] DSPhanTuDaySo = DaySo.Split(KeyTrongDay);

                                string[] DSPhanTuCuoiCungTrongBoCuoiCung = DSPhanTuDaySo[DSPhanTuDaySo.Length - 1].Split(KeyTrongMoiBo); // Kết quả là: (1 và 2)

                                string[] PhanTuCuoiCung = DSPhanTuCuoiCungTrongBoCuoiCung[DSPhanTuCuoiCungTrongBoCuoiCung.Length - 1].Split(')'); // Kết quả là: 2 và ""

                                if (Convert.ToInt32(PhanTuCuoiCung[0]) <= GioiHanPhanTu)
                                {
                                    //Lấy tổ hợp các vị trí có thể ẩn
                                    string[] DSViTriAn = LayViTri(SoPhanTu, SoPhanTuAn, ',', '!').Split(',');

                                    //Lặp từng trường hợp ẩn vị trí để đưa ra các khả năng dãy số 
                                    for (int s = 0; s <= DSViTriAn.Length - 1; s++)
                                    {
                                        //Danh sách các vị trí ẩn
                                        string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                        //Khởi tạo một dãy số null
                                        BaiToanDaySoModel modelItem = new BaiToanDaySoModel();

                                        //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                        modelItem.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), KeyTrongDay, '!', true);

                                        //Lấy đáp số của các phần tử bị ẩn
                                        modelItem.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), KeyTrongDay, '!', true);

                                        // Lấy đáp số sai
                                        modelItem.NoiDungDapAnSai = LayDapSoSai(modelItem.NoiDungDapAn);

                                        //Lấy câu hỏi hiển thị
                                        modelItem.CauHoiHienThi = "Tìm các bộ số thích hợp còn thiếu điền vào ô trống";

                                        //Lấy hướng dẫn giải
                                        modelItem.LoiGiaiCauHoi = HuongDan;

                                        //Lấy mã dãy số
                                        modelItem.MaDaySo = Guid.NewGuid();

                                        //Phạm vi phép toán
                                        modelItem.PhamViPhepToan = PhamViPhepToan;

                                        //Phạm vi phép toán
                                        modelItem.GoiYTraLoi = HuongDan;

                                        //Phạm vi phép toán
                                        modelItem.ThuocKhoiLop = ThuocKhoiLop;

                                        //Phân loại dãy số
                                        modelItem.PhanLoaiDaySo = PhanLoaiDaySo;

                                        //Lấy số thứ tự ngẫu nhiên
                                        modelItem.SapXepThuTu = rd.Next(25698, 95698);

                                        //Số lượng đáp án
                                        modelItem.SoLuongDapAn = DSVTAn.Length;

                                        //Số phần tử của dãy số
                                        modelItem.SoLuongPhanTu = SoPhanTu;

                                        modelItem.KetLuanCauHoi = "<br/>- Kết quả các bộ số phải tìm là: <b>" + modelItem.NoiDungDapAn.Replace(KeyTrongDay, ' ') + "</b><br/> - Kết quả các bộ số đầy đủ là: <b>" + DaySo.Replace(KeyTrongDay, ' ') + "</b>";

                                        //Thêm dãy số vào danh sách
                                        DanhSachDay.Add(modelItem);

                                    }

                                }
                            }
                        }
                    }
                }
            }
            // Sắp xếp các dãy số theo thứ tự tăng dần
            return DanhSachDay.OrderBy(m => m.SapXepThuTu).ToList<BaiToanDaySoModel>();
            
        }

        /// <summary>
        /// Tạo danh sách các cấp số nhân
        /// </summary>
        /// <param name="u1_min">Phần tử đầu tiên bé nhất</param>
        /// <param name="u1_max">Phần tử đầu tiên lớn nhất</param>
        /// <param name="d_min">Công bội bé nhất</param>
        /// <param name="d_max">Công bội nhất</param>
        /// <param name="SoPhanTu_min">Số  phần tử của dãy số bé nhất</param>
        /// <param name="SoPhanTu_max">Số  phần tử của dãy số lớn nhất</param>
        /// <param name="GioiHanPhanTu">Giá trị lớn nhất của phần tử lớn nhất</param>
        /// <param name="Key">Ký tự phân cách của các phần tử</param>
        /// <param name="CleverExerKindId">Thuộc loại bài tập</param>
        public void TaoCacCapSoNhan(int u1_min, int u1_max, int q_min, int q_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, char Key, string CleverExerKindId, string SoPhanTuAn)
        {
            ToolShareService AllToolShare = new ToolShareService();

            //Arithmetic(int MinValueItem, int MaxValueItem, bool ascending, int RangeCount, int u1, int d, char Key)
            for (int u1 = u1_min; u1 <= u1_max; u1++)
            {
                for (int q = q_min; q <= q_max; q++)
                {
                    for (int RangeCount = SoPhanTu_min; RangeCount <= SoPhanTu_max; RangeCount++)
                    {
                        //Sinh dãy số là cấp số nhân có phần tử đầu tiên u1, công bội q, ký tự phân cách giữa các phần tử là Key
                        string DaySoTang = Geomatric(true, RangeCount, u1, q, Key);

                        //Tạo dãy số giảm
                        string DaySoGiam = Geomatric(false, RangeCount, u1, q, Key);

                        //Hướng dẫn giải cho dãy tăng
                        string HuongDanTang = HelpGeomatric(true, RangeCount, u1, q, Key);

                        //Hướng dẫn giải cho dãy giảm
                        string HuongDanGiam = HelpGeomatric(false, RangeCount, u1, q, Key);

                        string[] DSPhanTuDaySo = DaySoTang.Split(Key);
                        int PhanTuCuoiCung=Convert.ToInt32(DSPhanTuDaySo[DSPhanTuDaySo.Length - 1]);
                        if (PhanTuCuoiCung <= GioiHanPhanTu && PhanTuCuoiCung>0)
                        {

                            //Lấy tổ hợp các vị trí có thể ẩn
                            string[] DSViTriAn = LayViTri(RangeCount,SoPhanTuAn, ',', '!').Split(',');

                            //Lặp từng trường hợp ẩn vị trí để đưa ra các khả năng dãy số 
                            for (int s = 0; s <= DSViTriAn.Length - 1; s++)
                            {

                                //Danh sách các vị trí ẩn
                                string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                #region Sinh dãy tăng

                                //Khởi tạo một dãy số null
                                CleverRangeModel modelItem = new CleverRangeModel();

                                //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                modelItem.CleverRangeValue = BienDoiDaySo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', true);

                                //Lấy đáp số của các phần tử bị ẩn
                                modelItem.CleverAnswers = LayDapSo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', true);

                                //Hướng dẫn giải dãy số 
                                modelItem.CleverRangeHelp = "<table class=\"MathHelp\">" +
                                                               "<tr>" +
                                                                   "<td>" +
                                                                      "<div class=\"HelpTitle\">" +
                                                                          " Hướng dẫn:" +
                                                                      "</div> " +
                                                                       "<p class=\"HelpContent\">" +
                                                                          "Tìm các số thích hợp còn thiếu: <b>" + modelItem.CleverRangeValue.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"HelpContent\">" +
                                                                          HuongDanTang +
                                                                     " </p>" +
                                                                       "<div class=\"HelpTitle\">" +
                                                                            "Kết quả:" +
                                                                       "</div>" +
                                                                     " <p class=\"HelpReturn\"> " +
                                                                     "- Kết quả các số phải tìm là: <b>" + modelItem.CleverAnswers.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"AllHelpReturn\"> " +
                                                                            "- Kết quả các số đầy đủ là: <b>" + DaySoTang.Replace('~', ' ') + "</b>" +
                                                                      "</p>" +
                                                                   "</td>" +
                                                               "</tr>" +
                                                           "</table>";

                                //Đọc danh sách các dãy số đã có để sinh số thứ tự và mã 
                                List<CleverRangeModel> ListCleverRange = GetListCleverRange("");

                                //Sinh ngẫu nhiên mã
                                List<ListSearch> ListID = new List<ListSearch>();
                                foreach (CleverRangeModel ItemId in ListCleverRange)
                                {
                                    ListSearch IdItem = new ListSearch();
                                    IdItem.FieldSearch = ItemId.CleverRangeId;
                                    ListID.Add(IdItem);
                                }
                                modelItem.CleverRangeId = AllToolShare.GetRandomId(ListID, "CR").ToString();

                                //Lấy số thứ thự
                                List<ListFindMax> ListOrder = new List<ListFindMax>();
                                foreach (CleverRangeModel Item in ListCleverRange)
                                {
                                    ListFindMax OrderItem = new ListFindMax();
                                    OrderItem.FieldFindMax = Item.CleverRangeOrder;
                                    ListOrder.Add(OrderItem);
                                }
                                modelItem.CleverRangeOrder = AllToolShare.GetMaxOrderby(ListOrder);

                                //Gán loại bài tập toán
                                modelItem.CleverExerKindId = CleverExerKindId;

                                //Gán bậc của dãy số
                                modelItem.CleverRangeLever = DSVTAn.Length.ToString();

                                //Lưu mới dãy số vào csdl
                                SaveNewCleverRange(modelItem);

                                #endregion

                                #region Sinh dãy giảm

                                //Khởi tạo một dãy số null
                                CleverRangeModel modelItemDes = new CleverRangeModel();

                                //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                modelItemDes.CleverRangeValue = BienDoiDaySo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', false);

                                //Lấy đáp số của các phần tử bị ẩn
                                modelItemDes.CleverAnswers = LayDapSo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', false);

                                //Hướng dẫn giải dãy số 
                                modelItemDes.CleverRangeHelp = "<table class=\"MathHelp\">" +
                                                               "<tr>" +
                                                                   "<td>" +
                                                                      "<div class=\"HelpTitle\">" +
                                                                          " Hướng dẫn:" +
                                                                      "</div> " +
                                                                       "<p class=\"HelpContent\">" +
                                                                          "Tìm các số thích hợp còn thiếu: <b>" + modelItemDes.CleverRangeValue.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"HelpContent\">" +
                                                                          HuongDanGiam +
                                                                     " </p>" +
                                                                       "<div class=\"HelpTitle\">" +
                                                                            "Kết quả:" +
                                                                       "</div>" +
                                                                     " <p class=\"HelpReturn\"> " +
                                                                     "- Kết quả các số phải tìm là: <b>" + modelItemDes.CleverAnswers.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"AllHelpReturn\"> " +
                                                                            "- Kết quả các số đầy đủ là: <b>" + DaySoGiam.Replace('~', ' ') + "</b>" +
                                                                      "</p>" +
                                                                   "</td>" +
                                                               "</tr>" +
                                                           "</table>";

                                //Đọc danh sách các dãy số đã có để sinh số thứ tự và mã 
                                List<CleverRangeModel> CleverRangeList = GetListCleverRange("");

                                //Sinh ngẫu nhiên mã
                                List<ListSearch> ListIDDes = new List<ListSearch>();
                                foreach (CleverRangeModel ItemId in CleverRangeList)
                                {
                                    ListSearch IdItem = new ListSearch();
                                    IdItem.FieldSearch = ItemId.CleverRangeId;
                                    ListIDDes.Add(IdItem);
                                }
                                modelItemDes.CleverRangeId = AllToolShare.GetRandomId(ListIDDes, "CR").ToString();

                                //Lấy số thứ thự
                                List<ListFindMax> ListOrderDes = new List<ListFindMax>();
                                foreach (CleverRangeModel Item in CleverRangeList)
                                {
                                    ListFindMax OrderItem = new ListFindMax();
                                    OrderItem.FieldFindMax = Item.CleverRangeOrder;
                                    ListOrderDes.Add(OrderItem);
                                }
                                modelItemDes.CleverRangeOrder = AllToolShare.GetMaxOrderby(ListOrderDes);

                                //Gán loại bài tập toán
                                modelItemDes.CleverExerKindId = CleverExerKindId;

                                //Gán bậc của dãy số
                                modelItemDes.CleverRangeLever = DSVTAn.Length.ToString();

                                //Lưu mới dãy số vào csdl
                                SaveNewCleverRange(modelItemDes);

                                #endregion
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tạo dãy số từ một cấp số cộng hoặc một cấp số nhân
        /// </summary>
        /// <param name="u1_min">Phần tử đầu tiên bé nhất</param>
        /// <param name="u1_max">Phần tử đầu tên lớn nhất</param>
        /// <param name="dq_min">Công sai, công bội bé nhất</param>
        /// <param name="dq_max">Công sai, công bội lớn nhất</param>
        /// <param name="SoPhanTu_min">Số phần tử của dãy bé nhất</param>
        /// <param name="SoPhanTu_max">Số phần tử của dãy lớn nhất</param>
        /// <param name="LoaiDaySo">Dãy số nguồn là cấp số cộng, nhân: 1- Cấp số cộng, 2- Cấp số nhân</param>
        /// <param name="LoaiPhepToan">Loại phép toán: 1- Tổng các phần tử, 2- Tích các phần tử</param>
        /// <param name="SoPhanTuThamGiaPhepToan_min">Số phần tử tham gia phép toán bé nhất</param>
        /// <param name="SoPhanTuThamGiaPhepToan_Max">Số phần tử tham gia phép toán lớn nhất</param>
        /// <param name="GioiHanPhanTu">Giới hạn phần tử lớn nhất</param>
        /// <param name="Key">Ký tự phân cách giữa các phần tử của dãy số</param>
        /// <param name="CleverExerKindId">Loại bài tập toan</param>
        public List<BaiToanDaySoModel> TaoCacDaySoTuCSCVaCSN(int u1_min, int u1_max, int dq_min, int dq_max, int SoPhanTu_min, int SoPhanTu_max, int LoaiDaySo, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan_min, int SoPhanTuThamGiaPhepToan_Max, int GioiHanPhanTu, char Key, string SoPhanTuAn, string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            List<BaiToanDaySoModel> CacDaySoDuocTao = new List<BaiToanDaySoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();

            //string DaySoSinhTuCapSoCongHoacCapSoNhan(int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, int LoaiPhepToan, int SoPhanTuThamGiaPhepToan, char Key)
            for (int u1 = u1_min; u1 <= u1_max; u1++)
            {
                for (int dq = dq_min; dq <= dq_max; dq++)
                {
                    for (int SoPhanTuThamGiaPhepToan = SoPhanTuThamGiaPhepToan_min; SoPhanTuThamGiaPhepToan <= SoPhanTuThamGiaPhepToan_Max; SoPhanTuThamGiaPhepToan++)
                    {
                        for (int RangeCount = SoPhanTu_min; RangeCount <= SoPhanTu_max; RangeCount++)
                        {
                            //Sinh dãy số là cấp số nhân có phần tử đầu tiên u1, công bội q, ký tự phân cách giữa các phần tử là Key
                            string DaySo = DaySoSinhTuCapSoCongHoacCapSoNhan(RangeCount, LoaiDaySo, u1, dq, LoaiPhepToan, SoPhanTuThamGiaPhepToan, '~');
                            string HuongDan = HuongDanDaySoSinhTuCapSoCongHoacCapSoNhan(RangeCount, LoaiDaySo, u1, dq, LoaiPhepToan, SoPhanTuThamGiaPhepToan, '~');
                            string[] DSPhanTuDaySo = DaySo.Split(Key);
                            int PhanTuCuoiCung = Convert.ToInt32(DSPhanTuDaySo[DSPhanTuDaySo.Length - 1]);
                            if (PhanTuCuoiCung <= GioiHanPhanTu && PhanTuCuoiCung > 0)
                            {
                                //Lấy tổ hợp các vị trí có thể ẩn
                                string[] DSViTriAn = LayViTri(RangeCount, SoPhanTuAn, ',', '!').Split(',');

                                //Lặp từng trường hợp ẩn vị trí để đưa ra các khả năng dãy số 
                                for (int s = 0; s <= DSViTriAn.Length - 1; s++)
                                {
                                    //Danh sách các vị trí ẩn
                                    string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                    //Khởi tạo một dãy số null
                                    BaiToanDaySoModel modelItem = new BaiToanDaySoModel();

                                    //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                    modelItem.NoiDungDaySo = BienDoiDaySo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                    //Lấy đáp số của các phần tử bị ẩn
                                    modelItem.NoiDungDapAn = LayDapSo(DaySo, DSViTriAn[s].Trim(), Key, '!', true);

                                    //Lấy câu hỏi hiển thị
                                    modelItem.CauHoiHienThi = "Tìm các số thích hợp điền vào ô trống";

                                    //Lấy hướng dẫn giải
                                    modelItem.LoiGiaiCauHoi = HuongDan;

                                    //Lấy mã dãy số
                                    modelItem.MaDaySo = Guid.NewGuid();

                                    //Phạm vi phép toán
                                    modelItem.PhamViPhepToan = PhamViPhepToan;

                                    //Phạm vi phép toán
                                    modelItem.ThuocKhoiLop = ThuocKhoiLop;

                                    //Phân loại dãy số
                                    modelItem.PhanLoaiDaySo = PhanLoaiDaySo;

                                    //Lấy số thứ tự ngẫu nhiên
                                    modelItem.SapXepThuTu = rd.Next(25698, 95698);

                                    //Số lượng đáp án
                                    modelItem.SoLuongDapAn = DSVTAn.Length;

                                    //Gợi ý trả lời
                                    modelItem.GoiYTraLoi = HuongDan;

                                    //Số phần tử của dãy số
                                    modelItem.SoLuongPhanTu = RangeCount;

                                    modelItem.KetLuanCauHoi = "<br/>- Kết quả các số phải tìm là: <b>" + modelItem.NoiDungDapAn.Replace(Key, ' ') + "</b><br/> - Kết quả các số đầy đủ là: <b>" + DaySo.Replace(Key, ' ') + "</b>";

                                    //Thêm dãy số vào danh sách
                                    CacDaySoDuocTao.Add(modelItem);

                                }
                            }
                        }
                    }
                }
            }

            // Sắp xếp các dãy số theo thứ tự tăng dần
            return CacDaySoDuocTao.OrderBy(m => m.SapXepThuTu).ToList<BaiToanDaySoModel>();
        }

        /// <summary>
        /// Tạo danh sách các cấp số nhân
        /// </summary>
        /// <param name="u1_min">Phần tử đầu tiên bé nhất</param>
        /// <param name="u1_max">Phần tử đầu tiên lớn nhất</param>
        /// <param name="d_min">Công bội bé nhất</param>
        /// <param name="d_max">Công bội nhất</param>
        /// <param name="SoPhanTu_min">Số  phần tử của dãy số bé nhất</param>
        /// <param name="SoPhanTu_max">Số  phần tử của dãy số lớn nhất</param>
        /// <param name="GioiHanPhanTu">Giá trị lớn nhất của phần tử lớn nhất</param>
        /// <param name="LoaiDaySo">Loại dãy số nguồn: 1-CSC, 2- CSN</param>
        /// <param name="Key">Ký tự phân cách của các phần tử</param>
        /// <param name="CleverExerKindId">Thuộc loại bài tập</param>
        public void TaoCacDaySoChinhPhuongTuDaySo(int u1_min, int u1_max, int qd_min, int qd_max, int SoPhanTu_min, int SoPhanTu_max, int GioiHanPhanTu, int LoaiDaySo, char Key, string CleverExerKindId, string SoPhanTuAn)
        {
            ToolShareService AllToolShare = new ToolShareService();

            //string DaySoChinhPhuongTuDaySo(int SoPhanTuDaySo, int LoaiDaySo, int PhanTuDauTien, int CongSaiCongBoi, char Key)
            for (int u1 = u1_min; u1 <= u1_max; u1++)
            {
                for (int q = qd_min; q <= qd_max; q++)
                {
                    for (int RangeCount = SoPhanTu_min; RangeCount <= SoPhanTu_max; RangeCount++)
                    {
                        //Sinh dãy số là cấp số nhân có phần tử đầu tiên u1, công bội q, ký tự phân cách giữa các phần tử là Key
                        string DaySoTang = DaySoChinhPhuongTuDaySo(true, RangeCount, LoaiDaySo, u1, q, '~');
                        string DaySoTangGiam = DaySoChinhPhuongTuDaySo(false, RangeCount, LoaiDaySo, u1, q, '~');
                        string HuongDanTang = HuongDanDaySoChinhPhuongTuDaySo(true, RangeCount, LoaiDaySo, u1, q, '~');
                        string HuongDanGiam = HuongDanDaySoChinhPhuongTuDaySo(false, RangeCount, LoaiDaySo, u1, q, '~');
                        string[] DSPhanTuDaySo = DaySoTang.Split(Key);
                        int PhanTuCuoiCung = Convert.ToInt32(DSPhanTuDaySo[DSPhanTuDaySo.Length - 1]);
                        if (PhanTuCuoiCung <= GioiHanPhanTu && PhanTuCuoiCung > 0)
                        {

                            //Lấy tổ hợp các vị trí có thể ẩn
                            string[] DSViTriAn = LayViTri(RangeCount, SoPhanTuAn,  ',', '!').Split(',');

                            //Lặp từng trường hợp ẩn vị trí để đưa ra các khả năng dãy số 
                            for (int s = 0; s <= DSViTriAn.Length - 1; s++)
                            {

                                //Danh sách các vị trí ẩn
                                string[] DSVTAn = DSViTriAn[s].Trim().Split('!');

                                #region Sinh dãy tăng

                                //Khởi tạo một dãy số null
                                CleverRangeModel modelItem = new CleverRangeModel();

                                //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                modelItem.CleverRangeValue = BienDoiDaySo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', true);

                                //Lấy đáp số của các phần tử bị ẩn
                                modelItem.CleverAnswers = LayDapSo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', true);

                                //Hướng dẫn giải dãy số 
                                modelItem.CleverRangeHelp = "<table class=\"MathHelp\">" +
                                                               "<tr>" +
                                                                   "<td>" +
                                                                      "<div class=\"HelpTitle\">" +
                                                                          " Hướng dẫn:" +
                                                                      "</div> " +
                                                                       "<p class=\"HelpContent\">" +
                                                                          "Tìm các số thích hợp còn thiếu: <b>" + modelItem.CleverRangeValue.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"HelpContent\">" +
                                                                          HuongDanTang +
                                                                     " </p>" +
                                                                       "<div class=\"HelpTitle\">" +
                                                                            "Kết quả:" +
                                                                       "</div>" +
                                                                     " <p class=\"HelpReturn\"> " +
                                                                     "- Kết quả các số phải tìm là: <b>" + modelItem.CleverAnswers.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"AllHelpReturn\"> " +
                                                                            "- Kết quả các số đầy đủ là: <b>" + DaySoTang.Replace('~', ' ') + "</b>" +
                                                                      "</p>" +
                                                                   "</td>" +
                                                               "</tr>" +
                                                           "</table>";

                                //Đọc danh sách các dãy số đã có để sinh số thứ tự và mã 
                                List<CleverRangeModel> ListCleverRange = GetListCleverRange("");

                                //Sinh ngẫu nhiên mã
                                List<ListSearch> ListID = new List<ListSearch>();
                                foreach (CleverRangeModel ItemId in ListCleverRange)
                                {
                                    ListSearch IdItem = new ListSearch();
                                    IdItem.FieldSearch = ItemId.CleverRangeId;
                                    ListID.Add(IdItem);
                                }
                                modelItem.CleverRangeId = AllToolShare.GetRandomId(ListID, "CR").ToString();

                                //Lấy số thứ thự
                                List<ListFindMax> ListOrder = new List<ListFindMax>();
                                foreach (CleverRangeModel Item in ListCleverRange)
                                {
                                    ListFindMax OrderItem = new ListFindMax();
                                    OrderItem.FieldFindMax = Item.CleverRangeOrder;
                                    ListOrder.Add(OrderItem);
                                }
                                modelItem.CleverRangeOrder = AllToolShare.GetMaxOrderby(ListOrder);

                                //Gán loại bài tập toán
                                modelItem.CleverExerKindId = CleverExerKindId;

                                //Gán bậc của dãy số
                                modelItem.CleverRangeLever = DSVTAn.Length.ToString();

                                //Lưu mới dãy số vào csdl
                                SaveNewCleverRange(modelItem);

                                #endregion

                                #region Sinh dãy giảm

                                //Khởi tạo một dãy số null
                                CleverRangeModel modelItemDes = new CleverRangeModel();

                                //Ẩn các phần tử của dãy số theo DSViTriAn[s].Trim()
                                modelItemDes.CleverRangeValue = BienDoiDaySo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', false);

                                //Lấy đáp số của các phần tử bị ẩn
                                modelItemDes.CleverAnswers = LayDapSo(DaySoTang, DSViTriAn[s].Trim(), Key, '!', false);

                                //Hướng dẫn giải dãy số 
                                modelItemDes.CleverRangeHelp = "<table class=\"MathHelp\">" +
                                                               "<tr>" +
                                                                   "<td>" +
                                                                      "<div class=\"HelpTitle\">" +
                                                                          " Hướng dẫn:" +
                                                                      "</div> " +
                                                                       "<p class=\"HelpContent\">" +
                                                                          "Tìm các số thích hợp còn thiếu: <b>" + modelItemDes.CleverRangeValue.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"HelpContent\">" +
                                                                          HuongDanGiam +
                                                                     " </p>" +
                                                                       "<div class=\"HelpTitle\">" +
                                                                            "Kết quả:" +
                                                                       "</div>" +
                                                                     " <p class=\"HelpReturn\"> " +
                                                                     "- Kết quả các số phải tìm là: <b>" + modelItemDes.CleverAnswers.Replace('~', ' ') + "</b>" +
                                                                     " </p>" +
                                                                     " <p class=\"AllHelpReturn\"> " +
                                                                            "- Kết quả các số đầy đủ là: <b>" + DaySoTangGiam.Replace('~', ' ') + "</b>" +
                                                                      "</p>" +
                                                                   "</td>" +
                                                               "</tr>" +
                                                           "</table>";

                                //Đọc danh sách các dãy số đã có để sinh số thứ tự và mã 
                                List<CleverRangeModel> CleverRangeList = GetListCleverRange("");

                                //Sinh ngẫu nhiên mã
                                List<ListSearch> ListIDDes = new List<ListSearch>();
                                foreach (CleverRangeModel ItemId in CleverRangeList)
                                {
                                    ListSearch IdItem = new ListSearch();
                                    IdItem.FieldSearch = ItemId.CleverRangeId;
                                    ListIDDes.Add(IdItem);
                                }
                                modelItemDes.CleverRangeId = AllToolShare.GetRandomId(ListIDDes, "CR").ToString();

                                //Lấy số thứ thự
                                List<ListFindMax> ListOrderDes = new List<ListFindMax>();
                                foreach (CleverRangeModel Item in CleverRangeList)
                                {
                                    ListFindMax OrderItem = new ListFindMax();
                                    OrderItem.FieldFindMax = Item.CleverRangeOrder;
                                    ListOrderDes.Add(OrderItem);
                                }
                                modelItemDes.CleverRangeOrder = AllToolShare.GetMaxOrderby(ListOrderDes);

                                //Gán loại bài tập toán
                                modelItemDes.CleverExerKindId = CleverExerKindId;

                                //Gán bậc của dãy số
                                modelItemDes.CleverRangeLever = DSVTAn.Length.ToString();

                                //Lưu mới dãy số vào csdl
                                SaveNewCleverRange(modelItemDes);

                                #endregion
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Lưu dãy số vào CSDL
        /// </summary>
        /// <param name="LoaiBaiTap">Loại dãy số</param>
        /// <param name="CleverExerKindId">Thuộc loại bài tập</param>
        /// <param name="Key">Ký tự phân cách các phần tử</param>
        public void LuuDaySoVaoCSDL(int LoaiBaiTap, string CleverExerKindId, char Key)
        {
            #region Phạm vi 10
            //TaoCacCapSoCong(0, 0, 1, 1, 11, 11, 100, '~', "LBTT680098145","7"); //Các số đếm từ 1 đến 10 (660 trường hợp)
            //TaoCacCapSoCong(0, 9, 2, 9, 5, 10, 10, '~', "LBTT233875948", "2,3"); // Phép cộng, trừ trong phạm vi 10 (130 trường hợp)
            //TaoCacBoSo(0, 9, 0, 9, 1, 9, 2, 3, 4, 9, 10, 1, 1, 1, ',', '~', "LBTT1684251965", "1"); //Tạo các bộ số (4 trường hợp)
            #endregion

            #region Phạm vi 10 đến 20
            //TaoCacCapSoCong(10, 20, 1, 1, 11, 11, 20, '~', "LBTT1763962337","5,6");
            //TaoCacCapSoCong(1, 19, 4, 9, 4, 9, 20, '~', "LBTT190265181");
            //TaoCacBoSo(1, 20, 0, 19, 1, 19, 2, 3, 4, 9, 20, 1, 1, 1, ',', '~', "LBTT760667041");

            //TaoCacCapSoNhan(1, 9, 2, 9, 4, 9, 100, '~', CleverExerKindId);
            //TaoCacDaySoTuCSCVaCSN(1, 9, 2, 9, 4, 9, 1, 2, 2, 3, 150, '~', CleverExerKindId);
            //TaoCacDaySoChinhPhuongTuDaySo(1, 9, 2, 9, 4, 6, 200, 1, '~', CleverExerKindId);
            #endregion 

            #region Phạm vi 10
            //TaoCacCapSoCong(1,9,2,9,4,9,50,'~',CleverExerKindId);
            //TaoCacBoSo(1,9,1,9,2,9,2,3,4,6,1500,2,2,2, ',', '~', CleverExerKindId);
            //TaoCacCapSoNhan(1, 9, 2, 9, 4, 9, 100, '~', CleverExerKindId);
            //TaoCacDaySoTuCSCVaCSN(1, 9, 2, 9, 4, 9, 1, 2, 2, 3, 150, '~', CleverExerKindId);
            //TaoCacDaySoChinhPhuongTuDaySo(1, 9, 2, 9, 4, 6, 200, 1, '~', CleverExerKindId);
            #endregion 

            
        }

        #endregion

        #region Dãy số mới
        /// <summary>
        /// Đọc danh sách các dãy số
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <param name="PhamViPhepToan"></param>
        /// <returns></returns>
        public List<BaiToanDaySoModel> DanhSachDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            List<BaiToanDaySoModel> TatCaDanhSachDaySo = (from DaySo in ListData.BaiToanDaySos
                                                          where DaySo.ThuocKhoiLop == ThuocKhoiLop && DaySo.PhamViPhepToan == PhamViPhepToan && DaySo.PhanLoaiDaySo == PhanLoaiDaySo
                                                          orderby DaySo.SapXepThuTu descending
                                                          select new BaiToanDaySoModel
                                                        {
                                                            MaDaySo = DaySo.MaDaySo,
                                                            NoiDungDaySo = DaySo.NoiDungDaySo,
                                                            CauHoiHienThi = DaySo.CauHoiHienThi,
                                                            LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi,
                                                            NoiDungDapAn = DaySo.NoiDungDapAn,
                                                            SoLuongDapAn = DaySo.SoLuongDapAn,
                                                            KetLuanCauHoi = DaySo.KetLuanCauHoi,
                                                            SapXepThuTu = DaySo.SapXepThuTu,
                                                            ThuocKhoiLop = DaySo.ThuocKhoiLop,
                                                            PhamViPhepToan = DaySo.PhamViPhepToan,
                                                            PhanLoaiDaySo = DaySo.PhanLoaiDaySo,
                                                            SoLuongPhanTu = DaySo.SoLuongPhanTu,
                                                            GoiYTraLoi = DaySo.GoiYTraLoi,
                                                        }).ToList<BaiToanDaySoModel>();
            return TatCaDanhSachDaySo;
        }



        /// <summary>
        /// Đọc random dãy số
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public BaiToanDaySoModel GetOneDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            IEnumerable<BaiToanDaySoModel> ResultList = (from DaySo in ListData.BaiToanDaySos
                                              where DaySo.ThuocKhoiLop == ThuocKhoiLop && DaySo.PhamViPhepToan == PhamViPhepToan && DaySo.PhanLoaiDaySo == PhanLoaiDaySo
                                              orderby DaySo.SapXepThuTu descending
                                              select new BaiToanDaySoModel
                                              {
                                                  MaDaySo = DaySo.MaDaySo,
                                                  NoiDungDaySo = DaySo.NoiDungDaySo,
                                                  CauHoiHienThi = DaySo.CauHoiHienThi,
                                                  LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi,
                                                  NoiDungDapAn = DaySo.NoiDungDapAn,
                                                  NoiDungDapAnSai = DaySo.NoiDungDapAnSai,
                                                  SoLuongDapAn = DaySo.SoLuongDapAn,
                                                  KetLuanCauHoi = DaySo.KetLuanCauHoi,
                                                  SapXepThuTu = DaySo.SapXepThuTu,
                                                  ThuocKhoiLop = DaySo.ThuocKhoiLop,
                                                  PhamViPhepToan = DaySo.PhamViPhepToan,
                                                  PhanLoaiDaySo = DaySo.PhanLoaiDaySo,
                                                  SoLuongPhanTu = DaySo.SoLuongPhanTu,
                                                  GoiYTraLoi = DaySo.GoiYTraLoi,
                                              });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }


        /// <summary>
        /// Đọc dãy số đầu tiên
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public BaiToanDaySoModel DocDaySoDauTien(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            BaiToanDaySoModel DaySoDauTien = (from DaySo in ListData.BaiToanDaySos
                                              where DaySo.ThuocKhoiLop == ThuocKhoiLop && DaySo.PhamViPhepToan == PhamViPhepToan && DaySo.PhanLoaiDaySo == PhanLoaiDaySo
                                              orderby DaySo.SapXepThuTu descending
                                              select new BaiToanDaySoModel
                                              {
                                                  MaDaySo = DaySo.MaDaySo,
                                                  NoiDungDaySo = DaySo.NoiDungDaySo,
                                                  CauHoiHienThi = DaySo.CauHoiHienThi,
                                                  LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi,
                                                  NoiDungDapAn = DaySo.NoiDungDapAn,
                                                  SoLuongDapAn = DaySo.SoLuongDapAn,
                                                  KetLuanCauHoi = DaySo.KetLuanCauHoi,
                                                  SapXepThuTu = DaySo.SapXepThuTu,
                                                  ThuocKhoiLop = DaySo.ThuocKhoiLop,
                                                  PhamViPhepToan = DaySo.PhamViPhepToan,
                                                  PhanLoaiDaySo = DaySo.PhanLoaiDaySo,
                                                  SoLuongPhanTu = DaySo.SoLuongPhanTu,
                                                  GoiYTraLoi = DaySo.GoiYTraLoi,
                                              }).First<BaiToanDaySoModel>();
            return DaySoDauTien;
        }

        /// <summary>
        /// Đọc một câu hỏi đối tượng hơn kém nhau
        /// </summary>
        /// <param name="MaCauHoi"></param>
        /// <returns></returns>
        public BaiToanDaySoModel DocMotDaySo(string MaDaySo)
        {
            Guid MaDaySoDoc = new Guid(MaDaySo);
            BaiToanDaySoModel MotDaySo = (from DaySo in ListData.BaiToanDaySos
                                          where DaySo.MaDaySo == MaDaySoDoc
                                          select new BaiToanDaySoModel
                                            {
                                                MaDaySo = DaySo.MaDaySo,
                                                NoiDungDaySo = DaySo.NoiDungDaySo,
                                                CauHoiHienThi = DaySo.CauHoiHienThi,
                                                LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi,
                                                NoiDungDapAn = DaySo.NoiDungDapAn,
                                                SoLuongDapAn = DaySo.SoLuongDapAn,
                                                KetLuanCauHoi = DaySo.KetLuanCauHoi,
                                                SapXepThuTu = DaySo.SapXepThuTu,
                                                ThuocKhoiLop = DaySo.ThuocKhoiLop,
                                                PhamViPhepToan = DaySo.PhamViPhepToan,
                                                PhanLoaiDaySo = DaySo.PhanLoaiDaySo,
                                                SoLuongPhanTu = DaySo.SoLuongPhanTu,
                                                GoiYTraLoi = DaySo.GoiYTraLoi,
                                            }).SingleOrDefault<BaiToanDaySoModel>();
            return MotDaySo;
        }

        /// <summary>
        /// Thêm mới một dãy số
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotDaySo(BaiToanDaySoModel DaySo)
        {
            try
            {
                Table<BaiToanDaySo> BangDaySo = ListData.GetTable<BaiToanDaySo>();
                BaiToanDaySo DaySoItem = new BaiToanDaySo();
                DaySoItem.MaDaySo = DaySo.MaDaySo;
                DaySoItem.NoiDungDaySo = DaySo.NoiDungDaySo;
                DaySoItem.CauHoiHienThi = DaySo.CauHoiHienThi;
                DaySoItem.LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi;
                DaySoItem.NoiDungDapAn = DaySo.NoiDungDapAn;
                DaySoItem.NoiDungDapAnSai = DaySo.NoiDungDapAnSai;
                DaySoItem.SoLuongDapAn = DaySo.SoLuongDapAn;
                DaySoItem.KetLuanCauHoi = DaySo.KetLuanCauHoi;
                DaySoItem.ThuocKhoiLop = DaySo.ThuocKhoiLop;
                DaySoItem.PhamViPhepToan = DaySo.PhamViPhepToan;
                DaySoItem.PhanLoaiDaySo = DaySo.PhanLoaiDaySo;
                DaySoItem.SoLuongPhanTu = DaySo.SoLuongPhanTu;
                DaySoItem.GoiYTraLoi = DaySo.GoiYTraLoi;
                BangDaySo.InsertOnSubmit(DaySoItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được dãy số này!";
            }
        }

        /// <summary>
        /// Sửa một dyax số
        /// </summary>
        /// <param name="model"></param>
        public string SuaCauHoi(BaiToanDaySoModel DaySo)
        {
            try
            {
                var DaySoItem = ListData.BaiToanDaySos.Single(m => m.MaDaySo == DaySo.MaDaySo);
                DaySoItem.MaDaySo = DaySo.MaDaySo;
                DaySoItem.NoiDungDaySo = DaySo.NoiDungDaySo;
                DaySoItem.CauHoiHienThi = DaySo.CauHoiHienThi;
                DaySoItem.LoiGiaiCauHoi = DaySo.LoiGiaiCauHoi;
                DaySoItem.NoiDungDapAn = DaySo.NoiDungDapAn;
                DaySoItem.SoLuongDapAn = DaySo.SoLuongDapAn;
                DaySoItem.KetLuanCauHoi = DaySo.KetLuanCauHoi;
                DaySoItem.ThuocKhoiLop = DaySo.ThuocKhoiLop;
                DaySoItem.PhamViPhepToan = DaySo.PhamViPhepToan;
                DaySoItem.PhanLoaiDaySo = DaySo.PhanLoaiDaySo;
                DaySoItem.SoLuongPhanTu = DaySo.SoLuongPhanTu;
                DaySoItem.GoiYTraLoi = DaySo.GoiYTraLoi;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được dãy số này!";
            }
        }

        /// <summary>
        /// Xóa một dãy số
        /// </summary>
        /// <param name="id"></param>
        public string XoaDaySo(string MaDaySo) 
        {
            Guid MaDaySoXoa = new Guid(MaDaySo);
            try
            {
                var DaySoCanXoa = ListData.BaiToanDaySos.Where(m => m.MaDaySo == MaDaySoXoa);
                ListData.BaiToanDaySos.DeleteAllOnSubmit(DaySoCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được dãy số này!";
            }
        }

        /// <summary>
        /// Xóa nhiều dãy số
        /// </summary>
        /// <param name="id"></param>
        public string XoaNhieuDaySo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiDaySo)
        {
            try
            {
                var CacDaySo = ListData.BaiToanDaySos.Where(m => m.ThuocKhoiLop == ThuocKhoiLop).Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m => m.PhanLoaiDaySo == PhanLoaiDaySo);
                ListData.BaiToanDaySos.DeleteAllOnSubmit(CacDaySo);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các dãy số này!";
            }
        }

        
        #endregion

    }
}