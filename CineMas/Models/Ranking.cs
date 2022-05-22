using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CineMas.Models
{
    public class Ranking
    {
        public int id { get; set; }
        public int idPelicula { get; set; }
        public int valor { get; set; }
        public string nombrePelicula { get; set; }
    }
}