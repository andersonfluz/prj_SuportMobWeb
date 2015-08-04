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

            List<Obra> obras = (from e in db.Obra select e).ToList();
            return obras;
        }

        public Obra BuscarObraId(int ido)
        {

            Obra obra = (from e in db.Obra where e.IDO == ido select e).SingleOrDefault();
            return obra;
        }

        public List<Obra> BuscarObrasPorUsuario(String userId)
        {
            List<int> idObras = (from e in db.UsuarioObra where e.Usuario == userId select e.Obra).ToList();
            List<Obra> obras = (from o in db.Obra where idObras.Contains(o.IDO) select o).ToList();
            return obras;
        }

        public Boolean salvarObra(Obra obra)
        {
            db.Obra.Add(obra);
            db.SaveChanges();
            return true;
        }
    }
}