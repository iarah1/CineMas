using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineMas.Models
{
    public class Pelicula
    {
        public int id { get; set; }
        public string nombre { get; set; }
        public string sinopsis { get; set; }
        public string director { get; set; }
        public string genero { get; set; }
        public int categoriaId { get; set; }
        public string categoria { get; set; }
        public string imgUrl { get; set; }
        public int calificacion { get; set; }
        public int vistas { get; set; }

    }

    public class Categoria
    {
        public int id { get; set; }
        public string nombre { get; set; }

    }


}