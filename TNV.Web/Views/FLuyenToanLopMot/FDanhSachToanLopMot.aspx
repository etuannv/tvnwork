<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FontEnd.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container action">
        <div class="row ">
            <div class="span2 visible-desktop">
                <ul class="list">
                    <li class="top mn-item" item-id="678"><a href="http://olm.vn/toan-mau-giao">Toán mẫu
                        giáo <b class="icon-chevron-right"></b></a></li>
                    <li class="mn-item active" item-id="673"><a href="http://olm.vn/toan-lop-1">Toán lớp
                        1 <b class="icon-chevron-right"></b></a></li>
                    <li class="mn-item" item-id="674"><a href="http://olm.vn/toan-lop-2">Toán lớp 2 <b
                        class="icon-chevron-right"></b></a></li>
                    <li class="mn-item" item-id="675"><a href="http://olm.vn/toan-lop-3">Toán lớp 3 <b
                        class="icon-chevron-right"></b></a></li>
                    <li class="mn-item" item-id="676"><a href="http://olm.vn/toan-lop-4">Toán lớp 4 <b
                        class="icon-chevron-right"></b></a></li>
                    <li class="mn-item" item-id="677"><a href="http://olm.vn/toan-lop-5">Toán lớp 5 <b
                        class="icon-chevron-right"></b></a></li>
                    <li class="bottom mn-item" item-id="682"><a href="http://olm.vn/toan-lop-6">Toán lớp
                        6 <b class="icon-chevron-right"></b></a></li>
                </ul>
                <div style="margin-bottom: 10px;" id="contest-list">
                    <div id="accordion1" class="accordion">
                        <div class="accordion-group">
                            <div class="accordion-heading">
                                <strong><a href="#collapseTwo" data-parent="#accordion1" data-toggle="collapse" class="accordion-toggle"
                                    style="color: #f60;">Bài kiểm tra lớp 1 </a></strong>
                            </div>
                            <div class="accordion-body" id="collapseTwo">
                                <div class="accordion-inner">
                                    <ul style="margin-left: 5px; font-size: 18px; list-style: none;">
                                        <li><a href="/contest/8/Kiểm-tra-tháng-4-(2013---2014).html">Kiểm tra tháng 4 (2013
                                            - 2014)</a></li><li><a href="/contest/13/Kiểm-tra-tháng-5-(2013---2014).html">Kiểm tra
                                                tháng 5 (2013 - 2014)</a></li><li><a href="/contest/22/Kiểm-tra-tháng-6-(2013---2014).html">
                                                    Kiểm tra tháng 6 (2013 - 2014)</a></li><li><a href="/contest/23/Kiểm-tra-tháng-9-(2014---2015).html">
                                                        Kiểm tra tháng 9 (2014 - 2015)</a></li><li><a href="/contest/28/Kiểm-tra-tháng-10-(2014---2015).html">
                                                            Kiểm tra tháng 10 (2014 - 2015)</a></li>
                                    </ul>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="alert alert-info">
                    <p>
                        Hãy đăng ký tài khoản để khám phá đầy đủ nội dung và chức năng của OnlineMath</p>
                    <a class="btn btn-danger" href="http://olm.vn/index.php?l=user.register">Đăng ký</a>
                </div>
            </div>
            <div class="span10 animated animated-quick zoomIn">
                <div class="bc">
                    <span class="bc-item bc-first"><a href="/">
                        <img class="bc-logo" src="/Content/font-end/img/bc-olm.png" />Online Math</a></span>
                    <span class="bc-item"><a href="/FLuyenToanLopMot/FDanhSachToanLopMot">Toán lớp 1</a></span>
                </div>
                <div>
                    <h3>
                        Luyện toán Lớp 1</h3>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSPhepToan2SoHang"); %>
                    </div>
                    <div class="span5 ct-block">
                        <% Html.RenderPartial("ucDSPhepToan3SoHang"); %>
                    </div>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanThemBot"); %>
                    </div>
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanHaiDoiTuong"); %>
                    </div>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanBaDoiTuong"); %>
                    </div>
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanDaySo"); %>
                    </div>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanVeThoiGian"); %>
                    </div>
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanGhepOCungGiaTri"); %>
                    </div>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanDocSo"); %>
                    </div>
                    <div style="" class="span5">
                        <% Html.RenderPartial("ucDSBaiToanSapXep"); %>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
