using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.GN
{
    public class ApplicationUserGN
    {
        ApplicationDbContext db;

        public ApplicationUserGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<ApplicationUser> usuariosPorPerfil(int perfil, string idUser, string filtro)
        {
            var listUsers = new List<ApplicationUser>();
            switch (perfil)
            {
                case 1: //Administrador
                    listUsers = new ApplicationUserDAO(db).retornarUsuarios(filtro);
                    break;
                case 5: //Gestor
                    var user = new ApplicationUserDAO(db).retornarUsuario(idUser);
                    var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                    listUsers = new ApplicationUserDAO(db).retornarUsuariosSetores(setores, filtro);
                    break;
                case 6: //Administrador Obra
                    var obras = new ObraDAO(db).BuscarObrasPorUsuario(idUser);
                    listUsers = new ApplicationUserDAO(db).retornarUsuariosObras(obras, filtro);
                    break;
                default:
                    listUsers = null;
                    break;
            }
            return listUsers;
        }
    }
}