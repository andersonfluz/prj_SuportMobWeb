using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using prj_chamadosBRA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Service
{
    public class EmailEnvioSender
    {
        public static void EnvioEmailAberturaChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AberturaChamado);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailAberturaChamado(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailDirecionamentoChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.DirecionamentoChamado);
                foreach (var email in emailEnvio)
                {
                    var historicoChamado = new ChamadoHistoricoDAO(db).buscarHistoricosPorId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailDirecionamentoChamado(historicoChamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAtualizacaoChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AtualizacaoChamado);
                foreach (var email in emailEnvio)
                {
                    var historicoChamado = new ChamadoHistoricoDAO(db).buscarHistoricosPorId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailAtualizacaoChamado(historicoChamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailEncerramentoChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.EncerramentoChamado);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailEncerramentoChamado(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailCancelamentoChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.CancelamentoChamado);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailCancelamentoChamado(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailReaberturaChamados()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.ReaberturaChamado);
                foreach (var email in emailEnvio)
                {
                    var historicoChamado = new ChamadoHistoricoDAO(db).buscarHistoricosPorId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailReaberturaChamado(historicoChamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailCriacaoUsuario()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.CriacaoUsuario);
                foreach (var email in emailEnvio)
                {
                    var User = new ApplicationUserDAO(db).retornarUsuario(email.InfoEmail);
                    var retorno = EmailServiceUtil.envioEmailCriacaoUsuario(User);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailRedefinicaoSenhaUsuario()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.RedefinicaoSenhaUsuario);
                foreach (var email in emailEnvio)
                {
                    var user = new ApplicationUserDAO(db).retornarUsuario(email.InfoEmail);
                    var objCript = new Criptografia();
                    objCript["id"] = user.Id.ToString();
                    var IdCriptografado = objCript.ToString();
                    var retorno = EmailServiceUtil.envioEmailRedefinicaoSenhaUsuario(user, IdCriptografado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAberturaTarefa()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AberturaTarefa);
                foreach (var email in emailEnvio)
                {
                    var tarefa = new TarefaDAO(db).BuscarTarefaId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailAberturaTarefa(tarefa);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailPrevisaoEntregaTarefa()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.PrevisaoEntregaTarefa);
                foreach (var email in emailEnvio)
                {
                    var tarefa = new TarefaDAO(db).BuscarTarefaId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailPrevisaoEntregaTarefa(tarefa);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailEntregaTarefa()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.EntregaTarefa);
                foreach (var email in emailEnvio)
                {
                    var tarefa = new TarefaDAO(db).BuscarTarefaId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailEntregaTarefa(tarefa);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemResponsavelTrintaMinutos()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemResponsavelTrintaMinutos);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemResponsavelTrintaMinutos(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemResponsavelUmaHora()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemResponsavelUmaHora);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemResponsavelUmaHora(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemResponsavelDuasHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemResponsavelDuasHoras);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemResponsavelDuasHoras(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemAtualizacaoDoisDiasTrintaMinutos()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemAtualizacaoDoisDiasTrintaMinutos);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemAtualizacaoDoisDiasTrintaMinutos(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemAtualizacaoDoisDiasUmaHora()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemAtualizacaoDoisDiasUmaHora);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemAtualizacaoDoisDiasUmaHora(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemAtualizacaoDoisDiasDuasHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemAtualizacaoDoisDiasDuasHoras);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemAtualizacaoDoisDiasDuasHoras(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailAlertaSemRetornoSolicitanteUmaOuSeisHoras()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.AlertaSemRetornoSolicitanteUmaOuSeisHoras);
                foreach (var email in emailEnvio)
                {
                    var chamado = new ChamadoDAO(db).BuscarChamadoId(Convert.ToInt32(email.InfoEmail));
                    var retorno = EmailServiceUtil.envioEmailSemRetornoSolicitanteUmaOuSeisHoras(chamado);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
        public static void EnvioEmailCadastroUsuarioExterno()
        {
            using (var db = new ApplicationDbContext())
            {
                var emailEnvio = new EmailEnvioDAO(db).BuscarEmailEnvioTipo((int)EmailTipo.EmailTipos.CadastroUsuarioExterno);
                foreach (var email in emailEnvio)
                {
                    var User = new ApplicationUserDAO(db).retornarUsuario(email.InfoEmail);
                    var retorno = EmailServiceUtil.envioEmailCadastroUsuarioExterno(User);
                    if (retorno == "0")
                    {
                        new EmailEnvioDAO(db).eliminarEmailEnvio(email);
                    }
                    else
                    {
                        email.Tentativas = email.Tentativas + 1;
                        email.Erro = retorno;
                        new EmailEnvioDAO(db).atualizarEmailEnvio(email);
                    }
                }
            }
        }
    }
}