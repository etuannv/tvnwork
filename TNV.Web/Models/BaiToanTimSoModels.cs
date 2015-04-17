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

    public class DanhSachBieuThucTimSoModel
    {
        [DisplayName("Biểu thức")]
        public string BieuThuc { get; set; }

        [DisplayName("Lời giải")]
        public string LoiGiaiBaiToan { get; set; }

        [DisplayName("Số phân tích")]
        public int SoPhanTich { get; set; }

        [DisplayName("Thứ tự sắp xếp")]
        public int ThuTuSapXep { get; set; }

    }

    public class BaiToanTimSoModel
    {
        [DisplayName("Mã câu hỏi:")]
        public Guid MaCauHoi { get; set; }

        [DisplayName("Chuỗi số hiển thị")]
        public string ChuoiSoHienThi { get; set; }

        [DisplayName("Thứ tự sắp xếp:")]
        public int ThuTuSapXep { get; set; }

        [DisplayName("Thuộc khối lớp:")]
        public string ThuocKhoiLop { get; set; }

        [DisplayName("Phạm vi phép toán:")]
        public string PhamViPhepToan { get; set; }

        [DisplayName("Phân loại bài toán:")]
        public string PhanLoaiBaiToan { get; set; }

        [DisplayName("Đáp án")]
        public string DapAn { get; set; }

        [DisplayName("Tên UserControl:")]
        public string UserControlName { get; set; }

        [DisplayName("Lời giải bài toán:")]
        public string LoiGiaiBaiToan { get; set; }
    }

    public class CapSoCongModel
    {
        public string CapSoCong { get; set; }
        public int ThuTuSapXep { get; set; }
        public string LoiGiai { get; set; }
    }

    #endregion

    public interface BaiToanTimSoService
    {
        List<BaiToanTimSoModel> DanhSachBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan);
        BaiToanTimSoModel BaiToanTimSoDauTien(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan);
        BaiToanTimSoModel GetOneBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan);
        BaiToanTimSoModel DocMotBaiToanTimSo(string MaCauHoi);
        string ThemMoiMotBaiToanTimSo(BaiToanTimSoModel BaiToan);
        string SuaCauHoi(BaiToanTimSoModel BaiToan);
        string XoaBaiToanTimSo(string MaCauHoi);
        string XoaNhieuBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan);
        List<CapSoCongModel> DanhSachCSC(int PhanTuDauTienBeNhat, int PhanTuDauTienLonNhat, int CongSai, int PhamViPhepToan, int SoLuongPhanTu);
        List<CapSoCongModel> SinhHoanVi(CapSoCongModel CapSoCong);
        CapSoCongModel LayMotCapSoCong(List<CapSoCongModel> DSCapSoCong, int SoThuTu);
        string LayDapAn(CapSoCongModel CapSoCongGoc, CapSoCongModel CapSoCongHoanVi);
        List<DanhSachBieuThucTimSoModel> PhanTichMotSoCongThem(int SoPhanTich, int PhamViPhepToan, int LoaiPhanTich, int SoCongThem, int SoBatDau = 1);
        List<DanhSachBieuThucTimSoModel> PhanTichMotSoTruBot(int SoPhanTich, int PhamViPhepToan, int LoaiPhanTich, int SoTruBot, int SoBatDau = 1);
        List<DanhSachBieuThucTimSoModel> SinhHoanVi(DanhSachBieuThucTimSoModel BieuThuc);
        string LayDapAn(DanhSachBieuThucTimSoModel BieuThucGoc, DanhSachBieuThucTimSoModel BieuThucHoanVi);
        DanhSachBieuThucTimSoModel LayMotHoanVi(List<DanhSachBieuThucTimSoModel> DSHoanVi, int SoThuTu);
        List<DanhSachBieuThucTimSoModel> PhanTichMotSoThanhTongHieuHaiSo(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1);
        DanhSachBieuThucTimSoModel PhanTichHaiSo(int SoThuNhat, int SoThuHai, int SoKhongDoi, int LoaiPhanTich = 1);
        List<DanhSachBieuThucTimSoModel> PhanTichTongHieu(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1, int PhamViPhepToan = 9);
        List<DanhSachBieuThucTimSoModel> PhanTichTongHieuBaCap(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1, int PhamViPhepToan = 9);
        int SoBanGhiTrenMotTrang { get; }
        int BuocNhay { get; }
    }

    public class BaiToanTimSoClass : BaiToanTimSoService
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

        #region Quản lý bài toán tìm số

        /// <summary>
        /// Phân tích một số thành tổng hoặc hiệu các số hạng cộng với một số không đổi
        /// </summary>
        /// <param name="SoPhanTich">Số càn phân tích</param>
        /// <param name="PhamViPhepToan">Phạm vi phép toán chỉ áp dụng trong trường hợp phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> PhanTichMotSoCongThem(int SoPhanTich, int PhamViPhepToan, int LoaiPhanTich, int SoCongThem, int SoBatDau=1)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();

            if (SoPhanTich + SoCongThem <= PhamViPhepToan)
            {
                if (LoaiPhanTich == 1)
                {
                    //Dạng A=B+C
                    for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                    {
                        DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                            + (SoPhanTich - i).ToString().Trim() + ";"
                                            + (SoPhanTich + SoCongThem).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (SoCongThem == 0)
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + "</b>";
                        }
                        else
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                        }
                        BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                        DSBieuThuc.Add(BieuThuc);
                    }
                }

                if (LoaiPhanTich == 2)
                {
                    //Dạng A=B-C
                    for (int i = PhamViPhepToan; i > SoPhanTich + SoBatDau; i--)
                    {
                        DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                            + (i - SoPhanTich).ToString().Trim() + ";"
                                            + (SoPhanTich + SoCongThem).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (SoCongThem == 0)
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + (i - SoPhanTich).ToString().Trim() + "</b>";
                        }
                        else
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " - " + (i - SoPhanTich).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                        }
                        BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                        DSBieuThuc.Add(BieuThuc);

                    }
                }

                if (LoaiPhanTich == 3)
                {
                    //Dạng A=B+C+D
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            if (i + j <= PhamViPhepToan && SoPhanTich - i - j >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (SoPhanTich - i - j).ToString().Trim() + ";"
                                                    + (SoPhanTich + SoCongThem).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoCongThem == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + (SoPhanTich - i - j).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + (SoPhanTich - i - j).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 4)
                {
                    //Dạng A=B+C-D
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = PhamViPhepToan; j > SoPhanTich - i + SoBatDau; j--)
                        {
                            if (i + j <= PhamViPhepToan && i + j - SoPhanTich >= SoBatDau && j >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (i + j - SoPhanTich).ToString().Trim() + ";"
                                                    + (SoPhanTich + SoCongThem).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoCongThem == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + (i + j - SoPhanTich).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + (i + j - SoPhanTich).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 5)
                {
                    //Dạng A=B-C+D
                    for (int i = PhamViPhepToan; i > SoPhanTich; i--)
                    {
                        for (int j = SoBatDau; j < i; j++)
                        {
                            if (SoPhanTich - i + j >= SoBatDau && i >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (SoPhanTich - i + j).ToString().Trim() + ";"
                                                    + (SoPhanTich + SoCongThem).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoCongThem == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " + " + (SoPhanTich - i + j).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " + " + (SoPhanTich - i + j).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 6)
                {
                    //Dạng A=B-C-D
                    for (int i = PhamViPhepToan; i > SoPhanTich; i--)
                    {
                        for (int j = SoBatDau; j < i; j++)
                        {
                            if (i - j - SoPhanTich >= SoBatDau && i >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (i - j - SoPhanTich).ToString().Trim() + ";"
                                                    + (SoPhanTich + SoCongThem).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoCongThem == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + (i - j - SoPhanTich).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + (i - j - SoPhanTich).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 7)
                {
                    //Dạng A=B+C+D+E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            for (int k = SoBatDau; k < SoPhanTich - i - j; k++)
                            {
                                if (i + j + k <= PhamViPhepToan && SoPhanTich - i - j - k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (SoPhanTich - i - j - k).ToString().Trim() + ";"
                                                        + (SoPhanTich + SoCongThem).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoCongThem == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " + " + (SoPhanTich - i - j - k).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " + " + (SoPhanTich - i - j - k).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 8)
                {
                    //Dạng A=B+C+D-E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            for (int k = PhamViPhepToan; k > SoPhanTich - i - j; k--)
                            {
                                if (i + j + k <= PhamViPhepToan && i + j + k - SoPhanTich >= SoBatDau && k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (i + j + k - SoPhanTich).ToString().Trim()
                                                        + ";" + (SoPhanTich + SoCongThem).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoCongThem == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " - " + (i + j + k - SoPhanTich).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " - " + (i + j + k - SoPhanTich).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 9)
                {
                    //Dạng A=B+C-D-E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = PhamViPhepToan; j > SoPhanTich - i; j--)
                        {
                            for (int k = PhamViPhepToan; k > SoPhanTich - i - j; k--)
                            {
                                if (i + j - k <= PhamViPhepToan && i + j - k - SoPhanTich >= SoBatDau && j >= SoBatDau && k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";" 
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (i + j - k - SoPhanTich).ToString().Trim()
                                                        + ";" + (SoPhanTich + SoCongThem).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoCongThem == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + k.ToString().Trim() + " - " + (i + j - k - SoPhanTich).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + k.ToString().Trim() + " - " + (i + j - k - SoPhanTich).ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 10)
                {
                    //Dạng A=B-C-D-E
                    for (int i = SoBatDau; i <= SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j <= SoPhanTich; j++)
                        {
                            for (int k = SoBatDau; k <= SoPhanTich; k++)
                            {
                                if (SoPhanTich + i + j + k <= PhamViPhepToan)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = (SoPhanTich + i + j + k).ToString().Trim() + ";"
                                                        + i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";" 
                                                        + (SoPhanTich + SoCongThem).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoCongThem == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + (SoPhanTich + i + j + k).ToString().Trim() + " - " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + k.ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich + SoCongThem).ToString().Trim() + " = " + (SoPhanTich + i + j + k).ToString().Trim() + " - " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + k.ToString().Trim() + " + " + SoCongThem.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich + SoCongThem;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }
            }
            List<DanhSachBieuThucTimSoModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();

            List<DanhSachBieuThucTimSoModel> DSBieuThucMoi = new List<DanhSachBieuThucTimSoModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucTimSoModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucTimSoModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Phân tích một số thành tổng hiệu hai số dạng 5 số
        /// </summary>
        /// <param name="SoBatDau">Số bắt đầu</param>
        /// <param name="SoPhanTich">Số cần phân tích</param>
        /// <param name="LoaiPhanTich">Phép cộng hay phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> PhanTichTongHieu(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1, int PhamViPhepToan=9)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();
            if (LoaiPhanTich == 1)
            {
                for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                {
                    //Lấy ngẫu nhiên một số khác i nhỏ hơn SoPhanTich
                    int SoThuNhat = AllToolShare.LayMaNgauNhien(SoBatDau, (SoPhanTich - (SoPhanTich % 2)) / 2, i.ToString().Trim());
                    int SoThuHai = SoPhanTich - SoThuNhat;

                    //Tổng hai số ở hai vị trí đối diện bằng nhau
                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                        + SoThuNhat.ToString() + ";"
                                        + (SoPhanTich - i).ToString().Trim() + ";"
                                        + SoThuHai.ToString().Trim() + ";"
                                        + SoPhanTich.ToString().Trim();
                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                    BieuThuc.LoiGiaiBaiToan = SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + " = " + SoThuNhat.ToString().Trim() + " + " + SoThuHai.ToString().Trim();
                    BieuThuc.SoPhanTich = SoPhanTich;
                    DSBieuThuc.Add(BieuThuc);
                }
            }

            if (LoaiPhanTich == 2)
            {
                for (int i = SoBatDau; i < PhamViPhepToan; i++)
                {
                    if (SoPhanTich + i <= PhamViPhepToan)
                    {
                        int SoThanhPhanThuNhat = i;
                        int SoThanhPhanThuHai = SoPhanTich + SoThanhPhanThuNhat;

                        int SoThanhPhanThuBa = AllToolShare.LayMaNgauNhien(SoBatDau, PhamViPhepToan, i.ToString().Trim());
                        int SoThanhPhanThuTu = SoPhanTich + SoThanhPhanThuBa;

                        //Tổng hai số ở hai vị trí đối diện bằng nhau
                        if (SoPhanTich + SoThanhPhanThuBa <= PhamViPhepToan)
                        {
                            DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                            BieuThuc.BieuThuc = SoThanhPhanThuNhat.ToString().Trim() + ";"
                                                + SoThanhPhanThuBa.ToString().Trim() + ";"
                                                + SoThanhPhanThuHai.ToString() + ";"
                                                + SoThanhPhanThuTu.ToString().Trim() + ";"
                                                + SoPhanTich.ToString().Trim();
                            BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            BieuThuc.LoiGiaiBaiToan = SoPhanTich.ToString().Trim() + " = " + SoThanhPhanThuHai.ToString().Trim() + " - " + SoThanhPhanThuNhat.ToString().Trim() + " = " + SoThanhPhanThuTu.ToString().Trim() + " - " + SoThanhPhanThuBa.ToString().Trim();
                            BieuThuc.SoPhanTich = SoPhanTich;
                            DSBieuThuc.Add(BieuThuc);
                        }
                    }
                }
            }

            List<DanhSachBieuThucTimSoModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();

            List<DanhSachBieuThucTimSoModel> DSBieuThucMoi = new List<DanhSachBieuThucTimSoModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucTimSoModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucTimSoModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Phân tích một số thành tổng hiệu hai số dạng 5 số
        /// </summary>
        /// <param name="SoBatDau">Số bắt đầu</param>
        /// <param name="SoPhanTich">Số cần phân tích</param>
        /// <param name="LoaiPhanTich">Phép cộng hay phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> PhanTichTongHieuBaCap(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1, int PhamViPhepToan = 9)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();
            if (LoaiPhanTich == 1)
            {
                for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                {
                    //Bộ phân tích thứ nhất
                    int SoThuNhat = i;
                    int SoThuNhatConLai = SoPhanTich - SoThuNhat;

                    //Bộ phân tích thứ 2
                    int SoThuHai = AllToolShare.LayMaNgauNhien(SoBatDau, (SoPhanTich - (SoPhanTich % 2)) / 2, i.ToString().Trim() + "$" + SoThuNhatConLai.ToString().Trim());
                    int SoThuHaiConLai = SoPhanTich - SoThuHai;

                    //Bộ phân tích thứ 3
                    int SoThuBa = AllToolShare.LayMaNgauNhien(SoBatDau, (SoPhanTich - (SoPhanTich % 2)) / 2, i.ToString().Trim() + "$" + SoThuNhatConLai.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuHaiConLai.ToString().Trim());
                    int SoThuBaConLai = SoPhanTich - SoThuBa;

                    //Tổng hai số ở hai vị trí đối diện bằng nhau
                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                    BieuThuc.BieuThuc = SoThuNhat.ToString().Trim() + ";"
                                        + SoThuHai.ToString() + ";"
                                        + SoThuBa.ToString().Trim() + ";"
                                        + SoThuNhatConLai.ToString().Trim() + ";"
                                        + SoThuHaiConLai.ToString().Trim() + ";"
                                        + SoThuBaConLai.ToString().Trim() + ";"
                                        + SoPhanTich.ToString().Trim();
                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                    BieuThuc.LoiGiaiBaiToan = SoPhanTich.ToString().Trim() + " = " + SoThuNhat.ToString().Trim() + " + " + SoThuNhatConLai.ToString().Trim() + " = " + SoThuHai.ToString().Trim() + " + " + SoThuHaiConLai.ToString().Trim() + " = " + SoThuBa.ToString().Trim() + " + " + SoThuBaConLai.ToString().Trim();
                    BieuThuc.SoPhanTich = SoPhanTich;
                    DSBieuThuc.Add(BieuThuc);
                }
            }

            if (LoaiPhanTich == 2)
            {
                for (int i = SoBatDau; i < PhamViPhepToan; i++)
                {
                    if (SoPhanTich + i <= PhamViPhepToan)
                    {
                        //Bộ phân tích thứ nhất
                        int SoThuNhat = i;
                        int SoThuNhatConLai = SoPhanTich + SoThuNhat;

                        //Bộ phân tích thứ 2
                        int SoThuHai = AllToolShare.LayMaNgauNhien(SoBatDau, PhamViPhepToan, SoThuNhat.ToString().Trim() + "$" + SoThuNhatConLai.ToString().Trim());
                        int SoThuHaiConLai = SoPhanTich + SoThuHai;

                        //Bộ phân tích thứ 3
                        int SoThuBa = AllToolShare.LayMaNgauNhien(SoBatDau, PhamViPhepToan, SoThuNhat.ToString().Trim() + "$" + SoThuNhatConLai.ToString().Trim() + "$" + SoThuHai.ToString().Trim() + "$" + SoThuHaiConLai.ToString().Trim());
                        int SoThuBaConLai = SoPhanTich + SoThuBa;

                        //Tổng hai số ở hai vị trí đối diện bằng nhau
                        if (SoThuNhatConLai <= PhamViPhepToan && SoThuHaiConLai <= PhamViPhepToan && SoThuBaConLai <= PhamViPhepToan)
                        {
                            DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                            BieuThuc.BieuThuc = SoThuNhat.ToString().Trim() + ";"
                                                + SoThuHai.ToString() + ";"
                                                + SoThuBa.ToString().Trim() + ";"
                                                + SoThuNhatConLai.ToString().Trim() + ";"
                                                + SoThuHaiConLai.ToString().Trim() + ";"
                                                + SoThuBaConLai.ToString().Trim() + ";"
                                                + SoPhanTich.ToString().Trim();
                            BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            BieuThuc.LoiGiaiBaiToan = SoPhanTich.ToString().Trim() + " = " + SoThuNhatConLai.ToString().Trim() + " - " + SoThuNhat.ToString().Trim() + " = " + SoThuHaiConLai.ToString().Trim() + " - " + SoThuHai.ToString().Trim() + " = " + SoThuBaConLai.ToString().Trim() + " - " + SoThuBa.ToString().Trim();
                            BieuThuc.SoPhanTich = SoPhanTich;
                            DSBieuThuc.Add(BieuThuc);
                        }
                    }
                }
            }

            List<DanhSachBieuThucTimSoModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();

            List<DanhSachBieuThucTimSoModel> DSBieuThucMoi = new List<DanhSachBieuThucTimSoModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucTimSoModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucTimSoModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Phân tích một số thành tổng hai số
        /// </summary>
        /// <param name="SoPhanTich">Số càn phân tích</param>
        /// <param name="PhamViPhepToan">Phạm vi phép toán chỉ áp dụng trong trường hợp phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> PhanTichMotSoThanhTongHieuHaiSo(int SoPhanTich, int LoaiPhanTich, int SoBatDau = 1)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();
            if (LoaiPhanTich == 1)
            {
                for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                {
                    //Lấy ngẫu nhiên một số khác i nhỏ hơn SoPhanTich
                    int SoThuNhat = AllToolShare.LayMaNgauNhien(SoBatDau, (SoPhanTich - (SoPhanTich % 2)) / 2, i.ToString().Trim());
                    int SoThuHai = SoPhanTich - SoThuNhat;

                    //Tổng hai số ở hai vị trí đối diện bằng nhau
                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                        + SoThuNhat.ToString()+";"
                                        + (SoPhanTich - i).ToString().Trim() + ";"
                                        + SoThuHai.ToString().Trim() ;
                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                    BieuThuc.LoiGiaiBaiToan = i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + " = " + SoThuNhat.ToString().Trim() + " + " + SoThuHai.ToString().Trim() + " = " + SoPhanTich.ToString().Trim();
                    BieuThuc.SoPhanTich = SoPhanTich;
                    DSBieuThuc.Add(BieuThuc);
                }
            }

            if (LoaiPhanTich == 2)
            {
                for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                {
                    //Lấy ngẫu nhiên một số khác i nhỏ hơn SoPhanTich
                    int SoThuNhat = AllToolShare.LayMaNgauNhien(SoBatDau, (SoPhanTich - (SoPhanTich % 2)) / 2, i.ToString().Trim());
                    int SoThuHai = SoPhanTich - SoThuNhat;

                    //Tổng hai số ở hai vị trí đối diện bằng nhau
                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                        + (SoPhanTich - i).ToString().Trim() + ";"
                                        + SoThuNhat.ToString() + ";"
                                        + SoThuHai.ToString().Trim();
                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                    BieuThuc.LoiGiaiBaiToan = i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + " = " + SoThuNhat.ToString().Trim() + " + " + SoThuHai.ToString().Trim() + " = " + SoPhanTich.ToString().Trim();
                    BieuThuc.SoPhanTich = SoPhanTich;
                    DSBieuThuc.Add(BieuThuc);
                }
            }

            List<DanhSachBieuThucTimSoModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();

            List<DanhSachBieuThucTimSoModel> DSBieuThucMoi = new List<DanhSachBieuThucTimSoModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucTimSoModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucTimSoModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Phân tích hai số thành tổng một số với một số không đổi: 7=5+2; 6=4+2
        /// </summary>
        /// <param name="SoThuNhat"></param>
        /// <param name="SoThuHai"></param>
        /// <param name="SoKhongDoi"></param>
        /// <returns></returns>
        public DanhSachBieuThucTimSoModel PhanTichHaiSo(int SoThuNhat, int SoThuHai, int SoKhongDoi, int LoaiPhanTich = 1)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            ToolShareService AllToolShare = new ToolShareService();
            DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
            if (LoaiPhanTich == 1)
            {
                BieuThuc.BieuThuc = SoThuNhat.ToString().Trim() + ";"
                                    + SoThuHai.ToString() + ";"
                                    + (SoThuNhat + SoKhongDoi).ToString().Trim() + ";"
                                    + (SoThuHai + SoKhongDoi).ToString().Trim();
            }
            else
            {
                BieuThuc.BieuThuc = SoThuNhat.ToString().Trim() + ";"
                                    + (SoThuNhat + SoKhongDoi).ToString().Trim() + ";"
                                    + (SoThuHai + SoKhongDoi).ToString().Trim() + ";"                
                                    + SoThuHai.ToString() ;
            }
            BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
            BieuThuc.LoiGiaiBaiToan = (SoThuNhat + SoKhongDoi).ToString().Trim() + " = " + SoThuNhat.ToString().Trim() + " + " + SoKhongDoi.ToString().Trim() + " và " + (SoThuHai + SoKhongDoi).ToString().Trim() + " = " + SoThuHai.ToString().Trim() + " + " + SoKhongDoi.ToString().Trim();
            BieuThuc.SoPhanTich = SoThuNhat + SoThuHai; //Không có vai trò gì
            return BieuThuc;
        }

        /// <summary>
        /// Phân tích một số thành tổng hoặc hiệu các số hạng trừ với một số không đổi
        /// </summary>
        /// <param name="SoPhanTich">Số càn phân tích</param>
        /// <param name="PhamViPhepToan">Phạm vi phép toán chỉ áp dụng trong trường hợp phép trừ</param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> PhanTichMotSoTruBot(int SoPhanTich, int PhamViPhepToan, int LoaiPhanTich, int SoTruBot, int SoBatDau = 1)
        {
            List<DanhSachBieuThucTimSoModel> DSBieuThuc = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            if (SoPhanTich - SoTruBot > 0)
            {
                if (LoaiPhanTich == 1)
                {
                    //Dạng A=B+C
                    for (int i = SoBatDau; i <= (SoPhanTich - (SoPhanTich % 2)) / 2; i++)
                    {
                        if (SoPhanTich - i >= SoBatDau)
                        {
                            DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                            BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                + (SoPhanTich - i).ToString().Trim() + ";"
                                                + (SoPhanTich - SoTruBot).ToString().Trim();
                            BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                            if (SoTruBot == 0)
                            {
                                BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + "</b>";
                            }
                            else
                            {
                                BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + (SoPhanTich - i).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                            }
                            BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                            DSBieuThuc.Add(BieuThuc);
                        }
                    }
                }

                if (LoaiPhanTich == 2)
                {
                    //Dạng A=B-C
                    for (int i = PhamViPhepToan; i >= SoPhanTich + SoBatDau; i--)
                    {
                        DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                        BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                            + (i - SoPhanTich).ToString().Trim() + ";"
                                            + (SoPhanTich - SoTruBot).ToString().Trim();
                        BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                        if (SoTruBot == 0)
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + (i - SoPhanTich).ToString().Trim() + "</b>";
                        }
                        else
                        {
                            BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " - " + (i - SoPhanTich).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                        }
                        BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                        DSBieuThuc.Add(BieuThuc);

                    }
                }

                if (LoaiPhanTich == 3)
                {
                    //Dạng A=B+C+D
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            if (i + j <= PhamViPhepToan && SoPhanTich - i - j >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (SoPhanTich - i - j).ToString().Trim() + ";"
                                                    + (SoPhanTich - SoTruBot).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoTruBot == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + (SoPhanTich - i - j).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + (SoPhanTich - i - j).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 4)
                {
                    //Dạng A=B+C-D
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = PhamViPhepToan; j > SoPhanTich - i; j--)
                        {
                            if (i + j <= PhamViPhepToan && i + j - SoPhanTich >= SoBatDau && j >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (i + j - SoPhanTich).ToString().Trim() + ";"
                                                    + (SoPhanTich - SoTruBot).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoTruBot == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + (i + j - SoPhanTich).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + (i + j - SoPhanTich).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 5)
                {
                    //Dạng A=B-C+D
                    for (int i = PhamViPhepToan; i > SoPhanTich; i--)
                    {
                        for (int j = SoBatDau; j < i; j++)
                        {
                            if (SoPhanTich - i + j > SoBatDau && i >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (SoPhanTich - i + j).ToString().Trim() + ";"
                                                    + (SoPhanTich - SoTruBot).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoTruBot == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " + " + (SoPhanTich - i + j).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " + " + (SoPhanTich - i + j).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 6)
                {
                    //Dạng A=B-C-D
                    for (int i = PhamViPhepToan; i > SoPhanTich; i--)
                    {
                        for (int j = SoBatDau; j < i; j++)
                        {
                            if (i - j - SoPhanTich >= SoBatDau && i >= SoBatDau)
                            {
                                DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                    + j.ToString().Trim() + ";"
                                                    + (i - j - SoPhanTich).ToString().Trim() + ";"
                                                    + (SoPhanTich - SoTruBot).ToString().Trim();
                                BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                if (SoTruBot == 0)
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + (i - j - SoPhanTich).ToString().Trim() + "</b>";
                                }
                                else
                                {
                                    BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + (i - j - SoPhanTich).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                }
                                BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                DSBieuThuc.Add(BieuThuc);
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 7)
                {
                    //Dạng A=B+C+D+E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            for (int k = SoBatDau; k < SoPhanTich - i - j; k++)
                            {
                                if (i + j + k <= PhamViPhepToan && SoPhanTich - i - j - k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (SoPhanTich - i - j - k).ToString().Trim() + ";"
                                                        + (SoPhanTich - SoTruBot).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoTruBot == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " + " + (SoPhanTich - i - j - k).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " + " + (SoPhanTich - i - j - k).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 8)
                {
                    //Dạng A=B+C+D-E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j < SoPhanTich - i; j++)
                        {
                            for (int k = PhamViPhepToan; k > SoPhanTich - i - j; k--)
                            {
                                if (i + j + k <= PhamViPhepToan && i + j + k - SoPhanTich >= SoBatDau && k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (i + j + k - SoPhanTich).ToString().Trim()
                                                        + ";" + (SoPhanTich - SoTruBot).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoTruBot == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " - " + (i + j + k - SoPhanTich).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " + " + k.ToString().Trim() + " - " + (i + j + k - SoPhanTich).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 9)
                {
                    //Dạng A=B+C-D-E
                    for (int i = SoBatDau; i < SoPhanTich; i++)
                    {
                        for (int j = PhamViPhepToan; j > SoPhanTich - i; j--)
                        {
                            for (int k = PhamViPhepToan; k > SoPhanTich - i - j; k--)
                            {
                                if (i + j - k <= PhamViPhepToan && i + j - k - SoPhanTich >= SoBatDau && j >= SoBatDau && k >= SoBatDau)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = i.ToString().Trim() + ";" + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (i + j - k - SoPhanTich).ToString().Trim()
                                                        + ";" + (SoPhanTich - SoTruBot).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoTruBot == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + k.ToString().Trim() + " - " + (i + j - k - SoPhanTich).ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + i.ToString().Trim() + " + " + j.ToString().Trim() + " - " + k.ToString().Trim() + " - " + (i + j - k - SoPhanTich).ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }

                if (LoaiPhanTich == 10)
                {
                    //Dạng A=B-C-D-E
                    for (int i = SoBatDau; i <= SoPhanTich; i++)
                    {
                        for (int j = SoBatDau; j <= SoPhanTich; j++)
                        {
                            for (int k = SoBatDau; k <= SoPhanTich; k++)
                            {
                                if (SoPhanTich + i + j + k <= PhamViPhepToan && SoPhanTich - SoTruBot > 0)
                                {
                                    DanhSachBieuThucTimSoModel BieuThuc = new DanhSachBieuThucTimSoModel();
                                    BieuThuc.BieuThuc = (SoPhanTich + i + j + k).ToString().Trim() + ";"
                                                        + i.ToString().Trim() + ";"
                                                        + j.ToString().Trim() + ";"
                                                        + k.ToString().Trim() + ";"
                                                        + (SoPhanTich - SoTruBot).ToString().Trim();
                                    BieuThuc.ThuTuSapXep = rd.Next(rd.Next(1256, 25638), rd.Next(25639, 123258));
                                    if (SoTruBot == 0)
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + SoPhanTich.ToString().Trim() + " = " + (SoPhanTich + i + j + k).ToString().Trim() + " - " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + k.ToString().Trim() + "</b>";
                                    }
                                    else
                                    {
                                        BieuThuc.LoiGiaiBaiToan = " <br/>&#160;&#160;&#160; - Ta thấy: <b> " + (SoPhanTich - SoTruBot).ToString().Trim() + " = " + (SoPhanTich + i + j + k).ToString().Trim() + " - " + i.ToString().Trim() + " - " + j.ToString().Trim() + " - " + k.ToString().Trim() + " - " + SoTruBot.ToString().Trim() + "</b>";
                                    }
                                    BieuThuc.SoPhanTich = SoPhanTich - SoTruBot;
                                    DSBieuThuc.Add(BieuThuc);
                                }
                            }
                        }
                    }
                }
            }
            List<DanhSachBieuThucTimSoModel> SapXepDSBieuThuc = DSBieuThuc.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();

            List<DanhSachBieuThucTimSoModel> DSBieuThucMoi = new List<DanhSachBieuThucTimSoModel>();
            //Sinh lại số thứ tự 
            int Dem = 0;
            foreach (DanhSachBieuThucTimSoModel Item in SapXepDSBieuThuc)
            {
                Dem++;
                DanhSachBieuThucTimSoModel NewItem = Item;
                NewItem.ThuTuSapXep = Dem;
                DSBieuThucMoi.Add(NewItem);
            }

            return DSBieuThucMoi;
        }

        /// <summary>
        /// Sinh ngẫu nhiên các cấp số côngk
        /// </summary>
        /// <param name="PhanTuDauTienBeNhat"></param>
        /// <param name="PhanTuDauTienLonNhat"></param>
        /// <param name="CongSai"></param>
        /// <param name="PhamViPhepToan"></param>
        /// <param name="SoLuongPhanTu"></param>
        /// <returns></returns>
        public List<CapSoCongModel> DanhSachCSC(int PhanTuDauTienBeNhat,int PhanTuDauTienLonNhat, int CongSai, int PhamViPhepToan, int SoLuongPhanTu)
        {
            List<CapSoCongModel> DSCSC = new List<CapSoCongModel>();
            Random rd=new Random();
            int Dem = 0;
            for (int PhanTuThuNhat = PhanTuDauTienBeNhat; PhanTuThuNhat <= PhanTuDauTienLonNhat; PhanTuThuNhat++)
            {
                Dem++;
                if (PhanTuThuNhat + (SoLuongPhanTu - 1) * CongSai <= PhamViPhepToan)
                {
                    string CapSoCong = "";
                    string LoiGiai = "";
                    for (int PhanTu = 1; PhanTu <= SoLuongPhanTu; PhanTu++)
                    {
                        if (String.IsNullOrEmpty(CapSoCong))
                        {
                            CapSoCong += (PhanTuThuNhat + (PhanTu - 1) * CongSai).ToString().Trim();
                            LoiGiai += "<br/>&#160;&#160;&#160;- Số thứ " + PhanTu.ToString().Trim() + " là: <b>" + (PhanTuThuNhat + (PhanTu - 1) * CongSai).ToString().Trim() + "</b>";
                        }
                        else
                        {
                            CapSoCong += ";" + (PhanTuThuNhat + (PhanTu - 1) * CongSai).ToString().Trim();
                            LoiGiai += "<br/>&#160;&#160;&#160;- Số thứ " + PhanTu.ToString().Trim() + " là: <b>" + (PhanTuThuNhat + (PhanTu - 2) * CongSai).ToString().Trim() + " + " + CongSai.ToString().Trim() + " = " + (PhanTuThuNhat + (PhanTu - 1) * CongSai).ToString().Trim() + "</b>";
                        }
                    }
                    CapSoCongModel NewItem = new CapSoCongModel();
                    NewItem.CapSoCong = CapSoCong;
                    NewItem.ThuTuSapXep = Dem;
                    NewItem.LoiGiai ="Ta thấy "+ SoLuongPhanTu.ToString().Trim() + " số hơn kém nhau " + CongSai + LoiGiai;
                    DSCSC.Add(NewItem);
                }
            }
            return DSCSC;
        }

        /// <summary>
        /// Sinh hoán vị của cấp số cộng
        /// </summary>
        /// <param name="CapSoCong"></param>
        /// <returns></returns>
        public List<CapSoCongModel> SinhHoanVi(CapSoCongModel CapSoCong)
        {
            List<CapSoCongModel> HoanVi = new List<CapSoCongModel>();
            Random rd = new Random();
            string[] CatCacPhanTu = CapSoCong.CapSoCong.Split(';');
            int Dem = 0;
            for (int i = 0; i <= CatCacPhanTu.Length-1; i++)
            {
                Dem++;
                string CapSoCongTraVe = "";
                CapSoCongModel NewItem = new CapSoCongModel();
                for (int j = 0; j <= CatCacPhanTu.Length - 1; j++)
                {
                    if (j == i)
                    {
                        if (String.IsNullOrEmpty(CapSoCongTraVe))
                        {
                            CapSoCongTraVe += "x";
                        }
                        else
                        {
                            CapSoCongTraVe += ";" + "x";
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(CapSoCongTraVe))
                        {
                            CapSoCongTraVe += CatCacPhanTu[j];
                        }
                        else
                        {
                            CapSoCongTraVe += ";" + CatCacPhanTu[j];
                        }
                    }
                }
                NewItem.CapSoCong = CapSoCongTraVe;
                NewItem.ThuTuSapXep = Dem;
                HoanVi.Add(NewItem);
            }
            return HoanVi.OrderBy(m => m.ThuTuSapXep).ToList<CapSoCongModel>();
        }

        /// <summary>
        /// Sinh hoán vị của biểu thức các số
        /// </summary>
        /// <param name="BieuThuc"></param>
        /// <returns></returns>
        public List<DanhSachBieuThucTimSoModel> SinhHoanVi(DanhSachBieuThucTimSoModel BieuThuc)
        {
            List<DanhSachBieuThucTimSoModel> HoanVi = new List<DanhSachBieuThucTimSoModel>();
            Random rd = new Random();
            string[] CatCacPhanTu = BieuThuc.BieuThuc.Split(';');
            int Dem = 0;
            for (int i = 0; i <= CatCacPhanTu.Length - 1; i++)
            {
                Dem++;
                string KetQuaTraVe = "";
                DanhSachBieuThucTimSoModel NewItem = new DanhSachBieuThucTimSoModel();
                for (int j = 0; j <= CatCacPhanTu.Length - 1; j++)
                {
                    if (j == i)
                    {
                        if (String.IsNullOrEmpty(KetQuaTraVe))
                        {
                            KetQuaTraVe += "x";
                        }
                        else
                        {
                            KetQuaTraVe += ";" + "x";
                        }
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(KetQuaTraVe))
                        {
                            KetQuaTraVe += CatCacPhanTu[j];
                        }
                        else
                        {
                            KetQuaTraVe += ";" + CatCacPhanTu[j];
                        }
                    }
                }
                NewItem.BieuThuc = KetQuaTraVe;
                NewItem.LoiGiaiBaiToan = BieuThuc.LoiGiaiBaiToan;
                NewItem.SoPhanTich = BieuThuc.SoPhanTich;
                NewItem.ThuTuSapXep = Dem;
                HoanVi.Add(NewItem);
            }
            return HoanVi.OrderBy(m => m.ThuTuSapXep).ToList<DanhSachBieuThucTimSoModel>();
        }

        /// <summary>
        /// Lấy một CSC bởi STT
        /// </summary>
        /// <param name="DSCapSoCong"></param>
        /// <param name="SoThuTu"></param>
        /// <returns></returns>
        public CapSoCongModel LayMotCapSoCong(List<CapSoCongModel> DSCapSoCong, int SoThuTu)
        {
            CapSoCongModel ReturnItem=new CapSoCongModel();
            foreach (CapSoCongModel Item in DSCapSoCong)
            {
                if (Item.ThuTuSapXep == SoThuTu)
                {
                    ReturnItem = Item;
                    break;
                }
            }
            return ReturnItem;
        }

        /// <summary>
        /// Lấy một hoán vị
        /// </summary>
        /// <param name="DSHoanVi"></param>
        /// <param name="SoThuTu"></param>
        /// <returns></returns>
        public DanhSachBieuThucTimSoModel LayMotHoanVi(List<DanhSachBieuThucTimSoModel> DSHoanVi, int SoThuTu)
        {
            DanhSachBieuThucTimSoModel ReturnItem = new DanhSachBieuThucTimSoModel();
            foreach (DanhSachBieuThucTimSoModel Item in DSHoanVi)
            {
                if (Item.ThuTuSapXep == SoThuTu)
                {
                    ReturnItem = Item;
                    break;
                }
            }
            return ReturnItem;
        }

        /// <summary>
        /// Lấy đáp án bài toán tìm số
        /// </summary>
        /// <param name="CapSoCongGoc"></param>
        /// <param name="CapSoCongHoanVi"></param>
        /// <returns></returns>
        public string LayDapAn(CapSoCongModel CapSoCongGoc, CapSoCongModel CapSoCongHoanVi)
        {
            string DapAn = "";
            string[] PhanTuCapSoCongGoc = CapSoCongGoc.CapSoCong.Split(';');
            string[] PhanTuCapSoCongHoanVi = CapSoCongHoanVi.CapSoCong.Split(';');
            for (int DA = 0; DA <= PhanTuCapSoCongGoc.Length - 1; DA++)
            {
                if (PhanTuCapSoCongHoanVi[DA] == "x")
                {
                    DapAn = PhanTuCapSoCongGoc[DA];
                    break;
                }

            }
            return DapAn;
        }

        /// <summary>
        /// Lấy đáp án bài toán biểu thức
        /// </summary>
        /// <param name="CapSoCongGoc"></param>
        /// <param name="CapSoCongHoanVi"></param>
        /// <returns></returns>
        public string LayDapAn(DanhSachBieuThucTimSoModel BieuThucGoc, DanhSachBieuThucTimSoModel BieuThucHoanVi)
        {
            string DapAn = "";
            string[] PhanTuBieuThucGoc = BieuThucGoc.BieuThuc.Split(';');
            string[] PhanTuBieuThucHoanVi = BieuThucHoanVi.BieuThuc.Split(';');
            for (int DA = 0; DA <= PhanTuBieuThucGoc.Length - 1; DA++)
            {
                if (PhanTuBieuThucHoanVi[DA] == "x")
                {
                    DapAn = PhanTuBieuThucGoc[DA];
                    break;
                }

            }
            return DapAn;
        }
        /// <summary>
        /// Đọc danh sách các bài toán về tìm số
        /// </summary>
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public List<BaiToanTimSoModel> DanhSachBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan)
        {
            List<BaiToanTimSoModel> TatCaDanhSach = (from BaiToan in ListData.BaiToanTimSos
                                                     where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.PhanLoaiBaiToan == PhanLoaiBaiToan
                                                     orderby BaiToan.ThuTuSapXep descending
                                                     select new BaiToanTimSoModel
                                                     {
                                                         MaCauHoi = BaiToan.MaCauHoi,
                                                         ChuoiSoHienThi = BaiToan.ChuoiSoHienThi,
                                                         DapAn = BaiToan.DapAn,
                                                         PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                         PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan,
                                                         UserControlName = BaiToan.UserControlName,
                                                         ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                         ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                         LoiGiaiBaiToan=BaiToan.LoiGiaiBaiToan,
                                                     }).ToList<BaiToanTimSoModel>();
            return TatCaDanhSach;
        }


        /// <summary>
        /// Đọc bài toán tìm số bat ky
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanTimSoModel GetOneBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan)
        {
            IEnumerable<BaiToanTimSoModel> ResultList = (from BaiToan in ListData.BaiToanTimSos
                                                        where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.PhanLoaiBaiToan.Contains(PhanLoaiBaiToan)
                                                        select new BaiToanTimSoModel
                                                        {
                                                            MaCauHoi = BaiToan.MaCauHoi,
                                                            ChuoiSoHienThi = BaiToan.ChuoiSoHienThi,
                                                            DapAn = BaiToan.DapAn,
                                                            PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                            PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan,
                                                            UserControlName = BaiToan.UserControlName,
                                                            ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                            ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                            LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan,
                                                        });
            int rnd = new Random().Next(ResultList.Count());
            return ResultList.Skip(rnd).Take(1).SingleOrDefault();
        }


        /// <summary>
        /// Đọc bài toán tìm số đầu tiên
        /// </summary>
        /// <param name="ThuocKhoiLop"></param>
        /// <returns></returns>
        public BaiToanTimSoModel BaiToanTimSoDauTien(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan)
        {
            BaiToanTimSoModel MotBaiToanTimSoDauTien = (from BaiToan in ListData.BaiToanTimSos
                                                        where BaiToan.ThuocKhoiLop == ThuocKhoiLop && BaiToan.PhamViPhepToan == PhamViPhepToan && BaiToan.PhanLoaiBaiToan == PhanLoaiBaiToan
                                                        orderby BaiToan.ThuTuSapXep descending
                                                        select new BaiToanTimSoModel
                                                        {
                                                            MaCauHoi = BaiToan.MaCauHoi,
                                                            ChuoiSoHienThi = BaiToan.ChuoiSoHienThi,
                                                            DapAn = BaiToan.DapAn,
                                                            PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                            PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan,
                                                            UserControlName = BaiToan.UserControlName,
                                                            ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                            ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                            LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan,
                                                        }).First<BaiToanTimSoModel>();
            return MotBaiToanTimSoDauTien;
        }

        /// <summary>
        /// Đọc một bài toán tìm số
        /// </summary>
        /// <param name="MaCauHoi"></param>
        /// <returns></returns>
        public BaiToanTimSoModel DocMotBaiToanTimSo(string MaCauHoi)
        {
            Guid MaCauHoiDoc = new Guid(MaCauHoi);
            BaiToanTimSoModel MotCauHoiDoc = (from BaiToan in ListData.BaiToanTimSos
                                                 where BaiToan.MaCauHoi == MaCauHoiDoc
                                                 select new BaiToanTimSoModel
                                                 {
                                                     MaCauHoi = BaiToan.MaCauHoi,
                                                     ChuoiSoHienThi = BaiToan.ChuoiSoHienThi,
                                                     DapAn = BaiToan.DapAn,
                                                     PhamViPhepToan = BaiToan.PhamViPhepToan,
                                                     PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan,
                                                     UserControlName = BaiToan.UserControlName,
                                                     ThuTuSapXep = BaiToan.ThuTuSapXep,
                                                     ThuocKhoiLop = BaiToan.ThuocKhoiLop,
                                                     LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan,
                                                 }).SingleOrDefault<BaiToanTimSoModel>();
            return MotCauHoiDoc;
        }

        /// <summary>
        /// Thêm mới một bài toán tìm số
        /// </summary>
        /// <param name="model"></param>
        public string ThemMoiMotBaiToanTimSo(BaiToanTimSoModel BaiToan)
        {
            try
            {
                Table<BaiToanTimSo> BangBaiToan = ListData.GetTable<BaiToanTimSo>();
                BaiToanTimSo BaiToanTimSoItem = new BaiToanTimSo();
                BaiToanTimSoItem.MaCauHoi = BaiToan.MaCauHoi;
                BaiToanTimSoItem.ChuoiSoHienThi = BaiToan.ChuoiSoHienThi;
                BaiToanTimSoItem.DapAn = BaiToan.DapAn;
                BaiToanTimSoItem.PhamViPhepToan = BaiToan.PhamViPhepToan;
                BaiToanTimSoItem.PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan;
                BaiToanTimSoItem.UserControlName = BaiToan.UserControlName;
                BaiToanTimSoItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BaiToanTimSoItem.LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan;
                BangBaiToan.InsertOnSubmit(BaiToanTimSoItem);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể thêm mới được bài toán này!";
            }
        }

        /// <summary>
        /// Sửa một dyax số
        /// </summary>
        /// <param name="model"></param>
        public string SuaCauHoi(BaiToanTimSoModel BaiToan)
        {
            try
            {
                var BaiToanTimSoItem = ListData.BaiToanTimSos.Single(m => m.MaCauHoi == BaiToan.MaCauHoi);
                BaiToanTimSoItem.ChuoiSoHienThi = BaiToan.ChuoiSoHienThi;
                BaiToanTimSoItem.DapAn = BaiToan.DapAn;
                BaiToanTimSoItem.PhamViPhepToan = BaiToan.PhamViPhepToan;
                BaiToanTimSoItem.PhanLoaiBaiToan = BaiToan.PhanLoaiBaiToan;
                BaiToanTimSoItem.UserControlName = BaiToan.UserControlName;
                BaiToanTimSoItem.ThuocKhoiLop = BaiToan.ThuocKhoiLop;
                BaiToanTimSoItem.LoiGiaiBaiToan = BaiToan.LoiGiaiBaiToan;
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể sửa được bài toán tìm số này!";
            }
        }

        /// <summary>
        /// Xóa một bài toán tìm số
        /// </summary>
        /// <param name="id"></param>
        public string XoaBaiToanTimSo(string MaCauHoi)
        {
            Guid MaBaiToanXoa = new Guid(MaCauHoi);
            try
            {
                var BaiToanCanXoa = ListData.BaiToanTimSos.Where(m => m.MaCauHoi == MaBaiToanXoa);
                ListData.BaiToanTimSos.DeleteAllOnSubmit(BaiToanCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được bài toán tìm số này!";
            }
        }

        /// <summary>
        /// Xóa nhiều bài toán tìm số
        /// </summary>
        /// <param name="id"></param>
        public string XoaNhieuBaiToanTimSo(string ThuocKhoiLop, string PhamViPhepToan, string PhanLoaiBaiToan)
        {
            try
            {
                var CacBaiToanTimSoCanXoa = ListData.BaiToanTimSos.Where(m => m.ThuocKhoiLop == ThuocKhoiLop).Where(m => m.PhamViPhepToan == PhamViPhepToan).Where(m => m.PhanLoaiBaiToan == PhanLoaiBaiToan);
                ListData.BaiToanTimSos.DeleteAllOnSubmit(CacBaiToanTimSoCanXoa);
                ListData.SubmitChanges();
                return "";
            }
            catch
            {
                return "Không thể xóa được các bài toán tìm số này!";
            }
        }

        #endregion

    }
}