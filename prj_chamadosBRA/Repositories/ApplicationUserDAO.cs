using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ApplicationUserDAO
    {
        ApplicationDbContext db;
        public ApplicationUserDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ApplicationUserDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public void atualizarApplicationUser(string id, ApplicationUser user)
        {
            ApplicationUser userUpdate = (from e in db.Users where e.Id == id select e).SingleOrDefault();
            userUpdate.Nome = user.Nome;
            userUpdate.UserName = user.UserName;
            userUpdate.PerfilUsuario = user.PerfilUsuario;
            userUpdate.Contato = user.Contato;
            db.SaveChanges();
        }

        public ApplicationUser retornarUsuario(string idUser)
        {
            ApplicationUser user = (from e in db.Users where e.Id == idUser select e).SingleOrDefault();
            return user;
        }

        public List<ApplicationUser> retornarUsuarios(string filtro)
        {            
            List<ApplicationUser> users = (from e in db.Users select e).ToList();
            if (users != null && filtro != null)
            {
                users = users.Where(s => s.UserName.ToLower().Contains(filtro.ToLower())
                                              || s.Nome.ToLower().Contains(filtro.ToLower())).ToList();
            }
            return users;
        }

        public List<ApplicationUser> retornarUsuariosSetor(Setor setor, string filtro)
        {
            List<String> userIds = (from e in db.UsuarioSetor where e.Setor == setor.Id select e.Usuario).ToList();
            List<ApplicationUser> users = (from e in db.Users where userIds.Contains(e.Id) select e).ToList();
            if (users != null && filtro != null)
            {
                users = users.Where(s => s.UserName.ToLower().Contains(filtro.ToLower())
                                              || s.Nome.ToLower().Contains(filtro.ToLower())).ToList();
            }
            return users;
        }

        public List<ApplicationUser> retornarUsuariosObra(int idObra, string filtro)
        {
            List<String> userIds = (from e in db.UsuarioObra where e.Obra == idObra select e.Usuario).ToList();
            List<ApplicationUser> users = (from e in db.Users where userIds.Contains(e.Id) select e).ToList();
            if (users != null && filtro != null)
            {
                users = users.Where(s => s.UserName.ToLower().Contains(filtro.ToLower())
                                              || s.Nome.ToLower().Contains(filtro.ToLower())).ToList();
            }
            return users;
        }

        
    }
}