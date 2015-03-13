<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.LogOnModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                <br /><br /><br /><br />
               <div class="ThongBao">
                    <%: MvcHtmlString.Create(ViewData["ThongBao"].ToString().Trim()) %><br /><br />
                    <img alt="Về trang chủ" title="Quay về trang chủ" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','index', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/TrangChu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
               </div>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
        </table>
</asp:Content>

