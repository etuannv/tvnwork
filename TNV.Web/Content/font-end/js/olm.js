(function(){
	var IM = {}, base_uri= $("body").attr("data-base"), ELM = $("#imessage"), loaded = false;
	function initModal(){
				$("#imsg-form").remove();
				var html = '<div class="modal fade" id="imsg-form">'
					+'<div class="modal-dialog"><div class="modal-content"><div class="modal-header">'
					+'<button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>'
					+'<strong class="modal-title" id="modal-title">Tin nhắn mới</strong>'
					+'</div>'
					+'<div class="modal-body" id="imsg-modal-body"><div id="imsg-status"></div><form style="margin: 0;"><fieldset><label>Đến:</label><div id="to_holder"></div><input placeholder="Tên người nhận (tên đăng nhập)" id="to" type="text" class="span5"/>'
					+'<label>Nội dung tin nhắn:</label><textarea placeholder="Nhập tin nhắn vào đây" rows="3" class="span5"></textarea></fieldset></form></div>'
					+'<div class="modal-footer">'
					+'<button type="button" id="btn-submit" class="btn btn-primary">Gửi tin nhắn</button>'
					+'<button type="button" class="btn btn-default" data-dismiss="modal">Hủy</button>'
					+'</div></div></div></div>';
					$(html).appendTo($("body").get(0));
	}
	IM.newMessage = function(to_username,name, avatar){
		initModal();
		$("#imsg-form").modal("show");
		$("#imsg-form .btn-primary").bind("click", function(){
			var _msg = $("#imsg-form textarea").val(), _to = $("#to").val();
			if(_to.length == 0){
				alert("Vui lòng nhập tên đăng nhập của người nhận");
				return false;
			}
			if(_msg.length == 0){
				alert("Vui lòng nhập nội dung tin nhắn");
				return false;
			}
			if(_msg.length > 255){
				alert("Tin nhắn quá dài, tin nhắn chỉ được có tối đa 255 chữ cái!");
				return false;
			}
			// send message
			$.ajax({
				url: "?g=message.send",
				type: "POST",
				data: { "to": _to, "msg": _msg},
				success: function(s){
					if(s == "OK"){
						alert("Tin nhắn của bạn đã được gửi đi");
						$("#imsg-form").modal("hide");
					}else{ $("#imsg-status").html("<p class='alert alert-error'>"+s+"</p>");}
				},
				error: function(){
					$("#imsg-status").html("<p class='alert alert-error'>Có lỗi xảy ra khi kết nối tới máy chủ, vui lòng thử lại!</p>");
				}
			});
		});
		if(to_username&&name){
			$("#to").val(to_username).hide();
			var css = "padding: 4px 8px; border-radius: 4px; box-shadow: 0px 1px 3px #999;margin-bottom: 10px; background: #FFF7CA; display: inline-block;";
			$("#to_holder").html("<div><img style='vertical-align: top; width: 40px; height: 40px;' src='"+avatar+"'/> <strong style='padding-bottom: 20px;'>"+name+"</strong></div>").attr("style",css);
		}
	}
	IM.initEvent = function(){
		$(".btn-imsg").click(function(){
			var that = $(this).closest(".msg-item"), avt = that.find("img.avatar-small").attr("src"), to = that.attr("data-from"), name = that.find("b.from_name").text();
			IM.newMessage(to,name,avt);
		});
		$(".msg-item .qa-content").click(function(){
			window.location.href= base_uri + "?l=message.byperson&id="+ $(this).closest(".msg-item").attr("data-from");
		});
	}
	IM.checkForMessage = function(){
		if(loaded) return;
		$.ajax({
			url: base_uri + "?g=message.LoadMessage",
			success: function(s){
				ELM.html(s);
				loaded = true;
				$(".imsg .imessage").removeClass("active");
				$(".imsg>.bad").remove();
				IM.initEvent();
			},
			error: function(){
				ELM.html("<p class='alert alert-error'>Có lỗi xảy ra khi xử lý dữ liệu !</p>");
			}
		});
	}
	
	$("#imessage-new").bind("click", function(event){IM.newMessage();});
	$(".imsg").bind("click", IM.checkForMessage);
	window.iMessage = IM.newMessage;
})();