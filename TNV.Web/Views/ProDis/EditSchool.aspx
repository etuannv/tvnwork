<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.SchoolModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">SỬA THÔNG TIN MỘT TRƯỜNG HỌC</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thể sửa thông tin trường học này!]") %>
        <asp:Table ID="Table1" runat="server" Width="70%">
            <asp:TableRow Height="30px">
                <asp:TableCell height="30px">
                    Chọn tỉnh - thành phố:
                </asp:TableCell>
                <asp:TableCell height="30px">
                    <%: Html.DropDownList("MaTinhTP", (SelectList)ViewData["DsTinhTP"], new { onchange = "Submitform('QuanTriHeThong','ProDis','EditSchool', '', '','', '','');", @style = "width:255px;" })%>
                    <%: Html.Hidden("MaTinhCu",ViewData["MaTinhTP"]) %>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell height="30px">
                    Chọn huyện - thành - thị xã:
                </asp:TableCell>
                <asp:TableCell height="30px">
                    <%: Html.DropDownList("MaHuyenThi", (SelectList)ViewData["DsHuyenThiXa"], new { @style = "width:255px;" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.SchoolId) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.SchoolId, new { @ReadOnly = "true", @Style = "width:250px" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.SchoolName) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.SchoolName, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.SchoolName)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.SchoolOrder)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.SchoolOrder, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.SchoolOrder)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell align="center" ColumnSpan=2>
                        <img class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','SaveEditSchool', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        <img class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','SchoolList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </asp:TableCell>
                </asp:TableRow>
            </asp:Table>
        </td>
        <td width="29%" valign="top" align="center">
            <%--Thêm nội dung cột bên phải--%>
        </td>
    </tr>
</table>
</asp:Content>
