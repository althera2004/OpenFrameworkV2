function Info(title, message, subtitle) {
    $("#myModalInfo .modal-title").html(title);
    $("#myModalInfo .modal-body").html(message);
    $("#myModalInfo .modal-subtitle").html(subtitle);
	$("#BtnModalInfo").click();
}

function UIInfo(title, text){
	swal({ "title": title, "text": text });
}

function Error(title, message, subtitle) {
    $("#myModalWarning .modal-title").html(title);
    $("#myModalWarning .modal-body").html(message);
    $("#myModalWarning .modal-subtitle").html(subtitle);
    $("#BtnModalWarning").click();
}