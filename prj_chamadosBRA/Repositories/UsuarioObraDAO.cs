using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioObraDAO
    {
        ApplicationDbContext db;
        public UsuarioObraDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public UsuarioObraDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Obra> buscarObrasDoUsuario(ApplicationUser user)
        {

            var obras = (from o in db.Obra
                         join uo in db.UsuarioObra on o equals uo.Obra
                         where uo.Usuario == user.Id
                         select o).ToList();
            return obras;
        }

        public UsuarioObra buscarUsuarioObra(ApplicationUser user, int idObra)
        {
            var obraParameter = new ObraDAO().BuscarObraId(idObra);
            var obra = (from uo in db.UsuarioObra
                        where uo.Usuario == user.Id && uo.Obra == obraParameter
                        select uo).SingleOrDefault();
            return obra;
        }

        public List<UsuarioObra> buscarUsuariosObras(ApplicationUser user)
        {

            var obra = (from uo in db.UsuarioObra
                        where uo.Usuario == user.Id
                        select uo).ToList();
            return obra;
        }

        public UsuarioObra buscarUsuarioObraPorId(int idusuarioobra)
        {

            var obra = (from uo in db.UsuarioObra
                        where uo.idUsuarioObra == idusuarioobra
                        select uo).SingleOrDefault();
            return obra;
        }


        public bool salvarUsuarioObra(UsuarioObra usuarioObra)
        {
            if (!existObraUsuario(new ApplicationUserDAO(db).retornarUsuario(usuarioObra.Usuario), usuarioObra.Obra.IDO))
            {
                db.UsuarioObra.Add(usuarioObra);
                db.SaveChanges();
            }
            return true;
        }

        public bool removerUsuarioObra(UsuarioObra usuarioObra)
        {
            db.UsuarioObra.Remove(usuarioObra);
            db.SaveChanges();
            return true;
        }

        public bool existObraUsuario(ApplicationUser user, int idObra)
        {
            var obras = (from uo in db.UsuarioObra
                         where uo.Usuario == user.Id && uo.Obra.IDO == idObra
                         select uo).ToList();
            return obras.Count > 0 ? true : false;
        }
    }
}