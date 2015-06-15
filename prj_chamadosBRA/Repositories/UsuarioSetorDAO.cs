using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioSetorDAO
    {
        public List<Setor> buscarSetoresDoUsuario(ApplicationUser user)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Setor> setores = (from o in ctx.Setor
                                    join us in ctx.UsuarioSetor on o.Id equals us.idSetor
                                    where us.Usuario.Id == user.Id
                                    select o).ToList();
                return setores;
            }
        }


        public Boolean salvarUsuarioSetor(ApplicationUser user, int idSetor)
        {
            using (var ctx = new ApplicationDbContext())
            {


                UsuarioSetor usuarioSetor = new UsuarioSetor();
                usuarioSetor.Usuario = user;
                usuarioSetor.idSetor = idSetor;
                ctx.UsuarioSetor.Add(usuarioSetor);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}