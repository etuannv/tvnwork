<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.LogOnModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">   
    <table width="100%" border="0px">
        <tr>
            <td width="71%" valign="top">
                <%--<% Html.RenderPartial("MainListNews", ViewData["MainListNews"]); %>--%>
            </td>
            <td width="29%" valign="top" align="center">
                <% Html.RenderPartial("LogOn"); %>
            </td>
        </tr>
    </table>

    x+ <%: Html.TextBox("HHH", 12, new { size="12" })%>=17

    <%
        List<TNV.Web.Models.BaiToanTimSoModel> NewList = new List<TNV.Web.Models.BaiToanTimSoModel>();
        for (int i = 1; i < 60; i = i +19)
        {
            TNV.Web.Models.BaiToanTimSoModel newm = new TNV.Web.Models.BaiToanTimSoModel();
            newm.ChuoiSoHienThi = i.ToString().Trim() + ";" + (i + 1).ToString().Trim() + ";" + (i + 2).ToString().Trim() + ";" + (i + 3).ToString().Trim() + ";" + (i + 4).ToString().Trim() + ";" + (i + 5).ToString().Trim() + ";" + (i + 6).ToString().Trim()
                                  + "$" + (i + 7).ToString().Trim() + ";" + (i + 8).ToString().Trim() + ";" + (i + 9).ToString().Trim() + ";" + (i + 10).ToString().Trim() + ";" + (i + 11).ToString().Trim() + ";" + (i + 12).ToString().Trim() + ";" + (i + 13).ToString().Trim()
                                  + "$" + (i + 14).ToString().Trim() + ";" + (i + 15).ToString().Trim() + ";" + (i + 16).ToString().Trim() + ";" + (i + 17).ToString().Trim() + ";" + (i + 18).ToString().Trim() + ";" + (i + 19).ToString().Trim() + ";" + "x";
            newm.UserControlName = "VongTronLucGiacBaySo";
            NewList.Add(newm);
        }
        foreach(TNV.Web.Models.BaiToanTimSoModel Item in NewList)
        {
            
        }
        %>
</asp:Content>

