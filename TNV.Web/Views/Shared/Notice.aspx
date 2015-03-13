<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.NoticeModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
        <table width="100%" border="0px">
        <tr>
            <td valign="top" align="center">
                <div class="BoxNotice">
                    <div class="BoxNoticeTitle">
                        THÔNG BÁO
                        <div style="margin-left:337px; margin-top:-22px;">
                            <img alt="Đóng cửa sổ" title="Đóng cửa sổ" class="ImageButton" onclick="Submitform('QuanTriHeThong','<%:Model.NoticeControler%>','<%:Model.NoticeAction%>', '<%:Model.memvar1%>', '<%:Model.memvar2%>','<%:Model.memvar3%>', '<%:Model.memvar4%>','<%:Model.memvar5%>');"  src='<%: Url.Content("~/Content/Image/Close.png") %>' onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5"/>
                        </div>
                    </div>
                    <div class="BoxNoticeContent">
                        <div class="validation-summary-errors"><%: MvcHtmlString.Create(Model.NoticeContent)%></div>
                    </div>
                </div>
            </td>
        </tr>
        </table>
</asp:Content>

