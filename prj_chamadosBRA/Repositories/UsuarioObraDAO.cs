using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioObraDAO
    {
        public Boolean salvarUsuarioObra(String idUser, int idObra)
        {
            using (var ctx = new ApplicationDbContext())
            {
                UsuarioObra usuarioObra = new UsuarioObra();
                usuarioObra.idUsuario = idUser;
                usuarioObra.idObra = idObra;
                ctx.UsuarioObra.Add(usuarioObra);
                ctx.SaveChanges();
                return true;
            }
        }
    }
}