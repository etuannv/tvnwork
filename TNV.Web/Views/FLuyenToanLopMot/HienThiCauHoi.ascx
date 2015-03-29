<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.PhepToanHaiSoHangModel>" %>
<tr>
    <td>
        <%if (Model.SoHangThuNhat == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapAn") %>
              
          <%}else{%>

        <%: Model.SoHangThuNhat%>
        <%} %>
        <%if (Model.PhepToan == "?")%>
         <%{%>
              <%=Html.TextBox("txtDapAn") %>
              
          <%}else{%>
        <%: Model.PhepToan%>
        <%} %>
        <%if (Model.SoHangThuHai == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapAn") %>
              
          <%}else{%>
        <%: Model.SoHangThuHai%>
        <%} %>
        <%if (Model.DauQuanHe == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapAn") %>
              
          <%}else{%>
        <%: Model.DauQuanHe%>
        <%} %>
        <%if (Model.KetQuaPhepToan == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapAn") %>
              
          <%}else{%>
        <%: Model.KetQuaPhepToan%>
        <%} %>
    </td>
</tr>
<tr>
    <td>
        Đáp An: <%: Model.DapAn%>
        <%Html.Hidden("hdfDapAn", @Model.DapAn); %>
    </td>
</tr>
