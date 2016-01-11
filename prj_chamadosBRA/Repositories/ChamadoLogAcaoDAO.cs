using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoLogAcaoDAO
    {
        ApplicationDbContext db;
        public ChamadoLogAcaoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ChamadoLogAcaoDAO()
        {
            db = new ApplicationDbContext();
        }

        public bool existeLog(int IdChamado, int ChamadoAcao)
        {
            var log = (from l in db.ChamadoLogAcao where l.IdChamado == IdChamado && l.ChamadoAcao.IdAcao == ChamadoAcao select l).ToList();
            if (log.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        public bool salvar(ChamadoLogAcao chamadoLogAcao)
        {
            db.ChamadoLogAcao.Add(chamadoLogAcao);
            db.SaveChanges();
            return true;
        }

        public void removerLogIndevido(int IdChamado, int ChamadoAcao)
        {
            var log = (from l in db.ChamadoLogAcao where l.IdChamado == IdChamado && l.ChamadoAcao.IdAcao == ChamadoAcao select l).FirstOrDefault();
            if (log != null)
            {
                db.ChamadoLogAcao.Remove(log);
                db.SaveChanges();
            }
        }
    }
}