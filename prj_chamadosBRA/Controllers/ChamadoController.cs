using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.Controllers
{
    public class ChamadoController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public ChamadoController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        // GET: Chamado
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                //Usuario Administrador
                if (Session["PerfilUsuario"].ToString().Equals("1"))
                {
                    //Usuario Vinculado a Obras
                    List<Obra> obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
                    Boolean isMatriz = false;
                    foreach (var obra in obras)
                    {
                        if (obra.Matriz.Value)
                        {
                            isMatriz = true;
                        }
                    }
                    if (isMatriz)
                    {
                        return View(new ChamadoDAO(db).BuscarChamados());
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras));
                    }

                }
                else
                {

                    return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user));
                }
            }
            catch (NullReferenceException ne)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Chamado/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetores(), "Id", "Nome");
            ViewBag.ObraDestino = new SelectList(new prj_chamadosBRA.Repositories.ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId()), "IDO", "Descricao");
            return View();
        }

        // POST: Chamado/Create
        [HttpPost]
        public ActionResult Create(Chamado chamado, HttpPostedFileBase upload, String SetorDestino, String ObraDestino)
        {
            try
            {
                ChamadoDAO cDAO = new ChamadoDAO(db);
                ObraDAO oDAO = new ObraDAO(db);
                SetorDAO sDAO = new SetorDAO(db);
                Setor setor;
                Obra obra;

                if (SetorDestino != null)
                {
                    setor = sDAO.BuscarSetorId(Int32.Parse(SetorDestino));
                    chamado.SetorDestino = setor;
                }

                if (ObraDestino != null)
                {
                    obra = oDAO.BuscarObraId(Int32.Parse(ObraDestino));
                    chamado.ObraDestino = obra;
                }

                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                chamado.DataHoraAbertura = DateTime.Now;

                if (user != null)
                {
                    chamado.ResponsavelAberturaChamado = user;
                }

                if (upload != null && upload.ContentLength > 0)
                {
                    ChamadoAnexoDAO caDAO = new ChamadoAnexoDAO(db);
                    var anexo = new ChamadoAnexo
                    {
                        NomeAnexo = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        anexo.arquivoAnexo = reader.ReadBytes(upload.ContentLength);
                    }
                    chamado.Anexos = new List<ChamadoAnexo> { anexo };
                }
                cDAO.salvarChamado(chamado);

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetores(), "Id", "Nome");
                ViewBag.ObraDestino = new SelectList(new prj_chamadosBRA.Repositories.ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId()), "IDO", "Descricao");
                return View();
            }
        }

        // GET: Chamado/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
        }

        // POST: Chamado/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Chamado/Delete/5
        [Authorize]
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
