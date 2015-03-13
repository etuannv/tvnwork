<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.ClassListModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">THÊM MỚI MỘT KHỐI LỚP</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thêm mới được khối lớp này!]") %>
        <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
        <asp:Table ID="Table1" runat="server" Width="70%">
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.ClassListId) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.ClassListId, new { @ReadOnly = "true", @Style = "width:250px" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.ClassListName) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.ClassListName, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.ClassListName)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.ClassListInfor)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextAreaFor(m => m.ClassListInfor, new { @Style = "width:250px; Height:50px;" })%><br />
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.ClassListOrder)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.ClassListOrder, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.ClassListOrder)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell align="center" ColumnSpan=2>
                        <img alt="Lưu mới một khối lớp" title="Lưu mới một khối lớp" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','SaveAddClassList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        <img alt="Quay lại danh sách khối lớp" title="Quay lại danh sách khối lớp" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','ClassList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
