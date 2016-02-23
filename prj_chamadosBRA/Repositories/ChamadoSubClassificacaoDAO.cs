using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoSubClassificacaoDAO
    {
        ApplicationDbContext db;
        public ChamadoSubClassificacaoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ChamadoSubClassificacaoDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<ChamadoSubClassificacao> BuscarSubClassificacoes()
        {
            var subclassificacoes = (from e in db.ChamadoSubClassificacao select e).OrderBy(s => s.Descricao).ToList();
            return subclassificacoes;
        }

        public ChamadoSubClassificacao BuscarSubClassificacao(int id)
        {
            var subclassificacao = (from e in db.ChamadoSubClassificacao where e.Id == id select e).OrderBy(s => s.Descricao).SingleOrDefault();
            return subclassificacao;
        }

        public List<ChamadoSubClassificacao> BuscarSubClassificacoesPorClassificacao(int idClassificacao)
        {
            var chamadoSubClassificacoes = (from e in db.ChamadoSubClassificacao where e.ChamadoClassificacao.Id == idClassificacao select e).OrderBy(s => s.Descricao).ToList();
            return chamadoSubClassificacoes;
        }

        public Boolean salvarSubClassificacao(ChamadoSubClassificacao subclassificacao)
        {
            db.ChamadoSubClassificacao.Add(subclassificacao);
            db.SaveChanges();
            return true;
        }

        public void atualizarSubClassificacao(int id, ChamadoSubClassificacao subClassificacao)
        {
            var subClassificacaoUpdate = (from e in db.ChamadoSubClassificacao where e.Id == id select e).SingleOrDefault();
            subClassificacaoUpdate.Descricao = subClassificacao.Descricao;
            db.SaveChanges();
        }
    }
}