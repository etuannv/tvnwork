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

namespace TNV.Web.Models
{

    #region Định nghĩa các Models

    public class ClassListModel
    {
        
        [DisplayName("Mã khối lớp:")]
        public string ClassListId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên khối lớp!")]
        [DisplayName("Tên khối lớp:")]
        public string ClassListName { get; set; }

        [DisplayName("Thông tin khối lớp:")]
        public string ClassListInfor { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int ClassListOrder { get; set; }

    }

    public class MathKindListModel
    {

        [DisplayName("Mã loại toán:")]
        public string MathKindListId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên loại toán!")]
        [DisplayName("Tên loại toán:")]
        public string MathKindListName { get; set; }

        [DisplayName("Thông tin loại toán:")]
        public string MathKindListInfor { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int MathKindListOrder { get; set; }

    }

    public class ExerKindModel
    {

        [DisplayName("Mã loại bài tập toán:")]
        public string ExerKindId { get; set; }

        [Required(ErrorMessage = "Phải nhập tên loại bài tập toán!")]
        [DisplayName("Tên loại bài tập toán:")]
        public string ExerKindName { get; set; }

        [DisplayName("Thông tin loại bài tập toán:")]
        public string ExerKindInfor { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int ExerKindOrder { get; set; }

        [DisplayName("Thuộc khoảng thời gian:")]
        public string TimeListId { get; set; }

        [DisplayName("Thuộc loại toán:")]
        public string MathKindListId { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ClassListId { get; set; }

    }

    #endregion

    public interface SystemManagerService
    {
        #region Quản lý danh mục khối lớp

        List<ClassListModel> GetClassList();
        ClassListModel GetOneClassList(string ClassListId);
        string SaveNewClassList(ClassListModel model);
        string SaveEditClassList(ClassListModel model);
        string DelClassList(string id);
        ClassListModel GetFirstClassList();

        #endregion
        
        #region Quản trị sách dạng toán
        List<MathKindListModel> GetMathKindList();
        MathKindListModel GetOneMathKindList(string MathKindListId);
        string SaveNewMathKind(MathKindListModel model);
        string SaveEditMathKind(MathKindListModel model);
        string DelMathKind(string id);
        #endregion

        #region Quản trị danh sách loại bài tập toán
        List<ExerKindModel> GetExerKind(string ClassListId, string MathKindListId, string TimeListId);
        ExerKindModel GetOneExerKind(string ExerKindId);
        string SaveNewExerKind(ExerKindModel model);
        string SaveEditExerKind(ExerKindModel model);
        string DelExerKind(string id);
        #endregion
    }

    public class SystemManagerClass : SystemManagerService
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext();

        #region Quản trị danh mục khối lớp
        /// <summary>
        /// Lấy tất cả các khối lớp
        /// </summary>
        /// <returns></returns>
        public List<ClassListModel> GetClassList()
        {
            List<ClassListModel> ClassItemList = (from ListClassItem in ListData.ClassLists
                                                  orderby ListClassItem.ClassListOrder ascending
                                                  select new ClassListModel
                                                     {
                                                         ClassListId = ListClassItem.ClassListId,
                                                         ClassListInfor = ListClassItem.ClassListInfor,
                                                         ClassListName = ListClassItem.ClassListName,
                                                         ClassListOrder = (int)ListClassItem.ClassListOrder,
                                                     }).ToList<ClassListModel>();
            return ClassItemList;
        }
        /// <summary>
        /// Lấy ra khối lớp học đầu tiên
        /// </summary>
        /// <returns></returns>
        public ClassListModel GetFirstClassList()
        {
            ClassListModel FirstClassItem = (from ClassItem in ListData.ClassLists
                                                  orderby ClassItem.ClassListOrder ascending
                                                  select new ClassListModel
                                                  {
                                                      ClassListId = ClassItem.ClassListId,
                                                      ClassListInfor = ClassItem.ClassListInfor,
                                                      ClassListName = ClassItem.ClassListName,
                                                      ClassListOrder = (int)ClassItem.ClassListOrder,
                                                  }).First<ClassListModel>();
            return FirstClassItem;
        }
        /// <summary>
        /// Lấy thông tin của một khối lớp
        /// </summary>
        /// <returns></returns>
        public ClassListModel GetOneClassList(string ClassListId) 
        {
            ClassListModel ClassItem = (from ListClassItem in ListData.ClassLists
                                        where ListClassItem.ClassListId == ClassListId
                                        select new ClassListModel
                                        {
                                            ClassListId = ListClassItem.ClassListId,
                                            ClassListInfor = ListClassItem.ClassListInfor,
                                            ClassListName = ListClassItem.ClassListName,
                                            ClassListOrder = (int)ListClassItem.ClassListOrder,
                                        }).Single<ClassListModel>();
            return ClassItem;
        }
        /// <summary>
        /// Thêm mới một khối lớp
        /// </summary>
        /// <param name="model"></param>
        public string SaveNewClassList(ClassListModel model) 
        {
            try
            {
                Table<ClassList> TableClassList = ListData.GetTable<ClassList>();
                ClassList ClassListItem = new ClassList();
                ClassListItem.ClassListId = model.ClassListId;
                ClassListItem.ClassListInfor = model.ClassListInfor;
                ClassListItem.ClassListName = model.ClassListName;
                ClassListItem.ClassListOrder = model.ClassListOrder;
                TableClassList.InsertOnSubmit(ClassListItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới được khối lớp này";
            }
        }
        /// <summary>
        /// Sửa thông tin của một khối lớp
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditClassList(ClassListModel model) 
        {
            try
            {
                var ClassListItem = ListData.ClassLists.Single(p => p.ClassListId == model.ClassListId);
                ClassListItem.ClassListInfor = model.ClassListInfor;
                ClassListItem.ClassListName = model.ClassListName;
                ClassListItem.ClassListOrder = model.ClassListOrder;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được khối lớp này";
            }
        }

        /// <summary>
        /// Xóa một khối lớp
        /// </summary>
        /// <param name="id"></param>
        public string DelClassList(string id)
        {
            try
            {
                var OneClassList = from ClassListItem in ListData.ClassLists
                                   where ClassListItem.ClassListId == id
                                   select ClassListItem;
                ListData.ClassLists.DeleteAllOnSubmit(OneClassList);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được khối lớp này";
            }
        }
        #endregion

        #region Quản trị danh sách dạng toán
        /// <summary>
        /// Lấy tất cả các dạng toán
        /// </summary>
        /// <returns></returns>
        public List<MathKindListModel> GetMathKindList()
        {
            List<MathKindListModel> MathKindList = (from MathKindItem in ListData.MathKindLists
                                                    orderby MathKindItem.MathKindListOrder ascending
                                                    select new MathKindListModel
                                                {
                                                    MathKindListId = MathKindItem.MathKindListId,
                                                    MathKindListInfor = MathKindItem.MathKindListInfor,
                                                    MathKindListName = MathKindItem.MathKindListName,
                                                    MathKindListOrder = (int)MathKindItem.MathKindListOrder,
                                                }).ToList<MathKindListModel>();
            return MathKindList;
        }
        /// <summary>
        /// Lấy thông tin của một dạng toán
        /// </summary>
        /// <returns></returns>
        public MathKindListModel GetOneMathKindList(string MathKindListId)
        {
            MathKindListModel MathKindItem = (from ItemMathKind in ListData.MathKindLists
                                              where ItemMathKind.MathKindListId == MathKindListId
                                              select new MathKindListModel
                                          {
                                              MathKindListId = ItemMathKind.MathKindListId,
                                              MathKindListInfor = ItemMathKind.MathKindListInfor,
                                              MathKindListName = ItemMathKind.MathKindListName,
                                              MathKindListOrder = (int)ItemMathKind.MathKindListOrder,
                                          }).Single<MathKindListModel>();
            return MathKindItem;
        }
        /// <summary>
        /// Thêm mới một dạng toán
        /// </summary>
        /// <param name="model"></param>
        public string SaveNewMathKind(MathKindListModel model)
        {
            try
            {
                Table<MathKindList> TableClassList = ListData.GetTable<MathKindList>();
                MathKindList MathKindItem = new MathKindList();
                MathKindItem.MathKindListId = model.MathKindListId;
                MathKindItem.MathKindListInfor = model.MathKindListInfor;
                MathKindItem.MathKindListName = model.MathKindListName;
                MathKindItem.MathKindListOrder = model.MathKindListOrder;
                TableClassList.InsertOnSubmit(MathKindItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới được dạng toán này";
            }
        }
        /// <summary>
        /// Sửa thông tin của một dạng toán
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditMathKind(MathKindListModel model)
        {
            try
            {
                var MathKindItem = ListData.MathKindLists.Single(p => p.MathKindListId == model.MathKindListId);
                MathKindItem.MathKindListInfor = model.MathKindListInfor;
                MathKindItem.MathKindListName = model.MathKindListName;
                MathKindItem.MathKindListOrder = model.MathKindListOrder;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được dạng toán này";
            }
        }

        /// <summary>
        /// Xóa một dạng toán
        /// </summary>
        /// <param name="id"></param>
        public string DelMathKind(string id)
        {
            try
            {
                var OneMathKind = from MathKindItem in ListData.MathKindLists
                                  where MathKindItem.MathKindListId == id
                                  select MathKindItem;
                ListData.MathKindLists.DeleteAllOnSubmit(OneMathKind);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được dạng toán này";
            }
        }
        #endregion

        #region Quản trị danh sách loại bài tập
        /// <summary>
        /// Đọc tất cả danh sách loại bài tập
        /// </summary>
        /// <returns></returns>
        public List<ExerKindModel> GetExerKind(string ClassListId, string MathKindListId, string TimeListId) 
        {
            List<ExerKindModel> ExerList=new List<ExerKindModel>();
            if (String.IsNullOrEmpty(ClassListId) && String.IsNullOrEmpty(MathKindListId) && String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (!String.IsNullOrEmpty(ClassListId) && String.IsNullOrEmpty(MathKindListId) && String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            orderby ExerItem.ExerKindOrder ascending
                            where ExerItem.ClassListId == ClassListId
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (String.IsNullOrEmpty(ClassListId) && !String.IsNullOrEmpty(MathKindListId) && String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            orderby ExerItem.ExerKindOrder ascending
                            where ExerItem.MathKindListId == MathKindListId
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (String.IsNullOrEmpty(ClassListId) && String.IsNullOrEmpty(MathKindListId) && !String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            where ExerItem.TimeListId == TimeListId
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (!String.IsNullOrEmpty(ClassListId) && !String.IsNullOrEmpty(MathKindListId) && String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            where ExerItem.ClassListId == ClassListId && ExerItem.TimeListId == TimeListId
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (!String.IsNullOrEmpty(ClassListId) && String.IsNullOrEmpty(MathKindListId) && !String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            where ExerItem.ClassListId == ClassListId && ExerItem.TimeListId == TimeListId
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (String.IsNullOrEmpty(ClassListId) && !String.IsNullOrEmpty(MathKindListId) && !String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            where ExerItem.MathKindListId == MathKindListId && ExerItem.TimeListId == TimeListId
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            else if (!String.IsNullOrEmpty(ClassListId) && !String.IsNullOrEmpty(MathKindListId) && !String.IsNullOrEmpty(TimeListId))
            {
                ExerList = (from ExerItem in ListData.ExerKinds
                            where ExerItem.ClassListId == ClassListId && ExerItem.MathKindListId == MathKindListId && ExerItem.TimeListId == TimeListId
                            orderby ExerItem.ExerKindOrder ascending
                            select new ExerKindModel
                            {
                                ExerKindId = ExerItem.ExerKindId,
                                ExerKindInfor = ExerItem.ExerKindInfor,
                                ExerKindName = ExerItem.ExerKindName,
                                ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                ClassListId = ExerItem.ClassListId,
                                MathKindListId = ExerItem.MathKindListId,
                                TimeListId = ExerItem.TimeListId,
                            }).ToList<ExerKindModel>();
            }
            return ExerList;
        }
        /// <summary>
        /// Lấy thông tin của một loại bài tập
        /// </summary>
        /// <param name="ExerKindId"></param>
        /// <returns></returns>
        public ExerKindModel GetOneExerKind(string ExerKindId) 
        {
            ExerKindModel OneExer = (from ExerItem in ListData.ExerKinds
                                     where ExerItem.ExerKindId == ExerKindId
                                     select new ExerKindModel
                                     {
                                         ExerKindId = ExerItem.ExerKindId,
                                         ExerKindInfor = ExerItem.ExerKindInfor,
                                         ExerKindName = ExerItem.ExerKindName,
                                         ExerKindOrder = (int)ExerItem.ExerKindOrder,
                                         ClassListId = ExerItem.ClassListId,
                                         MathKindListId = ExerItem.MathKindListId,
                                         TimeListId = ExerItem.TimeListId,
                                     }).Single<ExerKindModel>();
            return OneExer;
        }
        /// <summary>
        /// Lưu mới một loại bài tập toán
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveNewExerKind(ExerKindModel model) 
        {
            try
            {
                Table<ExerKind> TableExerKind = ListData.GetTable<ExerKind>();
                ExerKind ExerKindItem = new ExerKind();
                ExerKindItem.ExerKindId = model.ExerKindId;
                ExerKindItem.ExerKindInfor = model.ExerKindInfor;
                ExerKindItem.ExerKindName = model.ExerKindName;
                ExerKindItem.ExerKindOrder = model.ExerKindOrder;
                ExerKindItem.MathKindListId = model.MathKindListId;
                ExerKindItem.ClassListId = model.ClassListId;
                ExerKindItem.TimeListId = model.TimeListId;
                TableExerKind.InsertOnSubmit(ExerKindItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu mới được loại bài tập này";
            }            
        }
        /// <summary>
        /// Lưu sửa một loại bài tập toán
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string SaveEditExerKind(ExerKindModel model) 
        {
            try
            {
                var ExerKindItem = ListData.ExerKinds.Single(p => p.ExerKindId == model.ExerKindId);
                ExerKindItem.ExerKindInfor = model.ExerKindInfor;
                ExerKindItem.ExerKindName = model.ExerKindName;
                ExerKindItem.ExerKindOrder = model.ExerKindOrder;
                ExerKindItem.MathKindListId = model.MathKindListId;
                ExerKindItem.ClassListId = model.ClassListId;
                ExerKindItem.TimeListId = model.TimeListId;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể lưu sửa được loại bài tập này";
            }
        }
        /// <summary>
        /// Xóa một loại bài tập toán
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string DelExerKind(string id)
        {
            try
            {
                var OneExerKind = from ExerKindItem in ListData.ExerKinds
                                  where ExerKindItem.ExerKindId == id
                                  select ExerKindItem;
                ListData.ExerKinds.DeleteAllOnSubmit(OneExerKind);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được loại bài tập toán này";
            }
        }
        #endregion
    }
}