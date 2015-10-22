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

        public List<ChamadoAnexo> retornarListaAnexoChamado(int idChamado)
        {
            var chamadoAnexos = (from ca in db.ChamadoAnexo where ca.Chamado.Id == idChamado select ca).ToList();
            return chamadoAnexos;
        }
    }
}