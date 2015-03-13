<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.RegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
                <div class="TitleNomal">THÊM MỚI MỘT NGƯỜI DÙNG</div>
                <div class="validation-summary-errors">Ghi chú: Bạn phải nhập đầy đủ, chính xác các mục đanh dấu (<label class="Note">*</label>)</div>
                <%: Html.ValidationSummary(true, "[Thông báo: Không thể thêm mới được người dùng này!]") %>
                <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
                <script type="text/javascript">
                    $(document).ready(function () {
                        $("#MaTinh").change(function () {
                            $.ajaxSetup({ cache: false });
                            var DonViHCTinhHienTai = $("#MaTinh > option:selected").attr("value");
                            $.ajax({
                                url: '/ProDis/DanhSachCapHuyen/' + DonViHCTinhHienTai,
                                type: 'POST',
                                dataType: 'JSON',
                                data: JSON,
                                contentType: 'application/json; charset=utf-8',
                                success: function (DanhSachCapHuyen) {
                                    var DanhSachHuyen = "";
                                    $.each(eval(DanhSachCapHuyen), function (j, DonViHuyen) {
                                        if (j == 0) {
                                            DanhSachHuyen += "<option selected='selected' value='" + DonViHuyen.MaHuyenThi + "'>" + DonViHuyen.TenHuyenThi + "</option>";
                                            $.ajax({
                                                url: '/ProDis/DanhSachCacTruong/' + DonViHuyen.MaHuyenThi,
                                                type: 'POST',
                                                dataType: 'JSON',
                                                data: JSON,
                                                contentType: 'application/json; charset=utf-8',
                                                success: function (DanhSachTruong) {
                                                    var DSTruong = "";
                                                    $.each(eval(DanhSachTruong), function (i, DSCacTruong) {
                                                        if (i == 0) {
                                                            DSTruong += "<option selected='selected' value='" + DSCacTruong.SchoolId + "'>" + DSCacTruong.SchoolName + "</option>";
                                                        }
                                                        else {
                                                            DSTruong += "<option value='" + DSCacTruong.SchoolId + "'>" + DSCacTruong.SchoolName + "</option>";
                                                        }
                                                    });
                                                    $("#SchoolId").html(DSTruong);
                                                }
                                            });
                                        }
                                        else {
                                            DanhSachHuyen += "<option value='" + DonViHuyen.MaHuyenThi + "'>" + DonViHuyen.TenHuyenThi + "</option>";
                                        }
                                    });
                                    $("#MaHuyen").html(DanhSachHuyen);
                                }
                            });
                        });

                        $("#MaHuyen").change(function () {
                            var DonViHCHuyenHienTai = $("#MaHuyen > option:selected").attr("value");
                            $.ajax({
                                url: '/ProDis/DanhSachCacTruong/' + DonViHCHuyenHienTai,
                                type: 'POST',
                                dataType: 'JSON',
                                data: JSON,
                                contentType: 'application/json; charset=utf-8',
                                success: function (DanhSachTruong) {
                                    var DSTruong = "";
                                    $.each(eval(DanhSachTruong), function (i, DSCacTruong) {
                                        if (i == 0) {
                                            DSTruong += "<option selected='selected' value='" + DSCacTruong.SchoolId + "'>" + DSCacTruong.SchoolName + "</option>";
                                        }
                                        else {
                                            DSTruong += "<option value='" + DSCacTruong.SchoolId + "'>" + DSCacTruong.SchoolName + "</option>";
                                        }
                                    });
                                    $("#SchoolId").html(DSTruong);
                                }
                            });
                        });
                    });

                </script>
                <table width="87%">
                     <tr>
                        <td height="30px" width="50%" align="left">
                            <%:Html.LabelFor(m=>m.UserNames) %>(<label class="Note">*</label>)
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.UserNames, new { @class = "Input", @title = "Bạn hãy nhập \"Tên đăng nhập\" gồm các chữ cái thường, hoa, các chữ số. Tên đăng nhập được viết liền, không chứa dấu cách, là tên tiếng Anh hoặc tiếng Việt không có dấu. ví dụ: BaoChau, Bao_Chau, BaoChau2004...!" })%><br />
                            <%: Html.ValidationMessageFor(m => m.UserNames)%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            Mật khẩu đăng nhập
                        </td>
                        <td height="30px" align="left">
                           <b>123456</b>
                        </td>
                    </tr>
                     <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m=>m.FullNames) %>(<label class="Note">*</label>)
                            <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.FullNames, new { @class = "Input", @title = "\"Họ và tên thành viên\" là họ và tên đầy đủ của thành viên, ví dụ: Ngô Bảo Châu, Tô Bảo Châu.... Khi bạn đăng nhập vào hệ thống thì tên này sẽ được hiển thị."})%><br />
                            <%: Html.ValidationMessageFor(m => m.FullNames)%>
                        </td>
                    </tr>
                    
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.NgaySinh)%>
                        </td>
                        <td height="30px" align="left">
                            <input type="text" id="NgaySinhTV" title = "Bạn hãy chọn ngày sinh của bạn!" name="NgaySinhTV" value="<%:ViewData["NgaySinh"] %>" readonly="readonly" style="width:230px;" class = "Input"/>
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
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MobileAlias)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBoxFor(m => m.MobileAlias, new { @title = "Bạn nên nhập chính xác số điện thoại liên hệ của mình, Trong trường hợp nào đó quản trị hệ thống có thể liên hệ với bạn!", @class = "Input" })%><br />
                            <%: Html.ValidationMessageFor(m => m.MobileAlias)%>
                        </td>
                    </tr>
                    
                    <tr>
                        <td height="30px" align="left">
                             Chọn loại người dùng:
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("LoaiNguoiDung", (SelectList)ViewData["DSLoaiNguoiDung"], new { @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MaTinh)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("MaTinh", (SelectList)ViewData["DsTinhTP"], new { @title = "Bạn hãy chọn tỉnh, thành phố mà trường học của bạn đang hoạt động tại đó!", @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.MaHuyen)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("MaHuyen", (SelectList)ViewData["DSHuyen"], new { @title = "Bạn hãy chọn huyện, thành thị mà trường học của bạn đang hoạt động tại đó!", @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <% if (ViewData["LoaiNguoiDung"].ToString().Trim().ToLower() != "AdminOfSystem") %>
                    <%{ %>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.SchoolId)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("SchoolId", (SelectList)ViewData["DSTruongHoc"], new { @title = "Bạn hãy chọn trường học mà bạn đang theo học, nếu tên trường đó chưa có để bạn lựa chọn thì bạn có thể bộ sung thêm ở ô dưới đây!", @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <%} %>
                    <tr>
                        <td height="50px" align="right">
                            <img alt="Lưu thành viên" title="Lưu mới người dùng" class="ImageButton" onclick="SaveNewUser('QuanTriHeThong','Home','SaveNewUser', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/GuiHoSo.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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

