using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class TarefaDAO
    {
        ApplicationDbContext db;

        public TarefaDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Tarefa BuscarTarefaId(int id)
        {
            return db.Set<Tarefa>().Find(id);
        }

        public List<Tarefa> BuscarTarefasPorChamado(int IdChamado)
        {
            return db.Set<Tarefa>().Where(c=>c.Chamado.Id == IdChamado).ToList();
        }

        public List<Tarefa> BuscarTarefasPorResponsavel(ApplicationUser user)
        {
            return db.Set<Tarefa>().Where(c => c.Responsavel.Id == user.Id).ToList();
        }
    }
}