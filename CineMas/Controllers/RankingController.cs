using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CineMas.Models;

namespace CineMas.Controllers
{
    public class RankingController : Controller
    {
        
        // GET: Raking
        public ActionResult ShowRankings()
        {
            ViewBag.rankings = new ICineBD().GetRankings();
            ViewBag.pelicula = new ICineBD().GetPeliculaMasVista();
            return View();
        }

        // GET: Raking/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Raking/Create
        public ActionResult CreateRanking(int idPelicula)
        {
            ViewBag.peliculasById = new ICineBD().GetPeliculaById(idPelicula);
            return View();
        }

        // POST: Raking/Create
        [HttpPost]
        public ActionResult CreateRanking(FormCollection form)
        {
            Models.ICineBD ICine = new ICineBD();

            Ranking ranking = new Ranking();
            ranking.idPelicula = Convert.ToInt32(form["idPelicula"]);
            ranking.valor = Convert.ToInt32(form["rate"]);

            ICine.SaveRanking(ranking);
           
            return RedirectToAction("ShowPeliculas","Pelicula");
        }
    }
}
