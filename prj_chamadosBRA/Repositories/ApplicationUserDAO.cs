﻿using prj_chamadosBRA.Models;
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

        public List<ApplicationUser> retornarUsuariosSetor(Setor setor)
        {
            List<String> userIds = (from e in db.UsuarioSetor where e.Setor == setor.Id select e.Usuario).ToList();
            List<ApplicationUser> users = (from e in db.Users where userIds.Contains(e.Id) select e).ToList(); 
            return users;
        }

        
    }
}