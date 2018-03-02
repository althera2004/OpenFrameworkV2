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