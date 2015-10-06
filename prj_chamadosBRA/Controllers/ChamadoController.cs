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
using prj_chamadosBRA.GN;
using System.Net;
using System.IO;

namespace prj_chamadosBRA.Controllers
{
    [Authorize]
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
        public ActionResult Index(int? page, string tipoChamado, string filtro)
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
                                                new SelectListItem() { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", Session["tipoChamado"].ToString());

                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                //Usuario Administrador
                if (Session["PerfilUsuario"].ToString().Equals("1")
                    || Session["PerfilUsuario"].ToString().Equals("3")
                    || Session["PerfilUsuario"].ToString().Equals("5")
                    || Session["PerfilUsuario"].ToString().Equals("6"))
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
                            return View(new ChamadoDAO(db).BuscarChamados(filtro, false).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, false).ToPagedList(pageNumber, pageSize));
                        }

                    }
                    else
                    {

                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        int pageSize = 7;
                        int pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, false).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, false).ToPagedList(pageNumber, pageSize));
                        }
                    }

                }
                else
                {
                    List<Obra> obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                    }
                    int pageSize = 7;
                    int pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, false).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, false).ToPagedList(pageNumber, pageSize));
                    }

                }
            }
            catch (NullReferenceException ne)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult Acompanhamento(int? page, string tipoChamado, string filtro, bool? encerrado)
        {
            try
            {
                bool chamadosEncerrados;
                if (encerrado == true)
                {
                    Session["encerrado"] = chamadosEncerrados = true;
                }
                else
                {
                    Session["encerrado"] = chamadosEncerrados = false;
                }
                //Verificação de tipo de chamado
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
                //Criação da lista de tipos de chamados
                List<SelectListItem> dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem() { Text = "Todos", Value = "-2" },
                                                new SelectListItem() { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem() { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", Session["tipoChamado"].ToString());

                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                //Usuario para Gestao

                //Usuario Vinculado a Obras
                List<Obra> obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
                bool isMatriz = false;
                foreach (var obra in obras)
                {
                    if (obra.Matriz)
                    {
                        isMatriz = true;
                    }
                }
                int pageSize;
                int pageNumber;
                if (isMatriz && Session["PerfilUsuario"].ToString().Equals("1"))
                {
                    ViewBag.NomeObra = "";
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamados(filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }

                }
                else if (Session["PerfilUsuario"].ToString().Equals("2") || Session["PerfilUsuario"].ToString().Equals("3") || Session["PerfilUsuario"].ToString().Equals("4"))
                {
                    List<Obra> obra = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                    }                    
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                }
                else if (Session["PerfilUsuario"].ToString().Equals("5"))
                {
                    ViewBag.NomeObra = "- " + obras[0].Descricao;
                    List<Setor> setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeSetores(setores, filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setores, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    ViewBag.NomeObra = "";
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, chamadosEncerrados).ToPagedList(pageNumber, pageSize));
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

        // GET: Chamado/Encerrado/5
        public ActionResult Encerrado(int id)
        {
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            ViewBag.Classificacao = new ChamadoClassificacaoDAO().BuscarClassificacao(chamado.Classificacao.Value).Descricao.ToString();
            ViewBag.SubClassificacao = new ChamadoSubClassificacaoDAO().BuscarSubClassificacao(chamado.SubClassificacao.Value).Descricao.ToString();

            bool isSetor = false;
            bool isObra = false;
            List<Setor> setoresUsuario = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(manager.FindById(User.Identity.GetUserId()));
            List<Obra> obrasUsuario = new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId()));
            if (setoresUsuario.Contains(chamado.SetorDestino))
            {
                isSetor = true;
            }
            if (obrasUsuario.Contains(chamado.ObraDestino))
            {
                isObra = true;
            }
            if (Session["PerfilUsuario"].ToString().Equals("1") || (Session["PerfilUsuario"].ToString().Equals("6") && isObra))
            {
                ViewBag.VisualizarObsInterna = true;
            }
            else if ((Session["PerfilUsuario"].ToString().Equals("3") || Session["PerfilUsuario"].ToString().Equals("5")) && isSetor)
            {
                ViewBag.VisualizarObsInterna = true;
            }
            else
            {
                ViewBag.VisualizarObsInterna = false;
            }
            return View(chamado);
        }

        // GET: Chamado/Details/5
        public ActionResult ChamadoInfo(int id)
        {
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
        }

        // POST: Chamado/Edit/5
        [HttpPost]
        public async Task<ActionResult> ChamadoInfo(int id, string informacoesAcompanhamento)
        {
            try
            {
                ChamadoGN cGN = new ChamadoGN(db);
                if (await cGN.atualizarChamadoHistorico(id, informacoesAcompanhamento, manager.FindById(User.Identity.GetUserId())))
                {
                    TempData["notice"] = "Chamado Atualizado Com Sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                    return RedirectToAction("Edit", id);
                }
            }
            catch (Exception e)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Edit", id);
            }
        }

        // GET: Chamado/Create
        [Authorize]
        public ActionResult Create()
        {
            List<Obra> obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
            ViewBag.UserId = User.Identity.GetUserId();
            SelectList listObra = new SelectList(obras, "IDO", "Descricao");
            ViewBag.ObraDestino = listObra;
            if (listObra.Count() == 1)
            {
                ViewBag.ObraUsuario = obras[0].IDO;
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
                return Json(new SelectList(new ChamadoGN(db).RetornarSetoresPorObra(selectedValue), "Id", "Nome"));
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
                List<ApplicationUser> usuarios = new ApplicationUserDAO(db).retornarUsuariosObra(Convert.ToInt32(selectedValue), null);
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
                List<ApplicationUser> responsaveis = new ApplicationUserDAO(db).retornarUsuariosSetor(new SetorDAO().BuscarSetorId(Convert.ToInt32(selectedValue)), null);
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
        public async Task<ActionResult> Create(NovoChamadoViewModel chamadoVM, HttpPostedFileBase upload, String SetorDestino, String ObraDestino, String ResponsavelAberturaChamado)
        {
            try
            {
                this.ModelState.Remove("SetorDestino");
                this.ModelState.Remove("ObraDestino");
                this.ModelState.Remove("TipoChamado");
                //this.ModelState.Remove("DataHoraAtendimento");
                //this.ModelState.Remove("Classificacao");
                //this.ModelState.Remove("SubClassificacao");
                //this.ModelState.Remove("Solucao");
                //this.ModelState.Remove("ResponsavelAberturaChamado");                
                if (ModelState.IsValid)
                {
                    ChamadoGN cGN = new ChamadoGN(db);
                    Chamado chamado = new Chamado()
                    {
                        Assunto = chamadoVM.Assunto,
                        Descricao = chamadoVM.Descricao,
                        Observacao = chamadoVM.Observacao,
                        TipoChamado = chamadoVM.TipoChamado
                    };

                    if (await cGN.registrarChamado(chamado, upload, SetorDestino, ObraDestino, ResponsavelAberturaChamado, manager.FindById(User.Identity.GetUserId())))
                    {
                        TempData["notice"] = "Chamado Criado com Sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        List<Obra> obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                        ViewBag.UserId = User.Identity.GetUserId();
                        SelectList listObra = new SelectList(obras, "IDO", "Descricao");
                        ViewBag.ObraDestino = listObra;
                        if (listObra.Count() == 1)
                        {
                            ViewBag.SetorDestino = obras[0].IDO;
                        }
                        else
                        {
                            ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Nome");
                        }
                        TempData["notice"] = "Desculpe, estamos com problemas ao registrar o chamado.";
                        ViewBag.UserId = User.Identity.GetUserId();
                        return View();
                    }
                }
                else
                {
                    List<Obra> obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                    ViewBag.UserId = User.Identity.GetUserId();
                    SelectList listObra = new SelectList(obras, "IDO", "Descricao");
                    ViewBag.ObraDestino = listObra;
                    if (listObra.Count() == 1)
                    {
                        ViewBag.ObraUsuario = obras[0].IDO;
                        ViewBag.SetorDestino = obras[0].IDO;
                    }
                    else
                    {
                        ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Nome");
                    }
                    TempData["notice"] = "Verifique os campos.";
                    ViewBag.UserId = User.Identity.GetUserId();
                    return View();
                }
            }
            catch (Exception e)
            {
                List<Obra> obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                ViewBag.UserId = User.Identity.GetUserId();
                SelectList listObra = new SelectList(obras, "IDO", "Descricao");
                ViewBag.ObraDestino = listObra;
                if (listObra.Count() == 1)
                {
                    ViewBag.SetorDestino = obras[0].IDO;
                }
                else
                {
                    ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Nome");
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
                ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetoresPorObra(chamado.ObraDestino.IDO), "Id", "Nome", chamado.SetorDestino.Id);
                if (chamado.ResponsavelChamado != null)
                {
                    ViewBag.ddlResponsavelChamado = new SelectList(new ApplicationUserDAO(db).retornarUsuariosSetor(chamado.SetorDestino, null), "Id", "Nome", chamado.ResponsavelChamado.Id);
                }
                else
                {
                    ViewBag.ddlResponsavelChamado = new SelectList(new ApplicationUserDAO(db).retornarUsuariosSetor(chamado.SetorDestino, null), "Id", "Nome");
                }
            }
            else
            {
                List<SelectListItem> responsavel = new List<SelectListItem>();
                ViewBag.ddlResponsavelChamado = new SelectList(responsavel);
                ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetoresPorObra(chamado.ObraDestino.IDO), "Id", "Nome", "-- Selecione o Setor --");
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

        public async Task<ActionResult> AdicionarAnexo(string id)
        {
            try
            {
                ChamadoGN cGN = new ChamadoGN(db);
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {
                        cGN.registarAnexo(Convert.ToInt32(id), fileContent);
                        await cGN.atualizarChamadoHistorico(Convert.ToInt32(id), "Arquivo Inserido no Chamado N. " + id, manager.FindById(User.Identity.GetUserId()));
                    }
                }
            }
            catch (Exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Problemas no carregamento do aquivo.");
            }
            return Json("Arquivo Carregado com sucesso!");
        }


        // POST: Chamado/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, Chamado chamado, string SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento)
        {
            try
            {
                if (ddlResponsavelChamado != "")
                {
                    ChamadoGN cGN = new ChamadoGN(db);
                    if (await cGN.atualizarChamado(id, chamado, SetorDestino, ddlResponsavelChamado, informacoesAcompanhamento, manager.FindById(User.Identity.GetUserId())))
                    {
                        TempData["notice"] = "Chamado Atualizado Com Sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                        return RedirectToAction("Edit", id);
                    }
                }
                else
                {
                    TempData["notice"] = "Por favor informe o Técnico Responsavel pelo Atendimento.";
                    return RedirectToAction("Edit", id);
                }
            }
            catch (Exception e)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Edit", id);
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
            var model = new EncerramentoChamadoViewModel()
            {
                Id = chamado.Id,
                StatusChamado = chamado.StatusChamado,
                Solucao = chamado.Solucao,
                DataHoraAtendimento = chamado.DataHoraAtendimento,
                Classificacao = chamado.Classificacao,
                SubClassificacao = chamado.SubClassificacao,
                ResponsavelChamado = chamado.ResponsavelChamado,
                ObraDestino = chamado.ObraDestino
            };
            return View(model);
        }

        // POST: Chamado/Encerrar/5
        [HttpPost]
        public async Task<ActionResult> Encerrar(int id, EncerramentoChamadoViewModel chamado, String data, String hora)
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
                chamadoOriginal.ResponsavelChamado = manager.FindById(User.Identity.GetUserId());
                new ChamadoDAO(db).encerrarChamado(id, chamadoOriginal);
                await EmailServiceUtil.envioEmailEncerramentoChamado(chamadoOriginal);
                new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao()
                {
                    IdChamado = chamado.Id,
                    ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(2),
                    Texto = "Chamado Encerrado",
                    DataAcao = DateTime.Now,
                    UsuarioAcao = manager.FindById(User.Identity.GetUserId())

                });
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

        // GET: Chamado
        [Authorize]
        public ActionResult ChamadosEncerrados(int? page, string tipoChamado, string filtro)
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
                                                new SelectListItem() { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", Session["tipoChamado"].ToString());

                ApplicationUser user = manager.FindById(User.Identity.GetUserId());
                //Usuario Administrador
                if (Session["PerfilUsuario"].ToString().Equals("1")
                    || Session["PerfilUsuario"].ToString().Equals("3")
                    || Session["PerfilUsuario"].ToString().Equals("5")
                    || Session["PerfilUsuario"].ToString().Equals("6"))
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
                            return View(new ChamadoDAO(db).BuscarChamados(filtro, true).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true).ToPagedList(pageNumber, pageSize));
                        }

                    }
                    else
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        int pageSize = 7;
                        int pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, true).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true).ToPagedList(pageNumber, pageSize));
                        }
                    }

                }
                else
                {
                    List<Obra> obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                    }
                    int pageSize = 7;
                    int pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, true).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true).ToPagedList(pageNumber, pageSize));
                    }

                }
            }
            catch (NullReferenceException ne)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult ReaberturaChamado(string id)
        {
            Criptografia objCript = new Criptografia(id);
            int idChamado = Convert.ToInt32(objCript["id"].ToString());
            Chamado chamado = new ChamadoDAO(db).BuscarChamadoId(idChamado);

            if (chamado.StatusChamado.Value)
            {
                TimeSpan? date = DateTime.Now - chamado.DataHoraBaixa;
                int dias = date.Value.Days;
                if (dias <= 7)
                {
                    ReaberturaChamadoViewModel model = new ReaberturaChamadoViewModel()
                    {
                        Id = chamado.Id,
                        StatusChamado = chamado.StatusChamado,
                        ResponsavelChamado = chamado.ResponsavelChamado,
                        DataHoraAtendimento = chamado.DataHoraAtendimento,
                        Assunto = chamado.Assunto,
                        Solucao = chamado.Solucao
                    };
                    return View(model);
                }
                else
                {
                    TempData["notice"] = "Chamado não pode ser reaberto, pois o período de reabertura expirou.";
                    return RedirectToAction("Index", "Chamado");
                }
            }
            else
            {
                TempData["notice"] = "Chamado não pode ser reaberto, pois ele não foi encerrado.";
                return RedirectToAction("Index", "Chamado");
            }
        }

        // POST: Chamado
        [HttpPost]
        public async Task<ActionResult> ReaberturaChamado(string id, ReaberturaChamadoViewModel chamado)
        {
            ChamadoGN cGN = new ChamadoGN(db);
            Criptografia objCript = new Criptografia(id);
            int idChamado = Convert.ToInt32(objCript["id"].ToString());
            await cGN.reaberturaChamado(idChamado, chamado.JustificativaReabertura, manager.FindById(User.Identity.GetUserId()));

            TempData["notice"] = "Chamado Reaberto com Sucesso!";
            return RedirectToAction("Index");
        }
    }
}
