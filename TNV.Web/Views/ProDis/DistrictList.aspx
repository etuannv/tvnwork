<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.HuyenThiXaModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">DANH SÁCH HUYỆN - THÀNH PHỐ - THỊ XÃ</div>
         <table width='98%'>
             <tr>
                <td height="70px" align="center">
                    Chọn tỉnh - thành phố:<%: Html.DropDownList("MaTinhTP", (SelectList)ViewData["DsTinhTP"], new { onchange = "Submitform('QuanTriHeThong','ProDis','DistrictList', '', '','', '','');", @style = "width:255px;" })%>
                </td>
                <td height="30px" align="right">
                    <img alt="Thêm mới" title="Thêm mới thông tin: huyện - thành phố - thị xã" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','AddDistrict', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </td>
            </tr>
        </table>
		<table width='98%' class="Table1">
                <tr>
                    <th width="10%">
                        STT 
                    </th>
                    <th width="20%">
                        Mã huyện, thị xã
                    </th>
                    <th width="60%">
                        Tên huyện, thành phố, thị xã
                    </th>
                    <th width="10%">
                        Cập nhật
                    </th>
	        	</tr>
                <% int m = 0; %>
                <% foreach (TNV.Web.Models.HuyenThiXaModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td width="10%" align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td width="20%" align="left">
                                <label title="<%: Item.MaHuyenThi %>"><%: Item.MaHuyenThi %></label> 
                            </td>
                            <td width="60%" align="left">
                                <label title="<%: Item.TenHuyenThi %>"><%: Item.TenHuyenThi %></label> 
                            </td>
                            <td width="10%" align="center">
                                <img title="Sửa thông tin: <%: Item.TenHuyenThi %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','EditDistrict', '<%=Item.MaHuyenThi %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img title="Xóa thông tin: <%: Item.TenHuyenThi %>" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','ProDis','DelDistrict', '<%=Item.MaHuyenThi %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
	        		    </tr>
                <%} %>
            </table>
           
        </td>
        <td width="29%" valign="top" align="center">
            <%--Thêm nội dung cột bên phải--%>
        </td>
    </tr>
</table>
</asp:Content>

