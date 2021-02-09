using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FrontEnd.Modelos
{
    public class Materia
    {
        public int IdMateria { get; set; }
        public String Nombre { get; set; }
        public Materia()
        { }
        public Materia(int idmateria, String nombre)
        {
            IdMateria = idmateria;           
            Nombre = nombre;
        }
    }
}