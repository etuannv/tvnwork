<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

 <div class="menu">
        <ul id="nav">
			<li class="u">
				<a href="/Home/Index" class="parent">Trang chủ</a>
			</li>
		    <li><a href="#" class="parent"><span>Quản lý hệ thống</span></a>
                <ul class="icesubMenu">
                    <li><a href="/Home/UserManager"><span>Quản lý người dùng</span></a></li>
                    <li><a href="/ProDis/ProvinceList"><span>Danh sách các tỉnh</span></a></li>
                    <li><a href="/ProDis/DistrictList"><span>Danh sách các huyện</span></a></li>
                    <li><a href="/ProDis/SchoolList"><span>Danh sách trường học</span></a></li>
                    <li><a href="/SystemManager/ClassList"><span>Danh mục khối lớp</span></a></li>
                    <li><a href="/SystemManager/MathKindList"><span>Danh mục dạng toán</span></a></li>
                    <li><a href="/BackupRetore/DataBackup"><span>Sao lưu dữ liệu</span></a></li>
                    <li><a href="/BackupRetore/DataRestore"><span>Khôi phục dữ liệu</span></a></li>
                </ul>
            </li>
            <li><a href="#" class="parent"><span>Quản lý tin bài</span></a>
                <ul class="icesubMenu">
                    <li><a href="/NewsWebManager/NewsCategoryList"><span>Quản lý chuyên mục</span></a></li>
                    <li><a href="/NewsWebManager/ListNews"><span>Quản lý đăng tin bài</span></a></li>
                </ul>
            </li>
            <li><a href="#" class="parent"><span>Toán cơ bản</span></a>
                <ul class="icesubMenu">
                    <li><a href="/ToanCoBanLopMot/QuanTriToanCoBanLopMot"><span>Toán cơ bản lớp 1</span></a></li>
                    <li><a href="#"><span>Toán cơ bản lớp 2</span></a></li>
                    <li><a href="#"><span>Toán cơ bản lớp 3</span></a></li>
                    <li><a href="#"><span>Toán cơ bản lớp 4</span></a></li>
                    <li><a href="#"><span>Toán cơ bản lớp 5</span></a></li>
                </ul>
            </li>
            <li><a href="#" class="parent"><span>Toán nâng cao</span></a>
                <ul class="icesubMenu">
                    <li><a href="#"><span>Toán nâng cao lớp 1</span></a></li>
                    <li><a href="#"><span>Toán nâng cao lớp 2</span></a></li>
                    <li><a href="#"><span>Toán nâng cao lớp 3</span></a></li>
                    <li><a href="#"><span>Toán nâng cao lớp 4</span></a></li>
                    <li><a href="#"><span>Toán nâng cao lớp 5</span></a></li>
                </ul>
            </li>
            <li><a href="#" class="parent"><span>Toán thông minh</span></a>
                <ul class="icesubMenu">
                    <li><a href="/CleverMathManager/CleverMathKind"><span>Danh mục dạng toán</span></a></li>
                    <li><a href="/CleverMathManager/CleverExerKind"><span>Danh mục loại bài tập</span></a></li>
                    <li><a href="#"><span>Danh sách các bài tập</span></a></li>
                </ul>
            </li>
            <li><a href="#" class="parent"><span>Luyện thi toán</span></a>
                <ul class="icesubMenu">
                    <li><a href="#"><span>Luyện thi toán lớp 1</span></a></li>
                    <li><a href="#"><span>Luyện thi toán lớp 2</span></a></li>
                    <li><a href="#"><span>Luyện thi toán lớp 3</span></a></li>
                    <li><a href="#"><span>Luyện thi toán lớp 4</span></a></li>
                    <li><a href="#"><span>Luyện thi toán lớp 5</span></a></li>
                </ul>
            </li>
            <li><a href="#"><span>Trắc nghiệm toán thông minh</span></a>
                <ul class="icesubMenu">
                    <li><a href="#"><span>Kiểm tra IQ</span></a></li>
                    <li><a href="#"><span>Phát triển thông minh</span></a></li>
                </ul>
            </li>
        </ul>
    </div>
                                    