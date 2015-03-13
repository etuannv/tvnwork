<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.NewsContentModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

<table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="center">
                <div class="TitleNomal">SỬA NỘI DUNG BÀI VIẾT</div>
                <input type="hidden" name="PagesCurent" value="<%=ViewData["PagesCurent"]%>" />
                <%: Html.HiddenFor(m=>m.NewsId)%>
                <p class="validation-summary-errors"><%=ViewData["Error"] %></p>
                <asp:Table ID="Table1" Width="80%" runat="server">
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            Chọn chuyên mục:
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.DropDownList("NewsCatId", (SelectList)ViewData["NewsCategoryList"], new { @class = "Input1", @title = "Chọn chuyên mục tin bài" })%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsTitle) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextBoxFor(m => m.NewsTitle, new { @class = "Input1", @title = "Nhập tiêu đề bài viết" })%><br />
                            <%: Html.ValidationMessageFor(m => m.NewsTitle)%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsNarration) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextAreaFor(m => m.NewsNarration, new { @class = "Input1", @title = "Nhập lời dẫn bài viết" })%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsAuthor) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextBoxFor(m => m.NewsAuthor, new { @class = "Input1", @title = "Nhập tác giả bài viết" })%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsOrder) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextBoxFor(m => m.NewsOrder, new { @class = "Input1", @title = "Nhập thứ tự bài viết" })%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsImage) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <input type="file" name="NewsImage" title="Nhập ảnh đại diện" size="52" />
                            <%:Html.Hidden("NewsImageOld", ViewData["NewsImage"])%>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Table ID="Table2" Width="100%" runat="server">
                    <asp:TableRow Height="30px">
                        <asp:TableCell ColumnSpan=2>
                                <asp:Table ID="Table3" runat="server" align="center">
                                <asp:TableRow Height="30px">
                                    <asp:TableCell>
                                        <h2 class="SmallTitleNomal">NỘI DUNG BÀI VIẾT</h2>
                                    </asp:TableCell>
                                </asp:TableRow>
                                <asp:TableRow Height="30px">
                                    <asp:TableCell>
                                        <%: Html.TextAreaFor(m => m.NewsContents, new { @title = "Nhập nội dung bài viết" })%><br />
                                        <%: Html.ValidationMessageFor(m => m.NewsContents)%>
                                        <script type="text/javascript">
                                            window.onload = function () {
                                                var oFCKeditor = new FCKeditor('NewsContents');
                                                oFCKeditor.BasePath = "/Content/FckEditior/fckeditor/";
                                                oFCKeditor.Height = 600;
                                                oFCKeditor.Width = 650;
                                                oFCKeditor.ReplaceTextarea();
                                            }
                                        </script>
                                    </asp:TableCell>
                                </asp:TableRow>
                                </asp:Table>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell align="center" ColumnSpan=2>
                                <img alt="Lưu sửa bài viết" title="Lưu sửa bài viết" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','SaveEditNewsItem', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Quay về danh sách" title="Quay về danh sách" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','ListNews', '', '','', '','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
             </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
    </table>
</asp:Content>
