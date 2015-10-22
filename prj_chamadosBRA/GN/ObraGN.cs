using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.GN
{
    public class ObraGN
    {
        ApplicationDbContext db;

        public ObraGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Obra> obrasPorPerfil(int perfil)
        {
            var listObra = new List<Obra>();
            switch (perfil)
            {
                case 1: //Administrador
                    listObra = new ObraDAO(db).BuscarObras();
                    break;                
                default:
                    listObra = null;
                    break;
            }
            return listObra;
        }
    }
}