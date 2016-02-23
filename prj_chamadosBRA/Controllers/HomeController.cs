using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                if (Session["PerfilUsuario"].ToString() == "1" || Session["PerfilUsuario"].ToString() == "3" || Session["PerfilUsuario"].ToString() == "5" || Session["PerfilUsuario"].ToString() == "6")
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Acompanhamento", "Chamado");
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}