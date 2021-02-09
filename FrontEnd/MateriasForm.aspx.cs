using FrontEnd.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Linq;

namespace FrontEnd
{
    public partial class MateriasForm : System.Web.UI.Page
    {
        List<Materia> listaMaterias = new List<Materia>();
        protected void Page_Load(object sender, EventArgs e)
        {
            btnAceptar.Enabled = false;
           
            CrearListaDeMateriasXML(ref listaMaterias);
            llenarTabla();
        }
        public void CrearListaDeMateriasXML(ref List<Materia> listaDeItems)
        {
            //C:/Users/beton/source/repos/ExamenU2_XML/FrontEnd/AlmacenamientoXML/MateriasXML.xml
          
            var doc = XDocument.Load(Server.MapPath("~/AlmacenamientoXML/MateriasXML.xml"));

            listaDeItems = doc.Root
                .Descendants("Materia")
                .Select(node => new Materia
                {
                    IdMateria = int.Parse(node.Element("idM").Value),
                    Nombre = node.Element("Nombre").Value
                })
                .ToList();
            
           // Console.WriteLine(string.Join(",", listaDeItems.Select(x => x.ToString())));
            
        }

        public void llenarTabla()
        {
            listaMaterias.Sort((x, y) => x.Nombre.CompareTo(y.Nombre));
            //Evitar que agregue más columnas a la tabla
            gvMaterias.AutoGenerateColumns = false;
            gvMaterias.DataSource = null;
            gvMaterias.DataSource = listaMaterias;
            gvMaterias.DataBind();
        }
        protected void gvMaterias_RowCommand(object sender, GridViewCommandEventArgs e)
        {
             if (e.CommandName == "Eliminar")
            {
                IdMateriaEliminar.Value = e.CommandArgument.ToString();
                Response.Write("<script>window.addEventListener('load', function () {$('#mdlConfirmarEliminar').modal('show');});</script>");



            }
            else if (e.CommandName == "Gestionar")
            {
                Session["nombreMateriaG"] = listaMaterias[int.Parse(e.CommandArgument.ToString())].Nombre;
                Session["idMateriaG"] = listaMaterias[int.Parse(e.CommandArgument.ToString())].IdMateria;
                Response.Redirect("UnidadesForm.aspx");
            }

        }
        protected void btnConfirmarEliminar_Click(object sender, EventArgs e)
        {
            eliminarMateriaList();

            CrearListaDeMateriasXML(ref listaMaterias);
            llenarTabla();
        }
        public void eliminarMateriaList()
        {
            listaMaterias.RemoveAt(int.Parse(IdMateriaEliminar.Value));
            convert_List_XML_SAVE();
        }
        public void convert_List_XML_SAVE() 
        {
            XmlDocument doc = new XmlDocument();
            XmlElement RAIZ = doc.CreateElement("Materias");
            doc.AppendChild(RAIZ);

            XmlElement MATERIA;
            XmlElement IDMATERIA;
            XmlElement NOMBRE;

            foreach (Materia m in listaMaterias)
            {
                MATERIA = doc.CreateElement("Materia");
                RAIZ.AppendChild(MATERIA);

                IDMATERIA = doc.CreateElement("idM");
                IDMATERIA.AppendChild(doc.CreateTextNode(m.IdMateria.ToString()));
                MATERIA.AppendChild(IDMATERIA);

                NOMBRE = doc.CreateElement("Nombre");
                NOMBRE.AppendChild(doc.CreateTextNode(m.Nombre));
                MATERIA.AppendChild(NOMBRE);

            }
            doc.Save(Server.MapPath("~/AlmacenamientoXML/MateriasXML.xml"));
        }

        
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            Materia materiaNueva = new Materia(
                                    idMateriaMaximo()+1,
                                    txtAgregarEditarMaterias.Text.Trim());
            

            if (verificarRepetido(materiaNueva))
            {
                lblInfoError.Text = "Error, no se puede agregar la unidad " + txtAgregarEditarMaterias.Text + " porque ya existe";
            }
            else
            {
                lblInfoError.Text = "";
                listaMaterias.Add(materiaNueva);
                convert_List_XML_SAVE();
                llenarTabla();
                txtAgregarEditarMaterias.Text = "";
            }
            
        }

        public int idMateriaMaximo()
        {
            if (listaMaterias.Count == 0)
            {
                throw new InvalidOperationException("Lista Vacia");
            }
            int maxID = int.MinValue;
            foreach (Materia m in listaMaterias)
            {
                if (m.IdMateria > maxID)
                {
                    maxID = m.IdMateria;
                }
            }
            return maxID;
        }


        protected void txtAgregarEditarMaterias_OnTextChanged(object sender, EventArgs e)
        {

            btnAceptar.Enabled = true;

        }
        public bool verificarRepetido(Materia materiaAVerificar)
        {
            bool esRepetido = false;
            foreach (Materia m in listaMaterias)
            {
                if (m.Nombre.Equals(materiaAVerificar.Nombre))
                {
                    esRepetido = true;
                    break;
                }

            }
            return esRepetido;
        }

    }
   
}