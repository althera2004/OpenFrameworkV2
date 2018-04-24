function ToTime(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    try {
        console.log(value, typeof value);

        if (typeof value === "object") {
            return DateToText(value);
        }

        if (value.indexOf(":") !== -1) { return value; }
        value = value.substr(0, 10);
    }
    catch (e) {
        console.log("error ToTime", value);
    }
    var res = value.substr(8, 2) + "/" + value.substr(5, 2) + "/" + value.substr(0, 4);
    return res;
}

function ToHour(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (isNaN(value) && value.indexOf(":") !== -1) { return value; }
    if (isNaN(value * 1)) { return ""; }
    value = value * 1;
    var hour = Math.floor(value / 60).toString();
    var minute = (value % 60).toString();
    var res = ("0" + hour).slice(-2) + ":" + ("0" + minute).slice(-2);
    return res;
}

function ToDate(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value.indexOf("/") !== -1) {
        return value;
    }
    value = value.substr(0, 10);
    var res = value.substr(8, 2) + "/" + value.substr(5, 2) + "/" + value.substr(0, 4);
    return res;
}

function ToDateMonth(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value.indexOf("/") !== -1) {
        var val = value.split("/");
        value = val[2] + "-" + val[1] + "-" + val[0];
    }
    value = value.substr(0, 10);
    return monthNames[value.substr(5, 2) * 1] + "'" + value.substr(2, 2);
}

function ToDateTime(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value.indexOf("/") !== -1) {
        var val = value.split("/");
        value = val[2] + "-" + val[1] + "-" + val[0];
    }
    return value.substr(8, 2) + "/" + value.substr(5, 2) + "/" + value.substr(0, 4) + " " + value.substr(11);
}

function ToDateText(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value.indexOf("/") !== -1) {
        var val = value.split("/");
        value = val[2] + "-" + val[1] + "-" + val[0];
    }
    value = value.substr(0, 10);
    return (value.substr(8, 2) * 1).toString() + " de " + monthNames[value.substr(5, 2) * 1] + " de " + value.substr(0, 4);
}

function ToBooleanText(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true) { return Dictionary.Common_Yes; }
    if (value === false) { return Dictionary.Common_No; }
    if (value === "true") { return Dictionary.Common_Yes; }
    if (value === "false") { return Dictionary.Common_No; }
    return "";
}

function ToBooleanIcon(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true) { return "<i class=\"fa fa-lg fa-fw fa-check\"></i>"; }
    if (value === false) { return "<i class=\"fa fa-lg fa-fw fa-times\"></i>"; }
    return "";
}

function ToBooleanCheck(value) {
    if (typeof value !== "undefined" && value !== null && value === true) {
        return "<i class=\"fa fa-lg fa-fw fa-check-square-o\"></i>";
    }

    return "<i class=\"fa fa-lg fa-fw fa-square-o\"></i>";
}

function ToBooleanCheckNull(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value === true) { return "<i class=\"fa fa-lg fa-fw fa-check-square-o\"></i>"; }
    if (value === false) { return ""; }
    return "";
}

function ToMoneyFormat(value, decimals) {
    // console.log("ToMoneyFormat", value);
    if (isNaN(value)) { return ""; }
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }

    value = value.toString().replace(",", ".");

    value = value * 1;
    value = parseFloat(Math.round(value * 100) / 100).toFixed(decimals);
    var res = value;
    var entera = "";
    var enteraRes = "";
    var decimal = "";

    entera = value.split(".")[0];
    decimal = value.split(".")[1];
    if (typeof decimal === "undefined") {
        decimal = "0";
    }

    while (decimal.length < decimals) {
        decimal += "0";
    }

    while (entera.length > 0) {
        if (entera.length < 4) {
            enteraRes = entera + "." + enteraRes;
            entera = "";
        }
        else {
            enteraRes = entera.substr(entera.length - 3, 3) + "," + enteraRes;
            entera = entera.substr(0, entera.length - 3);
        }
    }

    if (decimals === 0) {
        return enteraRes.substr(0, enteraRes.length - 1);
    }

    return enteraRes.substr(0, enteraRes.length - 1) + "," + decimal;
}

function ToDecimalFormat(value, decimals) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }

    var isPercentage = false;
    if (value.toString().indexOf("%") !== -1) {
        console.log("Decimal percentage");
        isPercentage = true;
        value = value.split("%")[0];
    }

    var precision = Math.pow(10, decimals);
    value = parseFloat(Math.round(value * precision) / precision).toFixed(decimals);
    var res = value;
    var entera = "";
    var enteraRes = "";
    var decimal = "";

    entera = value.split(".")[0];
    decimal = value.split(".")[1];
    if (typeof decimal === "undefined") {
        decimal = "0";
    }

    while (decimal.length < decimals) {
        decimal += "0";
    }

    while (entera.length > 0) {
        if (entera.length < 4) {
            enteraRes = entera + "." + enteraRes;
            entera = "";
        }
        else {
            enteraRes = entera.substr(entera.length - 3, 3) + "," + enteraRes;
            entera = entera.substr(0, entera.length - 3);
        }
    }

    if (decimals === 0) {
        return enteraRes.substr(0, enteraRes.length - 1);
    }

    return enteraRes.substr(0, enteraRes.length - 1) + "," + decimal;
}

function ToPercentageFormat(value, decimals) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    return ToMoneyFormat(value, decimals) + "&nbsp;%";
}

function ToEmail(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    return "<a href=\"mailto:" + value + "\">" + value + "</a>";
}

function ToUrl(value) {
    if (typeof value === "undefined") { return ""; }
    if (value === null) { return ""; }
    if (value === "") { return ""; }
    if (value.indexOf("http:") === -1) { value = "http://" + value; }
    return "<a href=\"" + value + "\" target=\"_blank\">" + value + "</a>";
}

function ToSingleImage(x, value, type, full, field, instanceName, itemName) {
    if (typeof value === "undefined") { value = "/img/blank.gif"; }
    if (value === null) { value = "/img/blank.gif"; }
    if (value === "") { value = "/img/blank.gif"; }
    return "<div class=\"imagecontainer\"><img src=\"/Instances/" + CustomerName + "/data/img/" +  value + "?"+ (Math.random()) +"\" /></div>";
}

function ToSingleImageList(x, value, type, full, field, instanceName, itemName) {
    if (typeof value === "undefined") { value = "/img/blank.gif"; }
    if (value === null) { value = "/img/blank.gif"; }
    if (value === "") { value = "/img/blank.gif"; }
	
	if(value === "/img/blank.gif"){
		return "<div class=\"imagecontainer\" style=\"text-align:center;\"><img src=\"" +  value + "?"+ (Math.random()) +"\" style=\"width:30px;height:30px;border:1px solid #ddd;\" /></div>";
	}
	
	var image = "/Instances/" + CustomerName + "/data/img/" + value;
	
	var res = "<div class=\"imagecontainer\" style=\"text-align:center;\">";
	res += "<a href=\"#\" class=\"demo-trigger\" data-medium-url=\"" + image +"\" data-large-url=\"" + image + "\">";
    res += "<img src=\"" + image + "\" style=\"width:30px;height:30px;border:1px solid #ddd;\" />";
	res +="</a></div>";
	return res;
}

function GetDescription(data, type, full, field) {
    if (typeof data === "undefined") { return ""; }
    if (data === null) { return ""; }

    var type = jQuery.type(data);
    if (type === "string") {
        return data;
    }
    else if (type === "object") {
        return "<a href=\"#\" title=\"" + data.Id + "\">" + data.Description + "</a>";
    }
    else {
        var dataProp = field.settings.aoColumns[field.col].mData;
        if (dataProp.substr(dataProp.length - 2) === "Id") {
            var realData = dataProp.substr(0, dataProp.length - 2);
            if (typeof full[realData] === "undefined") { return data; }
            if (full[realData] === null) {
                if (data * 1 === 0) {
                    return "";
                }
                return data;
            }
			
            if (full[realData].Description !== "") {
                return full[realData].Description;
            }
            return data;
        }
    }

    return data;
}

function ToFixedList(data) {
    if (typeof data === "undefined") { return ""; }
    if (data === null) { return ""; }

    for (var x = 0; x < CoreFixedLists.length; x++) {
        for (var y = 0; y < CoreFixedLists[x].Data.length; y++) {
            if (CoreFixedLists[x].Data[y].Id === data) {
                return CoreFixedLists[x].Data[y].Description;
            }
        }
    }

    for (var i = 0; i < CustomFixedLists.length; i++) {
        for (var j = 0; j < CustomFixedLists[i].Data.length; j++) {
            if (CustomFixedLists[i].Data[j].Id === data) {
                return CustomFixedLists[i].Data[j].Description;
            }
        }
    }

    return "";
}

function ToFixedListLinked(data) {
    if (typeof data === "undefined") { return ""; }
    if (data === null) { return ""; }
    if (data.substr(data.length - 2) !== "Id") { return ToFixedList(data); }
    return data;
}

function ToAbstractItem(data, type, full, targetField) {
    if (data === null || targetField === null) { return ""; }
    if (data === "undefined" || targetField === "undefined") { return ""; }
    var itemName = full[targetField];
    var item = FK[itemName];
    var res = "";
    if (item !== null && typeof item !== "undefined") {
        for (var i = 0; i < items.length; i++) {
            if (item[i].Id * 1 === data * 1) {
                res = item[i].Description;
            }
        }
    }

    return res;
}

function ToIndirectFK(data, type, full, linkField, remoteItem, itemName, fieldName) {
    if(data === null || linkField === null || remoteItem === null || itemName === null) { return ""; }
    if(data === "undefined" || linkField === "undefined" || remoteItem === "undefined" || itemName === "undefined") { return ""; }
    if (fieldName.endsWith("Id"))
    {
        var res = null;
        eval("res= full." + fieldName.substr(0, fieldName.length - 2) + ";");
        return res;
    }

    var itemId = full[itemName + "Id"] * 1;
    var items = FK[itemName];
    var remotes = FK[remoteItem];
    var remoteId = 0;
    var res = "";

    if (typeof items !== "undefined") {
        for (var i = 0; i < items.length; i++) {
            if (items[i].Id * 1 === itemId * 1) {
                remoteId = items[i][remoteItem + "Id"];
            }
        }
    }

    if (typeof remotes !== "undefined") {
        for (var i = 0; i < remotes.length; i++) {
            if (remotes[i].Id * 1 === remoteId * 1) {
                res = remotes[i][linkField];
            }
        }
    }

    return res;
}

function MaxText(data, type, full, linkField, remoteItem, itemName, fieldName) {
    if (typeof data === "undefined" || data === null || data === "") {
        return "";
    }

    if (data.length > 50) {
        var res = data.substr(0, 50);
        var lastSpace = res.lastIndexOf(" ");
        res = res.substr(0, lastSpace) + "...";
        return "<span title=\"" + data + "\">" + res + "</span>";
    }

    return data;
}