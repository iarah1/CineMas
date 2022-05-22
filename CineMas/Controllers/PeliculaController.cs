using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CineMas.Models;

namespace CineMas.Controllers
{
    public class PeliculaController : Controller
    {
        //instacia de ICineBD
        
        
        // GET: Pelicula
        public ActionResult ShowPeliculas()
        {
            Session["CurrentPage"] = 1;

            ViewBag.categorias = new ICineBD().GetCategorias();

            return View();
        }

        [HttpGet]
        public ActionResult GetPeliculas(int pageId, int categoriaId, string search)
        {
            
            if (string.IsNullOrEmpty(search)) search = "";

            Session["CurrentPage"] = pageId;

            List<Pelicula> peliculaResult = new List<Pelicula>();
            List<Pelicula> peliculaList = new ICineBD().GetPeliculas(pageId, categoriaId, search);

            if (peliculaList.Count == 0)
            {
                pageId = pageId - 1;
                Session["CurrentPage"] = pageId;
                peliculaResult = new ICineBD().GetPeliculas(pageId, categoriaId, search);
            }
            else { peliculaResult = peliculaList; }

            //obtenemos listado de peliculas y se envia a la vista
            ViewBag.peliculas = peliculaResult;

            return PartialView();
        }
    }
}