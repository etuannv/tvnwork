<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.UserModel>" %>

  <div class="NewsGroup">
        <div class="title">
            Đăng ký thành viên
        </div>
        <div class="content">
            <table width="96%" border="0px"; >
                <tr>
                    <td>
                        <%:Html.LabelFor(m=>m.FullName) %>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.FullName, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m=>m.UserName) %>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.UserName, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m=>m.PassWord) %>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.PassWord, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m=>m.ConfirmPassWord) %>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.ConfirmPassWord, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m=>m.Email) %>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.Email, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m => m.MobileAlias)%>
                    </td>
                    <td>
                        <%: Html.TextBoxFor(m => m.MobileAlias, new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m => m.RoleId)%>
                    </td>
                    <td>
                        <%: Html.DropDownList("RoleId", (SelectList)ViewData["RoleList"], new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m => m.MaTinh)%>
                    </td>
                    <td>
                        <%: Html.DropDownList("MaTinh", (SelectList)ViewData["DsTinhTP"], new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m => m.MaHuyen)%>
                    </td>
                    <td>
                        <%: Html.DropDownList("MaHuyen", (SelectList)ViewData["DSHuyen"], new { @class = "Input" })%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%:Html.LabelFor(m => m.NgaySinh)%>
                    </td>
                    <td>
                        <input type="text" id="NgaySinh" name="NgaySinh" value="<%:ViewData["NgaySinh"] %>" readonly="readonly" class="Input" />
                        <script type="text/javascript">
                            $(document).ready(function () { $('#NgaySinh').datepicker({ showOn: 'button', buttonImage: '/Content/image/calendar.gif', duration: 0 }); });
                        </script>
                    </td>
                </tr>
            </table>
        </div>
  </div>


