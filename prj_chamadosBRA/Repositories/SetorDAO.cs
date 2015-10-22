using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class SetorDAO
    {
        ApplicationDbContext db;
        public SetorDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public SetorDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Setor> BuscarSetores()
        {
            var setores = (from e in db.Setor select e).ToList();
            return setores;
        }

        public List<Setor> BuscarSetoresPorObra(int idObra)
        {
            var setores = (from e in db.Setor where e.obra.IDO == idObra select e).ToList();
            return setores;
        }

        public List<Setor> BuscarSetoresPorObras(List<Obra> obras)
        {
            var idObras = (from e in obras select e.IDO).ToList();
            var setores = (from e in db.Setor where idObras.Contains(e.obra.IDO) select e).ToList();
            return setores;
        }

        public Setor BuscarSetorId(int id)
        {

            var setor = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            return setor;
        }

        public List<Setor> BuscarSetoresNome(string nome)
        {

            var setores = (from e in db.Setor where e.Nome == nome select e).ToList();
            return setores;
        }

        public Setor DetalhesSetor(int id)
        {
            var setor = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            return setor;
        }

        public Boolean salvarSetor(Setor setor)
        {

            db.Setor.Add(setor);
            db.SaveChanges();
            return true;
        }

        //public void eliminarSetor(int id)
        //{
        //    db.Entry(this.DetalhesSetor(id)).State = System.Data.Entity.EntityState.Deleted;
        //    db.SaveChanges();
        //}

        public void atualizarSetor(int id, Setor setor)
        {
            var setorUpdate = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            setorUpdate.Nome = setor.Nome;
            setorUpdate.Descricao = setor.Descricao;
            setorUpdate.Responsavel = setor.Responsavel;
            setorUpdate.EmailResponsavel = setor.EmailResponsavel;
            setorUpdate.EmailSetor = setor.EmailSetor;
            var obra = setorUpdate.obra;
            setorUpdate.obra = obra;            
            db.SaveChanges();
        }
    }
}