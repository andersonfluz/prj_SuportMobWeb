using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class SubClassificacaoController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public SubClassificacaoController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: SubClassificacao
        public ActionResult Index()
        {
            List<ChamadoSubClassificacao> subclassificacoes = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoes();
            return View(subclassificacoes);
        }

        // GET: SubClassificacao/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SubClassificacao/Create
        public ActionResult Create()
        {
            if (Session["PerfilUsuario"].ToString() == "1")
            {
                ViewBag.Classificacoes = new prj_chamadosBRA.Repositories.ChamadoClassificacaoDAO().BuscarClassificacoes();
            }
            else
            {
                ViewBag.Classificacoes = new prj_chamadosBRA.Repositories.ChamadoClassificacaoDAO().BuscarClassificacoesPorObras(new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId())));
            }            
            return View();
        }

        // POST: SubClassificacao/Create
        [HttpPost]
        public ActionResult Create(ChamadoSubClassificacao ChamadoSubClassificacao, String Classificacao)
        {
            try
            {
                ModelState.Remove("ChamadoClassificacao");
                if (ModelState.IsValid)
                {
                    ChamadoSubClassificacao.ChamadoClassificacao = new ChamadoClassificacaoDAO(db).BuscarClassificacao(Convert.ToInt32(Classificacao));
                    if (new ChamadoSubClassificacaoDAO(db).salvarSubClassificacao(ChamadoSubClassificacao))
                    {
                        TempData["notice"] = "SubClassificação criado com Sucesso!";
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

        // GET: SubClassificacao/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SubClassificacao/Edit/5
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

        // GET: SubClassificacao/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SubClassificacao/Delete/5
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
