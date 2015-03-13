<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.MathKindListModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">SỬA THÔNG TIN MỘT DẠNG TOÁN</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thể sửa được dạng toán này!]") %>
        <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
        <asp:Table ID="Table1" runat="server" Width="70%">
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.MathKindListId) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.MathKindListId, new { @ReadOnly = "true", @Style = "width:250px" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.MathKindListName) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.MathKindListName, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.MathKindListName)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.MathKindListInfor)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextAreaFor(m => m.MathKindListInfor, new { @Style = "width:250px; Height:50px;" })%><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.MathKindListOrder)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.MathKindListOrder, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.MathKindListOrder)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell align="center" ColumnSpan=2>
                        <img alt="Lưu sửa một dạng toán" title="Lưu sửa một dạng toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','SaveEditMathKindList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        <img alt="Quay lại danh sách dạng toán" title="Quay lại danh sách dạng toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','MathKindList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
        <td width="29%" valign="top" align="center">
            <%--Thêm nội dung cột bên phải--%>
            <%: Html.Hidden("PageCurent", ViewData["PageCurent"]) %>
        </td>
    </tr>
</table>
</asp:Content>
