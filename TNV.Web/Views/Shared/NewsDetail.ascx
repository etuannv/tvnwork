<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TNV.Web.Models.NewsContentModel>" %>
<% if (!String.IsNullOrEmpty(Model.NewsTitle))  {%>
<div class="NewsCat">
    <%:ViewData["CatTitle"]%>
</div>
<div class="NewsDetail">
    <div class="Title">
        <%: MvcHtmlString.Create(Model.NewsTitle) %>
    </div>
    <div class="Description">
       <%: MvcHtmlString.Create("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + Model.NewsNarration)%>
</div>
    <div class="ContentDetail">
       <%: MvcHtmlString.Create(Model.NewsContents)%>
    </div>
    <div class="Author"><%: MvcHtmlString.Create(Model.NewsAuthor)%></div>
</div>
<%} %>
