using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoHistoricoDAO
    {
        ApplicationDbContext db;
        public ChamadoHistoricoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<ChamadoHistorico> buscarHistoricosPorIdChamado(int idChamado)
        {
            List<ChamadoHistorico> chamadoHistoricos = (from ch in db.ChamadoHistorico where ch.chamado.Id == idChamado select ch).ToList();
            return chamadoHistoricos;
        }
    }
}