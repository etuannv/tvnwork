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
    public class PhepToanBaSoHangModel
    {
        [DisplayName("Mã phép toán:")]
        public Guid MaCauHoi { get; set; }

        [DisplayName("Số hạng thứ nhất:")]
        [Required(ErrorMessage = "Phải nhập Số hạng thứ nhất!")]
        public string SoHangThuNhat { get; set; }

        [DisplayName("Phép toán thứ nhất:")]
        [Required(ErrorMessage = "Phải nhập Phép toán thứ nhất!")]
        public string PhepToanThuNhat { get; set; }

        [DisplayName("Số hạng thứ hai:")]
        [Required(ErrorMessage = "Phải nhập Số hạng thứ hai!")]
        public string SoHangThuHai { get; set; }

        [DisplayName("Phép toán thứ hai:")]
        [Required(ErrorMessage = "Phải nhập Phép toán thứ hai!")]
        public string PhepToanThuHai { get; set; }

        [DisplayName("Số hạng thứ ba:")]
        [Required(ErrorMessage = "Phải nhập Số hạng thứ ba!")]
        public string SoHangThuBa { get; set; }

        [DisplayName("Dấu quan hệ của phép toán:")]
        [Required(ErrorMessage = "Phải nhập Dấu quan hệ của phép toán!")]
        public string QuanHePhepToan { get; set; }

        [DisplayName("Kết quả phép toán:")]
        [Required(ErrorMessage = "Phải nhập Kết quả phép toán!")]
        public string KetQuaPhepToan { get; set; }

        [DisplayName("Đáp án thứ nhất của câu hỏi:")]
        [Required(ErrorMessage = "Phải nhập Đáp án thứ nhất của câu hỏi!")]
        public string DapAnThuNhat { get; set; }

        [DisplayName("Đáp án thứ hai của câu hỏi:")]
        [Required(ErrorMessage = "Phải nhập Đáp án thứ hai của câu hỏi!")]
        public string DapAnThuHai { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int SapXepThuTu { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }
    }

    public interface PhepToanBaSoHangService
    {
        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }

        #region Quản trị danh sách các câu hỏi tính toán chứa một phép toán
        List<PhepToanBaSoHangModel> ListQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan);
        PhepToanBaSoHangModel FirstQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan);
        PhepToanBaSoHangModel TwoQuesTwoOperator(string MaCauHoi);
        string SaveAddQuesTwoOperator(PhepToanBaSoHangModel model);
        string SaveEditQuesTwoOperator(PhepToanBaSoHangModel model);
        string DelQuesTwoOperator(string MaCauHoi);
        string DelAllQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan);
        #endregion
    }

    public class PhepToanBaSoHangClass : PhepToanBaSoHangService
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

        #region Quản trị danh sách các câu hỏi tính toán chứa hai phép toán 
        /// <summary>
        /// Đọc danh sách các câu hỏi tính toán hai phép toán
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <param name="PhamViPhepToan"></param>
        /// <returns></returns>
        public List<PhepToanBaSoHangModel> ListQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            List<PhepToanBaSoHangModel> AllQuestionTwoOperator = (from TwoOperator in LinqContext.PhepToanBaSoHangs
                                                             where TwoOperator.ThuocKhoiLop == ThuocKhoiLop && TwoOperator.PhamViPhepToan == PhamViPhepToan
                                                             orderby TwoOperator.SapXepThuTu descending
                                                             select new PhepToanBaSoHangModel
                                                            {
                                                                MaCauHoi = TwoOperator.MaCauHoi,
                                                                SoHangThuNhat = TwoOperator.SoHangThuNhat,
                                                                PhepToanThuNhat = TwoOperator.PhepToanThuNhat,
                                                                SoHangThuHai = TwoOperator.SoHangThuHai,
                                                                PhepToanThuHai = TwoOperator.PhepToanThuHai,
                                                                SoHangThuBa = TwoOperator.SoHangThuBa,
                                                                QuanHePhepToan = TwoOperator.QuanHePhepToan,
                                                                KetQuaPhepToan = TwoOperator.KetQuaPhepToan,
                                                                DapAnThuNhat = TwoOperator.DapAnThuNhat,
                                                                DapAnThuHai = TwoOperator.DapAnThuHai,
                                                                SapXepThuTu = TwoOperator.SapXepThuTu != null ? (int)TwoOperator.SapXepThuTu : 0,
                                                                ThuocKhoiLop = TwoOperator.ThuocKhoiLop,
                                                                PhamViPhepToan=TwoOperator.PhamViPhepToan
                                                            }).ToList<PhepToanBaSoHangModel>();
            return AllQuestionTwoOperator;
        }

        /// <summary>
        /// Đọc câu hỏi tính toán một phép toán đầu tiên của khối lớp
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public PhepToanBaSoHangModel FirstQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            PhepToanBaSoHangModel FirstQuestionTwoOperator = (from TwoOperator in LinqContext.PhepToanBaSoHangs
                                                         where TwoOperator.ThuocKhoiLop == ThuocKhoiLop && TwoOperator.PhamViPhepToan == PhamViPhepToan
                                                         orderby TwoOperator.SapXepThuTu ascending
                                                         select new PhepToanBaSoHangModel
                                                         {
                                                             MaCauHoi = TwoOperator.MaCauHoi,
                                                             SoHangThuNhat = TwoOperator.SoHangThuNhat,
                                                             PhepToanThuNhat = TwoOperator.PhepToanThuNhat,
                                                             SoHangThuHai = TwoOperator.SoHangThuHai,
                                                             PhepToanThuHai = TwoOperator.PhepToanThuHai,
                                                             SoHangThuBa = TwoOperator.SoHangThuBa,
                                                             QuanHePhepToan = TwoOperator.QuanHePhepToan,
                                                             KetQuaPhepToan = TwoOperator.KetQuaPhepToan,
                                                             DapAnThuNhat = TwoOperator.DapAnThuNhat,
                                                             DapAnThuHai = TwoOperator.DapAnThuHai,
                                                             SapXepThuTu = TwoOperator.SapXepThuTu != null ? (int)TwoOperator.SapXepThuTu : 0,
                                                             ThuocKhoiLop = TwoOperator.ThuocKhoiLop,
                                                             PhamViPhepToan = TwoOperator.PhamViPhepToan
                                                         }).FirstOrDefault<PhepToanBaSoHangModel>();
            return FirstQuestionTwoOperator;
        }

        /// <summary>
        /// Đọc một câu hỏi tính toán một phép toán
        /// </summary>
        /// <param name="NewsCatId"></param>
        /// <returns></returns>
        public PhepToanBaSoHangModel TwoQuesTwoOperator(string MaCauHoi)
        {
            Guid MaCauHoiDoc = new Guid(MaCauHoi);
            PhepToanBaSoHangModel QuestionTwoOperator = (from TwoOperator in LinqContext.PhepToanBaSoHangs
                                                    where TwoOperator.MaCauHoi == MaCauHoiDoc
                                                    select new PhepToanBaSoHangModel
                                                    {
                                                        MaCauHoi = TwoOperator.MaCauHoi,
                                                        SoHangThuNhat = TwoOperator.SoHangThuNhat,
                                                        PhepToanThuNhat = TwoOperator.PhepToanThuNhat,
                                                        SoHangThuHai = TwoOperator.SoHangThuHai,
                                                        PhepToanThuHai = TwoOperator.PhepToanThuHai,
                                                        SoHangThuBa = TwoOperator.SoHangThuBa,
                                                        QuanHePhepToan = TwoOperator.QuanHePhepToan,
                                                        KetQuaPhepToan = TwoOperator.KetQuaPhepToan,
                                                        DapAnThuNhat = TwoOperator.DapAnThuNhat,
                                                        DapAnThuHai = TwoOperator.DapAnThuHai,
                                                        SapXepThuTu = TwoOperator.SapXepThuTu != null ? (int)TwoOperator.SapXepThuTu : 0,
                                                        ThuocKhoiLop = TwoOperator.ThuocKhoiLop,
                                                        PhamViPhepToan = TwoOperator.PhamViPhepToan
                                                    }).SingleOrDefault<PhepToanBaSoHangModel>();
            return QuestionTwoOperator;
        }
       
        /// <summary>
        /// Thêm mới một câu hỏi chứa một phép toán
        /// </summary>
        /// <param name="model"></param>
        public string SaveAddQuesTwoOperator(PhepToanBaSoHangModel model)
        {
            try
            {
                Table<PhepToanBaSoHang> TableQuesTwoOperator = LinqContext.GetTable<PhepToanBaSoHang>();
                PhepToanBaSoHang NewQuesTwoOperator = new PhepToanBaSoHang();
                NewQuesTwoOperator.MaCauHoi = model.MaCauHoi;
                NewQuesTwoOperator.SoHangThuNhat = model.SoHangThuNhat;
                NewQuesTwoOperator.PhepToanThuNhat = model.PhepToanThuNhat;
                NewQuesTwoOperator.SoHangThuHai = model.SoHangThuHai;
                NewQuesTwoOperator.PhepToanThuHai = model.PhepToanThuHai;
                NewQuesTwoOperator.SoHangThuBa = model.SoHangThuBa;
                NewQuesTwoOperator.QuanHePhepToan = model.QuanHePhepToan;
                NewQuesTwoOperator.KetQuaPhepToan = model.KetQuaPhepToan;
                NewQuesTwoOperator.DapAnThuNhat = model.DapAnThuNhat;
                NewQuesTwoOperator.DapAnThuHai = model.DapAnThuHai;
                NewQuesTwoOperator.ThuocKhoiLop = model.ThuocKhoiLop;
                NewQuesTwoOperator.PhamViPhepToan = model.PhamViPhepToan;
                TableQuesTwoOperator.InsertOnSubmit(NewQuesTwoOperator);
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
        public string SaveEditQuesTwoOperator(PhepToanBaSoHangModel model)
        {
            try
            {
                var EditQuesTwoOperator = LinqContext.PhepToanBaSoHangs.Single(m => m.MaCauHoi == model.MaCauHoi);
                EditQuesTwoOperator.MaCauHoi = model.MaCauHoi;
                EditQuesTwoOperator.SoHangThuNhat = model.SoHangThuNhat;
                EditQuesTwoOperator.PhepToanThuNhat = model.PhepToanThuNhat;
                EditQuesTwoOperator.SoHangThuHai = model.SoHangThuHai;
                EditQuesTwoOperator.PhepToanThuHai = model.PhepToanThuHai;
                EditQuesTwoOperator.SoHangThuBa = model.SoHangThuBa;
                EditQuesTwoOperator.QuanHePhepToan = model.QuanHePhepToan;
                EditQuesTwoOperator.KetQuaPhepToan = model.KetQuaPhepToan;
                EditQuesTwoOperator.DapAnThuNhat = model.DapAnThuNhat;
                EditQuesTwoOperator.DapAnThuHai = model.DapAnThuHai;
                EditQuesTwoOperator.ThuocKhoiLop = model.ThuocKhoiLop;
                EditQuesTwoOperator.PhamViPhepToan = model.PhamViPhepToan;
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được câu hỏi này!";
            }
        }

        /// <summary>
        /// Xóa một câu hỏi hai phép toán
        /// </summary>
        /// <param name="id"></param>
        public string DelQuesTwoOperator(string MaCauHoi)
        {
            Guid MaCauHoiXoa = new Guid(MaCauHoi);
            try
            {
                var QuesTwoOperator = LinqContext.PhepToanBaSoHangs.Where(m => m.MaCauHoi == MaCauHoiXoa);
                LinqContext.PhepToanBaSoHangs.DeleteAllOnSubmit(QuesTwoOperator);
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
        public string DelAllQuesTwoOperator(string ThuocKhoiLop, string PhamViPhepToan)
        {
            try
            {
                var QuesTwoOperator = LinqContext.PhepToanBaSoHangs.Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m=>m.ThuocKhoiLop==ThuocKhoiLop);
                LinqContext.PhepToanBaSoHangs.DeleteAllOnSubmit(QuesTwoOperator);
                LinqContext.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các câu hỏi này!";
            }
        }
        #endregion

    }
}