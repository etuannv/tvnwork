<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.MathKindListModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                 <div class="TitleNomal">DANH SÁCH CÁC DẠNG TOÁN</div>
                 <table width='98%'>
                     <tr>
                        <td height="30px" align="right">
                            <img alt="Thêm mới dạng toán" title="Thêm mới dạng toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','AddMathKindList', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
                            Tên dạng toán
                        </th>
                        <th width="30%">
                            Mô tả dạng toán
                        </th>
                        <th width="20%">
                            Thứ tự hiển thị
                        </th>
                        <th width="10%">
                            Cập nhật
                        </th>
	        	    </tr>
                <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                <% foreach (TNV.Web.Models.MathKindListModel Item in Model)
                    { %>
                        <% m++; %>
                        <tr>
                            <td align="center">
                                <label> <%: m.ToString().Trim() %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.MathKindListName %>"><%: Item.MathKindListName %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.MathKindListInfor %>"><%: Item.MathKindListInfor %></label> 
                            </td>
                            <td align="left">
                                <label title="<%: Item.MathKindListOrder %>"><%: Item.MathKindListOrder%></label> 
                            </td>
                            <td align="center">
                                <img alt="Sửa thông tin dạng toán" title="Sửa thông tin: <%: Item.MathKindListName %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','SystemManager','EditMathKindList', '<%=Item.MathKindListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa thông tin dạng toán" title="Xóa thông tin: <%: Item.MathKindListName %>" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn xóa dạng toán này?','SystemManager','DelMathKindList', '<%=Item.MathKindListId %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/><br />
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

