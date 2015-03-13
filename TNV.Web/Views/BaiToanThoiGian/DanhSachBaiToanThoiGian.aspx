<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanThoiGianModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN VỀ THỜI GIAN</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <img alt="Thêm bài toán thời gian mới" title="Thêm bài toán thời gian mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanThoiGian','ThemBaiToanThoiGian', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các bài toán thời gian" title="Tạo tự động các bài toán thời gian" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanThoiGian','TaoTuDongCacBaiToanThoiGian', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các bài toán thời gian" title="Xóa tất cả các bài toán thời gian" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanThoiGian','XoaTatCacBaiToanThoiGian', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.BaiToanThoiGianModel Item in Model)
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
                                        <a href="/BaiToanThoiGian/SuaBaiToanThoiGian/<%:Item.MaCauHoi%>" title="Sửa bài toán thời gian" class="SuaXoa">Sửa</a>|<a href="/BaiToanThoiGian/XoaBaiToanThoiGian/<%:Item.MaCauHoi%>" title="Xóa bài toán thời gian" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                <table width="100%" border="0px">
                                    <tr>
                                        <td align="left" valign="top">
                                            <%if(Item.SoDapAn==1)%>
                                               <%{ %>
                                                        Em hãy đọc thời gian dạng: <b>.... giờ .... phút ..... giây</b> từ đồng hồ sau:
                                               <%} 
                                                 else
                                                 {%>
                                                        Em hãy đọc thời gian dạng: <b>.... giờ .... phút ..... giây </b> hoặc <b>.... giờ kém .... phút kém .... giây</b> từ đồng hồ sau:
                                                 <%} %>
                                        </td>
                                        <td align="right" rowspan="2" width="200px">
                                            <ul class="clock">	
	   	                                        <li id="hour<%:Item.MaCauHoi%>" class="hour"></li>
		                                        <li id="min<%:Item.MaCauHoi%>" class="min"></li>
                                                <li id="sec<%:Item.MaCauHoi%>" class="sec"></li>
	                                        </ul>
                                             <script type="text/javascript">
                                                 $(document).ready(function () {
                                                     DrawClock('hour<%:Item.MaCauHoi%>', 'min<%:Item.MaCauHoi%>', 'sec<%:Item.MaCauHoi%>', <%:Item.Gio%>, <%:Item.Phut%>, <%:Item.Giay%>);
                                                 });
                                            </script>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                                <label class="DapAn">Đáp án:</label><br />
                                                <%if (Item.SoDapAn==1)
                                                  {%>
                                                        - <%:Item.DapAn %>

                                                  <%} 
                                                    else
                                                    {
                                                      string[] CacDapAn=Item.DapAn.Split('$');
                                                      %>
                                                            - <%: CacDapAn[0] %><br />
                                                            - <%: CacDapAn[1] %>
                                                      <%} %>
                                        </td>
                                    </tr>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','BaiToanThoiGian','DanhSachBaiToanThoiGian', '" + ViewData["ThuocKhoiLop"] + "', '','', '', '');" })%>
                            </td>
                        </tr>
                  </table>
            </td>
        </tr>
    </table>
 </asp:Content>

