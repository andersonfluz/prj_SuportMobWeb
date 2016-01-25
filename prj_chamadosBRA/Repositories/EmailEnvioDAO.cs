using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class EmailEnvioDAO
    {
        ApplicationDbContext db;
        public EmailEnvioDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public EmailEnvioDAO()
        {
            this.db = new ApplicationDbContext();
        }

        public EmailEnvio BuscarEmailEnvioId(int Id)
        {

            var envioEmail = (from e in db.EmailEnvio where e.Id == Id select e).SingleOrDefault();
            return envioEmail;
        }

        public List<EmailEnvio> BuscarEmailEnvioTipo(int EmailTipo)
        {

            var envioEmail = (from e in db.EmailEnvio where e.IdTipoEmail == EmailTipo select e).ToList();
            return envioEmail;
        }

        public void atualizarEmailEnvio(EmailEnvio emailEnvio)
        {
            var emailEnvioUpdate = (from e in db.EmailEnvio where e.Id == emailEnvio.Id select e).SingleOrDefault();
            emailEnvioUpdate.Tentativas = emailEnvio.Tentativas;
            emailEnvioUpdate.Erro = emailEnvio.Erro;
            db.SaveChanges();
        }


        public void salvarEmailEnvio(EmailEnvio emailEnvio)
        {
            db.EmailEnvio.Add(emailEnvio);
            db.SaveChanges();
        }

        public void eliminarEmailEnvio(EmailEnvio emailEnvio)
        {
            var envioemail = (from e in db.EmailEnvio where emailEnvio.Id == e.Id select e).SingleOrDefault();
            if (envioemail != null)
            {
                db.EmailEnvio.Remove(emailEnvio);
                db.SaveChanges();
            }
        }

    }
}