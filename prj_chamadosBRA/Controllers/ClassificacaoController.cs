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
    [Authorize]
    public class ClassificacaoController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
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
                var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId()));
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
            ViewBag.UserId = User.Identity.GetUserId();
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
                    var obra = new ObraDAO(db).BuscarObraId(Convert.ToInt32(Obra));
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
            catch (Exception)
            {
                TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                return View();
            }
        }

        // GET: Classificacao/Edit/5
        public ActionResult Edit(int id)
        {
            var classificacao = new ChamadoClassificacaoDAO(db).BuscarClassificacao(id);
            return View(classificacao);
        }

        // POST: Classificacao/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ChamadoClassificacao classificacao)
        {
            try
            {
                new ChamadoClassificacaoDAO(db).atualizarClassificacao(id, classificacao);
                TempData["notice"] = "Classificação Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
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
            catch (Exception)
            {
                return View();
            }
        }
    }
}
