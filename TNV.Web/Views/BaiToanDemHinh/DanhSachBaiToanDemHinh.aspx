<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanDemHinhModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN ĐẾM HÌNH</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <%: Html.Hidden("PhanLoaiBaiToan", ViewData["PhanLoaiBaiToan"])%>
                                <img alt="Thêm bài toán đếm hình mới" title="Thêm bài toán đếm hình mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanDemHinh','ThemBaiToanDemHinh', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.BaiToanDemHinhModel Item in Model)
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
                                        <a href="/BaiToanDemHinh/SuaBaiToanDemHinh/<%:Item.MaBaiToan %>" title="Sửa câu hỏi" class="SuaXoa">Sửa</a>|<a href="/BaiToanDemHinh/XoaBaiToanDemHinh/<%:Item.MaBaiToan %>" title="Xóa câu hỏi" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                <%: MvcHtmlString.Create(Item.NoiDungBaiToan)%> 
                            </div>
                            <div class="LoiGiaiBaiToan">
                                <label style="text-decoration:underline; font-size:12px; font-weight:bold">Lời giải:</label><br />
                                <%: MvcHtmlString.Create(Item.LoiGiaiBaiToan)%>
                            </div>
                            <div class="DapSoBaiToan">
                                <label style="text-decoration:underline; font-size:12px; font-weight:bold">Đáp số:</label>
                                <b><%: MvcHtmlString.Create(Item.DapAnBaiToan)%></b>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','BaiToanDemHinh','DanhSachBaiToanDemHinh', '" + ViewData["ThuocKhoiLop"] + "', '" + ViewData["PhanLoaiBaiToan"] + "');" })%>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

