function SetHeader() {
	$("#PageTitle").html(ItemDefinition.Layout.LabelPlural);		
	if(typeof ItemDefinition.Description !== "undefined" && ItemDefinition.Description !== null && ItemDefinition.Description !== ""){
		$("#PageSubtitle").html("&nbsp;/&nbsp;" + ItemDefinition.Description);
	}
	else{
		$("#PageSubtitle").html(" ");
	}
	$("#ActualBradCrumb").html(ItemDefinition.Layout.LabelPlural);
}