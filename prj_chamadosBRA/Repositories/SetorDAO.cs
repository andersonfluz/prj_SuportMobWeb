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

        public List<Setor> BuscarSetores()
        {

            List<Setor> setores = (from e in db.Setor select e).ToList();
            return setores;
        }

        public Setor BuscarSetorId(int id)
        {

            Setor setor = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            return setor;
        }

        public List<Setor> BuscarSetoresNome(string nome)
        {

            List<Setor> setores = (from e in db.Setor where e.Nome == nome select e).ToList();
            return setores;
        }

        public Setor DetalhesSetor(int id)
        {
            Setor setor = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            return setor;
        }

        public Boolean salvarSetor(Setor setor)
        {

            db.Setor.Add(setor);
            db.SaveChanges();
            return true;
        }

        public void eliminarSetor(int id)
        {
            db.Entry(this.DetalhesSetor(id)).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
        }

        public void atualizarSetor(int id, Setor setor)
        {
            Setor setorUpdate = (from e in db.Setor where e.Id == id select e).SingleOrDefault();
            setorUpdate.Nome = setor.Nome;
            setorUpdate.Descricao = setor.Descricao;
            setorUpdate.Responsavel = setor.Responsavel;
            setorUpdate.EmailResponsavel = setor.EmailResponsavel;
            setorUpdate.EmailSetor = setor.EmailSetor;
            db.SaveChanges();
        }
    }
}