$(document).ready(function () {
    /*
     *CORRECCION: El plugin requiere que la tabla tenga un thead con los encabezados de la
     * tabla y el gridview no genera este elemento, al traducir el gridview a tabla solo genera
     * un tbody y un tfooter, por lo que en el siguiente código adecúo la tabla antes de 
     * activar el plugin
     * */
    let tabla = $('#contenido_gvMaterias');
    //Obtengo la fila de los encabezados en el gridview colocó en el tbody (la primera)
    let fila = $(tabla).find("tbody tr:first").clone();
    //La elimino del tbody
    $(tabla).find("tbody tr:first").remove();
    //Creo un elemento thead y le añado la fila de encabezados
    let encabezado = $("<thead />").append(fila);
    //Añado el thead a la tabla antes del tbody
    $(tabla).children('tbody').before(encabezado);
    //Activamos el plugin
    $('#contenido_gvMaterias').DataTable();

    function validar(e) {
        if ($("#txtAgregarEditarMaterias").val().trim().length >= 5 && $("#txtAgregarEditarMaterias").val().trim().length <= 30) {
            $("#txtAgregarEditarMaterias").removeClass("is-invalid");
            $("#txtAgregarEditarMaterias").addClass("is-valid");
            $("#btnAceptar").prop("disabled", false);
        } else {
            $("#txtAgregarEditarMaterias").removeClass("is-valid");
            $("#txtAgregarEditarMaterias").addClass("is-invalid");
            $("#btnAceptar").prop("disabled", true);
        }
    }
    $("#txtAgregarEditarMaterias").on('keyup', validarTextBox);
    function validarTextBox() {
        if (event.keyCode != 13) {
            validar();
        }
    }


});
