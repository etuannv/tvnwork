<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.NewsContentModel>>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                <div class="TitleNomal">QUẢN TRỊ DANH SÁCH BÀI VIẾT</div>
                <input type="hidden" name="PagesCurent" value="<%=ViewData["PagesCurent"]%>" />
                <table id="Table2"  width="100%">
                    <tr>
                        <td align="right" width="40%">
                            <b>Chọn chuyên mục:</b>
                        </td>
                        <td align="left" width="40%">
                            <%: Html.DropDownList("NewsCatId", (SelectList)ViewData["NewsCategoryList"], new { onchange = "Submitform('QuanTriHeThong','NewsWebManager','ListNews', '1', '','', '', '');" })%>
                        </td>
                        <td align="right"  width="20%">
                            <img alt="Thêm tin bài" title="Thêm tin bài"  class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','AddNewsItem', '', '','', '', '','', '', '') ;" src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                </table>
                <table class="Table1"  width="680px">
                    <tr>
                        <th width="40px">
                            STT
                        </th>
                        <th width="200px">
                            Tiêu đề bài viết
                        </th>
                        <th width="200px">
                            Lời dẫn bài viết
                        </th>
                        <th width="100px">
                            Tác giả
                        </th>
                        <th width="70px">
                            Sắp xếp
                        </th>
                        <th width="70px">Cập nhật</th>
                    </tr>
                <%int i = (int)ViewData["RecordStart"]; %>
                <% foreach (var item in Model) { %>
                    <% i++; %>
                    <tr>
                        <td align="center">
                            <%=i.ToString().Trim() %>
                        </td>
                        <td align="left">
                            <%: item.NewsTitle %>
                        </td>
                        <td align="left">
                            <%: item.NewsNarration %>
                        </td>
                            <td align="left">
                            <%: item.NewsAuthor %>
                        </td>
                        <td align="left">
                            <%: item.NewsOrder %>
                        </td>
                        <td align="center"> 
                            <img alt="Xem nội dung tin bài" title="Xem nội dung tin bài" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','ViewNewsItem', '<%=item.NewsId %>', '','', '','') ;" src="<%: Url.Content("~/Content/Image/Search.png") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Sửa nội dung tin bài" title="Sửa nội dung tin bài" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','EditNewsItem', '<%=item.NewsId %>', '','', '','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Xóa nội dung tin bài" title="Xóa nội dung tin bài" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','NewsWebManager','DelNewsItem', '<%=item.NewsId %>', '','', '','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                <% } %>
                </table>
                <table id="Table1"  width="100%">
                    <tr>
                        <td align="center">
                            <% Html.RenderPartial("PagePostNormal", ViewData["Pages"]); %>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
    </table>
</asp:Content>


