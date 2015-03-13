<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.TimeListModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">SỬA THÔNG TIN MỘT KHOẢNG THỜI GIAN</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thể sửa được khoảng thời gian này!]") %>
        <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
        <asp:Table ID="Table1" runat="server" Width="70%">
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.TimeListId) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.TimeListId, new { @ReadOnly = "true", @Style = "width:250px" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.TimeListName) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.TimeListName, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.TimeListName)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.TimeListInfor)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextAreaFor(m => m.TimeListInfor, new { @Style = "width:250px; Height:50px;" })%><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.TimeListOrder)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.TimeListOrder, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.TimeListOrder)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell align="center" ColumnSpan=2>
                        <img alt="Lưu sửa một khoảng thời gian" title="Lưu sửa một khoảng thời gian" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','SaveEditTimeList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        <img alt="Quay lại danh sách khoảng thời gian" title="Quay lại danh sách khoảng thời gian" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','TimeList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
