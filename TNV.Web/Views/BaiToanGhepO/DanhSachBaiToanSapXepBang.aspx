<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanGhepOModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN SẮP XẾP GIÁ TRỊ TRONG BẢNG</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <%: Html.Hidden("PhamViPhepToan", ViewData["PhamViPhepToan"])%>
                                <%: Html.Hidden("ChieuNgang", ViewData["ChieuNgang"])%>
                                <%: Html.Hidden("ChieuDoc", ViewData["ChieuDoc"])%>
                                <%: Html.Hidden("LoaiBaiToan", ViewData["LoaiBaiToan"])%>
                                <img alt="Thêm bài toán sắp xếp bảng mới" title="Thêm bài toán sắp xếp bảng mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanGhepO','ThemBaiToanSapXepBang', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các bài toán sắp xếp bảng" title="Tạo tự động các bài toán sắp xếp bảng" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanGhepO','TaoTuDongCacBaiToanSapXepBang', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các bài toán sắp xếp bảng" title="Xóa tất cả các bài toán sắp xếp bảng" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanGhepO','XoaTatCacBaiToanSapXepBang', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.BaiToanGhepOModel Item in Model)
                 { %>
                       <% m++; %>
                       <div class="HopBaiToan">
                            <div class="TieuDeHoBaiToan">
                                <table width="100%" border="0px">
                                <tr>
                                    <td align="left" class="CauSo">
                                        Câu <%:m %>:
                                    </td>
                                    <td align="right">
                                        <a href="#" title="Sửa câu hỏi" class="SuaXoa">Sửa</a>|<a href="#" title="Xóa câu hỏi" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                Cho bảng gồm các ô, mỗi ô có chứa một biểu thức:
                                <table width="100%" border="0px" style="margin-top:10px; margin-bottom:10px;" align="center">
                                <% int Dem=0; %>
                                <% string[] DanhSachBieuThuc = Item.NoiDungBaiToan.Split('$');%>
                                <%for(int i=1; i<=Item.ChieuDoc; i++ )
                                  {%>
                                    <tr>
                                        <%for(int j=1; j<=Item.ChieuNgang; j++) 
                                        {%>
                                            <% Dem++; %>
                                            <td align="center" style="padding:3px;">
                                                <div class="HopBieuThuc">
                                                    <div class="STTHop"><%:Dem%></div>
                                                    <div class="BieuThuc"><%:DanhSachBieuThuc[Dem-1].ToString().Trim()%></div>
                                                </div>
                                            </td>
                                        <%} %>
                                    </tr>
                                  <%} %>
                                </table>
                            </div>
                            <div class="LoiGiaiBaiToan">
                                Em hãy viết số thứ tự của các ô chứa biểu thức có giá trị từ bé đến lớn:
                                <table width="100%" border="0px" style="margin-top:10px; margin-bottom:10px;" align="center">
                                <% int SoDem = 0; %>
                                <% for (int k = 1; k <= Item.ChieuDoc * Item.ChieuNgang; k++)
                                   {%>
                                        <% SoDem++; %>
                                        <% if (SoDem==1) 
                                        {%>
                                            <tr>
                                        <%} %>
                                            <td align="center">
                                                <div class="HopDapAn">&nbsp;&nbsp;</div>
                                            </td>
                                        <% if (SoDem == 8)
                                           {
                                               SoDem = 0;%>
                                            </tr>
                                        <%} %>
                                <%} %>
                                <%if (SoDem != 0)
                                  {%>
                                    </tr>
                                  <%} %>
                                </table>
                            </div>
                            <div class="DapSoBaiToan">
                                <label style="text-decoration:underline; font-size:12px; font-weight:bold">Đáp số:</label><br />
                                <table width="100%" border="0px" style="margin-top:10px; margin-bottom:10px;" align="center">
                                <% int SoDemDA = 0; %>
                                <% string[] DSDapAn = Item.NoiDungDapAn.Split('$'); %>
                                <% for (int k = 0; k <= DSDapAn.Length-1; k++)
                                   {%>
                                        <% SoDemDA++; %>
                                        <% if (SoDemDA == 1) 
                                        {%>
                                            <tr>
                                        <%} %>
                                            <td align="center">
                                                <div class="HopDapAn"><%:DSDapAn[k].Trim()%></div>
                                            </td>
                                        <% if (SoDemDA == 8)
                                           {
                                               SoDemDA = 0;%>
                                            </tr>
                                        <%} %>
                                <%} %>
                                <%if (SoDemDA != 0)
                                  {%>
                                    </tr>
                                  <%} %>
                                </table>
                            </div>
                        </div>    
                 <%} %>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','BaiToanGhepO','DanhSachBaiToanGhepO', '" + ViewData["ThuocKhoiLop"] + "', '" + ViewData["PhamViPhepToan"] + "','" + ViewData["ChieuNgang"] + "', '" + ViewData["ChieuDoc"] + "', '" + ViewData["LoaiBaiToan"] + "');" })%>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

