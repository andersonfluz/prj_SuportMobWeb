using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ObraDAO
    {
        ApplicationDbContext db;
        public ObraDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ObraDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Obra> BuscarObras()
        {

            var obras = (from e in db.Obra select e).ToList();
            return obras;
        }

        public Obra BuscarObraId(int ido)
        {

            var obra = (from e in db.Obra where e.IDO == ido select e).SingleOrDefault();
            return obra;
        }

        public List<Obra> BuscarObrasPorUsuario(String userId)
        {
            var idObras = (from e in db.UsuarioObra where e.Usuario == userId select e.Obra).ToList();
            var obras = (from o in db.Obra where idObras.Contains(o.IDO) select o).ToList();
            return obras;
        }

        public Boolean salvarObra(Obra obra)
        {
            db.Obra.Add(obra);
            db.SaveChanges();
            return true;
        }

        public void atualizarObra(int id, Obra obra)
        {
            var obraUpdate = (from e in db.Obra where e.IDO == id select e).SingleOrDefault();
            obraUpdate.Descricao = obra.Descricao;
            obraUpdate.Matriz = obra.Matriz;
            obraUpdate.CentroAdministrativo = obra.CentroAdministrativo;
            db.SaveChanges();
        }
    }
}