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
            var subclassificacoes = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoes();
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
            ViewBag.UserId = User.Identity.GetUserId();
            if (Session["PerfilUsuario"].ToString() == "1")
            {
                ViewBag.Obras = new ObraDAO().BuscarObras();
                ViewBag.Classificacoes = new ChamadoClassificacaoDAO().BuscarClassificacoes();
            }
            else
            {
                ViewBag.Obras = new ObraDAO().BuscarObrasPorUsuario(ViewBag.UserId);
                ViewBag.Classificacoes = new ChamadoClassificacaoDAO().BuscarClassificacoesPorSetores(new UsuarioSetorDAO(db).buscarSetoresDoUsuario(manager.FindById(User.Identity.GetUserId())));
            }
            return View();
        }

        // POST: SubClassificacao/Create
        [HttpPost]
        public ActionResult Create(ChamadoSubClassificacao ChamadoSubClassificacao, string Classificacao)
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
            catch (Exception)
            {
                TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                return View();
            }
        }

        // GET: SubClassificacao/Edit/5
        public ActionResult Edit(int id)
        {
            var subClassificacao = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacao(id);
            ViewBag.ddlClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetores(new UsuarioSetorDAO(db).buscarSetoresDoUsuario(new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId()))), "Id", "Descricao", subClassificacao.ChamadoClassificacao.Id);
            return View(subClassificacao);
        }

        // POST: SubClassificacao/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ChamadoSubClassificacao subClassificacao)
        {
            try
            {
                new ChamadoSubClassificacaoDAO(db).atualizarSubClassificacao(id, subClassificacao);
                TempData["notice"] = "SubClassificação Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
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
            catch (Exception)
            {
                return View();
            }
        }

        public ActionResult RetornaClassificacaoPorSetor(string selectedValue)
        {
            try
            {
                var classificacoes = new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(new SetorDAO(db).BuscarSetorId(Convert.ToInt32(selectedValue)));
                ActionResult json = Json(new SelectList(classificacoes, "Id", "Descricao"));
                return json;
            }
            catch (Exception)
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }

        public ActionResult RetornaSetoresPorObra(string selectedValue)
        {
            if (selectedValue != "")
            {
                var setores = new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
                return json;
            }
            else
            {
                var setores = new List<Setor>();
                ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
                return json;
            }
        }
    }
}
