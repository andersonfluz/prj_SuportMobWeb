using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoDAO
    {
        ApplicationDbContext db;
        public ChamadoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Chamado> BuscarChamados()
        {
            List<Chamado> chamados = (from e in db.Chamado select e).ToList();
            foreach (var chamado in chamados)
            {
                ApplicationUser resp = chamado.ResponsavelChamado;
                ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
                chamado.ResponsavelChamado = resp;
                chamado.ResponsavelAberturaChamado = respAb;
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDoUsuario(ApplicationUser user)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id select e).ToList();
            foreach (var chamado in chamados)
            {
                ApplicationUser resp = chamado.ResponsavelChamado;
                ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
                chamado.ResponsavelChamado = resp;
                chamado.ResponsavelAberturaChamado = respAb;
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeObras(List<Obra> obras)
        {
            List<Chamado> chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                chamados = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO select e).ToList();
            }
            foreach (var chamado in chamados)
            {
                ApplicationUser resp = chamado.ResponsavelChamado;
                ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
                chamado.ResponsavelChamado = resp;
                chamado.ResponsavelAberturaChamado = respAb;
            }
            return chamados;
        }

        public Chamado BuscarChamadoId(int id)
        {
            Chamado chamado = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            return chamado;
        }

        public Chamado DetalhesChamado(int id)
        {
            Chamado chamado = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            return chamado;
        }

        public Boolean salvarChamado(Chamado chamado)
        {
            db.Chamado.Add(chamado);
            db.SaveChanges();
            return true;
        }

        public ChamadoHistorico registrarHistorico(DateTime dataHora, ApplicationUser responsavel, String Historico, Chamado chamado)
        {
            try
            {
                ChamadoHistorico ch = new ChamadoHistorico();
                ch.chamado = chamado;
                ch.Data = dataHora;
                ch.Hora = dataHora;
                ch.Responsavel = responsavel;
                ch.Historico = Historico;
                new ChamadoHistoricoDAO(db).salvarHistorico(ch);
                return ch;
            }
            catch
            {
                throw;
            }
        }

        public void atualizarChamado(int id, Chamado chamado)
        {
            Chamado chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            chamadoUpdate.ObsevacaoInterna = chamado.ObsevacaoInterna;
            chamadoUpdate.SetorDestino = chamado.SetorDestino;
            chamadoUpdate.ResponsavelChamado = chamado.ResponsavelChamado;
            db.SaveChanges();
        }

    }
}