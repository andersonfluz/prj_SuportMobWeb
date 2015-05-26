using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class ChamadoController : Controller
    {
        // GET: Chamado
        public ActionResult Index()
        {
            return View(new ChamadoDAO().BuscarChamados());
        }

        // GET: Chamado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Chamado/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chamado/Create
        [HttpPost]
        public ActionResult Create(Chamado chamado)
        {
            try
            {
                ChamadoDAO cDAO = new ChamadoDAO();
                SetorDAO sDAO = new SetorDAO();
                var claimsIdentity = User.Identity as ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    // the principal identity is a claims identity.
                    // now we need to find the NameIdentifier claim
                    var userIdClaim = claimsIdentity.Claims
                        .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                    if (userIdClaim != null)
                    {
                        var userIdValue = userIdClaim.Value;
                        chamado.ResponsavelChamado = userIdValue;
                        chamado.DataHoraAbertura = DateTime.Now;

                    }
                }
                cDAO.salvarChamado(chamado);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Chamado/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Chamado/Edit/5
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

        // GET: Chamado/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Chamado/Delete/5
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
