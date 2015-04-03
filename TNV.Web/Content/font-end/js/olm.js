function increaseNum(hdfSource, destination, increNo) {
    destination.empty();
    var count = parseInt(hdfSource.val());
    destination.empty();
    var result = count + parseInt(increNo);
    destination.append(result);
    hdfSource.val(result);
}

function ajaxGet(pUrl, displayTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayPhepToan2SoHang(result, displayTarget);
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayPhepToan2SoHang(data, displayTarget) {
    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	question += '<h2>Điền số hoặc dấu thích hợp vào ô trống ? </h2>'
	question += '<h2>';
    if (jsondata.SoHangThuNhat == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo">';
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuNhat;
		question += '</span>';
    }

    if (jsondata.PhepToan == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo">';
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.PhepToan;
		question += '</span>';
    }

    if (jsondata.SoHangThuHai == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo">';
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuHai;
		question += '</span>';
    }

    if (jsondata.DauQuanHe == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo">';
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.DauQuanHe;
		question += '</span>';
    }

    if (jsondata.KetQuaPhepToan == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo">';
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.KetQuaPhepToan;
		question += '</span>';
    }

    question += '<input type="hidden" id="hdfDapAn" value="';
    question += jsondata.DapAn;
    question += '" />';
	question += '</h2>';
    displayTarget.append(question);
    $('#txtDapSo').focus();
}




function ajaxGetPhepToan3SoHang(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayPhepToan3SoHang(result, displayTarget);
		generateAnswerPhepToan3SoHang(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayPhepToan3SoHang(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var dapSoNum = 1;
	
	question += '<h2>Điền số hoặc dấu thích hợp vào ô trống ? </h2>'
	question += '<h2>';
    if (jsondata.SoHangThuNhat == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuNhat;
		question += '</span>';
    }

    if (jsondata.PhepToanThuNhat == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.PhepToanThuNhat;
		question += '</span>';
    }

    if (jsondata.SoHangThuHai == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuHai;
		question += '</span>';
    }
	
	if (jsondata.PhepToanThuHai == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.PhepToanThuHai;
		question += '</span>';
    }
	
	if (jsondata.SoHangThuBa == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuBa;
		question += '</span>';
    }

    if (jsondata.QuanHePhepToan == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.QuanHePhepToan;
		question += '</span>';
    }

    if (jsondata.KetQuaPhepToan == '?') {
        question += '<input type="text" value="" name="txtDapSo" id="txtDapSo'
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.KetQuaPhepToan;
		question += '</span>';
    }

    question += '<input type="hidden" id="hdfDapAn1" value="';
    question += jsondata.DapAnThuNhat;
    question += '" />';
	
	question += '<input type="hidden" id="hdfDapAn2" value="';
    question += jsondata.DapAnThuHai;
    question += '" />';
	question += '</h2>';
    displayTarget.append(question);
    $('#txtDapSo1').focus();
}


function generateAnswerPhepToan3SoHang(data, answerTarget) {

    var jsondata = JSON.parse(data);
    answerTarget.empty();
    var question = '';
	var dapSoNum = 1;
	
	question += ('<h3>Đáp án</h3>');
    if (jsondata.SoHangThuNhat == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuNhat;
		question += '</span>';
    }

    if (jsondata.PhepToanThuNhat == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.PhepToanThuNhat;
		question += '</span>';
    }

    if (jsondata.SoHangThuHai == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuHai;
		question += '</span>';
    }
	
	if (jsondata.PhepToanThuHai == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.PhepToanThuHai;
		question += '</span>';
    }
	
	if (jsondata.SoHangThuBa == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.SoHangThuBa;
		question += '</span>';
    }

    if (jsondata.QuanHePhepToan == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.QuanHePhepToan;
		question += '</span>';
    }

    if (jsondata.KetQuaPhepToan == '?') {
        question += '<input type="text" value="';
		if( dapSoNum ==1 ) question += jsondata.DapAnThuNhat;
		else  question += jsondata.DapAnThuHai;
		question += '" name="txtDapSo" readonly id="txtDapSo';
		question += dapSoNum;
		question += '">';
		dapSoNum += 1;
    }
    else {
		question += '<span class="sohang">';
        question += jsondata.KetQuaPhepToan;
		question += '</span>';
    }
	
    answerTarget.append(question);    
}



function ajaxGetBaiToanThemBot(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanThemBot(result, displayTarget);
		generateBaiToanThemBotAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function getRandomInt(min, max) {
  return Math.floor(Math.random() * (max - min + 1)) + min;
}

function gen3SoKhac(soDauVao)
{
	var dauVao = parseInt(soDauVao);
	var So1;
	var So2;
	var So3;
	var KetQua = [];
	var MinRange = 0 - dauVao;
	var MaxRange = dauVao;
	if(dauVao < 2) { MinRange = 0; MaxRange = 5;}
	if(2 < dauVao && dauVao > 30){ MinRange = 0-dauVao; MaxRange = dauVao;}
	if(dauVao > 30){ MinRange = -30; MaxRange = 30;}
	
	SoRandom = getRandomInt(MinRange, MaxRange);
	if(SoRandom == 0) SoRandom += 1;
	So1 = dauVao + SoRandom;
	do
	{
		SoRandom = getRandomInt(MinRange, MaxRange);
		if(SoRandom == 0) SoRandom += 1;
		So2 = dauVao + SoRandom;
	}
	while(So2 == So1)
	
	do
	{
		SoRandom = getRandomInt(MinRange, MaxRange);
		if(SoRandom == 0) SoRandom += 1;
		So3 = dauVao + SoRandom;
	}
	while(So3 == So1 || So3 == So2)
	
	KetQua.push(So1);
	KetQua.push(So2);
	KetQua.push(So3);

	return KetQua;
}



function sinhCauTraLoiBaiToanThemBot(dapAnDung){
	var KetQuaTemp1 ='';
	var KetQuaTemp2 ='';
	var KetQuaTemp3 ='';
	var arrString = dapAnDung.split("$"); 
	var arrKetQua = [];

	for(j = 0; j < arrString.length; j++)
	{
		if(j != 0)
		{
			KetQuaTemp1 += ' ; ';
			KetQuaTemp2 += ' ; ';
			KetQuaTemp3 += ' ; ';
		}
		var arr3SoKhac = gen3SoKhac(arrString[j]);
		KetQuaTemp1 += String(arr3SoKhac[0]);
		KetQuaTemp2 += String(arr3SoKhac[1]);
		KetQuaTemp3 += String(arr3SoKhac[2]);
	}
	arrKetQua.push(KetQuaTemp1);
	arrKetQua.push(KetQuaTemp2);
	arrKetQua.push(KetQuaTemp3);
	return arrKetQua;
}

function displayBaiToanThemBot(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var ViTriDapAn = getRandomInt(0, 3);
	var dapAn = jsondata.DapAnCauHoi;
	var htmlBuffer = [];
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2>');
	htmlBuffer.push(jsondata.NoiDungCauHoi);
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>Chọn câu trả lời:</h2>');
	htmlBuffer.push('<div id="questionctrl">');
	var temp = dapAn;
	var BoDapAnKhac = sinhCauTraLoiBaiToanThemBot(dapAn);
	var flagDapAnDung = 0;
	for (i = 0; i < 4; i++) {
		htmlBuffer.push('	<label class="qradio">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = dapAn.replace("$"," ; ");
			flagDapAnDung = 1
		}
		else
		{
			if(flagDapAnDung == 1) temp = BoDapAnKhac[i-1];
			 else temp = BoDapAnKhac[i];
		}
		htmlBuffer.push(temp);
		htmlBuffer.push('"/>');
		htmlBuffer.push('		<div style="display: inline-block; padding-left: 10px; font-size: 18px;">');
		htmlBuffer.push(temp);
		htmlBuffer.push('</div>');	
		htmlBuffer.push('	</label>');
	}
	htmlBuffer.push('</div>');
    htmlBuffer.push('<input type="hidden" id="hdfDapAn" value="');
    htmlBuffer.push(dapAn.replace("$"," ; "));
    htmlBuffer.push('" />');
	
	
    displayTarget.append(htmlBuffer.join('\n'));
}



function generateBaiToanThemBotAnswer(data, answerTarget) {

    var jsondata = JSON.parse(data);
    answerTarget.empty();
    var question = '';
	var dapSoNum = 1;
	
	question += ('<h3>Lời giải</h3>');
	question += '<p>';
	question += jsondata.LoiGiaiCauHoi;
	question += '</p>';
	
	
	question += ('<h3>Đáp án</h3>');
	question += '<p>';
	question += jsondata.KetLuanCauHoi;
	question += '</p>';
    answerTarget.append(question);    
}








//(function () {
//    var IM = {}, base_uri = $("body").attr("data-base"), ELM = $("#imessage"), loaded = false;
//    function initModal() {
//        $("#imsg-form").remove();
//        var html = '<div class="modal fade" id="imsg-form">'
//					+ '<div class="modal-dialog"><div class="modal-content"><div class="modal-header">'
//					+ '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
//					+ '<strong class="modal-title" id="modal-title">Tin nhắn mới</strong>'
//					+ '</div>'
//					+ '<div class="modal-body" id="imsg-modal-body"><div id="imsg-status"></div><form style="margin: 0;"><fieldset><label>Đến:</label><div id="to_holder"></div><input placeholder="Tên người nhận (tên đăng nhập)" id="to" type="text" class="span5"/>'
//					+ '<label>Nội dung tin nhắn:</label><textarea placeholder="Nhập tin nhắn vào đây" rows="3" class="span5"></textarea></fieldset></form></div>'
//					+ '<div class="modal-footer">'
//					+ '<button type="button" id="btn-submit" class="btn btn-primary">Gửi tin nhắn</button>'
//					+ '<button type="button" class="btn btn-default" data-dismiss="modal">Hủy</button>'
//					+ '</div></div></div></div>';
//        $(html).appendTo($("body").get(0));
//    }
//    IM.newMessage = function (to_username, name, avatar) {
//        initModal();
//        $("#imsg-form").modal("show");
//        $("#imsg-form .btn-primary").bind("click", function () {
//            var _msg = $("#imsg-form textarea").val(), _to = $("#to").val();
//            if (_to.length == 0) {
//                alert("Vui lòng nhập tên đăng nhập của người nhận");
//                return false;
//            }
//            if (_msg.length == 0) {
//                alert("Vui lòng nhập nội dung tin nhắn");
//                return false;
//            }
//            if (_msg.length > 255) {
//                alert("Tin nhắn quá dài, tin nhắn chỉ được có tối đa 255 chữ cái!");
//                return false;
//            }
//            // send message
//            $.ajax({
//                url: "?g=message.send",
//                type: "POST",
//                data: { "to": _to, "msg": _msg },
//                success: function (s) {
//                    if (s == "OK") {
//                        alert("Tin nhắn của bạn đã được gửi đi");
//                        $("#imsg-form").modal("hide");
//                    } else { $("#imsg-status").html("<p class='alert alert-error'>" + s + "</p>"); }
//                },
//                error: function () {
//                    $("#imsg-status").html("<p class='alert alert-error'>Có lỗi xảy ra khi kết nối tới máy chủ, vui lòng thử lại!</p>");
//                }
//            });
//        });
//        if (to_username && name) {
//            $("#to").val(to_username).hide();
//            var css = "padding: 4px 8px; border-radius: 4px; box-shadow: 0px 1px 3px #999;margin-bottom: 10px; background: #FFF7CA; display: inline-block;";
//            $("#to_holder").html("<div><img style='vertical-align: top; width: 40px; height: 40px;' src='" + avatar + "'/> <strong style='padding-bottom: 20px;'>" + name + "</strong></div>").attr("style", css);
//        }
//    }
//    IM.initEvent = function () {
//        $(".btn-imsg").click(function () {
//            var that = $(this).closest(".msg-item"), avt = that.find("img.avatar-small").attr("src"), to = that.attr("data-from"), name = that.find("b.from_name").text();
//            IM.newMessage(to, name, avt);
//        });
//        $(".msg-item .qa-content").click(function () {
//            window.location.href = base_uri + "?l=message.byperson&id=" + $(this).closest(".msg-item").attr("data-from");
//        });
//    }
//    IM.checkForMessage = function () {
//        if (loaded) return;
//        $.ajax({
//            url: base_uri + "?g=message.LoadMessage",
//            success: function (s) {
//                ELM.html(s);
//                loaded = true;
//                $(".imsg .imessage").removeClass("active");
//                $(".imsg>.bad").remove();
//                IM.initEvent();
//            },
//            error: function () {
//                ELM.html("<p class='alert alert-error'>Có lỗi xảy ra khi xử lý dữ liệu !</p>");
//            }
//        });
//    }

//    $("#imessage-new").bind("click", function (event) { IM.newMessage(); });
//    $(".imsg").bind("click", IM.checkForMessage);
//    window.iMessage = IM.newMessage;
//})();