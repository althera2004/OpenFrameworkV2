var ALUMNO_ItemToDelete = null;

function ALUMNO_AfterPageLoadList() {
    $("H2").html("Llistat d'Alumnes");
    $("#BtnNew").html("<i class=\"fa fa-plus\"></i>&nbsp;Afegir alumne");
    $(".btn-danger").removeAttr("onclick");
    $(".btn-danger").on("click", ALUMNO_DeleteConfirmed);
}

function afterdataTableSuccess(tableName) {
    if (tableName === "datatable_col_reorder_Alumno") {
        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_Alumno .btn-edit").each(function (index) {
            $(this).removeAttr("onclick");
        });

        // Se elimina el evento de BtnDelete que se gestiona a continuación
        $("#datatable_col_reorder_Alumno .BtnDelete").each(function (index) {
            $(this).removeAttr("onclick");
        });

        $("#datatable_col_reorder_Alumno tbody").unbind('click').on("click", "td .BtnDelete", function (e) {
            console.log(e);
            var table = $("#" + tableName).DataTable();
            var data = table.row($(this).parents('tr')).data();
            ALUMNO_Delete(data);
            $("#DeleteListLauncher").click();
            e.stopPropagation();
            return false;
        });

        $("#datatable_col_reorder_Alumno tbody").on("click", "tr", function () {
            var table = $("#datatable_col_reorder_Alumno").DataTable();
            var data = table.row(this).data();
            ALUMNO_GoFicha(data.Id);
        });

        $("#datatable_col_reorder_Alumno TR").addClass("pointer");
    }
}

function ALUMNO_GoFicha(id) {
    var params = new Array();
    GoEncryptedItemView("Alumno", id, "Custom", params);
}

function ALUMNO_Delete(data)
{
    console.log("delete", data);
    $("#dialog-delete H2").html("Eliminar <strong>alunne</strong>");
    ALUMNO_ItemToDelete = {
        "ItemName": "Alumno",
        "ItemDefinition": ItemDefinition,
        "ItemId": data.Id,
        "ItemDescription": GetItemDescription(ItemBuilderDefinition, data)
    }
    $("#ItemToDeleteLabel").html("Alumne");
    $("#itemDeletableName").html(ALUMNO_ItemToDelete.ItemDescription);
}

function ALUMNO_DeleteConfirmed() {
    $("#dialog-delete").click();
    var data = {
        "itemId": ALUMNO_ItemToDelete.ItemId,
        "itemName": "alumno",
        "instanceName": CustomerName,
        "applicationUserId": User.Id,
        "userDescription": User.TraceDescription
    };

    try {
        $("#working").click();
    }
    catch (e) {
        console.log(e);
    }

    $.ajax({
        type: "POST",
        url: "/Data/ItemDataBase.aspx/Inactive",
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        data: JSON.stringify(data, null, 2),
        success: function(msg) {
            $(".botTempo").click();

            if (msg.d.Success === false) {
                $.bigBox({
                    title: "Ha hagut una errada",
                    content: "<i class='fa fa-clock-o'></i>&nbsp;<i>" + msg.d.MessageError + "</i>",
                    color: "#C46A69",
                    icon: "fa fa-warning shake animated",
                    timeout: 40000
                });
            }
            else {
                TableRefresh("Alumno");
                $.bigBox({
                    title: "Alumne eliminat",
                    content: "S'ha eliminat correctament l'adjudicació <strong>" + ALUMNO_ItemToDelete.ItemDescription + "</strong>",
                    color: "#739E73",
                    timeout: 8000,
                    icon: "fa fa-check",
                    number: "4"
                });
                console.log("Delete success", itemToDelete);
                TableRefresh(msg.d.MessageError);
                itemToDelete = null;
            }
        },
        error: function(msg) {
            console.log(msg);
            $.smallBox({
                title: "Ha hagut una errada",
                content: "<i class='fa fa-clock-o'></i> <i>" + msg.Message + "</i>",
                color: "#C46A69",
                iconSmall: "fa fa-times fa-2x fadeInRight animated",
                timeout: 4000
            });
        }
    });

    return false;
}