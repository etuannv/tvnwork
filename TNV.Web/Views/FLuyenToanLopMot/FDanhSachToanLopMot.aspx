<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("FCacDangToanLopMot"); %>
            </td>
            <td width="71%" valign="top">
                
            </td>
        </tr>
    </table>

</asp:Content>
