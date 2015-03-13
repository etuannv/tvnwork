<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.TimeListModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                 <div class="TitleNomal">DANH SÁCH CÁC KHOẢNG THỜI GIAN</div>
                 <table width='98%'>
                     <tr>
                        <td height="30px" align="right">
                            <img alt="Thêm mới khoảng thời gian" title="Thêm mới khoảng thời gian" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','AddTimeList', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
                            Tên khoảng thời gian
                        </th>
                        <th width="30%">
                            Mô tả khoảng thời gian
                        </th>
                        <th width="20%">
                            Thứ tự hiển thị
                        </th>
                        <th width="10%">
                            Cập nhật
                        </th>
	        	    </tr>
                <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                <% foreach (TNV.Web.Models.TimeListModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.TimeListName %>"><%: Item.TimeListName %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.TimeListInfor %>"><%: Item.TimeListInfor %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.TimeListOrder %>"><%: Item.TimeListOrder%></label> 
                            </td>
                            <td align="center">
                                <img alt="Sửa thông tin khoảng thời gian" title="Sửa thông tin: <%: Item.TimeListName %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','EditTimeList', '<%=Item.TimeListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa thông tin khoảng thời gian" title="Xóa thông tin: <%: Item.TimeListName %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn xóa khoảng thời gian này?','SystemManager','DelTimeList', '<%=Item.TimeListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/><br />
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

