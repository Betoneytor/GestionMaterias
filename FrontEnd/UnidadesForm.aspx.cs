using FrontEnd.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace FrontEnd
{
    public partial class UnidadesForm : System.Web.UI.Page
    {
        List<Unidad> listaUnidadesPMateria = new List<Unidad>();
        List<Unidad> listaDeTodasLasUnidades = new List<Unidad>();
        int idMayorClaveActual=0;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["idMateriaG"] != null && Session["nombreMateriaG"] != null)
            {
                if (Session["vamosAEliminar"] != null)
                {
                    //no cargamos nada
                    btnAceptar.Enabled = false;
                    lblMatereriaGestionando.Text = "Materia Gestionando: " + Session["nombreMateriaG"];

                    CrearListaDeTodasLasUnidades(ref listaDeTodasLasUnidades);
                    extraerListaPorMateria_MaxIdU_LimpiarLTLM();
                    llenarTablaConMateriasPorUnidad();
                }
                else {
                    //hacemos carga normal
                    btnAceptar.Enabled = false;
                    lblMatereriaGestionando.Text = "Materia Gestionando: " + Session["nombreMateriaG"];

                    CrearListaDeTodasLasUnidades(ref listaDeTodasLasUnidades);
                    extraerListaPorMateria_MaxIdU_LimpiarLTLM();
                    llenarTablaConMateriasPorUnidad();
                }
            }
            else 
            {
                Response.Redirect("MateriasForm.aspx");
            }

        }
        public void CrearListaDeTodasLasUnidades(ref List<Unidad> listaDeItems)
        {
            var doc = XDocument.Load(Server.MapPath("~/AlmacenamientoXML/UnidadesXML.xml"));

            listaDeItems = doc.Root
                .Descendants("Unidad")
                .Select(node => new Unidad
                {
                    idUnidad = int.Parse(node.Element("idU").Value),
                    idMateria = int.Parse(node.Element("idM").Value),
                    NumeroUnidad = int.Parse(node.Element("numU").Value),
                    CalificacionUnidad = int.Parse(node.Element("calU").Value)
                })
                .ToList();
        }
        public void extraerListaPorMateria_MaxIdU_LimpiarLTLM() 
        {
            int contadorUActual = 0;
            
            foreach (Unidad u in listaDeTodasLasUnidades)
            {
                if (u.idMateria == int.Parse(Session["idMateriaG"].ToString()))
                {
                    listaUnidadesPMateria.Add(u);
                }
                contadorUActual = u.idUnidad;
                if (contadorUActual>idMayorClaveActual) 
                {
                    idMayorClaveActual = contadorUActual;
                }
            }

            foreach (Unidad u in listaUnidadesPMateria)
            {
                listaDeTodasLasUnidades.Remove(u);
            }

        }

        public void llenarTablaConMateriasPorUnidad()
        {
            listaUnidadesPMateria.Sort((x, y) => x.NumeroUnidad.CompareTo(y.NumeroUnidad));
            //Evitar que agregue más columnas a la tabla
            gvUnidades.AutoGenerateColumns = false;
            gvUnidades.DataSource = null;
            gvUnidades.DataSource = listaUnidadesPMateria;
            gvUnidades.DataBind();
        }

        protected void gvUnidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Eliminar")
            {
                //guardamos la unidad a eliminar en la Sesion, para evitar perdida de datos durante el load del modal PAra confirmar eliminacion
                rowTablaEliminar.Value = e.CommandArgument.ToString();
                    Session["idUnidadEliminar"] = int.Parse(gvUnidades.Rows[int.Parse(rowTablaEliminar.Value.ToString())].Cells[0].Text);
                    Session["numeroUnidadEliminar"] = int.Parse(gvUnidades.Rows[int.Parse(rowTablaEliminar.Value.ToString())].Cells[1].Text);
                    Session["calificacionUnidadEliminar"] = int.Parse(gvUnidades.Rows[int.Parse(rowTablaEliminar.Value.ToString())].Cells[2].Text);
                //procedemos a almacenar y limiar las listas tal cual estan para evitar perdida de datos durante el load del modal PAra confirmar eliminacion
                fusionarLista_clearPorMateria();

                guardarListaAArchivoXML_CLearAll();

                Session["vamosAEliminar"] = true;

                Response.Write("<script>window.addEventListener('load', function () {$('#mdlConfirmarEliminar').modal('show');});</script>");



            }

        }
        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            eliminarUnidadDeLista();
        }
        public void eliminarUnidadDeLista()
        {
            Unidad unidadAEliminar = new Unidad(
                                       int.Parse(Session["idUnidadEliminar"].ToString()),
                                       int.Parse(Session["idMateriaG"].ToString()),
                                       int.Parse(Session["numeroUnidadEliminar"].ToString()),
                                       int.Parse(Session["calificacionUnidadEliminar"].ToString())
                                                );

            foreach (Unidad u in listaUnidadesPMateria)
            {
                if (u.idUnidad == unidadAEliminar.idUnidad                      &&
                    u.idMateria == unidadAEliminar.idMateria                    &&
                    u.NumeroUnidad == unidadAEliminar.NumeroUnidad              &&
                    u.CalificacionUnidad == unidadAEliminar.CalificacionUnidad   )

                {
                    listaUnidadesPMateria.Remove(u);
                    break;
                }
            }

            fusionarLista_clearPorMateria();

                guardarListaAArchivoXML_CLearAll();

                CrearListaDeTodasLasUnidades(ref listaDeTodasLasUnidades);

                extraerListaPorMateria_MaxIdU_LimpiarLTLM();

                llenarTablaConMateriasPorUnidad();

                txtAgregarEditarNUnidad.Text = "";
                txtAgregarEditarCalificacion.Text = ""; 
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Unidad unidadParaAgregar = new Unidad(
                                     idMayorClaveActual+1,
                                     int.Parse(Session["idMateriaG"].ToString()),
                                     int.Parse(txtAgregarEditarNUnidad.Text.Trim()),
                                     int.Parse(txtAgregarEditarCalificacion.Text.Trim()));
            if (verificarRepetido(unidadParaAgregar)) 
            {
                lblInfoError.Text = "Error, no se puede agregar la unidad " + txtAgregarEditarNUnidad.Text + " porque ya existe";
            }
            else 
            {

                listaUnidadesPMateria.Add(unidadParaAgregar);

                fusionarLista_clearPorMateria();

                guardarListaAArchivoXML_CLearAll();

                CrearListaDeTodasLasUnidades(ref listaDeTodasLasUnidades);

                extraerListaPorMateria_MaxIdU_LimpiarLTLM();

                llenarTablaConMateriasPorUnidad();

                txtAgregarEditarNUnidad.Text = "";
                txtAgregarEditarCalificacion.Text = "";
            }
        }

        public void fusionarLista_clearPorMateria() 
        {
            foreach (Unidad u in listaUnidadesPMateria)
            {
               listaDeTodasLasUnidades.Add(u);
            }
            listaUnidadesPMateria.Clear();
        }
        public void guardarListaAArchivoXML_CLearAll()
        {
            XmlDocument doc = new XmlDocument();
            XmlElement RAIZ = doc.CreateElement("Unidades");
            doc.AppendChild(RAIZ);

            XmlElement UNIDAD;
            XmlElement IDUNIDAD;
            XmlElement IDMATERIA;
            XmlElement NUMEROUNIDAD;
            XmlElement NUMEROCALIFICACION;

            foreach (Unidad u in listaDeTodasLasUnidades)
            {
                UNIDAD = doc.CreateElement("Unidad");
                RAIZ.AppendChild(UNIDAD);

                IDUNIDAD = doc.CreateElement("idU");
                IDUNIDAD.AppendChild(doc.CreateTextNode(u.idUnidad.ToString()));
                UNIDAD.AppendChild(IDUNIDAD);

                IDMATERIA = doc.CreateElement("idM");
                IDMATERIA.AppendChild(doc.CreateTextNode(u.idMateria.ToString()));
                UNIDAD.AppendChild(IDMATERIA);

                NUMEROUNIDAD = doc.CreateElement("numU");
                NUMEROUNIDAD.AppendChild(doc.CreateTextNode(u.NumeroUnidad.ToString()));
                UNIDAD.AppendChild(NUMEROUNIDAD);

                NUMEROCALIFICACION = doc.CreateElement("calU");
                NUMEROCALIFICACION.AppendChild(doc.CreateTextNode(u.CalificacionUnidad.ToString()));
                UNIDAD.AppendChild(NUMEROCALIFICACION);
            }
            listaDeTodasLasUnidades.Clear();
            doc.Save(Server.MapPath("~/AlmacenamientoXML/UnidadesXML.xml"));
        }

        protected void btnRegresar_Click(object sender, EventArgs e)
        {
            Session["idMateriaG"] = null;
            Session["nombreMateriaG"] = null;
            Session["vamosAEliminar"] = null;
            Session["idUnidadEliminar"] = null;
            Session["numeroUnidadEliminar"] = null;
            Session["calificacionUnidadEliminar"] = null;
            Response.Redirect("MateriasForm.aspx");
        }
        public bool verificarRepetido(Unidad unidadAVerificar)
        {
            bool esRepetido = false;
            foreach (Unidad u in listaUnidadesPMateria)
            {
                if (u.NumeroUnidad == unidadAVerificar.NumeroUnidad)
                {
                    esRepetido = true;
                    break;
                }

            }
            return esRepetido;
        }

    }

}
    