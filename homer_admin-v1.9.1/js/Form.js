var footerButtons = null;

function RenderFooterButtons(tab) {
    console.log("RenderFooterButtons",tab);

    if (tab.Buttons !== null) {

        tab.Buttons.sort(function (a, b) {
            return a.Group - b.Group;
        });

        var res = "<div class=\"btn-group\"><button data-toggle=\"dropdown\" class=\"btn btn-info dropdown-toggle\">Acciones2 <span class=\"caret\"></span></button>";
        res += "<ul class=\"dropdown-menu pull-right pull-top\">";
        var actualGroup = tab.Buttons[0].Group;
        for (var x = 0; x < tab.Buttons.length; x++) {
            if (actualGroup !== tab.Buttons[x].Group) {
                res += "<li class=\"divider\"></li>";
            }

            var button = tab.Buttons[x];
            var icon = typeof button.Icon === "undefined" ? "" : button.Icon;
            var paddingLeft = typeof button.Icon === "undefined" ? "20" : "10";
            res += "<li><a style=\"padding:3px 20px 3px " + paddingLeft + "px;\" href=\"#\"><span class=\"fa " + icon + "\" style=\"display:inline;\"></span>&nbsp;&nbsp;" + button.Label + "</a></li>";
            actualGroup = tab.Buttons[x].Group;
        }

        res += "</ul></div>";

        res += "&nbsp;<button class=\"btn btn-primary btn-success\" type=\"button\"><i class=\"fa fa-check\"></i>&nbsp;Guardar</button>";
        res += "&nbsp;<button class=\"btn btn-primary btn-danger\" type=\"button\"><i class=\"fa fa-trash\"></i>&nbsp;Eliminar</button>";
        res += "&nbsp;<button class=\"btn btn-primary\" type=\"button\"><i class=\"fa fa-chevron-circle-left\"></i>&nbsp;Volver</button>";

        $("#footer-button").append(res);
    }
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

function RenderComboField(fieldDefinition, formFieldDefinition, numFields) {

    var fieldLabel = fieldDefinition.Name;
    if (typeof fieldDefinition.Label !== "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof formFieldDefinition.Label !== "undefined") {
        fieldLabel = formFieldDefinition.Label;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    var res = "<label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>";
    res += "         <div class=\"col-md-" + span.Field + "\">";
    res += "             <select id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control\"" + readOnly + ">";
	res += "                 <option value=\"0\">" + Dictionary.Common_Select + "</option>";
	
	var itemReferedName = fieldDefinition.Name.substr(0,fieldDefinition.Name.length-2);
	var referedItemsList = FK[itemReferedName];
	referedItemsList.sort((a, b) => a.Description < b.Description);
	for(var x=0;x<referedItemsList.length;x++){
		res += "   <option value=\"" + referedItemsList[x].Id + "\">" + referedItemsList[x].Description + "</option>";
    }
    res += "             </select>";
    res += "         </div>";
    return res;
}

function RenderTextField(fieldDefinition, formFieldDefinition, numFields) {

    var fieldLabel = fieldDefinition.Name;
    if (typeof fieldDefinition.Label !== "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof formFieldDefinition.Label !== "undefined") {
        fieldLabel = formFieldDefinition.Label;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    return "<label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
           "         <div class=\"col-md-" + span.Field + "\">" +
           "             <input id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control\"" + readOnly + " />" +
           "         </div>";
}

function RenderDateField(fieldDefinition, formFieldDefinition, numFields) {
    /*<div class="input-group date" id="datetimepicker1">
                                <span class="input-group-addon">
                                    <span class="fa fa-calendar"></span>
                                </span>
                            <input type="text" class="form-control">
                        </div>*/

    var fieldLabel = fieldDefinition.Name;
    if (typeof fieldDefinition.Label !== "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof formFieldDefinition.Label !== "undefined") {
        fieldLabel = formFieldDefinition.Label;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);
    return "<label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
        "         <div class=\"col-md-" + span.Field + "\">" +
        "             <div class=\"input-group date\" id=\"datetimepicker1\">" +
        "                <span class=\"input-group-addon\" >" +
        "                   <span class=\"fa fa-calendar\"></span>" +
        "                                    </span >" +
        "             <input id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"datepicker form-control\"" + readOnly + " />" +
        "                            </div>" +
        "         </div>";
}

function RenderTextAreaField(fieldDefinition, formFieldDefinition, numFields) {
    //{ "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },

    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";
    var rows = formFieldDefinition.Rows > 0 ? " rows=\"" + formFieldDefinition.Rows + "\"" : "";

    var span = CalculateSpan(numFields);

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
           "             <textarea id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control\"" + readOnly + rows + "></textarea>" +
           "         </div>";
}

function RenderIntegerField(fieldDefinition, formFieldDefinition, numFields) {
    //{ "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },

    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + "</label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
           "             <input id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control integerFormated\"" + readOnly + " />" +
           "         </div>";
}

function RenderEmailField(fieldDefinition, formFieldDefinition, numFields) {
    //{ "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },

    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
           "             <div class=\"input-group m-b\">" +
           "                 <span class=\"input-group-addon\">@</span>" +
           "                 <input id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control\"" + readOnly + " />" +
           "             </div>" +
           "         </div>";
}

function RenderUrlField(fieldDefinition, formFieldDefinition, numFields) {
    //{ "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },

    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    return "         <label id=\"" +fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
           "             <div class=\"input-group m-b\">" +
           "                 <span class=\"input-group-addon\"><i class=\"fa fa-external-link\" onclick=\"GoUrl('" + fieldDefinition.Name + "');\"></i></span>" +
           "                 <input id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control\"" + readOnly + " />" +
           "             </div>" +
           "         </div>";
}

function RenderBooleanField(fieldDefinition, formFieldDefinition, numFields) {
    //{ "Name": "CNAEId", "Label": "CNAE", "Type": "long", "ColumnDataType": "Text" },

    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);

    return "         <label id=\"" +fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
           "             <input type=\"checkbox\" />" +
           "         </div>";
}

function RenderImageField(fieldDefinition, formFieldDefinition, numFields) {
    var fieldLabel = formFieldDefinition.Label;
    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Label;
    }

    if (typeof fieldLabel === "undefined") {
        fieldLabel = fieldDefinition.Name;
    }

    var requiredLabel = fieldDefinition.Required === true ? "<span style=\"color:#f00;\">*</span>" : "";
    var readOnly = formFieldDefinition.ReadOnly === true ? " readonly=\"readonly\"" : "";

    var span = CalculateSpan(numFields);
	
	if(typeof fieldDefinition.Zoom !== "undefined" && fieldDefinition.Zoom === true) {
        return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\" onfocus=\"$('" + fieldDefinition.Name + "Butttons').show();\" onblur=\"$('" + fieldDefinition.Name + "Butttons').hide();\">" + fieldLabel + requiredLabel +
               "             <div style=\"width:32px;float:right;margin-top:20px;\" id=\"" + fieldDefinition.Name + "Butttons\" style=\"display:none;\">" +
               "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnAdd\"    class=\"btn btn-primary btn-info\"    title=\"Añadir imagen\"   onclick=\"OpenImageUploadPopup($('#" + fieldDefinition.Name + "').attr('src'), '" + fieldLabel + "', '" + fieldDefinition.Name + "');\">  <i class=\"fa fa-plus\"></i></button>" + 
               "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnChange\" class=\"btn btn-primary btn-success\" title=\"Cambiar imagen\"  onclick=\"OpenImageUploadPopup($('#" + fieldDefinition.Name + "').attr('src'), '" + fieldLabel + "', '" + fieldDefinition.Name + "');\"> <i class=\"fa fa-refresh\"></i></button>" +
               "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnDelete\" class=\"btn btn-primary btn-danger\"  title=\"Eliminar imagen\" onclick=\"DeleteImage('" + fieldDefinition.Name + "');\"><i class=\"fa fa-remove\"></i></button>" + 
               "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnView\"   class=\"btn btn-primary\"             title=\"Ver imagen\"      onclick=\"ViewImage('" + fieldDefinition.Name + "');\"><i class=\"fa fa-external-link\"></i></button>" +
               "             </div>" +
               "         </label>" +
               "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\" style=\"text-align:center;max-height:252px;height:252px;border:1px solid #333;background-image:url(/img/ImageUploadBackground.png)\" onfocus=\"$('" + fieldDefinition.Name + "Butttons').show();\" onblur=\"$('" + fieldDefinition.Name + "Butttons').hide();\">" +
               "             <a href=\"#\" id=\"" + fieldDefinition.Name + "Zoomer\"  class=\"demo-trigger\" style=\"max-width:100%;max-height:250px;\">" +
               "                 <img id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" src=\"\" style=\"max-width:100%;max-height:250px;\" />" +
               "             </a>" + 
               "         </div>";
	}
	
	return "         <label id=\"" +fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel +
           "             <div style=\"width:32px;float:right;margin-top:20px;\" id=\"" + fieldDefinition.Name + "Butttons\" style=\"display:none;\">" + 
           "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnAdd\"    class=\"btn btn-primary btn-info\"    title=\"Añadir imagen\"   onclick=\"OpenImageUploadPopup($('#" + fieldDefinition.Name + "').attr('src'), '" + fieldLabel + "', '" + fieldDefinition.Name + "');\">  <i class=\"fa fa-plus\"></i></button>" + 
           "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnChange\" class=\"btn btn-primary btn-success\" title=\"Cambiar imagen\"  onclick=\"OpenImageUploadPopup($('#" + fieldDefinition.Name + "').attr('src'), '" + fieldLabel + "', '" + fieldDefinition.Name + "');\"> <i class=\"fa fa-refresh\"></i></button>" +
           "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnDelete\" class=\"btn btn-primary btn-danger\"  title=\"Eliminar imagen\" onclick=\"DeleteImage('" + fieldDefinition.Name + "');\"><i class=\"fa fa-remove\"></i></button>" + 
           "                 <button type=\"button\" style=\"display:block;padding:0;width:30px;margin-top:4px;\" id=\"" + fieldDefinition.Name + "BtnView\"   class=\"btn btn-primary\"             title=\"Ver imagen\"      onclick=\"ViewImage('" + fieldDefinition.Name + "');\"><i class=\"fa fa-external-link\"></i></button>" +
           "             </div>" +
           "         </label>" +
           "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\" style=\"text-align:center;max-height:252px;height:252px;border:1px solid #333;background-image:url(/img/ImageUploadBackground.png)\">" +
           "                 <img id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" src=\"\" style=\"max-width:100%;max-height:250px;\" />" +
           "         </div>";
}

function RenderRow(itemDefinition, rowDefinition) {
    var res = "";

    if (typeof rowDefinition.Label !== "undefined") {
        res += "<h4 style=\"border-bottom:1px solid #ddd;padding-bottom:4px;margin-left:-4px;\">" + rowDefinition.Label + "</h4>";
    }

    res += "<div class=\"row\" style=\"padding:4px;\">";

    for (var x = 0; x < rowDefinition.Fields.length; x++) {
        var fieldName = rowDefinition.Fields[x].Name;

        var type = rowDefinition.Fields[x].Type;
        if (type === 1) {
            /*var span = CalculateSpan(rowDefinition.Fields.length);
            var spanFinal = span.Label + span.Field;
            res += "<div class=\"col-md-" + spanFinal + "\" style=\"text-alignment:center;\">";
            res += "<button id=\"" + rowDefinition.Fields[x].Name + "\" class=\"btn btn-info col-md-10\" type=\"button\" ><i class=\"fa fa-paste\"></i> " + rowDefinition.Fields[x].Label + "</button > ";
            res += "</div>";*/
            footerButtons.push({ "Id": rowDefinition.Fields[x].Name, "Label": rowDefinition.Fields[x].Label });
        }
        else if (type === 2) {
            var span = CalculateSpan(rowDefinition.Fields.length);
            var spanFinal = span.Label + span.Field;
            res += "<div class=\"col-md-" + spanFinal + "\">&nbsp;</div>";
        }
        else if (type === 4) {
            var span = CalculateSpan(rowDefinition.Fields.length);
            var spanFinal = span.Label + span.Field;
            res += "<div class=\"col-md-" + spanFinal + "\" id=\"" + fieldName + "\">&nbsp;</div>";
        }
        else {
            var field = GetFieldByName(itemDefinition, fieldName);
            if (field === null) {
                field = rowDefinition.Fields[x];
            }
			
			if(FieldIsFK(itemDefinition, fieldName) === true) {
				res += RenderComboField(field, rowDefinition.Fields[x], rowDefinition.Fields.length);
			}
			else
			{
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
					default:
						var span = CalculateSpan(rowDefinition.Fields.length);
						res += "         <label id=\"" + field.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + field.Label + "</label>" +
                               "         <label id=\"" + field.Name + "\" class=\"col-sm-" + span.Field + " control-label\">" + field.Type.toUpperCase() + "</label>";
						break;
				}
            }
        }
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

function RenderForm(itemDefinition, formDefinition) {
    footerButtons = [];
    var res = "";
    var resTabs = "";

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

    $("#LayoutModifiedByFullName").html("El puto amo");
    $("#LayoutModifiedOn").html("Cualquier dia");
    $("#FormTabs").html(resTabs);
    return res;
}

function FillForm(data) {
    var keys = Object.keys(data);
    for (var x = 0; x < keys.length; x++) {
        console.log(keys[x], data[keys[x]]);
		var field = GetFieldByName(ItemDefinition, keys[x]);
		if(field !== null){			
			switch(field.Type.toUpperCase()){
				case "IMAGE":
				console.log(field.name, data[keys[x]]);
				
				if(data[keys[x]] !== "")
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
				default:
					$("#" + keys[x]).val(data[keys[x]]);
					break;
			}
		}
    }
}

function GoUrl(controlId) {
    var url = $("#" + controlId).val();
    window.open(url);
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
		title: "Are you sure?",
		text: "Your will not be able to recover this imaginary file!",
		type: "warning",
		showCancelButton: true,
		confirmButtonColor: "#DD6B55",
		confirmButtonText: "Yes, delete it!",
		cancelButtonText: "No, cancel plx!",
		closeOnConfirm: true,
		closeOnCancel: false },
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
	var data = { "Id" : Data.Id };
	for(var x=0;x<ItemDefinition.Fields.length;x++){		
		var field = ItemDefinition.Fields[x];
		
		if(field.Name === "Id") { continue; }
		
		var fieldValue = null;
		
		if($("#" + field.Name).length > 0){
			if(field.Type === "Image") {
				fieldValue = $("#" + field.Name).attr("src");
				if(fieldValue === "/img/NoImage.png") {
					fieldValue = null;
				}
				else{
					var parts = fieldValue.split("/");
					fieldValue = parts[parts.length - 1].split("?")[0];
				}
			}
			else {
				fieldValue = $("#" + field.Name).val();
			}
		}
		
		//console.log(field.Name, fieldValue);
		data[field.Name] = fieldValue;
	}
	
    console.log("Data", data);
    return data;
}