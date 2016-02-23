using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoClassificacaoDAO
    {
        ApplicationDbContext db;
        public ChamadoClassificacaoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ChamadoClassificacaoDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public ChamadoClassificacao BuscarClassificacao(int id)
        {
            var classificacao = (from e in db.ChamadoClassificacao where e.Id == id select e).SingleOrDefault();
            return classificacao;
        }

        public List<ChamadoClassificacao> BuscarClassificacoes()
        {
            var classificacoes = (from e in db.ChamadoClassificacao select e).OrderBy(s => s.Descricao).ToList();
            return classificacoes;
        }

        //public List<ChamadoClassificacao> BuscarClassificacoesPorObra(Obra obra)
        //{
        //    var classificacoes = (from e in db.ChamadoClassificacao where e.Setor.obra.IDO == obra.IDO select e).ToList();
        //    return classificacoes;
        //}

        public List<ChamadoClassificacao> BuscarClassificacoesPorSetor(Setor setor)
        {
            var classificacoes = (from e in db.ChamadoClassificacao where e.Setor.Id == setor.Id select e).OrderBy(s => s.Descricao).ToList();
            return classificacoes;
        }

        public List<ChamadoClassificacao> BuscarClassificacoesPorObras(List<Obra> obras)
        {
            var idObras = (from e in obras select e.IDO).ToList();
            var classificacoes = (from e in db.ChamadoClassificacao where idObras.Contains(e.Setor.obra.IDO) select e).OrderBy(s => s.Descricao).ToList();
            return classificacoes;
        }

        public List<ChamadoClassificacao> BuscarClassificacoesPorSetores(List<Setor> setores)
        {
            var idSetores = (from e in setores select e.Id).ToList();
            var classificacoes = (from e in db.ChamadoClassificacao where idSetores.Contains(e.Setor.Id) select e).OrderBy(s => s.Descricao).ToList();
            return classificacoes;
        }

        public bool salvarClassificacao(ChamadoClassificacao classificacao, string idUsuario)
        {
            classificacao.DataCriacao = DateTime.Now;
            classificacao.Usuario = idUsuario;
            classificacao.Ativo = true;
            db.ChamadoClassificacao.Add(classificacao);
            db.SaveChanges();
            return true;
        }

        public void atualizarClassificacao(int id, ChamadoClassificacao classificacao, string idUsuario)
        {
            var classificacaoUpdate = (from e in db.ChamadoClassificacao where e.Id == id select e).SingleOrDefault();
            classificacaoUpdate.Descricao = classificacao.Descricao;
            classificacaoUpdate.Setor = classificacao.Setor;
            classificacao.DataAlteracao = DateTime.Now;
            classificacao.Usuario = idUsuario;
            db.SaveChanges();
        }
    }
}