using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class SetorDAO
    {
        public List<Setor> BuscarSetores()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Setor> setores = (from e in ctx.Setor select e).ToList();
                return setores;
            }
        }

        public List<Setor> BuscarSetoresNome(string nome)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Setor> setores = (from e in ctx.Setor where e.Nome == nome select e).ToList();
                return setores;
            }
        }

        public Setor DetalhesSetor(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Setor setor = (from e in ctx.Setor where e.Id == id select e).SingleOrDefault();
                return setor;
            }
        }

        public Boolean salvarSetor(Setor setor)
        {           
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Setor.Add(setor);
                ctx.SaveChanges();
                return true;
            }
        }

        public void eliminarSetor(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Entry(this.DetalhesSetor(id)).State = System.Data.Entity.EntityState.Deleted;
                ctx.SaveChanges();
            }
        }

        public void atualizarSetor(int id, Setor setor)
        {
            using (var ctx = new ApplicationDbContext())
            {
                Setor setorUpdate = (from e in ctx.Setor where e.Id == id select e).SingleOrDefault();
                setorUpdate.Nome = setor.Nome;
                setorUpdate.Descricao = setor.Descricao;
                setorUpdate.Responsavel = setor.Responsavel;
                setorUpdate.EmailResponsavel = setor.EmailResponsavel;
                setorUpdate.EmailSetor = setor.EmailSetor;
                ctx.SaveChanges();
            }
        }
    }
}