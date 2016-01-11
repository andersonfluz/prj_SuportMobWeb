using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;

namespace prj_chamadosBRA.Service
{
    public class AlertaSemAtualizacaoInterno
    {
        public static void AlertaDoisDiasTrintaMinutos()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemAtualizacao = new ChamadoDAO(db).BuscarChamadosSemAtualizaoPorDoisDiasTrintaMinutos();
                foreach (var chamado in chamadosSemAtualizacao)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemAtualizacaoUmDiaTrintaMinutos
                    });
                    //Alerta de Chamado Um dia e trinta minutos
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(7),
                        Texto = "Alerta de Chamado Sem Atualizacao por um Dia e Trinta Minutos",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
        public static void AlertaDoisDiasUmaHora()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemAtualizacao = new ChamadoDAO(db).BuscarChamadosSemAtualizacaoPorDoisDiasUmaHora();
                foreach (var chamado in chamadosSemAtualizacao)
                {
                    //Enfileirar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemAtualizacaoUmDiaUmaHora
                    });
                    //Alerta de Chamado de Uma Hora
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(8),
                        Texto = "Alerta de Chamado Sem Atualizacao Um dia e Uma Hora",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
        public static void AlertaDoisDiasDuasHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var chamadosSemAtualizacao = new ChamadoDAO(db).BuscarChamadosSemAtualizacaoPorDoisDiasDuasHoras();
                foreach (var chamado in chamadosSemAtualizacao)
                {
                    //Enviar email de alerta
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AlertaSemAtualizacaoUmDiaDuasHoras
                    });
                    //Alerta de Chamado de Duas Horas
                    new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                    {
                        IdChamado = chamado.Id,
                        ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(9),
                        Texto = "Alerta de Chamado Sem Atualizacao Por Um Dia e Duas Horas",
                        DataAcao = DateTime.Now,
                        UsuarioAcao = new ApplicationUserDAO(db).retornarUsuario("49d2a731-bcf5-429a-bd1b-c973963d87da")
                    });
                }
            }
        }
    }
}