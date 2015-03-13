<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.PagesModel>" %>
 <% int n=(int)Model.NumberPages; %> 
 <% int BatDau=1; %>
 <% int KetThuc=n; %>
 <% int Trangtruoc=1; %>
 <% int TrangSau=n; %>
 <% if (n>1) %>
 <%{ %>   
<!-- phân trang -->
    <div class="paging">
    <span>[Tổng số: <%:n %> trang] </span>
    <%if ((int)Model.CurentPage-3>1) %>
    <%{ %>
           <% BatDau= (int)Model.CurentPage-3;%>
    <% }%>
    <%if ((int)Model.CurentPage+3<n) %>
    <%{ %>
           <% KetThuc= (int)Model.CurentPage+3;%>
    <% }%>
    <%if ((int)Model.CurentPage==1) %>
    <%{ %>
           <a href="#">|<<</a>
    <% } else%>
     <%{ %>
           <a onclick="Submitform('QuanTriHeThong','<%:Model.Controler %>','<%:Model.Action %>', '1', '<%:Model.memvar2 %>','<%:Model.memvar3 %>', '<%:Model.memvar4 %>', '<%:Model.memvar5 %>');">|<<</a>
    <% } %>
    <%if ((int)Model.CurentPage-1>=1) %>
    <%{ %>
           <% Trangtruoc= (int)Model.CurentPage-1;%>
           <a onclick="Submitform('QuanTriHeThong','<%:Model.Controler %>','<%:Model.Action %>', '<%:Trangtruoc%>', '<%:Model.memvar2 %>','<%:Model.memvar3 %>', '<%:Model.memvar4 %>', '<%:Model.memvar5 %>');"><<</a>
    <% } else%>
     <%{ %>
           <a href="#"><<</a>
    <% } %>
    <%if (BatDau > 1) %>
    <%{ %>
        <span>...</span>
    <%} %>
    <% for (int i = BatDau; i <= KetThuc; i++) %>
    <%{ %>
        <% if (i ==(int)Model.CurentPage) %>
        <%{ %>
            <a class="current" href="#"><%:i%></a>
        <%} else%>
        <%{ %>
            <a onclick="Submitform('QuanTriHeThong','<%:Model.Controler %>','<%:Model.Action %>', '<%:i %>', '<%:Model.memvar2 %>','<%:Model.memvar3 %>', '<%:Model.memvar4 %>', '<%:Model.memvar5 %>');"><%:i%></a>
        <%}%>
    <% } %>
    <%if (KetThuc < n) %>
    <%{ %>
        <span>...</span>
    <%} %>
    <%if ((int)Model.CurentPage+1<=n) %>
    <%{ %>
           <% TrangSau = (int)Model.CurentPage + 1;%>
           <a onclick="Submitform('QuanTriHeThong','<%:Model.Controler %>','<%:Model.Action %>', '<%:TrangSau%>', '<%:Model.memvar2 %>','<%:Model.memvar3 %>', '<%:Model.memvar4 %>', '<%:Model.memvar5 %>');">>></a>
    <% } else%>
     <%{ %>
           <a href="#">>></a>
    <% } %>
    <%if ((int)Model.CurentPage==n) %>
    <%{ %>
           <a href="#">>>|</a>
    <% } else%>
     <%{ %>
           <a onclick="Submitform('QuanTriHeThong','<%:Model.Controler %>','<%:Model.Action %>', '<%:n %>', '<%:Model.memvar2 %>','<%:Model.memvar3 %>', '<%:Model.memvar4 %>', '<%:Model.memvar5 %>');">>>|</a>
    <% } %>
    </div>
<%}%> 
<!-- phân trang -->



