<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.RegisterModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
                <div class="TitleNomal">ĐĂNG KÝ THÀNH VIÊN MỚI</div>
                <div class="validation-summary-errors">Ghi chú: Bạn phải nhập đầy đủ, chính xác các mục đanh dấu (<label class="Note">*</label>)</div>
                <%: Html.ValidationSummary(true, "[Thông báo: Không thể đăng ký được thành viên này!]") %>
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
                            <%:Html.LabelFor(m=>m.PassWords) %>(<label class="Note">*</label>)
                        </td>
                        <td height="30px" align="left">
                            <%: Html.PasswordFor(m => m.PassWords, new { @class = "Input", @title = "Bạn hãy nhập \"Mật khẩu đăng nhập\" bao gồm các chữ cái thường hoặc chữ cái hoa, chữ số, và một số ký hiệu đặc biệt <b>! @ $</b>. Mật khẩu đăng nhập được viết liền, không chứa dấu cách, nên chứa đồng thời cả chữa cái, chữa số, ký hiệu đặc biệt. Độ dài tối thiểu của mật khẩu là 6 ký tự. Hai lần nhập mật khẩu phải trùng khớp!" })%><br />
                            <%: Html.ValidationMessageFor(m => m.PassWords)%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m=>m.ConfirmPassWords) %>(<label class="Note">*</label>)
                        </td>
                        <td height="30px" align="left">
                            <%: Html.PasswordFor(m => m.ConfirmPassWords, new { @class = "Input", @title = "Bạn hãy nhập \"Mật khẩu đăng nhập\" bao gồm các chữ cái thường hoặc chữ cái hoa, chữ số, và một số ký hiệu đặc biệt <b>! @ $</b>. Mật khẩu đăng nhập được viết liền, không chứa dấu cách, nên chứa đồng thời cả chữa cái, chữa số, ký hiệu đặc biệt. Độ dài tối thiểu của mật khẩu là 6 ký tự. Hai lần nhập mật khẩu phải trùng khớp!" })%><br />
                            <%: Html.ValidationMessageFor(m => m.ConfirmPassWords)%>
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
                        <td height="30px" align="left">
                            <%:Html.LabelFor(m => m.NgaySinh)%>
                        </td>
                        <td height="30px" align="left">
                            <input type="text" id="NgaySinhTV" class = "Input" title = "Bạn hãy chọn ngày sinh của bạn!" name="NgaySinhTV" value="<%:ViewData["NgaySinh"] %>" readonly="readonly" style="width:230px;" />
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
                            <%:Html.LabelFor(m => m.MaTinh)%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("MaTinh", (SelectList)ViewData["DsTinhTP"], new { @title = "Bạn hãy chọn tỉnh, thành phố mà trường học của bạn đang hoạt động tại đó!", @style = "width:255px;", @class = "Input" })%>
                            <%: Html.Hidden("MaTinhCu",ViewData["MaTinhCu"]) %>
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
                    <tr>
                        <td height="30px" align="left" rowspan="2">
                           Chọn tên trường bạn đang học, nếu chưa có thì nhập tên trường ở ô phía dưới
                        </td>
                        <td height="30px" align="left">
                            <%: Html.DropDownList("SchoolId", (SelectList)ViewData["DSTruongHoc"], new {@title = "Bạn hãy chọn trường học mà bạn đang theo học, nếu tên trường đó chưa có để bạn lựa chọn thì bạn có thể bộ sung thêm ở ô dưới đây!", @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            <%: Html.TextBox("SchoolName", ViewData["SchoolName"], new { @title = "Trong trường hợp Tên trường học của bạn chưa có trong danh sách để cho bạn lựa chọn, bạn có thể nhập bổ sung Tên trường học của bạn vào ô bên trên, ví dụ: Trường tiểu học Phạm Công Bình, Trường THCS Quang Trung,....", @style = "width:255px;", @class = "Input" })%>
                        </td>
                    </tr>
                    <tr>
                        <td height="30px" align="left">
                            Nhập mã bảo vệ:(<label class="Note">*</label>)&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<label class="Security"><%:ViewData["Security"]%></label>
                            <%:Html.Hidden("Security", ViewData["Security"])%>
                        </td>
                        <td height="30px" align="left">
                            <%: Html.TextBox("SecurityValue", ViewData["SecurityValue"], new { @title = "Bạn hãy nhập chính xác mã bảo vệ!", @class = "Input" })%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <div class="validation-summary-errors">QUY ĐỊNH CỦA TOÁN CHUYÊN TRANG TOÁN THÔNG MINH</div>
                            <div class="NoiQuy">
                                <p>1. Tất cả học sinh cấp tiểu học, đều có thể đăng ký thành viên để tham gia chuyên trang Toán thông minh qua địa chỉ http://toanthongminh.vn</p>
                                <p>2. Chỉ những học sinh đăng ký với các thông tin chính xác : Họ và tên, ngày sinh,lớp, trường, huyện (quận, thị xã, thành phố thuộc tỉnh), tỉnh (thành phố trực thuộc trung ương) được Ban tổ chức cấp trường xác nhận mới được trao các giải thưởng của chuyên trang hoặc tham gia các cuộc thi.</p>
                                <p>3. Các thành viên cần tự rèn luyện thông qua cuộc thi và không được thiếu trung thực khi thi.</p>
                                <p>4. Các thành viên chấp hành Thể lệ của cuộc thi và mọi thông báo từ Ban tổ chức trang web.</p>
                                <p>5. Các thành viên tham gia thi trên chuyên trang này phải có kế hoạch học tập và tu dưỡng toàn diện, không để việc tham gia thi ảnh hưởng tới mọi mặt hoạt động khác trong nhà trường.</p>
                                <p>6. Thành viên quên Tên đăng nhập hoặc Mật khẩu thì phải đăng kí thành viên lại và thi lại tất cả các vòng thi hiện có. Ban tổ chức không hỗ trợ việc tìm lại Tên truy cập hay Mật khẩu.</p>
                                <p>7. Trong quá trình chạy phiên bản thử nghiệm, trang web có thể xóa các thông tin đăng ký nếu cần thiết. Nếu đã đăng ký theo gói học tập có phí thì phụ huynh vui lòng liên hệ với ban tổ chức toanthongminh.vn để được trợ giúp.</p>
                             </div>
                        </td>
                    </tr>
                     <tr>
                        <td height="30px" colspan="2"  align="left">
                             (<label class="Note">*</label>)<label for="DongY">Tôi đồng ý với quy định của toanthongminh.vn</label>
                             <input type="checkbox" <%:ViewData["Checked"] %> value="true" name="DongY" id="DongY" />
                        </td>
                    </tr>
                    <tr>
                        <td height="50px" align="right">
                            <img alt="Đăng ký" class="ImageButton" onclick="SaveRegistry('QuanTriHeThong','Home','SaveRegistry', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/GuiHoSo.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                        <td height="50px" align="left">
                            <img alt="Đăng nhập" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','index', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/TrangChu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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

