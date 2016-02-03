using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.GN
{
    public class TarefaGN
    {
        ApplicationDbContext db;
        public TarefaGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Tarefa> BuscarTarefasPorResponsavel(ApplicationUser user)
        {
            return new TarefaDAO(db).BuscarTarefasPorResponsavel(user);
        }
    }
}