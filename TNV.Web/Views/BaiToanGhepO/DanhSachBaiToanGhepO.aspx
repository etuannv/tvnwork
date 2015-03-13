<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.BaiToanGhepOModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <div class="TitleNomal">DANH SÁCH BÀI TOÁN GHÉP HAI Ô CÙNG GIÁ TRỊ</div>
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
                                <img alt="Thêm bài toán ghép ô mới" title="Thêm bài toán ghép ô mới" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanGhepO','ThemBaiToanGhepO', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateOne.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Tạo tự động các bài toán ghép ô" title="Tạo tự động các bài toán ghép ô" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanGhepO','TaoTuDongCacBaiToanGhepO', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/CreateAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Xóa tất cả các bài toán ghép ô" title="Xóa tất cả các bài toán ghép ô" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','BaiToanGhepO','XoaTatCacBaiToanGhepO', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/DeleteAll.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
                            <%if (ViewData["LoaiBaiToan"].ToString().Trim() == "BaiToanGhepO")
                              {%>
                                    Cho bảng gồm các ô chứa số, phép tính
                                <%}
                              else
                              { %>
                                    Cho bảng gồm các ô chứa số, phép tính, số viết bằng chữ
                                  <%} %>
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
                                                    <div class="STTHop" title="Biểu thức này có số thứ tự là <%:Dem%>"><%:Dem%></div>
                                                    <div class="BieuThuc" title="Biểu thức này có số thứ tự là <%:Dem%>"><%:DanhSachBieuThuc[Dem-1].ToString().Trim()%></div>
                                                </div>
                                            </td>
                                        <%} %>
                                    </tr>
                                  <%} %>
                                </table>
                            </div>
                            <div class="LoiGiaiBaiToan">
                                <%if (ViewData["LoaiBaiToan"].ToString().Trim() == "BaiToanGhepO")
                                  {%>
                                        Em hãy chọn trong bảng các cặp ô chứa số hoặc kết quả phép tính có giá trị bằng nhau bằng cách viết cặp số thứ tự ô của chúng?
                                    <%}
                                  else
                                  { %>
                                        Em hãy chọn trong bảng các cặp ô chứa số hoặc kết quả phép tính với số viết bằng chữ có giá trị bằng nhau bằng cách viết cặp số thứ tự ô của chúng?
                                      <%} %>
                                
                                <table width="100%" border="0px" style="margin-top:10px; margin-bottom:10px;" align="center">
                                <% int SoDem = 0; %>
                                <% for (int k = 1; k <= (Item.ChieuDoc * Item.ChieuNgang) / 2; k++)
                                   {%>
                                        <% SoDem++; %>
                                        <% if (SoDem==1) 
                                        {%>
                                            <tr>
                                        <%} %>
                                            <td align="center">
                                                <div class="HopDapAn">&nbsp;&nbsp;</div> và <div class="HopDapAn">&nbsp;&nbsp;</div>
                                            </td>
                                        <% if (SoDem == 4)
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
                                <%if (ViewData["LoaiBaiToan"].ToString().Trim() == "BaiToanGhepO")
                                  {%>
                                        Các cặp ô chứa số hoặc kết quả phép tính có giá trị bằng nhau là:
                                    <%}
                                  else
                                  { %>
                                        Các cặp ô chứa số hoặc kết quả phép tính với số viết bằng chữ có giá trị bằng nhau là:
                                      <%} %>
                                
                                <table width="100%" border="0px" style="margin-top:10px; margin-bottom:10px;" align="center">
                                <% int SoDemDA = 0; %>
                                <% string[] DSDapAn = Item.NoiDungDapAn.Split('$'); %>
                                <% for (int k = 0; k < (Item.ChieuDoc * Item.ChieuNgang)/2; k++)
                                   {%>
                                        <% string[] DapAn = DSDapAn[k].Split(';'); %>
                                        <% SoDemDA++; %>
                                        <% if (SoDemDA == 1) 
                                        {%>
                                            <tr>
                                        <%} %>
                                            <td align="center">
                                                <div class="HopDapAn"><%:DapAn[0].Trim()%></div> và <div class="HopDapAn"><%:DapAn[1].Trim()%></div>
                                            </td>
                                        <% if (SoDemDA == 4)
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

