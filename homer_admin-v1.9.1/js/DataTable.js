function CreateList(definition, listId, listName) {
    RenderTableHeader(definition, listId);
    GetTableColumns(definition, listId);
    RenderTable(definition, listId, listName);
}

function RenderTableHeader(definition, listId) {
    var list = GetListById(definition.ItemName, listId);
    var res = "";
    if (list.Expandible === true) {
        res += "<th style=\"width:25px;\">&nbsp;</th>";
    }
    for (var x = 0; x < list.Columns.length; x++) {
        var column = list.Columns[x];
        console.log("Header column", column);
        if (typeof column.Expand !== "undefined" && column.Expand === true) {
            continue;
        }

        var source = column.DataProperty;
        var field = GetFieldByName(definition, source);
        var label = column.Label;
        if (typeof label === "undefined") {
            label = field.Label;
        }

        res += "<th>" + label + "</th>";
    }

    res += "<th>&nbsp;</th>"
    $("#tableHeader").html(res);
}

function GetTableColumns(definition, listId) {
    var list = GetListById(definition.ItemName, listId);
    var DescriptionIndex = null;
    for (var x = 0; x < list.Columns.length; x++) {
        var column = list.Columns[x];
        console.log("Column", column);
        if (x == 0 && list.Expandible) {
            columns.push({ data: column.DataProperty, "className": "details-control", "sortable": false });
            RenderColumns += "$(\"td\", row).eq(0).html(\"\");\n";
        }
        else {
            if (typeof column.Expand !== "undefined" && column.Expand === true) {
                continue;
            }

            var field = GetFieldByName(definition, column.DataProperty);
            var dataSource = column.DataProperty;
            if (typeof column.ReplacedBy !== "undefined") {
                dataSource = column.ReplacedBy;
            }

            if (typeof column.Descriptible !== "undefined" && column.Descriptible === true) {
                DescriptionIndex = dataSource;
            }

            columns.push({ data: dataSource });

            var renderColumn = null;
            if (typeof column.Render === "undefined" && column.Render !== null) {
                if (typeof field.DataFormat !== "undefined" && field.DataFormat !== null) {
                    column.Render = field.DataFormat.Name;
                }
            }

            if (field.Type === "url") {
                renderColumn = "$(\"td\", row).eq(" + x + ").html(ToWebPageBlank(data." + dataSource + "))";
            }
            else if (field.Type === "email") {
                console.log("email");
                renderColumn = "$(\"td\", row).eq(" + x + ").html(ToMail(data." + dataSource + "))";
            }
            else if (typeof column.Render !== "undefined") {
                if (column.Render.indexOf("#") !== -1) {
                    var func = column.Render.split("#")[0] + "data." + column.DataProperty + column.Render.split("#")[1];
                    renderColumn = "$(\"td\", row).eq(" + x + ").html(" + func + ")";
                }
                else {
                    renderColumn = "$(\"td\", row).eq(" + x + ").html(" + column.Render + "(data." + dataSource  + "))";
                }
            }

            if (typeof column.ToolTip !== "undefined") {
                if (renderColumn === null) {
                    renderColumn = "$(\"td\", row).eq(" + x + ").html(RenderSpanToolTip(data." + dataSource + ",data." + column.ToolTip + "))";
                }
                else {
                    toolTipField = column.ToolTip;
                    renderColumn += ".attr(\"title\", \"hola\")";
                }
            }

            if (renderColumn !== null) {
                RenderColumns += renderColumn + ";\n";
            }
        }
    }

    console.log("DescriptionIndex", DescriptionIndex);
    columns.push({ data: "Id", "sortable": false, "width": 150 });
    RenderColumns += "$(\"td\", row).eq(" + (columns.length - 1) + ").html(buttons(data, '" + DescriptionIndex + "', " + ItemDefinition.Lists[0].Duplicate + "));";
    return RenderColumns;
}

function RenderTable(itemDefiniton, listId, tableName, parameters) {
    var listDefinition = GetListDefinitionById(itemDefiniton, listId);
	console.log(listDefinition);
	var extraParams = "";
	if(typeof listDefinition.Parameters !== "undefined") {
		var params = listDefinition.Parameters;
		for(var x=0;x<params.length;x++) {
		
            if (params[x].Value === "#ApplicationUserId") {
				extraParams += "&" + params[x].Name + "=" + actualUser.Id;
			}
			else {
				extraParams += "&" + params[x].Name + "=" + params[x].Value;
			}
		
			console.log(params[x].Name, params[x].Value);
		}
	}
    console.log("Extraparams", extraParams);

    var tableSorting = [[1, "asc"]];
    if (typeof listDefinition.Sorting !== "undefined" && listDefinition.Sorting !== null) {
        tableSorting = [];
        for (var s = 0; s < listDefinition.Sorting.length; s++) {
            tableSorting.push([listDefinition.Sorting[s].Index , listDefinition.Sorting[s].SortingType ]);
        }
    }
		
    var table_Items = $("#" + tableName).dataTable(
    {
        "ajax":
        {
            "url": "/Instances/" + CustomerName + "/Data/ItemDataBase.aspx?Action=GetList&ItemName=" + queryParams.Item + "&ListId=" + queryParams.ListId + extraParams,
            "dataType": "json",
            "serverSide": true,
            "dataSrc": "data",
        },
		"deferRender": true,
		"scrollY": ($(window).height() - 330),
		"scrollCollapse": true,
        "processing": false,
		"pageLength": 50,
                "sDom": "<'dt-toolbar'<'col-xs-12 col-sm-4'f><'col-sm-8 col-xs-8 hidden-xs'C|l>>r" +
                "t" + 
                "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-sm-6 col-xs-12'p>>",
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "order": tableSorting,
        "aButtons": ["refresh", "copy", "csv", "pdf"],
        "columns": columns,
        "createdRow": function (row, data, index) { eval(RenderColumns); },
        "drawCallback": function () { 
			$(".demo-trigger").css("border", "4px solid #f00;"); 
			$(".demo-trigger").leroyZoom({
				"zoomTop": 20,
				"zoomLeft": 20,
				"parent": "body",
				"preload": Dictionary.Zoom.PreloadMessage,
				"error": Dictionary.Zoom.LoadErrorMessage
			});
			
			if (typeof ItemDefinition.Lists[0].GridMode !== "undefined") {
				
				if($("#BtnShowGrid").length === 0){
					$(".ColVis").after("<a href=\"\" class=\"btn btn-primary\" id=\"BtnShowGrid\" style=\"margin-right:4px;float:right;display:none;\"><i class=\"fa fa-th\"></i></a>");
					$(".ColVis").after("<a href=\"\" class=\"btn btn-primary\" id=\"BtnShowList\" style=\"margin-right:4px;float:right;display:none;\"><i class=\"fa fa-list\"></i></a>");
					$("#BtnShowGrid").on("click", ShowGrid);
					$("#BtnShowList").on("click", ShowList);
				}
	
				if($("#listTable_wrapper").hasClass("grid"))
				{
					ShowGrid();
				}
			}	

			$(".dataTables_scrollHeadInner").css("width","100%");
		},
		"fnPreDrawCallback": function (oSettings) {
			$(".dataTables_scrollBody").show();
			if (typeof ItemDefinition.Lists[0].GridMode !== "undefined") {
				// create a thumbs container if it doesn't exist. put it in the dataTables_scrollbody div
				if ($("#thumbs_container").length < 1) $(".dataTables_scrollBody").after("<div id=\"thumbs_container\" style=\"display:none;\"></div>");

				// clear out the thumbs container
				$("#thumbs_container").html("");
				if(ItemDefinition.Lists[0].GridMode === "default" && !$("#listTable_wrapper").hasClass("list")){
					$("#listTable_wrapper").addClass("grid");
				}
				else {
					$("#listTable_wrapper").addClass("list");
				}
			}
			else{
				$("#listTable_wrapper").addClass("list");
			}
			return true;
		},
		"fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
			if (typeof ItemDefinition.Lists[0].GridMode !== "undefined") {
				var res = "<div class=\"thumb_item\">";
				res += "<div class=\"thumbContent\">";
				res += "<h5>" + aData.N + "</h5>";
				res += "<div>";
				
				if(aData.I === "/img/blank.gif"){
					res+= "<img src=\"" + aData.I + "\">";
				}
				else{
					res += "<div class=\"imagecontainer\" style=\"text-align:center;\">";
					res += "<a href=\"#\" class=\"demo-trigger\" data-medium-url=\"" + aData.I + "\" data-large-url=\"" + aData.I + "\">";
					res += "<img src=\"" + aData.I + "\" />";
					res +="</a></div>";
				}				
				
				res += "</div><div class=\"thumb_data\">";
				res += " Ref: <strong>" + aData.R + "</strong><br />";
				res += " " + aData.T + "<br />";
				res += " " + aData.F + "<br />";
				res += "</div></div></div>";

				$("#thumbs_container").append(res);
				return nRow;
			}
		}
    });
	
	$("#" + tableName + "_wrapper .dataTables_filter input").on("blur", ListFilterChanged);

    // Add event listener for opening and closing details
    $("#" + tableName + " tbody").on("click", "tr .btn-Edit", function (e) {
        console.log(e.target.className);
        var table = $("#" + tableName).DataTable();
		var rowIndex = 0;
        var data = table.row($(this).parents('tr')).data();
		var tableData = [];
		var cont = 0;
		$.each(table.rows( { filter: 'applied' } ).data(), function() {
			tableData.push(this.Id);
			if(this.Id === data.Id){ 
				rowIndex = cont;
				console.log(cont, this.Id);
			}
			cont++;
		});
		listSerie.actual = rowIndex;
		listSerie.ids = tableData;
		console.log(listSerie);
        BtnEditClicked(data.Id);
        event.stopPropagation();
        return false;
    });

    // Add event listener for opening and closing details
    $("#" + tableName + " tbody").on("click", "tr .btn-Delete", function (e) {
        console.log(e.target.className);
        var table = $("#" + tableName).DataTable();
        var data = table.row($(this).parents('tr')).data();
        BtnDeleteClicked(data.Id);
        event.stopPropagation();
        return false;
    });

    // Add event listener for opening and closing details
    $("#" + tableName + " tbody").on("click", "tr .btn-Duplicate", function (e) {
        console.log(e.target.className);
        var table = $("#example1").DataTable();
        var data = table.row($(this).parents('tr')).data();
        BtnDuplicateClicked(data.Id);
        event.stopPropagation();
        return false;
    });

	$(".ColVis_MasterButton").remove();

    $(".refesh").on("click", TableReload);
    //$(".ColVis").hide();
    $(".ColVis").after("<span class=\"btn btn-primary btn-success\" id=\"BtnNew\" style=\"float:right;\"><i class=\"fa fa-plus\"></i> Añadir " + ItemDefinition.Layout.Label.toLowerCase() + "</span>");
	
    $("#BtnNew").on("click", function () { GoNew(ItemDefinition.ItemName, null) });


    if (typeof ItemDefinition.Lists[0].AutoClick !== "undefined" || ItemDefinition.Lists[0].AutoClick === true) {
        $("#" + tableName + " tbody").on("click", "tr", function (e) {
            console.log(e);
            var table = $("#" + tableName).DataTable();
            var data = table.row(this).data();
            BtnEditClicked(data.Id);
        });
    }

    // Add event listener for opening and closing details
    if (typeof ItemDefinition.Lists[0].Expandible !== "undefined" || ItemDefinition.Lists[0].Expandible === true) {
        $("#" + tableName + " tbody").on("click", "td.details-control", function (e) {
            var nTr = this.parentNode;
            var table = $("#" + tableName).dataTable();

            if (nTr.className.indexOf('shown') !== -1) {
                table.fnClose(nTr);
                $(nTr).removeClass('shown');
            }
            else {
                var nDetailsRow = table.fnOpen(nTr, fnFormatDetails(table.fnGetData(nTr)), 'details');
                $('div.innerDetails', nDetailsRow).slideDown();
                $(nTr).addClass('shown');
            }
            oldRow = nTr;
            event.stopPropagation();

            return false;
        });
    }

    eval("$(\"#side-menu [href='" + location.pathname + "']\").parent().addClass(\"active\")");
    $("#example1 TD").css("vertical-align", "middle");
}

function ShowGrid(){
	$("#listTable_wrapper").addClass("grid");
	$("#listTable_wrapper").removeClass("list");
	$(".dataTables_scrollBody").hide();
	$("#thumbs_container").show();
	if(ItemDefinition.Lists[0].GridMode.toUpperCase() !== "EXCLUSIVE"){
		$("#BtnShowGrid").hide();
		$("#BtnShowList").show();
	}
	$(".dataTables_scrollHeadInner").css("width","100%");
	return false;
}

function ShowList(){
	$("#listTable_wrapper").addClass("list");
	$("#listTable_wrapper").removeClass("grid");
	$(".dataTables_scrollBody").show();
	$("#thumbs_container").hide();
	if(ItemDefinition.Lists[0].GridMode.toUpperCase() === "DEFAULT"){
		$("#BtnShowGrid").show();
		$("#BtnShowList").hide();
	}
	$(".dataTables_scrollHeadInner").css("width","100%");
	return false;
}