function DrawClock(HourId, MinId, SecId, HourValue, MinValue, SecValue) {
    setInterval(function () 
    {
        var seconds = SecValue;
        var sdegree = seconds * 6;
        var srotate = "rotate(" + sdegree + "deg)";

        $("#" + SecId).css({ "-moz-transform": srotate, "-webkit-transform": srotate });

    }, 1000);
    setInterval(function () {
        var hours = HourValue;
        var mins = MinValue;
        var hdegree = hours * 30 + (mins / 2);
        var hrotate = "rotate(" + hdegree + "deg)";

        $("#" + HourId).css({ "-moz-transform": hrotate, "-webkit-transform": hrotate });

    }, 1000);
    setInterval(function () {
        var mins = MinValue;
        var mdegree = mins * 6;
        var mrotate = "rotate(" + mdegree + "deg)";
        $("#" + MinId).css({ "-moz-transform": mrotate, "-webkit-transform": mrotate });
    }, 1000);
}

function EnabelTab(obj, objid, selected) {
    var arr = explode('~', obj);
    var arrid = explode('~', objid);
    var idItem = '';
    var Item = ''
    for (var i = 0; i < arr.length; i++) {
        idItem = '#' + Trim(arrid[i]) + ' a';
        Item = Trim(arr[i]);
        if (Trim(arr[i]) == Trim(selected)) {
            $(idItem).addClass("acvited");
            document.getElementById(Item).style.display = 'block';
        }
        else {
            $(idItem).removeClass("acvited");
            document.getElementById(Item).style.display = 'none';
        }
    }
}

function Printer(witdh) 
{
    var disp_setting = 'toolbar=no,location=no,directories=yes,menubar=no,';
    disp_setting += 'scrollbars=yes,width=750, height=600, left=100, top=25';
    var content_vlue = document.getElementById('print_content').innerHTML;
    var docprint = window.open('', '', disp_setting);
    docprint.document.open();
    docprint.document.write('<html><head>');
    docprint.document.write('</head><body onLoad="self.print()"><center>');
    docprint.document.write('<div style="width:' + witdh + '">');
    docprint.document.write(content_vlue);
    docprint.document.write('</div>');
    docprint.document.write('</center></body></html>');
    docprint.document.close();
    docprint.focus();
}

function isNumber(n) {

    var checkNumber = false;
    if (Trim(n) == '') {
        checkNumber = true
    }else if (!isNaN(parseFloat(GetNumFromFormat(n))) && isFinite(GetNumFromFormat(n))) {
        checkNumber = true;
    }
    if (!checkNumber) {
        alert('Bạn nhập sai định dạng số!');
   }
    return checkNumber;
}

function isNumberInt(n) {
    var checkNumber = false;
    if (Trim(n) == '') {
        checkNumber = true
    } else if (!isNaN(parseFloat(n)) && isFinite(n)) {
        if (parseFloat(n) == parseInt(n)) {
            checkNumber = true;
        }
    }
    if (!checkNumber) 
    {
        alert('Bạn phải nhập số nguyên!');
    }

    return checkNumber;
}

$(function () 
{
    $('.Delete_Item').click(function () 
        {
        return confirm('Bạn có thực sự muốn xóa không?');
    });
});

function LogOn(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (memvar1 != '') {
        act = act + '/' + memvar1;
    }
    if (memvar2 != '') {
        act = act + '/' + memvar2;
    }
    if (memvar3 != '') {
        act = act + '/' + memvar3;
    }
    if (memvar4 != '') {
        act = act + '/' + memvar4;
    }
    if (memvar5 != '') {
        act = act + '/' + memvar5;
    }
    var kiemtra = true;
    if (Trim(document.QuanTriHeThong.UserNameLogin.value) == "") {
        alert("Bạn phải nhập tài khoản đăng nhập!")
        kiemtra = false;
    }
    else if (Trim(document.QuanTriHeThong.PasswordLogin.value) == "") {
        alert("Bạn phải nhập mật khẩu đăng nhập!")
        kiemtra = false;
    }
    
    if (kiemtra) {
        f.action = act;
        f.method = 'POST';
        f.submit();
    }

}

function HoiDapSuDung(formname, Controler, action, id, fieldvar, value) 
{
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (id != '') {
        act = act + '/' + id;
    }
    if (fieldvar != '') {
        act = act + '/?' + fieldvar + '=' + value;
    }
    var kiemtra = true;
    if (Trim(document.QuanTriHeThong.NguoiHoi.value) == "Nhập họ và tên người hỏi")
    {
        alert("Bạn phải nhập Họ và tên người hỏi!")
        kiemtra = false;
    } 
    else if(Trim(document.QuanTriHeThong.DiaChiLienHe.value) == "Nhập địa chỉ liên hệ")
    {
        alert("Bạn phải nhập Số điện thoại liên hệ!")
        kiemtra = false;
    }
    else if (Trim(document.QuanTriHeThong.SoDienThoai.value) == "Nhập số điện thoại liên hệ") {
        alert("Bạn phải nhập số điện thoại liên hệ!")
        kiemtra = false;
    }
    else if(Trim(document.QuanTriHeThong.ThuDienTu.value) == "Nhập địa chỉ thư điện tử")
    {
        alert("Bạn phải nhập địa chỉ thư điện tử!")
        kiemtra = false;
    }
    
    else if(Trim(document.QuanTriHeThong.NoiDungHoiDap.value) == "Nhập nội dung câu hỏi")
    {
        alert("Bạn phải nhập Nội dung câu hỏi!")
        kiemtra = false;
    }
    else if(Trim(document.QuanTriHeThong.MaXacNhan.value) == "")
    {
        alert("Bạn phải nhập Kết quả (bằng số) của phép toán!")
        kiemtra = false;
    }
    else if (Trim(document.QuanTriHeThong.MaXacNhan.value) != Trim(document.QuanTriHeThong.KetQuaPhepToan.value))
    {
        alert("Bạn nhập sai kết quả của phép toán!")
        kiemtra = false;
    }


    if(kiemtra)
    {
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
    
}

function ExportSubmit(formname, Controler, action, memvar1, memvar2, memvar3) {
    var content_value = document.getElementById(memvar3).innerHTML;
    var IdMemvar = "#" + memvar2;
    $(IdMemvar).val(content_value);
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (memvar1 != '') {
        act = act + '/' + memvar1;
    }
    if (memvar2 != '') {
        act = act + '/' + memvar2;
    }
    f.action = act;
    f.method = 'POST';
    f.submit();
}

function Submitform(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (memvar1 != '') {
        act = act + '/' + memvar1;
    }
    if (memvar2 != '') {
        act = act + '/' + memvar2;
    }
    if (memvar3 != '') {
        act = act + '/' + memvar3;
    }
    if (memvar4 != '') {
        act = act + '/' + memvar4;
    }
    if (memvar5 != '') {
        act = act + '/' + memvar5;
    }
    f.action = act;
    f.method = 'POST';
    f.submit();

}

function SubmitPage(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (memvar2 != '') {
        act = act + '/' + memvar2;
    }
    if (memvar3 != '') {
        act = act + '/' + memvar3;
    }
    if (memvar4 != '') {
        act = act + '/' + memvar4;
    }
    if (memvar5 != '') {
        act = act + '/' + memvar5;
    }
    $("#PageCurent").val(memvar1);
    f.action = act;
    f.method = 'POST';
    f.submit();
}

function ChangePassword(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var f = eval('document.' + formname);
    if (Trim(f.NewPassword.value) == '' || Trim(f.NewConfirmPasswords.value) == '') {
        alert("Bạn phải nhập mật khẩu mới và nhập lại mật khẩu mới!")
    }
    else if (Trim(f.NewPassword.value) != Trim(f.NewConfirmPasswords.value)) {
        alert("Hai mật khẩu không trùng khớp!")
    }
    else if (f.NewPassword.value.length < 6) {
        alert("Mật khẩu tối thiểu phải có 6 ký tự!")
    }
    else {
        var conf = confirm("Bạn thực sự muốn thay đổi mật khẩu!")
        if (conf) {
            var act = '/' + Controler + '/' + action;
            if (memvar1 != '') {
                act = act + '/' + memvar1;
            }
            if (memvar2 != '') {
                act = act + '/' + memvar2;
            }
            if (memvar3 != '') {
                act = act + '/' + memvar3;
            }
            if (memvar4 != '') {
                act = act + '/' + memvar4;
            }
            if (memvar5 != '') {
                act = act + '/' + memvar5;
            }
            f.action = act;
            f.method = 'POST';
            f.submit();
        }
    }
}

function SaveEditUser(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var f = eval('document.' + formname);
    var arr1 = explode(' ', Trim(f.UserNames.value));
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (Trim(f.FullNames.value) == '' || Trim(f.Email.value) == '') {
        alert("Bạn phải nhập đầy đủ các mục đánh dấu (*)!")
    }
    else if (!filter.test(f.Email.value)) {
        alert("Địa chỉ email nhập chưa đúng!")
    }
    else {
        var act = '/' + Controler + '/' + action;
        if (memvar1 != '') {
            act = act + '/' + memvar1;
        }
        if (memvar2 != '') {
            act = act + '/' + memvar2;
        }
        if (memvar3 != '') {
            act = act + '/' + memvar3;
        }
        if (memvar4 != '') {
            act = act + '/' + memvar4;
        }
        if (memvar5 != '') {
            act = act + '/' + memvar5;
        }
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
}

function SaveRegistry(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) 
{
    var f = eval('document.' + formname);
    var arr1 = explode(' ', Trim(f.UserNames.value));
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (Trim(f.SecurityValue.value) == '' || Trim(f.UserNames.value) == '' || Trim(f.PassWords.value) == '' || Trim(f.ConfirmPassWords.value) == '' || Trim(f.FullNames.value) == '' || Trim(f.Email.value) == '') {
        alert("Bạn phải nhập đầy đủ các mục đánh dấu (*)!")
    }
    else if (Trim(f.PassWords.value) != Trim(f.ConfirmPassWords.value)) {
        alert("Hai mật khẩu không trùng khớp!")
    }
    else if (arr1.length >1) 
    {
        alert("Tên đăng nhập không được chứa khoảng trắng!")
    }
    else if (!filter.test(f.Email.value)) 
    {
        alert("Địa chỉ email nhập chưa đúng!")
    }
    else if (f.DongY.checked==false) {
        alert("Bạn chưa đồng ý với quy định của toanthongminh.vn!")
    }
    else if (f.PassWords.value.length < 6) {
        alert("Mật khẩu tối thiểu phải có 6 ký tự!")
    }
    else if (Trim(f.Security.value) != Trim(f.SecurityValue.value)) {
        alert("Bạn nhập mã bảo vệ chưa đúng!")
    }
    else {
        var act = '/' + Controler + '/' + action;
        if (memvar1 != '') {
            act = act + '/' + memvar1;
        }
        if (memvar2 != '') {
            act = act + '/' + memvar2;
        }
        if (memvar3 != '') {
            act = act + '/' + memvar3;
        }
        if (memvar4 != '') {
            act = act + '/' + memvar4;
        }
        if (memvar5 != '') {
            act = act + '/' + memvar5;
        }
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
}
function SaveNewUser(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var f = eval('document.' + formname);
    var arr1 = explode(' ', Trim(f.UserNames.value));
    var filter = /^([a-zA-Z0-9_\.\-])+\@(([a-zA-Z0-9\-])+\.)+([a-zA-Z0-9]{2,4})+$/;

    if (Trim(f.UserNames.value) == '' || Trim(f.FullNames.value) == '' || Trim(f.Email.value) == '') {
        alert("Bạn phải nhập đầy đủ các mục đánh dấu (*)!")
    }
    else if (arr1.length > 1) {
        alert("Tên đăng nhập không được chứa khoảng trắng!")
    }
    else if (!filter.test(f.Email.value)) {
        alert("Địa chỉ email nhập chưa đúng!")
    }
    else {
        var act = '/' + Controler + '/' + action;
        if (memvar1 != '') {
            act = act + '/' + memvar1;
        }
        if (memvar2 != '') {
            act = act + '/' + memvar2;
        }
        if (memvar3 != '') {
            act = act + '/' + memvar3;
        }
        if (memvar4 != '') {
            act = act + '/' + memvar4;
        }
        if (memvar5 != '') {
            act = act + '/' + memvar5;
        }
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
}
function SubmitformDel(formname, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var conf = confirm("Bấm nút OK để thực hiện!")
    if (conf) {
        var f = eval('document.' + formname);
        var act = '/' + Controler + '/' + action;
        if (memvar1 != '') {
            act = act + '/' + memvar1;
        }
        if (memvar2 != '') {
            act = act + '/' + memvar2;
        }
        if (memvar3 != '') {
            act = act + '/' + memvar3;
        }
        if (memvar4 != '') {
            act = act + '/' + memvar4;
        }
        if (memvar5 != '') {
            act = act + '/' + memvar5;
        }
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
}
function SubmitformConf(formname, Mes, Controler, action, memvar1, memvar2, memvar3, memvar4, memvar5) {
    var conf = confirm(Mes)
    if (conf) {
        var f = eval('document.' + formname);
        var act = '/' + Controler + '/' + action;
        if (memvar1 != '') {
            act = act + '/' + memvar1;
        }
        if (memvar2 != '') {
            act = act + '/' + memvar2;
        }
        if (memvar3 != '') {
            act = act + '/' + memvar3;
        }
        if (memvar4 != '') {
            act = act + '/' + memvar4;
        }
        if (memvar5 != '') {
            act = act + '/' + memvar5;
        }
        f.action = act;
        f.method = 'POST';
        f.submit();
    }
}
function SubmitNormal(formname, Controler, action, id, value) {
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (id != '') {
        act = act + '/' + id;
    }
    if (value != '') {
        act = act + '/'+ value;
    }
    f.action = act;
    f.method = 'POST';
    f.submit();
}

function SubmitformGET(formname, Controler, action, id, fieldvar, value) {
    var f = eval('document.' + formname);
    var act = '/' + Controler + '/' + action;
    if (id != '') {
        act = act + '/' + id;
    }
    if (fieldvar != '') {
        act = act + '/?' + fieldvar + '=' + value;
    }
    if (action == "ExportToWord") {
        var content_vlue = document.getElementById('print_content').innerHTML;
        $(".WordContent").val(content_vlue);
    }
    f.action = act;
    f.method = 'GET';
    f.submit();
}

function SubmitformChecked(formname, Controler, action, id, fieldvar, value) {
    var f = eval('document.' + formname);
    var SoBanGhi = parseInt(f.NumOfRecord.value);
    var CheckName = '';
    var KiemTra = 0;
    var BienMa=''
    for (var i = 1; i <= SoBanGhi; i++) {
        BienMa = 'document.' + formname + '.IdName' + Trim(i.toString()) + '.value';
        CheckName = 'document.' + formname + '.cbx_' + eval(BienMa);
        if (eval(CheckName).checked == true) {
            KiemTra = 1;
            break;
        }
    }
    if (KiemTra == 0) {
      alert("Bạn chưa chọn thực phẩm!")
    }
    else {
        var f = eval('document.' + formname);
        f.action = '/' + Controler + '/' + action
        if (id != '') {
            f.action = '/' + Controler + '/' + action + '/' + id;
        }
        if (fieldvar != '') {
            f.action = '/' + Controler + '/' + action + '/' + id + '?' + fieldvar + '=' + value;
        }
        f.method = 'POST';
        f.submit();
    }
}

function SoSanhHaiNgay(Ngaycheck1, Ngaycheck2) 
{
    //Nếu Ngaycheck1 lớn hơn Ngaycheck2 thì trả về 1 và ngược lại
    var SoSanh = 0;

    arr1 = explode('/', Trim(Ngaycheck1));
    arr2 = explode('/', Trim(Ngaycheck2));
    if (parseInt(arr1[2]) > parseInt(arr2[2])) 
    {
        SoSanh = 1;
    }
    else if (parseInt(arr1[2]) == parseInt(arr2[2])) 
    {
        if (parseInt(arr1[1]) > parseInt(arr2[1])) 
        {
            SoSanh = 1;
        }
        else
            if (parseInt(arr1[1]) == parseInt(arr2[1])) 
            {
                if (parseInt(arr1[0]) > parseInt(arr2[0])) 
                {
                    SoSanh = 1;
                }
            }
    }
       
    return ;
    
}

function Submit(formname) 
{
    var f = eval('document.' + formname);
    f.method = 'POST';
    f.submit();
}
function TextChanged() 
{
    var f = 'document.QuanTriHeThong';
    f.Output.value = f.InputText.value;
}

function explode(delimiter, string, limit) {
    var emptyArray = { 0: '' };

    if (arguments.length < 2 || typeof arguments[0] == 'undefined' || typeof arguments[1] == 'undefined') {
        return null;
    }

    if (delimiter === '' || delimiter === false || delimiter === null) {
        return false;
    }

    if (typeof delimiter == 'function' || typeof delimiter == 'object' || typeof string == 'function' || typeof string == 'object') {
        return emptyArray;
    }
    if (delimiter === true) {
        delimiter = '1';
    }
    if (!limit) {
        return string.toString().split(delimiter.toString());
    } else {
        var splitted = string.toString().split(delimiter.toString()); var partA = splitted.splice(0, limit - 1);
        var partB = splitted.join(delimiter.toString());
        partA.push(partB);
        return partA;
    }
}

function FormatNum(str) {
    var number = parseFloat(str);
    var dau = '';
    if (number < 0) {
        dau = '-';
    }
    number = Math.abs(str);
    number1 = parseInt(number);
    number2 = number - number1;
    number2 = number2 * 10;
    number2 = number2.toFixed(0);
    var amount = new String(number1);
    amount = amount.split("").reverse();
    var output = "";
    for (var i = 0; i <= amount.length - 1; i++) {
        output = amount[i] + output;
        if ((i + 1) % 3 == 0 && (amount.length - 1) !== i) output = ',' + output;
    }
    if (number2 > 0) {
        return dau + output +'.'+ number2.toString();
    }
    else {
        return dau + output;
    }
}

function GetNumFromFormat(str) {
    var amount = new String(str);
    amount = amount.split("").reverse();
    var output = "";
    for (var i = 0; i <= amount.length - 1; i++) 
    {
        if (amount[i] != ",") 
        {
            output = amount[i] + output;
        }
    }
    return output;
}

function XuatKho(formname, Controler, action, id, fieldvar, value) 
{
    var NumberMealInfor = parseInt(document.QuanTriHeThong.NumMealInfor.value);
    var MealInforStart = parseInt(document.QuanTriHeThong.MealInforStart.value);
    var NgayBeNhat = Trim(document.QuanTriHeThong.MinApplyDate.value);
    var KiemTra = 0;
    var KiemTraNgay = 0;
    var KiemTraChonHet = 0;
    var DSNgay = '';
    var dem = 0;
    //Kiểm tra xem có check box nào đc check chưa
    for (var i = MealInforStart + 1; i <= NumberMealInfor; i++) 
    {
        //Lấy tên của CheckBox
        var CheckName = 'document.QuanTriHeThong.cbx_MealInfor_' + i.toString();
        var ApplyDate = 'document.QuanTriHeThong.ApplyDate' + i.toString() + '.value';
        if (eval(CheckName).checked == true) 
        {
            dem++;
            KiemTra = 1;
            if (dem == 1) 
            {
                DSNgay = DSNgay + eval(ApplyDate);
            }
            else 
            {
                DSNgay = DSNgay+'~' + eval(ApplyDate);
            }
        }
    }
    if (KiemTra == 0) 
    {
        alert('Bạn chưa chọn thực đơn nào!');
    }
    else 
    {

        //Kiểm tra xem các ngày coa trùng nhau không?
        var arr = explode('~', DSNgay);
        if (arr.length >= 2) {
            for (var i = 0; i < arr.length - 1; i++) {
                if (Trim(arr[i]) != Trim(arr[i + 1])) {
                    KiemTraNgay = 1;
                    break;
                }
            }
        }
		if (KiemTraNgay == 1) {
		    alert('Chọn thực đơn không cùng một ngày!');
		}
        else
        {
            //Kiểm tra xem có check box nào đc check chưa
            for (var i = MealInforStart + 1; i <= NumberMealInfor; i++) 
            {
                //Lấy tên của CheckBox
                var CheckName = 'document.QuanTriHeThong.cbx_MealInfor_' + i.toString();
                var ApplyDate = 'document.QuanTriHeThong.ApplyDate' + i.toString() + '.value';
                if (eval(CheckName).checked == false && eval(ApplyDate) == arr[0]) 
                {
                    KiemTraChonHet = 1;
                }
            }
            if (KiemTraChonHet == 1) {
                alert('Chưa chọn hết các thực đơn của một ngày!');
            }
            else {
                var arr1 = explode('~', DSNgay);
                if (arr1[0] != NgayBeNhat) {
                    alert('Bạn phải xuất kho tuần tự, có những thực đơn trước đó chưa xuất kho!');
                }
                else {
                    var f = eval('document.' + formname);
                    var act = '/' + Controler + '/' + action;
                    if (id != '') {
                        act = act + '/' + id;
                    }
                    if (fieldvar != '') {
                        act = act + '/?' + fieldvar + '=' + value;
                    }
                    f.action = act;
                    f.method = 'POST';
                    f.submit();
                }
            }
        }
        
    }
}

function HuyXuatKho(formname, Controler, action, id, fieldvar, value) 
{
    var NumOutStockInfor = parseInt(document.QuanTriHeThong.NumOutStockInfor.value);
    var OutStockInforStart = parseInt(document.QuanTriHeThong.OutStockInforStart.value);
    var NgayLonNhat = Trim(document.QuanTriHeThong.OutStockDateMax.value);
    var KiemTra = 0;
    var KiemTraNgay = 0;
    var DSNgay = '';
    var dem = 0;
    //Kiểm tra xem có check box nào đc check chưa
    for (var i = OutStockInforStart + 1; i <= NumOutStockInfor; i++) 
    {
        //Lấy tên của CheckBox
        var CheckName = 'document.QuanTriHeThong.OutStockInfor' + i.toString();
        var ApplyDate = 'document.QuanTriHeThong.OutStockDate' + i.toString() + '.value';
        if (eval(CheckName).checked == true) 
        {
            dem++;
            KiemTra = 1;
            if (dem == 1) 
            {
                DSNgay = DSNgay + eval(ApplyDate);
            }
            else 
            {
                DSNgay = DSNgay + '~' + eval(ApplyDate);
            }
        }
    }
    if (KiemTra == 0) 
    {
        alert('Bạn chưa chọn phiếu xuất kho nào!');
    }
    else 
    {
        var arr = explode('~', DSNgay);
        if (arr[0] != NgayLonNhat) 
        {
            KiemTra = 0;
        }
        else 
        {
            for (var i = OutStockInforStart + 1; i <= NumOutStockInfor - 1; i++) 
            {
                var CheckName = 'document.QuanTriHeThong.OutStockInfor' + i.toString();
                if (eval(CheckName).checked == false) 
                {
                    for (var j = i + 1; j <= NumOutStockInfor; j++) 
                    {
                        var CheckName1 = 'document.QuanTriHeThong.OutStockInfor' + j.toString();
                        if (eval(CheckName1).checked == true) 
                        {
                            KiemTra = 0;
                            break;
                        }
                    }
                }
                if (KiemTra == 0) 
                {
                    break;
                }
            }
        }
        if (KiemTra ==0) 
        {
            alert('Hủy xuất kho không đúng tứ tự!');
        }
        else 
        {
                var f = eval('document.' + formname);
                var act = '/' + Controler + '/' + action;
                if (id != '') 
                {
                    act = act + '/' + id;
                }
                if (fieldvar != '') 
                {
                    act = act + '/?' + fieldvar + '=' + value;
                }
                f.action = act;
                f.method = 'POST';
                f.submit();
        }
    }
}

function ChangeChildNumber(FoodNumber) 
{
    var FoodGroupId = 1;

    //Lấy định lượng
    var FoodCalo = 0;
    var FoodProtit = 0;
    var FoodLipit = 0;
    var FoodGluxit = 0;
    var FoodCalci = 0;
    var FoodPhosphor = 0;
    var FoodFe = 0;
    var FoodVitaminA = 0;
    var FoodVitaminBeta = 0;
    var FoodVitaminB1 = 0;
    var FoodVitaminB2 = 0;
    var FoodVitaminPP = 0;
    var FoodVitaminC = 0;
                     
    //Lấy các thành phần dinh dưỡng sinh ra
    var WeightOnChild = 0;
    var FoodPrice = 0;
    var WeightSumFood = 0;
    var EatPercent = 0;

    var WeightEatSumFood = 0;

    //Lấy số xuất ăn từ View
    var ChildNumber = parseFloat(GetNumFromFormat(document.QuanTriHeThong.NumPersonForMeal.value));
    if (isNaN(ChildNumber)) {
        ChildNumber = 0;
    }

    //Lấy giá của xuất ăn từ View
    var ChildPrice = parseFloat(GetNumFromFormat(document.QuanTriHeThong.MealPrice.value));

    //Tính số tiền thu được
    document.QuanTriHeThong.MoneySumIn.value = FormatNum((ChildNumber * ChildPrice).toFixed(0));

    var NumberFood = parseInt(FoodNumber);

    for (i = 1; i <= NumberFood; i++) 
    {
        //Đọc định lượng dinh dưỡng của thực phẩm
        FoodCalo = parseFloat(eval('document.QuanTriHeThong.FoodCalo' + Trim(i.toString()) + '.value'));
        FoodProtit = parseFloat(eval('document.QuanTriHeThong.FoodProtit' + Trim(i.toString()) + '.value'));
        FoodLipit = parseFloat(eval('document.QuanTriHeThong.FoodLipit' + Trim(i.toString()) + '.value'));
        FoodGluxit = parseFloat(eval('document.QuanTriHeThong.FoodGluxit' + Trim(i.toString()) + '.value'));

        FoodCalci = parseFloat(eval('document.QuanTriHeThong.FoodCalci' + Trim(i.toString()) + '.value'));
        FoodPhosphor = parseFloat(eval('document.QuanTriHeThong.FoodPhosphor' + Trim(i.toString()) + '.value'));
        FoodFe = parseFloat(eval('document.QuanTriHeThong.FoodFe' + Trim(i.toString()) + '.value'));
        FoodVitaminA = parseFloat(eval('document.QuanTriHeThong.FoodVitaminA' + Trim(i.toString()) + '.value'));

        FoodVitaminBeta = parseFloat(eval('document.QuanTriHeThong.FoodVitaminBeta' + Trim(i.toString()) + '.value'));
        FoodVitaminB1 = parseFloat(eval('document.QuanTriHeThong.FoodVitaminB1' + Trim(i.toString()) + '.value'));
        FoodVitaminPP = parseFloat(eval('document.QuanTriHeThong.FoodVitaminPP' + Trim(i.toString()) + '.value'));
        FoodVitaminC = parseFloat(eval('document.QuanTriHeThong.FoodVitaminC' + Trim(i.toString()) + '.value'));

        FoodVitaminB2 = parseFloat(eval('document.QuanTriHeThong.FoodVitaminB2' + Trim(i.toString()) + '.value'));

        //Đọc định lượng
        WeightOnChild = parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.WeightOnChild' + Trim(i.toString()) + '.value')));

        //Đọc giá của thực phẩm
        FoodPrice = parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.FoodPrice' + Trim(i.toString()) + '.value')));

        //Đọc nhóm thực phẩm (Động vật - Ngày vật)
        FoodGroupId = parseFloat(eval('document.QuanTriHeThong.FoodGroupId' + Trim(i.toString()) + '.value'));

        //Tỷ lệ ăn được
        EatPercent = parseFloat(eval('document.QuanTriHeThong.EatPercent' + Trim(i.toString()) + '.value'));

        //Tính tổng số khối lượng thực phẩm
        WeightSumFood = (WeightOnChild * ChildNumber).toFixed(0);
        var f1 = eval('document.QuanTriHeThong.WeightSumFood' + Trim(i.toString()));
        f1.value = FormatNum(WeightSumFood);

        //Tính tổng khối lượng thực phẩm ăn được
        var f9 = eval('document.QuanTriHeThong.WeightEat' + Trim(i.toString()));
        WeightEatSumFood = (WeightSumFood / 100 * EatPercent).toFixed(0);
        f9.value = FormatNum(WeightEatSumFood);

        //Tính tổng số tiền mua khối lượng thực phẩm
        var f2 = eval('document.QuanTriHeThong.SumMoney' + Trim(i.toString()));
        f2.value = FormatNum((WeightSumFood / 100 * FoodPrice).toFixed(0));

        //Tính tổng số calo
        var f3 = eval('document.QuanTriHeThong.SumCalo' + Trim(i.toString()));
        f3.value = FormatNum((WeightEatSumFood / 100 * FoodCalo).toFixed(0));

        //Tính tổng số bột đường
        var f4 = eval('document.QuanTriHeThong.SumGluxit' + Trim(i.toString()));
        f4.value = FormatNum((WeightEatSumFood / 100 * FoodGluxit).toFixed(0));

        //Tính tổng số gam chất đạm
        if (FoodGroupId != 1) 
        {
            var f5 = eval('document.QuanTriHeThong.SumProtitAnimals' + Trim(i.toString()));
            f5.value = FormatNum((WeightEatSumFood / 100 * FoodProtit).toFixed(0));
            var f6 = eval('document.QuanTriHeThong.SumProtitPlant' + Trim(i.toString()));
            f6.value = 0;
        }
        else 
        {
            var f5 = eval('document.QuanTriHeThong.SumProtitAnimals' + Trim(i.toString()));
            f5.value = 0;
            var f6 = eval('document.QuanTriHeThong.SumProtitPlant' + Trim(i.toString()));
            f6.value = FormatNum((WeightEatSumFood / 100 * FoodProtit).toFixed(0));
        }

        //Tính tổng số gam chất béo
        if (FoodGroupId != 1) 
        {
            var f7 = eval('document.QuanTriHeThong.SumLipitAnimals' + Trim(i.toString()));
            f7.value = FormatNum((WeightEatSumFood / 100 * FoodLipit).toFixed(0));
            var f8 = eval('document.QuanTriHeThong.SumLipitPlant' + Trim(i.toString()));
            f8.value = 0;
        }
        else 
        {
            var f7 = eval('document.QuanTriHeThong.SumLipitAnimals' + Trim(i.toString()));
            f7.value = 0;
            var f8 = eval('document.QuanTriHeThong.SumLipitPlant' + Trim(i.toString()));
            f8.value = FormatNum((WeightEatSumFood / 100 * FoodLipit).toFixed(0));
        }

        //Tính tổng số Calci
        var f10 = eval('document.QuanTriHeThong.SumCalcium' + Trim(i.toString()));
        f10.value = FormatNum((WeightEatSumFood / 100 * FoodCalci).toFixed(0));

        //Tính tổng số Phosphor
        var f11 = eval('document.QuanTriHeThong.SumPhosphor' + Trim(i.toString()));
        f11.value = FormatNum((WeightEatSumFood / 100 * FoodPhosphor).toFixed(0));

        //Tính tổng số sắt
        var f12 = eval('document.QuanTriHeThong.SumIron' + Trim(i.toString()));
        f12.value = FormatNum((WeightEatSumFood / 100 * FoodFe).toFixed(0));

        //Tính tổng số Vitamin A
        var f13 = eval('document.QuanTriHeThong.SumVitaminA' + Trim(i.toString()));
        f13.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminA).toFixed(0));

        //Tính tổng số Vitamin Beta
        var f14 = eval('document.QuanTriHeThong.SumVitaminBeta' + Trim(i.toString()));
        f14.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminBeta).toFixed(0));

        //Tính tổng số Vitamin B1
        var f15 = eval('document.QuanTriHeThong.SumVitaminB1' + Trim(i.toString()));
        f15.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminB1).toFixed(0));

        //Tính tổng số Vitamin B2
        var f18 = eval('document.QuanTriHeThong.SumVitaminB2' + Trim(i.toString()));
        f18.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminB2).toFixed(0));

        //Tính tổng số Vitamin PP
        var f16 = eval('document.QuanTriHeThong.SumVitaminPP' + Trim(i.toString()));
        f16.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminPP).toFixed(0));

        //Tính tổng số Vitamin C
        var f17 = eval('document.QuanTriHeThong.SumVitaminC' + Trim(i.toString()));
        f17.value = FormatNum((WeightEatSumFood / 100 * FoodVitaminC).toFixed(0));

    }
}

function CountNutri(Order, FoodNumber) 
{
    var WeightOnChild = 0;
    var NumPersonForMeal = 0;
    var WeightSumFood = 0;
    var FoodPrice = 0;
    var SumMoney = 0;
    var FoodCalo = 0;
    var FoodProtit = 0;
    var FoodLipit = 0;
    var FoodGluxit = 0;

    var FoodCalci = 0;
    var FoodPhosphor = 0;
    var FoodFe = 0;
    var FoodVitaminA = 0;
    var FoodVitaminBeta = 0;
    var FoodVitaminB1 = 0;
    var FoodVitaminB2 = 0;
    var FoodVitaminPP = 0;
    var FoodVitaminC = 0;

    var SumCalo = 0;
    var SumProtit = 0;
    var SumLipit = 0;
    var SumGluxit = 0;
    var SumVitaminC = 0;
    var SumVitaminPP = 0;
    var SumVitaminB2 = 0;
    var SumVitaminB1 = 0;
    var SumVitaminBeta = 0;
    var SumVitaminA = 0;
    var SumIron = 0;
    var SumPhosphor = 0;
    var SumCalcium = 0;

    var WeightEat = 0;
    var FoodGroup =1;
    var EatPercent=0;
    var FoodNumber=0
    var f1=eval('document.QuanTriHeThong.WeightOnChild' + Trim(Order));
    if (isNumber(f1.value) && f1.value.length != 0) 
    {
        WeightOnChild = parseFloat(GetNumFromFormat(f1.value));
    }
    else
    {
        f1.value=0;
    }

    var f16 = eval('document.QuanTriHeThong.NumPersonForMeal');
    if (isNumber(f16.value) && f16.value.length != 0) 
    {
        NumPersonForMeal = parseFloat(GetNumFromFormat(f16.value));
    }

    var f2=eval('document.QuanTriHeThong.FoodPrice' + Trim(Order));
    if (isNumber(f2.value) && f2.value.length != 0) 
    {
        FoodPrice = parseFloat(GetNumFromFormat(f2.value));
        f2.value=FormatNum(FoodPrice);
    }
    else
    {
            f2.value=0;
    }
    
    var f3=eval('document.QuanTriHeThong.FoodCalo' + Trim(Order));                                                                            
    if (isNumber(f3.value) && f3.value.length != 0) 
    {
        FoodCalo = parseFloat(f3.value);
    }

    var f4=eval('document.QuanTriHeThong.FoodProtit' + Trim(Order));
    if (isNumber(f4.value) && f4.value.length != 0) 
    {
        FoodProtit = parseFloat(f4.value);
    }

    var f5=eval('document.QuanTriHeThong.FoodLipit' + Trim(Order));
    if (isNumber(f5.value) && f5.value.length != 0) 
    {
        FoodLipit = parseFloat(f5.value);
    }

    var f6=eval('document.QuanTriHeThong.FoodGluxit' + Trim(Order));
    if (isNumber(f6.value) && f6.value.length != 0) 
    {
        FoodGluxit = parseFloat(f6.value);
    }

    var v1 = eval('document.QuanTriHeThong.FoodCalci' + Trim(Order));
    if (isNumber(v1.value) && v1.value.length != 0) {
        FoodCalci = parseFloat(v1.value);
    }
   
    var v2 = eval('document.QuanTriHeThong.FoodPhosphor' + Trim(Order));
    if (isNumber(v2.value) && v2.value.length != 0) {
        FoodPhosphor = parseFloat(v2.value);
    }

    var v3 = eval('document.QuanTriHeThong.FoodFe' + Trim(Order));
    if (isNumber(v3.value) && v3.value.length != 0) {
        FoodFe = parseFloat(v3.value);
    }

    var v4 = eval('document.QuanTriHeThong.FoodVitaminA' + Trim(Order));
    if (isNumber(v4.value) && v4.value.length != 0) {
        FoodVitaminA = parseFloat(v4.value);
    }

    var v5 = eval('document.QuanTriHeThong.FoodVitaminBeta' + Trim(Order));
    if (isNumber(v5.value) && v5.value.length != 0) {
        FoodVitaminBeta = parseFloat(v5.value);
    }

    var v6 = eval('document.QuanTriHeThong.FoodVitaminB1' + Trim(Order));
    if (isNumber(v6.value) && v6.value.length != 0) {
        FoodVitaminB1 = parseFloat(v6.value);
    }
   
    var v7 = eval('document.QuanTriHeThong.FoodVitaminB2' + Trim(Order));
    if (isNumber(v7.value) && v7.value.length != 0) {
        FoodVitaminB2 = parseFloat(v7.value);
    }

    var v8 = eval('document.QuanTriHeThong.FoodVitaminPP' + Trim(Order));
    if (isNumber(v8.value) && v8.value.length != 0) {
        FoodVitaminPP = parseFloat(v8.value);
    }

    var v9 = eval('document.QuanTriHeThong.FoodVitaminC' + Trim(Order));
    if (isNumber(v9.value) && v9.value.length != 0) {
        FoodVitaminC = parseFloat(v9.value);
    }
    
    var f7=eval('document.QuanTriHeThong.FoodGroupId' + Trim(Order));
    if (isNumber(f7.value) && f7.value.length != 0) 
    {
        FoodGroup = parseFloat(f7.value);
    }

    var f8=eval('document.QuanTriHeThong.EatPercent' + Trim(Order));
    if (isNumber(f8.value) && f8.value.length != 0) 
    {
        EatPercent = parseFloat(f8.value);
    }

    var f9=eval('document.QuanTriHeThong.FoodNumber');
    if (isNumber(f9.value) && f9.value.length != 0) 
    {
        FoodNumber = parseFloat(f9.value);
    }

    WeightSumFood = WeightOnChild*NumPersonForMeal;
    WeightSumFood = WeightSumFood.toFixed(0);
    var f10=eval('document.QuanTriHeThong.WeightSumFood' + Trim(Order));
    f10.value = FormatNum(WeightSumFood);

    WeightEat = WeightSumFood * EatPercent / 100;
    WeightEat = WeightEat.toFixed(0);
    var f26 = eval('document.QuanTriHeThong.WeightEat' + Trim(Order));
    f26.value = FormatNum(WeightEat);

    SumMoney=WeightSumFood/100*FoodPrice;
    SumMoney = SumMoney.toFixed(0);
    var f11=eval('document.QuanTriHeThong.SumMoney' + Trim(Order));
    f11.value = FormatNum(SumMoney);

    SumCalo = WeightEat / 100 * FoodCalo;
    SumCalo = SumCalo.toFixed(0);
    var f12=eval('document.QuanTriHeThong.SumCalo' + Trim(Order));
    f12.value = FormatNum(SumCalo);

    SumProtit = WeightEat/100*FoodProtit;
    SumProtit = SumProtit.toFixed(0);
    if (FoodGroup!=1)
    {
        var f13=eval('document.QuanTriHeThong.SumProtitAnimals' + Trim(Order));
        f13.value = FormatNum(SumProtit);
    }
    else
    {
        var f13=eval('document.QuanTriHeThong.SumProtitPlant' + Trim(Order));
        f13.value = FormatNum(SumProtit);
    }

    SumLipit = WeightEat/100*FoodLipit;
    SumLipit = SumLipit.toFixed(0);
    if (FoodGroup!=1)
    {
        var f14=eval('document.QuanTriHeThong.SumLipitAnimals' + Trim(Order));
        f14.value = FormatNum(SumLipit);
    }
    else
    {
         var f14=eval('document.QuanTriHeThong.SumLipitPlant' + Trim(Order));
         f14.value = FormatNum(SumLipit);
    }

    SumGluxit = WeightEat/100*FoodGluxit;
    SumGluxit = SumGluxit.toFixed(0);
    var f15=eval('document.QuanTriHeThong.SumGluxit' + Trim(Order));
    f15.value = FormatNum(SumGluxit);

    SumVitaminC = WeightEat / 100 * FoodVitaminC;
    SumVitaminC = SumVitaminC.toFixed(0);
    var f16 = eval('document.QuanTriHeThong.SumVitaminC' + Trim(Order));
    f16.value = FormatNum(SumVitaminC);

    SumVitaminPP = WeightEat / 100 * FoodVitaminPP;
    SumVitaminPP = SumVitaminPP.toFixed(0);
    var f17 = eval('document.QuanTriHeThong.SumVitaminPP' + Trim(Order));
    f17.value = FormatNum(SumVitaminPP);

    SumVitaminB2 = WeightEat / 100 * FoodVitaminPP;
    SumVitaminB2 = SumVitaminB2.toFixed(0);
    var f18 = eval('document.QuanTriHeThong.SumVitaminB2' + Trim(Order));
    f18.value = FormatNum(SumVitaminB2);

    SumVitaminB1 = WeightEat / 100 * FoodVitaminB1;
    SumVitaminB1 = SumVitaminB1.toFixed(0);
    var f19 = eval('document.QuanTriHeThong.SumVitaminB1' + Trim(Order));
    f19.value = FormatNum(SumVitaminB1);

    SumVitaminB2 = WeightEat / 100 * FoodVitaminB2;
    SumVitaminB2 = SumVitaminB2.toFixed(0);
    var f20 = eval('document.QuanTriHeThong.SumVitaminB2' + Trim(Order));
    f20.value = FormatNum(SumVitaminB2);

    SumVitaminBeta = WeightEat / 100 * FoodVitaminBeta;
    SumVitaminBeta = SumVitaminBeta.toFixed(0);
    var f21 = eval('document.QuanTriHeThong.SumVitaminBeta' + Trim(Order));
    f21.value = FormatNum(SumVitaminBeta);

    SumVitaminA = WeightEat / 100 * FoodVitaminA;
    SumVitaminA = SumVitaminA.toFixed(0);
    var f22 = eval('document.QuanTriHeThong.SumVitaminA' + Trim(Order));
    f22.value = FormatNum(SumVitaminA);

    SumIron = WeightEat / 100 * FoodFe;
    SumIron = SumIron.toFixed(0);
    var f23 = eval('document.QuanTriHeThong.SumIron' + Trim(Order));
    f23.value = FormatNum(SumIron);

    SumPhosphor = WeightEat / 100 * FoodPhosphor;
    SumPhosphor = SumPhosphor.toFixed(0);
    var f24 = eval('document.QuanTriHeThong.SumPhosphor' + Trim(Order));
    f24.value = FormatNum(SumPhosphor);

    SumCalcium = WeightEat / 100 * FoodCalci;
    SumCalcium = SumCalcium.toFixed(0);
    var f25 = eval('document.QuanTriHeThong.SumCalcium' + Trim(Order));
    f25.value = FormatNum(SumCalcium);

    PutValueMoney(FoodNumber);
}

function PutValueMoney(FoodNumber) {
    var f = eval('document.QuanTriHeThong');
    var TotalMoneyFood = 0; //Tổng số tiền mua thực phẩm
    var TotalCaloFood = 0; // Tổng số Calo
    var TotalProtitFoodAnimals = 0; //Tổng số gam chất đạm động vật
    var TotalProtitFoodPlants = 0; //Tổng số gam chất đạm thực vật
    var TotalLipitFoodAnimals = 0; //Tổng số gam chất béo động vật
    var TotalLipitFoodPlants = 0; //Tổng số gam chất béo thực vật
    var TotalGluxitFood = 0; //Tổng số gam chất bột đường

    var SumVitaminC = 0; //Tổng số chất Vitamin C
    var SumVitaminPP = 0; //Tổng số chất Vitamin PP
    var SumVitaminB2 = 0; //Tổng số chất Vitamin B2
    var SumVitaminB1 = 0; //Tổng số chất Vitamin B1
    var SumVitaminBeta = 0; //Tổng số chất Vitamin Beta
    var SumVitaminA = 0; //Tổng số chất Vitamin A
    var SumIron = 0; //Tổng số chất Sắt
    var SumPhosphor = 0; //Tổng số chất Phosphor
    var SumCalcium = 0; //Tổng số chất Calci

    var NumberFood = parseInt(FoodNumber);
    for (i = 1; i <= NumberFood; i++) 
    {
        TotalMoneyFood = TotalMoneyFood + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumMoney' + Trim(i.toString()) + '.value')));
        TotalCaloFood = TotalCaloFood + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumCalo' + Trim(i.toString()) + '.value')));
        TotalProtitFoodAnimals = TotalProtitFoodAnimals + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumProtitAnimals' + Trim(i.toString()) + '.value')));
        TotalProtitFoodPlants = TotalProtitFoodPlants + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumProtitPlant' + Trim(i.toString()) + '.value')));
        TotalLipitFoodAnimals = TotalLipitFoodAnimals + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumLipitAnimals' + Trim(i.toString()) + '.value')));
        TotalLipitFoodPlants = TotalLipitFoodPlants + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumLipitPlant' + Trim(i.toString()) + '.value')));
        TotalGluxitFood = TotalGluxitFood + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumGluxit' + Trim(i.toString()) + '.value')));

        SumCalcium = SumCalcium + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumCalcium' + Trim(i.toString()) + '.value')));
        SumPhosphor = SumPhosphor + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumPhosphor' + Trim(i.toString()) + '.value')));
        SumIron = SumIron + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumIron' + Trim(i.toString()) + '.value')));
        SumVitaminA = SumVitaminA + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminA' + Trim(i.toString()) + '.value')));
        SumVitaminBeta = SumVitaminBeta + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminBeta' + Trim(i.toString()) + '.value')));
        SumVitaminB1 = SumVitaminB1 + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminB1' + Trim(i.toString()) + '.value')));
        SumVitaminB2 = SumVitaminB2 + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminB2' + Trim(i.toString()) + '.value')));
        SumVitaminPP = SumVitaminPP + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminPP' + Trim(i.toString()) + '.value')));
        SumVitaminC = SumVitaminC + parseFloat(GetNumFromFormat(eval('document.QuanTriHeThong.SumVitaminC' + Trim(i.toString()) + '.value')));

    }
    //Tính tỷ lệ đạm giữa động vật - Thực vật
    var SumProtitFood = TotalProtitFoodAnimals + TotalProtitFoodPlants //Tổng số gam đạm
    var ProtitRatioAnimals = isNaN((TotalProtitFoodAnimals / SumProtitFood * 100).toFixed(0)) ? 0 : (TotalProtitFoodAnimals / SumProtitFood * 100).toFixed(0);
    var ProtitRatioPlants = isNaN((TotalProtitFoodPlants / SumProtitFood * 100).toFixed(0)) ? 0 : (TotalProtitFoodPlants / SumProtitFood * 100).toFixed(0);
    f.ProtitRatio.value = ProtitRatioAnimals.toString() + '% - ' + ProtitRatioPlants + '%';

    //Tính tỷ lệ đạm giữa động vật - Thực vật
    var SumLipitFood = TotalLipitFoodAnimals + TotalLipitFoodPlants //Tổng số gam béo
    var LipitRatioAnimals = isNaN((TotalLipitFoodAnimals / SumLipitFood * 100).toFixed(0)) ? 0 : (TotalLipitFoodAnimals / SumLipitFood * 100).toFixed(0);
    var LipitRatioPlants = isNaN((TotalLipitFoodPlants / SumLipitFood * 100).toFixed(0)) ? 0 : (TotalLipitFoodPlants / SumLipitFood * 100).toFixed(0);
    f.LipitRatio.value = LipitRatioAnimals.toString() + '% - ' + LipitRatioPlants + '%';

    //Tính các kết quả của thực đơn
    var ChildNumber = parseFloat(GetNumFromFormat(f.NumPersonForMeal.value)); //Lấy số xuất ăn

    //Tính kết quả dinh dưỡng trên một trẻ
    var CaloOnAChild = (TotalCaloFood / ChildNumber).toFixed(0);
    if (isNaN(CaloOnAChild)) CaloOnAChild = 0;
    var ProtitOnAChild = (SumProtitFood / ChildNumber).toFixed(0);
    if (isNaN(ProtitOnAChild)) ProtitOnAChild = 0;
    var LipitOnAChild = (SumLipitFood / ChildNumber).toFixed(0);
    if (isNaN(LipitOnAChild)) LipitOnAChild = 0;
    var GluxitOnAChild = (TotalGluxitFood / ChildNumber).toFixed(0);
    if (isNaN(GluxitOnAChild)) GluxitOnAChild = 0;

    var SumCalciumOnAChild = ( SumCalcium/ ChildNumber).toFixed(0);
    if (isNaN(SumCalciumOnAChild)) SumCalciumOnAChild = 0;

    var SumPhosphorOnAChild = ( SumPhosphor/ ChildNumber).toFixed(0);
    if (isNaN(SumPhosphorOnAChild)) SumPhosphorOnAChild = 0;

    var SumIronOnAChild = ( SumIron/ ChildNumber).toFixed(0);
    if (isNaN(SumIronOnAChild)) SumIronOnAChild = 0;

    //1 mcg Vitamin Beta Carôten = 0.167 mcg VitaminA
    var SumVitaminAOnAChild = (SumVitaminA / ChildNumber + SumVitaminBeta / ChildNumber*0.167).toFixed(0);
    if (isNaN(SumVitaminAOnAChild)) SumVitaminAOnAChild = 0;

    var SumVitaminB1OnAChild = (SumVitaminB1 / ChildNumber).toFixed(0);
    if (isNaN(SumVitaminB1OnAChild)) SumVitaminB1OnAChild = 0;

    var SumVitaminB2OnAChild = ( SumVitaminB2/ ChildNumber).toFixed(0);
    if (isNaN(SumVitaminB2OnAChild)) SumVitaminB2OnAChild = 0;

    var SumVitaminPPOnAChild = ( SumVitaminPP/ ChildNumber).toFixed(0);
    if (isNaN(SumVitaminPPOnAChild)) SumVitaminPPOnAChild = 0;

    var SumVitaminCOnAChild = (SumVitaminC / ChildNumber).toFixed(0);
    if (isNaN(SumVitaminCOnAChild)) SumVitaminCOnAChild = 0;

    f.CaloResult.value = FormatNum(CaloOnAChild);
    f.ProtitResult.value = FormatNum(ProtitOnAChild);
    f.LipitResult.value = FormatNum(LipitOnAChild);
    f.GluxitResult.value = FormatNum(GluxitOnAChild);

    f.CalciumResult.value = FormatNum(SumCalciumOnAChild);
    f.PhosphorResult.value = FormatNum(SumPhosphorOnAChild);
    f.IronResult.value = FormatNum(SumIronOnAChild);
    f.VitaminAResult.value = FormatNum(SumVitaminAOnAChild);

    f.VitaminB1Result.value = FormatNum(SumVitaminB1OnAChild);
    f.VitaminB2Result.value = FormatNum(SumVitaminB2OnAChild);

    f.VitaminPPResult.value = FormatNum(SumVitaminPPOnAChild);
    f.VitaminCResult.value = FormatNum(SumVitaminCOnAChild);


    //Lấy các định mức dinh dưỡng tại trường
    var CaloInSchool = parseFloat(GetNumFromFormat(f.CaloInSchoolResult.value));
    var ProtitInSchool = parseFloat(GetNumFromFormat(f.ProtitInSchoolResult.value));
    var LipitInSchool = parseFloat(GetNumFromFormat(f.LipitInSchoolResult.value));
    var GluxitInSchool = parseFloat(GetNumFromFormat(f.GluxitInSchoolResult.value));

    var CalciumDMTT = parseFloat(GetNumFromFormat(f.CalciumDMTT.value));
    var PhosphorDMTT = parseFloat(GetNumFromFormat(f.PhosphorDMTT.value));
    var IronDMTT = parseFloat(GetNumFromFormat(f.IronDMTT.value));
    var VitaminADMTT = parseFloat(GetNumFromFormat(f.VitaminADMTT.value));
    var VitaminB1DMTT = parseFloat(GetNumFromFormat(f.VitaminB1DMTT.value));
    var VitaminB2DMTT = parseFloat(GetNumFromFormat(f.VitaminB2DMTT.value));
    var VitaminPPDMTT = parseFloat(GetNumFromFormat(f.VitaminPPDMTT.value));
    var VitaminCDMTT = parseFloat(GetNumFromFormat(f.VitaminCDMTT.value));



    //Lấy các định mức dinh dưỡng trong ngày
    var CaloInHome = parseFloat(GetNumFromFormat(f.CaloInHomeResult.value));
    var ProtitInHome = parseFloat(GetNumFromFormat(f.ProtitInHomeResult.value));
    var LipitInHome = parseFloat(GetNumFromFormat(f.LipitInHomeResult.value));
    var GluxitInHome = parseFloat(GetNumFromFormat(f.GluxitInHomeResult.value));

    var CalciumDMTN = parseFloat(GetNumFromFormat(f.CalciumDMTN.value));
    var PhosphorDMTN = parseFloat(GetNumFromFormat(f.PhosphorDMTN.value));
    var IronDMTN = parseFloat(GetNumFromFormat(f.IronDMTN.value));
    var VitaminADMTN = parseFloat(GetNumFromFormat(f.VitaminADMTN.value));
    var VitaminB1DMTN = parseFloat(GetNumFromFormat(f.VitaminB1DMTN.value));
    var VitaminB2DMTN = parseFloat(GetNumFromFormat(f.VitaminB2DMTN.value));
    var VitaminPPDMTN = parseFloat(GetNumFromFormat(f.VitaminPPDMTN.value));
    var VitaminCDMTN = parseFloat(GetNumFromFormat(f.VitaminCDMTN.value));

    //Tính tỷ lệ % dinh dưỡng đạt được tại trường
    if (CaloInSchool != 0) {
        f.CaloRateInSchoolResult.value = FormatNum(isNaN((CaloOnAChild / CaloInSchool * 100).toFixed(0)) ? 0 : (CaloOnAChild / CaloInSchool * 100).toFixed(0)) + '%';
    }
    else {
        f.CaloRateInSchoolResult.value = '0%'; 
    }
    if (ProtitInSchool != 0) {
        f.ProtitRateInSchoolResult.value = FormatNum(isNaN((ProtitOnAChild / ProtitInSchool * 100).toFixed(0)) ? 0 : (ProtitOnAChild / ProtitInSchool * 100).toFixed(0)) + '%';
    }
    else {
        f.ProtitRateInSchoolResult.value = '0%';
    }
    if (LipitInSchool != 0) {
        f.LipitRateInSchoolResult.value = FormatNum(isNaN((LipitOnAChild / LipitInSchool * 100).toFixed(0)) ? 0 : (LipitOnAChild / LipitInSchool * 100).toFixed(0)) + '%';
    }
    else {
        f.LipitRateInSchoolResult.value = '0%';
    }
    if (LipitInSchool != 0) {

        f.GluxitRateInSchoolResult.value = FormatNum(isNaN((GluxitOnAChild / GluxitInSchool * 100).toFixed(0)) ? 0 : (GluxitOnAChild / GluxitInSchool * 100).toFixed(0)) + '%';
    }
    else {
        f.GluxitRateInSchoolResult.value = '0%';
    }

    if (CalciumDMTT != 0) {

        f.CalciumDMTTDD.value = FormatNum(isNaN((SumCalciumOnAChild / CalciumDMTT * 100).toFixed(0)) ? 0 : (SumCalciumOnAChild / CalciumDMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.CalciumDMTTDD.value = '0%';
    }

    if (PhosphorDMTT != 0) {

        f.PhosphorDMTTDD.value = FormatNum(isNaN((SumPhosphorOnAChild / PhosphorDMTT * 100).toFixed(0)) ? 0 : (SumPhosphorOnAChild / PhosphorDMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.PhosphorDMTTDD.value = '0%';
    }

    if (IronDMTT != 0) {

        f.IronDMTTDD.value = FormatNum(isNaN((SumIronOnAChild / IronDMTT * 100).toFixed(0)) ? 0 : (SumIronOnAChild / IronDMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.IronDMTTDD.value = '0%';
    }

    if (VitaminADMTT != 0) {

        f.VitaminADMTTDD.value = FormatNum(isNaN((SumVitaminAOnAChild / VitaminADMTT * 100).toFixed(0)) ? 0 : (SumVitaminAOnAChild / VitaminADMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminADMTTDD.value = '0%';
    }


    if (VitaminB1DMTT != 0) {

        f.VitaminB1DMTTDD.value = FormatNum(isNaN((SumVitaminB1OnAChild / VitaminB1DMTT * 100).toFixed(0)) ? 0 : (SumVitaminB1OnAChild / VitaminB1DMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminB1DMTTDD.value = '0%';
    }

    if (VitaminB2DMTT != 0) {

        f.VitaminB2DMTTDD.value = FormatNum(isNaN((SumVitaminB2OnAChild / VitaminB2DMTT * 100).toFixed(0)) ? 0 : (SumVitaminB2OnAChild / VitaminB2DMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminB2DMTTDD.value = '0%';
    }

    if (VitaminPPDMTT != 0) {

        f.VitaminPPDMTTDD.value = FormatNum(isNaN((SumVitaminPPOnAChild / VitaminPPDMTT * 100).toFixed(0)) ? 0 : (SumVitaminPPOnAChild / VitaminPPDMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminPPDMTTDD.value = '0%';
    }

    if (VitaminCDMTT != 0) {

        f.VitaminCDMTTDD.value = FormatNum(isNaN((SumVitaminCOnAChild / VitaminPPDMTT * 100).toFixed(0)) ? 0 : (SumVitaminCOnAChild / VitaminCDMTT * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminCDMTTDD.value = '0%';
    }
    
    //Tính tỷ lệ % dinh dưỡng đạt trong ngày
    if (CaloInHome != 0) {
        f.CaloRateInHomeResult.value = FormatNum(isNaN((CaloOnAChild / CaloInHome * 100).toFixed(0)) ? 0 : (CaloOnAChild / CaloInHome * 100).toFixed(0)) + '%';
    }
    else {
        f.CaloRateInHomeResult.value = '0%';
    }
    if (ProtitInHome != 0) {
        f.ProtitRateInHomeResult.value = FormatNum(isNaN((ProtitOnAChild / ProtitInHome * 100).toFixed(0)) ? 0 : (ProtitOnAChild / ProtitInHome * 100).toFixed(0)) + '%';
    }
    else {
        f.ProtitRateInHomeResult.value = '0%';
    }
    if (LipitInHome != 0) {
        f.LipitRateInHomeResult.value = FormatNum(isNaN((LipitOnAChild / LipitInHome * 100).toFixed(0)) ? 0 : (LipitOnAChild / LipitInHome * 100).toFixed(0)) + '%';
    }
    else {
        f.LipitRateInHomeResult.value = '0%';
    }
    if (GluxitInHome != 0) {
        f.GluxitRateInHomeResult.value = FormatNum(isNaN((GluxitOnAChild / GluxitInHome * 100).toFixed(0)) ? 0 : (GluxitOnAChild / GluxitInHome * 100).toFixed(0)) + '%';
    }
    else {
        f.GluxitRateInHomeResult.value = '0%';
    }

    if (CalciumDMTN != 0) {

        f.CalciumDMTNDD.value = FormatNum(isNaN((SumCalciumOnAChild / CalciumDMTN * 100).toFixed(0)) ? 0 : (SumCalciumOnAChild / CalciumDMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.CalciumDMTNDD.value = '0%';
    }

    if (PhosphorDMTN != 0) {

        f.PhosphorDMTNDD.value = FormatNum(isNaN((SumPhosphorOnAChild / PhosphorDMTN * 100).toFixed(0)) ? 0 : (SumPhosphorOnAChild / PhosphorDMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.PhosphorDMTNDD.value = '0%';
    }

    if (IronDMTN != 0) {

        f.IronDMTNDD.value = FormatNum(isNaN((SumIronOnAChild / IronDMTN * 100).toFixed(0)) ? 0 : (SumIronOnAChild / IronDMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.IronDMTNDD.value = '0%';
    }

    if (VitaminADMTN != 0) {

        f.VitaminADMTNDD.value = FormatNum(isNaN((SumVitaminAOnAChild / VitaminADMTN * 100).toFixed(0)) ? 0 : (SumVitaminAOnAChild / VitaminADMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminADMTNDD.value = '0%';
    }

    if (VitaminB1DMTN != 0) {

        f.VitaminB1DMTNDD.value = FormatNum(isNaN((SumVitaminB1OnAChild / VitaminB1DMTN * 100).toFixed(0)) ? 0 : (SumVitaminB1OnAChild / VitaminB1DMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminB1DMTNDD.value = '0%';
    }

    if (VitaminB2DMTN != 0) {

        f.VitaminB2DMTNDD.value = FormatNum(isNaN((SumVitaminB2OnAChild / VitaminB2DMTN * 100).toFixed(0)) ? 0 : (SumVitaminB2OnAChild / VitaminB2DMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminB2DMTNDD.value = '0%';
    }

    if (VitaminPPDMTN != 0) {

        f.VitaminPPDMTNDD.value = FormatNum(isNaN((SumVitaminPPOnAChild / VitaminPPDMTN * 100).toFixed(0)) ? 0 : (SumVitaminPPOnAChild / VitaminPPDMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminPPDMTNDD.value = '0%';
    }

    if (VitaminCDMTN != 0) {

        f.VitaminCDMTNDD.value = FormatNum(isNaN((SumVitaminCOnAChild / VitaminPPDMTN * 100).toFixed(0)) ? 0 : (SumVitaminCOnAChild / VitaminCDMTN * 100).toFixed(0)) + '%';
    }
    else {
        f.VitaminCDMTNDD.value = '0%';
    }

    //Tính Tỷ lệ P:L:G
    var SumCaloOnAChild = ProtitOnAChild * 4 + LipitOnAChild * 9 + GluxitOnAChild * 4; //Số calo sinh ra từ thực phẩm
    f.ProtitPLGRate.value = (isNaN((ProtitOnAChild * 400 / SumCaloOnAChild).toFixed(0)) ? 0 : (ProtitOnAChild * 400 / SumCaloOnAChild).toFixed(0))+'%';
    f.LipitPLGRate.value = (isNaN((LipitOnAChild * 900 / SumCaloOnAChild).toFixed(0)) ? 0 : (LipitOnAChild * 900 / SumCaloOnAChild).toFixed(0))+'%';
    f.GluxitPLGRate.value = (isNaN((GluxitOnAChild * 400 / SumCaloOnAChild).toFixed(0)) ? 0 : (GluxitOnAChild * 400 / SumCaloOnAChild).toFixed(0))+'%';

    //Tính tiền cho thực đơn
    var MoneyIn = parseFloat(GetNumFromFormat(f.MoneySumIn.value));
    var MoneyYesterday = parseFloat(GetNumFromFormat(f.MoneyYesterday.value));
    var MoneyOutOther = parseFloat(GetNumFromFormat(f.MoneyOutOther.value));
    f.MoneySumOut.value = FormatNum(TotalMoneyFood);
    f.MoneyNextday.value = FormatNum(MoneyIn + MoneyYesterday - MoneyOutOther - TotalMoneyFood);     
}

function ChangeMoney() {
    var TotalMoneyFood = parseFloat(GetNumFromFormat(document.QuanTriHeThong.MoneySumOut.value));
    var MoneyIn = parseFloat(GetNumFromFormat(document.QuanTriHeThong.MoneySumIn.value));
    var MoneyYesterday = parseFloat(GetNumFromFormat(document.QuanTriHeThong.MoneyYesterday.value));
    var MoneyOutOther = parseFloat(GetNumFromFormat(document.QuanTriHeThong.MoneyOutOther.value));
    document.QuanTriHeThong.MoneyNextday.value = FormatNum(MoneyIn + MoneyYesterday - MoneyOutOther - TotalMoneyFood);
}


function Trim(sString) {
    while (sString.substring(0, 1) == ' ') {
        sString = sString.substring(1, sString.length);
    }
    while (sString.substring(sString.length - 1, sString.length) == ' ') {
        sString = sString.substring(0, sString.length - 1);
    }
    return sString;
}



//1. Hàm đọc số có ba chữ số;
var ChuSo = new Array(" không ", " một ", " hai ", " ba ", " bốn ", " năm ", " sáu ", " bảy ", " tám ", " chín ");
var Tien = new Array("", " nghìn", " triệu", " tỷ", " nghìn tỷ", " triệu tỷ");
function DocSo3ChuSo(baso) {
    
    var tram;
    var chuc;
    var donvi;
    var KetQua = "";
    tram = parseInt(baso / 100);
    chuc = parseInt((baso % 100) / 10);
    donvi = baso % 10;
    if (tram == 0 && chuc == 0 && donvi == 0) return "";
    if (tram != 0) {
        KetQua += ChuSo[tram] + " trăm ";
        if ((chuc == 0) && (donvi != 0)) KetQua += " linh ";
    }
    if ((chuc != 0) && (chuc != 1)) {
        KetQua += ChuSo[chuc] + " mươi";
        if ((chuc == 0) && (donvi != 0)) KetQua = KetQua + " linh ";
    }
    if (chuc == 1) KetQua += " mười ";
    switch (donvi) {
        case 1:
            if ((chuc != 0) && (chuc != 1)) {
                KetQua += " mốt ";
            }
            else {
                KetQua += ChuSo[donvi];
            }
            break;
        case 5:
            if (chuc == 0) {
                KetQua += ChuSo[donvi];
            }
            else {
                KetQua += " lăm ";
            }
            break;
        default:
            if (donvi != 0) {
                KetQua += ChuSo[donvi];
            }
            break;
    }
    return KetQua;
}
//2. Hàm đọc số thành chữ (Sử dụng hàm đọc số có ba chữ số)
function DocTienBangChu(SoTien) {
    var lan = 0;
    var i = 0;
    var so = 0;
    var KetQua = "";
    var tmp = "";
    var ViTri = new Array();
    if (SoTien < 0) return "Số tiền âm !";
    if (SoTien == 0) return "Không đồng !";
    if (SoTien > 0) {
        so = SoTien;
    }
    else {
        so = -SoTien;
    }
    if (SoTien > 8999999999999999) {
        //SoTien = 0;
        return "Số quá lớn!";
    }
    ViTri[5] = Math.floor(so / 1000000000000000);
    if (isNaN(ViTri[5]))
        ViTri[5] = "0";
    so = so - parseFloat(ViTri[5].toString()) * 1000000000000000;
    ViTri[4] = Math.floor(so / 1000000000000);
    if (isNaN(ViTri[4]))
        ViTri[4] = "0";
    so = so - parseFloat(ViTri[4].toString()) * 1000000000000;
    ViTri[3] = Math.floor(so / 1000000000);
    if (isNaN(ViTri[3]))
        ViTri[3] = "0";
    so = so - parseFloat(ViTri[3].toString()) * 1000000000;
    ViTri[2] = parseInt(so / 1000000);
    if (isNaN(ViTri[2]))
        ViTri[2] = "0";
    ViTri[1] = parseInt((so % 1000000) / 1000);
    if (isNaN(ViTri[1]))
        ViTri[1] = "0";
    ViTri[0] = parseInt(so % 1000);
    if (isNaN(ViTri[0]))
        ViTri[0] = "0";
    if (ViTri[5] > 0) {
        lan = 5;
    }
    else if (ViTri[4] > 0) {
        lan = 4;
    }
    else if (ViTri[3] > 0) {
        lan = 3;
    }
    else if (ViTri[2] > 0) {
        lan = 2;
    }
    else if (ViTri[1] > 0) {
        lan = 1;
    }
    else {
        lan = 0;
    }
    for (i = lan; i >= 0; i--) {
        tmp = DocSo3ChuSo(ViTri[i]);
        KetQua += tmp;
        if (ViTri[i] > 0) KetQua += Tien[i];
        if ((i > 0) && (tmp.length > 0)) KetQua += ','; //&& (!string.IsNullOrEmpty(tmp))
    }
    if (KetQua.substring(KetQua.length - 1) == ',') {
        KetQua = KetQua.substring(0, KetQua.length - 1);
    }
    KetQua = KetQua.substring(1, 2).toUpperCase() + KetQua.substring(2);
    return KetQua; //.substring(0, 1);//.toUpperCase();// + KetQua.substring(1);
}