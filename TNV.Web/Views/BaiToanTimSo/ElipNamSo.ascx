﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.BaiToanTimSoModel>" %>
<% string[] CacDay = Model.ChuoiSoHienThi.Split('$'); %>
<table id="Table2" align="center"  width="<%:CacDay.Length*130%>px" cellpadding="0px" cellspacing="0px">
    <tr>
        <% for (int i = 0; i <= CacDay.Length - 1; i++)
            { %>
                <% string[] CacSo = CacDay[i].Split(';'); %>
                <td align="center" width="130px" >
                    <div class="ElipNamSo">
                        <div class="ElipNamSo_So1">
                        <%:CacSo[0]%>
                        </div>
                        <div class="ElipNamSo_So2">
                        <%:CacSo[1]%>
                        </div>
                        <div class="ElipNamSo_So3">
                        <%:CacSo[2]%>
                        </div>
                        <div class="ElipNamSo_So4">
                        <%:CacSo[3]%>
                        </div>
                        <div class="ElipNamSo_So5">
                        <%:CacSo[4]%>
                        </div>
                    </div>
                </td>
        <%} %>
    </tr>
</table>
