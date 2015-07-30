using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.GN;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class SetorController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public SetorController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        // GET: Setor
        public ActionResult Index()
        {         
            int perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
            List<Setor> setores = new SetorGN(db).setoresPorPerfil(perfil, User.Identity.GetUserId());
            if (setores == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(setores);
            }
            
        }

        // GET: Setor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Setor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Setor/Create
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

        // GET: Setor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Setor/Edit/5
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

        // GET: Setor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Setor/Delete/5
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
