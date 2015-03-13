<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.BaiToanDemHinhModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("MenuQuanTriToanLopMot"); %>
            </td>
            <td width="71%" valign="top" align="center">
                <div class="TitleNomal">THÊM MỚI BÀI TOÁN ĐẾM HÌNH</div>
                <%: Html.Hidden("PageCurent", ViewData["PageCurent"])%>
                <%: Html.Hidden("ThuocKhoiLop", ViewData["ThuocKhoiLop"])%>
                <%: Html.Hidden("PhanLoaiBaiToan", ViewData["PhanLoaiBaiToan"])%>
                <asp:Table ID="Table1" Width="98%" runat="server">
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <h2 class="SmallTitleNomal">NỘI DUNG BÀI TOÁN</h2>
                            <%: Html.TextAreaFor(m => m.NoiDungBaiToan)%><br />
                            <script type="text/javascript">
                                $(document).ready(function () {
                                    var oFCKeditor4 = new FCKeditor('NoiDungBaiToan');
                                    oFCKeditor4.BasePath = "/Content/FckEditior/fckeditor/";
                                    oFCKeditor4.Height = 400;
                                    oFCKeditor4.Width = 680;
                                    oFCKeditor4.ReplaceTextarea();
                                }); 
                            </script>
                        </asp:TableCell>
                    </asp:TableRow>
                     <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <h2 class="SmallTitleNomal">LỜI GIẢI BÀI TOÁN</h2>
                            <%: Html.TextAreaFor(m => m.LoiGiaiBaiToan)%><br />
                            <script type="text/javascript">
                                $(document).ready(function () {
                                    var oFCKeditor4 = new FCKeditor('LoiGiaiBaiToan');
                                    oFCKeditor4.BasePath = "/Content/FckEditior/fckeditor/";
                                    oFCKeditor4.Height = 400;
                                    oFCKeditor4.Width = 680;
                                    oFCKeditor4.ReplaceTextarea();
                                }); 
                            </script>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.DapAnBaiToan) %>
                            <%: Html.TextBoxFor(m => m.DapAnBaiToan, new { @class = "Input1", @title = "Đáp án của bài toán" })%><br />
                            <%: Html.ValidationMessageFor(m => m.DapAnBaiToan)%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell align="center">
                                <img alt="Lưu mới bài toán" title="Lưu mới bài toán" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanDemHinh','LuuMoiBaiToanDemHinh', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Quay về danh sách" title="Quay về danh sách" class="ImageButton" onclick="Submitform('QuanTriHeThong','BaiToanDemHinh','DanhSachBaiToanDemHinh', '<%:ViewData["ThuocKhoiLop"] %>', '<%:ViewData["PhanLoaiBaiToan"] %>','', '','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
    </table>
 </asp:Content>
