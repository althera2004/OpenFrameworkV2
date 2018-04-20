function GetDefinitionByItemName(itemName) {
    for (var x = 0; x < ItemDefinitions.length; x++) {
        if (ItemDefinitions[x].ItemName === itemName) {
            return ItemDefinitions[x];
        }
    }

    return null;
}

function GetListById(itemName, listId) {
    var item = GetDefinitionByItemName(itemName);
    if (item === null) {
        return null;
    }

    if (typeof item.Lists === "undefined") {
        return null;
    }

    if (item.Lists.length === 0) {
        return null;
    }

    for (var l = 0; l < item.Lists.length; l++) {
        if (item.Lists[l].Id = listId) {
            return item.Lists[l];
        }
    }

    return null;
}

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

function FieldIsFK(itemDefinition, fieldName) {
	if (typeof itemDefinition === "undefined" || itemDefinition === null) {
		return false;
	}
	
	for(var x=0; x< itemDefinition.ForeignValues.length;x++){
		if(fieldName === itemDefinition.ForeignValues[x].ItemName + "Id") {
			return true;
		}
	}
	
	return false;
}

function GetFKFromField(itemDefinition, fieldName) {
    if (typeof itemDefinition === "undefined" || itemDefinition === null) {
        return false;
    }

    for (var x = 0; x < itemDefinition.ForeignValues.length; x++) {
        if (fieldName === itemDefinition.ForeignValues[x].ItemName + "Id") {
            return FK[itemDefinition.ForeignValues[x].ItemName];
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

    var afterLoad = null;
    eval("afterLoad = typeof " + queryParams.ItemName.toUpperCase() + "_AfterLoad;")
    console.log("AfterLoad", afterLoad);
    if (afterLoad === "function") {
        eval(queryParams.ItemName.toUpperCase() + "_AfterLoad();");
    }
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

function GetUrlParameters(definition) {
    var parameters = "";
    if (typeof definition.FKList !== "undefined") {
        if (definition.FKList.Filter !== []) {
            var filter = definition.FKList.Filter;
            for (var x = 0; x < filter.length; x++) {
                parameters += "&" + filter[x].FieldName + "=";
                if (filter[x].Condition.Value === "#ApplicationUserId") {
                    parameters += actualUser.Id;
                }
                else {
                    parameters += filter[x].Condition.Value;
                }
            }
        }
    }

    return parameters;
}

function FKGetUrl(itemName) {
    var definition = GetDefinitionByItemName(itemName);
    var parameters = GetUrlParameters(definition);
    return "/Data/FKScript.aspx?r=1&itemName=" + itemName + "&InstanceName=" + CustomerName + parameters + "&ac=" + guid();
}

function ReloadFK(itemName) {
    $.getJSON(FKGetUrl(itemName), function (data) {
		FK[data.ItemName] = data.Data;
	});
}

function LoadItem(itemName) {
    console.log("LoadItem", itemName);
    var parameters = "";

    $.getJSON("/Instances/" + CustomerName + "/ItemDefinition/" + itemName + ".item?ac=" + guid(), function (json) {
        console.log("Definition", json);
        ItemDefinition = json;
        ItemDefinitions.push(ItemDefinition);

        $.getJSON(FKGetUrl(ItemDefinition.ItemName), function (data) {
            console.log("loaded", itemName + " --> " + data.Data.length);
            $("#Count_" + itemName).html("&nbsp(" + data.Data.length + ")");
            UpdateLoading(itemName, data.Data.length);
            FK[data.ItemName] = data.Data;
        });
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
		PreventDirty("test.html")
        RefreshFK();
        $("#side-menu a").show();
	}
}

$("#ItemsLoadedProgressBar").html(Dictionary.Common_Loading + "... 0 / " + totalItems);

function SaveActualForm(){
	Save(GetFormData());
}

function DeleteActualForm(){
	alert("delete");
}

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
            if (Data === null || Data.Id < 0) {
                Data.Id = msg.d.ReturnValue.split('|')[1] * 1;
            }
            GetById(ItemDefinition.ItemName, Data.Id);
        },
        error: function (msg, text) {
            console.log(msg);
        }
    });
}

function GetDifferences(obj1, obj2, _Q) {
		_Q = (_Q == undefined)? new Array : _Q;        

		function size(obj) {
	        var size = 0;
	        for (var keyName in obj){
	        	if(keyName != null) size++;
	        }
	        return size;
	    };
 
		if (size(obj1) != size(obj2)) {
			//console.log('JSON compare - size not equal > '+keyName)
		};
 
		var newO2 = jQuery.extend(true, {}, obj2);
 
	    for(var keyName in obj1){
	        var value1 = obj1[keyName],
	        	value2 = obj2[keyName];
 
			delete newO2[keyName];
 
			if (typeof value1 != typeof value2 && value2 == undefined) {
				_Q.push(['missing', keyName, value1, value2])
			}else if (typeof value1 != typeof value2) {
				_Q.push(['diffType', keyName, value1, value2])
			}else{
		        // For jQuery objects:
		        if (value1 && value1.length && (value1[0] !== undefined && value1[0].tagName)) {
					if (!value2 || value2.length != value1.length || !value2[0].tagName || value2[0].tagName != value1[0].tagName) {
						_Q.push(['diffJqueryObj', keyName, value1, value2])
					}
				}else if(value1 && value1.length && (value1.tagName !== value2.tagName)){
					_Q.push(['diffHtmlObj', keyName, value1, value2])
				}else if (typeof value1 == 'function' || typeof value2 == 'function') {
					_Q.push(['function', keyName, value1, value2])
				}else if(typeof value1 == 'object'){
					var equal = GetDifferences(value1, value2, _Q);
				}else if (value1 != value2 && keyName !== "Active") {
					_Q.push(['diffValue', keyName, value1, value2])
				}
			};
	    }
 
        for (var keyName in newO2) {
            var oldValue = "";

            // si es nuevo no hay obj1
            if (obj1 !== null) {
                oldValue = obj1[keyName];
            }
			_Q.push(['new', keyName, oldValue, newO2[keyName]])
		}
 
		/*
	    */
		return _Q;
	}; // END compare()

    function GetById(itemName, itemId)
    {
        var dataSent = {
            "itemName": itemName,
            "itemId": itemId,
            "instanceName": CustomerName
        };

        $.ajax({
            type: "POST",
            url: "/Data/ItemDataBase.aspx/GetByIdJson",
            data: JSON.stringify(dataSent),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: false,
            success: function (msg) {
                eval("Data=" + msg.d + ";");
                console.log(Data);
                FillForm(Data);
                $(".demo-trigger").css("border", "4px solid #f00;");
                $(".demo-trigger").leroyZoom({
                    "zoomTop": 20,
                    "zoomLeft": 20,
                    "parent": "body",
                    "preload": Dictionary.Zoom.PreloadMessage,
                    "error": Dictionary.Zoom.LoadErrorMessage
                });
                LayoutModified(Data);

                $(".i-checks").iCheck({
                    checkboxClass: "icheckbox_square-green",
                    radioClass: "iradio_square-green"
                });

            },
            error: function (msg, text) {
                alert(text);
            }
        });
    }

function GetFKById(itemName, id) {
    if (FK[itemName] !== null) {
        for (var x = 0; x < FK[itemName].length; x++) {
            if (FK[itemName][x].Id === id * 1) {
                return FK[itemName][x];
            }
        }
    }

    return null;
    }

function GetByIdFromList(list, id) {
    for (var x = 0; x < list.length; x++) {
        if (list[x].Id === id) {
            return list[x];
        }
    }

    return null;
}

function GetByFieldFromList(list, fieldName, searchedValue) {
    for (var x = 0; x < list.length; x++) {
        if (list[x][fieldName] === searchedValue) {
            return list[x];
        }
    }

    return null;
}