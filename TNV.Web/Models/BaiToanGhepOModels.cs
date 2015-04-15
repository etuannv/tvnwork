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

    public class DanhSachBieuThucModel
    {
        [DisplayName("Biểu thức")]
        public string BieuThuc { get; set; }

        [DisplayName("Kết quả biểu thức")]
        public int KetQuaBieuThuc { get; set; }

        [DisplayName("Thứ tự sắp xếp")]
        public int ThuTuSapXep { get; set; }
    }

    //Dạng đáp án 2;3: 2 là vị trí thứ nhất, 3 là vị trí thứ 2
    public class DanhSachDapAnModel
    {
        [DisplayName("Vị trí ghép thứ nhất")]
        public string ViTriGhepThuNhat { get; set; }

        [DisplayName("Vị trí ghép thứ hai")]
        public string ViTriGhepThuHai { get; set; }

        [DisplayName("Thứ tự sắp xếp")]
        public int ThuTuSapXep { get; set; }
    }

    public class BaiToanGhepOModel
    {
        [DisplayName("Mã câu hỏi:")]
        public Guid MaBaiToan { get; set; }

        [DisplayName("Nội dung câu hỏi:")]
        public string NoiDungBaiToan { get; set; }

        [DisplayName("Nội dung đáp án:")]
        public string NoiDungDapAn { get; set; }

        [DisplayName("Nội dung giá trị:")]
        public string NoiDungGiaTri { get; set; }

        [DisplayName("Số biểu thức theo chiều ngang:")]
        public int ChieuNgang { get; set; }

        [DisplayName("Số biểu thức theo chiều dọc:")]
        public int ChieuDoc { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }

        [DisplayName("Loại bài toán:")]
        public string LoaiBaiToan { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int ThuTuSapXep { get; set; }
    }
    #endregion

    public interface BaiToanGhepOService
    {
        #region Quản lý bài toán ghép O
        string XoaNhieuBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan);
        string XoaBaiToanGhepO(string MaBaiToan);
        string SuaCauHoi(BaiToanGhepOModel BaiToan);
        string ThemMoiMotBaiToanGhepO(BaiToanGhepOModel BaiToan);
        BaiToanGhepOModel DocMotBaiToanGhepO(string MaBaiToan);
        BaiToanGhepOModel BaiToanGhepODauTien(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc,  string LoaiBaiToan);
        BaiToanGhepOModel GetOneBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan);
        List<BaiToanGhepOModel> DanhSachBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan);
        string DocDanhSachDapAn(List<DanhSachDapAnModel> DanhSachDapAn);
        List<DanhSachDapAnModel> DocDanhSachDapAn(string DanhSachDapAn);
        string DocDanhSachBieuThuc(List<DanhSachBieuThucModel> DanhSachBieuThuc, bool BieuThuc);
        List<DanhSachBieuThucModel> DocDanhSachBieuThuc(string DanhSachBieuThuc, string DanhSachGiaTri);
        List<DanhSachBieuThucModel> PhanTichMotSo(int SoPhanTich, int PhamViPhepToan);
        DanhSachBieuThucModel LayMotBieuThuc(List<DanhSachBieuThucModel> DanhSachBieuThuc, int STTBieuThuc);
        DanhSachBieuThucModel LayMotBieuThuc(int SoCanDoc, bool DocSo);
        #endregion
        

        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }
    }

    public class BaiToanGhepOClass : BaiToanGhepOService
    {
        public int SoBanGhiTrenMotTrang
        {
            get
            {
                return 15;
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

        /// <summary>
        /// Phân tích một số thành tổng hoặc hiệu ba số hạng
        /// </summary>
        /// <param name="SoPhanTich">Số càn phân tích</param>
        /// <param name="PhamViPhepToan">Phạm vi phép toán chỉ áp dụng trong trường hợp phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucModel> PhanTichMotSo(int SoPhanTich, int PhamViPhepToan)
        {
            List<DanhSachBieuThucModel> DSBieuThuc = new List<DanhSachBieuThucModel>();
            Random rd = new Random();

            //Giữ nguyên số không phân tích
            DanhSachBieuThucModel BieuThuc1 = new DanhSachBieuThucModel();
            BieuThuc1.BieuThuc = SoPhanTich.ToString().Trim();
            BieuThuc1.ThuTuSapXep = rd.Next(1256, 258963);
            BieuThuc1.KetQuaBieuThuc = SoPhanTich;
            DSBieuThuc.Add(BieuThuc1);

            //Dạng A=B+C
            for (int i = 1; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
            {
                DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                BieuThuc.BieuThuc = i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim();
                BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                BieuThuc.KetQuaBieuThuc = SoPhanTich;
                DSBieuThuc.Add(BieuThuc);
            }

            //Dạng A=B+C+D
            for (int i = 1; i < SoPhanTich; i++)
            {
                for (int j = 1; j < SoPhanTich - i; j++)
                {
                    if (i + j <= PhamViPhepToan && SoPhanTich - i - j>0)
                    {
                        DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + (SoPhanTich - i - j).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                        BieuThuc.KetQuaBieuThuc = SoPhanTich;
                        DSBieuThuc.Add(BieuThuc);
                    }
                }
            }

            //Dạng A=B+C-D
            for (int i = 1; i < SoPhanTich; i++)
            {
                for (int j = PhamViPhepToan; j > SoPhanTich - i; j--)
                {
                    if (i + j <= PhamViPhepToan && i + j - SoPhanTich>0)
                    {
                        DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + (i + j - SoPhanTich).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                        BieuThuc.KetQuaBieuThuc = SoPhanTich;
                        DSBieuThuc.Add(BieuThuc);
                    }
                }
            }

            //Dạng A=B-C
            for (int i = PhamViPhepToan; i > SoPhanTich; i--)
            {
                DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                BieuThuc.BieuThuc = i.ToString().Trim() + " - " + (i - SoPhanTich).ToString().Trim();
                BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                BieuThuc.KetQuaBieuThuc = SoPhanTich;
                DSBieuThuc.Add(BieuThuc);

            }

            //Dạng A=B-C+D
            for (int i = PhamViPhepToan; i > SoPhanTich; i--)
            {
                for (int j = 1; j < i; j++)
                {
                    if (SoPhanTich - i + j > 0)
                    {
                        DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + " - " + j.ToString().Trim() + " + " + (SoPhanTich - i + j).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                        BieuThuc.KetQuaBieuThuc = SoPhanTich;
                        DSBieuThuc.Add(BieuThuc);
                    }
                }
            }

            //Dạng A=B-C-D
            for (int i = PhamViPhepToan; i > SoPhanTich; i--)
            {
                for (int j = 1; j < i; j++)
                {
                    if (i - j - SoPhanTich > 0)
                    {
                        DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + (i - j - SoPhanTich).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(1256, 258963);
                        BieuThuc.KetQuaBieuThuc = SoPhanTich;
                        DSBieuThuc.Add(BieuThuc);
                    }
                }
            }
            List<DanhSachBieuThucModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucModel>();

            List<DanhSachBieuThucModel> DSBieuThucMoi = new List<DanhSachBieuThucModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Lấy một biểu thức bởi STT của nó
        /// </summary>
        /// <param name="DanhSachBieuThuc">Danh sách các biểu thức</param>
        /// <param name="STTBieuThuc">Số thứ tự biểu thức cần lấy</param>
        /// <returns></returns>
        public DanhSachBieuThucModel LayMotBieuThuc(List<DanhSachBieuThucModel> DanhSachBieuThuc, int STTBieuThuc)
        {
            DanhSachBieuThucModel Return = (from GetItem in DanhSachBieuThuc.Where(m => m.ThuTuSapXep == STTBieuThuc)
                                            select new DanhSachBieuThucModel
                                            {
                                                BieuThuc = GetItem.BieuThuc,
                                                ThuTuSapXep = GetItem.ThuTuSapXep,
                                                KetQuaBieuThuc = GetItem.KetQuaBieuThuc
                                            }).Single<DanhSachBieuThucModel>();

            return Return;
        }
        /// <summary>
        /// Lấy một biểu thức đọc số
        /// </summary>
        /// <param name="SoCanDoc">Số cần đọc</param>
        /// <returns></returns>
        public DanhSachBieuThucModel LayMotBieuThuc(int SoCanDoc, bool DocSo)
        {

            ToolShareService AllToolShare = new ToolShareService();
            DanhSachBieuThucModel Return = new DanhSachBieuThucModel();
            if (DocSo)
            {
                Return.BieuThuc = AllToolShare.ReadNumber(SoCanDoc.ToString().Trim());
            }
            else
            {
                Return.BieuThuc = SoCanDoc.ToString().Trim();
            }
            Return.KetQuaBieuThuc = SoCanDoc;
            Return.ThuTuSapXep = AllToolShare.LayMaNgauNhien(25633, 598666, "");
            return Return;
        }
        /// <summary>
        /// Đọc danh sách các biểu thức từ một xâu ký tự 1+2$2+1$
        /// </summary>
        /// <param name="DanhSachBieuThuc">Biểu thức cần đọc</param>
        /// <returns></returns>
        public List<DanhSachBieuThucModel> DocDanhSachBieuThuc(string DanhSachBieuThuc, string DanhSachGiaTri)
        {
            List<DanhSachBieuThucModel> CacBieuThuc = new List<DanhSachBieuThucModel>();

            string[] DSBieuThuc = DanhSachBieuThuc.Split('$');
            string[] DSGiaTri = DanhSachGiaTri.Split('$');
            for (int i = 0; i < DSBieuThuc.Length; i++ )
            {
                DanhSachBieuThucModel BieuThuc = new DanhSachBieuThucModel();
                BieuThuc.BieuThuc = DSBieuThuc[i].ToString();
                BieuThuc.KetQuaBieuThuc = Convert.ToInt32(DSGiaTri[i]);
                BieuThuc.ThuTuSapXep = i + 1;
                CacBieuThuc.Add(BieuThuc);
            }
            return CacBieuThuc;
        }

        /// <summary>
        /// Chuyển danh sách biểu thức về dạng chuỗi ký tự
        /// </summary>
        /// <param name="DanhSachBieuThuc">Danh sách biểu thức dạng List</param>
        /// <param name="BieuThuc">true: Đọc danh sách biểu thức; false: Đọc danh sách kết quả biểu thức</param>
        /// <returns></returns>
        public string DocDanhSachBieuThuc(List<DanhSachBieuThucModel> DanhSachBieuThuc, bool BieuThuc)
        {
            string KetQua = "";
            if (BieuThuc)
            {
                foreach (DanhSachBieuThucModel Item in DanhSachBieuThuc)
                {
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        KetQua += Item.BieuThuc;
                    }
                    else
                    {
                        KetQua += "$" + Item.BieuThuc;
                    }
                }
            }
            else
            {
                foreach (DanhSachBieuThucModel Item in DanhSachBieuThuc)
                {
                    if (String.IsNullOrEmpty(KetQua))
                    {
                        KetQua += Item.KetQuaBieuThuc;
                    }
                    else
                    {
                        KetQua += "$" + Item.KetQuaBieuThuc;
                    }
                }
            }
            return KetQua;
        }

        /// <summary>
        /// Lấy danh sách các đáp án từ chuỗi các đáp án
        /// </summary>
        /// <param name="DanhSachDapAn"></param>
        /// <returns></returns>
        public List<DanhSachDapAnModel> DocDanhSachDapAn(string DanhSachDapAn)
        {
            List<DanhSachDapAnModel> CacDapAn = new List<DanhSachDapAnModel>();

            string[] DSBieuThuc = DanhSachDapAn.Split('$'); 
            int Dem = 0;
            foreach (string Item in DSBieuThuc)
            {
                Dem++;
                string[] CatViTri = Item.Split(';');
                DanhSachDapAnModel DapAn = new DanhSachDapAnModel();
                DapAn.ViTriGhepThuNhat = CatViTri[0];
                DapAn.ViTriGhepThuHai = CatViTri[1];
                DapAn.ThuTuSapXep = Dem;
                CacDapAn.Add(DapAn);
            }
            return CacDapAn;
        }

        /// <summary>
        /// Chuyển danh sách đáp án về dạng chuỗi ký tự 1;2$3;4$
        /// </summary>
        /// <param name="DanhSachDapAn">Danh sách đáp án dạng List</param>
        /// <returns></returns>
        public string DocDanhSachDapAn(List<DanhSachDapAnModel> DanhSachDapAn)
        {
            string KetQua = "";

            foreach (DanhSachDapAnModel Item in DanhSachDapAn)
            {
                if (String.IsNullOrEmpty(KetQua))
                {
                    KetQua += Item.ViTriGhepThuNhat + ";" + Item.ViTriGhepThuHai;
                }
                else
                {
                    KetQua += "$" + Item.ViTriGhepThuNhat + ";" + Item.ViTriGhepThuHai;
                }
            }
            return KetQua;
        }
        /// <summary>
        /// Đọc danh sách các bài toán về ghép o
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public List<BaiToanGhepOModel> DanhSachBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan)
        {
            List<BaiToanGhepOModel> TatCaDanhSach = (from BaiToan in ListData.BaiToanGhepOs
                                                     where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.ChieuNgang == ChieuNgang && BaiToan.ChieuDoc == ChieuDoc && BaiToan.LoaiBaiToan == LoaiBaiToan
                                                     orderby BaiToan.ThuTuSapXep descending
                                                     select new BaiToanGhepOModel
                                                   {
                                                       MaBaiToan = BaiToan.MaBaiToan,
                                                       NoiDungBaiToan = BaiToan.NoiDungBaiToan,
                                                       NoiDungDapAn = BaiToan.NoiDungDapAn,
                                                       ChieuDoc = BaiToan.ChieuDoc,
                                                       ChieuNgang = BaiToan.ChieuNgang,
                                                       PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                       ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                       ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                       NoiDungGiaTri = BaiToan.NoiDungGiaTri,
                                                       LoaiBaiToan = BaiToan.LoaiBaiToan,
                                                   }).ToList<BaiToanGhepOModel>();
            return TatCaDanhSach;
        }


        /// <summary>
        /// Đọc bài toán ghép o bat ky
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanGhepOModel GetOneBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan)
        {
            IEnumerable<BaiToanGhepOModel> ResultList = (from BaiToan in ListData.BaiToanGhepOs
                                                         where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.ChieuNgang == ChieuNgang && BaiToan.ChieuDoc == ChieuDoc && BaiToan.LoaiBaiToan == LoaiBaiToan
                                                         orderby BaiToan.ThuTuSapXep descending
                                                         select new BaiToanGhepOModel
                                                         {
                                                             MaBaiToan = BaiToan.MaBaiToan,
                                                             NoiDungBaiToan = BaiToan.NoiDungBaiToan,
                                                             NoiDungDapAn = BaiToan.NoiDungDapAn,
                                                             ChieuDoc = BaiToan.ChieuDoc,
                                                             ChieuNgang = BaiToan.ChieuNgang,
                                                             PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                             ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                             ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                             NoiDungGiaTri = BaiToan.NoiDungGiaTri,
                                                             LoaiBaiToan = BaiToan.LoaiBaiToan,
                                                         });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }

        /// <summary>
        /// Đọc bài toán ghép o đầu tiên
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanGhepOModel BaiToanGhepODauTien(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan)
        {
            BaiToanGhepOModel MotBaiToanGhepODauTien = (from BaiToan in ListData.BaiToanGhepOs
                                                        where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.ChieuNgang == ChieuNgang && BaiToan.ChieuDoc == ChieuDoc && BaiToan.LoaiBaiToan == LoaiBaiToan
                                                        orderby BaiToan.ThuTuSapXep descending
                                                        select new BaiToanGhepOModel
                                                        {
                                                            MaBaiToan = BaiToan.MaBaiToan,
                                                            NoiDungBaiToan = BaiToan.NoiDungBaiToan,
                                                            NoiDungDapAn = BaiToan.NoiDungDapAn,
                                                            ChieuDoc = BaiToan.ChieuDoc,
                                                            ChieuNgang = BaiToan.ChieuNgang,
                                                            PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                            ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                            ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                            NoiDungGiaTri = BaiToan.NoiDungGiaTri,
                                                            LoaiBaiToan = BaiToan.LoaiBaiToan,
                                                        }).First<BaiToanGhepOModel>();
            return MotBaiToanGhepODauTien;
        }

        /// <summary>
        /// Đọc một bài toán ghép o
        /// </summary>
        /// <param name="MaCauHoi"></param>
        /// <returns></returns>
        public BaiToanGhepOModel DocMotBaiToanGhepO(string MaBaiToan)
        {
            Guid MaCauHoiDoc = new Guid(MaBaiToan);
            BaiToanGhepOModel MotCauHoiDoc = (from BaiToan in ListData.BaiToanGhepOs
                                              where BaiToan.MaBaiToan == MaCauHoiDoc
                                                 select new BaiToanGhepOModel
                                                   {
                                                       MaBaiToan = BaiToan.MaBaiToan,
                                                       NoiDungBaiToan = BaiToan.NoiDungBaiToan,
                                                       NoiDungDapAn = BaiToan.NoiDungDapAn,
                                                       ChieuDoc = BaiToan.ChieuDoc,
                                                       ChieuNgang = BaiToan.ChieuNgang,
                                                       PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                       ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                       ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                       NoiDungGiaTri = BaiToan.NoiDungGiaTri,
                                                       LoaiBaiToan=BaiToan.LoaiBaiToan,
                                                   }).SingleOrDefault<BaiToanGhepOModel>();
            return MotCauHoiDoc;
        }

        /// <summary>
        /// Thêm mới một bài toán ghép o
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotBaiToanGhepO(BaiToanGhepOModel BaiToan)
        {
            try
            {
                Table<BaiToanGhepO> BangBaiToan = ListData.GetTable<BaiToanGhepO>();
                BaiToanGhepO BaiToanGhepOItem = new BaiToanGhepO();
                BaiToanGhepOItem.MaBaiToan = BaiToan.MaBaiToan;
                BaiToanGhepOItem.NoiDungBaiToan = BaiToan.NoiDungBaiToan;
                BaiToanGhepOItem.NoiDungDapAn = BaiToan.NoiDungDapAn;
                BaiToanGhepOItem.ChieuDoc = BaiToan.ChieuDoc;
                BaiToanGhepOItem.ChieuNgang = BaiToan.ChieuNgang;
                BaiToanGhepOItem.PhamViPhepToan = BaiToan.PhamViPhepToan;
                BaiToanGhepOItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BaiToanGhepOItem.NoiDungGiaTri = BaiToan.NoiDungGiaTri;
                BaiToanGhepOItem.LoaiBaiToan = BaiToan.LoaiBaiToan;
                BangBaiToan.InsertOnSubmit(BaiToanGhepOItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được bài toán này!";
            }
        }

        /// <summary>
        /// Sửa một bài toán ghép O
        /// </summary>
        /// <param name="model"></param>
        public string SuaCauHoi(BaiToanGhepOModel BaiToan) 
        {
            try
            {
                var BaiToanGhepOItem = ListData.BaiToanGhepOs.Single(m => m.MaBaiToan == BaiToan.MaBaiToan);
                BaiToanGhepOItem.NoiDungBaiToan = BaiToan.NoiDungBaiToan;
                BaiToanGhepOItem.NoiDungDapAn = BaiToan.NoiDungDapAn;
                BaiToanGhepOItem.ChieuDoc = BaiToan.ChieuDoc;
                BaiToanGhepOItem.ChieuNgang = BaiToan.ChieuNgang;
                BaiToanGhepOItem.PhamViPhepToan = BaiToan.PhamViPhepToan;
                BaiToanGhepOItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BaiToanGhepOItem.NoiDungGiaTri = BaiToan.NoiDungGiaTri;
                BaiToanGhepOItem.LoaiBaiToan = BaiToan.LoaiBaiToan;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được bài toán này!";
            }
        }

        /// <summary>
        /// Xóa một bài toán ghép O
        /// </summary>
        /// <param name="id"></param>
        public string XoaBaiToanGhepO(string MaBaiToan)  
        {
            Guid MaBaiToanXoa = new Guid(MaBaiToan);
            try
            {
                var BaiToanCanXoa = ListData.BaiToanGhepOs.Where(m => m.MaBaiToan == MaBaiToanXoa);
                ListData.BaiToanGhepOs.DeleteAllOnSubmit(BaiToanCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được bài toán này!";
            }
        }

        /// <summary>
        /// Xóa nhiều bài toán ghép o
        /// </summary>
        /// <param name="id"></param>
        public string XoaNhieuBaiToanGhepO(string ThuocKhoiLop, string PhamViPhepToan, int ChieuNgang, int ChieuDoc, string LoaiBaiToan)
        {
            try
            {
                var CacBaiToanGhepOCanXoa = ListData.BaiToanGhepOs.Where(m => m.ThuocKhoiLop == ThuocKhoiLop).Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m => m.ChieuNgang == ChieuNgang).Where(m => m.ChieuDoc == ChieuDoc).Where(m => m.LoaiBaiToan == LoaiBaiToan);
                ListData.BaiToanGhepOs.DeleteAllOnSubmit(CacBaiToanGhepOCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các bài toán này!";
            }
        }

    }
}