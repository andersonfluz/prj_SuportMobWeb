using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using prj_chamadosBRA.Utils;
using System;

namespace prj_chamadosBRA.Service
{
    public class AlertaSemResponsavel
    {
        public static void AlertaTrintaMinutos()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemResponsavel = new ChamadoDAO(db).BuscarChamadosSemResponsaveisPorTrintaMinutos();
                foreach (var chamado in chamadosSemResponsavel)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemResponsavelTrintaMinutos
                    });
                    //Alerta de Chamado de trinta minutos
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(4),
                        Texto = "Alerta de Chamado Sem Responsavel Trinta Minutos",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
        public static void AlertaUmaHora()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemResponsavel = new ChamadoDAO(db).BuscarChamadosSemResponsaveisPorUmaHora();
                foreach (var chamado in chamadosSemResponsavel)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemResponsavelUmaHora
                    });
                    EmailServiceUtil.envioEmailSemResponsavelUmaHora(chamado);
                    //Alerta de Chamado de Uma Hora
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(5),
                        Texto = "Alerta de Chamado Sem Responsavel Uma Hora",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }

        }
        public static void AlertaDuasHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemResponsavel = new ChamadoDAO(db).BuscarChamadosSemResponsaveisPorDuasHoras();
                foreach (var chamado in chamadosSemResponsavel)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemResponsavelDuasHoras
                    });
                    //Alerta de Chamado de Duas Horas
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(6),
                        Texto = "Alerta de Chamado Sem Responsavel Duas Hora",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
    }
}