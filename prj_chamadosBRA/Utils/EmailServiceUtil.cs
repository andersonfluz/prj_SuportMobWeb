using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Web;
using System.Web.Helpers;
using System.ComponentModel;

namespace prj_chamadosBRA.Utils
{
    public class EmailServiceUtil
    {
        #region Envio de Emails

        public static string envioEmailAberturaChamado(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamado.ResponsavelAberturaChamado.UserName));

                    if (chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        if (chamado.SetorDestino.SetorCorporativo != null)
                        {
                            var usuarios = new ApplicationUserDAO().retornarUsuariosSetorTipoChamado(new SetorDAO().BuscarSetorId(chamado.SetorDestino.SetorCorporativo.Value), chamado.TipoChamado.Value);

                            foreach (var usuario in usuarios)
                            {
                                //mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                                mail.CC.Add(usuario.UserName);
                            }

                        }
                        else
                        {
                            var usuarios = new ApplicationUserDAO().retornarUsuariosSetor(chamado.SetorDestino, null);

                            foreach (var usuario in usuarios)
                            {
                                //mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                                mail.CC.Add(usuario.UserName);
                            }
                        }
                    }
                    mail.Subject = "HelpMe! - Notificação Abertura Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAbertura(chamado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailDirecionamentoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.Chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.Chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.Chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.Chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "HelpMe! - Notificação Alteracao Chamado N. " + chamadoHistorico.Chamado.Id;
                    mail.Body = montarCorpoMensagemAlteracao(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailAtualizacaoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.Chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.Chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.Chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.Chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "HelpMe! - Notificação Alteracao Chamado N. " + chamadoHistorico.Chamado.Id;
                    mail.Body = montarCorpoMensagemAlteracao(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailEncerramentoChamado(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamado.ResponsavelAberturaChamado.UserName));
                    if (chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamado.ResponsavelChamado.UserName);
                    }
                    mail.Subject = "HelpMe! - Notificação Encerramento do Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemEncerramento(chamado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailCancelamentoChamado(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamado.ResponsavelAberturaChamado.UserName));
                    if (chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamado.ResponsavelChamado.UserName);
                    }
                    mail.Subject = "HelpMe! - Notificação Cancelamento do Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemCancelamento(chamado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailCriacaoUsuario(ApplicationUser user)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(user.UserName));
                    mail.Subject = "HelpMe! - Criação de novo usuario na plataforma";
                    mail.Body = montarCorpoMensagemCriacaoUsuario(user);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailCadastroUsuarioExterno(ApplicationUser user)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(user.UserName));
                    mail.Subject = "HelpMe! - Cadastro de novo usuário na plataforma";
                    mail.Body = montarCorpoMensagemCadastroUsuarioExterno(user);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailAberturaTarefa(Tarefa tarefa)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(tarefa.Responsavel.UserName));
                    mail.CC.Add(tarefa.Solicitante.UserName);
                    mail.Subject = "HelpMe! - Notificação Abertura de Tarefa N. " + tarefa.Id;
                    mail.Body = montarCorpoMensagemAberturaTarefa(tarefa);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailPrevisaoEntregaTarefa(Tarefa tarefa)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(tarefa.Solicitante.UserName));
                    mail.CC.Add(tarefa.Responsavel.UserName);
                    mail.Subject = "HelpMe! - Notificação Previsão de Entrega de Tarefa N. " + tarefa.Id;
                    mail.Body = montarCorpoMensagemPrevisaoEntregaTarefa(tarefa);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailEntregaTarefa(Tarefa tarefa)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(tarefa.Solicitante.UserName));
                    mail.CC.Add(tarefa.Responsavel.UserName);
                    mail.Subject = "HelpMe! - Notificação Entrega de Tarefa N. " + tarefa.Id;
                    mail.Body = montarCorpoMensagemEntregaTarefa(tarefa);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailRedefinicaoSenhaUsuario(ApplicationUser user, string IdCriptografado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(user.UserName));
                    mail.Subject = "HelpMe! - Redefinição de senha do usuario na plataforma";
                    mail.Body = montarCorpoMensagemReiniciarSenhaUsuario(user, IdCriptografado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailReaberturaChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.Chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.Chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.Chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.Chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.Chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "HelpMe! - Notificação Reabertura Chamado N. " + chamadoHistorico.Chamado.Id;
                    mail.Body = montarCorpoMensagemReabertura(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";

                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string envioEmailSemResponsavelTrintaMinutos(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    if (chamado.SetorDestino == null)
                    {
                        var usuarios = new ApplicationUserDAO().retornarUsuariosObra(chamado.ObraDestino.IDO, null);
                        foreach (var usuario in usuarios)
                        {
                            mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                            //mail.To.Add(new MailAddress(usuario.UserName));
                        }
                    }
                    else
                    {
                        if (chamado.SetorDestino.SetorCorporativo != null)
                        {
                            var usuarios = new ApplicationUserDAO().retornarUsuariosSetorTipoChamado(new SetorDAO().BuscarSetorId(chamado.SetorDestino.SetorCorporativo.Value), chamado.TipoChamado.Value);

                            foreach (var usuario in usuarios)
                            {
                                mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                                //mail.CC.Add(usuario.UserName);
                            }

                        }
                        else
                        {
                            var usuarios = new ApplicationUserDAO().retornarUsuariosSetor(chamado.SetorDestino, null);

                            foreach (var usuario in usuarios)
                            {
                                mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                                //mail.CC.Add(usuario.UserName);
                            }
                        }
                    }
                    mail.Subject = "HelpMe! - Alerta de chamado sem responsavel - Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAlertaSemResponsavel(chamado, "trinta minutos");
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemResponsavelUmaHora(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    if (chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                            //mail.To.Add(new MailAddress(setor.EmailSetor));
                        }
                    }
                    else
                    {
                        mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                        //mail.To.Add(new MailAddress(chamado.SetorDestino.EmailResponsavel));
                    }
                    mail.Subject = "HelpMe! - Alerta de chamado sem responsavel - Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAlertaSemResponsavel(chamado, "uma hora");
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemResponsavelDuasHoras(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    if (chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                            //mail.To.Add(new MailAddress(setor.EmailResponsavel));
                        }
                    }
                    else
                    {
                        mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                        //mail.To.Add(new MailAddress(chamado.SetorDestino.EmailResponsavel));
                    }
                    mail.Subject = "HelpMe! - Alerta de chamado sem responsavel - Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAlertaSemResponsavel(chamado, "duas horas");
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemAtualizacaoDoisDiasTrintaMinutos(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());
                    mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                    //mail.To.Add(new MailAddress(chamado.ResponsavelChamado.UserName));
                    mail.Subject = "HelpMe! - Alerta de chamado sem atualização - Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAlertaSemAtualizacao(chamado, "dois dias");
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemAtualizacaoDoisDiasUmaHora(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    if (chamado.ResponsavelChamado.EnvioEmailSuperior.Value)
                    {
                        mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                        mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                        //mail.To.Add(new MailAddress(new ApplicationUserDAO().retornarUsuario(chamado.ResponsavelChamado.Superior).UserName));
                        mail.Subject = "HelpMe! - Alerta de chamado sem atualização - Chamado N. " + chamado.Id;
                        mail.Body = montarCorpoMensagemAlertaSemAtualizacao(chamado, "dois dias");
                        mail.IsBodyHtml = true;
                        var smtpClient = new SmtpClient();
                        smtpClient.Send(mail);
                    }
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemAtualizacaoDoisDiasDuasHoras(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());
                    mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                    var superior = new ApplicationUserDAO().retornarUsuario(chamado.ResponsavelChamado.Superior);
                    var superiorDoSuperior = new ApplicationUserDAO().retornarUsuario(superior.Superior);
                    if (superiorDoSuperior.EnvioEmailSuperior.Value)
                    {
                        mail.To.Add(new MailAddress(superiorDoSuperior.UserName));
                        mail.Subject = "HelpMe! - Alerta de chamado sem atualização - Chamado N. " + chamado.Id;
                        mail.Body = montarCorpoMensagemAlertaSemAtualizacao(chamado, "dois dias");
                        mail.IsBodyHtml = true;
                        var smtpClient = new SmtpClient();
                        smtpClient.Send(mail);
                    }
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailSemRetornoSolicitanteUmaOuSeisHoras(Chamado chamado)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());
                    //mail.To.Add(new MailAddress(chamado.ResponsavelAberturaChamado.UserName));                    
                    mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                    mail.Subject = "HelpMe! - Alerta de chamado sem retorno do solicitante - Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAlertaSemRetornoSolicitante(chamado, "uma hora");
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public static string envioEmailTravados()
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());
                    mail.To.Add(new MailAddress("ti.anderson@cav-ba.com.br"));
                    mail.Subject = "HelpMe! - Alerta de emails travados";
                    mail.Body = montarCorpoMensagemEmailsTravados();
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    smtpClient.Send(mail);
                    return "0";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }



        #endregion

        #region Montagem de Corpo dos Emails

        private static string montarCorpoMensagemAbertura(Chamado chamado)
        {
            var mensagem = "Nova Solicitação N. " + chamado.Id;
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";


            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemEncerramento(Chamado chamado)
        {
            var mensagem = "Encerramento Solicitação N. " + chamado.Id;
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            var dataHoraAtendimento = chamado.DataHoraAtendimento.ToString();
            var classificacao = new ChamadoClassificacaoDAO().BuscarClassificacao(chamado.Classificacao.Value).Descricao;
            var subClassificacao = new ChamadoSubClassificacaoDAO().BuscarSubClassificacao(chamado.SubClassificacao.Value).Descricao;
            var objCript = new Criptografia();
            objCript["id"] = chamado.Id.ToString();
            var IdCriptografado = objCript.ToString();

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data/Hora Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + dataHoraAtendimento + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Classificação:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + classificacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "SubClassificação:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + subClassificacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solução:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + chamado.Solucao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Informação Importante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Caso não concorde com o encerramento do chamado, poderá solicitar dentro de 7 dias a reabertura do mesmo <a href='http://suporte.ani.org.br/ChamadosBRA/Chamado/ReaberturaChamado/?id=" + IdCriptografado + "'>aqui</a></td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemCancelamento(Chamado chamado)
        {
            var mensagem = "Cancelamento de Solicitação N. " + chamado.Id;
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = !chamado.Cancelado ? "Chamado Aberto" : "Chamado Cancelado";
            var dataHoraCancelamento = chamado.DataHoraCancelamento.ToString();
            var objCript = new Criptografia();
            objCript["id"] = chamado.Id.ToString();
            var IdCriptografado = objCript.ToString();

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data/Hora Cancelamento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + dataHoraCancelamento + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Justificativa de Cancelamento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + chamado.JustificativaCancelamento + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Informação Importante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Caso não concorde com o encerramento do chamado, poderá solicitar dentro de 7 dias a reabertura do mesmo <a href='http://suporte.ani.org.br/ChamadosBRA/Chamado/ReaberturaChamado/?id=" + IdCriptografado + "'>aqui</a></td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAlteracao(ChamadoHistorico chamadoHistorico)
        {
            var mensagem = "Alteração da Solicitação N. " + chamadoHistorico.Chamado.Id;
            var mensagemSetorObra = chamadoHistorico.Chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamadoHistorico.Chamado.ObraDestino.Descricao : chamadoHistorico.Chamado.SetorDestino.Descricao + " - " + chamadoHistorico.Chamado.ObraDestino.Descricao;
            var assunto = chamadoHistorico.Chamado.Assunto;
            var descricao = chamadoHistorico.Chamado.Descricao;
            var observacao = chamadoHistorico.Chamado.Observacao;
            var obra = chamadoHistorico.Chamado.ObraDestino.Descricao;
            var setorDestino = chamadoHistorico.Chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamadoHistorico.Chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamadoHistorico.Chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamadoHistorico.Chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamadoHistorico.Chamado.ResponsavelChamado.Nome;
            var statusChamado = chamadoHistorico.Chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            var observacoesInterna = chamadoHistorico.Chamado.ObsevacaoInterna == null ? "" : chamadoHistorico.Chamado.ObsevacaoInterna;
            var historico = chamadoHistorico.Historico;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em style='color:red;'>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Historico de Alteração:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top; color:red'><b>"
                                   + historico + "</b></td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemCriacaoUsuario(ApplicationUser user)
        {
            var corpoMensagem = "<div>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='color: rgb(0,0,0); font-family: verdana; font-size: 16pt'>"
                                + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='padding-right: 2px; padding-left: 2px'>"
                                + "ChamadosBRA - "
                                + "Criação de Usuario"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%; margin-top: 6px; border-bottom-color: rgb(156,163,173); border-bottom-width: 1px; border-bottom-style: solid'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td colspan='3'>"
                                + "Parabens " + user.Nome + " ! <br />"
                                + "<br />"
                                + "Seu acesso a ferramenta de chamados já está liberado. Para acessa-la siga as "
                                + "instruções abaixo: <br />"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. Abra seu navegador de internet e digite o endereço: "
                                + "<a href='http://portal.colegioantoniovieira.com.br/ChamadosBRA/'>http://portal.colegioantoniovieira.com.br/ChamadosBRA/</a>"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. No campo usuário digite seu email: " + user.UserName + "  <br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. E para o primeiro acesso, utilize a senha: 123456 <br />"
                                + "<br />"
                                + "Pronto! Já poderá utilizar a ferramenta. Sugerimos que altere imediatamente sua "
                                + "senha."
                                + "<br />"
                                + "<br />"
                                + "Equipe de suporte CAS.<br />"
                                + "&nbsp;</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemCadastroUsuarioExterno(ApplicationUser user)
        {
            var corpoMensagem = "<div>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='color: rgb(0,0,0); font-family: verdana; font-size: 16pt'>"
                                + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='padding-right: 2px; padding-left: 2px'>"
                                + "ChamadosBRA - "
                                + "Criação de Usuario"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%; margin-top: 6px; border-bottom-color: rgb(156,163,173); border-bottom-width: 1px; border-bottom-style: solid'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td colspan='3'>"
                                + "Parabens " + user.Nome + " ! <br />"
                                + "<br />"
                                + "Seu acesso a ferramenta de chamados já está liberado. Para acessa-la siga as "
                                + "instruções abaixo: <br />"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. Abra seu navegador de internet e digite o endereço: "
                                + "<a href='http://portal.colegioantoniovieira.com.br/ChamadosBRA/'>http://portal.colegioantoniovieira.com.br/ChamadosBRA/</a>"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. No campo usuário digite seu email: " + user.UserName + "  <br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. E utilize a senha informada no cadastro <br />"
                                + "<br />"
                                + "Pronto! Já poderá utilizar a ferramenta."
                                + "<br />"
                                + "<br />"
                                + "Equipe de suporte CAS.<br />"
                                + "&nbsp;</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemReiniciarSenhaUsuario(ApplicationUser user, string IdCriptografado)
        {
            var corpoMensagem = "<div>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='color: rgb(0,0,0); font-family: verdana; font-size: 16pt'>"
                                + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td style='padding-right: 2px; padding-left: 2px'>"
                                + "ChamadosBRA - "
                                + "Redefinição de Senha do Usuario"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<table cellspacing='0' cellpadding='0' style='width: 100%; margin-top: 6px; border-bottom-color: rgb(156,163,173); border-bottom-width: 1px; border-bottom-style: solid'>"
                                + "<tbody>"
                                + "<tr>"
                                + "<td colspan='3'>"
                                + "Prezado " + user.Nome + " ! <br />"
                                + "<br />"
                                + "Conforme solicitação clique no botão abaixo para recuperar sua senha: "
                                + "<br />"
                                + "<br />"
                                + "<br />"
                                + "<a href='http://suporte.ani.org.br/ChamadosBRA/Account/RedefinicaoSenha/?idCript=" + IdCriptografado + "' style='background: #B7181D;background-image: -webkit-linear-gradient(top, #B7181D, #c26e71);  background-image: -moz-linear-gradient(top, #B7181D, #c26e71);  background-image: -ms-linear-gradient(top, #B7181D, #c26e71);  background-image: -o-linear-gradient(top, #B7181D, #c26e71);  background-image: linear-gradient(to bottom, #B7181D, #c26e71);  -webkit-border-radius: 28;  -moz-border-radius: 28; border-radius: 28px;  font-family: Arial; color: #ffffff; font-size: 20px;  padding: 10px 20px 10px 20px;  text-decoration: none;'>Recupere sua Senha!</a>"
                                + "<br />"
                                + "<br />"
                                + "<br />"
                                + "Ou Copie o link Abaixo e cole no seu navegador: "
                                + "<br />"
                                + "http://suporte.ani.org.br/ChamadosBRA/Account/RedefinicaoSenha/?idCript=" + IdCriptografado
                                + "<br />"
                                + "<br />"
                                + "<br />"
                                + "Equipe de suporte CAS.<br />"
                                + "&nbsp;</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemReabertura(ChamadoHistorico chamadoHistorico)
        {
            var mensagem = "Reabertura da Solicitação N. " + chamadoHistorico.Chamado.Id;
            var mensagemSetorObra = chamadoHistorico.Chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamadoHistorico.Chamado.ObraDestino.Descricao : chamadoHistorico.Chamado.SetorDestino.Descricao + " - " + chamadoHistorico.Chamado.ObraDestino.Descricao;
            var assunto = chamadoHistorico.Chamado.Assunto;
            var descricao = chamadoHistorico.Chamado.Descricao;
            var observacao = chamadoHistorico.Chamado.Observacao;
            var obra = chamadoHistorico.Chamado.ObraDestino.Descricao;
            var setorDestino = chamadoHistorico.Chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamadoHistorico.Chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamadoHistorico.Chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamadoHistorico.Chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamadoHistorico.Chamado.ResponsavelChamado.Nome;
            var statusChamado = chamadoHistorico.Chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            var observacoesInterna = chamadoHistorico.Chamado.ObsevacaoInterna == null ? "" : chamadoHistorico.Chamado.ObsevacaoInterna;
            var historico = chamadoHistorico.Historico;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observações Internas:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacoesInterna + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Historico de Alteração:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + historico + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAlertaSemResponsavel(Chamado chamado, string tempo)
        {
            var mensagem = "Alerta solicitação N. " + chamado.Id + " Sem responsavel para atendimento";
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";


            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Mensagem de Alerta:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "A solicitação está sem responsavel a mais de " + tempo + ", favor verificar e direcionar o chamado.</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAlertaSemAtualizacao(Chamado chamado, string tempo)
        {
            var mensagem = "Alerta solicitação N. " + chamado.Id + " sem atualização do andamento";
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";


            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Mensagem de Alerta:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "A solicitação está sem atualização a mais de " + tempo + ", favor verificar e dar andamento no chamado.</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAlertaSemRetornoSolicitante(Chamado chamado, string tempo)
        {
            var mensagem = "Alerta solicitação N. " + chamado.Id + " sem retorno do solicitante";
            var mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            var assunto = chamado.Assunto;
            var descricao = chamado.Descricao;
            var observacao = chamado.Observacao;
            var obra = chamado.ObraDestino.Descricao;
            var setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            var statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";


            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Mensagem de Alerta:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "A solicitação está sem retorno a mais de " + tempo + ", favor verificar e dar retorno ao chamado.</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Observacao:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + observacao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + obra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel Atendimento:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + responsavelChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemEmailsTravados()
        {
            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Mensagem de Alerta:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Algum(ns) Email(s) estão travados no envio, favor verificar.</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAberturaTarefa(Tarefa tarefa)
        {
            var mensagem = "<b>Nova Tarefa</b> N. " + tarefa.Id + " do Chamado N. " + tarefa.Chamado.Id;
            var mensagemSetorObra = tarefa.Chamado.ObraDestino.Descricao;
            var setorDestino = tarefa.Chamado.SetorDestino.Descricao;
            var assunto = tarefa.Assunto;
            var descricao = tarefa.Descricao;
            var Solicitante = tarefa.Solicitante.Nome;
            var Responsavel = tarefa.Responsavel.Nome;
            var statusChamado = !tarefa.StatusTarefa ? "Tarefa Aberta" : "Tarefa Fechada";
            var DataHoraAbertura = tarefa.DataAbertura.ToString();
            var Natureza = tarefa.Natureza.Descricao;
            var SubNatureza = tarefa.SubNatureza.Descricao;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'><b>"
                                   + descricao + "</b></td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Abertura:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Natureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Natureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "SubNatureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + SubNatureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + mensagemSetorObra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Solicitante + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top color:red'><b>"
                                   + Responsavel + "</b></td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemPrevisaoEntregaTarefa(Tarefa tarefa)
        {
            var mensagem = "Previsão de Entrega da Tarefa N. " + tarefa.Id + " do Chamado N. " + tarefa.Chamado.Id;
            var mensagemSetorObra = tarefa.Chamado.ObraDestino.Descricao;
            var setorDestino = tarefa.Chamado.SetorDestino.Descricao;
            var assunto = tarefa.Assunto;
            var descricao = tarefa.Descricao;
            var Solicitante = tarefa.Solicitante.Nome;
            var Responsavel = tarefa.Responsavel.Nome;
            var statusChamado = !tarefa.StatusTarefa ? "Tarefa Aberta" : "Tarefa Fechada";
            var DataHoraAbertura = tarefa.DataAbertura.ToString();
            var DataHoraPrevisaoEntrega = tarefa.DataPrevisaoEntrega.ToString();
            var Natureza = tarefa.Natureza.Descricao;
            var SubNatureza = tarefa.SubNatureza.Descricao;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Abertura:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Previsão de Entrega:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top; color:blue'>"
                                   + DataHoraPrevisaoEntrega + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Natureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Natureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "SubNatureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + SubNatureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + mensagemSetorObra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Solicitante + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Responsavel + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemEntregaTarefa(Tarefa tarefa)
        {
            var mensagem = "Entrega da Tarefa N. " + tarefa.Id + " do Chamado N. " + tarefa.Chamado.Id;
            var mensagemSetorObra = tarefa.Chamado.ObraDestino.Descricao;
            var setorDestino = tarefa.Chamado.SetorDestino.Descricao;
            var assunto = tarefa.Assunto;
            var descricao = tarefa.Descricao;
            var Solicitante = tarefa.Solicitante.Nome;
            var Responsavel = tarefa.Responsavel.Nome;
            var statusChamado = !tarefa.StatusTarefa ? "Tarefa Aberta" : "Tarefa Fechada";
            var DataHoraAbertura = tarefa.DataAbertura.ToString();
            var DataHoraPrevisaoEntrega = tarefa.DataPrevisaoEntrega.ToString();
            var DataHoraEntrega = tarefa.DataEntrega.ToString();
            var Natureza = tarefa.Natureza.Descricao;
            var SubNatureza = tarefa.SubNatureza.Descricao;
            var Solucao = tarefa.Solucao;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Abertura:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Previsao de Entrega:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraPrevisaoEntrega + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Entrega:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top; color:blue'>"
                                   + DataHoraEntrega + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Natureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Natureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "SubNatureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + SubNatureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solução:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Solucao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + mensagemSetorObra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Solicitante + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Responsavel + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static string montarCorpoMensagemAprovacaoTarefa(Tarefa tarefa)
        {
            var mensagem = "Entrega da Tarefa N. " + tarefa.Id + " do Chamado N. " + tarefa.Chamado.Id;
            var mensagemSetorObra = tarefa.Chamado.ObraDestino.Descricao;
            var setorDestino = tarefa.Chamado.SetorDestino.Descricao;
            var assunto = tarefa.Assunto;
            var descricao = tarefa.Descricao;
            var Solicitante = tarefa.Solicitante.Nome;
            var Responsavel = tarefa.Responsavel.Nome;
            var statusChamado = !tarefa.StatusTarefa ? "Tarefa Aberta" : "Tarefa Fechada";
            var AprovacaoTarefa = !tarefa.Aprovado.Value ? "Tarefa Aprovada" : "Tarefa Não Aprovada";
            var DataHoraAbertura = tarefa.DataAbertura.ToString();
            var DataHoraPrevisaoEntrega = tarefa.DataPrevisaoEntrega.ToString();
            var DataHoraEntrega = tarefa.DataEntrega.ToString();
            var Natureza = tarefa.Natureza.Descricao;
            var SubNatureza = tarefa.SubNatureza.Descricao;
            var Justificativa = tarefa.Justificativa;

            var corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='color:rgb(0,0,0); font-family:verdana; font-size:16pt'>"
                                   + "<table width='100%' class='x_breadcrumb' cellspacing='0' cellpadding='0'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td style='padding-right:2px; padding-left:2px'>" + mensagemSetorObra + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<em>" + mensagem + "</em></td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<table cellspacing='0' cellpadding='0' style='width:100%; margin-top:6px; border-bottom-color:rgb(156,163,173); border-bottom-width:1px; border-bottom-style:solid'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td colspan='3'>"
                                   + "<table width='100%' cellspacing='0' cellpadding='0' style='padding:3px 3px 6px; border:1px solid rgb(232,234,236); background-color:rgb(248,248,249)'>"
                                   + "<tbody>"
                                   + "<tr>"
                                   + "<td>"
                                   + "<table border='0' cellspacing='0' cellpadding='0'>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td colspan='3' style='height:10px; line-height:1px; font-size:1px'>&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Assunto:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + assunto + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Descrição:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + descricao + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Abertura:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraAbertura + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Previsao de Entrega:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + DataHoraPrevisaoEntrega + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Data da Entrega:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top; color:blue'>"
                                   + DataHoraEntrega + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Natureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Natureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "SubNatureza:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + SubNatureza + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Aprovado:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + AprovacaoTarefa + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Justificativa:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Justificativa + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Obra:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + mensagemSetorObra + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Setor:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + setorDestino + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Solicitante:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Solicitante + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Responsavel:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + Responsavel + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "<tr>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + "Status:</td>"
                                   + "<td style='padding:0px 7px; font-family:tahoma; font-size:8pt; vertical-align:top'>"
                                   + statusChamado + "</td>"
                                   + "<td style='text-align:left; padding-right:8px; padding-left:5px; font-family:tahoma,sans-serif; font-size:8pt; font-weight:normal; text-decoration:none; vertical-align:top'>"
                                   + "&nbsp;</td>"
                                   + "</tr>"
                                   + "</tbody>"
                                   + "</table>"
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do HelpMe!</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - HelpMe!</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        #endregion
    }
}