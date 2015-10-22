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

            var listCA = (from e in db.CentroAdministrativo select e).ToList();
            return listCA;
        }

        public CentroAdministrativo BuscarCentroAdministrativo(int idCA)
        {

            var centroAdm = (from e in db.CentroAdministrativo where e.idCA == idCA select e).SingleOrDefault();
            return centroAdm;
        }

        public Boolean salvarCentroAdministrativo(CentroAdministrativo centroAdministrativo)
        {
            db.CentroAdministrativo.Add(centroAdministrativo);
            db.SaveChanges();
            return true;
        }
    }
}