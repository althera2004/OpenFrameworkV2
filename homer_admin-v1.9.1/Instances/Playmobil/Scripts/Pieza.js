function PIEZA_CustomActions() {
    console.log("PIEZA_CustomActions");
}

function PIEZA_AfterLoad() {
    console.log("PIEZA_AfterLoad");
    PIEZA_LayoutAddToCollection();
}

function PIEZA_LayoutAddToCollection(){
	console.log();
    ReloadFK("MiPieza");
    $("#AddToMiPieza").remove();
    $("#RemoveFromMiPieza").remove();
	if (typeof Data.Id !== "undefined") {
        var miPieza = GetByFieldFromList(FK.MiPieza, "Description", Data.Referencia + "-" + Data.Nombre);
        if (miPieza === null || miPieza.Active === false) {
            var addButton = "<button type=\"button\" id=\"AddToMiPieza\" class=\"btn btn-primary btn-success\" onclick=\"PIEZA_AddToCollection();\"><i class=\"fa fa-plus\"></i>&nbsp;Añadir a mi colección</button>";
			$("#footer-button").prepend(addButton);
		}
		else {
            var removeButton = "<button type=\"button\" id=\"RemoveFromMiPieza\" class=\"btn btn-primary btn-danger\" onclick=\"PIEZA_RemoveFromCollection();\"><i class=\"fa fa-trash\"></i>&nbsp;Eliminar de mi colección</button>";
			$("#footer-button").prepend(removeButton);
		}
    }
}

function PIEZA_AddToCollection(){
	var data = {
        "piezaId": Data.Id,
        "applicationUserId": actualUser.Id
    };

    try { $("#working").click(); } catch (e) { console.log(e); }

    $.ajax({
        "type": "POST",
        "url": "/Instances/Playmobil/Data/ItemDataBase.aspx/AddToCollection",
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "data": JSON.stringify(data, null, 2),
        "success": function (msg) {
            $(".botTempo").click();
            if (msg.d.Success === false) {
                PopupErrorBig(msg.d.MessageError)
            } else {
                UIInfo("Operación realizada", "Se ha añadido la pieza a tu colección");
				PIEZA_LayoutAddToCollection();
            }
        },
        error: function (msg) {
            $(".botTempo").click();
            Error("error", "toma casques", msg.responseText);
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