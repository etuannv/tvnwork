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
    public class PhepToanHaiSoHangModel
    {
        [DisplayName("Mã phép toán:")]
        public Guid MaCauHoi { get; set; }

        [DisplayName("Số hạng thứ nhất:")]
        [Required(ErrorMessage = "Phải nhập Số hạng thứ nhất!")]
        public string SoHangThuNhat { get; set; }

        [DisplayName("Phép toán:")]
        [Required(ErrorMessage = "Phải nhập Phép toán!")]
        public string PhepToan { get; set; }

        [DisplayName("Số hạng thứ hai:")]
        [Required(ErrorMessage = "Phải nhập Số hạng thứ hai!")]
        public string SoHangThuHai { get; set; }

        [DisplayName("Dấu quan hệ của phép toán:")]
        [Required(ErrorMessage = "Phải nhập Dấu quan hệ của phép toán!")]
        public string DauQuanHe { get; set; }

        [DisplayName("Kết quả phép toán:")]
        [Required(ErrorMessage = "Phải nhập Kết quả phép toán!")]
        public string KetQuaPhepToan { get; set; }

        [DisplayName("Đáp án của câu hỏi:")]
        [Required(ErrorMessage = "Phải nhập Đáp án của câu hỏi!")]
        public string DapAn { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int SapXepThuTu { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }
    }

    public interface PhepToanHaiSoHangService
    {
        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }

        #region Quản trị danh sách các câu hỏi tính toán chứa một phép toán
        List<PhepToanHaiSoHangModel> ListQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan);
        PhepToanHaiSoHangModel FirstQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan);
        PhepToanHaiSoHangModel RandomQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan);
        PhepToanHaiSoHangModel OneQuesOneOperator(string MaCauHoi);
        string SaveAddQuesOneOperator(PhepToanHaiSoHangModel model);
        string SaveEditQuesOneOperator(PhepToanHaiSoHangModel model);
        string DelQuesOneOperator(string MaCauHoi);
        string DelAllQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan);
        #endregion
    }

    public class PhepToanHaiSoHangClass : PhepToanHaiSoHangService
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

        ToanThongMinhDataContext LinqContext = new ToanThongMinhDataContext();

        #region Quản trị danh sách các câu hỏi tính toán chứa một phép toán 
        /// <summary>
        /// Đọc danh sách các câu hỏi tính toán một phép toán
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <param name="PhamViPhepToan"></param>
        /// <returns></returns>
        public List<PhepToanHaiSoHangModel> ListQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            List<PhepToanHaiSoHangModel> AllQuestionOneOperator = (from OneOperator in LinqContext.MotPhepToans
                                                             where OneOperator.ThuocKhoiLop == ThuocKhoiLop && OneOperator.PhamViPhepToan == PhamViPhepToan
                                                             orderby OneOperator.SapXepThuTu descending
                                                             select new PhepToanHaiSoHangModel
                                                            {
                                                                MaCauHoi = OneOperator.MaCauHoi,
                                                                SoHangThuNhat = OneOperator.SoHangThuNhat,
                                                                PhepToan = OneOperator.PhepToan,
                                                                SoHangThuHai = OneOperator.SoHangThuHai,
                                                                DauQuanHe=OneOperator.DauQuanHe,
                                                                KetQuaPhepToan = OneOperator.KetQuaPhepToan,
                                                                DapAn = OneOperator.DapAn,
                                                                SapXepThuTu = OneOperator.SapXepThuTu != null ? (int)OneOperator.SapXepThuTu : 0,
                                                                ThuocKhoiLop = OneOperator.ThuocKhoiLop,
                                                                PhamViPhepToan=OneOperator.PhamViPhepToan
                                                            }).ToList<PhepToanHaiSoHangModel>();
            return AllQuestionOneOperator;
        }

        /// <summary>
        /// Đọc câu hỏi tính toán một phép toán đầu tiên của khối lớp
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public PhepToanHaiSoHangModel FirstQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            PhepToanHaiSoHangModel FirstQuestionOneOperator = (from OneOperator in LinqContext.MotPhepToans
                                                         where OneOperator.ThuocKhoiLop == ThuocKhoiLop && OneOperator.PhamViPhepToan == PhamViPhepToan
                                                         orderby OneOperator.SapXepThuTu ascending
                                                         select new PhepToanHaiSoHangModel
                                                         {
                                                             MaCauHoi = OneOperator.MaCauHoi,
                                                             SoHangThuNhat = OneOperator.SoHangThuNhat,
                                                             PhepToan = OneOperator.PhepToan,
                                                             SoHangThuHai = OneOperator.SoHangThuHai,
                                                             DauQuanHe = OneOperator.DauQuanHe,
                                                             KetQuaPhepToan = OneOperator.KetQuaPhepToan,
                                                             DapAn = OneOperator.DapAn,
                                                             SapXepThuTu = OneOperator.SapXepThuTu != null ? (int)OneOperator.SapXepThuTu : 0,
                                                             ThuocKhoiLop = OneOperator.ThuocKhoiLop,
                                                             PhamViPhepToan = OneOperator.PhamViPhepToan
                                                         }).FirstOrDefault<PhepToanHaiSoHangModel>();
            return FirstQuestionOneOperator;
        }

        /// <summary>
        /// Đọc câu hỏi ngẫu nhiên
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public PhepToanHaiSoHangModel RandomQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            
            
            
            //int index = new Random().Next(count);
            IEnumerable<PhepToanHaiSoHangModel> ResultList = (from OneOperator in LinqContext.MotPhepToans
                                                               where OneOperator.ThuocKhoiLop == ThuocKhoiLop && OneOperator.PhamViPhepToan == PhamViPhepToan
                                                               select new PhepToanHaiSoHangModel
                                                               {
                                                                   MaCauHoi = OneOperator.MaCauHoi,
                                                                   SoHangThuNhat = OneOperator.SoHangThuNhat,
                                                                   PhepToan = OneOperator.PhepToan,
                                                                   SoHangThuHai = OneOperator.SoHangThuHai,
                                                                   DauQuanHe = OneOperator.DauQuanHe,
                                                                   KetQuaPhepToan = OneOperator.KetQuaPhepToan,
                                                                   DapAn = OneOperator.DapAn,
                                                                   SapXepThuTu = OneOperator.SapXepThuTu != null ? (int)OneOperator.SapXepThuTu : 0,
                                                                   ThuocKhoiLop = OneOperator.ThuocKhoiLop,
                                                                   PhamViPhepToan = OneOperator.PhamViPhepToan
                                                               });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Đọc một câu hỏi tính toán một phép toán
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public PhepToanHaiSoHangModel OneQuesOneOperator(string MaCauHoi)
        {
            Guid MaCauHoiDoc = new Guid(MaCauHoi);
            PhepToanHaiSoHangModel QuestionOneOperator = (from OneOperator in LinqContext.MotPhepToans
                                                    where OneOperator.MaCauHoi == MaCauHoiDoc
                                                    select new PhepToanHaiSoHangModel
                                                    {
                                                        MaCauHoi = OneOperator.MaCauHoi,
                                                        SoHangThuNhat = OneOperator.SoHangThuNhat,
                                                        PhepToan = OneOperator.PhepToan,
                                                        SoHangThuHai = OneOperator.SoHangThuHai,
                                                        DauQuanHe = OneOperator.DauQuanHe,
                                                        KetQuaPhepToan = OneOperator.KetQuaPhepToan,
                                                        DapAn = OneOperator.DapAn,
                                                        SapXepThuTu = OneOperator.SapXepThuTu != null ? (int)OneOperator.SapXepThuTu : 0,
                                                        ThuocKhoiLop = OneOperator.ThuocKhoiLop,
                                                        PhamViPhepToan = OneOperator.PhamViPhepToan
                                                    }).SingleOrDefault<PhepToanHaiSoHangModel>();
            return QuestionOneOperator;
        }
       
        /// <summary>
        /// Thêm mới một câu hỏi chứa một phép toán
        /// </summary>
        /// <param name="model"></param>
        public string SaveAddQuesOneOperator(PhepToanHaiSoHangModel model)
        {
            try
            {
                Table<MotPhepToan> TableQuesOneOperator = LinqContext.GetTable<MotPhepToan>();
                MotPhepToan NewQuesOneOperator = new MotPhepToan();
                NewQuesOneOperator.MaCauHoi = model.MaCauHoi;
                NewQuesOneOperator.SoHangThuNhat = model.SoHangThuNhat;
                NewQuesOneOperator.PhepToan = model.PhepToan;
                NewQuesOneOperator.SoHangThuHai = model.SoHangThuHai;
                NewQuesOneOperator.DauQuanHe = model.DauQuanHe;
                NewQuesOneOperator.KetQuaPhepToan = model.KetQuaPhepToan;
                NewQuesOneOperator.DapAn = model.DapAn;
                NewQuesOneOperator.ThuocKhoiLop = model.ThuocKhoiLop;
                NewQuesOneOperator.PhamViPhepToan = model.PhamViPhepToan;
                TableQuesOneOperator.InsertOnSubmit(NewQuesOneOperator);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được câu hỏi này!";
            }
        }

        /// <summary>
        /// Sửa một câu hỏi chứa một phép toán
        /// </summary>
        /// <param name="model"></param>
        public string SaveEditQuesOneOperator(PhepToanHaiSoHangModel model)
        {
            try
            {
                var EditQuesOneOperator = LinqContext.MotPhepToans.Single(m => m.MaCauHoi == model.MaCauHoi);
                EditQuesOneOperator.SoHangThuNhat = model.SoHangThuNhat;
                EditQuesOneOperator.PhepToan = model.PhepToan;
                EditQuesOneOperator.SoHangThuHai = model.SoHangThuHai;
                EditQuesOneOperator.DauQuanHe = model.DauQuanHe;
                EditQuesOneOperator.KetQuaPhepToan = model.KetQuaPhepToan;
                EditQuesOneOperator.DapAn = model.DapAn;
                EditQuesOneOperator.ThuocKhoiLop = model.ThuocKhoiLop;
                EditQuesOneOperator.PhamViPhepToan = model.PhamViPhepToan;
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được câu hỏi này!";
            }
        }

        /// <summary>
        /// Xóa một câu hỏi một phép toán
        /// </summary>
        /// <param name="id"></param>
        public string DelQuesOneOperator(string MaCauHoi)
        {
            Guid MaCauHoiXoa = new Guid(MaCauHoi);
            try
            {
                var QuesOneOperator = LinqContext.MotPhepToans.Where(m => m.MaCauHoi == MaCauHoiXoa);
                LinqContext.MotPhepToans.DeleteAllOnSubmit(QuesOneOperator);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được câu hỏi này!";
            }
        }

        /// <summary>
        /// Xóa tất cả các câu hỏi một phép toán
        /// </summary>
        /// <param name="id"></param>
        public string DelAllQuesOneOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            try
            {
                var QuesOneOperator = LinqContext.MotPhepToans.Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m=>m.ThuocKhoiLop==ThuocKhoiLop);
                LinqContext.MotPhepToans.DeleteAllOnSubmit(QuesOneOperator);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được câu hỏi này!";
            }
        }
        #endregion

    }
}