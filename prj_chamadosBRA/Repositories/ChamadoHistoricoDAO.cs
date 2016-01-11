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
            var chamadoHistoricos = (from ch in db.ChamadoHistorico where ch.Chamado.Id == idChamado && !ch.RetornoQuestionamento orderby ch.Data descending select ch).ToList();
            return chamadoHistoricos;
        }

        public ChamadoHistorico buscarHistoricosPorId(int IdHistoricoChamado)
        {
            var chamadoHistoricos = (from ch in db.ChamadoHistorico where ch.idChamadoHistorico == IdHistoricoChamado select ch).SingleOrDefault();
            return chamadoHistoricos;
        }

        public Boolean salvarHistorico(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                db.ChamadoHistorico.Add(chamadoHistorico);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}