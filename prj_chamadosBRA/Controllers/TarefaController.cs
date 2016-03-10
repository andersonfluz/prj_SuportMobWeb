using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using prj_chamadosBRA.GN;

namespace prj_chamadosBRA.Controllers
{
    [Authorize]
    public class TarefaController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public TarefaController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Tarefa
        [Authorize]
        public async Task<ActionResult> Index()
        {
            var tarefas = new TarefaGN(db).BuscarTarefasPorResponsavelOuTerceirizado(manager.FindById(User.Identity.GetUserId()));
            var tarefaVM = new List<ListaTarefaViewModel>();
            foreach(var tarefa in tarefas)
            {
                tarefaVM.Add(new ListaTarefaViewModel
                {
                    Id = tarefa.Id,
                    Assunto = tarefa.Assunto,
                    Solicitante = tarefa.Solicitante,
                    TipoTarefa = tarefa.TipoTarefa.Value,
                    DataAbertura = tarefa.DataAbertura,
                    DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega
                });
            }
            return View(tarefaVM);
        }

        // GET: Tarefa/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id.Value);
            var tarefaVM = new DetalhesTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                DataAbertura = tarefa.DataAbertura,
                DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega,
                Chamado = tarefa.Chamado,
                Natureza = tarefa.Natureza,
                SubNatureza = tarefa.SubNatureza,
                Responsavel = tarefa.Responsavel,
                Solicitante = tarefa.Solicitante,
                Aprovado = tarefa.Aprovado,
                JustificativaAprovacao = tarefa.Justificativa
            };
            return View(tarefaVM);
        }

        // GET: Tarefa/Create
        public ActionResult Create(int id)
        {
            var tarefa = new NovaTarefaViewModel
            {
                Chamado = new ChamadoDAO(db).BuscarChamadoId(id)
            };
            if (tarefa.Chamado.SetorDestino.SetorCorporativo != null)
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(new SetorDAO(db).BuscarSetorId(tarefa.Chamado.SetorDestino.SetorCorporativo.Value)), "Id", "Descricao");
            }
            else
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(tarefa.Chamado.SetorDestino), "Id", "Descricao");
            }
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

            ViewBag.ddlResponsavelTerceirizado = new SelectList(new ApplicationUserDAO(db).retornarUsuariosTerceirizados(), "Id", "Nome");
            
            return View(tarefa);
        }

        // POST: Tarefa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NovaTarefaViewModel tarefaVM)
        {
            var idChamado = tarefaVM.Chamado.Id;
            if (ModelState.IsValid)
            {
                var chamado = new ChamadoDAO(db).BuscarChamadoId(tarefaVM.Chamado.Id);
                ApplicationUser responsavel = null;
                var aDAO = new ApplicationUserDAO(db);
                if(tarefaVM.TipoResponsavel == "1")
                {
                    responsavel = chamado.ResponsavelAberturaChamado;
                }else if (tarefaVM.TipoResponsavel == "2" && tarefaVM.Responsavel != null)
                {
                    responsavel = aDAO.retornarUsuario(tarefaVM.Responsavel);
                }else if(tarefaVM.TipoResponsavel == "3" && tarefaVM.ResponsavelTerceiros != null)
                {
                    responsavel = aDAO.retornarUsuario(tarefaVM.ResponsavelTerceiros);
                }else if(tarefaVM.TipoResponsavel == "4")
                {
                    responsavel = aDAO.retornarUsuario(chamado.ResponsavelAberturaChamado.Superior);
                }
                if(responsavel == null)
                {
                    TempData["sucess"] = "Informe para quem será a tarefa.";
                    return RedirectToAction("Edit", "Chamado", new { id = idChamado });
                }
                var tarefa = new Tarefa
                {
                    Assunto = tarefaVM.Assunto,
                    Descricao = tarefaVM.Descricao,
                    TipoTarefa = tarefaVM.TipoTarefa,
                    Responsavel = responsavel,
                    DataAbertura = DateTime.Now,
                    Solicitante = new ApplicationUserDAO(db).retornarUsuario(User.Identity.GetUserId()),
                    DataPrevisaoEntrega = null,
                    Terceirizado = tarefaVM.Terceirizado,
                    Chamado = chamado,
                    Natureza = new ChamadoClassificacaoDAO(db).BuscarClassificacao(tarefaVM.Natureza.Value),
                    SubNatureza = new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacao(tarefaVM.SubNatureza.Value),
                    Especialista = tarefaVM.Especialista
                };
                if (new TarefaGN(db).gerarTarefa(tarefa))
                {
                    TempData["sucess"] = "Tarefa Gerada com sucesso!";
                    return RedirectToAction("Edit", "Chamado", new { id = idChamado });
                }
            }
            TempData["notice"] = "Desculpe, estamos com problemas ao atualizar o chamado.";
            return RedirectToAction("Edit", "Chamado", new { id = idChamado });
        }

        // GET: Tarefa/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id);
            var tarefaVM = new EditarTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                Chamado = tarefa.Chamado,
                DataAbertura = tarefa.DataAbertura,
                Responsavel = tarefa.Responsavel.Id,
                Natureza = tarefa.Natureza.Id,
                SubNatureza = tarefa.SubNatureza.Id
            };
            ViewBag.NomeResponsavel = tarefa.Responsavel.Nome;
            if (tarefa.Chamado.SetorDestino.SetorCorporativo != null)
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(new SetorDAO(db).BuscarSetorId(tarefa.Chamado.SetorDestino.SetorCorporativo.Value)), "Id", "Descricao");
                ViewBag.SetorDestinoSubClassificacao = new SelectList(new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoesPorClassificacao(tarefaVM.Natureza.Value), "Id", "Descricao");
            }
            else
            {
                ViewBag.SetorDestinoClassificacao = new SelectList(new ChamadoClassificacaoDAO(db).BuscarClassificacoesPorSetor(tarefa.Chamado.SetorDestino), "Id", "Descricao");
                ViewBag.SetorDestinoSubClassificacao = new SelectList(new ChamadoSubClassificacaoDAO(db).BuscarSubClassificacoesPorClassificacao(tarefaVM.Natureza.Value), "Id", "Descricao");
            }
            return View(tarefaVM);
        }

        //// POST: Tarefa/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit(EditarTarefaViewModel tarefaVM)
        //{
        //    var tarefa = new TarefaGN(db).BuscarTarefasPorId(tarefaVM.Id);
        //    tarefa.
        //}

        // GET: Tarefa/Edit/5
        public async Task<ActionResult> PrevisaoEntrega(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id.Value);
            var tarefaVM = new PrevisaoTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                DataAbertura = tarefa.DataAbertura,
                DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega,
                Chamado = tarefa.Chamado,
                Natureza = tarefa.Natureza,
                SubNatureza = tarefa.SubNatureza,
                Responsavel = tarefa.Responsavel,
                Solicitante = tarefa.Solicitante
            };
            return View(tarefaVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> PrevisaoEntrega(PrevisaoTarefaViewModel tarefa)
        {
            if (ModelState.IsValid)
            {
                var tarefaAtualizada = new TarefaGN(db).BuscarTarefasPorId(tarefa.Id);
                tarefaAtualizada.DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega;
                
                if (new TarefaGN(db).atualizarTarefa(tarefaAtualizada))
                {
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = tarefaAtualizada.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.PrevisaoEntregaTarefa
                    });
                    TempData["sucess"] = "Previsão de Entrega da tarefa gravada com sucesso!";
                    return RedirectToAction("Index", "Tarefa");
                }
                else
                {
                    TempData["problem"] = "Problemas ao atualizar a tarefa!";
                    return RedirectToAction("Index", "Tarefa");
                }

            }
            else
            {
                TempData["problem"] = "Verifique os dados informados!";
                return RedirectToAction("Index", "Tarefa");
            }

        }

        public async Task<ActionResult> Encerrar(int id)
        {
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id);
            var tarefaVM = new EncerramentoTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                DataAbertura = tarefa.DataAbertura,
                Responsavel = tarefa.Responsavel,
                Solicitante = tarefa.Solicitante,
                Chamado = tarefa.Chamado

            };
            return View(tarefaVM);
        }

        // POST: Tarefa/Encerrar/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Encerrar(EncerramentoTarefaViewModel tarefa, string hora)
        {
            if (ModelState.IsValid)
            {
                var tarefaAtualizada = new TarefaGN(db).BuscarTarefasPorId(tarefa.Id);
                tarefa.DataEntrega = tarefa.DataEntrega.AddHours(Convert.ToDouble(hora.Substring(0, 2)));
                tarefa.DataEntrega = tarefa.DataEntrega.AddMinutes(Convert.ToDouble(hora.Substring(3, 2)));
                tarefaAtualizada.DataEntrega = tarefa.DataEntrega;
                tarefaAtualizada.Solucao = tarefa.Solucao;
                tarefaAtualizada.StatusTarefa = true;
                if (new TarefaGN(db).atualizarTarefa(tarefaAtualizada))
                {
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = tarefaAtualizada.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.EntregaTarefa
                    });
                    TempData["sucess"] = "Tarefa entregue com sucesso!";
                    return RedirectToAction("Index", "Tarefa");
                }
                else
                {
                    TempData["problem"] = "Problemas ao atualizar a tarefa!";
                    return RedirectToAction("Index", "Tarefa");
                }

            }
            else
            {
                TempData["problem"] = "Verifique os dados informados!";
                return RedirectToAction("Index", "Tarefa");
            }
        }

        public async Task<ActionResult> Aprovacao(int id)
        {
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id);
            var tarefaVM = new AprovacaoTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                DataAbertura = tarefa.DataAbertura,
                Responsavel = tarefa.Responsavel,
                Solicitante = tarefa.Solicitante,
                Chamado = tarefa.Chamado,
                Aprovado = tarefa.Aprovado

            };
            return View(tarefaVM);
        }

        // POST: Tarefa/Aprovacao/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Aprovacao(AprovacaoTarefaViewModel tarefa)
        {
            if (ModelState.IsValid)
            {
                var tarefaAtualizada = new TarefaGN(db).BuscarTarefasPorId(tarefa.Id);
                tarefaAtualizada.DataEntrega = DateTime.Now;
                tarefaAtualizada.Aprovado = tarefa.Aprovado;
                tarefaAtualizada.Justificativa = tarefa.Justificativa;
                tarefaAtualizada.StatusTarefa = true;
                if (new TarefaGN(db).atualizarTarefa(tarefaAtualizada))
                {
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = tarefaAtualizada.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.EntregaTarefa
                    });
                    TempData["sucess"] = "Tarefa entregue com sucesso!";
                    return RedirectToAction("Index", "Tarefa");
                }
                else
                {
                    TempData["problem"] = "Problemas ao atualizar a tarefa!";
                    return RedirectToAction("Index", "Tarefa");
                }

            }
            else
            {
                TempData["problem"] = "Verifique os dados informados!";
                return RedirectToAction("Index", "Tarefa");
            }
        }
        
        // GET: Tarefa
        [Authorize]
        public async Task<ActionResult> Encerrados()
        {
            var tarefas = new TarefaGN(db).BuscarTarefasPorResponsavelOuTerceirizadoEncerrados(manager.FindById(User.Identity.GetUserId()));
            var tarefaVM = new List<ListaTarefaViewModel>();
            foreach (var tarefa in tarefas)
            {
                tarefaVM.Add(new ListaTarefaViewModel
                {
                    Id = tarefa.Id,
                    Assunto = tarefa.Assunto,
                    Solicitante = tarefa.Solicitante,
                    DataAbertura = tarefa.DataAbertura,
                    DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega,
                    DataEntrega = tarefa.DataEntrega.Value
                });
            }
            return View(tarefaVM);
        }

        // GET: Tarefa
        [Authorize]
        public async Task<ActionResult> EncerradoInfo(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tarefa = new TarefaGN(db).BuscarTarefasPorId(id.Value);
            var tarefaVM = new DetalhesTarefaViewModel
            {
                Id = tarefa.Id,
                Assunto = tarefa.Assunto,
                Descricao = tarefa.Descricao,
                DataAbertura = tarefa.DataAbertura,
                DataPrevisaoEntrega = tarefa.DataPrevisaoEntrega,
                Chamado = tarefa.Chamado,
                Natureza = tarefa.Natureza,
                SubNatureza = tarefa.SubNatureza,
                Responsavel = tarefa.Responsavel,
                Solicitante = tarefa.Solicitante,
                DataEntrega = tarefa.DataEntrega,
                Solucao = tarefa.Solucao,
                Aprovado = tarefa.Aprovado,
                JustificativaAprovacao = tarefa.Justificativa
            };
            return View(tarefaVM);
        }
    }
}
