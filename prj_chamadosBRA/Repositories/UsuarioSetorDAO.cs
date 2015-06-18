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
            List<Setor> setores = (from o in db.Setor
                                   join us in db.UsuarioSetor on o.Id equals us.Setor.Id
                                   where us.Usuario.Id == user.Id
                                   select o).ToList();
            return setores;
        }


        public Boolean salvarUsuarioSetor(UsuarioSetor usuarioSetor)
        {
            db.UsuarioSetor.Add(usuarioSetor);
            db.SaveChanges();
            return true;
        }
    }
}