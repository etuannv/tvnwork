<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.NewsCategoryModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="Center">
                <div class="TitleNomal">SỬA NỘI DUNG MỘT CHUYÊN MỤC TIN</div>
                <input type="hidden" name="PagesCurent" value="<%=ViewData["PagesCurent"]%>" />
                <%:Html.HiddenFor(m=>m.NewsCategoryId)%>
                <p class="validation-summary-errors"><%=ViewData["Error"] %></p>
                <asp:Table ID="Table1" runat="server">
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsCategoryTitle) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextBoxFor(m => m.NewsCategoryTitle)%><br />
                            <%: Html.ValidationMessageFor(m => m.NewsCategoryTitle)%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell>
                            <%: Html.LabelFor(m => m.NewsCategoryOrder) %>
                        </asp:TableCell>
                        <asp:TableCell>
                            <%: Html.TextBoxFor(m => m.NewsCategoryOrder)%><br />
                            <%: Html.ValidationMessageFor(m => m.NewsCategoryOrder)%>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow Height="30px">
                        <asp:TableCell align="center" ColumnSpan=2>
                                <img alt="Lưu sửa một chuyên mục tin" title="Lưu sửa một chuyên mục tin" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','SaveEditNewsCategory', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Luu.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                                <img alt="Quay về danh sách chuyên mục tin"  title="Quay về danh sách chuyên mục tin" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','NewsCategoryList', '', '','','','');"  src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
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
