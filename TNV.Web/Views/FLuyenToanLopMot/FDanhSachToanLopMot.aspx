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
                        <div class="content-box">
                            <div class="content-title grad-green2">
                                <a href="/hoi-dap">Phép toán 2 số hạng</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#haisohang").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="haisohang">
                                        <h3>
                                            A1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongPhamVi10/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.11</span><span class="skill-tree-skill-name">Phép
                                                        cộng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruPhamVi10/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.12</span><span class="skill-tree-skill-name">Phép
                                                        trừ</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongSoSanhPhamVi10/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.13</span><span class="skill-tree-skill-name">Phép
                                                        cộng so sánh</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruSoSanhPhamVi10/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.14</span><span class="skill-tree-skill-name">Phép
                                                        trừ so sánh</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            A2. Phạm vi 20</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongPhamVi20/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.21</span><span class="skill-tree-skill-name">Phép
                                                        cộng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruPhamVi20/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.22</span><span class="skill-tree-skill-name">Phép
                                                        trừ</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongSoSanhPhamVi20/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.23</span><span class="skill-tree-skill-name">Phép
                                                        cộng so sánh</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruSoSanhPhamVi20/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.24</span><span class="skill-tree-skill-name">Phép
                                                        trừ so sánh</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            A3. Phạm vi 30</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongPhamVi30/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.31</span><span class="skill-tree-skill-name">Phép
                                                        cộng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruPhamVi30/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.32</span><span class="skill-tree-skill-name">Phép
                                                        trừ</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongSoSanhPhamVi30/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.33</span><span class="skill-tree-skill-name">Phép
                                                        cộng so sánh</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruSoSanhPhamVi30/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.34</span><span class="skill-tree-skill-name">Phép
                                                        trừ so sánh</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            A4. Phép cộng trừ không nhớ</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongKhongNho/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.41</span><span class="skill-tree-skill-name">Phép
                                                        cộng không nhớ</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruKhongNho/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.42</span><span class="skill-tree-skill-name">Phép
                                                        trừ không nhớ</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            A5. Phép cộng trừ có nhớ</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/CongCoNho/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.51</span><span class="skill-tree-skill-name">Phép
                                                        cộng có nhớ</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanHaiSoHang/TruCoNho/CLS1847290691">
                                                    <span class="skill-tree-skill-number">A.52</span><span class="skill-tree-skill-name">Phép
                                                        trừ có nhớ</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="span5 ct-block">
                        <div class="content-box">
                            <div class="content-title grad-pink text-center">
                                <a href="/Toan-vui-hang-tuan">Phép toán ba số hạng</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#basohang").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="basohang">
                                        <h3>
                                            B1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi10/CLS1847290691">
                                                    <span class="skill-tree-skill-number">B.11</span><span class="skill-tree-skill-name">Cộng
                                                        trừ 3 số</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            B2. Phạm vi 20</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi20/CLS1847290691">
                                                    <span class="skill-tree-skill-number">B.21</span><span class="skill-tree-skill-name">Cộng
                                                        trừ 3 số hạng</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            B3. Phạm vi 30</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi30/CLS1847290691">
                                                    <span class="skill-tree-skill-number">B.31</span><span class="skill-tree-skill-name">Cộng
                                                        trừ 3 số hạng</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            B4. Phạm vi 30 đến 100</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/PhepToanBaSoHang/CongTruPhamVi30Den100/CLS1847290691">
                                                    <span class="skill-tree-skill-number">B.41</span><span class="skill-tree-skill-name">Cộng
                                                        trừ 3 số hạng</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <a></a>
                        </div>
                        <a></a>
                    </div>
                    <a></a>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <div class="content-box">
                            <div class="content-title grad-blue">
                                <a href="/hoi-dap">Bài toán thêm bớt</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#baitoanthembot").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="baitoanthembot">
                                        <h3>
                                            C1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/ThemSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.11</span><span class="skill-tree-skill-name">Thêm
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/BotSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.12</span><span class="skill-tree-skill-name">Bớt
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangMot">
                                                    <span class="skill-tree-skill-number">C.13</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangHai">
                                                    <span class="skill-tree-skill-number">C.14</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            C2. Phạm vi 20</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi20/ThemSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.21</span><span class="skill-tree-skill-name">Thêm
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi20/BotSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.22</span><span class="skill-tree-skill-name">Bớt
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi20/TimSoDoiTuongBanDauDangMot">
                                                    <span class="skill-tree-skill-number">C.23</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi20/TimSoDoiTuongBanDauDangHai">
                                                    <span class="skill-tree-skill-number">C.34</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            C3. Phạm vi 30</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30/ThemSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.21</span><span class="skill-tree-skill-name">Thêm
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30/BotSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.22</span><span class="skill-tree-skill-name">Bớt
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30/TimSoDoiTuongBanDauDangMot">
                                                    <span class="skill-tree-skill-number">C.23</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30/TimSoDoiTuongBanDauDangHai">
                                                    <span class="skill-tree-skill-number">C.34</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            C4. Phạm vi từ 30 đến 100</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30Den100/ThemSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.21</span><span class="skill-tree-skill-name">Thêm
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30Den100/BotSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.22</span><span class="skill-tree-skill-name">Bớt
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30Den100/TimSoDoiTuongBanDauDangMot">
                                                    <span class="skill-tree-skill-number">C.23</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi30Den100/TimSoDoiTuongBanDauDangHai">
                                                    <span class="skill-tree-skill-number">C.34</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="" class="span5">
                        <div class="content-box">
                            <div class="content-title grad-blue">
                                <a href="/hoi-dap">Bài toán hai đối tượng</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#baitoanhaidoituong").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="baitoanhaidoituong">
                                        <h3>
                                            D1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/TongHaiDoiTuong">
                                                    <span class="skill-tree-skill-number">D.11</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.12</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangBa">
                                                    <span class="skill-tree-skill-number">D.13</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangBon">
                                                    <span class="skill-tree-skill-number">D.14</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangNam">
                                                    <span class="skill-tree-skill-number">D.15</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/HaiDoiTuongHonKemNhau">
                                                    <span class="skill-tree-skill-number">D.16</span><span class="skill-tree-skill-name">Hai
                                                        đối tượng hơn kém nhau</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/BietTongHaiDoiTuongDangMot">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi10/BietTongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            D2. Phạm vi 20</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/TongHaiDoiTuong">
                                                    <span class="skill-tree-skill-number">D.11</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.12</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangBa">
                                                    <span class="skill-tree-skill-number">D.13</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangBon">
                                                    <span class="skill-tree-skill-number">D.14</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangNam">
                                                    <span class="skill-tree-skill-number">D.15</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/HaiDoiTuongHonKemNhau">
                                                    <span class="skill-tree-skill-number">D.16</span><span class="skill-tree-skill-name">Hai
                                                        đối tượng hơn kém nhau</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/BietTongHaiDoiTuongDangMot">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi20/BietTongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            D3. Phạm vi 30</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/TongHaiDoiTuong">
                                                    <span class="skill-tree-skill-number">D.11</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.12</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangBa">
                                                    <span class="skill-tree-skill-number">D.13</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangBon">
                                                    <span class="skill-tree-skill-number">D.14</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangNam">
                                                    <span class="skill-tree-skill-number">D.15</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/HaiDoiTuongHonKemNhau">
                                                    <span class="skill-tree-skill-number">D.16</span><span class="skill-tree-skill-name">Hai
                                                        đối tượng hơn kém nhau</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/BietTongHaiDoiTuongDangMot">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30/BietTongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            D4. Phạm vi từ 30 đến 100</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuong">
                                                    <span class="skill-tree-skill-number">D.11</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.12</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangBa">
                                                    <span class="skill-tree-skill-number">D.13</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangBon">
                                                    <span class="skill-tree-skill-number">D.14</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100PhamVi30Den100/TongHaiDoiTuongDangNam">
                                                    <span class="skill-tree-skill-number">D.15</span><span class="skill-tree-skill-name">Tổng
                                                        hai đối tượng (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/HaiDoiTuongHonKemNhau">
                                                    <span class="skill-tree-skill-number">D.16</span><span class="skill-tree-skill-name">Hai
                                                        đối tượng hơn kém nhau</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/BietTongHaiDoiTuongDangMot">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/2/PhamVi30Den100/BietTongHaiDoiTuongDangHai">
                                                    <span class="skill-tree-skill-number">D.17</span><span class="skill-tree-skill-name">Biết
                                                        tổng hai đối tượng (Dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a>
                        <br class="clear">
                    </a>
                </div>
                <div class="row">
                    <div style="" class="span5">
                        <div class="content-box">
                            <div class="content-title grad-blue">
                                <a href="/hoi-dap">Bài toán ba đối tượng</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#baitoanhaibatuong").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="baitoanhaibatuong">
                                        <h3>
                                            E1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/TongBaDoiTuong">
                                                    <span class="skill-tree-skill-number">E.11</span><span class="skill-tree-skill-name">Tổng
                                                        ba đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMot">
                                                    <span class="skill-tree-skill-number">E.12</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangHai">
                                                    <span class="skill-tree-skill-number">E.13</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBa">
                                                    <span class="skill-tree-skill-number">E.14</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBon">
                                                    <span class="skill-tree-skill-number">E.15</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangNam">
                                                    <span class="skill-tree-skill-number">E.16</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangSau">
                                                    <span class="skill-tree-skill-number">E.17</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 6)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBay">
                                                    <span class="skill-tree-skill-number">E.18</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 7)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangTam">
                                                    <span class="skill-tree-skill-number">E.19</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 8)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangChin">
                                                    <span class="skill-tree-skill-number">E.110</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 9)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoi">
                                                    <span class="skill-tree-skill-number">E.111</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 10)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiMot">
                                                    <span class="skill-tree-skill-number">E.112</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 11)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiHai">
                                                    <span class="skill-tree-skill-number">E.113</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 12)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiBa">
                                                    <span class="skill-tree-skill-number">E.114</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 13)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiBon">
                                                    <span class="skill-tree-skill-number">E.115</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 14)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            E2. Phạm vi 20</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/TongBaDoiTuong">
                                                    <span class="skill-tree-skill-number">E.21</span><span class="skill-tree-skill-name">Tổng
                                                        ba đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMot">
                                                    <span class="skill-tree-skill-number">E.22</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangHai">
                                                    <span class="skill-tree-skill-number">E.23</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBa">
                                                    <span class="skill-tree-skill-number">E.24</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBon">
                                                    <span class="skill-tree-skill-number">E.25</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangNam">
                                                    <span class="skill-tree-skill-number">E.26</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangSau">
                                                    <span class="skill-tree-skill-number">E.27</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 6)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBay">
                                                    <span class="skill-tree-skill-number">E.28</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 7)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangTam">
                                                    <span class="skill-tree-skill-number">E.29</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 8)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangChin">
                                                    <span class="skill-tree-skill-number">E.210</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 9)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoi">
                                                    <span class="skill-tree-skill-number">E.211</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 10)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiMot">
                                                    <span class="skill-tree-skill-number">E.212</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 11)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiHai">
                                                    <span class="skill-tree-skill-number">E.213</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 12)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiBa">
                                                    <span class="skill-tree-skill-number">E.214</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 13)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiBon">
                                                    <span class="skill-tree-skill-number">E.215</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 14)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            E3. Phạm vi 30</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/TongBaDoiTuong">
                                                    <span class="skill-tree-skill-number">E.31</span><span class="skill-tree-skill-name">Tổng
                                                        ba đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMot">
                                                    <span class="skill-tree-skill-number">E.32</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangHai">
                                                    <span class="skill-tree-skill-number">E.33</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBa">
                                                    <span class="skill-tree-skill-number">E.34</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBon">
                                                    <span class="skill-tree-skill-number">E.35</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangNam">
                                                    <span class="skill-tree-skill-number">E.36</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangSau">
                                                    <span class="skill-tree-skill-number">E.37</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 6)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBay">
                                                    <span class="skill-tree-skill-number">E.38</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 7)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangTam">
                                                    <span class="skill-tree-skill-number">E.39</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 8)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangChin">
                                                    <span class="skill-tree-skill-number">E.310</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 9)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoi">
                                                    <span class="skill-tree-skill-number">E.311</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 10)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiMot">
                                                    <span class="skill-tree-skill-number">E.312</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 11)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiHai">
                                                    <span class="skill-tree-skill-number">E.313</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 12)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiBa">
                                                    <span class="skill-tree-skill-number">E.314</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 13)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiBon">
                                                    <span class="skill-tree-skill-number">E.315</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 14)</span></a></li>
                                            </ol>
                                        </div>
                                        <h3>
                                            E4. Phạm vi từ 30 đến 100</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/TongBaDoiTuong">
                                                    <span class="skill-tree-skill-number">E.41</span><span class="skill-tree-skill-name">Tổng
                                                        ba đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMot">
                                                    <span class="skill-tree-skill-number">E.42</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangHai">
                                                    <span class="skill-tree-skill-number">E.43</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 2)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBa">
                                                    <span class="skill-tree-skill-number">E.44</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 3)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBon">
                                                    <span class="skill-tree-skill-number">E.45</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 4)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangNam">
                                                    <span class="skill-tree-skill-number">E.46</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 5)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangSau">
                                                    <span class="skill-tree-skill-number">E.47</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 6)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBay">
                                                    <span class="skill-tree-skill-number">E.48</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 7)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangTam">
                                                    <span class="skill-tree-skill-number">E.49</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 8)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangChin">
                                                    <span class="skill-tree-skill-number">E.410</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 9)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoi">
                                                    <span class="skill-tree-skill-number">E.411</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 10)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiMot">
                                                    <span class="skill-tree-skill-number">E.412</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 11)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiHai">
                                                    <span class="skill-tree-skill-number">E.413</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 12)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiBa">
                                                    <span class="skill-tree-skill-number">E.414</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 13)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanBaDoiTuong/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiBon">
                                                    <span class="skill-tree-skill-number">E.415</span><span class="skill-tree-skill-name">Ba
                                                        đối tượng hơn kém nhau (Dạng 14)</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="" class="span5">
                        <div class="content-box">
                            <div class="content-title grad-blue">
                                <a href="/hoi-dap">Bài toán dãy số</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                    <script type="text/javascript">
                                        $(function () {
                                            $("#baitoandayso").accordion({
                                                collapsible: true,
                                                heightStyle: "content"
                                            });
                                        });
                                    </script>
                                    <div id="baitoandayso">
                                        <h3>
                                            C1. Phạm vi 10</h3>
                                        <div>
                                            <ol class="skill-tree-skills">
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/ThemSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.11</span><span class="skill-tree-skill-name">Thêm
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/BotSoLuongDoiTuong">
                                                    <span class="skill-tree-skill-number">C.12</span><span class="skill-tree-skill-name">Bớt
                                                        số lượng đối tượng</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangMot">
                                                    <span class="skill-tree-skill-number">C.13</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 1)</span></a></li>
                                                <li class="skill-tree-skill-node"><a class="skill-tree-skill-link" href="/FLuyenToanLopMot/BaiToanThemBot/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangHai">
                                                    <span class="skill-tree-skill-number">C.14</span><span class="skill-tree-skill-name">Tìm
                                                        số lượng ban đầu (dạng 2)</span></a></li>
                                            </ol>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <a>
                        <br class="clear">
                    </a>
                </div>
                <a>
                    <br class="clear">
                </a>
            </div>
        </div>
    </div>
    <script type="text/javascript">
		$(function(){
            var base = "http://olm.vn/";
			function getImgPath(id){

				id = id.substr(10);
				var r = base + "images/thumb/thumb_" + id + ".png";
				return r;
			}
			$(".skill-tip").qtip(
			{
			content: {
			text: function(api) {
			var txt = "&lt;h4&gt;Ví dụ mẫu:&lt;/h4&gt;";
			var img = getImgPath($(this).attr('id'));
			txt += "&lt;p&gt;&lt;img style='max-width: 400px;' src=\""+img+"\" /&gt;&lt;/p&gt;";
			return txt;
			}
			},
			position: {
				viewport: $(window),
				target: "mouse",
				adjust: {
					mouse: true,
					x: 15, y:20
				}
			},
			style: {
				classes: 'ui-tooltip ui-tooltip-rounded ui-tooltip-shadow',
				tip: { corner: false}
			}

			}
			);// qtip
			window.changeLocation = function(id_skill){
				window.location.href = ("http://olm.vn/index.php?l=stat.skill&amp;p=0&amp;id_skill="+id_skill);
				return false;
			}
			var scores = JSON.parse('"[]"');
			for(var i = 0; i&lt; scores.length; i++){
				var score = scores[i];
				var id_skill = score.id_skill; score = score.score;
				if(document.getElementById("skill_link"+id_skill)){
                    if(score &lt;100){
					document.getElementById("skill_link"+id_skill).innerHTML += " &lt;span onclick = 'return window.changeLocation("+id_skill+");' class='score'&gt;"+score+"&lt;/span&gt;";
                    }else{
                        document.getElementById("skill_link"+id_skill).innerHTML +=  "&lt;span onclick = 'return window.changeLocation("+id_skill+");' class='score'&gt;"+score+"&lt;/span&gt; &lt;img src='"+base+"images/bonus24.png' onclick = 'return window.changeLocation("+id_skill+");' /&gt;";
                    }
				}
			}
		});
	
    </script>
    </div>
</asp:Content>
