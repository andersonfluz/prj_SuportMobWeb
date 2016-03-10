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
    [Authorize]
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
            var perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
            var setores = new SetorGN(db).setoresPorPerfil(perfil, User.Identity.GetUserId());
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
                ViewBag.UserId = User.Identity.GetUserId();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Setor/Create
        [HttpPost]
        public ActionResult Create(Setor setor, string obra)
        {
            try
            {
                this.ModelState.Remove("obra");
                if (ModelState.IsValid)
                {
                    setor.obra = new ObraDAO(db).BuscarObraId(Convert.ToInt32(obra));
                    setor.DataCriacao = DateTime.Now;
                    setor.Usuario = User.Identity.GetUserId();
                    
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
            catch (Exception)
            {
                TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                return View();
            }
        }

        // GET: Setor/Edit/5
        public ActionResult Edit(int id)
        {
            var setor = new SetorDAO(db).BuscarSetorId(id);
            return View(setor);
        }

        // POST: Setor/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Setor setor)
        {
            try
            {
                setor.DataAlteracao = DateTime.Now;
                setor.Usuario = User.Identity.GetUserId();
                new SetorDAO(db).atualizarSetor(id, setor);
                TempData["notice"] = "Setor Atualizado Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                return View();
            }
        }

        // GET: Setor/Delete/5
        public ActionResult Delete(int id)
        {
            var setor = new SetorDAO(db).BuscarSetorId(id);
            return View();
        }

        // POST: Setor/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Setor setor)
        {
            try
            {
                setor.DataAlteracao = DateTime.Now;
                setor.Usuario = User.Identity.GetUserId();
                setor.Ativo = false;
                new SetorDAO(db).atualizarSetor(id, setor);
                TempData["notice"] = "Setor Eliminado Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View();
            }
        }
    }
}
