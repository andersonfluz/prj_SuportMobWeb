using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ObraDAO
    {
        public List<Obra> BuscarObras()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<Obra> obras = (from e in ctx.Obra select e).ToList();
                return obras;
            }
        }

        public List<Obra> BuscarObrasPorUsuario(ApplicationUser user)
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<int> idObras = (from e in ctx.UsuarioObra where e.Usuario.Id == user.Id select e.idObra).ToList();
                List<Obra> obras = (from o in ctx.Obra where idObras.Contains(o.IDO) select o).ToList();
                return obras;
            }
        }
    }
}