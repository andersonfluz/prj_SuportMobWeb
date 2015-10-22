using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.GN
{
    public class SetorGN
    {
        ApplicationDbContext db;

        public SetorGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Setor> setoresPorPerfil(int perfil, string idUser)
        {
            var listSetor = new List<Setor>();
            switch (perfil)
            {
                case 1: //Administrador
                    listSetor = new SetorDAO(db).BuscarSetores();
                    break;
                case 6: //Administrador Obra
                    var obras = new ObraDAO(db).BuscarObrasPorUsuario(idUser);
                    listSetor = new SetorDAO(db).BuscarSetoresPorObras(obras);
                    break;
                default:
                    listSetor = null;
                    break;
            }
            return listSetor;
        }
    }
}