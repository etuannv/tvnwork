<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.LogOnModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            
            <td width="71%" valign="top">
               <% Html.RenderPartial("Profile", ViewData["Profile"]); %>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
        </table>
</asp:Content>

