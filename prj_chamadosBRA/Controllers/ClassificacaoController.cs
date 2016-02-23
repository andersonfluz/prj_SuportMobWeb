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
            else if (Session["PerfilUsuario"].ToString() == "6")
            {
                var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId()));
                classificacoes = new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorObras(obras);
            }
            else if (Session["PerfilUsuario"].ToString() == "5")
            {
                var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(manager.FindById(User.Identity.GetUserId()));
                classificacoes = new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetores(setores);
            }
            else
            {
                return RedirectToAction("Acompanhamento", "Chamado");
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
            var user = new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId());

            if (Session["PerfilUsuario"].ToString().Equals("1"))
            {
                //montagem do dropdownlist de obras
                var obras = new SelectList(new ObraDAO(db).BuscarObras(), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;

                //montagem do dropdownlist de setores
                var setores = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;

                //montagem do dropdownlist de setores corporativos
                var setoresCorp = new SelectList(new UsuarioSetorDAO(db).buscarSetoresCorporativosPrincipaisVinculadosAoUsuario(user), "Id", "Descricao");
                ViewBag.SetoresDisponiveisCorp = setoresCorp;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                //montagem do dropdownlist de obras
                var obras = new SelectList(new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId()), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                //montagem do dropdownlist de setores
                var setores = new SelectList(new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;
            }
            return View();
        }

        // POST: Classificacao/Create
        [HttpPost]
        public ActionResult Create(ChamadoClassificacao ChamadoClassificacao, String SetoresDisponiveis, String SetoresDisponiveisCorp)
        {
            try
            {
                if (SetoresDisponiveis.Equals("") && SetoresDisponiveisCorp.Equals(""))
                {
                    TempData["notice"] = "Selecione o Setor.";
                    return View();
                }
                ModelState.Remove("Setor");
                if (ModelState.IsValid)
                {

                    var setor = new SetorDAO(db).BuscarSetorId(Convert.ToInt32(SetoresDisponiveis + SetoresDisponiveisCorp));
                    ChamadoClassificacao.Setor = setor;
                    if (new ChamadoClassificacaoDAO(db).salvarClassificacao(ChamadoClassificacao, User.Identity.GetUserId()))
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

        // GET: Classificacao/Edit/5
        public ActionResult Edit(int id)
        {
            var classificacao = new ChamadoClassificacaoDAO(db).BuscarClassificacao(id);
            var user = new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId());
            if (Session["PerfilUsuario"].ToString().Equals("1"))
            {
                //montagem do dropdownlist de obras
                var obras = new SelectList(new ObraDAO(db).BuscarObras(), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;

                //montagem do dropdownlist de setores
                var setores = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;

                //montagem do dropdownlist de setores corporativos
                var setoresCorp = new SelectList(new UsuarioSetorDAO(db).buscarSetoresCorporativosDoUsuario(user), "Id", "Descricao");
                ViewBag.SetoresCorporativos = setoresCorp;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("6"))
            {
                //montagem do dropdownlist de obras
                var obras = new SelectList(new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId()), "IDO", "Descricao");
                ViewBag.ObrasDisponiveis = obras;
            }
            else if (Session["PerfilUsuario"].ToString().Equals("5"))
            {
                //montagem do dropdownlist de setores
                var setores = new SelectList(new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user), "Id", "Descricao");
                ViewBag.SetoresDisponiveis = setores;
            }
            return View(classificacao);
        }

        // POST: Classificacao/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ChamadoClassificacao classificacao, String SetoresDisponiveis, String SetoresDisponiveisCorp)
        {
            try
            {
                classificacao.Setor = new SetorDAO(db).BuscarSetorId(Convert.ToInt32(SetoresDisponiveis+SetoresDisponiveisCorp));
                new ChamadoClassificacaoDAO(db).atualizarClassificacao(id, classificacao, User.Identity.GetUserId());
                TempData["notice"] = "Classificação Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                TempData["notice"] = "Problemas ao atualizar a classificação!";
                return RedirectToAction("Edit", new { id = id });
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
