<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.ClassListModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                 <div class="TitleNomal">DANH SÁCH CÁC KHỐI LỚP</div>
                 <table width='98%'>
                     <tr>
                        <td height="30px" align="right">
                            <img alt="Thêm mới khối lớp" title="Thêm mới khối lớp" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','AddClassList', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>
                <div class="validation-summary-errors"><%: ViewData["Mes"] %></div>
		        <table width='98%' class="Table1">
                    <tr>
                        <th width="10%">
                            STT 
                        </th>
                        <th width="30%">
                            Tên khối lớp
                        </th>
                        <th width="30%">
                            Mô tả khối lớp
                        </th>
                        <th width="20%">
                            Thứ tự hiển thị
                        </th>
                        <th width="10%">
                            Cập nhật
                        </th>
	        	    </tr>
                <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                <% foreach (TNV.Web.Models.ClassListModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.ClassListName %>"><%: Item.ClassListName %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.ClassListInfor %>"><%: Item.ClassListInfor %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.ClassListOrder %>"><%: Item.ClassListOrder%></label> 
                            </td>
                            <td align="center">
                                <img alt="Sửa thông tin khối lớp" title="Sửa thông tin: <%: Item.ClassListId %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','EditClassList', '<%=Item.ClassListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa thông tin khối lớp" title="Xóa thông tin: <%: Item.ClassListId %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn xóa khối lớp này?','SystemManager','DelClassList', '<%=Item.ClassListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/><br />
                            </td>
	        		    </tr>
                <%} %>
                        <tr>
                            <td align="center" colspan="8">
                                <% Html.RenderPartial("PagePostNormal", ViewData["Page"]); %>
                                <%: Html.Hidden("PageCurent", ViewData["PageCurent"]) %>
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

