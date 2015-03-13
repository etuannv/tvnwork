<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<script type="text/javascript">
    $(document).ready(function () {
        $("#TreeGroup").treeview({
            collapsed: true,
            animated: "medium",
            control: "#treecontrol",
            persist: "location"
        });
    }); 
</script>
<div class="cssColTable">
    <div class="treeTool" id="treecontrol">
        <a class="collapse" href="#" title="Đóng tất cả các nhánh">Đóng tất cả</a> 
        <a class="expand" href="#" title="Mở tất cả các nhánh">Mở tất cả</a>
    </div>
    <ul class="filetree treeview" id="TreeGroup">
        <li id="146" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">PHÉP TOÁN HAI SỐ HẠNG</a></span>
            <ul style="display: none;">
                 <li id="Li1" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongPhamVi10/CLS1847290691" class="">Phép cộng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruPhamVi10/CLS1847290691" class="">Phép trừ</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongSoSanhPhamVi10/CLS1847290691" class="">Phép cộng so sánh</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruSoSanhPhamVi10/CLS1847290691" class="">Phép trừ so sánh</a></span></li>
                    </ul>
                </li>
                <li id="Li2" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongPhamVi20/CLS1847290691" class="">Phép cộng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruPhamVi20/CLS1847290691" class="">Phép trừ </a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongSoSanhPhamVi20/CLS1847290691" class="">Phép cộng so sánh</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruSoSanhPhamVi20/CLS1847290691" class="">Phép trừ so sánh</a></span></li>
                    </ul>
                </li>
                <li id="Li3" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongPhamVi30/CLS1847290691" class="">Phép cộng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruPhamVi30/CLS1847290691" class="">Phép trừ </a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongSoSanhPhamVi30/CLS1847290691" class="">Phép cộng so sánh</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruSoSanhPhamVi30/CLS1847290691" class="">Phép trừ so sánh</a></span></li>
                    </ul>
                </li>
                <li id="Li4" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHÉP CỘNG TRỪ KHÔNG NHỚ</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongKhongNho/CLS1847290691" class="">Phép cộng không nhớ</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruKhongNho/CLS1847290691" class="">Phép trừ không nhớ</a></span></li>
                    </ul>
                </li>
                <li id="Li5" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHÉP CỘNG TRỪ CÓ NHỚ</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/CongCoNho/CLS1847290691" class="">Phép cộng có nhớ</a></span></li>
                        <li class="unselect"><span class="file"><a href="/PhepToanHaiSoHang/PhepToanHaiSoHang/TruCoNho/CLS1847290691" class="">Phép trừ có nhớ</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="151" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">PHÉP TOÁN BA SỐ HẠNG</a></span>
            <ul style="display: none;">
                <li class="unselect"><span class="file"><a href="/PhepToanBaSoHang/PhepToanBaSoHang/CongTruPhamVi10/CLS1847290691" class="">Cộng trừ trong phạm vi 10</a></span></li>
                <li class="unselect"><span class="file"><a href="/PhepToanBaSoHang/PhepToanBaSoHang/CongTruPhamVi20/CLS1847290691" class="">Cộng trừ trong phạm vi 20</a></span></li>
                <li class="unselect"><span class="file"><a href="/PhepToanBaSoHang/PhepToanBaSoHang/CongTruPhamVi30/CLS1847290691" class="">Cộng trừ trong phạm vi 30</a></span></li>
                <li class="unselect"><span class="file"><a href="/PhepToanBaSoHang/PhepToanBaSoHang/CongTruPhamVi30Den100/CLS1847290691" class="">Cộng trừ trong phạm vi 30 đến 100</a></span></li>
            </ul>
        </li>
        <li id="Li14" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN THÊM BỚT</a></span>
            <ul style="display: none;">
                 <li id="Li15" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi10/ThemSoLuongDoiTuong" class="">Thêm số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi10/BotSoLuongDoiTuong" class="">Bớt số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangMot" class="">Tìm số đối tượng ban đầu(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi10/TimSoDoiTuongBanDauDangHai" class="">Tìm số đối tượng ban đầu(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li16" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi20/ThemSoLuongDoiTuong" class="">Thêm số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi20/BotSoLuongDoiTuong" class="">Bớt số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi20/TimSoDoiTuongBanDauDangMot" class="">Tìm số đối tượng ban đầu(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi20/TimSoDoiTuongBanDauDangHai" class="">Tìm số đối tượng ban đầu(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li17" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                       <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30/ThemSoLuongDoiTuong" class="">Thêm số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30/BotSoLuongDoiTuong" class="">Bớt số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30/TimSoDoiTuongBanDauDangMot" class="">Tìm số đối tượng ban đầu(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30/TimSoDoiTuongBanDauDangHai" class="">Tìm số đối tượng ban đầu(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li24" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 ĐẾN 100</a></span>
                    <ul style="display: none;">
                       <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30Den100/ThemSoLuongDoiTuong" class="">Thêm số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30Den100/BotSoLuongDoiTuong" class="">Bớt số lượng đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30Den100/TimSoDoiTuongBanDauDangMot" class="">Tìm số đối tượng ban đầu(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/1/PhamVi30Den100/TimSoDoiTuongBanDauDangHai" class="">Tìm số đối tượng ban đầu(Dạng 2)</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="Li8" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN HAI ĐỐI TƯỢNG</a></span>
            <ul style="display: none;">
                 <li id="Li9" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/TongHaiDoiTuong" class="">Tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangHai" class="">Tổng của hai đối tượng(Dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangBa" class="">Tổng của hai đối tượng(Dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangBon" class="">Tổng của hai đối tượng(Dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/TongHaiDoiTuongDangNam" class="">Tổng của hai đối tượng(Dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/HaiDoiTuongHonKemNhau" class="">Hai đối tượng hơn kém nhau</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/BietTongHaiDoiTuongDangMot" class="">Biết tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/BietTongHaiDoiTuongDangHai" class="">Biết tổng của hai đối tượng(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li10" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/TongHaiDoiTuong" class="">Tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangHai" class="">Tổng của hai đối tượng(Dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangBa" class="">Tổng của hai đối tượng(Dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangBon" class="">Tổng của hai đối tượng(Dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/TongHaiDoiTuongDangNam" class="">Tổng của hai đối tượng(Dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/HaiDoiTuongHonKemNhau" class="">Hai đối tượng hơn kém nhau</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/BietTongHaiDoiTuongDangMot" class="">Biết tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/BietTongHaiDoiTuongDangHai" class="">Biết tổng của hai đối tượng(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li11" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/TongHaiDoiTuong" class="">Tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangHai" class="">Tổng của hai đối tượng(Dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangBa" class="">Tổng của hai đối tượng(Dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangBon" class="">Tổng của hai đối tượng(Dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/TongHaiDoiTuongDangNam" class="">Tổng của hai đối tượng(Dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/HaiDoiTuongHonKemNhau" class="">Hai đối tượng hơn kém nhau</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/BietTongHaiDoiTuongDangMot" class="">Biết tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/BietTongHaiDoiTuongDangHai" class="">Biết tổng của hai đối tượng(Dạng 2)</a></span></li>
                    </ul>
                </li>
                <li id="Li25" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 ĐẾN 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuong" class="">Tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangHai" class="">Tổng của hai đối tượng(Dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangBa" class="">Tổng của hai đối tượng(Dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangBon" class="">Tổng của hai đối tượng(Dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/TongHaiDoiTuongDangNam" class="">Tổng của hai đối tượng(Dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/HaiDoiTuongHonKemNhau" class="">Hai đối tượng hơn kém nhau</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/BietTongHaiDoiTuongDangMot" class="">Biết tổng của hai đối tượng(Dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/BietTongHaiDoiTuongDangHai" class="">Biết tổng của hai đối tượng(Dạng 2)</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        
        <li id="Li6" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN BA ĐỐI TƯỢNG</a></span>
            <ul style="display: none;">
                 <li id="Li7" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/TongBaDoiTuong" class="">Tổng của ba đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMot" class="">Ba đối tượng hơn kém nhau(dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangHai" class="">Ba đối tượng hơn kém nhau(dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBa" class="">Ba đối tượng hơn kém nhau(dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBon" class="">Ba đối tượng hơn kém nhau(dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangNam" class="">Ba đối tượng hơn kém nhau(dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangSau" class="">Ba đối tượng hơn kém nhau(dạng 6)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangBay" class="">Ba đối tượng hơn kém nhau(dạng 7)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangTam" class="">Ba đối tượng hơn kém nhau(dạng 8)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangChin" class="">Ba đối tượng hơn kém nhau(dạng 9)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoi" class="">Ba đối tượng hơn kém nhau(dạng 10)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiMot" class="">Ba đối tượng hơn kém nhau(dạng 11)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiHai" class="">Ba đối tượng hơn kém nhau(dạng 12)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiBa" class="">Ba đối tượng hơn kém nhau(dạng 13)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi10/BaDoiTuongHonKemNhauDangMuoiBon" class="">Ba đối tượng hơn kém nhau(dạng 14)</a></span></li>
                    </ul>
                </li>
                <li id="Li12" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/TongBaDoiTuong" class="">Tổng của ba đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMot" class="">Ba đối tượng hơn kém nhau(dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangHai" class="">Ba đối tượng hơn kém nhau(dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBa" class="">Ba đối tượng hơn kém nhau(dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBon" class="">Ba đối tượng hơn kém nhau(dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangNam" class="">Ba đối tượng hơn kém nhau(dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangSau" class="">Ba đối tượng hơn kém nhau(dạng 6)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangBay" class="">Ba đối tượng hơn kém nhau(dạng 7)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangTam" class="">Ba đối tượng hơn kém nhau(dạng 8)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangChin" class="">Ba đối tượng hơn kém nhau(dạng 9)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoi" class="">Ba đối tượng hơn kém nhau(dạng 10)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiMot" class="">Ba đối tượng hơn kém nhau(dạng 11)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiHai" class="">Ba đối tượng hơn kém nhau(dạng 12)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiBa" class="">Ba đối tượng hơn kém nhau(dạng 13)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi20/BaDoiTuongHonKemNhauDangMuoiBon" class="">Ba đối tượng hơn kém nhau(dạng 14)</a></span></li>
                    </ul>
                </li>
                <li id="Li13" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/TongBaDoiTuong" class="">Tổng của ba đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMot" class="">Ba đối tượng hơn kém nhau(dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangHai" class="">Ba đối tượng hơn kém nhau(dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBa" class="">Ba đối tượng hơn kém nhau(dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBon" class="">Ba đối tượng hơn kém nhau(dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangNam" class="">Ba đối tượng hơn kém nhau(dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangSau" class="">Ba đối tượng hơn kém nhau(dạng 6)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangBay" class="">Ba đối tượng hơn kém nhau(dạng 7)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangTam" class="">Ba đối tượng hơn kém nhau(dạng 8)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangChin" class="">Ba đối tượng hơn kém nhau(dạng 9)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoi" class="">Ba đối tượng hơn kém nhau(dạng 10)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiMot" class="">Ba đối tượng hơn kém nhau(dạng 11)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiHai" class="">Ba đối tượng hơn kém nhau(dạng 12)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiBa" class="">Ba đối tượng hơn kém nhau(dạng 13)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30/BaDoiTuongHonKemNhauDangMuoiBon" class="">Ba đối tượng hơn kém nhau(dạng 14)</a></span></li>
                    </ul>
                </li>

                <li id="Li23" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 ĐẾN 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/TongBaDoiTuong" class="">Tổng của ba đối tượng</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMot" class="">Ba đối tượng hơn kém nhau(dạng 1)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangHai" class="">Ba đối tượng hơn kém nhau(dạng 2)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBa" class="">Ba đối tượng hơn kém nhau(dạng 3)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBon" class="">Ba đối tượng hơn kém nhau(dạng 4)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangNam" class="">Ba đối tượng hơn kém nhau(dạng 5)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangSau" class="">Ba đối tượng hơn kém nhau(dạng 6)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangBay" class="">Ba đối tượng hơn kém nhau(dạng 7)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangTam" class="">Ba đối tượng hơn kém nhau(dạng 8)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangChin" class="">Ba đối tượng hơn kém nhau(dạng 9)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoi" class="">Ba đối tượng hơn kém nhau(dạng 10)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiMot" class="">Ba đối tượng hơn kém nhau(dạng 11)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiHai" class="">Ba đối tượng hơn kém nhau(dạng 12)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiBa" class="">Ba đối tượng hơn kém nhau(dạng 13)</a></span></li>
                        <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/3/PhamVi30Den100/BaDoiTuongHonKemNhauDangMuoiBon" class="">Ba đối tượng hơn kém nhau(dạng 14)</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="Li18" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN DÃY SỐ</a></span>
            <ul style="display: none;">
                 <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi100/DayBoSo2So" class="">Dãy các bộ 2 số phạm vi 100</a></span></li>
                 <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi100/DayBoSo3So" class="">Dãy các bộ 3 số phạm vi 100</a></span></li>
                 <li id="Li19" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi10/DaySoDem" class="">Dãy số đếm</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi10/CapSoCong" class="">Cấp số cộng</a></span></li>
                    </ul>
                </li>
                <li id="Li20" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi20/DaySoDem" class="">Dãy số đếm</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi20/CapSoCong" class="">Cấp số cộng</a></span></li>
                    </ul>
                </li>
                <li id="Li21" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi30/DaySoDem" class="">Dãy số đếm</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi30/CapSoCong" class="">Cấp số cộng</a></span></li>
                    </ul>
                </li>
                <li id="Li22" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 đến 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi30_100/DaySoDem" class="">Dãy số đếm</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanDaySo/DanhSachDaySo/CLS1847290691/PhamVi30_100/CapSoCong" class="">Cấp số cộng</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="Li26" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN VỀ THỜI GIAN</a></span>
            <ul style="display: none;">
                <li class="unselect"><span class="file"><a href="/BaiToanThoiGian/DanhSachBaiToanThoiGian/CLS1847290691" class="">Đọc giờ đồng hồ</a></span></li>
                <li id="Li42" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">BÀI TOÁN TÍNH TUỔI</a></span>
                    <ul style="display: none;">
                        <li id="Li44" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/BaiToanTinhTuoiDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi10/BaiToanTinhTuoiDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                        <li id="Li45" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/BaiToanTinhTuoiDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi20/BaiToanTinhTuoiDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                        <li id="Li46" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/BaiToanTinhTuoiDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30/BaiToanTinhTuoiDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                        <li id="Li47" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">PHẠM VI 30 ĐẾN 100</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/BaiToanTinhTuoiDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/DoiTuongHonKemNhau/DanhSachCauHoi/CLS1847290691/2/PhamVi30Den100/BaiToanTinhTuoiDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>

        <li id="Li27" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN GHÉP Ô CÙNG GIÁ TRỊ</a></span>
            <ul style="display: none;">
                <li id="Li28" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/4/3/BaiToanGhepO" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/4/4/BaiToanGhepO" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/5/4/BaiToanGhepO" class="">Kích thước bảng 5x4</a></span></li>
                    </ul>
                </li>
                <li id="Li29" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/4/3/BaiToanGhepO" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/4/4/BaiToanGhepO" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/5/4/BaiToanGhepO" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/6/4/BaiToanGhepO" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/6/5/BaiToanGhepO" class="">Kích thước bảng 6x5</a></span></li>
                    </ul>
                </li>
                <li id="Li30" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/4/3/BaiToanGhepO" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/4/4/BaiToanGhepO" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/5/4/BaiToanGhepO" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/4/BaiToanGhepO" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/5/BaiToanGhepO" class="">Kích thước bảng 6x5</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/6/BaiToanGhepO" class="">Kích thước bảng 6x6</a></span></li>
                    </ul>
                </li>
                <li id="Li31" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 Đến 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/4/3/BaiToanGhepO" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/4/4/BaiToanGhepO" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/5/4/BaiToanGhepO" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/4/BaiToanGhepO" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/5/BaiToanGhepO" class="">Kích thước bảng 6x5</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/6/BaiToanGhepO" class="">Kích thước bảng 6x6</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="Li37" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN ĐỌC SỐ</a></span>
            <ul style="display: none;">
                <li id="Li38" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/4/3/BaiToanDocSo" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/4/4/BaiToanDocSo" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi10/5/4/BaiToanDocSo" class="">Kích thước bảng 5x4</a></span></li>
                    </ul>
                </li>
                <li id="Li39" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/4/3/BaiToanDocSo" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/4/4/BaiToanDocSo" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/5/4/BaiToanDocSo" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/6/4/BaiToanDocSo" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi20/6/5/BaiToanDocSo" class="">Kích thước bảng 6x5</a></span></li>
                    </ul>
                </li>
                <li id="Li40" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/4/3/BaiToanDocSo" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/4/4/BaiToanDocSo" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/5/4/BaiToanDocSo" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/4/BaiToanDocSo" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/5/BaiToanDocSo" class="">Kích thước bảng 6x5</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30/6/6/BaiToanDocSo" class="">Kích thước bảng 6x6</a></span></li>
                    </ul>
                </li>
                <li id="Li41" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 Đến 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/4/3/BaiToanDocSo" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/4/4/BaiToanDocSo" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/5/4/BaiToanDocSo" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/4/BaiToanDocSo" class="">Kích thước bảng 6x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/5/BaiToanDocSo" class="">Kích thước bảng 6x5</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanGhepO/CLS1847290691/PhamVi30Den100/6/6/BaiToanDocSo" class="">Kích thước bảng 6x6</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>
        <li id="Li32" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">SẮP XẾP CÁC GIÁ TRỊ TRONG BẢNG</a></span>
            <ul style="display: none;">
                <li id="Li33" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi10/3/3/BaiToanSapXepBang" class="">Kích thước bảng 3x3</a></span></li>
                    </ul>
                </li>
                <li id="Li34" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi20/3/3/BaiToanSapXepBang" class="">Kích thước bảng 3x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi20/4/3/BaiToanSapXepBang" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi20/4/4/BaiToanSapXepBang" class="">Kích thước bảng 4x4</a></span></li>
                    </ul>
                </li>
                <li id="Li35" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30/3/3/BaiToanSapXepBang" class="">Kích thước bảng 3x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30/4/3/BaiToanSapXepBang" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30/4/4/BaiToanSapXepBang" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30/5/4/BaiToanSapXepBang" class="">Kích thước bảng 5x4</a></span></li>
                    </ul>
                </li>
                <li id="Li36" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 Đến 100</a></span>
                    <ul style="display: none;">
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30Den100/3/3/BaiToanSapXepBang" class="">Kích thước bảng 3x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30Den100/4/3/BaiToanSapXepBang" class="">Kích thước bảng 4x3</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30Den100/4/4/BaiToanSapXepBang" class="">Kích thước bảng 4x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30Den100/5/4/BaiToanSapXepBang" class="">Kích thước bảng 5x4</a></span></li>
                        <li class="unselect"><span class="file"><a href="/BaiToanGhepO/DanhSachBaiToanSapXepBang/CLS1847290691/PhamVi30Den100/5/5/BaiToanSapXepBang" class="">Kích thước bảng 5x5</a></span></li>
                    </ul>
                </li>
            </ul>
        </li>

        <li id="Li43" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN TÌM SỐ CÒN THIẾU</a></span>
            <ul style="display: none;">
                <li id="Li48" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 10</a></span>
                    <ul style="display: none;">
                        <li id="Li52" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BA SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaSoDangNam" class="">Dạng 5</a></span></li>
                            </ul>
                        </li>
                        <li id="Li53" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BỐN SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangMuoi" class="">Dạng 10</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangMuoiMot" class="">Dạng 11</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangMuoiHai" class="">Dạng 12</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBonSoDangMuoiBa" class="">Dạng 13</a></span></li>
                            </ul>
                        </li>
                        <li id="Li60" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN NĂM SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanNamSoDangMuoi" class="">Dạng 10</a></span></li>
                            </ul>
                        </li>
                        <li id="Li64" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BẢY SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaySoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi10/BaiToanBaySoDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li id="Li49" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 20</a></span>
                    <ul style="display: none;">
                        <li id="Li55" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BA SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaSoDangNam" class="">Dạng 5</a></span></li>
                            </ul>
                        </li>
                        <li id="Li54" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BỐN SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangMuoi" class="">Dạng 10</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangMuoiMot" class="">Dạng 11</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangMuoiHai" class="">Dạng 12</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBonSoDangMuoiBa" class="">Dạng 13</a></span></li>
                            </ul>
                        </li>
                        <li id="Li61" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN NĂM SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanNamSoDangMuoi" class="">Dạng 10</a></span></li>
                            </ul>
                        </li>
                        <li id="Li66" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BẢY SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaySoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi20/BaiToanBaySoDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li id="Li50" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30</a></span>
                    <ul style="display: none;">
                        <li id="Li56" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BA SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaSoDangNam" class="">Dạng 5</a></span></li>
                            </ul>
                        </li>
                        <li id="Li57" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BỐN SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangMuoi" class="">Dạng 10</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangMuoiMot" class="">Dạng 11</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangMuoiHai" class="">Dạng 12</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBonSoDangMuoiBa" class="">Dạng 13</a></span></li>
                            </ul>
                        </li>
                        <li id="Li62" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN NĂM SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanNamSoDangMuoi" class="">Dạng 10</a></span></li>
                            </ul>
                        </li>
                        <li id="Li65" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BẢY SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaySoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30/BaiToanBaySoDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                    </ul>
                </li>
                <li id="Li51" class="unselect expandable">
                    <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                    <span class="folder"><a href="#" class="">PHẠM VI 30 Đến 100</a></span>
                    <ul style="display: none;">
                        <li id="Li58" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BA SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaSoDangNam" class="">Dạng 5</a></span></li>
                            </ul>
                        </li>
                        <li id="Li59" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BỐN SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangMuoi" class="">Dạng 10</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangMuoiMot" class="">Dạng 11</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangMuoiHai" class="">Dạng 12</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBonSoDangMuoiBa" class="">Dạng 13</a></span></li>
                            </ul>
                        </li>
                        <li id="Li63" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN NĂM SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangHai" class="">Dạng 2</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangBa" class="">Dạng 3</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangBon" class="">Dạng 4</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangNam" class="">Dạng 5</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangSau" class="">Dạng 6</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangBay" class="">Dạng 7</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangTam" class="">Dạng 8</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangChin" class="">Dạng 9</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanNamSoDangMuoi" class="">Dạng 10</a></span></li>
                            </ul>
                        </li>
                        <li id="Li67" class="unselect expandable">
                            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
                            <span class="folder"><a href="#" class="">BÀI TOÁN BẢY SỐ</a></span>
                            <ul style="display: none;">
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaySoDangMot" class="">Dạng 1</a></span></li>
                                <li class="unselect"><span class="file"><a href="/BaiToanTimSo/DanhSachBaiToanTimSo/CLS1847290691/PhamVi30Den100/BaiToanBaySoDangHai" class="">Dạng 2</a></span></li>
                            </ul>
                        </li>
                    </ul>
                </li>
            </ul>
        </li>

        <li id="Li68" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="#" class="">BÀI TOÁN ĐẾM HÌNH</a></span>
            <ul style="display: none;">
                <li class="unselect"><span class="file"><a href="/BaiToanDemHinh/DanhSachBaiToanDemHinh/CLS1847290691/DemDoanThang" class="">Đếm đoạn thẳng</a></span></li>
                <li class="unselect"><span class="file"><a href="/BaiToanDemHinh/DanhSachBaiToanDemHinh/CLS1847290691/DemTamGiac" class="">Đếm tam giác</a></span></li>
                <li class="unselect"><span class="file"><a href="/BaiToanDemHinh/DanhSachBaiToanDemHinh/CLS1847290691/DemTuGiac" class="">Đếm tứ giác</a></span></li>
            </ul>
        </li>
    </ul>
</div>

        