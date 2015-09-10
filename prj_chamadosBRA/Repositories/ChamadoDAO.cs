using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            List<Chamado> chamados = (from e in db.Chamado where e.StatusChamado != true select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosTipoChamado(int? tipoChamado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.StatusChamado != true && e.TipoChamado == tipoChamado select e).ToList();
            //foreach (var chamado in chamados)
            //{
            //    ApplicationUser resp = chamado.ResponsavelChamado;
            //    ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
            //    chamado.ResponsavelChamado = resp;
            //    chamado.ResponsavelAberturaChamado = respAb;
            //}
            return chamados;
        }

        public List<Chamado> BuscarChamadosDoUsuario(ApplicationUser user)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado != true select e).ToList();
            //foreach (var chamado in chamados)
            //{
            //    ApplicationUser resp = chamado.ResponsavelChamado;
            //    ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
            //    chamado.ResponsavelChamado = resp;
            //    chamado.ResponsavelAberturaChamado = respAb;
            //}
            return chamados;
        }

        public List<Chamado> BuscarChamadosDoUsuarioTipoChamado(ApplicationUser user, int? tipoChamado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado != true && e.TipoChamado == tipoChamado select e).ToList();
            //foreach (var chamado in chamados)
            //{
            //    ApplicationUser resp = chamado.ResponsavelChamado;
            //    ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
            //    chamado.ResponsavelChamado = resp;
            //    chamado.ResponsavelAberturaChamado = respAb;
            //}
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeObras(List<Obra> obras)
        {
            List<Chamado> chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                chamados = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado != true select e).ToList();
            }
            //foreach (var chamado in chamados)
            //{
            //    ApplicationUser resp = chamado.ResponsavelChamado;
            //    ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
            //    chamado.ResponsavelChamado = resp;
            //    chamado.ResponsavelAberturaChamado = respAb;
            //}
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeObrasTipoChamado(List<Obra> obras, int? tipoChamado)
        {
            List<Chamado> chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                chamados = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado != true && e.TipoChamado == tipoChamado select e).ToList();
            }
            //foreach (var chamado in chamados)
            //{
            //    ApplicationUser resp = chamado.ResponsavelChamado;
            //    ApplicationUser respAb = chamado.ResponsavelAberturaChamado;
            //    chamado.ResponsavelChamado = resp;
            //    chamado.ResponsavelAberturaChamado = respAb;
            //}
            return chamados;
        }

        public Chamado BuscarChamadoId(int id)
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

        public void atualizarChamado(int id, Chamado chamado)
        {
            try
            {
                Chamado chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
                chamadoUpdate.ObsevacaoInterna = chamado.ObsevacaoInterna;
                chamadoUpdate.SetorDestino = chamado.SetorDestino;
                chamadoUpdate.ObraDestino = chamado.ObraDestino;
                chamadoUpdate.ResponsavelChamado = chamado.ResponsavelChamado;
                chamadoUpdate.TipoChamado = chamado.TipoChamado;
                //db.Entry(chamado).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception e)
            {

            }
        }

        public void encerrarChamado(int id, Chamado chamado)
        {
            Chamado chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            chamadoUpdate.DataHoraAtendimento = chamado.DataHoraAtendimento;
            chamadoUpdate.Classificacao = chamado.Classificacao;
            chamadoUpdate.SubClassificacao = chamado.SubClassificacao;
            chamadoUpdate.DataHoraBaixa = chamado.DataHoraBaixa;
            chamadoUpdate.ObraDestino = chamado.ObraDestino;
            chamadoUpdate.Solucao = chamado.Solucao;
            chamadoUpdate.StatusChamado = true;
            db.SaveChanges();
        }
    }
}