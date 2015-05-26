using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class PerfilUsuarioDAO
    {
        public List<PerfilUsuario> BuscarPerfis()
        {
            using (var ctx = new ApplicationDbContext())
            {
                List<PerfilUsuario> perfis = (from e in ctx.PerfilUsuario select e).ToList();
                return perfis;
            }
        }
    }
}