﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/FontEnd.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.PhepToanHaiSoHangModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container action">
        <div class="row">
            <div class="span10 col-lg">
                <div class="qholder">
                    <div style="margin-bottom: 10px;" class="bc bc-math">
                        <span class="bc-item bc-first"><a href="/"><img class="bc-logo" src="/Content/font-end/img/bc-olm.png">Học toán</a></span>
                        <span class="bc-item"><a href="/FLuyenToanLopMot/FDanhSachToanLopMot">Lớp một</a></span>
                        <span class="bc-item"><b><a href="/FLuyenToanLopMot/PhepToanHaiSoHang/<%=ViewData["PhamVi"] %>/<%=ViewData["ThuocKhoiLop"] %>">Phép toán 2 số hạng</a></b></span>
                    </div>
                    <div>
                        <div class="question" id="question">
                            <input type="hidden" id="hdfPhamVi" value="<%=ViewData["PhamVi"] %>" />
                            <input type="hidden" id="hdfKhoiLop" value="<%=ViewData["ThuocKhoiLop"] %>" />
                            <h3>
                                Điền số hoặc dấu thích hợp vào ô trống ?</h3>
                            <div id="question-content" class="question-content">
                            </div>
                        </div>
                    </div>
                    <br class="clearfix" />
                    <div id="std_btn" class="std_btn">
                        <input id="btnResult" name="btnResult" type="button" class="btn btn-success btn-large"
                            value="Gửi kết quả" />
                        <input id="btnOtherQuestion" name="btnResult" type="button" class="btn btn-danger btn-large btn-float-right"
                            value="Câu hỏi khác" />
                    </div>
                    <div id="result-dialog" class="dialog-message" title="Kết quả">
                    </div>
                    <script type="text/javascript">
                        // Ready function - get data
                        jQuery(document).ready(function () {
                            var phamvi = $('#hdfPhamVi').val();
                            var thuockhoilop = $('#hdfKhoiLop').val();
                            var RequestUrl = '/FLuyenToanLopMot/GetOnePhepToan2SoHang/' + phamvi + '/' + thuockhoilop;
                            ajaxGet(RequestUrl, $('#question-content'));
                        });

                        $('#btnOtherQuestion').click(function () {
                            var phamvi = $('#hdfPhamVi').val();
                            var thuockhoilop = $('#hdfKhoiLop').val();
                            var RequestUrl = '/FLuyenToanLopMot/GetOnePhepToan2SoHang/' + phamvi + '/' + thuockhoilop;
                            //Increase question number
                            increaseNum($('#hdfQuestionCount'), $('#questionval'), 1);
                            ajaxGet(RequestUrl, $('#question-content'));
                        });
                        // Button send result click
                        $('#btnResult').click(function () {
                            var phamvi = $('#hdfPhamVi').val();
                            var thuockhoilop = $('#hdfKhoiLop').val();
                            var RequestUrl = '/FLuyenToanLopMot/GetOnePhepToan2SoHang/' + phamvi + '/' + thuockhoilop;
                            var resultdialog = $("#result-dialog");
                            resultdialog.empty();
                            var dapan = $('#hdfDapAn').val();
                            var dapso = $('#txtDapSo').val();
                            if (dapso == '') {
                                resultdialog.append('<h3>Bạn chưa trả lời ?<h3>');
                                resultdialog.dialog({
                                    position: {
                                        my: 'top',
                                        at: 'top',
                                        of: $('#question')
                                    },
                                    width: 'auto', // overcomes width:'auto' and maxWidth bug
                                    maxWidth: 600,
                                    height: 'auto',
                                    modal: true,
                                    buttons: {
                                        "Đóng lại và tiếp tục": function () {
                                            $(this).dialog("close");
                                            $("#txtDapSo").focus();
                                        },
                                        "Bỏ qua câu này": function () {
                                            $(this).dialog("close");
                                            //Increase question number
                                            increaseNum($('#hdfQuestionCount'), $('#questionval'), 1);
                                            ajaxGet(RequestUrl, $('#question-content'));
                                        }
                                    }
                                });

                                return;
                            }

                            if (dapan != dapso) {
                                resultdialog.append('<h3>Chưa chính xác</h3><p style="color:red;">Đáp án là: ' + dapan + '</p>');
                            }
                            else {
                                resultdialog.append('<h3>Đúng</h3>');
                                increaseNum($('#hdfscore_input'), $('#scoreval'), 10);
                            }
                            //Show result dialog
                            resultdialog.dialog({
                                position: {
                                    my: 'top',
                                    at: 'top',
                                    of: $('#question')
                                },
                                width: '300px', // overcomes width:'auto' and maxWidth bug
                                maxWidth: 600,
                                modal: true,
                                buttons: {
                                    Ok: function () {
                                        $(this).dialog("close");
                                        ajaxGet(RequestUrl, $('#question-content'));
                                    }
                                }
                            });
                            //Increase question number
                            increaseNum($('#hdfQuestionCount'), $('#questionval'), 1);
                        });


                        
                    </script>
                </div>
            </div>
            <!-- SPAN-->
            <div class="span2 col-sm">
                <div class="sm-count">
                    <div class="text-center time-hold">
                        <div id="mytimer" class="time-label grad-green2">
                        </div>
                        <script type="text/javascript">
                            jQuery(document).ready(function () {
                                $('#mytimer').timer('start'); //Same as $('#divId').timer('start')
                            });
                        </script>
                    </div>
                </div>
                <div class="sm-score">
                    <div style="text-align: center;" id="score">
                        <div class="score-label grad-pink">
                            Điểm</div>
                        <div id="scoreval">
                            0</div>
                        <input type="hidden" value="0" id="hdfscore_input" />
                    </div>
                </div>
                <div class="sm-question">
                    <div style="text-align: center;" id="question-count">
                        <div class="score-label grad-blue">
                            Số câu</div>
                        <div id="questionval">
                            0</div>
                        <input type="hidden" value="0" id="hdfQuestionCount" />
                    </div>
                </div>
                <div class="visible-desktop count-prc alert">
                    Hôm nay, bạn còn <strong id="count_prc">26</strong> lượt làm bài tập miễn phí.
                </div>
                <div class="clearfix">
                </div>
            </div>
        </div>
    </div>
</asp:Content>
