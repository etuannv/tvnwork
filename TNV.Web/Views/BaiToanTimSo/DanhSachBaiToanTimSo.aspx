<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanTimSoModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN VỀ TÌM SỐ</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <%: Html.Hidden("PhamViPhepToan", ViewData["PhamViPhepToan"])%>
                                <%: Html.Hidden("PhanLoaiBaiToan", ViewData["PhanLoaiBaiToan"])%>
                                <img alt="Thêm bài toán tìm số mới" title="Thêm bài toán tìm số mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanTimSo','ThemBaiToanTimSo', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các bài toán tìm số" title="Tạo tự động các bài toán tìm số" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanTimSo','TaoTuDongCacBaiToanTimSo', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các bài toán tìm số" title="Xóa tất cả các bài toán tìm số" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanTimSo','XoaTatCacBaiToanTimSo', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.BaiToanTimSoModel Item in Model)
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
                                        <a href="/BaiToanTimSo/SuaBaiToanTimSo/<%:Item.MaCauHoi%>" title="Sửa bài toán tìm số" class="SuaXoa">Sửa</a>|<a href="/BaiToanTimSo/XoaBaiToanTimSo/<%:Item.MaCauHoi%>" title="Xóa bài toán tìm số" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                Em hãy tìm giá trị của x:
                                <% Html.RenderPartial(Item.UserControlName, Item); %>
                            </div>
                            <div class="TieuDeLoiGiai">
                                Lời giải: 
                            </div>
                            <div class="LoiGiaiBaiToan">
                                <%:MvcHtmlString.Create(Item.LoiGiaiBaiToan)%>
                            </div>
                            <div class="DapSoBaiToan">
                                <label class="DapAn">Đáp án:</label> <i>x = <%:Item.DapAn%></i>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','BaiToanTimSo','DanhSachBaiToanTimSo', '" + ViewData["ThuocKhoiLop"] + "', '" + ViewData["PhamViPhepToan"] + "','" + ViewData["PhanLoaiBaiToan"] + "', '', '');" })%>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

