<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.PhepToanHaiSoHangModel>" %>
        <%if (Model.SoHangThuNhat == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapSo") %>
              
          <%}else{%>

        <%: Model.SoHangThuNhat%>
        <%} %>
        <%if (Model.PhepToan == "?")%>
         <%{%>
              <%=Html.TextBox("txtDapSo")%>
              
          <%}else{%>
        <%: Model.PhepToan%>
        <%} %>
        <%if (Model.SoHangThuHai == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapSo")%>
              
          <%}else{%>
        <%: Model.SoHangThuHai%>
        <%} %>
        <%if (Model.DauQuanHe == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapSo")%>
              
          <%}else{%>
        <%: Model.DauQuanHe%>
        <%} %>
        <%if (Model.KetQuaPhepToan == "?")%>
          <%{%>
              <%=Html.TextBox("txtDapSo")%>
              
          <%}else{%>
        <%: Model.KetQuaPhepToan%>
        <%} %>
         <input type="hidden" id="hdfDapAn" value="<%: Model.DapAn%>" />
