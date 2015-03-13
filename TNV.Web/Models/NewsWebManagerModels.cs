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
    public class NewsCategoryModel
    {
        
        [DisplayName("Mã chuyên mục tin:")]
        public string NewsCategoryId { get; set; }

        [Required(ErrorMessage = "Phải nhập Tên chuyên mục tin!")]
        [DisplayName("Tên chuyên mục tin:")]
        public string NewsCategoryTitle { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        [Required(ErrorMessage = "Phải nhập số thứ tự hiển thị!")]
        public int NewsCategoryOrder { get; set; }

    }
    public class NewsContentModel
    {
        [DisplayName("Mã bài viết:")]
        public string NewsId { get; set; }

        [DisplayName("Mã chuyên mục:")]
        public string NewsCatId { get; set; }

        [DisplayName("Tác giả bài viết:")]
        public string NewsAuthor { get; set; }

        [DisplayName("Lời dẫn bài viết:")]
        public string NewsNarration { get; set; }

        [DisplayName("Tiêu đề bài viết:")]
        [Required(ErrorMessage = "Phải nhập tiêu đề bài viết!")]
        public string NewsTitle { get; set; }

        [DisplayName("Nội dung bài viết:")]
        [Required(ErrorMessage = "Phải nhập nội dung bài viết!")]
        public string NewsContents { get; set; }

        [DisplayName("Ảnh đại diện:")]
        public HttpPostedFileBase NewsImage { get; set; }

        [DisplayName("Đường dẫn ảnh đại diện:")]
        public string PathNewsImage { get; set; }

        [DisplayName("Thứ tự hiển thị:")]
        public int NewsOrder { get; set; }
    }
    public interface NewsCategoryService
    {
        //Quản lý danh sách chuyên mục tin
        List<NewsCategoryModel> GetListNewsCategory();
        NewsCategoryModel GetOneNewsCategory(string NewsCategoryId);
        void SaveNewNewsCategory(NewsCategoryModel model);
        void SaveEditNewsCategory(NewsCategoryModel model);
        void DelNewsCategory(string id);
        bool CategoryInUse(string CategoryId);
        //Quản lý bài viết
        void DelNewsContent(string id);
        void SaveEditNewsContent(NewsContentModel model);
        void SaveNewNewsContent(NewsContentModel model);
        bool FindNewsContent(string NewsId);
        NewsContentModel GetOneNewsContent(string NewsId);
        List<NewsContentModel> GetListNewsContent(string NewsCatId);
        List<NewsContentModel> GetListNewsContent();
        List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum);
        NewsContentModel GetFirstNewsContent();
        List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum, string NewsCatId, string NewsId);
        List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum, string NewsCatId);
    }

    public class NewsCategoryClass : NewsCategoryService
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext();

        #region Quản trị bài viết
        /// <summary>
        /// Đọc danh sách tin bài của một chuyên mục
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<NewsContentModel> GetListNewsContent(string NewsCatId)
        {
            List<NewsContentModel> ListNewsContent = (from ListNewsContentItem in ListData.NewsContents
                                                      where ListNewsContentItem.NewsCatId == NewsCatId.Trim()
                                                      orderby ListNewsContentItem.NewsOrder descending
                                                      select new NewsContentModel
                                                       {
                                                           NewsId = ListNewsContentItem.NewsId,
                                                           NewsAuthor = ListNewsContentItem.NewsAuthor,
                                                           NewsCatId = ListNewsContentItem.NewsCatId,
                                                           NewsContents = ListNewsContentItem.NewsContents,
                                                           PathNewsImage = ListNewsContentItem.NewsImage,
                                                           NewsNarration = ListNewsContentItem.NewsNarration,
                                                           NewsOrder = (int)ListNewsContentItem.NewsOrder,
                                                           NewsTitle = ListNewsContentItem.NewsTitle
                                                       }).ToList<NewsContentModel>();
            return ListNewsContent;
        }
        /// <summary>
        /// Đọc tất cả tin bài
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<NewsContentModel> GetListNewsContent() 
        {
            List<NewsContentModel> ListNewsContent = (from ListNewsContentItem in ListData.NewsContents
                                                      orderby ListNewsContentItem.NewsOrder descending
                                                      select new NewsContentModel
                                                      {
                                                          NewsId = ListNewsContentItem.NewsId,
                                                          NewsAuthor = ListNewsContentItem.NewsAuthor,
                                                          NewsCatId = ListNewsContentItem.NewsCatId,
                                                          NewsContents = ListNewsContentItem.NewsContents,
                                                          PathNewsImage = ListNewsContentItem.NewsImage,
                                                          NewsNarration = ListNewsContentItem.NewsNarration,
                                                          NewsOrder = (int)ListNewsContentItem.NewsOrder,
                                                          NewsTitle = ListNewsContentItem.NewsTitle
                                                      }).ToList<NewsContentModel>();
            return ListNewsContent;
        }
        /// <summary>
        /// Lấy một số tin bài từ vị trí a, lấy b bản ghi
        /// </summary>
        /// <param name="NumNews"></param>
        /// <returns></returns>
        public List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum)
        {
            List<NewsContentModel> ListNewsContent = (from ListNewsContentItem in ListData.NewsContents
                                                      orderby ListNewsContentItem.NewsOrder descending
                                                      select new NewsContentModel
                                                      {
                                                          NewsId = ListNewsContentItem.NewsId,
                                                          NewsAuthor = ListNewsContentItem.NewsAuthor,
                                                          NewsCatId = ListNewsContentItem.NewsCatId,
                                                          NewsContents = ListNewsContentItem.NewsContents,
                                                          PathNewsImage = ListNewsContentItem.NewsImage,
                                                          NewsNarration = ListNewsContentItem.NewsNarration,
                                                          NewsOrder = (int)ListNewsContentItem.NewsOrder,
                                                          NewsTitle = ListNewsContentItem.NewsTitle
                                                      }).Skip(RecordFrom).Take(RecordNum).ToList<NewsContentModel>();
            return ListNewsContent;
        }

        /// <summary>
        /// Lấy một số tin khác của một chuyên mục
        /// </summary>
        /// <param name="RecordFrom"></param>
        /// <param name="RecordNum"></param>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum, string NewsCatId, string NewsId) 
        {
            List<NewsContentModel> ListNewsContent = (from ListNewsContentItem in ListData.NewsContents
                                                      where ListNewsContentItem.NewsCatId == NewsCatId && ListNewsContentItem.NewsId != NewsId
                                                      orderby ListNewsContentItem.NewsOrder descending
                                                      select new NewsContentModel
                                                      {
                                                          NewsId = ListNewsContentItem.NewsId,
                                                          NewsAuthor = ListNewsContentItem.NewsAuthor,
                                                          NewsCatId = ListNewsContentItem.NewsCatId,
                                                          NewsContents = ListNewsContentItem.NewsContents,
                                                          PathNewsImage = ListNewsContentItem.NewsImage,
                                                          NewsNarration = ListNewsContentItem.NewsNarration,
                                                          NewsOrder = (int)ListNewsContentItem.NewsOrder,
                                                          NewsTitle = ListNewsContentItem.NewsTitle
                                                      }).Skip(RecordFrom).Take(RecordNum).ToList<NewsContentModel>();
            return ListNewsContent;
        }
        /// <summary>
        /// Lấy một số tin bài đầu tiên của một chuyên mục
        /// </summary>
        /// <param name="RecordFrom"></param>
        /// <param name="RecordNum"></param>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public List<NewsContentModel> GetListNewsContent(int RecordFrom, int RecordNum, string NewsCatId)
        {
            List<NewsContentModel> ListNewsContent = (from ListNewsContentItem in ListData.NewsContents
                                                      where ListNewsContentItem.NewsCatId == NewsCatId
                                                      orderby ListNewsContentItem.NewsOrder descending
                                                      select new NewsContentModel
                                                      {
                                                          NewsId = ListNewsContentItem.NewsId,
                                                          NewsAuthor = ListNewsContentItem.NewsAuthor,
                                                          NewsCatId = ListNewsContentItem.NewsCatId,
                                                          NewsContents = ListNewsContentItem.NewsContents,
                                                          PathNewsImage = ListNewsContentItem.NewsImage,
                                                          NewsNarration = ListNewsContentItem.NewsNarration,
                                                          NewsOrder = (int)ListNewsContentItem.NewsOrder,
                                                          NewsTitle = ListNewsContentItem.NewsTitle
                                                      }).Skip(RecordFrom).Take(RecordNum).ToList<NewsContentModel>();
            return ListNewsContent;
        }
        /// <summary>
        /// Lấy bài viết đầu tiên
        /// </summary>
        /// <returns></returns>
        public NewsContentModel GetFirstNewsContent()
        {
            NewsContentModel FirstNewsContent = (from FirstNewsContentItem in ListData.NewsContents
                                                orderby FirstNewsContentItem.NewsOrder descending
                                                select new NewsContentModel
                                                {
                                                    NewsId = FirstNewsContentItem.NewsId,
                                                    NewsAuthor = FirstNewsContentItem.NewsAuthor,
                                                    NewsCatId = FirstNewsContentItem.NewsCatId,
                                                    NewsContents = FirstNewsContentItem.NewsContents,
                                                    PathNewsImage = FirstNewsContentItem.NewsImage,
                                                    NewsNarration = FirstNewsContentItem.NewsNarration,
                                                    NewsOrder = (int)FirstNewsContentItem.NewsOrder,
                                                    NewsTitle = FirstNewsContentItem.NewsTitle
                                                }).First<NewsContentModel>();
            return FirstNewsContent;
        }
        /// <summary>
        /// Lấy một tin bài bởi Id của nó
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public NewsContentModel GetOneNewsContent(string NewsId) 
        {
            NewsContentModel OneNewsContent = (from OneNewsContentItem in ListData.NewsContents
                                               where OneNewsContentItem.NewsId == NewsId
                                               select new NewsContentModel
                                               {
                                                   NewsId = OneNewsContentItem.NewsId,
                                                   NewsAuthor = OneNewsContentItem.NewsAuthor,
                                                   NewsCatId = OneNewsContentItem.NewsCatId,
                                                   NewsContents = OneNewsContentItem.NewsContents,
                                                   PathNewsImage = OneNewsContentItem.NewsImage,
                                                   NewsNarration = OneNewsContentItem.NewsNarration,
                                                   NewsOrder = (int)OneNewsContentItem.NewsOrder,
                                                   NewsTitle = OneNewsContentItem.NewsTitle
                                               }).Single<NewsContentModel>();
            return OneNewsContent;
        }
        /// <summary>
        /// Tìm một tin bài bởi Id của nó
        /// </summary>
        /// <param name="NewsId"></param>
        /// <returns></returns>
        public bool FindNewsContent(string NewsId) 
        {
            NewsContentModel OneNewsContent = (from OneNewsContentItem in ListData.NewsContents
                                               where OneNewsContentItem.NewsId == NewsId
                                               select new NewsContentModel
                                               {
                                                   NewsId = OneNewsContentItem.NewsId,
                                                   NewsAuthor = OneNewsContentItem.NewsAuthor,
                                                   NewsCatId = OneNewsContentItem.NewsCatId,
                                                   NewsContents = OneNewsContentItem.NewsContents,
                                                   PathNewsImage = OneNewsContentItem.NewsImage,
                                                   NewsNarration = OneNewsContentItem.NewsNarration,
                                                   NewsOrder = (int)OneNewsContentItem.NewsOrder,
                                                   NewsTitle = OneNewsContentItem.NewsTitle
                                               }).Single<NewsContentModel>();
            if (OneNewsContent != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Kiểm tra chuyên mục đã có tin bài hay chưa
        /// </summary>
        /// <param name="CategoryId"></param>
        /// <returns></returns>
        public bool CategoryInUse(string CategoryId)
        {
            int CountNewsContent = (from OneNewsContentItem in ListData.NewsContents
                                    where OneNewsContentItem.NewsCatId == CategoryId
                                    select new NewsContentModel
                                    {
                                        NewsId = OneNewsContentItem.NewsId,
                                        NewsAuthor = OneNewsContentItem.NewsAuthor,
                                        NewsCatId = OneNewsContentItem.NewsCatId,
                                        NewsContents = OneNewsContentItem.NewsContents,
                                        PathNewsImage = OneNewsContentItem.NewsImage,
                                        NewsNarration = OneNewsContentItem.NewsNarration,
                                        NewsOrder = (int)OneNewsContentItem.NewsOrder,
                                        NewsTitle = OneNewsContentItem.NewsTitle
                                    }).ToList<NewsContentModel>().Count;
            if (CountNewsContent > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Thêm mới tin bài
        /// </summary>
        /// <param name="model"></param>
        public void SaveNewNewsContent(NewsContentModel model) 
        {
            Table<NewsContent> TableNewsCategory = ListData.GetTable<NewsContent>();
            NewsContent NewsContentItem = new NewsContent();
            NewsContentItem.NewsId = model.NewsId;
            NewsContentItem.NewsAuthor = model.NewsAuthor;
            NewsContentItem.NewsCatId = model.NewsCatId;
            NewsContentItem.NewsContents = model.NewsContents;
            NewsContentItem.NewsImage = model.PathNewsImage;
            NewsContentItem.NewsNarration = model.NewsNarration;
            NewsContentItem.NewsOrder = model.NewsOrder;
            NewsContentItem.NewsTitle = model.NewsTitle;
            TableNewsCategory.InsertOnSubmit(NewsContentItem);
            ListData.SubmitChanges();
        }
        /// <summary>
        /// Sửa một tin bài
        /// </summary>
        /// <param name="model"></param>
        public void SaveEditNewsContent(NewsContentModel model) 
        {
            var NewsContentItem = ListData.NewsContents.Single(p => p.NewsId == model.NewsId);
            NewsContentItem.NewsId = model.NewsId;
            NewsContentItem.NewsAuthor = model.NewsAuthor;
            NewsContentItem.NewsCatId = model.NewsCatId;
            NewsContentItem.NewsContents = model.NewsContents;
            NewsContentItem.NewsImage = model.PathNewsImage;
            NewsContentItem.NewsNarration = model.NewsNarration;
            NewsContentItem.NewsOrder = model.NewsOrder;
            NewsContentItem.NewsTitle = model.NewsTitle;
            ListData.SubmitChanges();
        }

        /// <summary>
        /// Xóa một tin bài
        /// </summary>
        /// <param name="id"></param>
        public void DelNewsContent(string id) 
        {
            var OneNewsContent = from NewsContentItem in ListData.NewsContents
                                 where NewsContentItem.NewsId == id
                                 select NewsContentItem;
            ListData.NewsContents.DeleteAllOnSubmit(OneNewsContent);
            ListData.SubmitChanges();
        }
        #endregion

        #region Quản trị chuyên mục tin
        /// <summary>
        /// Lấy tất cả các chuyên mục tin
        /// </summary>
        /// <returns></returns>
        public List<NewsCategoryModel> GetListNewsCategory()
        {
            List<NewsCategoryModel> ListNewsCategory = (from ListNewsCategoryItem in ListData.NewsCategories
                                                        select new NewsCategoryModel
                                                    {
                                                        NewsCategoryId = ListNewsCategoryItem.NewsCategoryId,
                                                        NewsCategoryTitle = ListNewsCategoryItem.NewsCategoryTitle,
                                                        NewsCategoryOrder = (int)ListNewsCategoryItem.NewsCategoryOrder
                                                    }).ToList<NewsCategoryModel>();
            return ListNewsCategory;
        }
        /// <summary>
        /// Lấy thông tin của một chuyên mục tin
        /// </summary>
        /// <returns></returns>
        public NewsCategoryModel GetOneNewsCategory(string NewsCategoryId) 
        {
            NewsCategoryModel OneNewsCategory = (from NewsCategoryItem in ListData.NewsCategories
                                                 where NewsCategoryItem.NewsCategoryId == NewsCategoryId
                                                 select new NewsCategoryModel
                                                 {
                                                     NewsCategoryId = NewsCategoryItem.NewsCategoryId,
                                                     NewsCategoryTitle = NewsCategoryItem.NewsCategoryTitle,
                                                     NewsCategoryOrder = (int)NewsCategoryItem.NewsCategoryOrder
                                                 }).Single<NewsCategoryModel>();
            return OneNewsCategory;
        }
        /// <summary>
        /// Thêm mới một chuyên mục tin
        /// </summary>
        /// <param name="model"></param>
        public void SaveNewNewsCategory(NewsCategoryModel model) 
        {
            Table<NewsCategory> TableNewsCategory = ListData.GetTable<NewsCategory>();
            NewsCategory NewsCategory = new NewsCategory();
            NewsCategory.NewsCategoryId = model.NewsCategoryId;
            NewsCategory.NewsCategoryOrder = model.NewsCategoryOrder;
            NewsCategory.NewsCategoryTitle = model.NewsCategoryTitle;
            TableNewsCategory.InsertOnSubmit(NewsCategory);
            ListData.SubmitChanges();
        }
        /// <summary>
        /// Sửa thông tin của một chuyên mục tin
        /// </summary>
        /// <param name="model"></param>
        public void SaveEditNewsCategory(NewsCategoryModel model) 
        {
            var NewsCategoryItem = ListData.NewsCategories.Single(p => p.NewsCategoryId == model.NewsCategoryId);
            NewsCategoryItem.NewsCategoryOrder = model.NewsCategoryOrder;
            NewsCategoryItem.NewsCategoryTitle = model.NewsCategoryTitle;
            ListData.SubmitChanges();
        }

        /// <summary>
        /// Xóa một chuyên mục tin
        /// </summary>
        /// <param name="id"></param>
        public void DelNewsCategory(string id)
        {
            var OneNewsCategory = from NewsCategoryItem in ListData.NewsCategories
                                  where NewsCategoryItem.NewsCategoryId == id
                                  select NewsCategoryItem;
            ListData.NewsCategories.DeleteAllOnSubmit(OneNewsCategory);
            ListData.SubmitChanges();
        }
        #endregion
    }
}