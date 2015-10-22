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
    public class ObraController : Controller
    {
         private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ObraController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }
        // GET: Obra
        [Authorize]
        public ActionResult Index()
        {
            var perfil = Convert.ToInt32(Session["PerfilUsuario"].ToString());
            var obras = new ObraGN(db).obrasPorPerfil(perfil);
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
        public ActionResult Create(Obra obra, String CentroAdministrativo)
        {
            try
            {
                this.ModelState.Remove("CentroAdministrativo");
                if (ModelState.IsValid)
                {
                    if (obra.Matriz)
                    {
                        var centroAdm = new CentroAdministrativo
                        {
                            Nome = obra.Descricao
                        };
                        if (new CentroAdministrativoDAO(db).salvarCentroAdministrativo(centroAdm))
                        {
                            obra.CentroAdministrativo = centroAdm;
                        }
                    }
                    else
                    {
                        var centroAdm = new CentroAdministrativoDAO(db).BuscarCentroAdministrativo(Convert.ToInt32(CentroAdministrativo));
                        obra.CentroAdministrativo = centroAdm;                        
                    }
                    
                    if (new ObraDAO(db).salvarObra(obra))
                    {
                        TempData["notice"] = "Obra criada com Sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    TempData["notice"] = "Algo errado Aconteceu, tente novamente.";
                    return RedirectToAction("Index");
                }

            }
            catch (Exception)
            {
                return View();
            }
        }

        // GET: Obra/Edit/5
        public ActionResult Edit(int id)
        {
            var obra = new ObraDAO(db).BuscarObraId(id);
            ViewBag.ddlCentroAdministrativo = new SelectList(new CentroAdministrativoDAO(db).BuscarCentrosAdministrativos(), "IdCA", "Nome", obra.CentroAdministrativo.idCA);
            return View(obra);
        }

        // POST: Obra/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Obra obra, string ddlCentroAdministrativo)
        {
            try
            {
                obra.CentroAdministrativo = new CentroAdministrativoDAO(db).BuscarCentroAdministrativo(Convert.ToInt32(ddlCentroAdministrativo));
                new ObraDAO(db).atualizarObra(id, obra);
                TempData["notice"] = "Obra Atualizada Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (Exception)
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
            catch (Exception)
            {
                return View();
            }
        }
    }
}
