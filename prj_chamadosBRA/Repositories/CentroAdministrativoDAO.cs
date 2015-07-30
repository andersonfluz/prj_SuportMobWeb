using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class CentroAdministrativoDAO
    {
        ApplicationDbContext db;
        public CentroAdministrativoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public CentroAdministrativoDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<CentroAdministrativo> BuscarCentrosAdministrativos()
        {

            List<CentroAdministrativo> listCA = (from e in db.CentroAdministrativo select e).ToList();
            return listCA;
        }
    }
}