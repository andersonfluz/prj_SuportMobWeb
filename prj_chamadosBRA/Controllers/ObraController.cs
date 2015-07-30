using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.GN;
using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class ObraController : Controller
    {
         private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ObraController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Obra
        public ActionResult Index()
        {
            int perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
            List<Obra> obras = new ObraGN(db).obrasPorPerfil(perfil, User.Identity.GetUserId());
            if (obras == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(obras);
            }
        }

        // GET: Obra/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Obra/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Obra/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Obra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Obra/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Obra/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Obra/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
