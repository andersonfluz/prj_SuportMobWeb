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
    public class ClassificacaoController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ClassificacaoController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Classificacao
        public ActionResult Index()
        {
            List<ChamadoClassificacao> classificacoes;
            if (Session["PerfilUsuario"].ToString() == "1")
            {
                classificacoes = new ChamadoClassificacaoDAO(db).BuscarClassificacoes();
            }
            else
            {
                List<Obra> obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId()));
                classificacoes = new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorObras(obras);
            }
            return View(classificacoes);
        }

        // GET: Classificacao/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Classificacao/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Classificacao/Create
        [HttpPost]
        public ActionResult Create(ChamadoClassificacao ChamadoClassificacao, String Obra)
        {
            try
            {
                if (Obra.Equals(""))
                {
                    TempData["notice"] = "Selecione a Obra.";
                    return View();
                }
                ModelState.Remove("Obra");
                if (ModelState.IsValid)
                {
                    Obra obra = new ObraDAO(db).BuscarObraId(Convert.ToInt32(Obra));
                    ChamadoClassificacao.Obra = obra;
                    if (new ChamadoClassificacaoDAO(db).salvarClassificacao(ChamadoClassificacao))
                    {
                        TempData["notice"] = "Classificação criado com Sucesso!";
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

        // GET: Classificacao/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Classificacao/Edit/5
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

        // GET: Classificacao/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Classificacao/Delete/5
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
