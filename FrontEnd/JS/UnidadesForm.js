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

    $("#txtAgregarEditarNUnidad").on('keyup', validarTextBox);
    $("#txtAgregarEditarCalificacion").on('keyup', validarTextBox);


    function validarNUnidad(e) {
        if (parseInt($("#txtAgregarEditarNUnidad").val().trim()) >= 1 && parseInt($("#txtAgregarEditarNUnidad").val().trim()) <= 15) {
            $("#txtAgregarEditarNUnidad").removeClass("is-invalid");
            $("#txtAgregarEditarNUnidad").addClass("is-valid");
            
            
        } else {
            $("#txtAgregarEditarNUnidad").removeClass("is-valid");
            $("#txtAgregarEditarNUnidad").addClass("is-invalid");
           
        }
    }
    function validarCUnidad(e) {
        if (parseInt($("#txtAgregarEditarCalificacion").val().trim()) >= 1 && parseInt($("#txtAgregarEditarCalificacion").val().trim()) <= 100) {
            $("#txtAgregarEditarCalificacion").removeClass("is-invalid");
            $("#txtAgregarEditarCalificacion").addClass("is-valid");
            

        } else {
            $("#txtAgregarEditarCalificacion").removeClass("is-valid");
            $("#txtAgregarEditarCalificacion").addClass("is-invalid");

        }
    }
    


    function validarTextBox() {
        if (event.keyCode != 13) {
            validarNUnidad();
            validarCUnidad();
            
            if ($("#txtAgregarEditarNUnidad").hasClass("is-valid") & $("#txtAgregarEditarCalificacion").hasClass("is-valid")) {
                $("#btnAceptar").prop("disabled", false);

            } else {
                $("#btnAceptar").prop("disabled", true);
            }
           
        }
    }
    
});