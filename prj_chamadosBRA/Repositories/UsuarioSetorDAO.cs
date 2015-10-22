using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioSetorDAO
    {
        ApplicationDbContext db;
        public UsuarioSetorDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public UsuarioSetorDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Setor> buscarSetoresDoUsuario(ApplicationUser user)
        {
            var setores = (from o in db.Setor
                                   join us in db.UsuarioSetor on o.Id equals us.Setor
                                   where us.Usuario == user.Id
                                   select o).ToList();
            return setores;
        }


        public bool salvarUsuarioSetor(UsuarioSetor usuarioSetor)
        {
            db.UsuarioSetor.Add(usuarioSetor);
            db.SaveChanges();
            return true;
        }

        internal bool existSetorUsuario(ApplicationUser user, int idSetores)
        {
            var setores = (from uo in db.UsuarioSetor
                         where uo.Usuario == user.Id && uo.Setor == idSetores
                           select uo).ToList();
            return setores.Count > 0 ? true : false;
        }
    }
}