function RenderSpanToolTip(data, tooltip) {
    if (data === null) {
        return "";
    }
    console.log("tooltip", tooltip);
    return "<span title=\"" + tooltip + "\">" + data + "</span>";
}

function ToWebPage(data) {
    if (data === null) { return ""; }
    if (data === "") { return ""; }
    return "<i class=\"fa fa-external-link\"></i>&nbsp;<a href=\"" + data.Description + "\" >" + data.Description + "</a>";
}

function ToWebPageBlank(data) {
    if (data === null) { return ""; }
    if (data === "") { return ""; }
    var res = "<i class=\"fa fa-external-link\"></i>&nbsp;<a href=\"" + data + "\" target=\"_blank\">" + data + "</a>";
    return res;
}

function ToMail(mail, label) {
    if (mail === null || mail === "") { return ""; }
    if (typeof label === "undefined" || label === null || label === "") { label = mail; }
    return "<a href=\"mailto:" + mail + "\">" + mail + "</a>";
}

function buttons(data, descriptionIndex, duplicate) {
    var label = "";
    if (descriptionIndex !== null) {
        eval("label = data." + descriptionIndex + ";");
    }

    var res = "<span class=\"btn-Edit btn btn-default btn-circle btn-sm\" title=\"Editar " + label + "\"><i class=\"fa fa-pencil\"></i></span>";
    res += "&nbsp;";
    res += "<span class=\"btn-Delete btn btn-default btn-circle btn-sm\" title=\"Eliminar " + label + "\"><i class=\"fa fa-trash\"></i></span>";
    if (duplicate === true) {
        res += "&nbsp;";
        res += "<span class=\"btn-Duplicate btn btn-default btn-circle btn-sm\" title=\"Crear nuevo a partir de " + label + "\"><i class=\"fa fa-copy\"></i></span>";
    }
    return res;
}

function BtnEditClicked(id) {
    GoEncryptedView(ItemDefinition.ItemName, id);
}

function BtnDeleteClicked(id) {
    alert("delete " + id);
}

function BtnDuplicateClicked(id) {
    alert("duplicate " + id);
}

function MaxText(rowIndex, data) {
    if (typeof data === "undefined" || data === null || data === "") { return ""; }

    if (data.length > 50) {
        var res = data.substr(0, 50);
        var lastSpace = res.lastIndexOf(" ");
        res = res.substr(0, lastSpace) + "...";
        return "<span title=\"" + data + "\">" + res + "</span>";
    }

    return data;
}