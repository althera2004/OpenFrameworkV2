var ACTIVIDAD_Context = {
    "Id":null
}

function ACTIVIDAD_CustomActions() {
    if (typeof Data.Id !== "undefined") {
        ACTIVIDAD_Context.Id = Data.Id;
    }

    select2Combobox("ActividadTxtProveedorId");
    $("#ActividadTxtProveedorId").on("change", ACTIVIDAD_FillComboMonitor);

    ACTIVIDAD_FillComboMomnitor();
}

function ACTIVIDAD_FillComboMonitor() {
    var proveedorId = $("#ActividadTxtProveedorId").val() * 1;

    console.log("ACTIVIDAD_FillComboMomnitor", proveedorId);

    var candidatos = [];
    for (var x = 0; x < FK["Monitor"].length; x++) {
        if (FK["Monitor"][x].ProveedorId === proveedorId) {
            candidatos.push(FK["Monitor"][x]);
        }
    }

    FillCombo("ActividadTxtMonitorId", candidatos, true, true);

    if (candidatos.length > 0) {
        EnableSelect("ActividadTxtMonitorId");
    }
}