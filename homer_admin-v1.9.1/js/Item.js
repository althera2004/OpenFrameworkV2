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

function FieldIsFK(itemDefinition, fieldName){
	if (typeof itemDefinition === "undefined" || itemDefinition === null) {
		return false;
	}
	
	for(var x=0; x< itemDefinition.ForeignValues.length;x++){
		if(fieldName === itemDefinition.ForeignValues[x].ItemName + "Id"){
			return true;
		}
	}
	
	return false;
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
				$("#itemsTable").append("<div class=\"col col-sm-3\" id=\"Item_" + itemNames[x] + "_Status\">&nbsp;<i class=\"fa fa-gear\" style=\"color:#333;\"></i>" + itemNames[x] + "</div>");
				totalItems++;
			}
			
			for (var n = 0; n < itemNames.length; n++) {
				LoadItem(itemNames[n]);
			}
        },
        error: function (msg, text) {
            alert(text);
        }
    });
}

var loaded = 0;
var totalItems = 0;
		
var RefreshFKActual = 0;
function RefreshFK() {
	ReloadFK(itemNames[RefreshFKActual]);
	RefreshFKActual++;
	if (RefreshFKActual >= itemNames.length) {
		RefreshFKActual = 0;
	}
	setTimeout(RefreshFK, 10000);
}

function ReloadFK(itemName){
	$.getJSON("/Data/FKScript.aspx?r=1&itemName=" + itemName + "&InstanceName=" + CustomerName + "&ac=" + guid(), function (data) {
		FK[data.ItemName] = data.Data;
	});
}

function LoadItem(itemName) {
	console.log("LoadItem", itemName);
	$.getJSON("/Data/FKScript.aspx?r=1&itemName=" + itemName + "&InstanceName=" + CustomerName + "&ac=" + guid(), function (data) {
		console.log("loaded", itemName + " --> " + data.Data.length);
		$("#Count_" + itemName).html("&nbsp(" + data.Data.length + ")");
		UpdateLoading(itemName, data.Data.length);
		FK[data.ItemName] = data.Data;
	});
}

function UpdateLoading(itemName, total) {
	loaded++;
	console.log("Loaded", loaded + " of " + totalItems);
	$("#ItemsLoadedProgressBar").css("width", (loaded * 100 / totalItems) + "%");
	$("#ItemsLoadedProgressBar").html(Dictionary.Common_Loading + "... " + loaded + " / " + totalItems);
	$("#Item_" + itemName + "_Status").html("<i class=\"fa fa-check\" style=\"color:#3c3;\"></i>&nbsp;<strong>" + itemName + "</strong>&nbsp<i>(" + total + ")</i>");
	if (loaded === totalItems) {
		$("#left-panel").show();
		$("#HeaderControls").show();
		test("test.html")
		RefreshFK();
	}
}

$("#ItemsLoadedProgressBar").html(Dictionary.Common_Loading + "... 0 / " + totalItems);

function Save(data) {
    //string itemName, dynamic itemData, string instanceName, long applicationUserId)
    var sendData = {
        "itemName": ItemDefinition.ItemName,
        "itemData": JSON.stringify(data).split('{').join('^'),
        "instanceName": CustomerName,
        "applicationUserId": actualUser.Id
    };

    console.log(sendData);
    $.ajax({
        type: "POST",
        url: "/Data/ItemDataBase.aspx/Save",
        data: JSON.stringify(sendData),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        async: false,
        success: function (msg) {
            console.log(msg.d.ReturnValue);         
        },
        error: function (msg, text) {
            console.log(msg);
        }
    });

}