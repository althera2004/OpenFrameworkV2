function INSCRIPCION_CustomActions() {
    if (typeof ACTIVIDAD_Context !== "undefined") {
        $("#InscripcionTxtActividadId").val(ACTIVIDAD_Context.Id);
        DisableSelect("InscripcionTxtActividadId");
    }
}