<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.TinhThanhPhoModel>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<table width="100%" border="0px">
<tr>
    <td width="71%" valign="top" align="center">
        <div class="TitleNomal">DANH SÁCH TỈNH VÀ THÀNH PHỐ TRÊN TOÀN QUỐC</div>
         <div class="Button">
                <img alt="Đăng nhập" title="Thêm mới thông tin tỉnh thành phố" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','AddProDis', '', '','', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
         </div>

		<table width='98%' class="Table2">
                <% int m = 0; %>
                <% foreach (TNV.Web.Models.TinhThanhPhoModel Item in Model)
                    { %>
                        <% m++; %>
                        <% if (m%3==1) 
                        {%>
                            <tr>
                        <%} %>
                            <td width="8%">
                                <img title="Sửa thông tin: <%: Item.TenTinhTP %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','ProDis','EditProDis', '<%=Item.MaTinhTP %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img title="Xóa thông tin: <%: Item.TenTinhTP %>" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','ProDis','DelProDis', '<%=Item.MaTinhTP %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                            <td width="25%">
                                <label title="<%: Item.TenTinhTP %>"> <%: m.ToString().Trim() %>. <%: Item.TenTinhTP %></label> 
                            </td>
                            

                        <% if (m%3==0) 
                        {%>
                            </tr>
                        <%} %>
               
                <%} %>
                <% if (m%3==1) 
                {%>
				    <td width="33%" colspan="2"></td>
                    <td width="33%" colspan="2"></td>
			    </tr>
                <%} %>
                <% if (m%3==2) 
                {%>
				    <td width="33%" colspan="2"></td>
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

