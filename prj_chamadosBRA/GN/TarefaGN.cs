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

        public List<Tarefa> BuscarTarefasPorResponsavelOuTerceirizado(ApplicationUser user)
        {
            return new TarefaDAO(db).BuscarTarefasPorResponsavelOuTerceirizado(user);
        }

        public List<Tarefa> BuscarTarefasPorResponsavelOuTerceirizadoEncerrados(ApplicationUser user)
        {
            return new TarefaDAO(db).BuscarTarefasPorResponsavelOuTerceirizadoEncerrados(user);
        }

        public Tarefa BuscarTarefasPorId(int id)
        {
            return new TarefaDAO(db).BuscarTarefaId(id);
        }

        public bool gerarTarefa(Tarefa tarefa)
        {
            try
            {
                new TarefaDAO(db).salvarTarefa(tarefa);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool atualizarTarefa(Tarefa tarefa)
        {
            try
            {
                new TarefaDAO(db).atualizarTarefa(tarefa);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}