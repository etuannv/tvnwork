<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.UserModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                 <div class="TitleNomal">DANH SÁCH THÀNH VIÊN</div>
                 <table width='98%'>
                     <tr>
                        <td height="70px" align="center">
                            Chọn loại thành viên:<%: Html.DropDownList("LoaiNguoiDung", (SelectList)ViewData["DSLoaiNguoiDung"], new { onchange = "Submitform('QuanTriHeThong','Home','UserManager', '', '','', '','');", @style = "width:255px;", @class="Input" })%>
                            <%: Html.Hidden("LoaiNguoiDungCu", ViewData["LoaiNguoiDungCu"])%>
                        </td>
                        <td height="30px" align="right">
                            <img alt="Thêm mới người dùng" title="Thêm mới người dùng" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','AddNewUser', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>

		        <table width='98%' class="Table1">
                    <tr>
                        <th width="5%">
                            STT 
                        </th>
                        <th width="8%">
                            Tên đăng nhập
                        </th>
                        <th width="15%">
                            Họ và tên thành viên
                        </th>
                        <th width="7%">
                            Ngày đăng ký
                        </th>
                        <th width="20%">
                            Địa chỉ liên hệ
                        </th>
                        <% if (ViewData["LoaiNguoiDungCu"].ToString().Trim().ToLower() != "AdminOfSystem") %>
                        <%{ %>
                        <th width="20%">
                            Học sinh trường
                        </th>
                        <%} %>
 
                        <th width="15%">
                            Hộp thư điện tử
                        </th>
                        <th width="10%">
                            Cập nhật
                        </th>
	        	    </tr>
                    <% int m = 0; %>
                <% foreach (TNV.Web.Models.UserModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.UserName %>"><%: Item.UserName %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.FullName %>"><%: Item.FullName %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.CreateDate.ToString("dd/MM/yyyy") %>"><%: Item.CreateDate.ToString("dd/MM/yyyy")%></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.TenHuyen%> - <%: Item.TenTinh%>"><%: Item.TenHuyen%> - <%: Item.TenTinh%></label> 
                            </td>
                            <% if (ViewData["LoaiNguoiDungCu"].ToString().Trim().ToLower() != "AdminOfSystem") %>
                            <%{ %>
                            <td align="left">
                                <label title="<%: Item.SchoolName%>"><%: Item.SchoolName%></label> 
                            </td>
                            <%} %>
                            
                            <td align="left">
                                <label title="<%: Item.Email%>"><%: Item.Email%></label> 
                            </td>
                            <td align="center">
                                <img alt="Sửa thông tin thành viên" title="Sửa thông tin: <%: Item.FullName %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','Home','EditUser', '<%=Item.UserId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa thông tin thành viên" title="Xóa thông tin: <%: Item.FullName %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn xóa người dùng này?','Home','DelUser', '<%=Item.UserId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/><br />
                                <img alt="Mật khẩu mặc định" title="Reset mật khẩu: <%: Item.FullName %> về 123456" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn Reset lại mật khẩu?','Home','RestPass', '<%=Item.UserId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/ResetPass.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <% if (Item.Prevent.ToString().Trim()=="1")%>
                                <%{ %>
                                    <img alt="Khóa tài khoản người dùng" title="Khóa tài khoản người dùng: <%: Item.FullName %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn khóa tài khoản này?','Home','PreventUser', '<%=Item.UserId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/PowerOff.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <%} else
                                {%>
                                    <img alt="Mở khóa tài khoản người dùng" title="Mở khóa tài khoản người dùng: <%: Item.FullName %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn mở khóa tài khoản này?','Home','NotPreventUser', '<%=Item.UserId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/PowerOn.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <%} %>
                            </td>
	        		    </tr>
                <%} %>
                        <tr>
                            <td align="center" colspan="8">
                                <%: Html.Hidden("PageCurent", ViewData["PageCurent"]) %>
                                <% Html.RenderPartial("PagePostNormal", ViewData["Page"]); %>
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

