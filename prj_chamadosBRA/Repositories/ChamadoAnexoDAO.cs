using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoAnexoDAO
    {
        ApplicationDbContext db;
        public ChamadoAnexoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }
        public Boolean salvarChamadoAnexo(ChamadoAnexo chamadoAnexo)
        {
            db.ChamadoAnexo.Add(chamadoAnexo);
            db.SaveChanges();
            return true;
        }
    }
}