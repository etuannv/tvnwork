<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.NewsContentModel>" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top" align="left">
                <input type="hidden" name="PagesCurent" value="<%=ViewData["PagesCurent"]%>" />
                <input type="hidden" name="NewsCatId" value="<%=ViewData["NewsCatId"]%>" />
                <% Html.RenderPartial("NewsDetail", Model);%>
                <div class="divbutton" align="right">
                    <img alt="Quay về danh sách tin bài" title="Quay về danh sách tin bài" class="ImageButton" onclick="Submitform('QuanTriHeThong','NewsWebManager','ListNews', '', '','', '','');" src='<%: Url.Content("~/Content/Image/Quay.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                </div>
              </td>
            <td width="29%" valign="top" align="center">
                <%--Thêm nội dung cột bên phải--%>
            </td>
        </tr>
    </table>
</asp:Content>


