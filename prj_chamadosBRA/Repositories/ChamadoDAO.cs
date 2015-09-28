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

        public List<Chamado> BuscarChamados(string filtro, bool encerrado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.StatusChamado == encerrado select e).ToList();
            if(filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosTipoChamado(int? tipoChamado, string filtro, bool encerrado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e).ToList();
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDoUsuario(ApplicationUser user, string filtro, bool encerrado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado != true select e).ToList();
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDoUsuarioTipoChamado(ApplicationUser user, int? tipoChamado, string filtro, bool encerrado)
        {
            List<Chamado> chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e).ToList();
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeObras(List<Obra> obras, string filtro, bool encerrado)
        {
            List<Chamado> chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                var chamadosList = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado == encerrado select e).ToList();
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeObrasTipoChamado(List<Obra> obras, int? tipoChamado, string filtro, bool encerrado)
        {
            List<Chamado> chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                var chamadosList = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e).ToList();
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
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