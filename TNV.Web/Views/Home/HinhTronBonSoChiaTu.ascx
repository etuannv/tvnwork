<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ToanThongMinh.Models.BaiToanTimSoModel>" %>
<table id="Table2" align="center"  width="490px" cellpadding="0px" cellspacing="0px">
    <tr>
        <% string[] CacDay = Model.ChuoiSoHienThi.Split('$'); %>
        <% for (int i = 0; i <= CacDay.Length - 1; i++)
            { %>
                <% string[] CacSo = CacDay[i].Split(';'); %>
                <td align="center" width="130px" >
                    <div class="HinhTronBonSoChiaTu">
                        <div class="HinhTronBonSoChiaTu_So1">
                        <%:CacSo[0]%>
                        </div>
                        <div class="HinhTronBonSoChiaTu_So2">
                        <%:CacSo[1]%>
                        </div>
                        <div class="HinhTronBonSoChiaTu_So3">
                        <%:CacSo[2]%>
                        </div>
                        <div class="HinhTronBonSoChiaTu_So4">
                        <%:CacSo[3]%>
                        </div>
                    </div>
                </td>
        <%} %>
    </tr>
</table>
