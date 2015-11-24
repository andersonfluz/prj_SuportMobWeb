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
                var perfis = (from e in ctx.PerfilUsuario select e).ToList();
                return perfis;
            }
        }

        public PerfilUsuario BuscarPerfil(Int32 idPerfil)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var perfil = (from e in ctx.PerfilUsuario where e.IdPerfil == idPerfil select e).SingleOrDefault();
                return perfil;
            }
        }

        public static List<PerfilUsuario> BuscarPerfisParaGestor()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var perfis = (from e in ctx.PerfilUsuario where e.IdPerfil == 3 || e.IdPerfil == 4 select e).ToList();
                return perfis;
            }
        }

        public static List<PerfilUsuario> BuscarPerfisParaAdmObra()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var perfis = (from e in ctx.PerfilUsuario where e.IdPerfil == 3 || e.IdPerfil == 4 || e.IdPerfil == 5 select e).ToList();
                return perfis;
            }
        }

    }
}