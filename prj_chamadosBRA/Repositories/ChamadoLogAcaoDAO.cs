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
        public bool salvar(ChamadoLogAcao chamadoLogAcao)
        {
            db.ChamadoLogAcao.Add(chamadoLogAcao);
            db.SaveChanges();
            return true;
        }
    }
}