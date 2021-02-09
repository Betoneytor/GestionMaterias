<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UnidadesForm.aspx.cs" Inherits="FrontEnd.UnidadesForm" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>Tus Calificaciones</title>
     <link rel="stylesheet" href="/Bootstrap/CSS/bootstrap.min.css">
    
    <link rel="stylesheet" type="text/css" href="./DataTables/CSS/jquery.dataTables.min.css">
    
    
    <script type="text/javascript" charset="utf8" src="./DataTables/JS/jquery-3.5.1.js"></script>
    <script type="text/javascript" charset="utf8" src="./DataTables/JS/jquery.dataTables.min.js"></script>
    <script type="text/javascript" charset="utf8" src="./JS/UnidadesForm.js"></script>

     <script type="text/javascript" charset="utf8" src="/Bootstrap/JS/bootstrap.min.js"></script>
     <script type="text/javascript" charset="utf8" src="/Bootstrap/JS/jquery-3.3.3.slim.min.js"></script>
     <script type="text/javascript" charset="utf8" src="/Bootstrap/JS/popper.min.js"></script>
</head>
<body>
     <form id="form1" runat="server" novalidate>
     <div class="form-group">
         
         <h3>Tus Calificaciones</h3>
         <asp:Button ID="btnRegresar" class="btn btn-info  float-right" Font-Bold="True"  runat="server" Text="Regresar" OnClick="btnRegresar_Click" />
      </div>
    

       <!-------------------INICIO MODAL---------------------->
    <div class="modal" id="mdlConfirmarEliminar" tabindex="-1" role="dialog">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title">Confirmar Eliminar</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
        </button>
      </div>
      <div class="modal-body">
        <p>Estás seguro que deseas eliminar?</p>
      </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-secondary" data-dismiss="modal">Cancelar</button>
        <asp:Button ID="btnConfirmarEliminar" CssClass="btn btn-danger"  OnClick="btnConfirmarEliminar_Click" runat="server" Text="Eliminar" />
      </div>
    </div>
  </div>
</div>
    <!--FIN MODAL -->


        <asp:HiddenField ID="rowTablaEliminar" runat="server" />
      <asp:HiddenField ID="idAgregarUnidad" runat="server" Value="0"/>

         <asp:Label ID="lblMatereriaGestionando" runat="server" Font-Bold="True" ForeColor="#030746" Font-Size="Medium"></asp:Label>

        <div class="form-row">
            <div class="col-md-6">
                <asp:Label ID="lblAgregarEditarNUnidad" for="lblAgregarEditarNUnidad" runat="server" Font-Bold="True" ForeColor="#030746" Text=" Numero Unidad" ></asp:Label>
                <asp:TextBox ID="txtAgregarEditarNUnidad" CssClass="form-control" placeholder="Numero Unidad" runat="server" autocomplete="off"></asp:TextBox>
                <div class="valid-feedback">
                    Formato Correcto!
                </div>
                <div class="invalid-feedback">
                    El numero de la unidad es obligatorio y debe tener solo numeros del 1 al 15.
                </div>
               
            </div>
            
        </div>
        <div class="form-row">
            <div class="col-md-6">
                <asp:Label ID="lblAgregarEditarCalificacion" for="txtAgregarEditarCalificacion" runat="server" Font-Bold="True" ForeColor="#030746" Text="Agregar Calificacion" ></asp:Label>
                <asp:TextBox ID="txtAgregarEditarCalificacion" CssClass="form-control" placeholder="Calificacion" runat="server" autocomplete="off" onkeyup="validarTextBox()"></asp:TextBox>
                <div class="valid-feedback">
                    Formato Correcto!
                </div>
                <div class="invalid-feedback">
                     El numero de la Calificacion es obligatorio y debe tener solo numeros del 1 al 100.
                </div>
            </div>
            
        </div>
        <div class="form-group">
            <asp:Label ID="lblInfoError" runat="server" Font-Bold="True" ForeColor="#FF3300"></asp:Label><br />
            
            <asp:Button ID="btnAceptar" CssClass="btn btn-success" runat="server" Text="Agregar Calificacion" OnClick="btnAceptar_Click"  />
            
            
        </div>
       
        <asp:GridView ID="gvUnidades" CssClass="table table-bordered table-striped" runat="server" 
             DataKeyNames="numeroUnidad" OnRowCommand="gvUnidades_RowCommand">
            <Columns>
                <asp:BoundField DataField="idUnidad" HeaderText="Clave(IDU)" />
                <asp:BoundField DataField="numeroUnidad" HeaderText="Unidad(numU)" />
                <asp:BoundField DataField="calificacionUnidad" HeaderText="Calificacion(numCal)" />
                <asp:ButtonField CommandName="Eliminar"  ButtonType="Button" ControlStyle-CssClass="btn btn-danger" Text="Eliminar"/>
               
            </Columns>
        </asp:GridView>
   
    </form>
</body>
</html>
