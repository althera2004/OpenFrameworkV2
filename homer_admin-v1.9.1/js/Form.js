var footerButtons = null;

function RenderFooterButtons(formDefinition) {
    var res = "";
    var noGroupButtons = "";
    var GroupButtons = "";
    if (typeof formDefinition.Buttons !== "undefined" && formDefinition.Buttons !== null) {
        formDefinition.Buttons.sort(function (a, b) {
            return a.Group - b.Group;
        });


        var actualGroup = formDefinition.Buttons[0].Group;
        var firstButtonGroup = true;
        for (var x = 0; x < formDefinition.Buttons.length; x++) {

            var button = formDefinition.Buttons[x];

            if (typeof button.Group === "undefined") {
                noGroupButtons += "&nbsp;<button id=\"" + button.Id + "\" class=\"btn btn-primary btn-info\" type=\"button\" onclick=\"" + button.Action + "()\"><i class=\"fa fa-check\"></i>&nbsp" + JavaScriptText(button.Label) + "</button>";
            }
            else {
                if (actualGroup !== formDefinition.Buttons[x].Group && firstButtonGroup === false) {
                    GroupButtons += "<li class=\"divider\"></li>";
                }

                var icon = typeof button.Icon === "undefined" ? "" : button.Icon;
                var paddingLeft = typeof button.Icon === "undefined" ? "20" : "10";
                GroupButtons += "<li><a style=\"padding:3px 20px 3px " + paddingLeft + "px;\" href=\"#\"><span class=\"fa " + icon + "\" style=\"display:inline;\"></span>&nbsp;&nbsp;" + JavaScriptText(button.Label) + "</a></li>";
                actualGroup = formDefinition.Buttons[x].Group;
                firstButtonGroup = false;
            }
        }

        if (GroupButtons !== "") {
            res = "<div class=\"btn-group\"><button data-toggle=\"dropdown\" class=\"btn btn-info dropdown-toggle\">Acciones&nbsp;<span class=\"caret\"></span></button>";
            res += "<ul class=\"dropdown-menu pull-right pull-top\">";
            res += GroupButtons;
            res += "</ul></div>";
        }
    }

    res += noGroupButtons;

	res += "&nbsp;<button id=\"BtnForm-Save\" class=\"btn btn-primary btn-success\" type=\"button\"><i class=\"fa fa-check\"></i>&nbsp" + Dictionary.Common_Save + "</button>";
	res += "&nbsp;<button id=\"BtnForm-Delete\" class=\"btn btn-primary btn-danger\" type=\"button\"><i class=\"fa fa-trash\"></i>&nbsp;" + Dictionary.Common_Delete + "</button>";
	res += "&nbsp;<button id=\"BtnForm-Back\" class=\"btn btn-primary\" type=\"button\"><i class=\"fa fa-chevron-circle-left\"></i>&nbsp;" + Dictionary.Common_Back + "</button>";

	$("#footer-button").append(res);
	
	$("#BtnForm-Save").on("click", SaveActualForm);
	$("#BtnForm-Delete").on("click", DeleteActualForm);
	$("#BtnForm-Back").on("click", NavigationHistoryBackList);
}

function CalculateSpan(numFields) {
    var labelSpan = 2;
    var fieldSpan = 4;

    if (numFields === 4) {
        labelSpan = 1;
        fieldSpan = 2;
    }

    if (numFields === 3) {
        labelSpan = 1;
        fieldSpan = 3;
    }

    if (numFields === 1) {
        labelSpan = 2;
        fieldSpan = 10;
    }

    return { "Label": labelSpan, "Field": fieldSpan };
}

function RenderRow(itemDefinition, rowDefinition) {
    var res = "";

    if (typeof rowDefinition.Label !== "undefined") {
        res += "<h4 style=\"border-bottom:1px solid #ddd;padding-bottom:4px;margin-left:-4px;\">" + rowDefinition.Label + "</h4>";
    }

    res += "<div class=\"row\" style=\"padding:4px;\">";

    if (typeof rowDefinition.Fields !== "undefined") {
        for (var x = 0; x < rowDefinition.Fields.length; x++) {
            var fieldName = rowDefinition.Fields[x].Name;

            var type = rowDefinition.Fields[x].Type;
            var span = CalculateSpan(rowDefinition.Fields.length);
            var spanFinal = span.Label + span.Field;
            if (type === 1) {
                /*var span = CalculateSpan(rowDefinition.Fields.length);
                var spanFinal = span.Label + span.Field;
                res += "<div class=\"col-md-" + spanFinal + "\" style=\"text-alignment:center;\">";
                res += "<button id=\"" + rowDefinition.Fields[x].Name + "\" class=\"btn btn-info col-md-10\" type=\"button\" ><i class=\"fa fa-paste\"></i> " + rowDefinition.Fields[x].Label + "</button > ";
                res += "</div>";*/
                footerButtons.push({ "Id": rowDefinition.Fields[x].Name, "Label": rowDefinition.Fields[x].Label });
            }
            else if (type === 2) {
                res += "<div class=\"col-md-" + spanFinal + "\">&nbsp;</div>";
            }
            else if (type === 4) {
                res += "<div class=\"col-md-" + spanFinal + "\" id=\"" + fieldName + "\">&nbsp;</div>";
            }
            else {
                var field = GetFieldByName(itemDefinition, fieldName);
                if (field === null) {
                    field = rowDefinition.Fields[x];
                }

                if (FieldIsFK(itemDefinition, fieldName) === true) {
                    res += RenderComboField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                }
                else {
                    switch (field.Type.toUpperCase()) {
                        case "TEXT":
                            res += RenderTextField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "DECIMAL":
                            res += RenderTextField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "INT":
                        case "INTEGER":
                        case "LONG":
                            res += RenderIntegerField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "EMAIL":
                            res += RenderEmailField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "URL":
                            res += RenderUrlField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "TEXTAREA":
                            res += RenderTextAreaField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "DATETIME":
                            res += RenderDateField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "BOOLEAN":
                        case "BOOL":
                            res += RenderBooleanField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "IMAGE":
                            res += RenderImageField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        case "DOCUMENTFILE":
                            res += RenderDocumentField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
                            break;
                        default:
                            res += "         <label id=\"" + field.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + field.Label + "</label>" +
                                "         <label id=\"" + field.Name + "\" class=\"col-sm-" + span.Field + " control-label\">" + field.Type.toUpperCase() + "</label>";
                            break;
                    }
                }
            }
        }
    }

    if (typeof rowDefinition.ItemList !== "undefined") {
        console.log("RenderRow", rowDefinition.ItemList )
        res += rowDefinition.ListId;
        res += "<pre>" + JSON.stringify(GetListById(rowDefinition.ItemList, rowDefinition.ListId)) + "</pre>";
        res += "<div class=\"panel-body\" id=\"List" + rowDefinition.ItemList + "_" + rowDefinition.ListId + "\">hola</div>";

    }

    res += "</div>";
    return res;
}

function RenderTab(itemDefinition, tabDefinition) {
    var res = "";
    for (var x = 0; x < tabDefinition.Rows.length; x++) {
        res += RenderRow(itemDefinition, tabDefinition.Rows[x]);
    }

    return res;
}

function RenderTabsSelector(formDefinition) {
    // Render tabs selector
    var res = "";
    if (formDefinition.Tabs.length > 0) {
        res = "<ul class=\"nav nav-tabs\" style=\"background-color:#f7f7f7;\">";
        for (var t = 0; t < formDefinition.Tabs.length; t++) {
            var label = formDefinition.Tabs[t].Label;
            if (typeof label === "undefined" || label === null || label === "") {
                label = Dictionary.Common_MainTab;
            }
            var style = t === 0 ? " style=\"margin-left:12px;margin-top:12px;\"" : " style=\"margin-left:4px;margin-top:12px;\"";
            var cssClass = t === 0 ? "active" : "";
            res += "<li class=\"" + cssClass + "\"" + style + "><a data-toggle=\"tab\" href=\"#tab-" + (t + 1) + "\">" + label + "</a></li>";
        }

        res += "</ul>";
    }

    return res;
}

function RenderForm(itemDefinition, formDefinition) {
    console.clear();
    console.log("RenderForm", formDefinition);

    if (typeof formDefinition === "undefined" || formDefinition === null) {
        formDefinition = CreateDefaultForm(ItemDefinition);
    }

    footerButtons = [];
    var res = "";
    var resTabs = "";
    var tabSelector = RenderTabsSelector(formDefinition);

    for (var x = 0; x < formDefinition.Tabs.length; x++) {
        resTabs += "<li class=\"" + (x === 0 ? " active" : "") + "\">";
        resTabs += "<a data-toggle=\"tab\" href=\"#tab-" + (x + 1) + "\" aria-expanded=\"false\">";
        if (typeof formDefinition.Tabs[x].Icon !== "undefined" && formDefinition.Tabs[x].Icon !== null) {
            resTabs += "<i class=\"fa fa-" + formDefinition.Tabs[x].Icon + "\"></i>&nbsp;";
        }
        resTabs += "<span class=\"font-extra-bold\">" + formDefinition.Tabs[x].Label + "</span>";
        resTabs += "</a></li > ";

        res += "<div id=\"tab-" + (x + 1) + "\" class=\"tab-pane" + (x === 0 ? " active" : "") + "\">";
        res += "<!-- Tab " + (x + 1) + " -->";
        res += "<div class=\"panel-body\">";
        res += RenderTab(itemDefinition, formDefinition.Tabs[x]);
        res += "</div></div>";
    }

    //Renderizar listas internas
    var formLists = [];
    for (var t = 0; t < formDefinition.Tabs.length; t++) {
        for (var r = 0; r < formDefinition.Tabs[t].Rows.length; r++) {
            if (typeof formDefinition.Tabs[t].Rows[r].ItemList !== "undefined") {
                formLists.push(formDefinition.Tabs[t].Rows[r]);
            }
        }
    }

    for (var l = 0; l < formLists.length; l++) {
        $("#List" + formLists[l].ItemList + "_" + formLists[l].ListId).html("wwwwww");
    }

    console.log("formlists", formLists);

    $("#MainForm").prepend(tabSelector);

    $("#LayoutModifiedByFullName").html("El puto amo");
    $("#LayoutModifiedOn").html("Cualquier dia");
    $("#FormTabs").html(resTabs);
	
	RenderFooterButtons(formDefinition);
    return res;
}

function FillForm(data) {
	console.log("FillForm", data);
    var keys = Object.keys(data);
    for (var x = 0; x < keys.length; x++) {
        console.log(keys[x], data[keys[x]]);
		var field = GetFieldByName(ItemDefinition, keys[x]);
		if(field !== null){			
			switch(field.Type.toUpperCase()){
				case "IMAGE":
				console.log(field.name, data[keys[x]]);
				
				if(data[keys[x]] !== "" && data[keys[x]] !== null)
				{
					$("#" + field.Name).attr("src", "/Instances/" + CustomerName + "/data/img/" + data[keys[x]] );
					if(typeof field.Zoom !== "undefined" && field.Zoom === true) {
						$("#" + field.Name + "Zoomer").data( "medium-url", "/Instances/" + CustomerName + "/data/img/" + data[keys[x]] );
						$("#" + field.Name + "Zoomer").data( "large-url", "/Instances/" + CustomerName + "/data/img/" + data[keys[x]] );
					}
					$("#" + field.Name + "BtnAdd").hide();
				}
				else
				{
					$("#" + field.Name).attr("src", "/img/NoImage.png" );
					$("#" + field.Name + "BtnChange").hide();
					$("#" + field.Name + "BtnDelete").hide();
					$("#" + field.Name + "BtnView").hide();
				}
					
                break;
                case "BOOL":
                case "BOOLEAN":
                    if (data[keys[x]] === true) {
                        $("#" + keys[x]).attr("checked", true);
                    }
                break;
				default:
					$("#" + keys[x]).val(data[keys[x]]);
					break;
			}
		}
    }
}

function ShowFormErrors(messages) {
    var list = "<strong>Revise los siguientes errores:</strong><ul>";
    for (var x = 0; x < messages.length; x++) {
        list += "<li>" + messages[x] + "</li>";
    }

    list += "</ul>";
    ShowAlertPanel(4, list);
}

function ShowAlertPanel(type, message, duration) {
    $("#AlertPanel").remove();
    var icon = "check";
    var color = "success";

    switch (type) {
        case 1:
            icon = "check";
            color = "success";
            break;
        case 2:
            icon = "info";
            color = "info";
            break;
        case 3:
            icon = "warning";
            color = "warning";
            break;
        case 4:
            icon = "close";
            color = "danger";
            break;
    }


    var res = "<div class=\"alert alert-" + color + "\" id=\"AlertPanel\" style=\"margin:0;\">";
    res += "<i class=\"fa fa-" + icon + "\"></i> " + message;
    res += "</div>";
    $("#WorkArea").prepend(res);

    if (typeof duration !== "undefined" && duration !== null) {
        setTimeout(function () { $("#AlertPanel").fadeOut("slow") }, duration);
    }
}

function HideAlertPanel() {
    $("#AlertPanel").remove();
}

function DeleteImage(fieldName) {
	swal({
		"title": "Are you sure?",
		"text": "Your will not be able to recover this imaginary file!",
		"type": "warning",
		"showCancelButton": true,
		"confirmButtonColor": "#DD6B55",
		"confirmButtonText": "Yes, delete it!",
		"cancelButtonText": "No, cancel plx!",
		"closeOnConfirm": true,
		"closeOnCancel": false },
	function (isConfirm) {
		if (isConfirm) {
			DeleteImageConfirmed(fieldName);
		} else {
			swal("Cancelled", "Your imaginary file is safe :)", "error");
		}
	});
}

function DeleteImageConfirmed(fieldName){
	console.clear();
	var data = {
        "itemName": ItemDefinition.ItemName,
        "itemId": Data.Id,
		"instanceName": CustomerName,
		"fieldName": fieldName,
		"applicationUserId": actualUser.Id
	};
	$.ajax({
        "type": "POST",
        "url": "/Data/ItemDataBase.aspx/DeleteImage",
        "data": JSON.stringify(data),
        "contentType": "application/json; charset=utf-8",
        "dataType": "json",
        "async": false,
        "success": function (msg) { 
			$("#" + fieldName).attr("src", "/img/NoImage.png" );
			$("#" + fieldName + "BtnChange").hide();
			$("#" + fieldName + "BtnDelete").hide();
			$("#" + fieldName + "BtnView").hide();
            $("#" + fieldName + "BtnAdd").show();
            $("#" + fieldName + "Zoomer").data("medium-url", "/img/NoImage.png" );
            $("#" + fieldName + "Zoomer").data("large-url", "/img/NoImage.png" );
        },
        "error": function (msg, text) {
            alert(text);
        }
    });
}

function ViewImage(fieldName) {
	var image = $("#" + fieldName).attr("src");
	window.open(image);
}

function GetFormData() {
	var itemId = -1;
	if(typeof Data !== "undefined" && Data !== null){
		itemId = Data.Id;
	}
	
	var data = { "Id" : itemId };
	for(var x=0;x<ItemDefinition.Fields.length;x++){		
		var field = ItemDefinition.Fields[x];
		
		if(field.Name === "Id") { continue; }
		
		var fieldValue = null;
		
		if($("#" + field.Name).length > 0){
			
			if(FieldIsFK(ItemDefinition, field.Name))
			{
				fieldValue = $("#" + field.Name).val() * 1;
				if(fieldValue === 0){
					fieldValue = null;
				}
			}
			else if(field.Type === "Image") {
				fieldValue = $("#" + field.Name).attr("src");
				if(fieldValue === "/img/NoImage.png") {
					fieldValue = null;
				}
				else{
					var parts = fieldValue.split("/");
					fieldValue = parts[parts.length - 1].split("?")[0];
				}
			}
			else if(field.Type === "long" || field.Type === "int" || field.Type === "decimal"){
				if($("#" + field.Name).val() === ""){
					fieldValue = null;
				}
				else{
					fieldValue = $("#" + field.Name).val() * 1;
				}
			}
			else if(field.Type === "boolean"){
				var n = $("#" + field.Name + ":checked" ).length;
				fieldValue = n === 1;
            }
            else if (field.Type === "datetime") {
                if ($("#" + field.Name).val() === "") {
                    fieldValue = null;
                }
                else {
                    fieldValue = $("#" + field.Name).val();
                }

                console.log("GetFormData Datetime", fieldValue);
            }
			else {
				fieldValue = $("#" + field.Name).val();
			}
		}
		
		//console.log(field.Name, fieldValue);
		data[field.Name] = fieldValue;
	}
	
	data["Active"] = true;
    console.log("Data", data);
    return data;
}

function CreateDefaultForm(definition) {

    var res = {
        "Id": "Custom",
        "FormType": "Custom",
        "Tabs":
        [
            {
                "Label": "Datos principales",
                "Rows": []
            }
        ]
    }

    var pair = false;
    for (var f = 0; f <definition.Fields.length; f++){
        var row = {
            "Fields": [{ "Name": definition.Fields[f].Name }]
        }
        res.Tabs[0].Rows.push(row);
    }

    /*{
            "Id": "Custom",
            "FormType": "Custom",
            "Tabs": [
                {
                    "Label": "Datos",
                    "Rows": [
                        {
                            "Fields": [
                                { "Name": "Nombre" },
                                { "Name": "LugarId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "ProveedorId" },
                                { "Name": "MonitorId" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "PrecioUnico" },
                                { "Name": "TipoPago" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "Precio2" },
                                { "Name": "Precio3" }
                            ]
                        },
                        {
                            "Fields": [
                                { "Name": "QuorumMin" },
                                { "Name": "QuorumMax" }
                            ]
                        }
                    ]
                },
                {
                    "Label": "Inscripciones",
                    "Rows": [
                        {
                            "ItemList": "Inscripcion",
                            "ListId": "Custom",
                            "FilterFields": [ { "Field": "Actividad" } ]
                        }
                    ]
                }
            ]
        }*/

    return res;
}