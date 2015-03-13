<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<TNV.Web.Models.NewsCategoryModel>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                <div class="TitleNomal">QUẢN TRỊ DANH SÁCH CHUYÊN MỤC TIN</div>
                <input type="hidden" name="PagesCurent" value="<%=ViewData["PagesCurent"]%>" />
                <div class="divbutton" align="right">
                    <img alt="Thêm mới chuyên mục" title="Thêm mới chuyên mục" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','AddNewsCategory', '', '','', '', '');"  src='<%: Url.Content("~/Content/Image/Them.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </div>
                <table class="Table1"  width="680px">
                    <tr>
                        <th width="50px">
                            STT
                        </th>
                        <th width="150px">
                            Mã chuyên mục tin
                        </th>
                        <th width="260px">
                            Tiêu đề chuyên mục tin
                        </th>
                        <th width="150px">
                            Thứ tự sắp xếp
                        </th>
                        <th width="70px">
                            Cập nhật
                        </th>
                    </tr>
                <%int i = (int)ViewData["RecordStart"]; %>
                <% foreach (var item in Model) { %>
                    <% i++; %>
                    <tr>
                        <td align="center">
                            <%=i.ToString().Trim() %>
                        </td>
                        <td align="left">
                            <%: item.NewsCategoryId %>
                        </td>
                        <td align="left">
                            <%: item.NewsCategoryTitle %>
                        </td>
                        <td align="left">
                            <%: item.NewsCategoryOrder %>
                        </td>
                        <td align="center"> 
                            <img alt="Sửa thông tin của chuyên mục tin bài: <%: item.NewsCategoryTitle %>" title="Sửa thông tin của chuyên mục tin bài: <%: item.NewsCategoryTitle %>" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','EditNewsCategory', '<%=item.NewsCategoryId %>', '','') ;" src="<%: Url.Content("~/Content/Image/Edit.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                            <img alt="Xóa thông tin của chuyên mục tin bài: <%: item.NewsCategoryTitle %>" title="Xóa thông tin của chuyên mục tin bài: <%: item.NewsCategoryTitle %>" class="ImageButton" onclick="SubmitformDel('QuanTriHeThong','NewsWebManager','DelNewsCategory', '<%=item.NewsCategoryId %>', '','') ;" src="<%: Url.Content("~/Content/Image/delete.gif") %>" onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </td>
                    </tr>
                <% } %>
                    <tr>
                        <td align="left" colspan="5">
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


