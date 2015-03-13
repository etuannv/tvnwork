<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<TNV.Web.Models.NewsContentModel>>" %>
<div class="Hot-new">
        <div class="new-boder">
            <div class="MostNew">
                <div class="img">
                    <a title="<%:ViewData["NewsTitle"]%>" href="/Home/NewsDetail/<%:ViewData["NewsId"]%>"><img width="420px" height="260px" title="<%:ViewData["NewsTitle"]%>" alt="<%:ViewData["NewsTitle"]%>" src="<%:ViewData["NewsImage"]%>"/></a>
                </div>
                <div class="title">
                    <a title="<%:ViewData["NewsTitle"]%>" href="/Home/NewsDetail/<%:ViewData["NewsId"]%>"><%:ViewData["NewsTitle"]%></a>
                </div>
                <div class="Narration"><%:ViewData["NewsNarration"]%></div>
                <div class="detail">
                    <a title="<%:ViewData["NewsTitle"]%>" href="/Home/NewsDetail/<%:ViewData["NewsId"]%>">Chi tiết</a>
                </div>
            </div>
        </div>
        <div class="SomeHotNews">
            <div class="rem-cua">
                <div class="center">
                    <div class="title">Bài viết mới</div>
                    <% foreach (var item in Model)
                       { %>
                            <div class="item">
                                <a title="<%: item.NewsTitle %>" href="/Home/NewsDetail/<%: item.NewsId.Trim()%>"><%: item.NewsTitle %></a>
                                <br/>
                                <p title="<%: item.NewsNarration %>" class="info"><%: item.NewsNarration %></p>
                            </div>
                     <%} %>
                </div>
            </div>
        </div>
    </div>


