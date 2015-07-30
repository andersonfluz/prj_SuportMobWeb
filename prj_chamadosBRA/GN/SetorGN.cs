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
            List<Setor> listSetor = new List<Setor>();
            switch (perfil)
            {
                case 1: //Administrador
                    listSetor = new SetorDAO(db).BuscarSetores();
                    break;
                case 6: //Administrador Obra
                    List<Obra> obras = new ObraDAO(db).BuscarObrasPorUsuario(idUser);
                    listSetor = new SetorDAO(db).BuscarSetoresPorObra(obras[0].IDO);
                    break;
                default:
                    listSetor = null;
                    break;
            }
            return listSetor;
        }
    }
}