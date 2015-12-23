using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class UsuarioSetorDAO
    {
        ApplicationDbContext db;
        public UsuarioSetorDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public UsuarioSetorDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Setor> buscarSetoresDoUsuario(ApplicationUser user)
        {
            var setores = (from o in db.Setor
                           join us in db.UsuarioSetor on o equals us.Setor
                           where us.Usuario == user.Id
                           select o).ToList();
            return setores;
        }

        public List<Setor> buscarSetoresCorporativosDoUsuario(ApplicationUser user)
        {
            var setores = (from o in db.Setor
                           join us in db.UsuarioSetor on o equals us.Setor
                           where us.Usuario == user.Id && o.SetorCorporativo != null
                           select o).ToList();
            return setores;
        }

        public List<Setor> buscarSetoresCorporativosPrincipaisVinculadosAoUsuario(ApplicationUser user)
        {
            var idsetores = (from o in db.Setor
                           join us in db.UsuarioSetor on o equals us.Setor
                           where us.Usuario == user.Id && o.SetorCorporativo != null
                           select o.SetorCorporativo).ToList();
            var setorescorporativos = (from o in db.Setor where idsetores.Contains(o.Id) select o).ToList();
            return setorescorporativos;
        }

        public UsuarioSetor buscarUsuarioSetorPorId(int idusuariosetor)
        {

            var setor = (from uo in db.UsuarioSetor
                        where uo.idUsuarioSetor == idusuariosetor
                        select uo).SingleOrDefault();
            return setor;
        }

        public List<UsuarioObra> buscarUsuarioObradeSetoresCorporativosDoUsuario(ApplicationUser user)
        {

            var setorcorporativo = (from s in db.Setor
                                    join us in db.UsuarioSetor on s.Id equals us.Setor.Id
                                    where us.Usuario == user.Id && s.SetorCorporativo != null
                                    select s.SetorCorporativo).ToList();
            //var usuariossetores = (from o in db.UsuarioObra
            //                       join us in db.UsuarioSetor on o.Obra equals us.Setor.obra
            //                       join s in db.Setor on us.Setor equals s
            //                       where s.SetorCorporativo == setorcorporativo && us.Setor == s
            //                       group o by new
            //                       {
            //                           o.idUsuarioObra,
            //                           o.Obra,
            //                           o.Usuario,
            //                       } into gcs
            //                       select new UsuarioObra
            //                       {
            //                           idUsuarioObra = gcs.Key.idUsuarioObra,
            //                           Obra = gcs.Key.Obra,
            //                           Usuario = gcs.Key.Usuario
            //                       });

            var usuariossetores = (from o in db.UsuarioObra
                                   join us in db.UsuarioSetor on o.Obra equals us.Setor.obra
                                   join s in db.Setor on us.Setor equals s
                                   where setorcorporativo.Contains(s.SetorCorporativo) && us.Setor == s && us.Usuario == o.Usuario
                                   group o by new { o.idUsuarioObra, o.Obra, o.Usuario }
                                   into grp
                                   select new
                                   {
                                       idUsuarioObra = grp.Key.idUsuarioObra,
                                       Obra = grp.Key.Obra,
                                       Usuario = grp.Key.Usuario
                                   }).ToList().Select(c => new UsuarioObra
                                   {
                                       idUsuarioObra = c.idUsuarioObra,
                                       Obra = c.Obra,
                                       Usuario = c.Usuario
                                   }).ToList();
            return usuariossetores;
        }

        public List<UsuarioSetor> buscarUsuariosSetores(ApplicationUser user)
        {

            var usuariosetores = (from uo in db.UsuarioSetor
                                  where uo.Usuario == user.Id
                                  select uo).ToList();
            return usuariosetores;
        }

        public bool salvarUsuarioSetor(UsuarioSetor usuarioSetor)
        {
            if (!existSetorUsuario(new ApplicationUserDAO(db).retornarUsuario(usuarioSetor.Usuario), usuarioSetor.Setor.Id))
            {
                db.UsuarioSetor.Add(usuarioSetor);
                db.SaveChanges();
            }
            return true;
        }

        public bool removerUsuarioSetor(UsuarioSetor usuarioSetor)
        {
            db.UsuarioSetor.Remove(usuarioSetor);
            db.SaveChanges();
            return true;
        }

        internal bool existSetorUsuario(ApplicationUser user, int idSetores)
        {
            var setores = (from uo in db.UsuarioSetor
                           where uo.Usuario == user.Id && uo.Setor.Id == idSetores
                           select uo).ToList();
            return setores.Count > 0 ? true : false;
        }
    }
}