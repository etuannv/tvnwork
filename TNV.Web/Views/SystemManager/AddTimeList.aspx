<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.TimeListModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">THÊM MỚI MỘT KHOẢNG THỜI GIAN</div>
        <%: Html.ValidationSummary(true, "[Thông báo: Không thêm mới được khoảng thời gian này!]") %>
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
                        <img alt="Lưu mới một khoảng thời gian" title="Lưu mới một khoảng thời gian" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','SaveAddTimeList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
<table class="MathHelp"><tr><td><div class="HelpTitle"> Hướng dẫn:</div> <p class="HelpContent">Tìm các số thích hợp còn thiếu: <b>... ... 2 ... 4 ... ... ... 8 ... 10</b> </p> <p class="HelpContent"> Các số trên có quy luật: <b>Từ số đứng vị trí thứ 2 trở đi, số đứng sau bằng số ngay trước nó cộng với số 1 </b>. <br/> Tức là:<br/>- Số thứ 1 là: <b>0</b><br/>- Số thứ 2 là: <b>1 = 0 + 1</b><br/>- Số thứ 3 là: <b>2 = 1 + 1</b><br/>- Số thứ 4 là: <b>3 = 2 + 1</b><br/>- Số thứ 5 là: <b>4 = 3 + 1</b><br/>- Số thứ 6 là: <b>5 = 4 + 1</b><br/>- Số thứ 7 là: <b>6 = 5 + 1</b><br/>- Số thứ 8 là: <b>7 = 6 + 1</b><br/>- Số thứ 9 là: <b>8 = 7 + 1</b><br/>- Số thứ 10 là: <b>9 = 8 + 1</b><br/>- Số thứ 11 là: <b>10 = 9 + 1</b> </p><div class="HelpTitle">Kết quả:</div> <p class="HelpReturn"> - Kết quả các số phải tìm là: <b>0 1 3 5 6 7 9</b> </p> <p class="AllHelpReturn"> - Kết quả các số đầy đủ là: <b>0 1 2 3 4 5 6 7 8 9 10</b></p></td></tr></table>
</asp:Content>
