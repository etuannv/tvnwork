<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FontEnd.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.PhepToanHaiSoHangModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("FCacDangToanLopMot"); %>
            </td>
            <td width="71%" valign="top">
                <h4>Luyện phép toán 2 số hạng</h4>
                <table>
                    <thead><tr><td>Điền dấu thích hợp vào ô trống</td></tr></thead>
                    <tbody id="question-content"></tbody>
                </table>
                <%= Ajax.ActionLink("Gửi kết quả", "GetNextQuestion", new { memvar1 = "CongPhamVi10", memvar2 = "CLS1847290691" }, new AjaxOptions { UpdateTargetId = "question-content" })%>
                <%= Ajax.ActionLink("Bài khác", "GetNextQuestion", new { memvar1 = "CongPhamVi10", memvar2 = "CLS1847290691" }, new AjaxOptions { UpdateTargetId = "question-content" })%>
                
            </td>

        </tr>
    </table>
</asp:Content>
