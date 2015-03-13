<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.DoiTuongHonKemNhauModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH CÂU HỎI VỀ CÁC ĐỐI TƯỢNG HƠN KÉM NHAU</div>
                <div>
                    <table id="Table2" align="center"  width="98%" cellpadding="0px" cellspacing="0px">
                        <tr>
                            <td align="left" width="18%">
                                [Tổng số: <b><%:ViewData["TongSo"]%></b>] 
                            </td>
                            <td align="right">
                                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                                <%: Html.Hidden("SoLuongDoiTuong", ViewData["SoLuongDoiTuong"])%>
                                <%: Html.Hidden("PhamViPhepToan", ViewData["PhamViPhepToan"])%>
                                <%: Html.Hidden("LoaiCauHoi", ViewData["LoaiCauHoi"])%>
                                <img alt="Thêm câu hỏi mới" title="Thêm câu hỏi mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','DoiTuongHonKemNhau','ThemCauHoiMoi', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các câu hỏi" title="Tạo tự động các câu hỏi" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','DoiTuongHonKemNhau','TaoTuDongCacCauHoi', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các câu hỏi" title="Xóa tất cả các câu hỏi" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','DoiTuongHonKemNhau','XoaTatCacCauHoi', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            </td>
                        </tr>
                    </table>
                 </div>
                 <% int m =Convert.ToInt32(ViewData["StartOrder"].ToString().Trim()); %>
                 <% foreach (TNV.Web.Models.DoiTuongHonKemNhauModel Item in Model)
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
                                        <a href="/DoiTuongHonKemNhau/SuaCauHoi/<%:Item.MaCauHoi%>" title="Sửa câu hỏi" class="SuaXoa">Sửa</a>|<a href="/DoiTuongHonKemNhau/XoaCauHoi/<%:Item.MaCauHoi%>" title="Xóa câu hỏi" class="SuaXoa">Xóa</a>
                                    </td>
                                </tr>
                                </table>
                            </div>
                            <div class="NoiDungHopBaiToan">
                                <%:Item.NoiDungCauHoi%>
                            </div>
                            <div class="TieuDeLoiGiai">
                                Lời giải:
                            </div>
                            <div class="LoiGiaiBaiToan">
                                <%:MvcHtmlString.Create(Item.LoiGiaiCauHoi)%>
                            </div>
                            <div class="DapSoBaiToan">
                                <label class="DapAn">Đáp án:</label> <%:Item.KetLuanCauHoi%>
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
                                #<%: Html.DropDownList("NumRecPerPages", (SelectList)ViewData["ListToSelect"], new { @Style = "width:50px", onchange = "Submitform('QuanTriHeThong','DoiTuongHonKemNhau','DanhSachCauHoi', '" + ViewData["ThuocKhoiLop"] + "', '" + ViewData["SoLuongDoiTuong"] + "','" + ViewData["PhamViPhepToan"] + "', '" + ViewData["LoaiCauHoi"] + "', '');" })%>
                            </td>
                        </tr>
                  </table>

                  <%:ViewData["PhanTich"]%>
            </td>
        </tr>
    </table>
 </asp:Content>

