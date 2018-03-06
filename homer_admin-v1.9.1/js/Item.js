function GetFieldByName(itemDefinition, fieldName) {
    if (typeof itemDefinition !== "undefined" && itemDefinition !== null) {
        var fields = GetFields(itemDefinition);
        if (fields !== null) {
            for (var x = 0; x < fields.length; x++) {
                if (fields[x].Name === fieldName) {
                    return fields[x];
                }
            }
        }
    }

    return null;
}

function GetFields(itemDefinition) {
    if (typeof itemDefinition !== "undefined" && itemDefinition !== null) {
        return itemDefinition.Fields;
    }
    return null;
}

function LayoutModified(data) {
    $("#LayoutModifiedByFullName").html(Data.ModifiedByFullName);
    $("#LayoutModifiedOn").html(DateToText(GetDateFromTextComplete(Data.ModifiedOn)));
}

function GetListDefinitionById(itemDefinition, listId) {
	for(var x=0;x<itemDefinition.Lists.length;x++) {
		if(itemDefinition.Lists[x].Id.toUpperCase() === listId.toUpperCase()) {
			return itemDefinition.Lists[x];
		}
	}
	
	return null;
}

function GetItemNames() {
	console.log("GetItemNames");
    $.ajax({
        type: "POST",
        url: "/Data/AsynchronousMessages.aspx/GetAllItems",
        data: JSON.stringify({ "instanceName": CustomerName }),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {            
            console.log(msg.d.ReturnValue);
			itemNames = msg.d.ReturnValue;
			for(var x=0;x<itemNames.length;x++){
				$("#itemsTable").append("<div class=\"col col-sm-3\">" + itemNames[x] + "</div>");
			}
			
			for (var x = 0; x < itemNames.length; x++) {
				LoadItem(itemNames[x]);
			}
        },
        error: function (msg, text) {
            alert(text);
        }
    });
}

var loaded = 0;
var totalItems = 0;

function LoadItem(itemName) {
	$.getJSON("/Data/FKScript.aspx?itemName=" + itemName + "&InstanceName=" + CustomerName + "&ac=" + guid(), function (data) {
		console.log("loaded", itemName + " --> " + data.Data.length);
		$("#Count_" + itemName).html("&nbsp(" + data.Data.length + ")");
		UpdateLoading(itemName);
		FK[data.ItemName] = data.Data;
	});
}

function UpdateLoading(itemName) {
	loaded++;
	$("#ItemsLoadedProgressBar").css("width", (loaded * 100 / totalItems) + "%");
	$("#status").html("Loading... " + loaded + " / " + totalItems);
	$("#Item_" + itemName + "_Status").html("<i class=\"fa fa-check\" style=\"color:#3c3;\"></i>");
	if (loaded === totalItems) {
		$("#left-panel").show();
		$("#HeaderControls").show();
		RefreshFK();
		$("nav").show();
		//GoEncryptedList("Tematica", "Custom");
	}
}

$("#status").html("Loading... 0 / " + totalItems);