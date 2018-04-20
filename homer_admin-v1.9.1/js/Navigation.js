var urlDestination = null;
function PreventDirty(url){
	urlDestination = url;
	if(PageType === "form"){
		var origin = Data;
		var actual = GetFormData();
		if(JSON.stringify(origin) !== JSON.stringify(actual))
		{
			var differences = GetDifferences(origin, actual);
			console.clear();
			console.log(differences);
			
			var res = "" +
					"<table style=\"width:100%;border:1px solid #ccc;\"><thead><tr>" +
						"<th>Dato</div>" +
						"<th>Valor original</th>" +
						"<th>Valor modificado</th>" +
					"</tr></thead>";
					
            for (var x = 0; x < differences.length; x++){
                var field = GetFieldByName(ItemDefinition, differences[x][1]);

                var label = field.Label;
                var oldValue = differences[x][2];
                var newValue = differences[x][3];

                if (FieldIsFK(ItemDefinition, differences[x][1])) {
                    var FK = GetFKFromField(ItemDefinition, differences[x][1]);
                    var oldRef = GetByIdFromList(FK, oldValue);
                    var newRef = GetByIdFromList(FK, newValue);

                    if (oldRef === null) {
                        oldValue = "<i>"+ Dictionary.Common_None + "</i>";
                    }
                    else {
                        oldValue = oldRef.Description;
                    }

                    if (newRef === null) {
                        newValue = "<i>" + Dictionary.Common_None + "</i>";
                    }
                    else {
                        newValue = newRef.Description;
                    }
                }

				res += "<tr>" +
						"<td>" + label + "</td>" +
						"<td>" + oldValue + "</td>" +
						"<td>" + newValue + "</td>" +
					"</tr>";
			}
			
			res += "</table>";
			
			$("#myModalDirtyResults").html(res);
			$("#BtnModalDirty").click();
			return false;
		}
	}
	
	GoUrl(url);
}

function GoUrl(url) {
	$("#myModalDirtyCancel").click()
	if(typeof url === "undefined"){
		url = urlDestination;
	}
	
	urlDestination = null;
	navigationHistory.push(url);
	loadURL(url, $("#main"));
}

function ViewImagePage(controlId) {
    var url = $("#" + controlId).val();
    window.open(url);
}

function NavigationHistoryBack(){
	navigationHistory.pop();
	var url = navigationHistory.pop();
	PreventDirty(url);
}

function NavigationHistoryBackList(){
	var url = navigationHistory.pop();
	while(url.indexOf("ItemList.html") === -1){
		url = navigationHistory.pop();
	}
	PreventDirty(url);
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
	PreventDirty(url);
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
	PreventDirty(url);
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