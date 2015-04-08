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

    public class NoticeModel
    {
        [DisplayName("Lời thông báo")]
        public string NoticeContent { get; set; }

        [DisplayName("Tên Controler quay lại")]
        public string NoticeControler { get; set; }

        [DisplayName("Tên Action quay lại")]
        public string NoticeAction { get; set; }

        [DisplayName("Giá trị biến 1")]
        public string memvar1 { get; set; }

        [DisplayName("Giá trị biến 2")]
        public string memvar2 { get; set; }

        [DisplayName("Giá trị biến 3")]
        public string memvar3 { get; set; }

        [DisplayName("Giá trị biến 4")]
        public string memvar4 { get; set; }

        [DisplayName("Giá trị biến 5")]
        public string memvar5 { get; set; }
    }

    public class ErrorModel
    {
        [DisplayName("Lời thông báo")]
        public string ErrorContent { get; set; }

        [DisplayName("Tên Controler quay lại")]
        public string ErrorControler { get; set; }

        [DisplayName("Tên Action quay lại")]
        public string ErrorAction { get; set; }

        [DisplayName("Menu bên trái")]
        public string LeftMenuName { get; set; }

        [DisplayName("Giá trị biến 1")]
        public string memvar1 { get; set; }

        [DisplayName("Giá trị biến 2")]
        public string memvar2 { get; set; }

        [DisplayName("Giá trị biến 3")]
        public string memvar3 { get; set; }

        [DisplayName("Giá trị biến 4")]
        public string memvar4 { get; set; }

        [DisplayName("Giá trị biến 5")]
        public string memvar5 { get; set; }
    }

    public class BackHomeModel
    {
        public string Controler { get; set; }
        public string Action { get; set; }
        public string ControlerName { get; set; }
        public string ActionName { get; set; }
        public string memvar1 { get; set; }
        public string memvar2 { get; set; }
        public string memvar3 { get; set; }
        public string memvar4 { get; set; }
        public string memvar5 { get; set; }
    }

    public class PagesModel
    {
        public string Controler { get; set; }
        public string Action { get; set; }
        public string memvar2 { get; set; }
        public string memvar3 { get; set; }
        public string memvar4 { get; set; }
        public string memvar5 { get; set; }
        public int NumberPages { get; set; }
        public int CurentPage { get; set; }
    }

    public class ListMemvarModel
    {
        public string Memvar1 { get; set; }
        public string Value1 { get; set; }
        public string Memvar2 { get; set; }
        public string Value2 { get; set; }
    }

    public class AjaxPagesModel
    {
        public string FuncName { get; set; }
        public int NumberPages { get; set; }
        public int CurentPage { get; set; }
        public string Controler { get; set; }
        public string Action { get; set; }
        public string memvar1 { get; set; }
        public string memvar2 { get; set; }
        public string memvar3 { get; set; }
        public string memvar4{ get; set; }
        public string memvar5 { get; set; }
    }

    public class FileUploadModel
    {
        [Required(ErrorMessage = "Phải chọn File sao lưu có định dạng \".bak\"")]
        [DisplayName("Chọn file upload:")]
        public HttpPostedFileBase FileUpload { get; set; }
    }

    public class ListFindMax
    {
        public int FieldFindMax { get; set; }
    }

    public class ListSelectItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    public class ListSearch
    {
        public string FieldSearch { get; set; }
    }

    public class PagesSelect
    {
        public string Values { get; set; }
        public string TitleActive { get; set; }
    }

    public class CheckDupItemModel
    {
        public string ID { get; set; }
        public string FieldValue { get; set; }
    }

    public class ErrorsModel
    {
        public string TextError { get; set; }
        public string TaskCurrent { get; set; }
        public string Controler { get; set; }
        public string Action { get; set; }
        public string memvar1 { get; set; }
        public string memvar2 { get; set; }
        public string memvar3 { get; set; }
        public string memvar4 { get; set; }
        public string memvar5 { get; set; }
    }

    public interface ShareService
    {
        string GetRandomId(List<ListSearch> ListItem, string Key);
        bool SearchValue(List<ListSearch> ListItem, string KeySearch);
        int GetMaxOrderby(List<ListFindMax> ListItem);
        bool SearchDupValue(List<CheckDupItemModel> ListItem, string IdCheck, string ValueCheck);
        int GetNumberPages(int NumRecord, int NumRecInPages);
        string GetRandomValue(string Key);
        string GetDateNow();
        double ConvertToNumber(string TheNumber, string DigitGroupSplit, string DecimalSplit);
        string NumberFormat(string Number, string DigitGroupSplit, string DecimalSplit);
        string ReadNumber(string numinput);
        double GetNumberFromNumberFormat(string Number, string DigitGroupSplit, string DecimalSplit);
        string GetDateFormDateTime(DateTime DateInput);
        string GetDateAddYear(DateTime DateAdd, int YearAdd);
        bool CheckExpiredDate(DateTime Date);
        List<PagesSelect> CreateList(int Start, int end, int step);
        string BoDauTiengViet(string str);
        string ChuanHoaXauTiengViet(string inputstr);
        string HuyDauCachTrongXau(string str);
        string GetPage(AjaxPagesModel Model);
        int SoSanhHaiNgay(string NgayThangNamChuan, string NgayThangNamSoSanh);
        string CreateDate(string str_Date);
        int LayMaNgauNhien(int Start, int End, string NotIn);

        
    }

    public class ToolShareService : ShareService
    {
        ToanThongMinhDataContext ListData = new ToanThongMinhDataContext();

        /// <summary>
        /// Chuẩn hóa một ngày tháng năm
        /// </summary>
        /// <param name="str_Date"></param>
        /// <returns></returns>
        public string CreateDate(string str_Date)
        {
            string ReturnDate = "";
            if (!String.IsNullOrEmpty(str_Date))
            {
                string[] InputDate = str_Date.Trim().Split('/');
                if (InputDate.Length == 1)
                {
                    ReturnDate = "01/06/" + str_Date.Trim();
                }
                else if (InputDate.Length == 2)
                {
                    ReturnDate = "15/" + str_Date.Trim();
                }
                else
                {
                    ReturnDate = str_Date;
                }
            }
            return ReturnDate;
        }

        /// <summary>
        /// Tạo một ListModel làm selectList
        /// </summary>
        /// <param name="Start"></param>
        /// <param name="end"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public List<PagesSelect> CreateList(int Start, int end, int step)
        {
            List<PagesSelect> ListNumRecord = new List<PagesSelect>();
            for (int i = Start; i <= end; i = i + step)
            {
                PagesSelect Item = new PagesSelect();
                Item.TitleActive = i.ToString().Trim();
                Item.Values = i.ToString().Trim();
                ListNumRecord.Add(Item);
            }
            return ListNumRecord;
        }
        /// <summary>
        /// Kiểm tra có sự trùng lặp bản ghi hay không
        /// </summary>
        /// <param name="ListItem"></param>
        /// <param name="IdCheck"></param>
        /// <param name="ValueCheck"></param>
        /// <returns></returns>
        public bool SearchDupValue(List<CheckDupItemModel> ListItem, string IdCheck, string ValueCheck)
        {
            bool KiemTra = true;
            foreach (var item in ListItem)
            {
                if (item.ID.Trim() != IdCheck.Trim() && item.FieldValue.Trim() == ValueCheck.Trim())
                {
                    KiemTra = false;
                    break;
                }
            }
            return KiemTra;
        }
  
        /// <summary>
        /// Lấy giá trị lớn nhất của một cột trong ListItem
        /// </summary>
        /// <param name="ListItem"></param>
        /// <returns></returns>
        public int GetMaxOrderby(List<ListFindMax> ListItem)
        {
            int kq = 0;
            foreach (var item in ListItem)
            {
                if (item.FieldFindMax > kq)
                {
                    kq = item.FieldFindMax;
                }
            }
            return kq + 1;
        }
        
        /// <summary>
        /// Lấy số trang 
        /// </summary>
        /// <param name="NumRecord"></param>
        /// <param name="NumRecInPages"></param>
        /// <returns></returns>
        public int GetNumberPages(int NumRecord, int NumRecInPages)
        {
            int kq = 1;
            if (NumRecInPages != 0)
            {
                kq = (int)(NumRecord / NumRecInPages);
                if (kq * NumRecInPages < NumRecord)
                {
                    kq++;
                }
            }
            return kq;
        }
        /// <summary>
        /// Kiểm tra xem một mã nào đó đã có chưa
        /// </summary>
        /// <param name="ListItem">List mã đem kiểm tra</param>
        /// <param name="KeySearch">Mã cần kiểm tra</param>
        /// <returns></returns>
        public bool SearchValue(List<ListSearch> ListItem, string KeySearch)
        {
            bool KiemTra = true;
            foreach (var item in ListItem)
            {
                if (item.FieldSearch == KeySearch)
                {
                    KiemTra = false;
                    break;
                }
            }
            return KiemTra;
        }
        /// <summary>
        /// Sinh ngẫu nhiên một mã
        /// </summary>
        /// <param name="ListItem">ListItem là list mã đã có</param>
        /// <param name="Key">Key là Tiền tố của mã</param>
        /// <returns></returns>
        public string GetRandomId(List<ListSearch> ListItem, string Key)
        {
            Random rd = new Random();
            int GiaTriNgauNhien = rd.Next(1000, 1999999999);
            string IDRandom = Key  + Convert.ToString(GiaTriNgauNhien).Trim();
            while (!SearchValue(ListItem, IDRandom))
            {
                GiaTriNgauNhien = rd.Next(1000, 1999999999);
                IDRandom = Key + Convert.ToString(GiaTriNgauNhien);
            }
            return IDRandom;
        }

        /// <summary>
        /// Sinh ngẫu nhiên giá trị
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        public string GetRandomValue(string Key)
        {
            Random rd = new Random();
            int GiaTriNgauNhien = rd.Next(1000, 1999999999);
            string IDRandom = Key + Convert.ToString(GiaTriNgauNhien).Trim();
            return IDRandom;
        }

        bool KiemTraMa(string MaKiemTra, string[] DanhSachMa)
        {
            bool KetQua = true;
            for (int i = 0; i < DanhSachMa.Length; i++)
            {
                if (MaKiemTra.Trim() == DanhSachMa[i].Trim())
                {
                    KetQua = false;
                    break;
                }
            }
            return KetQua;
        }

        public int LayMaNgauNhien(int Start, int End, string NotIn)
        {
            Random rd = new Random();
            int GiaTriNgauNhien = (rd.Next(rd.Next(1, 683624), rd.Next(683625, 5983625)) % End) + 1;
            if (!String.IsNullOrEmpty(NotIn))
            {
                string[] DanhSachMa = NotIn.Split('$');
                while (!KiemTraMa(GiaTriNgauNhien.ToString().Trim(), DanhSachMa) || GiaTriNgauNhien < Start)
                {
                    GiaTriNgauNhien = (rd.Next(rd.Next(1, 683624), rd.Next(683625, 5983625)) % End) + 1;
                }
            }
            else
            {
                while (GiaTriNgauNhien < Start)
                {
                    GiaTriNgauNhien = (rd.Next(rd.Next(1, 683624), rd.Next(683625, 5983625)) % End) + 1;
                }
            }
            return GiaTriNgauNhien;
        }

        /// <summary>
        /// Lấy ngày hiện tại dạng dd/mm/yyyy
        /// </summary>
        /// <returns></returns>
        public string GetDateNow()
        {
            string Dte = DateTime.Now.Date.Day.ToString();
            string Mth = DateTime.Now.Month.ToString();
            string yr = DateTime.Now.Year.ToString();
            if (DateTime.Now.Date.Day < 10)
            {
                Dte = "0" + DateTime.Now.Date.Day.ToString().Trim();
            }
            if (DateTime.Now.Month < 10)
            {
                Mth = "0" + DateTime.Now.Month.ToString().Trim();
            }
            return Dte + "/" + Mth + "/" + yr;
        }

        /// <summary>
        /// Kiểm tra xem tài khoản đã hết hạn hay chưa? Hết hạn: False; Còn hạn: True
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public bool CheckExpiredDate(DateTime Date)
        {
            DateTime DateNow = DateTime.Now.Date;
            if (DateNow <= Date)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// So sánh hai ngày
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public int SoSanhHaiNgay(string NgayThangNamChuan, string NgayThangNamSoSanh)
        {
            int Ketqua = 0;
            if (String.IsNullOrEmpty(NgayThangNamSoSanh))
            {
                Ketqua= 0; //Không so sánh được
            }
            else
            {
                string[] CatNgayThangNamSoSanh = NgayThangNamSoSanh.Trim().Split('/');
                string[] CatNgayThangNamChuan = NgayThangNamChuan.Trim().Split('/');
                if (CatNgayThangNamSoSanh.Length == 1)
                {
                    try
                    {
                        if (Convert.ToInt32(CatNgayThangNamSoSanh[0]) > Convert.ToInt32(CatNgayThangNamChuan[2]))
                        {
                            Ketqua= 1; //Lớn hơn
                        } 
                        else if (Convert.ToInt32(CatNgayThangNamSoSanh[0]) == Convert.ToInt32(CatNgayThangNamChuan[2]))
                        {
                            Ketqua= 2; //Bằng nhau
                        }
                        else
                        {
                            Ketqua= 3; //bé  hơn
                        }
                    }
                    catch
                    {
                        Ketqua= 0; //Không so sánh được
                    }
                }
                else if (CatNgayThangNamSoSanh.Length == 2)
                {
                    try
                    {
                        if (Convert.ToInt32(CatNgayThangNamSoSanh[1]) > Convert.ToInt32(CatNgayThangNamChuan[2]))
                        {
                            Ketqua= 1; //Lớn hơn
                        }
                        else if (Convert.ToInt32(CatNgayThangNamSoSanh[1]) < Convert.ToInt32(CatNgayThangNamChuan[2]))
                        {
                            Ketqua= 3; //bé  hơn
                        }
                        else
                        {
                            if (Convert.ToInt32(CatNgayThangNamSoSanh[0]) == Convert.ToInt32(CatNgayThangNamChuan[1]))
                            {
                                Ketqua= 2; //Bằng nhau
                            }
                            if (Convert.ToInt32(CatNgayThangNamSoSanh[0]) < Convert.ToInt32(CatNgayThangNamChuan[1]))
                            {
                                Ketqua= 3; //bé  hơn
                            }
                            if (Convert.ToInt32(CatNgayThangNamSoSanh[0]) > Convert.ToInt32(CatNgayThangNamChuan[1]))
                            {
                                Ketqua= 1; //lớn  hơn
                            }
                        }
                    }
                    catch
                    {
                        Ketqua= 0; //Không so sánh được
                    }
                }
                else if (CatNgayThangNamSoSanh.Length == 3)
                {
                    try
                    {
                        System.Globalization.DateTimeFormatInfo DateFormat = new System.Globalization.DateTimeFormatInfo();
                        DateFormat.ShortDatePattern = "dd/MM/yyyy";
                        DateTime NgayChuan = Convert.ToDateTime(NgayThangNamChuan, DateFormat);
                        DateTime NgaySoSanh = Convert.ToDateTime(NgayThangNamSoSanh, DateFormat);
                        if (NgaySoSanh > NgayChuan)
                        {
                            Ketqua= 1; //lớn  hơn
                        }
                        else if (NgaySoSanh == NgayChuan)
                        {
                            Ketqua= 2; //Bằng nhau
                        }
                        else
                        {
                            Ketqua= 3; //bé  hơn
                        }
                    }
                    catch
                    {
                        Ketqua= 0; //Không so sánh được
                    }
                }
                else
                {
                    Ketqua= 0; //Không so sánh được
                }
            }
            return Ketqua;
       }

        
        /// <summary>
        /// Ngày này của những năm sau
        /// </summary>
        /// <param name="DateAdd"></param>
        /// <param name="YearAdd"></param>
        /// <returns></returns>
        public string GetDateAddYear(DateTime DateAdd, int YearAdd)
        {
            string Dte = DateAdd.Day.ToString();
            string Mth = DateAdd.Month.ToString();
            string yr = (DateAdd.Year + YearAdd).ToString();
            if (DateAdd.Day < 10)
            {
                Dte = "0" + DateAdd.Day.ToString().Trim();
            }
            if (DateAdd.Month < 10)
            {
                Mth = "0" + DateAdd.Month.ToString().Trim();
            }
            return Dte + "/" + Mth + "/" + yr;
        }

        /// <summary>
        /// Chuyển từ một ngày dạng DateTime sang "dd/mm/yyyy"
        /// </summary>
        /// <param name="DateInput"></param>
        /// <returns></returns>
        public string GetDateFormDateTime(DateTime DateInput)
        {
            string day = "";
            if (DateInput.Day < 10)
            {
                day = "0" + DateInput.Day.ToString().Trim();
            }
            else
            {
                day = DateInput.Day.ToString().Trim();
            }
            string month = "";
            if (DateInput.Month < 10)
            {
                month = "0" + DateInput.Month.ToString().Trim();
            }
            else
            {
                month = DateInput.Month.ToString().Trim();
            }
            return day + "/" + month + "/" + DateInput.Year.ToString().Trim();
        }
        /// <summary>
        /// Chuyển một sâu định dạng 12,365.25 thành 12365.25
        /// </summary>
        /// <param name="TheNumber"></param>
        /// <param name="DigitGroupSplit"></param>
        /// <param name="DecimalSplit"></param>
        /// <returns></returns>
        public double ConvertToNumber(string TheNumber, string DigitGroupSplit, string DecimalSplit)
        {
            string Return = "";
            for (int i = 0; i < TheNumber.Length; i++)
            {
                if (TheNumber.Substring(i, 1) != DigitGroupSplit)
                {
                    Return = Return + TheNumber.Substring(i, 1);
                }
            }
            return Convert.ToDouble(Return);
        }
        /// <summary>
        /// Định dạng số từ 12354.36 thành 12,354.36
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="DigitGroupSplit"></param>
        /// <param name="DecimalSplit"></param>
        /// <returns></returns>
        public string NumberFormat(string Number, string DigitGroupSplit, string DecimalSplit)
        {
            string dauam = "";
            double so = Convert.ToDouble(Number.Trim());
            if (Convert.ToDouble(Number.Trim()) < 0)
            {
                dauam = "-";
                so = -Convert.ToDouble(Number.Trim());
            }
            string num1 = Convert.ToString(so);
            string[] s = num1.Split('.');
            string g3, str = "";
            while (s[0].Length > 3)
            {
                g3 = s[0].Substring(s[0].Length - 3, 3);
                s[0] = s[0].Substring(0, s[0].Length - 3);
                str = DigitGroupSplit + g3 + str;
            }
            str = s[0] + str;
            if (s.Length == 2)
            {
                return (dauam + str + DecimalSplit + s[1]);
            }
            else
            {
                return dauam + str;
            }
        }

        private static readonly string[] VietnameseSigns = new string[]
        {
 
            "aAeEoOuUiIdDyY",
 
            "áàạảãâấầậẩẫăắằặẳẵ",
 
            "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
 
            "éèẹẻẽêếềệểễ",
 
            "ÉÈẸẺẼÊẾỀỆỂỄ",
 
            "óòọỏõôốồộổỗơớờợởỡ",
 
            "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
 
            "úùụủũưứừựửữ",
 
            "ÚÙỤỦŨƯỨỪỰỬỮ",
 
            "íìịỉĩ",
 
            "ÍÌỊỈĨ",
 
            "đ",
 
            "Đ",
 
            "ýỳỵỷỹ",
 
            "ÝỲỴỶỸ"
 
        };
        /// <summary>
        /// Bỏ dấu trong xâu, chuẩn hóa tiếng việt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string BoDauTiengViet(string str) 
        {
           
            for (int i = 1; i < VietnameseSigns.Length; i++)
            {
                for (int j = 0; j < VietnameseSigns[i].Length; j++)
                    str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
            }
            return str;
        }

        /// <summary>
        /// Hủy bỏ các dấu cách trong chuỗi ký tự
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public string HuyDauCachTrongXau(string str)
        {
            string KetQua = "";
            if (!String.IsNullOrEmpty(str))
            {
                string[] DanhSachCacTu = str.Trim().Split(' ');
                for (int i = 0; i <= DanhSachCacTu.Length - 1; i++)
                {
                    KetQua = KetQua + DanhSachCacTu[i];
                }
            }
            return KetQua;
        }
        /// <summary>
        /// Chuẩn hóa xâu tiếng việt
        /// </summary>
        /// <param name="inputstr"></param>
        /// <returns></returns>
        public string ChuanHoaXauTiengViet(string inputstr) 
        {
            string st1 = "";
            if (!String.IsNullOrEmpty(inputstr))
            {
                string st = inputstr.Trim().ToLower();

                while (st.Trim().Length != 0)
                {
                    st += " ";
                    int i = st.IndexOf(" ");
                    string d = st.Substring(0, i);
                    d = char.ToUpper(d[0]) + d.Substring(1);
                    st = st.Substring(i + 1).Trim();
                    st1 += d.Trim() + " ";
                }
            }
            return st1;
        }
        /// <summary>
        /// Chuyển một số thực đã định dạng về dạng số có thể tính toán được
        /// </summary>
        /// <param name="Number"></param>
        /// <param name="DigitGroupSplit"></param>
        /// <param name="DecimalSplit"></param>
        /// <returns></returns>
        public double GetNumberFromNumberFormat(string Number, string DigitGroupSplit, string DecimalSplit)
        {
            double KetQua;
            if (!String.IsNullOrEmpty(Number))
            {
                string StringNumber = Number.Trim();
                string Return = "";
                for (int i = 0; i < Number.Length; i++)
                {
                    if (StringNumber.Substring(i, 1) != DigitGroupSplit)
                    {
                        Return = Return + StringNumber.Substring(i, 1);
                    }
                }
                try
                {
                    KetQua = double.Parse(Return);
                }
                catch
                {
                    KetQua = 0;
                }
            }
            else
            {
                KetQua = 0;
            }
            return KetQua;
        }

        #region Đọc số
        /// <summary>
        /// Đọc số
        /// </summary>
        /// <param name="numinput"></param>
        /// <returns></returns>
        public string ReadNumber(string numinput)
        {
            string str = numinput.Trim();
            return No2Str(str);
        }

        public string No2Str(string num)
        {
            string intstr, fracstr, am;
            // Xử lý khi là số âm
            if (num.Substring(0, 1) == "-")
            {
                am = "âm ";
                num = num.Replace("-", "");
            }
            else { am = ""; }

            string[] str = num.Split('.');
            // Số quá lớn nhiều hơn 27 chữ số
            if (str[0].Length > 28)
            {
                return "";
            }

            string s;
            // Xử lý phần số, nếu là có số thập phân hoặc không
            if (str.Length == 2)
            {
                intstr = IntNum2Str(str[0]);
                if (str[1].Length <= 2)
                {
                    if (str[1].Substring(0, 1) == "0")
                    {
                        fracstr = FracNum2Str(str[1]);
                    }
                    else
                    {
                        fracstr = IntNum2Str(str[1]);
                    }
                }
                else
                {
                    fracstr = FracNum2Str(str[1]);
                }
                s = (intstr + " phảy " + fracstr);
            }
            else
            {
                intstr = IntNum2Str(str[0]);
                s = intstr;
            }
            s = am + s;
            string chuhoa = s.Substring(0, 1).ToUpper();
            s = s.Substring(1, s.Length - 1);
            return (chuhoa + s);
        }

        public string IntNum2Str(string num)
        {
            string[] Cap = { "", " nghìn ", " triệu ", " tỷ ", " nghìn tỷ ", " triệu tỷ ", " tỷ tỷ ", " nghìn tỷ tỷ " };
            string kq = "", str = num, g3, kqtg;
            int caps = 0;
            while (str.Length > 3)
            {
                g3 = str.Substring(str.Length - 3, 3);
                str = str.Substring(0, str.Length - 3);
                if (g3 != "000")
                { kqtg = Group32StrX(g3) + Cap[Convert.ToByte(caps)]; }
                else { kqtg = ""; }
                kq = kqtg + kq;
                caps++;
            }
            //Chuẩn bị trước khi sử dụng hàm Group32Str1
            while (str.Length < 3)
            { str = "0" + str; }

            if ((str == "000") && (num.Length <= 3))
            { kqtg = "không"; }
            else
            { kqtg = Group32StrX(str) + Cap[Convert.ToByte(caps)]; }
            kq = kqtg + kq;
            return kq;
        }

        public string Group32StrX(string num)
        {
            string[] No = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string kq, tram, chuc, donvi;
            // Trăm
            if (num.Substring(0, 1) == "0")
            { tram = ""; }
            else
            { tram = No[Convert.ToByte(num.Substring(0, 1))] + " trăm "; }
            // Chục
            switch (num.Substring(1, 1))
            {
                case "0": if (num.Substring(2, 1) != "0" && num.Substring(0, 1) != "0")
                    {
                        chuc = "linh ";
                    }
                    else { chuc = ""; }; break;
                case "1": chuc = "mười "; break;
                default:
                    chuc = No[Convert.ToByte(num.Substring(1, 1))] + " mươi "; break;
            }
            // Đơn vị
            switch (num.Substring(2, 1))
            {
                case "0": donvi = ""; break;
                case "1": if ((num.Substring(1, 1) == "0") || (num.Substring(1, 1) == "1"))
                    {
                        donvi = "một";
                    }
                    else
                    {
                        donvi = "mốt";
                    }; break;
                case "5":
                    if (num.Substring(1, 1) != "0")
                    {
                        donvi = "lăm";
                    }
                    else
                    {
                        donvi = "năm";
                    }; break;
                default:
                    donvi = No[Convert.ToByte(num.Substring(2, 1))]; break;
            }
            kq = tram + chuc + donvi;
            return kq;
        }
        public string FracNum2Str(string num)
        {
            string[] No = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string kq = "";
            for (int i = 0; i < num.Length; i++)
            {
                kq += No[Convert.ToByte(num.Substring(i, 1))] + " ";
            }
            return kq;
        }
        #endregion

        public string GetPage(AjaxPagesModel Model)
        {
            string HTMLReturn = "";
            int n = (int)Model.NumberPages;
            int BatDau = 1;
            int KetThuc = n;
            int Trangtruoc = 1;
            int TrangSau = n;
            if (n > 1)
            {
                HTMLReturn = HTMLReturn + "<div class=\"paging\">";
                HTMLReturn = HTMLReturn + "<span>[Tổng số: " + n + " trang] </span>";
                if ((int)Model.CurentPage - 3 > 1)
                {
                    BatDau = (int)Model.CurentPage - 3;
                }
                if ((int)Model.CurentPage + 3 < n)
                {
                    KetThuc = (int)Model.CurentPage + 3;
                }
                if ((int)Model.CurentPage == 1)
                {
                    HTMLReturn = HTMLReturn + "<a href=\"#\">|<<</a>";
                }
                else
                {
                    HTMLReturn = HTMLReturn + "<a onclick=\"" + Model.FuncName + "('1');\">|<<</a>";
                }
                if ((int)Model.CurentPage - 1 >= 1)
                {
                    Trangtruoc = (int)Model.CurentPage - 1;
                    HTMLReturn = HTMLReturn + "<a onclick=\"" + Model.FuncName + " ('" + Trangtruoc + "');\"><<</a>";
                }
                else
                {
                    HTMLReturn = HTMLReturn + "<a href=\"#\"><<</a>";
                }
                if (BatDau > 1)
                {
                    HTMLReturn = HTMLReturn + "<span>...</span>";
                }
                for (int i = BatDau; i <= KetThuc; i++)
                {
                    if (i == (int)Model.CurentPage)
                    {
                        HTMLReturn = HTMLReturn + "<a class=\"current\" href=\"#\">" + i + "</a>";
                    }
                    else
                    {
                        HTMLReturn = HTMLReturn + "<a onclick=\"" + Model.FuncName + "('" + i + "');\">" + i + "</a>";
                    }
                }
                if (KetThuc < n)
                {
                    HTMLReturn = HTMLReturn + "<span>...</span>";
                }
                if ((int)Model.CurentPage + 1 <= n)
                {
                    TrangSau = (int)Model.CurentPage + 1;
                    HTMLReturn = HTMLReturn + "<a onclick=\"" + Model.FuncName + "('" + TrangSau + "');\">>></a>";
                }
                else
                {
                    HTMLReturn = HTMLReturn + "<a href=\"#\">>></a>";
                }
                if ((int)Model.CurentPage == n)
                {
                    HTMLReturn = HTMLReturn + "<a href=\"#\">>>|</a>";
                }
                else
                {
                    HTMLReturn = HTMLReturn + "<a onclick=\"" + Model.FuncName + "('" + n + "');\">>>|</a>";
                }
                HTMLReturn = HTMLReturn + "</div>";
            }
            return HTMLReturn;
        }
    }
}