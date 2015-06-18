using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using prj_chamadosBRA.Utils;
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
                if (Session["PerfilUsuario"].ToString().Equals("1") 
                    || Session["PerfilUsuario"].ToString().Equals("3")
                    || Session["PerfilUsuario"].ToString().Equals("5"))
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
                        ViewBag.NomeObra = "";
                        return View(new ChamadoDAO(db).BuscarChamados());
                    }
                    else
                    {
                        ViewBag.NomeObra = "- "+obras[0].Descricao;
                        return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras));
                    }

                }
                else
                {
                    ViewBag.NomeObra = "";
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
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
        }

        // GET: Chamado/Create
        [Authorize]
        public ActionResult Create()
        {
            List<Obra> obras = new prj_chamadosBRA.Repositories.ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
            ViewBag.UserId = User.Identity.GetUserId();
            SelectList listObra = new SelectList(obras, "IDO", "Descricao");
            ViewBag.ObraDestino = listObra;
            if (listObra.Count() == 1)
            {
                ViewBag.SetorDestino = obras[0].IDO;
            }
            else
            {
                ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetores(), "Id", "Nome");
            }
            return View();
        }

        public ActionResult RetornaSetoresPorObra(string selectedValue)
        {
            try
            {
                List<Setor> setores = new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(setores, "Id", "Nome"));
                return json;
            }
            catch
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }

        public ActionResult RetornaResponsaveisPorSetor(string selectedValue)
        {
            try
            {
                List<ApplicationUser> responsaveis = new ApplicationUserDAO(db).retornarUsuariosSetor(new SetorDAO().BuscarSetorId(Convert.ToInt32(selectedValue)));
                ActionResult json = Json(new SelectList(responsaveis, "Id", "Nome"));
                return json;
            }
            catch
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }


        // POST: Chamado/Create
        [HttpPost]
        public ActionResult Create(Chamado chamado, HttpPostedFileBase upload, String SetorDestino, String ObraDestino)
        {
            try
            {
                this.ModelState.Remove("SetorDestino");
                this.ModelState.Remove("ObraDestino");
                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                chamado.DataHoraAbertura = DateTime.Now;

                if (user != null)
                {
                    chamado.ResponsavelAberturaChamado = user;
                }
                if (ModelState.IsValid)
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
                    else
                    {
                        if (Session["PerfilUsuario"].ToString() == "3")
                        {
                            obra = oDAO.BuscarObrasPorUsuario(User.Identity.GetUserId())[0];
                            chamado.ObraDestino = obra;
                        }
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
                    EmailService email = new EmailService();
                    email.envioEmailAberturaChamado(chamado);
                    return RedirectToAction("Index");
                }
                else
                {
                    List<Obra> obras = new prj_chamadosBRA.Repositories.ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                    ViewBag.UserId = User.Identity.GetUserId();
                    SelectList listObra = new SelectList(obras, "IDO", "Descricao");
                    ViewBag.ObraDestino = listObra;
                    if (listObra.Count() == 1)
                    {
                        ViewBag.SetorDestino = obras[0].IDO;
                    }
                    else
                    {
                        ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetores(), "Id", "Nome");
                    }
                    ViewBag.UserId = User.Identity.GetUserId();
                    return View();
                }
            }
            catch (Exception e)
            {
                List<Obra> obras = new prj_chamadosBRA.Repositories.ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                ViewBag.UserId = User.Identity.GetUserId();
                SelectList listObra = new SelectList(obras, "IDO", "Descricao");
                ViewBag.ObraDestino = listObra;
                if (listObra.Count() == 1)
                {
                    ViewBag.SetorDestino = obras[0].IDO;
                }
                else
                {
                    ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetores(), "Id", "Nome");
                } 
                ViewBag.UserId = User.Identity.GetUserId();
                return View();
            }
        }

        // GET: Chamado/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            if (chamado.SetorDestino != null)
            {
                ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetoresPorObra(chamado.ObraDestino.IDO), "Id", "Nome", chamado.SetorDestino.Id);
                if (chamado.ResponsavelChamado != null)
                {
                    ViewBag.ddlResponsavelChamado = new SelectList(new prj_chamadosBRA.Repositories.ApplicationUserDAO(db).retornarUsuariosSetor(chamado.SetorDestino), "Id", "Nome", chamado.ResponsavelChamado.Id);
                }
                else
                {
                    ViewBag.ddlResponsavelChamado = new SelectList(new prj_chamadosBRA.Repositories.ApplicationUserDAO(db).retornarUsuariosSetor(chamado.SetorDestino), "Id", "Nome");
                }
            }
            else
            {
                List<SelectListItem> responsavel = new List<SelectListItem>();
                ViewBag.ddlResponsavelChamado = new SelectList(responsavel);
                ViewBag.SetorDestino = new SelectList(new prj_chamadosBRA.Repositories.SetorDAO(db).BuscarSetoresPorObra(chamado.ObraDestino.IDO), "Id", "Nome", "-- Selecione o Setor --");
            }
            return View(chamado);
        }

        public FileContentResult FileDownload(int id)
        {
            //declare byte array to get file content from database and string to store file name
            byte[] fileData;
            string fileName;
            //using LINQ expression to get record from database for given id value
            var record = from p in db.ChamadoAnexo
                         where p.idAnexo == id
                         select p;
            //only one record will be returned from database as expression uses condtion on primary field
            //so get first record from returned values and retrive file content (binary) and filename
            fileData = (byte[])record.First().arquivoAnexo.ToArray();
            fileName = record.First().NomeAnexo.Trim();
            //return file and provide byte file content and file name
            return File(fileData, "text", fileName);
        }

        // POST: Chamado/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Chamado chamado, String SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento)
        {
            try
            {
                ChamadoDAO cDAO = new ChamadoDAO(db);
                SetorDAO sDAO = new SetorDAO(db);
                Chamado chamadoOrigem = cDAO.BuscarChamadoId(id);
                ChamadoHistorico chamadoHistorico;
                chamadoOrigem.ObsevacaoInterna = chamado.ObsevacaoInterna;
                chamadoOrigem.TipoChamado = chamado.TipoChamado;
                //Atualização de Setor
                if (chamadoOrigem.SetorDestino != null && SetorDestino != null)
                {
                    if (chamadoOrigem.SetorDestino.Id != Convert.ToInt32(SetorDestino))
                    {
                        Setor setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                        chamadoOrigem.SetorDestino = setor;
                        chamadoOrigem.ResponsavelChamado = null;
                        cDAO.atualizarChamado(id, chamadoOrigem);
                        chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                        new EmailService().envioEmailDirecionamentoChamado(chamadoHistorico);
                    }
                }
                else if (chamadoOrigem.SetorDestino == null && SetorDestino != null)
                {
                    Setor setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                    chamadoOrigem.SetorDestino = setor;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                    new EmailService().envioEmailDirecionamentoChamado(chamadoHistorico);
                }

                //Atualização de Responsavel pelo Chamado
                if (chamadoOrigem.ResponsavelChamado != null && ddlResponsavelChamado != null)
                {
                    if (chamadoOrigem.ResponsavelChamado.Id != ddlResponsavelChamado)
                    {
                        ApplicationUser user = manager.FindById(ddlResponsavelChamado);
                        chamadoOrigem.ResponsavelChamado = user;
                        cDAO.atualizarChamado(id, chamadoOrigem);
                        chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                        new EmailService().envioEmailDirecionamentoChamado(chamadoHistorico);
                    }
                }
                else if (chamadoOrigem.ResponsavelChamado == null && ddlResponsavelChamado != "")
                {
                    ApplicationUser user = manager.FindById(ddlResponsavelChamado);
                    chamadoOrigem.ResponsavelChamado = user;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                    new EmailService().envioEmailDirecionamentoChamado(chamadoHistorico);
                }

                if (informacoesAcompanhamento == null || informacoesAcompanhamento != "")
                {
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), informacoesAcompanhamento, chamadoOrigem);
                    new EmailService().envioEmailDirecionamentoChamado(chamadoHistorico);
                }

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
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
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

        // GET: Chamado/Encerrar/5
        public ActionResult Encerrar(int id)
        {
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
        }

        // POST: Chamado/Encerrar/5
        [HttpPost]
        public ActionResult Encerrar(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add encerrar logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult RetornaSubClassPorClass(string selectedValue)
        {
            try
            {
                List<ChamadoSubClassificacao> subClassificacoes = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoesPorClassificacao(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(subClassificacoes, "Id", "Nome"));
                return json;
            }
            catch
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }
        

    }
}
