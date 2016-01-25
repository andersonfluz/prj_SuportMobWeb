using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using prj_chamadosBRA.Utils;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using prj_chamadosBRA.GN;
using System.Net;

namespace prj_chamadosBRA.Controllers
{
    [Authorize]
    public class ChamadoController : AsyncController
    {
        private UserManager<ApplicationUser> manager;
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        public ChamadoController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }


        // GET: Chamado
        [Authorize]
        public ActionResult Index(int? page, string tipoChamado, string filtro, string sortOrder)
        {
            try
            {
                if (tipoChamado == null)
                {
                    tipoChamado = "-2";
                }
                ViewBag.CurrentTipoChamado = tipoChamado;
                ViewBag.CurrentFiltro = filtro;
                ViewBag.CurrentSort = sortOrder;
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem { Text = "Todos", Value = "-2" },
                                                new SelectListItem { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", tipoChamado);

                var user = manager.FindById(User.Identity.GetUserId());
                //Usuario com permissão de Gestão
                if (Session["PerfilUsuario"].ToString().Equals("1")
                    || Session["PerfilUsuario"].ToString().Equals("3")
                    || Session["PerfilUsuario"].ToString().Equals("5")
                    || Session["PerfilUsuario"].ToString().Equals("6")
                    || Session["PerfilUsuario"].ToString().Equals("7"))
                {
                    //Usuario Vinculado a Obras
                    var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    var setores = new UsuarioSetorDAO(db).buscarSetoresCorporativosDoUsuario(user);                    
                    if(setores.Count > 0)
                    {                        
                        obras = new List<Obra>();
                        foreach(var setor in setores)
                        {
                            obras.AddRange(obras = new ObraDAO(db).BuscarObrasSetoresCorporativos(setor));
                        }
                    }
                    var isMatriz = false;
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
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamados(filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(tipoChamado), filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }

                    }
                    else if (isMatriz && Session["PerfilUsuario"].ToString().Equals("7"))
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTecnicoRM(obras, filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTecnicoRMTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }
                    else if (isMatriz && (Session["PerfilUsuario"].ToString().Equals("3") || Session["PerfilUsuario"].ToString().Equals("5")))
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        var setoresUsuario = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeSetores(setoresUsuario, filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setoresUsuario, Convert.ToInt32(tipoChamado), filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }
                    else
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }

                }
                else
                {
                    var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                    }
                    var pageSize = 7;
                    var pageNumber = (page ?? 1);
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, false, sortOrder).ToPagedList(pageNumber, pageSize));
                    }

                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult Acompanhamento(int? page, string tipoChamado, string filtro, bool? encerrado, string sortOrder)
        {
            try
            {
                bool chamadosEncerrados;
                if (encerrado == true)
                {
                    ViewBag.CurrentEncerrado = chamadosEncerrados = true;
                }
                else
                {
                    ViewBag.CurrentEncerrado = chamadosEncerrados = false;
                }
                //Verificação de tipo de chamado
                if (tipoChamado == null)
                {
                    tipoChamado = "-2";
                }
                ViewBag.CurrentTipoChamado = tipoChamado;
                ViewBag.CurrentFiltro = filtro;
                ViewBag.CurrentSort = sortOrder;
                //Criação da lista de tipos de chamados
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem { Text = "Todos", Value = "-2" },
                                                new SelectListItem { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", tipoChamado);

                var user = manager.FindById(User.Identity.GetUserId());
                //Usuario para Gestao

                //Usuario Vinculado a Obras
                var obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
                var isMatriz = false;
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
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamados(filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }

                }
                else if (Session["PerfilUsuario"].ToString().Equals("2") || Session["PerfilUsuario"].ToString().Equals("3") || Session["PerfilUsuario"].ToString().Equals("4") || Session["PerfilUsuario"].ToString().Equals("7"))
                {
                    var obra = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + " - " + obras[i].Descricao;
                    }
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                }
                else if (Session["PerfilUsuario"].ToString().Equals("5"))
                {
                    ViewBag.NomeObra = "- " + obras[0].Descricao;
                    var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        var chamados = new ChamadoDAO(db).BuscarChamadosDeSetores(setores, filtro, chamadosEncerrados, sortOrder).ToList();
                        chamados.AddRange(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, chamadosEncerrados, sortOrder).ToList());
                        return View(chamados.ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        var chamados = new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setores, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder).ToList();
                        chamados.AddRange(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder).ToList());
                        return View(chamados.ToPagedList(pageNumber, pageSize));
                    }
                }
                else
                {
                    ViewBag.NomeObra = "";
                    pageSize = 7;
                    pageNumber = (page ?? 1);
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult Triagem(int? page, string tipoChamado, string filtro, string obraSelected, string sortOrder)
        {
            try
            {
                //Verificação de tipo de chamado
                if (tipoChamado == null)
                {
                    tipoChamado = "-2";
                }
                if (obraSelected == null)
                {
                    obraSelected = "-1";
                }
                ViewBag.CurrentObra = obraSelected;
                ViewBag.CurrentTipoChamado = tipoChamado;
                ViewBag.CurrentFiltro = filtro;
                ViewBag.CurrentSort = sortOrder;
                //Criação da lista de tipos de chamados
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem { Text = "Todos", Value = "-2" },
                                                new SelectListItem { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", tipoChamado);

                var user = manager.FindById(User.Identity.GetUserId());

                var ddlObraItens = new List<SelectListItem>();
                ddlObraItens.Add(new SelectListItem { Text = "Todas", Value = "-1" });
                var obrasSetoresCorp = new ObraDAO(db).BuscarObrasSetoresCorporativos(new UsuarioSetorDAO(db).buscarSetoresCorporativosDoUsuario(user)[0]);
                foreach (var obra in obrasSetoresCorp)
                {
                    ddlObraItens.Add(new SelectListItem { Text = obra.Descricao, Value = obra.IDO.ToString() });
                }
                ViewBag.ddlObra = new SelectList(ddlObraItens, "Value", "Text", obraSelected);
                //Usuario para Gestao

                //Usuario Vinculado a setor
                var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                var isMatriz = false;
                foreach (var setor in setores)
                {
                    if (setor.SetorCorporativo != null)
                    {
                        isMatriz = true;
                    }
                }
                int pageSize;
                int pageNumber;
                if (isMatriz)
                {
                    ViewBag.NomeObra = "";
                    pageSize = 7;
                    pageNumber = (page ?? 1);

                    if (obraSelected == null || obraSelected == "-1")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosSemResponsaveis(filtro, sortOrder, Convert.ToInt32(tipoChamado)).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosSemResponsaveisPorObra(Convert.ToInt32(obraSelected), Convert.ToInt32(tipoChamado), filtro, sortOrder).ToPagedList(pageNumber, pageSize));
                    }

                }
                return RedirectToAction("Triagem", "Home");
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado/TriagemInfo/5
        public ActionResult TriagemInfo(int id)
        {
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            var user = manager.FindById(User.Identity.GetUserId());
            var dropdownResponsaveis = new List<SelectListItem>();
            dropdownResponsaveis.Add(new SelectListItem { Text = "-- Selecione o Responsavel --", Value = "-1" });
            var usuariosObras = new UsuarioSetorDAO(db).buscarUsuarioObradeSetoresCorporativosDoUsuario(user);
            foreach (var usuarioObra in usuariosObras)
            {
                var NomeUsuario = new ApplicationUserDAO(db).retornarUsuario(usuarioObra.Usuario).Nome;
                dropdownResponsaveis.Add(new SelectListItem { Text = NomeUsuario + " - " + usuarioObra.Obra.Descricao, Value = usuarioObra.Usuario });

            }
            dropdownResponsaveis = dropdownResponsaveis.OrderBy(e => e.Text).ToList();
            ViewBag.ddlResponsavelChamado = new SelectList(dropdownResponsaveis, "Value", "Text", "-1");
            return View(chamado);
        }

        // POST: Chamado/TriagemInfo/5
        [HttpPost]
        public async Task<ActionResult> TriagemInfo(int id, string ddlResponsavelChamado)
        {
            try
            {
                var cGN = new ChamadoGN(db);
                var cDAO = new ChamadoDAO(db);
                var chamado = cDAO.BuscarChamadoId(id);
                chamado.ResponsavelChamado = new ApplicationUserDAO(db).retornarUsuario(ddlResponsavelChamado);
                if ((DateTime.Now - chamado.DataHoraAbertura).TotalMinutes < 30)
                {
                    new ChamadoLogAcaoDAO(db).removerLogIndevido(chamado.Id, 4);
                }
                if (await cGN.atualizarChamadoHistorico(id, "O Chamado foi direcionado para o Usuario " + chamado.ResponsavelChamado.Nome, manager.FindById(User.Identity.GetUserId())))
                {
                    TempData["notice"] = "Chamado Direcionado Com Sucesso!";
                    return RedirectToAction("Triagem", "Chamado");
                }
                else
                {
                    TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                    return RedirectToAction("Triagem", "Chamado");
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Triagem", "Chamado");
            }
        }

        // GET: Chamado/AssumirChamado/5
        public async Task<ActionResult> AssumirChamado(int id)
        {
            try
            {
                var cGN = new ChamadoGN(db);
                var cDAO = new ChamadoDAO(db);
                var chamado = cDAO.BuscarChamadoId(id);
                chamado.ResponsavelChamado = new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId());

                if (await cGN.atualizarChamadoHistorico(id, "O Chamado foi direcionado para o Usuario " + chamado.ResponsavelChamado.Nome, manager.FindById(User.Identity.GetUserId())))
                {
                    TempData["notice"] = "Chamado Assumido Com Sucesso!";
                    return RedirectToAction("Triagem", "Chamado");
                }
                else
                {
                    TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                    return RedirectToAction("Triagem", "Chamado");
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Triagem", "Chamado");
            }
        }


        // GET: Chamado/Details/5
        public ActionResult Details(int id)
        {
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            return View(chamado);
        }

        // GET: Chamado/Encerrado/5
        public ActionResult Encerrado(int id)
        {
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            ViewBag.Classificacao = new ChamadoClassificacaoDAO().BuscarClassificacao(chamado.Classificacao.Value).Descricao.ToString();
            ViewBag.SubClassificacao = new ChamadoSubClassificacaoDAO().BuscarSubClassificacao(chamado.SubClassificacao.Value).Descricao.ToString();

            var isSetor = false;
            var isObra = false;
            var setoresUsuario = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(manager.FindById(User.Identity.GetUserId()));
            var obrasUsuario = new UsuarioObraDAO(db).buscarObrasDoUsuario(manager.FindById(User.Identity.GetUserId()));
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
            else if ((Session["PerfilUsuario"].ToString().Equals("3") || Session["PerfilUsuario"].ToString().Equals("5") || Session["PerfilUsuario"].ToString().Equals("7")) && isSetor)
            {
                ViewBag.VisualizarObsInterna = true;
            }
            else
            {
                ViewBag.VisualizarObsInterna = false;
            }
            return View(chamado);
        }

        public ActionResult EncerradoInfo(int id)
        {
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            if (!chamado.Cancelado)
            {
                ViewBag.Classificacao = new ChamadoClassificacaoDAO(db).BuscarClassificacao(chamado.Classificacao.Value).Descricao;
                ViewBag.SubClassificacao = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacao(chamado.SubClassificacao.Value).Descricao;
            }
            else
            {
                ViewBag.Classificacao = "Chamado Cancelado";
                ViewBag.SubClassificacao = "Chamado Cancelado";

            }
            return View(chamado);
        }


        // GET: Chamado/Details/5
        public ActionResult ChamadoInfo(int id)
        {
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            var chamadoInfo = new ChamadoInfoViewModel
            {
                Id = chamado.Id,
                Assunto = chamado.Assunto,
                Descricao = chamado.Descricao,
                Observacao = chamado.Observacao,
                SetorDestino = chamado.SetorDestino,
                ObraDestino = chamado.ObraDestino,
                TipoChamado = chamado.TipoChamado,
                DataHoraAbertura = chamado.DataHoraAbertura
            };
            return View(chamadoInfo);
        }

        // POST: Chamado/Edit/5
        [HttpPost]
        public async Task<ActionResult> ChamadoInfo(ChamadoInfoViewModel chamadoInfo)
        {
            try
            {
                var cGN = new ChamadoGN(db);
                if (await cGN.atualizarChamadoHistorico(chamadoInfo.Id, chamadoInfo.InformacoesAcompanhamento, manager.FindById(User.Identity.GetUserId())))
                {
                    TempData["notice"] = "Chamado Atualizado Com Sucesso!";
                    return RedirectToAction("Acompanhamento");
                }
                else
                {
                    TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                    return RedirectToAction("Acompanhamento");
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Acompanhamento");
            }
        }

        // GET: Chamado/Create
        [Authorize]
        public ActionResult Create()
        {
            var obras = new List<Obra>();
            if ((bool)Session["UsuarioSetorCorporativo"])
            {
                obras = new ObraDAO(db).BuscarObrasSetoresCorporativos(new UsuarioSetorDAO(db).buscarSetoresCorporativosDoUsuario(new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId()))[0]);
            }
            else
            {
                obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
            }
            ViewBag.UserId = User.Identity.GetUserId();
            ViewBag.ObraDestino = obras;
            if (obras.Count() >= 1)
            {
                ViewBag.ObraUsuario = obras[0].IDO;
                ViewBag.SetorDestino = obras[0].IDO;
            }
            else
            {
                ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetores(), "Id", "Nome");
            }
            return View();
        }

        public ActionResult RetornaSetoresPorObra(string selectedValue)
        {
            try
            {
                return Json(new SelectList(ChamadoGN.RetornarSetoresPorObra(selectedValue), "Id", "Nome"));
            }
            catch (Exception)
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }

        public ActionResult RetornaUsuariosPorObra(string selectedValue)
        {
            try
            {
                var usuarios = new ApplicationUserDAO(db).retornarUsuariosObra(Convert.ToInt32(selectedValue), null);
                ActionResult json = Json(new SelectList(usuarios, "Id", "Nome"));
                return json;
            }
            catch (Exception)
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }

        public ActionResult RetornaResponsaveisPorSetor(string selectedValue)
        {
            try
            {
                var responsaveis = new ApplicationUserDAO(db).retornarUsuariosSetor(new SetorDAO().BuscarSetorId(Convert.ToInt32(selectedValue)), null);
                ActionResult json = Json(new SelectList(responsaveis, "Id", "Nome"));
                return json;
            }
            catch (Exception)
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
                this.ModelState.Remove("ResponsavelAberturaChamado");
                if (ModelState.IsValid)
                {
                    var cGN = new ChamadoGN(db);
                    var chamado = new Chamado
                    {
                        Assunto = chamadoVM.Assunto,
                        Descricao = chamadoVM.Descricao,
                        Observacao = chamadoVM.Observacao,
                        TipoChamado = chamadoVM.TipoChamado
                    };
                    var chamadoRegistro = cGN.registrarChamado(chamado, upload, SetorDestino, ObraDestino, ResponsavelAberturaChamado, manager.FindById(User.Identity.GetUserId()));
                    if (chamadoRegistro != null)
                    {
                        TempData["notice"] = "Chamado Criado com Sucesso!";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        var obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                        ViewBag.UserId = User.Identity.GetUserId();
                        var listObra = new SelectList(obras, "IDO", "Descricao");
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
                    var obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                    ViewBag.UserId = User.Identity.GetUserId();
                    var listObra = new SelectList(obras, "IDO", "Descricao");
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
            catch (Exception)
            {
                var obras = new ObraDAO(db).BuscarObrasPorUsuario(User.Identity.GetUserId());
                ViewBag.UserId = User.Identity.GetUserId();
                var listObra = new SelectList(obras, "IDO", "Descricao");
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
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            ViewBag.listaChamadoHistorico = new ChamadoHistoricoDAO(db).buscarHistoricosPorIdChamado(id);
            ViewBag.listaChamadoAnexo = new ChamadoAnexoDAO(db).retornarListaAnexoChamado(id);
            if (chamado.SetorDestino != null)
            {
                ViewBag.SetorDestino = new SelectList(new SetorDAO(db).BuscarSetoresPorObra(chamado.ObraDestino.IDO), "Id", "Nome", chamado.SetorDestino.Id);

                #region DropDownResponsaveis
                var user = manager.FindById(User.Identity.GetUserId());
                var dropdownResponsaveis = new List<SelectListItem>();
                dropdownResponsaveis.Add(new SelectListItem { Text = "-- Selecione o Responsavel --", Value = "-1" });
                if (new SetorDAO(db).BuscarSetorId(chamado.SetorDestino.Id).SetorCorporativo == null)
                {
                    var usuarios = new ApplicationUserDAO(db).retornarUsuariosSetor(new SetorDAO(db).BuscarSetorId(chamado.SetorDestino.Id), null).ToList();
                    foreach (var usuario in usuarios)
                    {
                        dropdownResponsaveis.Add(new SelectListItem { Text = usuario.Nome, Value = usuario.Id });

                    }
                }
                else
                {
                    var usuariosObras = new UsuarioSetorDAO(db).buscarUsuarioObradeSetoresCorporativosDoUsuario(user);
                    foreach (var usuarioObra in usuariosObras)
                    {
                        var NomeUsuario = new ApplicationUserDAO(db).retornarUsuario(usuarioObra.Usuario).Nome;
                        dropdownResponsaveis.Add(new SelectListItem { Text = NomeUsuario + " - " + usuarioObra.Obra.Descricao, Value = usuarioObra.Usuario });

                    }
                    dropdownResponsaveis = dropdownResponsaveis.OrderBy(e => e.Text).Distinct().ToList();
                }
                #endregion

                if (chamado.ResponsavelChamado != null)
                {
                    if (chamado.TipoChamado == 1)
                    {
                        ViewBag.ddlResponsavelChamado = new SelectList(dropdownResponsaveis, "Value", "Text", chamado.ResponsavelChamado.Id);
                    }
                    else
                    {
                        ViewBag.ddlResponsavelChamado = new SelectList(dropdownResponsaveis, "Value", "Text", chamado.ResponsavelChamado.Id);
                    }
                }
                else
                {
                    if (chamado.TipoChamado == 1)
                    {
                        ViewBag.ddlResponsavelChamado = new SelectList(dropdownResponsaveis, "Value", "Text");
                    }
                    else
                    {
                        ViewBag.ddlResponsavelChamado = new SelectList(dropdownResponsaveis, "Value", "Text");
                    }
                }
            }
            else
            {
                var responsavel = new List<SelectListItem>();
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
                var cGN = new ChamadoGN(db);
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
                    var cGN = new ChamadoGN(db);
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
                    TempData["notice"] = "Por favor falta informar o Técnico Responsavel pelo Atendimento.";
                    return RedirectToAction("Edit", id);
                }
            }
            catch (Exception)
            {
                TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
                return RedirectToAction("Edit", id);
            }
        }

        // GET: Chamado/Encerrar/5
        public ActionResult Encerrar(int id)
        {
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            chamado.StatusChamado = true;
            var model = new EncerramentoChamadoViewModel
            {
                Id = chamado.Id,
                StatusChamado = chamado.StatusChamado,
                Solucao = chamado.Solucao,
                DataHoraAtendimento = chamado.DataHoraAtendimento,
                Classificacao = chamado.Classificacao,
                SubClassificacao = chamado.SubClassificacao,
                ResponsavelChamado = chamado.ResponsavelChamado,
                ObraDestino = chamado.ObraDestino,
                SetorDestino = chamado.SetorDestino
            };
            if (chamado.ResponsavelChamado == null)
            {
                model.ResponsavelChamado = manager.FindById(User.Identity.GetUserId());
            }
            if (chamado.SetorDestino.SetorCorporativo != null)
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(new SetorDAO(db).BuscarSetorId(chamado.SetorDestino.SetorCorporativo.Value)), "Id", "Descricao");
            }
            else
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(chamado.SetorDestino), "Id", "Descricao");
            }
            return View(model);
        }

        // POST: Chamado/Encerrar/5
        [HttpPost]
        public async Task<ActionResult> Encerrar(int id, EncerramentoChamadoViewModel chamado, String data, String hora)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var chamadoOriginal = new ChamadoDAO(db).BuscarChamadoId(id);

                    var atendimento = DateTime.Now;
                    if (string.IsNullOrEmpty(data) || String.IsNullOrEmpty(hora))
                    {
                        atendimento = DateTime.Now;
                    }
                    else
                    {
                        atendimento = Convert.ToDateTime(data + " " + hora);
                    }
                    chamadoOriginal.DataHoraAtendimento = atendimento;
                    chamadoOriginal.Classificacao = chamado.Classificacao;
                    chamadoOriginal.SubClassificacao = chamado.SubClassificacao;
                    chamadoOriginal.Solucao = chamado.Solucao;
                    chamadoOriginal.DataHoraBaixa = DateTime.Now;
                    chamadoOriginal.ResponsavelChamado = manager.FindById(User.Identity.GetUserId());
                    chamadoOriginal.ErroOperacional = chamado.ErroOperacional;
                    new ChamadoDAO(db).encerrarChamado(id, chamadoOriginal);
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamadoOriginal.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.EncerramentoChamado
                    });
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
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
                catch (FormatException)
                {
                    TempData["notice"] = "Erro no Encerramento. Verifique a data e hora do encerramento.";
                    return Redirect("..\\Edit\\" + id);
                }
                catch (DbEntityValidationException ve)
                {
                    var erro = "";
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
                catch (Exception)
                {
                    //string js = @"<script>alert('Erro no Encerramento do Chamado, Por favor Verifique os campos');</script>";
                    //return JavaScript(js);
                    TempData["notice"] = "Erro no Encerramento do Chamado, Por favor Verifique os campos.";
                    return Redirect("..\\Edit\\" + id);
                }
            }
            else
            {
                TempData["notice"] = "Erro no Encerramento do Chamado, Por favor Verifique os campos.";
                return Redirect("..\\Edit\\" + id);
            }
        }

        // GET: Chamado/Cancelar/5
        public ActionResult Cancelar(int id)
        {
            var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
            chamado.StatusChamado = true;
            var model = new CancelamentoChamadoViewModel
            {
                Id = chamado.Id,
                JustificativaCancelamento = chamado.JustificativaCancelamento
            };
            return View(model);
        }

        // POST: Chamado/Cancelar/5
        [HttpPost]
        public async Task<ActionResult> Cancelar(int id, CancelamentoChamadoViewModel chamado)
        {
            try
            {
                var chamadoOriginal = new ChamadoDAO(db).BuscarChamadoId(id);
                chamadoOriginal.JustificativaCancelamento = chamado.JustificativaCancelamento;
                chamadoOriginal.ResponsavelCancelamento = manager.FindById(User.Identity.GetUserId());
                new ChamadoDAO(db).cancelarChamado(id, chamadoOriginal);
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamado.Id.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.CancelamentoChamado
                });
                new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                {
                    IdChamado = chamado.Id,
                    ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(2),
                    Texto = "Chamado Cancelado",
                    DataAcao = DateTime.Now,
                    UsuarioAcao = manager.FindById(User.Identity.GetUserId())

                });
                TempData["notice"] = "Chamado Cancelado com Sucesso!";
                return RedirectToAction("Index");
            }
            catch (FormatException)
            {
                TempData["notice"] = "Erro no Cancelamento.";
                return Redirect("..\\Index\\" + id);
            }
            catch (DbEntityValidationException ve)
            {
                var erro = "";
                foreach (var eve in ve.EntityValidationErrors)
                {
                    foreach (var ev in eve.ValidationErrors)
                    {
                        erro = erro + "O campo: " + ev.PropertyName + " deu erro: " + ev.ErrorMessage;
                    }
                }
                TempData["notice"] = "Erro no Cancelamento: " + erro;
                return Redirect("..\\Index\\" + id);
            }
            catch (Exception)
            {
                //string js = @"<script>alert('Erro no Encerramento do Chamado, Por favor Verifique os campos');</script>";
                //return JavaScript(js);
                TempData["notice"] = "Erro no Cancelamento do Chamado, Por favor Verifique os campos.";
                return Redirect("..\\Index\\" + id);
            }
        }

        public ActionResult RetornaSubClassPorClass(string selectedValue)
        {
            try
            {
                var subClassificacoes = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoesPorClassificacao(Convert.ToInt32(selectedValue));
                ActionResult json = Json(new SelectList(subClassificacoes, "Id", "Descricao"));
                return json;
            }
            catch (Exception)
            {
                return Json(new SelectList(String.Empty, "Id", "Nome")); ;
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult ChamadosEncerrados(int? page, string tipoChamado, string filtro, string sortOrder)
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
                var dropdownItems = new List<SelectListItem>();
                dropdownItems.AddRange(new[]{
                                                new SelectListItem { Text = "Todos", Value = "-2" },
                                                new SelectListItem { Text = "Totvs RM", Value = "1" },
                                                new SelectListItem { Text = "Outros", Value = "2" }
                                            });
                ViewBag.TipoChamado = new SelectList(dropdownItems, "Value", "Text", Session["tipoChamado"].ToString());

                var user = manager.FindById(User.Identity.GetUserId());
                //Usuario Administrador
                if (Session["PerfilUsuario"].ToString().Equals("1")
                    || Session["PerfilUsuario"].ToString().Equals("3")
                    || Session["PerfilUsuario"].ToString().Equals("5")
                    || Session["PerfilUsuario"].ToString().Equals("6")
                    || Session["PerfilUsuario"].ToString().Equals("7"))
                {
                    //Usuario Vinculado a Obras
                    var obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
                    var isMatriz = false;
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
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamados(filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }

                    }
                    else if (isMatriz && Session["PerfilUsuario"].ToString().Equals("7"))
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTecnicoRM(obras, filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosTecnicoRMTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }
                    else if (isMatriz && Session["PerfilUsuario"].ToString().Equals("3"))
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        var setoresUsuario = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                        if (tipoChamado == null || tipoChamado == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeSetores(setoresUsuario, filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setoresUsuario, Convert.ToInt32(tipoChamado), filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }
                    else
                    {
                        ViewBag.NomeObra = "";
                        for (int i = 0; i < obras.Count; i++)
                        {
                            ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                        }
                        var pageSize = 7;
                        var pageNumber = (page ?? 1);
                        if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                        else
                        {
                            return View(new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                        }
                    }

                }
                else
                {
                    var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                    ViewBag.NomeObra = "";
                    for (int i = 0; i < obras.Count; i++)
                    {
                        ViewBag.NomeObra = ViewBag.NomeObra + "- " + obras[i].Descricao;
                    }
                    var pageSize = 7;
                    var pageNumber = (page ?? 1);
                    if (Session["tipoChamado"] == null || Session["tipoChamado"].ToString() == "-2")
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(Session["tipoChamado"].ToString()), filtro, true, sortOrder).ToPagedList(pageNumber, pageSize));
                    }

                }
            }
            catch (NullReferenceException)
            {
                return RedirectToAction("Login", "Account");
            }
        }

        // GET: Chamado
        [Authorize]
        public ActionResult ReaberturaChamado(string id)
        {
            var objCript = new Criptografia(id);
            var idChamado = Convert.ToInt32(objCript["id"].ToString());
            var chamado = new ChamadoDAO(db).BuscarChamadoId(idChamado);

            if (chamado.Cancelado)
            {
                var date = DateTime.Now - chamado.DataHoraCancelamento;
                var dias = date.Value.Days;
                if (dias <= 7)
                {
                    var model = new ReaberturaChamadoViewModel
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
            else if (chamado.StatusChamado.Value)
            {
                var date = DateTime.Now - chamado.DataHoraBaixa;
                var dias = date.Value.Days;
                if (dias <= 7)
                {
                    var model = new ReaberturaChamadoViewModel
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
            var cGN = new ChamadoGN(db);
            var objCript = new Criptografia(id);
            var idChamado = Convert.ToInt32(objCript["id"].ToString());
            await cGN.reaberturaChamado(idChamado, chamado.JustificativaReabertura, manager.FindById(User.Identity.GetUserId()));

            TempData["notice"] = "Chamado Reaberto com Sucesso!";
            return RedirectToAction("Index");
        }

    }
}
