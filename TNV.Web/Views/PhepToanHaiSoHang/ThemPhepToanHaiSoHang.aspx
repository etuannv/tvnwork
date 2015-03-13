<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.PhepToanHaiSoHangModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <%: Html.Hidden("PhamViPhepToan", ViewData["PhamViPhepToan"])%>
                
                <table class="TableInput" align="center" width="70%" border="0px">
                    <tr height="30px">
                        <td align="left"  valign="middle" colspan="2">
                            <div class="TitleNomal">THÊM MỚI PHÉP TOÁN HAI SỐ HẠNG</div>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" width="40%"  valign="middle">
                            Chọn khối lớp:
                        </td>
                        <td align="left" valign="middle" width="60%" >
                            <%: Html.DropDownList("ThuocKhoiLop", (SelectList)ViewData["DSKhoiLop"], new { @class = "Input", @Style = "width:250px" })%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.SoHangThuNhat) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.SoHangThuNhat, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.SoHangThuNhat)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.PhepToan) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.PhepToan, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.PhepToan)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.SoHangThuHai) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.SoHangThuHai, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.SoHangThuHai)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.DauQuanHe) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.DauQuanHe, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.DauQuanHe)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.KetQuaPhepToan) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.KetQuaPhepToan, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.KetQuaPhepToan)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="left" valign="middle">
                            <%: Html.LabelFor(m => m.DapAn) %>
                        </td>
                        <td align="left" valign="middle">
                            <%: Html.TextBoxFor(m => m.DapAn, new { @class = "Input", @Style = "width:250px" })%><br />
                            <%: Html.ValidationMessageFor(m => m.DapAn)%>
                        </td>
                    </tr>
                    <tr height="30px">
                        <td align="center" valign="middle" colspan="2">
                            <img alt="Lưu mới phép toán" title="Lưu mới phép toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','PhepToanHaiSoHang','LuuMoiPhepToanHaiSoHang', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Quay về danh sách"  title="Quay về danh sách" class="ImageButton" onclick="Submitform('QuanTriHeThong','PhepToanHaiSoHang','PhepToanHaiSoHang','<%:ViewData["PhamViPhepToan"]%>', '<%:ViewData["ThuocKhoiLop"] %>', '','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
 </asp:Content>

