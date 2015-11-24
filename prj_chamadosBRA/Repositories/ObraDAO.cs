using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ObraDAO
    {
        ApplicationDbContext db;
        public ObraDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public ObraDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public List<Obra> BuscarObras()
        {

            var obras = (from e in db.Obra where e.Ativo select e).ToList();
            return obras;
        }

        public Obra BuscarObraId(int ido)
        {

            var obra = (from e in db.Obra where e.IDO == ido && e.Ativo select e).SingleOrDefault();
            return obra;
        }

        public List<Obra> BuscarObrasPorUsuario(String userId)
        {
            var Obras = (from e in db.UsuarioObra where e.Usuario == userId select e.Obra).Where(e => e.Ativo).ToList();            
            return Obras;
        }

        public List<Obra> BuscarObrasSetoresCorporativos(Setor setorCorporativo)
        {
            var SetoresCoporativos = (from s in db.Setor where s.SetorCorporativo == setorCorporativo.SetorCorporativo select s).ToList();
            var Obras = new List<Obra>();
            foreach(var setor in SetoresCoporativos)
            {
                Obras.Add(setor.obra);
            }
            return Obras;
        }

        public Boolean salvarObra(Obra obra)
        {
            db.Obra.Add(obra);
            db.SaveChanges();
            return true;
        }

        public bool desativarObra(Obra obra, string usuario)
        {
            obra.DataAlteracao = DateTime.Now;
            obra.Usuario = usuario;
            obra.Ativo = false;
            this.atualizarObra(obra.IDO, obra, obra.Usuario);
            return true;
        }

        public void atualizarObra(int id, Obra obra, string usuario)
        {
            var obraUpdate = (from e in db.Obra where e.IDO == id select e).SingleOrDefault();
            obraUpdate.Descricao = obra.Descricao;
            obraUpdate.Usuario = usuario;
            obraUpdate.DataAlteracao = DateTime.Now;
            obraUpdate.Matriz = obra.Matriz;
            obraUpdate.CentroAdministrativo = obra.CentroAdministrativo;
            db.SaveChanges();
        }
    }
}