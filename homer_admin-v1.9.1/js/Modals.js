function Info(title, message, subtitle) {
    $("#myModalInfo .modal-title").html(title);
    $("#myModalInfo .modal-body").html(message);
    $("#myModalInfo .modal-subtitle").html(subtitle);
	$("#BtnModalInfo").click();
}

function UIInfo(title, text){
	swal({ "title": title, "text": text });
}