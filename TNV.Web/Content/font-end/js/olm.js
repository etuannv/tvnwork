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

function replaceAll(find, replace, str) 
{
  while( str.indexOf(find) > -1)
  {
	str = str.replace(find, replace);
  }
  return str;
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
		htmlBuffer.push('	<label class="qradio');
		if(i==0) htmlBuffer.push(' checked');
		htmlBuffer.push('">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = replaceAll('$', ' ; ', dapAn);
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
    htmlBuffer.push( replaceAll("$"," ; ", dapAn));
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





function ajaxGetBaiToanDaySo(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanDaySo(result, displayTarget);
		generateBaiToanDaySoAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}





function displayBaiToanDaySo(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var ViTriDapAn = getRandomInt(0, 3);
	var dapAn = jsondata.NoiDungDapAn;
	var dapAnSai = jsondata.NoiDungDapAnSai;
	var htmlBuffer = [];
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2>');
	htmlBuffer.push(jsondata.CauHoiHienThi);
	htmlBuffer.push('</h2>');
	
	htmlBuffer.push('<h2>');
	var noiDungDay = replaceAll( '~', ' ; ' , jsondata.NoiDungDaySo)
	
	noiDungDay = replaceAll('...', '<img alt="Điền số thích hợp vào ô trống" title="Điền số thích hợp vào ô trống" class="otrong"  src="/Content/Image/OTrong.png") onmouseout="this.style.opacity=100" onmouseover="this.style.opacity=0.5" />', noiDungDay);
	
	
	htmlBuffer.push(noiDungDay);
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>Chọn câu trả lời:</h2>');
	htmlBuffer.push('<div id="questionctrl">');
	var temp = dapAn;
	var BoDapAnKhac = dapAnSai.split('#');
	var flagDapAnDung = 0;
	for (i = 0; i < 4; i++) {
		htmlBuffer.push('	<label class="qradio');
		if(i==0) htmlBuffer.push(' checked');
		htmlBuffer.push('">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = dapAn;
			flagDapAnDung = 1
		}
		else
		{
			if(flagDapAnDung == 1) temp = BoDapAnKhac[i-1];
			 else temp = BoDapAnKhac[i];
		}
		temp = replaceAll('~', ' ; ', temp);
		htmlBuffer.push(temp);
		htmlBuffer.push('"/>');
		htmlBuffer.push('		<div style="display: inline-block; padding-left: 10px; font-size: 18px;">');
		htmlBuffer.push(temp);
		htmlBuffer.push('</div>');	
		htmlBuffer.push('	</label>');
	}
	htmlBuffer.push('</div>');
    htmlBuffer.push('<input type="hidden" id="hdfDapAn" value="');
    htmlBuffer.push( replaceAll("~" , " ; " , dapAn));
    htmlBuffer.push('" />');
	
	
    displayTarget.append(htmlBuffer.join('\n'));
}



function generateBaiToanDaySoAnswer(data, answerTarget) {

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


function ajaxGetBaiToanThoiGian(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanThoiGian(result, displayTarget);
		generateBaiToanThoiGianAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayBaiToanThoiGian(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var ViTriDapAn = getRandomInt(0, 3);
	var dapAn = jsondata.DapAn;
	var dapAnSai = jsondata.DapAnSai;
	var htmlBuffer = [];
	
	var seconds = jsondata.Giay;
	var mins = jsondata.Phut;
	var hours = jsondata.Gio;
	// Tinh vong quay cho giay
	
	var sdegree = seconds * 6;
	var srotate = "rotate(" + sdegree + "deg)";
	var scss	= '-moz-transform:' + srotate + ';-webkit-transform:' + srotate;
	
	// Tinh vong quay cho phut
	
	var mdegree = mins * 6;
	var mrotate = "rotate(" + mdegree + "deg)";
	var mcss	= '-moz-transform:' + mrotate + ';-webkit-transform:' + mrotate;
	
	// Tinh vong quay cho gio
	
	var hdegree = hours * 30 + (mins / 2);
	var hrotate = "rotate(" + hdegree + "deg)";
	var hcss	= '-moz-transform:' + hrotate + ';-webkit-transform:' + hrotate;
	
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2>');
	htmlBuffer.push('Đồng hồ đang chỉ mấy giờ?');
	htmlBuffer.push('</h2>');
	
	htmlBuffer.push('<h2>');
	
	htmlBuffer.push('<ul class="clock">');
	htmlBuffer.push('	<li id="hour>" class="hour" style="');
	htmlBuffer.push(hcss);
	htmlBuffer.push('"></li>');
	htmlBuffer.push('	<li id="min>" class="min" style="');
	htmlBuffer.push(mcss);
	htmlBuffer.push('"></li>');
	htmlBuffer.push('	<li id="sec" class="sec" style="');
	htmlBuffer.push(scss);
	htmlBuffer.push('"></li>');
	htmlBuffer.push('</ul>');
	
	
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>Chọn câu trả lời:</h2>');
	htmlBuffer.push('<div id="questionctrl">');
	var temp = dapAn;
	var BoDapAnKhac = dapAnSai.split('#');
	var flagDapAnDung = 0;
	for (i = 0; i < 4; i++) {
		htmlBuffer.push('	<label class="qradio');
		if(i==0) htmlBuffer.push(' checked');
		htmlBuffer.push('">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = dapAn;
			flagDapAnDung = 1
		}
		else
		{
			if(flagDapAnDung == 1) temp = BoDapAnKhac[i-1];
			 else temp = BoDapAnKhac[i];
		}
		temp = replaceAll('~', ' ; ', temp);
		htmlBuffer.push(temp);
		htmlBuffer.push('"/>');
		htmlBuffer.push('		<div style="display: inline-block; padding-left: 10px; font-size: 18px;">');
		htmlBuffer.push(temp);
		htmlBuffer.push('</div>');	
		htmlBuffer.push('	</label>');
	}
	htmlBuffer.push('</div>');
    htmlBuffer.push('<input type="hidden" id="hdfDapAn" value="');
    htmlBuffer.push( replaceAll("~" , " ; " , dapAn));
    htmlBuffer.push('" />');
	
	
    displayTarget.append(htmlBuffer.join('\n'));
}

function generateBaiToanThoiGianAnswer(data, answerTarget) {

    var jsondata = JSON.parse(data);
    answerTarget.empty();
    var question = '';
	var dapSoNum = 1;
	
	
	question += ('<h3>Đáp án</h3>');
	question += '<p>';
	question += jsondata.DapAn;
	question += '</p>';
    answerTarget.append(question);    
}


function ajaxGetBaiToanGhepO(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanGhepO(result, displayTarget);
		generateBaiToanGhepOAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayBaiToanGhepO(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var htmlBuffer = [];
	var dem = 0;
	var m = 0;
	var n = 0;
	var chieudoc = jsondata.ChieuDoc;
	var chieungang = jsondata.ChieuNgang;
	var bieuThucArr = jsondata.NoiDungBaiToan.split('$');
	
	
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2 style="overflow:hidden;">');
	
	for(m = 1; m <= chieudoc; m++)
	{
		for(n = 1; n <= chieungang; n++)
		{
			dem++;
			
			htmlBuffer.push('	<div class="HopBieuThuc">');
			htmlBuffer.push('		<div class="STTHop" title="Biểu thức này có số thứ tự là '); htmlBuffer.push(dem); htmlBuffer.push('">')
			htmlBuffer.push(dem)
			htmlBuffer.push('		</div>');
			htmlBuffer.push('		<div class="BieuThuc" title="Biểu thức này có số thứ tự là '); htmlBuffer.push(dem); htmlBuffer.push('">')
			htmlBuffer.push(bieuThucArr[dem-1]);
			htmlBuffer.push('</div>');
			htmlBuffer.push('	</div>');
			
		}
	}
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>');
	htmlBuffer.push('Điền các ô có kết quả phép toán bằng nhau?');
	htmlBuffer.push('</h2>');
	htmlBuffer.push('<div id="questionctrl" style="padding-bottom: 50px; float:left;">');
	for(m = 1; m <= (chieudoc * chieungang) / 2; m++)
	{
			htmlBuffer.push('	<div class="HopDapAn">');
			
			htmlBuffer.push('	<input type="text" class="dapso" value="" name="txtDapSo"' + 'id="txtDapSo' + m + '1' + '">');
			htmlBuffer.push('bằng')
			htmlBuffer.push('	<input type="text" class="dapso" value="" name="txtDapSo"' + 'id="txtDapSo' + m + '2' + '">');
			htmlBuffer.push('	</div>');
	}
	htmlBuffer.push('</div>');
    htmlBuffer.push('<input type="hidden" id="hdfDapAn" value="' + jsondata.NoiDungDapAn + '" />');

    displayTarget.append(htmlBuffer.join('\n'));
	$('#txtDapSo11').focus();
}



function generateBaiToanGhepOAnswer(data, answerTarget) {
	var jsondata = JSON.parse(data);
	var m = 0;
	var n = 0;
	var chieudoc = jsondata.ChieuDoc;
	var chieungang = jsondata.ChieuNgang;
    var htmlBuffer = [];
	var dapAnArr = jsondata.NoiDungDapAn.split('$');
	
	answerTarget.empty();
	htmlBuffer.push('<h3>Đáp án đúng là:</h3>');
	for(m = 1; m <= (chieudoc * chieungang) / 2; m++)
	{
			var MangPhanTu = dapAnArr[m-1].split(';')
			htmlBuffer.push('	<div class="HopDapAn">');
			
			htmlBuffer.push('	<input type="text" class="dapan" readonly value="' + MangPhanTu[0] + '" name="txtDapSo"' + 'id="txtDapSo' + m + '1' + '">');
			htmlBuffer.push('bằng')
			htmlBuffer.push('	<input type="text" class="dapan" readonly value="' + MangPhanTu[1] + '" name="txtDapSo"' + 'id="txtDapSo' + m + '2' + '">');
			htmlBuffer.push('	</div>');
			
	}
	htmlBuffer.push('</div>');
	
    answerTarget.append(htmlBuffer.join('\n'));    
}

function ajaxGetBaiToanSapXep(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanSapXep(result, displayTarget);
		generateBaiToanSapXepAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayBaiToanSapXep(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
    var question = '';
	var htmlBuffer = [];
	var dem = 0;
	var m = 0;
	var n = 0;
	var chieudoc = jsondata.ChieuDoc;
	var chieungang = jsondata.ChieuNgang;
	var bieuThucArr = jsondata.NoiDungBaiToan.split('$');
	
	
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2 style="overflow:hidden;">');
	
	for(m = 1; m <= chieudoc; m++)
	{
		for(n = 1; n <= chieungang; n++)
		{
			dem++;
			
			htmlBuffer.push('	<div class="HopBieuThuc">');
			htmlBuffer.push('		<div class="STTHop" title="Biểu thức này có số thứ tự là '); htmlBuffer.push(dem); htmlBuffer.push('">')
			htmlBuffer.push(dem)
			htmlBuffer.push('		</div>');
			htmlBuffer.push('		<div class="BieuThuc" title="Biểu thức này có số thứ tự là '); htmlBuffer.push(dem); htmlBuffer.push('">')
			htmlBuffer.push(bieuThucArr[dem-1]);
			htmlBuffer.push('</div>');
			htmlBuffer.push('	</div>');
			
		}
	}
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>');
	htmlBuffer.push('Điền các thứ tự các ô có kết quả biểu thức từ bé đến lớn?');
	htmlBuffer.push('</h2>');
	htmlBuffer.push('<div id="questionctrl" style="padding-bottom: 50px; float:left;">');
	for(m = 1; m <= (chieudoc * chieungang); m++)
	{
			htmlBuffer.push('	<div class="HopDapAnSapXep">');
			htmlBuffer.push('	<input type="text" class="dapso" value="" name="txtDapSo"' + 'id="txtDapSo' + m + '1' + '">');
			htmlBuffer.push('	</div>');
			if(m !=  (chieudoc * chieungang))
			{
				htmlBuffer.push('<div style="float:left; font-size:38px; padding: 2px;"><</div>')
			}
	}
	htmlBuffer.push('</div>');
    htmlBuffer.push('<input type="hidden" id="hdfDapAn" value="' + jsondata.NoiDungDapAn + '" />');
	
    displayTarget.append(htmlBuffer.join('\n'));
	$('#txtDapSo11').focus();
}



function generateBaiToanSapXepAnswer(data, answerTarget) {
	var jsondata = JSON.parse(data);
	var m = 0;
	var n = 0;
	var chieudoc = jsondata.ChieuDoc;
	var chieungang = jsondata.ChieuNgang;
    var htmlBuffer = [];
	var dapAnArr = jsondata.NoiDungDapAn.split('$');
	
	answerTarget.empty();
	htmlBuffer.push('<h3>Đáp án đúng là:</h3>');
	for(m = 1; m <= (chieudoc * chieungang); m++)
	{
			htmlBuffer.push('	<div class="HopDapAnSapXep">');
			htmlBuffer.push('	<input type="text" class="dapan" readonly value="' + dapAnArr[m-1] + '" name="txtDapSo"' + 'id="txtDapSo' + m + '1' + '">');
			htmlBuffer.push('	</div>');
			if(m !=  (chieudoc * chieungang))
			{
				htmlBuffer.push('<div style="float:left; font-size:38px; padding: 2px;"><</div>')
			}
	}
	htmlBuffer.push('</div>');
	
    answerTarget.append(htmlBuffer.join('\n'));    
}

function ajaxGetBaiToanTimSo(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanTimSo(result, displayTarget);
		generateBaiToanTimSoAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayBaiToanTimSo(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
	var htmlBuffer = [];
	var controlString = '';
	var dapAn = jsondata.DapAn;
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2 style="overflow:hidden; line-height:17px;">');
	
	switch (jsondata.UserControlName) {
		case 'BonSoHinhVuong':
			controlString = buildBonSoHinhVuongString(jsondata);
		break;
		
		case 'BonSoHinhVuong1':
			controlString = buildBonSoHinhVuong1String(jsondata);
		break;
		
		case 'ElipNamSo':
			controlString = buildElipNamSoString(jsondata);
		break;
		
		case 'HinhTronBaSo':
			controlString = buildHinhTronBaSoString(jsondata);
		break;
		
		case 'HinhTronBonSo':
			controlString = buildHinhTronBonSoString(jsondata);
		break;
		
		case 'HinhTronBonSoChiaTu':
			controlString = buildHinhTronBonSoChiaTuString(jsondata);
		break;
		
		case 'HinhVuongNamSo':
			controlString = buildHinhVuongNamSoString(jsondata);
		break;
		
		case 'HinhVuongTamGiac':
			controlString = buildHinhVuongTamGiacString(jsondata);
		break;
		
		case 'LucGiacBaySo':
			controlString = buildLucGiacBaySoString(jsondata);
		break;
		
		case 'TamGiacHinhTronBonSo':
			controlString = buildTamGiacHinhTronBonSoString(jsondata);
		break;
		
		case 'TamGiacNoiTiepBonSo':
			controlString = buildTamGiacNoiTiepBonSoString(jsondata);
		break;
		
		case 'VongTronLucGiacBaySo':
			controlString = buildVongTronLucGiacBaySoString(jsondata);
		break;
	}
	
	htmlBuffer.push(controlString);
	
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>Chọn câu trả lời:</h2>');
	htmlBuffer.push('<div id="questionctrl">');
	var temp = dapAn;
	var ViTriDapAn = getRandomInt(0, 3);
	var BoDapAnKhac = gen3SoKhac(dapAn);
	var flagDapAnDung = 0;
	for (i = 0; i < 4; i++) {
		htmlBuffer.push('	<label class="qradio');
		if(i==0) htmlBuffer.push(' checked');
		htmlBuffer.push('">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = dapAn
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
    htmlBuffer.push(dapAn);
    htmlBuffer.push('" />');
	
	
    displayTarget.append(htmlBuffer.join('\n'));
}



function generateBaiToanTimSoAnswer(data, answerTarget) {
	var jsondata = JSON.parse(data);
    var htmlBuffer = [];
	answerTarget.empty();
	
	htmlBuffer.push('<h3>Lời giải</h3>');
	htmlBuffer.push('<p>' + jsondata.LoiGiaiBaiToan + '</p>');
	htmlBuffer.push('<h3>Đáp án</h3>');
	htmlBuffer.push('<p>' + jsondata.DapAn + '</p>');
    answerTarget.append(htmlBuffer.join('\n')); 

}


function buildBonSoHinhVuongString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="BonSoHinhVuong">');
		MyBuffer.push('		<div class="BonSoHinhVuong_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}


function buildBonSoHinhVuong1String(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="BonSoHinhVuong1">');
		MyBuffer.push('		<div class="BonSoHinhVuong1_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong1_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong1_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="BonSoHinhVuong1_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}

function buildElipNamSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="ElipNamSo">');
		MyBuffer.push('		<div class="ElipNamSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="ElipNamSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="ElipNamSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="ElipNamSo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('		<div class="ElipNamSo_So5">' + CacSo[4] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildHinhTronBaSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="HinhTronBaSo">');
		MyBuffer.push('		<div class="HinhTronBaSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="HinhTronBaSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="HinhTronBaSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildHinhTronBonSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="HinhTronBonSo">');
		MyBuffer.push('		<div class="HinhTronBonSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	
	return MyBuffer.join('\n');
}
function buildHinhTronBonSoChiaTuString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="HinhTronBonSoChiaTu">');
		MyBuffer.push('		<div class="HinhTronBonSoChiaTu_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSoChiaTu_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSoChiaTu_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="HinhTronBonSoChiaTu_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildHinhVuongNamSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="HinhVuongNamSo">');
		MyBuffer.push('		<div class="HinhVuongNamSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="HinhVuongNamSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="HinhVuongNamSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="HinhVuongNamSo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('		<div class="HinhVuongNamSo_So5">' + CacSo[4] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildHinhVuongTamGiacString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="HinhVuongTamGiac">');
		MyBuffer.push('		<div class="HinhVuongTamGiac_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="HinhVuongTamGiac_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="HinhVuongTamGiac_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildLucGiacBaySoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="LucGiacBaySo">');
		MyBuffer.push('		<div class="LucGiacBaySo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So5">' + CacSo[4] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So6">' + CacSo[5] + '</div>');
		MyBuffer.push('		<div class="LucGiacBaySo_So7">' + CacSo[6] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildTamGiacHinhTronBonSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="TamGiacHinhTronBonSo">');
		MyBuffer.push('		<div class="TamGiacHinhTronBonSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="TamGiacHinhTronBonSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="TamGiacHinhTronBonSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="TamGiacHinhTronBonSo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildTamGiacNoiTiepBonSoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="TamGiacNoiTiepBonSo">');
		MyBuffer.push('		<div class="TamGiacNoiTiepBonSo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="TamGiacNoiTiepBonSo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="TamGiacNoiTiepBonSo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="TamGiacNoiTiepBonSo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}
function buildVongTronLucGiacBaySoString(data){
	var MyBuffer =[];
	var CacDay = data.ChuoiSoHienThi.split('$');
	for(i =0; i < CacDay.length; i++)
	{
		var CacSo = CacDay[i].split(';');
		MyBuffer.push('<div class="VongTronLucGiacBaySo">');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So1">' + CacSo[0] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So2">' + CacSo[1] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So3">' + CacSo[2] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So4">' + CacSo[3] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So5">' + CacSo[4] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So6">' + CacSo[5] + '</div>');
		MyBuffer.push('		<div class="VongTronLucGiacBaySo_So7">' + CacSo[6] + '</div>');
		MyBuffer.push('</div>');
	}
	return MyBuffer.join('\n');
}




















































function ajaxGetBaiToanDemHinh(pUrl, displayTarget, answerTarget) {
    $.ajax({
        url: pUrl,
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html'
    })
    .success(function (result, target) {
        displayBaiToanDemHinh(result, displayTarget);
		generateBaiToanDemHinhAnswer(result, answerTarget)
    })
    .error(function (xhr, status) {
        alert(status);
    })
}

function displayBaiToanDemHinh(data, displayTarget) {

    var jsondata = JSON.parse(data);
    displayTarget.empty();
	var htmlBuffer = [];
	var controlString = '';
	var dapAn = jsondata.DapAnBaiToan;
    // Sinh noi dung cau hoi
	htmlBuffer.push('<h2 style="overflow:hidden; line-height:27px;">');
	htmlBuffer.push(jsondata.NoiDungBaiToan);
	htmlBuffer.push('</h2>');
	
	// Sinh noi dung cau tra loi
	
	htmlBuffer.push('<br\>');
	htmlBuffer.push('<h2>Chọn câu trả lời:</h2>');
	htmlBuffer.push('<div id="questionctrl">');
	var temp = dapAn;
	var ViTriDapAn = getRandomInt(0, 3);
	var BoDapAnKhac = gen3SoKhac(dapAn);
	var flagDapAnDung = 0;
	for (i = 0; i < 4; i++) {
		htmlBuffer.push('	<label class="qradio');
		if(i==0) htmlBuffer.push(' checked');
		htmlBuffer.push('">');
		htmlBuffer.push('		<input type="radio" id="');
		htmlBuffer.push(i);
		htmlBuffer.push('" name="questionctrl" value="');
		if(i == ViTriDapAn)
		{
			temp = dapAn
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
    htmlBuffer.push(dapAn);
    htmlBuffer.push('" />');
	
    displayTarget.append(htmlBuffer.join('\n'));
}



function generateBaiToanDemHinhAnswer(data, answerTarget) {
	var jsondata = JSON.parse(data);
    var htmlBuffer = [];
	answerTarget.empty();
	
	if( jsondata.LoiGiaiBaiToan != '')
	{
		htmlBuffer.push('<h3>Lời giải</h3>');
		htmlBuffer.push('<p>' + jsondata.LoiGiaiBaiToan + '</p>');
	}
	htmlBuffer.push('<h3>Đáp án</h3>');
	htmlBuffer.push('<p>' + jsondata.DapAnBaiToan + '</p>');
    answerTarget.append(htmlBuffer.join('\n')); 

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