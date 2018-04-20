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
    res += "             <select id=\"" + fieldDefinition.Name + "\" name=\"" + fieldDefinition.Name + "\" type=\"text\" placeholder=\"" + fieldLabel + "\" class=\"form-control form-select\"" + readOnly + ">";
    res += "                 <option value=\"0\">" + Dictionary.Common_Select + "</option>";

    var itemReferedName = fieldDefinition.Name.substr(0, fieldDefinition.Name.length - 2);
    var referedItemsList = FK[itemReferedName];
    referedItemsList.sort((a, b) => a.Description < b.Description);
    for (var x = 0; x < referedItemsList.length; x++) {
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

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
        "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
        "             <div class=\"input-group m-b\">" +
        "                 <span class=\"input-group-addon\"><i class=\"fa fa-external-link\" onclick=\"ViewImagePage('" + fieldDefinition.Name + "');\"></i></span>" +
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

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel + "</label>" +
        "         <div id=\"" + fieldDefinition.Name + "Div\" class=\"col-md-" + span.Field + "\">" +
        "               <div class=\"checkbox\">" +
        "                    <label class=\"\" style=\"padding-left:0!important;\">" +
        "                           <input id=\"" + fieldDefinition.Name + "\" type=\"checkbox\" class=\"i-checks\" style=\"position: absolute; opacity: 0;\">" +
        "                   </label>" +
        "               </div>" +
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

    if (typeof fieldDefinition.Zoom !== "undefined" && fieldDefinition.Zoom === true) {
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

    return "         <label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\">" + fieldLabel + requiredLabel +
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

function RenderDocumentField(fieldDefinition, formFieldDefinition, numFields) {
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

    return "<label id=\"" + fieldDefinition.Name + "Label\" class=\"col-sm-" + span.Label + " control-label\" onfocus=\"$('" + fieldDefinition.Name + "Butttons').show();\" onblur=\"$('" + fieldDefinition.Name + "Butttons').hide();\">" + fieldLabel + requiredLabel + "</label>" +
        "<div class=\"input-group\">" +
        "    <input id=\"" + fieldDefinition.Name + "\" type=\"text\" class=\"form-control\">" +
        "        <span class=\"input-group-btn\">" +
        "            <button id=\"Btn" + fieldDefinition.Name + "New\"    type=\"button\" class=\"btn btn-primary\" style=\"height:29px;padding-top:4px;\" onclick=\"OpenDocumentUploadPopup($('#" + fieldDefinition.Name + "').attr('src'), '" + fieldLabel + "', '" + fieldDefinition.Name + "');\"><i class=\"fa fa-upload\"></i>" + "</button>" +
        "            <button id=\"Btn" + fieldDefinition.Name + "View\"   type=\"button\" class=\"btn btn-primary\" style=\"height:29px;padding-top:4px;\"><i class=\"fa fa-download\"></i>" + "</button>" +
        "            <button id=\"Btn" + fieldDefinition.Name + "Delete\" type=\"button\" class=\"btn btn-danger\"  style=\"height:29px;padding-top:4px;\"><i class=\"fa fa-close\"></i>" + "</button>" +
        "        </span>" +
        "</div>";
}