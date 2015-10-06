using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoAcaoDAO
    {
        ApplicationDbContext db;
        public ChamadoAcaoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ChamadoAcaoDAO()
        {
            db = new ApplicationDbContext();
        }

        public ChamadoAcao buscarChamadoAcaoPorId(int id)
        {
            ChamadoAcao chamadoAcao = (from e in db.ChamadoAcao where e.IdAcao == id select e).SingleOrDefault();
            return chamadoAcao;
        }
    }
}