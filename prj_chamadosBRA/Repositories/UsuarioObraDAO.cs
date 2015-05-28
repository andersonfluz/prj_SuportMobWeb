using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioObraDAO
    {
        public List<Obra> buscarObrasDoUsuario(ApplicationUser user)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Obra> obras = (from o in ctx.Obra
                                    join uo in ctx.UsuarioObra on o.IDO equals uo.idObra
                                    where uo.Usuario.Id == user.Id
                                    select o).ToList();
                return obras;
            }
        }


        public Boolean salvarUsuarioObra(ApplicationUser user, int idObra)
        {
            using (var ctx = new ApplicationDbContext())
            {


                UsuarioObra usuarioObra = new UsuarioObra();
                usuarioObra.Usuario = user;
                usuarioObra.idObra = idObra;
                ctx.UsuarioObra.Add(usuarioObra);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}