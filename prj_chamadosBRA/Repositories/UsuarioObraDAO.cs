using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioObraDAO
    {
        ApplicationDbContext db;
        public UsuarioObraDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public UsuarioObraDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Obra> buscarObrasDoUsuario(ApplicationUser user)
        {

            List<Obra> obras = (from o in db.Obra
                                join uo in db.UsuarioObra on o.IDO equals uo.Obra.IDO
                                where uo.Usuario.Id == user.Id
                                select o).ToList();
            return obras;
        }


        public Boolean salvarUsuarioObra(UsuarioObra usuarioObra)
        {            
            db.UsuarioObra.Add(usuarioObra);
            db.SaveChanges();
            return true;
        }
    }
}