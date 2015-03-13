<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%if (Request.IsAuthenticated) 
  {%>
        Xin chào: <b><%:ViewData["TenThanhVien"]%></b>(<%:ViewData["LoaiThanhVien"]%>)!
        [ <%: Html.ActionLink("Đăng xuất", "LogOff", "Account") %> ][<%: Html.ActionLink("Đổi mật khẩu", "ChangePassword", "Home")%>][<%: Html.ActionLink("Hồ sơ", "Profile", "Home")%>]<br />
        <%if (ViewData["Role"].ToString().Trim()=="3")
        {%>
            Ngày đăng ký: <b><%: ViewData["NgayDangKy"] %></b> - Số lần đăng nhập:<b><%: ViewData["SoLanDangNhap"] %></b>
        <%} %>
        <%if (ViewData["Role"].ToString().Trim()=="2")
        {%>
            Ngày tính phí: <b><%: ViewData["NgayTinhPhi"]%></b> - Ngày hết hạn: <b><%: ViewData["NgayHetHan"]%></b> - Số lần đăng nhập: <b><%: ViewData["SoLanDangNhap"] %></b>
        <%} %>
   <%}%>
