<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.SchoolModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">DANH SÁCH CÁC TRƯỜNG HỌC</div>
         <table width='98%'>
             <tr>
                <td height="30px" align="left">
                    Chọn tỉnh - thành phố:
                </td>
                <td height="30px" align="left">
                    <%: Html.DropDownList("MaTinhTP", (SelectList)ViewData["DsTinhTP"], new { onchange = "Submitform('QuanTriHeThong','ProDis','SchoolList', '', '','', '','');", @style = "width:255px;" })%>
                    <%: Html.Hidden("MaTinhCu",ViewData["MaTinhTP"]) %>
                </td>
                <td height="30px" align="right" rowspan="2" valign="bottom">
                    <img alt="Thêm mới" title="Thêm mới thông tin: Trường học" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','AddSchool', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </td>
            </tr>
            <tr>
                <td height="30px" align="left">
                    Chọn huyện - thành - thị xã:
                </td>
                <td height="30px" align="left">
                    <%: Html.DropDownList("MaHuyenThi", (SelectList)ViewData["DsHuyenThiXa"], new { onchange = "Submitform('QuanTriHeThong','ProDis','SchoolList', '', '','', '','');", @style = "width:255px;" })%>
                </td>
            </tr>
        </table>
		<table width='98%' class="Table1">
                <tr>
                    <th width="10%">
                        STT 
                    </th>
                    <th width="60%">
                        Tên trường học
                    </th>
                    <th width="20%">
                        Thứ tự sắp xếp
                    </th>
                    <th width="10%">
                        Cập nhật
                    </th>
	        	</tr>
                <% int m = 0; %>
                <% foreach (TNV.Web.Models.SchoolModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td width="10%" align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td width="60%" align="left">
                                <label title="<%: Item.SchoolName %>"><%: Item.SchoolName %></label> 
                            </td>
                            <td width="20%" align="left">
                                <label title="<%: Item.SchoolOrder %>"><%: Item.SchoolOrder %></label> 
                            </td>
                            <td width="10%" align="center">
                                <img title="Sửa thông tin: <%: Item.SchoolName %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','EditSchool', '<%=Item.SchoolId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img title="Xóa thông tin: <%: Item.SchoolName %>" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','ProDis','DelSchool', '<%=Item.SchoolId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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

