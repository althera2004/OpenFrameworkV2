function test(url) {
	loadURL(url, $("#main"));
}
		
function GoNew(itemName, itemId, params) {
    GoEncryptedView(itemName, -1, params);
}
		
function GoEncryptedView(itemName, itemId, params) {
    var query = "&ItemName=" + itemName + "&Id=" + itemId;

    if (typeof params !== "undefined" && params !== null) {
        for (var x = 0; x < params.length; x++) {
            query += "&" + params[x];
        }
    }

    var url = "ItemView.html?" + $.base64.encode(guid() + query);
	console.log(url);
	test(url);
	return false;
}

function GoEncryptedList(ItemName, listId) {
	var query = "";
	//Añadir el id de la lista a la query
	if (typeof listId !== "undefined" && listId !== null && listId !== "") {
		query = $.base64.encode(guid() + "&Item=" + ItemName + "&InstanceName=" + CustomerName + "&ListId=" + listId);
	}
	else {
		query = $.base64.encode(guid() + "&Item=" + ItemName + "&InstanceName=" + CustomerName);
	}
	
	var url = "ItemList.html?" + query;
	test(url);
}

function DecodeEncryptedQuery() {
    if (typeof codedQuery !== "undefined") {

        if (codedQuery.indexOf("?") === -1) {
            return {};
        }
        var params = $.base64.decode(codedQuery.split("?")[1]).split("&");

        var res = "{";
        for (var x = 1; x < params.length; x++) {
            if (x > 1) {
                res += ",";
            }

            res += "\"" + params[x].split("=")[0] + "\":\"" + params[x].split("=")[1] + "\"";
        }

        res += "}";

        return JSON.parse(res);
    }

    return [];
}