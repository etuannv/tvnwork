<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.RegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
                <div id="FooterTitle">SỬA THÔNG TIN MỘT NGƯỜI DÙNG</div>
                <div class="validation-summary-errors">Ghi chú: Bạn phải nhập đầy đủ, chính xác các mục đanh dấu (<label class="Note">*</label>)</div>
                <%: Html.ValidationSummary(true, "[Thông báo: Không thể sửa thông tin được người dùng này!]") %>
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
                <table width="87%">
                     <tr>
                        <td height="30px" width="50%" align="left">
                            <%:Html.LabelFor(m=>m.UserNames) %>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.UserNames, new { @class = "Input", @readonly="true", @title = "Bạn hãy nhập \"Tên đăng nhập\" gồm các chữ cái thường, hoa, các chữ số. Tên đăng nhập được viết liền, không chứa dấu cách, là tên tiếng Anh hoặc tiếng Việt không có dấu. ví dụ: BaoChau, Bao_Chau, BaoChau2004...!" })%><br />
                            <%: Html.ValidationMessageFor(m => m.UserNames)%>
                            <%: Html.Hidden("PassWords","123456") %>
                            <%: Html.Hidden("ConfirmPassWords", "123456")%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <div class="Explanation">(<b>Tên đăng nhập</b> được viết từ các chữ cái thường, hoa, các chữ số. 
                            Tên đăng nhập được viết liền, không chứa dấu cách, là tên tiếng Anh hoặc tiếng Việt không có dấu. 
                            ví dụ: <b>BaoChau, Bao_Chau, BaoChau2004...</b>)</div>
                        </td>
                    </tr>
                     <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m=>m.FullNames) %>(<label class="Note">*</label>)
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.FullNames, new { @class = "Input", @title = "\"Họ và tên thành viên\" là họ và tên đầy đủ của thành viên, ví dụ: Ngô Bảo Châu, Tô Bảo Châu.... Khi bạn đăng nhập vào hệ thống thì tên này sẽ được hiển thị."})%><br />
                            <%: Html.ValidationMessageFor(m => m.FullNames)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <div class="Explanation">(<b>Họ và tên thành viên</b> là họ và tên đầy đủ của thành viên, 
                            ví dụ: <b>Ngô Bảo Châu, Tô Bảo Châu...</b>. Khi bạn đăng nhập vào hệ thống thì tên này sẽ được hiển thị.)</div>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.NgaySinh)%>
                        </td>
                        <td height="30px" align="left">
                            <input type="text" id="NgaySinhTV" , title = "Bạn hãy chọn ngày sinh của bạn!", name="NgaySinhTV" value="<%:ViewData["NgaySinh"] %>" readonly="readonly" style="width:230px;" />
                            <script type="text/javascript">
                                $(document).ready(function () { $('#NgaySinhTV').datepicker({ showOn: 'button', buttonImage: '/Content/image/calendar.gif', duration: 0 }); });
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m=>m.Email) %>(<label class="Note">*</label>)
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.Email, new { @title = "Bạn phải nhập chính xác địa chỉ Email của bạn, địa chỉ email này có tác dụng khi bạn mất tài khỏa đăng nhập, hệ thống có thể khởi tạo lại tài khoản cho bạn và gửi thông tin tài khoản về địa chỉ Email này", @class = "Input" })%><br />
                            <%: Html.ValidationMessageFor(m => m.Email)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <div class="Explanation">(Bạn phải nhập chính xác địa chỉ Email của bạn, địa chỉ email này có tác dụng khi bạn mất tài khỏa đăng nhập, hệ thống có thể khởi tạo lại tài khoản cho bạn và gửi thông tin tài khoản về địa chỉ Email này)</div>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MobileAlias)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.MobileAlias, new { @title = "Bạn nên nhập chính xác số điện thoại liên hệ của mình, Trong trường hợp nào đó quản trị hệ thống có thể liên hệ với bạn!", @class = "Input" })%><br />
                            <%: Html.ValidationMessageFor(m => m.MobileAlias)%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="left">
                            <div class="Explanation">(Bạn nên nhập chính xác số điện thoại liên hệ của mình, Trong trường hợp nào đó quản trị hệ thống có thể liên hệ với bạn)</div>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                             Chọn loại người dùng:
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("LoaiNguoiDung", (SelectList)ViewData["DSLoaiNguoiDung"], new { onchange = "Submitform('QuanTriHeThong','Home','EditUser', '', '','', '','');", @style = "width:255px;" })%>
                            <%: Html.Hidden("LoaiNguoiDungCu",ViewData["LoaiNguoiDungCu"]) %>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MaTinh)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("MaTinh", (SelectList)ViewData["DsTinhTP"], new { onchange = "Submitform('QuanTriHeThong','Home','EditUser', '', '','', '','');", @title = "Bạn hãy chọn tỉnh, thành phố mà trường học của bạn đang hoạt động tại đó!", @style = "width:255px;" })%>
                            <%: Html.Hidden("MaTinhCu", ViewData["MaTinhCu"])%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MaHuyen)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("MaHuyen", (SelectList)ViewData["DSHuyen"], new { onchange = "Submitform('QuanTriHeThong','Home','EditUser', '', '','', '','');", @title = "Bạn hãy chọn huyện, thành thị mà trường học của bạn đang hoạt động tại đó!", @style = "width:255px;" })%>
                        </td>
                    </tr>
                    <% if (ViewData["LoaiNguoiDung"].ToString().Trim().ToLower() != "AdminOfSystem") %>
                    <%{ %>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.SchoolId)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("SchoolId", (SelectList)ViewData["DSTruongHoc"], new { @title = "Bạn hãy chọn trường học mà bạn đang theo học, nếu tên trường đó chưa có để bạn lựa chọn thì bạn có thể bộ sung thêm ở ô dưới đây!", @style = "width:255px;" })%>
                        </td>
                    </tr>
                    <%} %>
                    
                     <% if (ViewData["LoaiNguoiDung"].ToString().Trim().ToLower() != "AdminOfSystem" && ViewData["LoaiNguoiDung"].ToString().Trim().ToLower() != "normaluser") %>
                    <%{ %>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.StartDate)%>
                        </td>
                        <td height="30px" align="left">
                            <input type="text" id="StartDateMoney" , title = "Bạn hãy chọn ngày bắt đầu tính phí!", name="StartDateMoney" value="<%:ViewData["StartDateMoney"] %>" readonly="readonly" style="width:230px;" />
                            <script type="text/javascript">
                                $(document).ready(function () { $('#StartDateMoney').datepicker({ showOn: 'button', buttonImage: '/Content/image/calendar.gif', duration: 0 }); });
                            </script>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.ExpiredDate)%>
                        </td>
                        <td height="30px" align="left">
                            <input type="text" id="ExpiredDateMoney" , title = "Bạn hãy chọn ngày hết hạn tính phí!", name="ExpiredDateMoney" value="<%:ViewData["ExpiredDateMoney"] %>" readonly="readonly" style="width:230px;" />
                            <script type="text/javascript">
                                $(document).ready(function () { $('#ExpiredDateMoney').datepicker({ showOn: 'button', buttonImage: '/Content/image/calendar.gif', duration: 0 }); });
                            </script>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td height="50px" align="right">
                            <img alt="Lưu thành viên" title="Lưu sửa người dùng" class="ImageButton" onclick="SaveEditUser('QuanTriHeThong','Home','SaveEditUser', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/GuiHoSo.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                        <td height="50px" align="left">
                            <img alt="Quay lại" title="Quay về danh sách" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','UserManager', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
    </table>
</asp:Content>

