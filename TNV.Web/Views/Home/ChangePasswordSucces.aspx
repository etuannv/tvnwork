<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
               <br />
               <br />
               <div class="validation-summary-errors">Thông báo: Bạn đã thay đổi mật khẩu thành công, hãy nhớ mật khẩu mới!</div>
               <table width="80%">
                    <tr>
                        <td height="30px" align="center" colspan="2">
                            <img alt="Trang chủ" title"Về trang chủ" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','index', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/TrangChu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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

