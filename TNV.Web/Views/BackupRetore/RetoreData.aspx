<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.FileUploadModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
               <div class="TitleNomal">KHÔI PHỤC DỮ LIỆU</div>
                    <div style="padding-left:50px; padding-right:50px;">
                        <fieldset>
                            <p class="validation-summary-errors"><%=ViewData["Error"] %></p>
                            <asp:Table ID="Table1" runat="server" Width="580px">
                                <asp:TableRow Height="30px">
                                    <asp:TableCell>
                                        Chọn cơ sở dữ liệu sao lưu:
                                    </asp:TableCell>
                                    <asp:TableCell>
                                        <input type="file" name="FileUpload" size="35" /><br />
                                    </asp:TableCell>
                                </asp:TableRow>
                    
                                <asp:TableRow Height="30px">
                                    <asp:TableCell align="center" ColumnSpan=2>
                                            <img alt="Khôi phục dữ liệu" title="Khôi phục dữ liệu" class="ImageButton" onclick="Submitform('QuanTriHeThong','BackupRetore','MakeRetoreData', '', '','');"  src='<%: Url.Content("~/Content/Image/KhoiPhuc.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </fieldset>
                    </div>
                    <br />
                    <div style="padding-left:20px; padding-right:20px;">
                        <fieldset style="width:650px">
                            <h2 class="SmallTitleNomal">HƯỚNG DẪN KHÔI PHỤC DỮ LIỆU</h2>
                            <p class="huongdan">
                                <b>Khôi phục dữ liệu</b> là công việc quan trọng thường làm khi có sự cố xảy ra. 
                                Khôi phục dữ liệu là việc lấy một phiên bản dữ liệu đã được sao lưu trước đó thay thế bản dữ liệu chính thức đã bị sự cố. <br />
                                &nbsp;&nbsp;&nbsp;&nbsp; 1. Chọn File dữ liệu sao lưu <br />
                                &nbsp;&nbsp;&nbsp;&nbsp; 2. Bấm nút "Khôi phục dữ liệu" <br />
                                <span class="CacBuoc">Ghi chú:</span> Khi khôi phục xong phiên bản dữ liệu cũ bị mất đi, thay vào đó là phiên bản dữ liệu mới được sao lưu trước đó.
                            </p>
                            </fieldset> 
                    </div>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
        </table>
</asp:Content>
