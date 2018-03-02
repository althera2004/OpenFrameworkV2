function OpenImageUploadPopup(imageFile, name, fieldName) {
	$("#MyModalImageErrorMessageDiv").hide();
	$("#MyModalImageErrorMessage").html("");
    if (typeof Data.Id === "undefined" || Data.Id < 0) {
        toastr.error('Primero debe guardar el item');
        return false;
    }
	
	if(imageFile !== ""){
		$("#MyModalImageAction").html(Dictionary.Common_Replace);
	}
	else{		
		$("#MyModalImageAction").html(Dictionary.Common_Add);
	}

    $("#UploadImageFieldName").html(fieldName);
    var actual = document.getElementById("UploadImage1");
    var nueva = document.getElementById("UploadImage2");
    actual.src = imageFile + "?" + Math.random();
    actual.alt = name;
    actual.title = name;
    nueva.src = "/img/noimage.png";
    nueva.alt = fieldName;
    nueva.title = Dictionary.Common_SelectImage;
    $("#BtnModalImage").click();
    $("#UploadImageFile").removeAttr("change");
    $("#UploadImageFile").on("change", function () { readURLImage(this); });

    console.log(fieldName);
}

function readURLImage(input) {
    console.log("readUrlImage", input.id);
    $("#MyModalImageErrorMessageDiv").hide();
	$("#MyModalImageErrorMessage").html("");
    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) { $("#UploadImage2").attr("src", e.target.result); };
        reader.readAsDataURL(input.files[0]);
    }
}
function UploadImageValidation() {
    var source = document.getElementById("UploadImage2");
    if (source.src.indexOf("noimage.png") !== -1) {		
		$("#MyModalImageErrorMessageDiv").show();
		$("#MyModalImageErrorMessage").html("Hay que seleccionar una imagen");
        return false;
    }

    UploadImageGo();
}

var UploadImageData = null;
function UploadImageGo() {
    console.log("UploadImageGo");
    //check whether browser fully supports all File API
    if (window.File && window.FileReader && window.FileList && window.Blob) {
        var imgdata;
        var image = document.getElementById("UploadImage2");
        if (image !== null) {
            imgdata = image.src.split(";base64,")[1];
        }

        UploadImageData = {
            "instanceName": CustomerName,
            "itemName": 'Pieza',//ItemName,
            "itemId": 311,//Data.Id,
            "itemField": image.alt,
            "image": imgdata
        };

        console.log("UploadImageGo", UploadImageData);

        if (imgdata !== null) {
            $.ajax({
                "type": "POST",
                "url": "/Data/AsynchronousMessages.aspx/SaveImage",
                "data": JSON.stringify(UploadImageData, null, 2),
                "contentType": "application/json; charset=utf-8",
                "success": function (msg) {
                    console.log(msg.d.ReturnValue);
                    var res;
                    eval("res=" + msg.d.ReturnValue);
                    document.getElementById("UploadImage1").src = res.url;
                    eval("Data." + UploadImageData.itemField + "= '" + res.url + "';");
                    $("#" + UploadImageData.itemField).attr("src", res.url);
                    $("#dialog-uploadImage").click();
                },
                "error": function (xhr, status, msg) {
                    toastr.danger(msg);
                }
            });
        }
    }
    else {
        ShowSavingError();
        return false;
    }
}