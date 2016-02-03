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
    public class TarefaController : Controller
    {
        private UserManager<ApplicationUser> manager;
        private ApplicationDbContext db = new ApplicationDbContext();
        public TarefaController()
        {
            manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
        }

        // GET: Tarefa
        public async Task<ActionResult> Index()
        {            
            return View(new TarefaGN(db).BuscarTarefasPorResponsavel(manager.FindById(User.Identity.GetUserId())));
        }

        // GET: Tarefa/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = await db.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // GET: Tarefa/Create
        public ActionResult Create(int id)
        {
            var tarefa = new TarefaViewModel
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
            return View(tarefa);
        }

        // POST: Tarefa/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                db.Tarefa.Add(tarefa);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(tarefa);
        }

        // GET: Tarefa/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = await db.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // POST: Tarefa/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Assunto,Descricao,Especialista,StatusTarefa,Solucao,Cancelado,JustificativaCancelamento")] Tarefa tarefa)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tarefa).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(tarefa);
        }

        // GET: Tarefa/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tarefa tarefa = await db.Tarefa.FindAsync(id);
            if (tarefa == null)
            {
                return HttpNotFound();
            }
            return View(tarefa);
        }

        // POST: Tarefa/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Tarefa tarefa = await db.Tarefa.FindAsync(id);
            db.Tarefa.Remove(tarefa);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
