<%@ Page Language="C#" MasterPageFile="~/Views/Shared/FontEnd.Master" Inherits="System.Web.Mvc.ViewPage<TNV.Web.Models.LogOnModel>" %>

<asp:Content ID="MainContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="banner-hold" style="z-index: 0;">
        <div id="notification" class="container">
        </div>
        <div class="banner-holder">
            <div class="container banner">
                <div class="hidden-phone hidden-tablet text-center">
                    <img src="/Content/font-end/img/myolm.png" id="main-logo" title="Online Math"><br>
                    <div class="user-cmd">
                        <a class="btn btn-primary btn-large" href="http://olm.vn/index.php?l=user.register">
                            Đăng ký tài khoản</a> hoặc <a style="line-height: 32px; padding: 4px 18px; padding-bottom: 6px;"
                                class="btn btn-danger btn-large" href="http://olm.vn/index.php?g=user.gpluslogin&amp;r                    edirect=true">
                                <img class="img" style="margin-top: -2px; width: 30px; height: 30px; border-radius: 4px;"
                                    src="/Content/font-end/img/gplus.png">
                                Đăng nhập bằng Google</a>
                        <br class="clear">
                    </div>
                </div>
                <form class="hidden-desktop home-form" action="http://olm.vn/?g=user.login" method="post">
                <input name="username" placeholder="Tên đăng nhập Online Math" type="text">
                <input id="password" name="password" placeholder="Mật khẩu" type="password">
                <div class="text-center">
                    <label>
                        <a href="http://olm.vn/?l=user.forgot">Quên mật khẩu</a></label>
                    <label class="checkbox">
                        <span style="display: inline-block;">
                            <input checked="checked" name="remember" value="yes" type="checkbox">
                            Ghi nhớ đăng nhập</span>
                    </label>
                    <label>
                        <a href="http://olm.vn/?l=user.gpluslogin&amp;redirect=true">
                            <img class="img" style="width: 28px; height: 28px; border-radius: 4px;" src="/Content/font-end/img/gplus.png">
                            Đăng nhập bằng Google</a>
                    </label>
                    <button type="submit" class="btn btn-primary">
                        Đăng nhập</button>
                    <a class="btn btn-success" href="http://olm.vn/?l=user.register">Đăng ký</a>
                </div>
                <input name="logintask" value="1" type="hidden">
                <input name="return" value="http%3A%2F%2Folm.vn%2F" type="hidden">
                </form>
            </div>
        </div>
    </div>
    <div class="container action">
        <div class="container banner">
            <div class="row" style="margin-bottom: 20px;">
                <h1 class="text-center" style="color: #f30;">
                    Tự học toán với Online Math</h1>
                <div class="span2">
                    <a href="http://olm.vn/toan-mau-giao" title="Luyện toán mẫu giáo" class="grade grad-pink">
                        <span class="grade-lop">Mẫu giáo</span><span class="grade-number">0</span></a></div>
                <div class="span2">
                    <a href="/FLuyenToanLopMot/FDanhSachToanLopMot" title="Toán lớp 1" class="grade grad-green">
                        <span class="grade-lop">Lớp</span><span class="grade-number">1</span></a></div>
                <div class="span2">
                    <a href="/FLuyenToanLopHai/FDanhSachToanLopHai" title="Toán lớp 2" class="grade grad-red">
                        <span class="grade-lop">Lớp</span><span class="grade-number">2</span></a></div>
                <div class="span2">
                    <a href="/FLuyenToanLopBa/FDanhSachToanLopBa" title="Toán lớp 3" class="grade grad-orange">
                        <span class="grade-lop">Lớp</span><span class="grade-number">3</span></a></div>
                <div class="span2">
                    <a href="/FLuyenToanLopBon/FDanhSachToanLopBon" title="Toán lớp 4" class="grade grad-blue">
                        <span class="grade-lop">Lớp</span><span class="grade-number">4</span></a></div>
                <div class="span2">
                    <a href="/FLuyenToanLopNam/FDanhSachToanLopNam" title="Toán lớp 5" class="grade grad-green2">
                        <span class="grade-lop">Lớp</span><span class="grade-number">5</span></a></div>
            </div>
        </div>
        <div class="row">
            <div class="span6 " style="">
                <div class="content-box">
                    <div class="content-title grad-green2">
                        <a href="http://olm.vn/hoi-dap">Giúp tôi giải toán</a>
                    </div>
                    <div class="feature">
                        <div class="scroll">
                            <div class="alert">
                                Học thày chẳng tày học bạn <a href="http://olm.vn/hoi-dap/create" style="color: #fff;"
                                    class="btn btn-warning btn-small btn-close">Gửi câu hỏi</a>
                            </div>
                            <div class="media qa-media">
                                <div class="pull-left">
                                    <div>
                                        <img style="width: 50px; height: 50px;" class="avatar-small" src="/Content/font-end/img/s1.jpg"></div>
                                </div>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href="http://olm.vn/thanhvien/tu4a1cvt13">hoang nguyen anh tu</a></h4>
                                    <div class="qa-content" style="position: relative;">
                                        <a href="http://olm.vn/hoi-dap/question/62906.html">
                                            <p>
                                                Đội văn nghệ của trường có số bạn nam bằng 2/3 số bạn nữ. Nếu số bạn nữ tăng lên
                                                20 bạn và số bạn nam tăng lên 5 bạn thì số bạn nữ gấp 2 lần số bạn nam. Tính số
                                                bạn nam và số bạn nữ.</p>
                                        </a><a href="http://olm.vn/hoi-dap/question/62906.html?auto=1" class="btn btn-small btn-success btn-hover"
                                            style="position: absolute; bottom: 3px; left: 0px;">Trả lời</a>
                                        <div style="text-align: right; margin-top: 5px; padding: 3px; color: #090;">
                                            9 câu trả lời</div>
                                    </div>
                                </div>
                            </div>
                            <div class="media qa-media">
                                <div class="pull-left">
                                    <div>
                                        <img style="width: 50px; height: 50px;" class="avatar-small" src="/Content/font-end/img/avt165927_60by60.jpg"></div>
                                </div>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href="http://olm.vn/thanhvien/thienkhanhming">Lâm Thiện Khánh</a></h4>
                                    <div class="qa-content" style="position: relative;">
                                        <a href="http://olm.vn/hoi-dap/question/62671.html">
                                            <p>
                                                Một hình chữ nhật,nếu tăng chiều rộng đã cho bằng chiều dài thì diện tích đó tăng
                                                thêm 20m vuông,còn khi giảm chiều dài cho chiều rộng thì diện tích giảm 16m vuông.Tính
                                                diện tích của hình chữ nhật ?</p>
                                        </a><a href="http://olm.vn/hoi-dap/question/62671.html?auto=1" class="btn btn-small btn-success btn-hover"
                                            style="position: absolute; bottom: 3px; left: 0px;">Trả lời</a>
                                        <div style="text-align: right; margin-top: 5px; padding: 3px; color: #090;">
                                            9 câu trả lời</div>
                                    </div>
                                </div>
                            </div>
                            <div class="media qa-media">
                                <div class="pull-left">
                                    <div>
                                        <img style="width: 50px; height: 50px;" class="avatar-small" src="/Content/font-end/img/s5.jpg"></div>
                                </div>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href="http://olm.vn/thanhvien/ankhanh2909">Phạm An Khánh</a></h4>
                                    <div class="qa-content" style="position: relative;">
                                        <a href="http://olm.vn/hoi-dap/question/62385.html">
                                            <p>
                                                Chứng minh rằng trong 6 số tự nhiên bất kỳ có 3 chữ số ta luôn tìm được hai số để
                                                khi viết liền nhau thành một số có sáu chữ số và chia hết cho 7.</p>
                                        </a><a href="http://olm.vn/hoi-dap/question/62385.html?auto=1" class="btn btn-small btn-success btn-hover"
                                            style="position: absolute; bottom: 3px; left: 0px;">Trả lời</a>
                                        <div style="text-align: right; margin-top: 5px; padding: 3px; color: #090;">
                                            4 câu trả lời</div>
                                    </div>
                                </div>
                            </div>
                            <div class="media qa-media">
                                <div class="pull-left">
                                    <div>
                                        <img style="width: 50px; height: 50px;" class="avatar-small" src="/Content/font-end/img/s3.jpg"></div>
                                </div>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href="http://olm.vn/thanhvien/matto2005">Đỗ Thị Thảo Hiền</a></h4>
                                    <div class="qa-content" style="position: relative;">
                                        <a href="http://olm.vn/hoi-dap/question/62236.html">
                                            <p>
                                                Lớp 4A có 29 hs,lớp4B có 35 hs.Biết số hs trai của cả hai lớp bằng nhau,số hs gái
                                                lớp 4A bằng 2/3&nbsp;số hs gái lớp 4B.Tính số hs trái hs gái mỗi lớp?</p>
                                        </a><a href="http://olm.vn/hoi-dap/question/62236.html?auto=1" class="btn btn-small btn-success btn-hover"
                                            style="position: absolute; bottom: 3px; left: 0px;">Trả lời</a>
                                        <div style="text-align: right; margin-top: 5px; padding: 3px; color: #090;">
                                            9 câu trả lời</div>
                                    </div>
                                </div>
                            </div>
                            <div class="media qa-media">
                                <div class="pull-left">
                                    <div>
                                        <img style="width: 50px; height: 50px;" class="avatar-small" src="/Content/font-end/img/avt125676_60by60.jpg"></div>
                                </div>
                                <div class="media-body">
                                    <h4 class="media-heading">
                                        <a href="http://olm.vn/thanhvien/mauhyun2014@gmail.com">hyun mau</a></h4>
                                    <div class="qa-content" style="position: relative;">
                                        <a href="http://olm.vn/hoi-dap/question/62049.html">
                                            <p>
                                                tìm số có 3 chữ số <u>abc </u>biết tích&nbsp;a.b.c= a+b+c</p>
                                        </a><a href="http://olm.vn/hoi-dap/question/62049.html?auto=1" class="btn btn-small btn-success btn-hover"
                                            style="position: absolute; bottom: 3px; left: 0px;">Trả lời</a>
                                        <div style="text-align: right; margin-top: 5px; padding: 3px; color: #090;">
                                            44 câu trả lời</div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span6 ct-block">
                <div class="content-box">
                    <div class="content-title grad-pink text-center">
                        <a href="http://olm.vn/Toan-vui-hang-tuan">Toán vui mỗi tuần</a>
                    </div>
                    <div class="feature">
                        <div class="scroll">
                            <div class="alert">
                                Giải thưởng 1 tháng VIP đang chờ bạn <a href="http://olm.vn/tin-tuc/Bai-toan-44.html"
                                    style="color: #fff;" class="btn btn-warning btn-small btn-close">Gửi lời giải</a></div>
                            <h2>
                                <a href="http://olm.vn/tin-tuc/Bai-toan-44.html">Bài toán 44</a><a></a></h2>
                            <div>
                                <p>
                                    <a><span style="display: inline-block" class="svgedit">
                                        <svg width="380" height="174" xmlns="http://www.w3.org/2000/svg"> <!-- Created with Method Draw - http://github.com/duopixel/Method-Draw/ --> <g><title></title> <rect x="-1" y="-1" width="382" height="176" id="canvas_background" fill="#fff"></rect> <g id="canvasGrid" display="none"> <rect id="svg_13" width="100%" height="100%" x="0" y="0" stroke-width="0" fill="url(#gridpattern)"></rect> </g> </g> <g><title></title> <line fill="none" stroke="#000" stroke-width="2.5" x1="38" y1="42" x2="341.16497" y2="42" id="svg_1" stroke-linejoin="undefined" stroke-linecap="undefined"></line> <line fill="none" stroke-width="2.5" x1="44" y1="113" x2="186" y2="99" id="svg_2" stroke-linejoin="undefined" stroke-linecap="undefined" stroke="#000"></line> <line fill="none" stroke="#000" stroke-width="2.5" x1="323" y1="118" x2="185" y2="99" id="svg_3" stroke-linejoin="undefined" stroke-linecap="undefined"></line> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="13" y="46" id="svg_4" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">A</text> <text fill="#000000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="353" y="44" id="svg_5" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve" stroke="#000">B</text> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="11" y="120" id="svg_6" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">C</text> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="342" y="123" id="svg_7" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">D</text> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="179" y="92" id="svg_8" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">E</text> <rect fill="none" stroke-width="2.5" stroke-opacity="null" fill-opacity="null" x="38" y="14" width="24" height="26" id="svg_9" stroke="#000"></rect> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="44" y="34" id="svg_10" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">1</text> <rect fill="none" stroke="#000" stroke-width="2.5" stroke-opacity="null" fill-opacity="null" x="42" y="83" width="28" height="28" id="svg_11" transform="rotate(-7 56.000000000000355,97.0000000000001) "></rect> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="51" y="104" id="svg_12" font-size="19" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve" transform="rotate(-9 56.28906249999988,97.50000000000001) ">2</text> <line stroke="#000" fill="none" stroke-opacity="null" fill-opacity="null" x1="63" y1="28" x2="96" y2="28" id="svg_14" stroke-linejoin="null" stroke-linecap="null" stroke-dasharray="2,2"></line> <line fill="none" stroke="#000" stroke-dasharray="2,2" stroke-opacity="null" fill-opacity="null" x1="88" y1="21" x2="98" y2="28" id="svg_15" stroke-linejoin="null" stroke-linecap="null"></line> <line fill="none" stroke="#000" stroke-width="null" stroke-dasharray="2,2" stroke-opacity="null" fill-opacity="null" x1="97" y1="28" x2="90" y2="36" id="svg_16" stroke-linejoin="null" stroke-linecap="null"></line> <line fill="none" stroke-width="null" stroke-dasharray="2,2" stroke-opacity="null" fill-opacity="null" x1="69" y1="97" x2="105" y2="92" id="svg_17" stroke-linejoin="null" stroke-linecap="null" stroke="#000"></line> <line fill="none" stroke="#000" stroke-width="null" stroke-dasharray="2,2" stroke-opacity="null" fill-opacity="null" x1="93" y1="89" x2="101" y2="92" id="svg_18" stroke-linejoin="null" stroke-linecap="null"></line> <line fill="none" stroke="#000" stroke-width="null" stroke-dasharray="2,2" stroke-opacity="null" fill-opacity="null" x1="100" y1="92" x2="95" y2="98" id="svg_19" stroke-linejoin="null" stroke-linecap="null"></line> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="157" y="33" id="svg_20" font-size="16" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">25 km/h</text> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="109" y="98" id="svg_21" font-size="16" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve" transform="rotate(-6 137.46093749999991,92.50000000000011) ">20 km/h</text> <text fill="#000000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="233" y="102" id="svg_22" font-size="16" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve" transform="rotate(9 261.4609375000001,96.49999999999974) " stroke="#000">30 km/h</text> <text fill="#000000" stroke="#000" stroke-width="0" stroke-opacity="null" fill-opacity="null" x="85" y="150" id="svg_23" font-size="16" font-family="Helvetica, Arial, sans-serif" text-anchor="left" xml:space="preserve">AB = CE + ED, CE = ED</text> </g> </svg>
                                    </span></a>
                                </p>
                                <p>
                                    <a>Ô tô thứ nhất đi trên đoạn đường phẳng từ A đến B với vận tốc 25 km/h. Ô tô thứ hai
                                        đi từ C đến D qua ngọn đồi, khi lên dốc (đoạn CE) với vận tốc 20 km/h, khi xuống
                                        dốc (đoạn ED) với vận tốc 30 km/h. Biết tổng quãng đường hai ô tô đi như nhau (AB
                                        = CE + ED) và biết độ dài&nbsp;đoạn lên dốc bằng độ dài đoạn xuống dốc (CE = ED).
                                        Hỏi&nbsp;xe nào đến đích nhanh hơn nếu chúng cùng xuất phát tại một thời điểm? Vì
                                        sao?</a></p>
                                <p>
                                    <a>Bạn trình bày đáp án của mình vào ô&nbsp;Gửi ý kiến bên dưới. Đáp án của các bạn
                                        sẽ được công bố vào Thứ Sáu, ngày 3/4/2015. Ba bạn có lời giải hay và sớm nhất sẽ
                                        được tặng 1 tháng VIP của OnlineMath. Câu đố tiếp theo sẽ lên mạng vào Thứ Bảy,
                                        ngày 4/4/2015.</a></p>
                                <p>
                                    <a>Gợi ý: Vận tốc trung bình của xe thứ hai trên cả đoạn đường từ C đến D là bao nhiêu?</a></p>
                            </div>
                            <a></a>
                        </div>
                        <a></a>
                    </div>
                    <a></a>
                </div>
                <a></a>
            </div>
            <a>
                <br class="clear">
            </a>
        </div>
        <a></a>
        <div class="row">
            <a></a>
            <div class="span6 " style="">
                <a></a>
                <div class="content-box">
                    <a></a>
                    <div class="content-title grad-blue">
                        <a></a><a href="http://olm.vn/thongtin">Tin tức - sự kiện</a>
                    </div>
                    <div class="feature">
                        <div>
                            <a href="http://olm.vn/tin-tuc/Cau-hoi-thuong-gap.html">
                                <img class="thumb" src="/Content/font-end/img/olm_faq.png">
                                <div>
                                    <h2>
                                        Câu hỏi thường gặp</h2>
                                    Chúng tôi nhận được rất nhiều câu hỏi và ý kiến đóng góp từ phía phụ huynh, học
                                    sinh và các thầy cô giáo. Các ý kiến phản hồi là nguồn động viên to lớn cho những
                                    người làm ra OnlineMath. Sau đây là các câu hỏi thường gặp liên quan đến sử dụng
                                    OnlineMath.
                                </div>
                            </a>
                            <br class="clear">
                        </div>
                        <ul class="content-list">
                            <li><a href="http://olm.vn/tin-tuc/Gop-y.html" class="block-link">Góp ý</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Cau-tao-so.html" class="block-link">Cấu tạo số</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Qui-dinh-moi-ve-danh-gia-hoc-sinh-tieu-hoc.html"
                                class="block-link">Qui định mới về đánh giá học sinh tiểu học</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Gioi-thieu-trang-web-luyen-thi-tot-nghiep-THPT-va-tuyen-sinh-2015.html"
                                class="block-link">Giới thiệu trang web luyện thi tốt nghiệp THPT và tuyển sinh
                                2015</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Gioi-thieu-ve-Online-Math.html" class="block-link">
                                Giới thiệu về Online Math</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Phuong-phap-tinh-nguoc.html" class="block-link">Phương
                                pháp tính ngược</a></li>
                            <li><a href="http://olm.vn/tin-tuc/Cach-soan-cong-thuc-toan-ve-hinh-va-so-do-Chuyen-muc-Giup-toi-giai-toan.html"
                                class="block-link">Cách soạn công thức toán, vẽ hình và sơ đồ - Chuyên mục Giúp
                                tôi giải toán</a></li>
                        </ul>
                        <p class="text-center">
                            <a href="http://olm.vn/thongtin" class="btn">Xem thêm tin tức..</a></p>
                    </div>
                </div>
            </div>
            <div class="span6 ct-block hidden-phone">
                <div class="content-box">
                    <div class="content-title grad-orange text-center">
                        <a href="http://olm.vn/contest">Kỳ thi trên Online Math</a>
                    </div>
                    <div class="feature" id="contest-rank">
                        <div class="flip-container">
                            <div class="flipper">
                                <div class="front">
                                    <h2 class="text-center">
                                        Mới dự thi</h2>
                                    <table class="table table-stripped table-bordered">
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/avt181813_60by60.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/thanhlecute">Nguyễn Hồ Thanh Lê</a>
                                                </td>
                                                <td>
                                                    10
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 9 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 1
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/s5.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/trucqpa">truong thi thanh truc</a>
                                                </td>
                                                <td>
                                                    10
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 9 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 2
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/s4.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/chuotnhat2903">nguyen thuy duong</a>
                                                </td>
                                                <td>
                                                    10
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 9 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 1
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/s6.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/tpthu123">phạm thị thu</a>
                                                </td>
                                                <td>
                                                    9
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 9 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 1
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/avt181645_60by60.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/quynhnhu15978">Nguyễn Thị Như Quỳnh</a>
                                                </td>
                                                <td>
                                                    9
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 9 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 2
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <img src="/Content/font-end/img/s6.jpg" class="avt-small">
                                                </td>
                                                <td>
                                                    <a href="http://olm.vn/thanhvien/vanvu_2004">Vu Khanh Van</a>
                                                </td>
                                                <td>
                                                    9
                                                </td>
                                                <td>
                                                    <a>Kiểm tra tháng 12 (2014 - 2015)</a>
                                                </td>
                                                <td>
                                                    Lớp 5
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                                <div class="back text-center">
                                    <h2>
                                        Đang tải dữ liệu...</h2>
                                    <img src="/Content/font-end/img/loader.gif">
                                </div>
                            </div>
                        </div>
                        <div class="text-center" id="contest-ctrl">
                            <button class="btn btn-small btn-warning" data-grade="all">
                                Tất cả</button>
                            <button class="btn btn-small" data-grade="1">
                                Lớp 1</button>
                            <button class="btn btn-small" data-grade="2">
                                Lớp 2</button>
                            <button class="btn btn-small" data-grade="3">
                                Lớp 3</button>
                            <button class="btn btn-small" data-grade="4">
                                Lớp 4</button>
                            <button class="btn btn-small" data-grade="5">
                                Lớp 5</button>
                        </div>
                        <p>
                            <a class="contest-link" href="http://olm.vn/contest">Hãy <u>dự thi ngay</u> để có cơ
                                hội được đăng tên vào trang chính của Online Math!</a></p>
                    </div>
                </div>
            </div>
            <br class="clear">
        </div>
        <div class="container text-slide">
            <div class="grad grad-orange" id="grad" style="border-radius: 16px;">
                <div id="text-slide0" style="padding: 50px 0px; text-align: center; display: block;">
                    <div style="font-size: 30px; font-family: 'Segoe UI Light', SegoeUIWL; line-height: 40px;">
                        Với Online Math
                    </div>
                    <h2 style="font-size: 41px;">
                        Học mà như chơi, chơi mà vẫn học</h2>
                </div>
                <div id="text-slide1" style="padding: 50px 0px; text-align: center; display: none;">
                    <h2 style="font-size: 41px;">
                        Mỗi một ngày học</h2>
                    <h2 style="font-size: 41px;">
                        Là một ngày vui</h2>
                </div>
                <div id="text-slide2" style="padding: 50px 0px; text-align: center; display: none;">
                    <h2 style="font-size: 30px;">
                        Không những học cái đúng, mà cần học cả những điều sai</h2>
                    <h2 style="font-size: 30px;">
                        Nhưng chỉ làm theo điều đúng, không được làm theo cái sai</h2>
                </div>
                <div id="text-slide3" style="padding: 50px 0px; text-align: center; display: none;">
                    <h2 style="font-size: 41px;">
                        Không có học trò dốt</h2>
                    <h2 style="font-size: 41px;">
                        Mà chỉ có thầy chưa giỏi</h2>
                </div>
                <div id="text-slide4" style="padding: 50px 0px; text-align: center; display: none;">
                    <h2 style="font-size: 41px;">
                        Online Math giúp học sinh hứng thú khi làm toán</h2>
                    <div style="font-size: 20px; font-family: 'Segoe UI Light', SegoeUIWL; line-height: 40px;">
                        PGS.TS Nguyễn Văn Trào - Phó Hiệu trưởng - ĐH Sư phạm Hà Nội
                    </div>
                </div>
                <div id="text-slide5" style="padding: 50px 0px; text-align: center; display: none;">
                    <h2 style="font-size: 41px;">
                        Online Math - www.olm.vn</h2>
                    <h2 style="font-size: 41px;">
                        Ngôi trường thứ hai của tất cả các bạn học sinh</h2>
                </div>
            </div>
            <br class="clear">
        </div>
        <script type="text/javascript">
            $(function () {
                // text slider
                var _s_index = 0, grad = ["green2", "blue", "orange", "green", "pink", "red"], _cclass = "green2";
                function nextSlide() {
                    $("#grad").removeClass("grad-" + _cclass);
                    _cclass = grad[_s_index % 5];
                    $("#grad").addClass("grad-" + _cclass);
                    var id = "text-slide" + _s_index % 6;
                    _s_index++;
                    $("#" + id).fadeIn(800).delay(8000).fadeOut(800, nextSlide);
                }
                nextSlide();

                // contest recent list 
                function initPageEvent() {
                    $("#contest-ctrl .btn").click(function () {
                        var that = $(this); that.parent().find(".btn-warning").removeClass("btn-warning")
                        that.addClass("btn-warning");
                        $("#contest-rank").addClass("ani");
                        var grade = that.attr("data-grade");
                        setTimeout(function (str) {
                            $.ajax({
                                url: "?g=content.AjaxContestTable&grade=" + grade,
                                success: function (str) {
                                    var elm = $("#contest-rank>.flip-container");
                                    elm.html(""); elm.parent().removeClass("ani")
                                    elm.html(str);
                                }
                            });
                        }, 1000);
                    });
                }
                initPageEvent();
                // latex
                $("span.math-q").each(function (i, e) {
                    var latex = e.innerHTML.replace(/\\\(|\\\)/g, ""); e.innerHTML = "<img src='http://latex.codecogs.com/gif.latex?" + encodeURI(latex) + "' />";
                });
            }); //
        </script>
        <div class="row">
            <div class="span6 " style="">
                <div class="content-box border">
                    <div class="content-title grad-pink" style="margin-bottom: 10px;">
                        <a href="http://olm.vn/">Thông tin về Online Math</a>
                    </div>
                    <div class="feature">
                        <div class="media">
                            <a class="pull-left" href="">
                                <img style="width: 80px; height: 80px; padding-left: 10px;" class="media-object hidden-phone"
                                    src="/Content/font-end/img/phone.png">
                            </a>
                            <div class="media-body">
                                <div style="padding-left: 5px;">
                                    <h3 class="media-heading" style="font-weight: normal;">
                                        Điện thoại hỗ trợ</h3>
                                    <div style="font-size: 24px; color: #f70; line-height: 30px;">
                                        0942 754209 hoặc 0915 343532</div>
                                </div>
                            </div>
                        </div>
                        <div class="media">
                            <a class="pull-left" href="">
                                <img style="width: 80px; height: 80px; padding-left: 10px;" class="media-object hidden-phone"
                                    src="/Content/font-end/img/email.png">
                            </a>
                            <div class="media-body">
                                <div style="padding-left: 5px;">
                                    <h3 class="media-heading" style="font-weight: normal;">
                                        Địa chỉ liên hệ</h3>
                                    <p style="color: #090;">
                                        Phòng 602 nhà K1, Trường Đại học Sư phạm Hà Nội - số 136 Xuân Thủy, Quận Cầu Giấy,
                                        Tp. Hà Nội. Email: olm@hnue.edu.vn</p>
                                </div>
                            </div>
                        </div>
                        <div class="text-center">
                            <h3 class="segoe">
                                Đơn vị bảo trợ Online Math</h3>
                            <table style="width: 100%;">
                                <tbody>
                                    <tr>
                                        <td>
                                            <div class="sponsor">
                                                <a href="http://ccs1.hnue.edu.vn/">
                                                    <img src="/Content/font-end/img/ccs.png"></a>
                                                <div class="caption">
                                                    <a href="http://ccs1.hnue.edu.vn/">TT. Khoa học<br>
                                                        tính toán</a></div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="sponsor">
                                                <a href="http://hnue.edu.vn/">
                                                    <img src="/Content/font-end/img/hnue.png"></a>
                                                <div class="caption">
                                                    <a href="http://hnue.edu.vn/">Đại học Sư phạm Hà Nội</a></div>
                                            </div>
                                        </td>
                                        <td>
                                            <div class="sponsor">
                                                <a href="http://crtp.hnue.edu.vn/">
                                                    <img src="/Content/font-end/img/crtp1.png"></a>
                                                <div class="caption">
                                                    <a href="http://crtp.hnue.edu.vn/">TT. Nghiên cứu &amp; Sản xuất Học liệu</a></div>
                                            </div>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
            <div class="span6">
                <div class="content-box">
                    <div class="content-title grad-blue" style="margin-bottom: 10px;">
                        <a href="http://www.facebook.com/olm.vn">Online Math trên Facebook</a>
                    </div>
                    <div class="feature text-center">
                        <h3 class="segoe">
                            Đang online: 699, số thành viên: 181430
                        </h3>
                        facebook here
                    </div>
                </div>
            </div>
            <br class="clear">
        </div>
    </div>
</asp:Content>
