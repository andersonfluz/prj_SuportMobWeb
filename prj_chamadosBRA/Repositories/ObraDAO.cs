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
    }
}