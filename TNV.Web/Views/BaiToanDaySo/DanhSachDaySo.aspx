<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanDaySoModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN VỀ DÃY SỐ</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <%: Html.Hidden("PhamViPhepToan", ViewData["PhamViPhepToan"])%>
                                <%: Html.Hidden("PhanLoaiDaySo", ViewData["PhanLoaiDaySo"])%>
                                <img alt="Thêm dãy mới" title="Thêm dãy mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanDaySo','ThemDaySoMoi', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các dãy số" title="Tạo tự động các dãy số" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanDaySo','TaoTuDongCacDaySo', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các dãy số" title="Xóa tất cả các dãy số" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanDaySo','XoaTatCacDaySo', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.BaiToanDaySoModel Item in Model)
                 { %>
                       <% m++; %>
                       <div class="HopBaiToan">
                            <div class="TieuDeHoBaiToan">
                                <table width="100%" border="0px">
                                <tr>
                                    <td align="left" class="CauSo">
                                        Câu <%:m%>: 
                                    </td>
                                    <td align="right">
                                        <a href="/BaiToanDaySo/SuaDaySo/<%:Item.MaDaySo%>" title="Sửa dãy số" class="SuaXoa">Sửa</a>|<a href="/BaiToanDaySo/XoaDaySo/<%:Item.MaDaySo%>" title="Xóa dãy số" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                <%:Item.CauHoiHienThi%>
                            </div>
                            <div class="DaySo">
                                <%:MvcHtmlString.Create(Item.NoiDungDaySo.Replace("~",", ").Replace("...", "<img alt=\"Điền số thích hợp vào ô trống\" title=\"Điền số thích hợp vào ô trống\" class=\"ImageButton\"  src=\""+ Url.Content("~/Content/Image/OTrong.png")+"\" onmouseout=\"this.style.opacity=100\" onmouseover=\"this.style.opacity=0.5\"/>"))%>
                            </div>
                            <div class="TieuDeLoiGiai">
                                Lời giải: 
                            </div>
                            <div class="LoiGiaiBaiToan">
                                <%:MvcHtmlString.Create(Item.LoiGiaiCauHoi)%>
                            </div>
                            <div class="DapSoBaiToan">
                                <label class="DapAn">Đáp án:</label> <%:MvcHtmlString.Create(Item.KetLuanCauHoi)%>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','BaiToanDaySo','DanhSachDaySo', '" + ViewData["ThuocKhoiLop"] + "', '" + ViewData["PhamViPhepToan"] + "','" + ViewData["PhanLoaiDaySo"] + "', '', '');" })%>
                                
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

