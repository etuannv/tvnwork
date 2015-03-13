<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.PhepToanHaiSoHangModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH PHÉP TOÁN HAI SỐ HẠNG</div>
                <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                    <tr>
                        <td align="left" width="15%">
                            [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                        </td>
                        <td align="right">
                            <img alt="Thêm mới phép toán" title="Thêm mới phép toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','PhepToanHaiSoHang','ThemPhepToanHaiSoHang', '<%:ViewData["PhamViPhepToan"] %>', '<%:ViewData["ThuocKhoiLop"] %>','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Tạo tự động các phép toán" title="Tạo tự động các phép toán" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','PhepToanHaiSoHang','TaoTuDongPhepToanHaiSoHang', '<%:ViewData["PhamViPhepToan"] %>', '<%:ViewData["ThuocKhoiLop"] %>','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Xóa tất cả các phép toán" title="Xóa tất cả các phép toán" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','PhepToanHaiSoHang','XoaTatCacPhepToanHaiSoHang', '<%:ViewData["PhamViPhepToan"] %>', '<%:ViewData["ThuocKhoiLop"] %>','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>
                <table width='98%' align="center" class="Table1">
                    <tr>
                        <td width="15%" class="Dong1">
                            <b>Câu</b> 
                        </td>
                        <td width="20%" class="Dong1">
                            <b>Phép toán</b> 
                        </td>
                        <td width="10%" class="Dong1">
                            <b>Đáp án</b>
                        </td>
                        <% if (User.IsInRole("AdminOfSystem"))
                        { %>
                            <td width="5%" class="Dong1">
                                <b>Sửa|Xóa</b>
                            </td>
                        <%} %>
                        <td width="15%" class="Dong2">
                            <b>Câu</b>
                        </td>
                        <td width="20%" class="Dong2">
                            <b>Phép toán</b>
                        </td>
                        <td width="10%" class="Dong2">
                            <b>Đáp án</b>
                        </td>
                        <% if (User.IsInRole("AdminOfSystem"))
                        {%>
                            <td width="5%" class="Dong2">
                                <b>Sửa|Xóa</b>
                            </td>
                        <%}%>
	        	    </tr>
                    <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                    <% int dem = 0; %>
                    <% foreach (TNV.Web.Models.PhepToanHaiSoHangModel Item in Model)
                        { %>
                            <% m++; dem++; %>
                            <% if (dem == 1)
                            {%>
                            <tr>
                            <%} %>
                                <td align="center" class="Dong<%:dem%>">
                                    <label>Câu <%: m.ToString().Trim() %></label> 
                                </td>
                                <td align="center" class="Dong<%:dem%>">
                                    <%if (Item.SoHangThuNhat == "?")
                                      {%>
                                        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"  src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <%}
                                    else
                                    {%>
                                        <%: Item.SoHangThuNhat %>
                                    <%} %>
                                    <%if (Item.PhepToan == "?")
                                      {%>
                                        <img alt="Điền dấu thích hợp vào ô trống" title="Điền dấu thích hợp vào ô trống" class="ImageButton"  src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <%}
                                    else
                                    {%>
                                        <%: Item.PhepToan%>
                                    <%} %>

                                    <%if (Item.SoHangThuHai == "?")
                                      {%>
                                        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"  src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <%}
                                    else
                                    {%>
                                        <%: Item.SoHangThuHai%>
                                    <%} %>
                                    <%if (Item.DauQuanHe == "?")
                                      {%>
                                        <img alt="Điền dấu thích hợp vào ô trống" title="Điền dấu thích hợp vào ô trống" class="ImageButton"  src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <%}
                                    else
                                    {%>
                                        <%: Item.DauQuanHe%>
                                    <%} %>
                                    <%if (Item.KetQuaPhepToan == "?")
                                      {%>
                                        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"  src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <%}
                                    else
                                    {%>
                                        <%: Item.KetQuaPhepToan%>
                                    <%} %>
                                </td>
                                <td align="center" class="Dong<%:dem%>">
                                     <%: Item.DapAn%>
                                </td>
                                <% if (User.IsInRole("AdminOfSystem"))
                                { %>
                                <td align="center" class="Dong<%:dem%>">
                                    <img alt="Sửa phép toán hai số hạng" title="Sửa phép toán hai số hạng" class="ImageButton" onclick="Submitform('QuanTriHeThong','PhepToanHaiSoHang','SuaPhepToanHaiSoHang', '<%=Item.MaCauHoi %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    <img alt="Xóa phép toán hai số hạng" title="Xóa phép toán hai số hạng" class="ImageButton" onclick="SubmitformConf('QuanTriHeThong','Bạn thực sự muốn xóa phép toán hai số hạng này?','PhepToanHaiSoHang','XoaPhepToanHaiSoHang', '<%=Item.MaCauHoi %>', '','','','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/><br />
                                </td>
                                <%} %>
	        		        <% if (dem == 2)
                            {
                                dem = 0;%>
                                </tr>
                                
                            <%} %>
                        <%} %>
                        <% if (dem == 1)
                        {%>
                            </tr>
                        <%} %>
                        </table>
                        <br />
                    <table align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                                <% if (ViewData["LoadNumberPage"].ToString() == "1") %>
                            <%{ %>
                            <td align="center" width="90%"  class="Topinfor">
                                <% Html.RenderPartial("NewPage", ViewData["Page"]); %> 
                            </td>
                            <%} %>
                            <td align="right"  class="Topinfor">
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','PhepToanHaiSoHang','PhepToanHaiSoHang', '" + ViewData["PhamViPhepToan"] + "', '"+ViewData["ThuocKhoiLop"]+"','', '', '');" })%>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

