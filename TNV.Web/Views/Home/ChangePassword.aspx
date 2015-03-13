<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
               <div id="FooterTitle">THAY ĐỔI MẬT KHẨU</div>
               <table width="80%">
                     <tr>
                        <td height="30px" align="left">
                            Nhập mật khẩu mới:
                        </td>
                        <td height="30px" align="left">
                            <%: Html.Password("NewPassword", "", new { @class = "Input", @title = "Bạn hãy nhập \"Mật khẩu đăng nhập\" bao gồm các chữ cái thường hoặc chữ cái hoa, chữ số, và một số ký hiệu đặc biệt <b>! @ $</b>. Mật khẩu đăng nhập được viết liền, không chứa dấu cách, nên chứa đồng thời cả chữa cái, chữa số, ký hiệu đặc biệt. Độ dài tối thiểu của mật khẩu là 6 ký tự. Hai lần nhập mật khẩu phải trùng khớp!"})%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            Nhập lại mật khẩu mới:
                        </td>
                        <td height="30px" align="left">
                            <%: Html.Password("NewConfirmPasswords", "", new { @class = "Input", @title = "Bạn hãy nhập \"Mật khẩu đăng nhập\" bao gồm các chữ cái thường hoặc chữ cái hoa, chữ số, và một số ký hiệu đặc biệt <b>! @ $</b>. Mật khẩu đăng nhập được viết liền, không chứa dấu cách, nên chứa đồng thời cả chữa cái, chữa số, ký hiệu đặc biệt. Độ dài tối thiểu của mật khẩu là 6 ký tự. Hai lần nhập mật khẩu phải trùng khớp!" })%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="center" colspan="2">
                            <img alt="Thay đổi mật khẩu" title="Bấm nút để thay đổi mật khẩu" class="ImageButton" onclick="ChangePassword('QuanTriHeThong','Home','SaveNewPassword', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/DoiMatKhau.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Về trang chủ" title="Quay về trang chủ" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','index', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/TrangChu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
              </table>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
        </table>
</asp:Content>

