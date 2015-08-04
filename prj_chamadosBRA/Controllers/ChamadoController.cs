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
using PagedList;

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
        public ActionResult Index(int? page, string tipoChamado)
        {
            try
            {
                if (tipoChamado != null)
                {
                    Session["tipoChamado"] = tipoChamado;
                }
                else
                {
                    if (Session["tipoChamado"] != null)
                    {
                        if (tipoChamado != null)
                        {
                            Session["tipoChamado"] = tipoChamado;
                        }
                    }
                    else
                    {
                        Session["tipoChamado"] = "-2";
                    }
                }
                List<SelectListItem> dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem() { Text = "Todos", Value = "-2" },
                                                new SelectListItem() { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem() { Text = "Outros", Value = "2" },
                                                new SelectListItem() { Text = "Indefinido", Value = "-1" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", Session["tipoChamado"].ToString());

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
                        if (obra.Matriz)
                        {
                            isMatriz = true;
                        }
                    }
                    if (isMatriz && Session["PerfilUsuario"].ToString().Equals("1"))
                    {
                        ViewBag.NomeObra = "";
                        int pageSize = 7;
                        int pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamados().ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(Session["tipoChamado"].ToString())).ToPagedList(pageNumber, pageSize));
                        }

                    }
                    else
                    {
                        ViewBag.NomeObra = "- " + obras[0].Descricao;
                        int pageSize = 7;
                        int pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(Session["tipoChamado"].ToString())).ToPagedList(pageNumber, pageSize));
                        }
                    }

                }
                else
                {
                    ViewBag.NomeObra = "";
                    int pageSize = 7;
                    int pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(Session["tipoChamado"].ToString())).ToPagedList(pageNumber, pageSize));
                    }

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
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
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

        public ActionResult RetornaUsuariosPorObra(string selectedValue)
        {
            try
            {
                List<ApplicationUser> usuarios = new ApplicationUserDAO(db).retornarUsuariosObra(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(usuarios, "Id", "Nome"));
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
        public async Task<ActionResult> Create(Chamado chamado, HttpPostedFileBase upload, String SetorDestino, String ObraDestino, String ResponsavelAberturaChamado)
        {
            try
            {
                this.ModelState.Remove("SetorDestino");
                this.ModelState.Remove("ObraDestino");
                this.ModelState.Remove("DataHoraAtendimento");
                this.ModelState.Remove("Classificacao");
                this.ModelState.Remove("SubClassificacao");
                this.ModelState.Remove("Solucao");
                this.ModelState.Remove("ResponsavelAberturaChamado");
                this.ModelState.Remove("TipoChamado");
                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                chamado.DataHoraAbertura = DateTime.Now;
                chamado.StatusChamado = false;
                if (chamado.TipoChamado == null)
                {
                    chamado.TipoChamado = 2;
                }
                if (user != null)
                {
                    chamado.ResponsavelCriacaoChamado = user;
                    if (ResponsavelAberturaChamado != null)
                    {
                        if (ResponsavelAberturaChamado != "")
                        {
                            chamado.ResponsavelAberturaChamado = manager.FindById(ResponsavelAberturaChamado);
                        }
                        else
                        {
                            chamado.ResponsavelAberturaChamado = user;
                        }
                    }
                    else
                    {
                        chamado.ResponsavelAberturaChamado = user;
                    }

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
                        obra = oDAO.BuscarObrasPorUsuario(User.Identity.GetUserId())[0];
                        chamado.ObraDestino = obra;
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
                    await EmailService.envioEmailAberturaChamado(chamado);
                    TempData["notice"] = "Chamado Criado com Sucesso!";
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
                    TempData["notice"] = "Verifique os campos.";
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
                TempData["notice"] = "Verifique os campos.";
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
        public async Task<ActionResult> Edit(int id, Chamado chamado, String SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento)
        {
            try
            {
                ChamadoDAO cDAO = new ChamadoDAO(db);
                SetorDAO sDAO = new SetorDAO(db);
                Chamado chamadoOrigem = cDAO.BuscarChamadoId(id);
                ChamadoHistorico chamadoHistorico;
                chamadoOrigem.ObsevacaoInterna = chamado.ObsevacaoInterna;
                if (chamadoOrigem.TipoChamado != chamado.TipoChamado)
                {
                    chamadoOrigem.TipoChamado = chamado.TipoChamado;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    if (chamado.TipoChamado == 1)
                    {
                        chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Tipo de Chamado foi alterado para Totvs RM", chamadoOrigem);
                    }
                    else if (chamado.TipoChamado == 2)
                    {
                        chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Tipo de Chamado foi alterado para Outros", chamadoOrigem);
                    }
                }

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
                        await EmailService.envioEmailDirecionamentoChamado(chamadoHistorico);
                    }
                }
                else if (chamadoOrigem.SetorDestino == null && SetorDestino != null)
                {
                    Setor setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                    chamadoOrigem.SetorDestino = setor;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                    await EmailService.envioEmailDirecionamentoChamado(chamadoHistorico);
                }

                //Atualização de Responsavel pelo Chamado
                if (chamadoOrigem.ResponsavelChamado != null && ddlResponsavelChamado != null)
                {
                    if (chamadoOrigem.ResponsavelChamado.Id != ddlResponsavelChamado)
                    {
                        if (ddlResponsavelChamado == "-1")
                        {
                            chamadoOrigem.ResponsavelChamado = null;
                            cDAO.atualizarChamado(id, chamadoOrigem);
                        }
                        else
                        {
                            ApplicationUser user = manager.FindById(ddlResponsavelChamado);
                            chamadoOrigem.ResponsavelChamado = user;
                            cDAO.atualizarChamado(id, chamadoOrigem);
                            chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                            await EmailService.envioEmailDirecionamentoChamado(chamadoHistorico);

                        }
                    }
                }
                else if (chamadoOrigem.ResponsavelChamado == null && ddlResponsavelChamado != "")
                {
                    ApplicationUser user = manager.FindById(ddlResponsavelChamado);
                    chamadoOrigem.ResponsavelChamado = user;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                    await EmailService.envioEmailDirecionamentoChamado(chamadoHistorico);
                }

                if (informacoesAcompanhamento == null || informacoesAcompanhamento != "")
                {
                    chamadoHistorico = cDAO.registrarHistorico(DateTime.Now, manager.FindById(User.Identity.GetUserId()), informacoesAcompanhamento, chamadoOrigem);
                    await EmailService.envioEmailDirecionamentoChamado(chamadoHistorico);
                }
                TempData["notice"] = "Chamado Atualizado Com Sucesso!";
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new ChamadoDAO(db).BuscarChamadoId(id));
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
            chamado.StatusChamado = true;
            return View(chamado);
        }

        // POST: Chamado/Encerrar/5
        [HttpPost]
        public async Task<ActionResult> Encerrar(int id, Chamado chamado, String data, String hora)
        {
            try
            {

                Chamado chamadoOriginal = new ChamadoDAO(db).BuscarChamadoId(id);

                DateTime atendimento = Convert.ToDateTime(data + " " + hora);
                chamadoOriginal.DataHoraAtendimento = atendimento;
                chamadoOriginal.Classificacao = chamado.Classificacao;
                chamadoOriginal.SubClassificacao = chamado.SubClassificacao;
                chamadoOriginal.Solucao = chamado.Solucao;
                chamadoOriginal.DataHoraBaixa = DateTime.Now;
                new ChamadoDAO(db).encerrarChamado(id, chamadoOriginal);
                await EmailService.envioEmailEncerramentoChamado(chamadoOriginal);
                TempData["notice"] = "Chamado Encerrado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (System.FormatException fe)
            {
                TempData["notice"] = "Erro no Encerramento. Verifique a data e hora do encerramento.";
                return Redirect("..\\Edit\\" + id);
            }
            catch (DbEntityValidationException ve)
            {
                string erro = "";
                foreach (var eve in ve.EntityValidationErrors)
                {                    
                    foreach (var ev in eve.ValidationErrors)
                    {
                        erro = erro + "O campo: " + ev.PropertyName + " deu erro: " + ev.ErrorMessage;
                    }
                }
                TempData["notice"] = "Erro no Encerramento: " + erro;
                return Redirect("..\\Edit\\" + id);
            }
            catch (Exception e)
            {
                //string js = @"<script>alert('Erro no Encerramento do Chamado, Por favor Verifique os campos');</script>";
                //return JavaScript(js);
                TempData["notice"] = "Erro no Encerramento do Chamado, Por favor Verifique os campos.";
                return Redirect("..\\Edit\\" + id);
            }
        }

        public ActionResult RetornaSubClassPorClass(string selectedValue)
        {
            try
            {
                List<ChamadoSubClassificacao> subClassificacoes = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoesPorClassificacao(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(subClassificacoes, "Id", "Descricao"));
                return json;
            }
            catch
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }


    }
}
