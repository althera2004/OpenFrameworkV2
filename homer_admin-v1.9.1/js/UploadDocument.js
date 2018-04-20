var DocumentUploadContext = {
    "fieldName": null,
    "field": null,
    "file": null,
    "data": null,
    "normalize": true,
    "serialize": false,
    "replace": false,
    "name": true
};

function DocumentUploadPopupReset() {
    DocumentUploadContext.file = null;
    $("#MyModalDocumentNuevoName").html("<i style=\"cursor:pointer;color: #777;\" onclick=\"$('#" + DocumentUploadContext.fieldName + "').click();\">" + Dictionary.Common_Select + "</i>");
    $("#MyModalDocumentNuevoType").html("&nbsp;");
    $("#MyModalDocumentNuevoSize").html("&nbsp;");
    $("#MyModalDocumentErrorMessage").html("&nbsp;");
    $("#MyModalDocumentErrorMessage").hide();
    $("#MyModalDocumentErrorMessageDiv").hide();
}

function OpenDocumentUploadPopup(imageFile, name, fieldName) {
    DocumentUploadContext.field = GetFieldByName(ItemDefinition, fieldName);
    DocumentUploadPopupReset();
    var actualFileName = $("#" + fieldName).val();
    DocumentUploadContext.fieldName = fieldName;
    console.log("actualFileName", actualFileName);
    if (actualFileName === "") {
        $("#MyModalDocumentActualName").html("<i style=\"color:#777;\">" + Dictionary.Common_None + "</i>");
        $("#MyModalDocumentAction").html(Dictionary.Common_Add);
    }
    else {
        $("#MyModalDocumentActualName").actualFileName;
        $("#MyModalDocumentAction").html(Dictionary.Common_Replace);
    }

    $("#MyModalDocumentErrorMessage").hide();
    $("#MyModalDocumentErrorMessage").html("");
    if (typeof Data.Id === "undefined" || Data.Id < 0) {
        toastr.error("Primero debe guardar el item");
        return false;
    }    

    $("#UploadDocumentFieldName").html(DocumentUploadContext.field.Label);
    $("#BtnModalDocument").click();
    $("#UploadDocumentFile").removeAttr("change");
    $("#UploadDocumentFile").on("change", function () { readURLDocument(this); });
}

function readURLDocument(input) {
    $("#MyModalDocumentErrorMessageDiv").hide();
    $("#MyModalDocumentErrorMessage").html("");
    if (input.files && input.files[0]) {
        console.log("file", input.files[0]);
        $("#MyModalDocumentNuevoName").html(input.files[0].name);
        $("#MyModalDocumentNuevoType").html(input.files[0].type);
        $("#MyModalDocumentNuevoSize").html(humanFileSize(input.files[0].size, true));
        DocumentUploadContext.file = input.files[0];
        if (DocumentUploadContext.field !== null) {
            if (typeof DocumentUploadContext.field.MIME !== "undefined" && DocumentUploadContext.field.MIME !== null && DocumentUploadContext.field.MIME.length > 0) {
                var accepted = false;
                var mimes = DocumentUploadContext.field.MIME;
                for (var x = 0; x < mimes.length; x++) {
                    if (mimes[x] === input.files[0].type) {
                        accepted = true;
                        break;
                    }
                }

                if (accepted === false) {
                    DocumentUploadPopupReset();
                    var res = "Tipo de fichero no permitido.<br />Se admiten los ficheros:<ul>";
                    for (var m = 0; m < mimes.length; m++) {
                        res += "<li>" + mimes[m] + "</li>";
                    }

                    res += "</ul>";
                    $("#MyModalDocumentErrorMessage").html(res);
                    $("#MyModalDocumentErrorMessage").show();
                    $("#MyModalDocumentErrorMessageDiv").show();
                }
            }
        }
    }
}

function UploadDocumentValidation() {
    if (DocumentUploadContext.file === null) {
        $("#MyModalDocumentErrorMessageDiv").show();
        $("#MyModalDocumentErrorMessage").show();
        $("#MyModalDocumentErrorMessage").html(Dictionary.Common_Error_NoDocumentSelected);
        return false;
    }

    UploadDocumentGo();
}

function UploadDocumentGo() {
    DocumentUploadContext.data = null;
    console.log("UploadDocumentGo");
    var formData = new FormData();
    formData.append("instanceName", CustomerName);
    formData.append("itemName", ItemDefinition.ItemName);
    formData.append("fieldName", DocumentUploadContext.fieldName);
    formData.append("itemId", Data.Id);
    formData.append("data", DocumentUploadContext.file);
    formData.append("name", DocumentUploadContext.name === true ? "1" : "0");
    formData.append("normalize", DocumentUploadContext.normalize === true ? "1" : "0");
    formData.append("serialize", DocumentUploadContext.serialize === true ? "1" : "0");
    formData.append("replace", DocumentUploadContext.replace === true ? "1" : "0");

    console.log(formData);

    var xhr = new XMLHttpRequest();
    xhr.open("POST", "/Data/DocumentUpload.aspx", true);
    xhr.onreadystatechange = function () {
        if (xhr.readyState === 4) {
            if (xhr.status === 200) {
                console.log("Response", xhr);
                var res;
                eval("res = " + xhr.response + ";");
                console.log("Response2", res);
                $("#" + DocumentUploadContext.fieldName).val(res.ReturnValue);
                Data[DocumentUploadContext.fieldName] = res.ReturnValue;
                $("#MyModalDocumentBtnClose").click();
            }
            else {
                alert(xhr.responseText);
            }
        }
    };

    xhr.send(formData);
    return false;
}