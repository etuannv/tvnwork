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
            <span class="folder"><a href="/noidung/gioithieu/Lists/GioiThieuChung/View_Detail.aspx?ItemID=19" class="">GIỚI THIỆU</a> <i>(3)</i></span>
            <ul style="display: none;">
                <li id="153" class="unselect"><span class="file"><a href="/noidung/gioithieu/Lists/GioiThieuChung/View_Detail.aspx?ItemID=22" class="">Kinh tế - Văn hóa - Xã hội</a></span></li>
                <li id="155" class="unselect last"><span class="file"><a href="/noidung/gioithieu/Lists/GioiThieuChung/View_Detail.aspx?ItemID=20" class="">Lịch sử phát triển</a></span></li>
            </ul>
        </li>
        <li id="151" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="/noidung/tintuc/Pages/default.aspx" class="">TIN TỨC</a> <i>(4)</i></span>
            <ul style="display: none;">
                <li id="158" class="unselect"><span class="file"><a href="/noidung/tintuc/Lists/ThoiSuTongHop/View_Detail.aspx" class="">Thời sự tổng hợp</a></span></li>
                <li id="185" class="unselect"><span class="file"><a href="#" class="selected">An ninh quốc phòng</a></span></li>
                <li id="159" class="unselect"><span class="file"><a href="/noidung/tintuc/Lists/KinhTe/View_Detail.aspx" class="">Kinh tế chính trị</a></span></li>
                <li id="186" class="unselect last"><span class="file"><a href="/noidung/tintuc/Lists/VanHoaXaHoi/View_Detail.aspx" class="">Văn hóa xã hội</a></span></li>
            </ul>
        </li>
        <li id="172" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="/noidung/he-thong-chinh-tri/Lists/HeThongChinhTri/View_Detail.aspx?ItemId=1" class="">HỆ THỐNG CHÍNH TRỊ</a> <i>(3)</i></span>
            <ul style="display: none;">
                <li id="182" class="unselect"><span class="file"><a href="/noidung/he-thong-chinh-tri/Lists/HeThongChinhTri/View_Detail.aspx?ItemID=1">Huyện ủy</a></span></li>
                <li id="183" class="unselect"><span class="file"><a href="/noidung/he-thong-chinh-tri/Lists/HeThongChinhTri/View_Detail.aspx?ItemID=2">Ủy ban nhân dân</a></span></li>
                <li id="184" class="unselect last"><span class="file"><a href="/noidung/he-thong-chinh-tri/Lists/HeThongChinhTri/View_Detail.aspx?ItemID=3">Ủy ban MTTQ</a></span></li>
            </ul>
        </li>

        <li id="171" class="unselect expandable">
            <div class="hitarea unselect-hitarea expandable-hitarea"></div>
            <span class="folder"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx" class="">DÀNH CHO DU KHÁCH</a> <i>(5)</i></span>
            <ul style="display: none;">
                <li class="unselect"><span class="file"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx?ItemID=2" class="">Tuyến địa điểm du lịch</a></span></li>
                <li class="unselect"><span class="file"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx?ItemID=3" class="">Di tích lễ hội</a></span></li>
                <li class="unselect"><span class="file"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx?ItemID=3" class="">Ẩm thực</a></span></li>
                <li class="unselect"><span class="file"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx?ItemID=5" class="">Vui chơi giải trí</a></span></li>
                <li class="unselect last"><span class="file"><a href="/noidung/danh-cho-du-khach/Lists/DanhChoDuKhach/View_Detail.aspx?ItemID=6" class="">Văn học nghệ thuật</a></span></li>
            </ul>
        </li>
        <li class="unselect expandable lastExpandable">    
            <div class="hitarea unselect-hitarea expandable-hitarea lastExpandable-hitarea"></div>
            <span class="folder"><a href="/noidung/quy-hoach/Lists/QuyHoach/View_Detail.aspx" class="">QUY HOẠCH</a> <i>(4)</i></span>
            <ul style="display: none;">
                <li class="unselect"><span class="file"><a href="/noidung/quy-hoach/Lists/QuyHoach/View_Detail.aspx?ItemID=2">Kinh tế xã hội</a></span></li>
                <li class="unselect"><span class="file"><a href="/noidung/quy-hoach/Lists/QuyHoach/View_Detail.aspx?ItemID=3">Nông thôn mới</a></span></li>
                <li id="175" class="unselect"><span class="file"><a href="/noidung/quy-hoach/Lists/QuyHoach/View_Detail.aspx?ItemID=4">Du lịch và làng nghề</a></span></li>
                <li id="176" class="unselect last"><span class="file"><a href="/noidung/quy-hoach/Lists/QuyHoach/View_Detail.aspx?ItemID=5">Dành cho các nhà đầu tư</a></span></li>
            </ul>
         </li>
    </ul>
</div>

        