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
            if (Session["PerfilUsuario"].ToString() == "6" || Session["PerfilUsuario"].ToString() == "1")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Setor/Create
        [HttpPost]
        public ActionResult Create(Setor setor, String obra)
        {
            try
            {
                this.ModelState.Remove("obra");
                if (ModelState.IsValid)
                {
                    if (Session["PerfilUsuario"].ToString() == "6")
                    {
                        ApplicationUser user = new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId());
                        setor.obra = new UsuarioObraDAO(db).buscarObrasDoUsuario(user)[0];
                    }
                    else
                    {
                        setor.obra = new ObraDAO(db).BuscarObraId(Convert.ToInt32(obra));
                    }

                    if (new SetorDAO(db).salvarSetor(setor))
                    {
                        TempData["notice"] = "Setor criado com Sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                        return View();
                    }
                }
                else
                {
                    TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                    return View();
                }


            }
            catch
            {
                TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
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
