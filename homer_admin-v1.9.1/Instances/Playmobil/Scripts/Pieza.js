function PIEZA_CustomActions() {
    UPLOADDOCUMENT_Context.Name = true;
    UPLOADDOCUMENT_Context.Normalize = true;
    UPLOADDOCUMENT_Context.Serialize = false;
    UPLOADDOCUMENT_Context.Replace = true;
    $("#PiezaTxtImagenLabel").parent().remove();
    $("#PiezaTxtImagen1Label").parent().remove();
    $("#PiezaTxtImagen2Label").parent().remove();
	$("#PiezaTxtImagen").hide();
	$("#PiezaTxtImagen1").hide();
	$("#PiezaTxtImagen2").hide();
	$("#PiezaTxtImagen").parent().removeClass("col-3");
	$("#PiezaTxtImagen1").parent().removeClass("col-3");
	$("#PiezaTxtImagen2").parent().removeClass("col-3");
	$("#PiezaTxtImagen").parent().addClass("col-sm-4");
	$("#PiezaTxtImagen1").parent().addClass("col-sm-4");
	$("#PiezaTxtImagen2").parent().addClass("col-sm-4");
	$("#PiezaTxtImagen").next().css("height","250px");
	$("#PiezaTxtImagen").next().css("max-height","250px");
	$("#PiezaTxtImagen1").next().css("height","250px");
	$("#PiezaTxtImagen1").next().css("max-height","250px");
	$("#PiezaTxtImagen2").next().css("height","250px");
    $("#PiezaTxtImagen2").next().css("max-height", "250px");

    PIEZA_LayoutAddToCollection();
}

function PIEZA_LayoutAddToCollection(){
	console.log();
	ReloadFK("MiPieza");
	$("#btn-Add").remove();
	if (typeof Data.Id !== "undefined") {
        var miPieza = GetByFieldFromList(FK.MiPieza, "Referencia", Data.Referencia);

		if (miPieza === null) {
			var addButton = "<button type=\"button\" id=\"btn-Add\" class=\"btn btn-primary btn-success\" onclick=\"PIEZA_AddToCollection();\"><i class=\"fa fa-plus\"></i>&nbsp;Añadir a mi colección</button>";
			$("#footer-button").prepend(addButton);
		}
		else {
			var removeButton = "<button type=\"button\" id=\"btn-Remove\" class=\"btn btn-primary btn-warning\" onclick=\"PIEZA_AddToCollection();\"><i class=\"fa fa-minus\"></i>&nbsp;Eliminar de mi colección</button>";
			$("#footer-button").prepend(removeButton);
		}
    }
}

function PIEZA_AddToCollection(){
	var data = {
        "piezaId": Data.Id,
        "applicationUserId": User.Id
    };

    try { $("#working").click(); } catch (e) { console.log(e); }

    $.ajax({
        "type": "POST",
        "url": "/CustomersFramework/Playmobil/Data/ItemDataBase.aspx/AddToCollection",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            $(".botTempo").click();
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError)
            } else {
                PopupActionResult("Se ha añadido la pieza a tu colección");
				PIEZA_LayoutAddToCollection();
            }
        },
        error: function (msg) {
            $(".botTempo").click();
            PopupErrorSmall(msg.responseText);
        }
    });

    return false;
}

function PIEZA_GridRender(nRow, aData, iDisplayIndex, iDisplayIndexFull) {
    var res = "<div class=\"thumb_item\">";
    res += "<div class=\"thumbContent\">";
    res += "<strong>" + aData.N + "</strong><br />";
    res += "<div style=\"float:left;width:50%;min-height:150px;max-height:150px;\">";
    res += "<img src=\"" + aData.I + "\">";
    res += "</div>";
    res += " Ref: <strong>" + aData.R + "</strong><br />";
    res += " " + aData.T + "<br />";
    res += " " + aData.F + "<br />";
    res += "</div><div>";
    return res;
}