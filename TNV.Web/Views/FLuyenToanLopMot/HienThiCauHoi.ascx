<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.PhepToanHaiSoHangModel>" %>
<tr>
    <td>
    hehe
        <%if (Model.SoHangThuNhat == "?")
          {%>
        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"
            src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100"
            onmouseover="this.style.opacity=0.5" />
        <%}
          else
          {%>
        <%: Model.SoHangThuNhat %>
        <%} %>
        <%if (Model.PhepToan == "?")
          {%>
        <img alt="Điền dấu thích hợp vào ô trống" title="Điền dấu thích hợp vào ô trống"
            class="ImageButton" src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100"
            onmouseover="this.style.opacity=0.5" />
        <%}
          else
          {%>
        <%: Model.PhepToan%>
        <%} %>
        <%if (Model.SoHangThuHai == "?")
          {%>
        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"
            src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100"
            onmouseover="this.style.opacity=0.5" />
        <%}
          else
          {%>
        <%: Model.SoHangThuHai%>
        <%} %>
        <%if (Model.DauQuanHe == "?")
          {%>
        <img alt="Điền dấu thích hợp vào ô trống" title="Điền dấu thích hợp vào ô trống"
            class="ImageButton" src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100"
            onmouseover="this.style.opacity=0.5" />
        <%}
          else
          {%>
        <%: Model.DauQuanHe%>
        <%} %>
        <%if (Model.KetQuaPhepToan == "?")
          {%>
        <img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="ImageButton"
            src="<%: Url.Content("~/Content/Image/OTrong.png") %>" onmouseout="this.style.opacity=100"
            onmouseover="this.style.opacity=0.5" />
        <%}
          else
          {%>
        <%: Model.KetQuaPhepToan%>
        <%} %>
    </td>
</tr>
<tr>
    <td>
        Đáp số: 
        <%Html.TextBox("txtDapSo"); %>
    </td>
</tr>
<tr>
    <td>
        Đáp An: <%: Model.DapAn%>
    </td>
</tr>
