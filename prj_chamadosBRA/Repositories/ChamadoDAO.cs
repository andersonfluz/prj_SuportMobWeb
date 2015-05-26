using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoDAO
    {
        public List<Chamado> BuscarChamados()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Chamado> chamados = (from e in ctx.Chamado select e).ToList();
                return chamados;
            }
        }

        public List<Chamado> BuscarChamadoId(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Chamado> chamado = (from e in ctx.Chamado where e.Id == id select e).ToList();
                return chamado;
            }
        }

        public Chamado DetalhesChamado(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Chamado chamado = (from e in ctx.Chamado where e.Id == id select e).SingleOrDefault();
                return chamado;
            }
        }

        public Boolean salvarChamado(Chamado chamado)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Chamado.Add(chamado);
                ctx.SaveChanges();
                return true;
            }
        }

        //public void eliminarSetor(int id)
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        ctx.Entry(this.DetalhesSetor(id)).State = System.Data.Entity.EntityState.Deleted;
        //        ctx.SaveChanges();
        //    }
        //}

        //public void atualizarSetor(int id, Setor setor)
        //{
        //    using (var ctx = new ApplicationDbContext())
        //    {
        //        Setor setorUpdate = (from e in ctx.Setor where e.Id == id select e).SingleOrDefault();
        //        setorUpdate.Nome = setor.Nome;
        //        setorUpdate.Descricao = setor.Descricao;
        //        setorUpdate.Responsavel = setor.Responsavel;
        //        setorUpdate.EmailResponsavel = setor.EmailResponsavel;
        //        setorUpdate.EmailSetor = setor.EmailSetor;
        //        ctx.SaveChanges();
        //    }
        //}
    }
}