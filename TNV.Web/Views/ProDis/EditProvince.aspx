<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.TinhThanhPhoModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">SỬA THÔNG TIN TỈNH, THÀNH PHỐ</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thể lưu sửa thông tin tỉnh, thành phố này!]") %>
        <asp:Table ID="Table1" runat="server" Width="70%">
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.MaTinhTP) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.MaTinhTP, new { @ReadOnly = "true", @Style = "width:250px" })%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.TenTinhTP) %>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.TenTinhTP, new { @Style = "width:250px" })%><br />
                    <%: Html.ValidationMessageFor(m => m.TenTinhTP)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell>
                    <%: Html.LabelFor(m => m.ThuTuSapXep)%>
                </asp:TableCell>
                <asp:TableCell>
                    <%: Html.TextBoxFor(m => m.ThuTuSapXep, new { @Style="width:250px"})%><br />
                    <%: Html.ValidationMessageFor(m => m.ThuTuSapXep)%>
                </asp:TableCell>
            </asp:TableRow>
            <asp:TableRow Height="30px">
                <asp:TableCell align="center" ColumnSpan=2>
                        <img class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','SaveEditProDis', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        <img class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','ProvinceList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
