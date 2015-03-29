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
                    <span class="bc-item bc-first"><a href="http://olm.vn/">
                        <img class="bc-logo" src="http://olm.vn/images/olm.png">Online Math</a></span><span
                            class="bc-item"><a href="http://olm.vn/hoctoan">Học toán</a></span><span class="bc-item"><a
                                href="http://olm.vn/toan-lop-1">Lớp 1</a></span></div>
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
                <div class="row">
                    <div style="" class="span5">
                        <div class="content-box">
                            <div class="content-title grad-blue">
                                <a href="/hoi-dap">Phép toán thêm bớt</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="span5 ct-block">
                        <div class="content-box">
                            <div class="content-title grad-orange text-center">
                                <a href="/Toan-vui-hang-tuan">Bài toán hai đối tượng</a>
                            </div>
                            <div class="feature">
                                <div class="scroll">
                                </div>
                            </div>
                        </div>
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
