using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;

namespace prj_chamadosBRA.Service
{
    public class AlertaSemRetornoSolicitante
    {
        public static void AlertaSemRetornoUmaOuSeisHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemRetorno = new ChamadoDAO(db).BuscarChamadosSemRetornoPorUmaOuSeisHoras();
                foreach (var chamado in chamadosSemRetorno)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemRetornoSolicitanteUmaOuSeisHoras
                    });
                    //Alerta de Chamado de Uma Hora
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(10),
                        Texto = "Alerta de Chamado Sem Retorno Uma ou Seis Horas",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
    }
}