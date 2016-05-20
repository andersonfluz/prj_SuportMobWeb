using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace prj_chamadosBRA.Controllers
{
    public class PainelBordoController : ApiController
    {
        public ApplicationUserManager UserManager
        {
            get { return HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
        }

        public string UserIdentityId
        {
            get
            {
                var user = UserManager.FindById(User.Identity.GetUserId());
                return user.Id;
            }
        }


        public IEnumerable<ChamadoViewBI> Get(int parametro)
        {
            return Chamados();
           //switch (parametro)
           // {
           //     case 0:
           //         return Chamados();
           //     case 1:
           //         return ChamadosHoje();
           //     case 2:
           //         return ChamadosHojeTecnico();
           //     default:
           //         return Chamados();
           // }
        }

        public IEnumerable<ChamadoViewBI> Chamados()
        {
            var db = new ApplicationDbContext();
            var chamados = new ChamadoDAO(db).BuscarChamadosTIView();            
            return chamados;
        }

        public IEnumerable<ChamadoInfoViewModel> ChamadosHoje()
        {
            var db = new ApplicationDbContext();
            var chamados = new ChamadoDAO(db).BuscarChamadosAbertosHoje();
            var chamadosVM = new List<ChamadoInfoViewModel>();
            foreach (var chamado in chamados)
            {
                chamadosVM.Add(new ChamadoInfoViewModel
                {
                    Id = chamado.Id,
                    Assunto = chamado.Assunto,
                    DataHoraAbertura = chamado.DataHoraAbertura
                });
            }
            return chamadosVM;
        }

        public IEnumerable<ChamadoInfoViewModel> ChamadosHojeTecnico()
        {
            var db = new ApplicationDbContext();
            var chamados = new ChamadoDAO(db).BuscarChamadosAbertosHoje(UserIdentityId);
            var chamadosVM = new List<ChamadoInfoViewModel>();
            foreach (var chamado in chamados)
            {
                chamadosVM.Add(new ChamadoInfoViewModel
                {
                    Id = chamado.Id,
                    Assunto = chamado.Assunto,
                    DataHoraAbertura = chamado.DataHoraAbertura
                });
            }
            return chamadosVM;
        }
    }
}
