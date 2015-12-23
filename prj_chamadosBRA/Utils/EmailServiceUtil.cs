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
        public async Task envioEmailAberturaChamadoAsync(Chamado chamado)
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
                        mail.CC.Add(chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "ChamadosBRA - Notificação Abertura Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemAbertura(chamado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {

            }

        }

        public async Task envioEmailDirecionamentoChamadoAsync(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                    mail.Body = montarCorpoMensagemAlteracao(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task envioEmailAtualizacaoChamadoAsync(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                    mail.Body = montarCorpoMensagemAlteracao(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {

            }
        }

        public async Task envioEmailEncerramentoChamado(Chamado chamado)
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
                    mail.Subject = "ChamadosBRA - Notificação Encerramento do Chamado N. " + chamado.Id;
                    mail.Body = montarCorpoMensagemEncerramento(chamado);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task envioEmailCriacaoUsuario(ApplicationUser user)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(user.UserName));
                    mail.Subject = "ChamadosBRA - Criação de novo usuario na plataforma";
                    mail.Body = montarCorpoMensagemCriacaoUsuario(user);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
            }
        }

        public async Task envioEmailRedefinicaoSenhaUsuario(ApplicationUser user)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(user.UserName));
                    mail.Subject = "ChamadosBRA - Redefinição de senha do usuario na plataforma";
                    mail.Body = montarCorpoMensagemReiniciarSenhaUsuario(user);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task envioEmailReaberturaChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["emailDe"].ToString());

                    mail.To.Add(new MailAddress(chamadoHistorico.chamado.ResponsavelAberturaChamado.UserName));
                    if (chamadoHistorico.chamado.ResponsavelChamado != null)
                    {
                        mail.CC.Add(chamadoHistorico.chamado.ResponsavelChamado.UserName);
                    }
                    else if (chamadoHistorico.chamado.SetorDestino == null)
                    {
                        var setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.chamado.ObraDestino.IDO);
                        foreach (var setor in setores)
                        {
                            mail.CC.Add(setor.EmailSetor);
                        }
                    }
                    else
                    {
                        mail.CC.Add(chamadoHistorico.chamado.SetorDestino.EmailSetor);
                    }
                    mail.Subject = "ChamadosBRA - Notificação Reabertura Chamado N. " + chamadoHistorico.chamado.Id;
                    mail.Body = montarCorpoMensagemReabertura(chamadoHistorico);
                    mail.IsBodyHtml = true;
                    var smtpClient = new SmtpClient();
                    await smtpClient.SendMailAsync(mail);
                }
            }
            catch (Exception e)
            {
            }
        }


        private static String montarCorpoMensagemAbertura(Chamado chamado)
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
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemEncerramento(Chamado chamado)
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
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemAlteracao(ChamadoHistorico chamadoHistorico)
        {
            var mensagem = "Alteração da Solicitação N. " + chamadoHistorico.chamado.Id;
            var mensagemSetorObra = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamadoHistorico.chamado.ObraDestino.Descricao : chamadoHistorico.chamado.SetorDestino.Descricao + " - " + chamadoHistorico.chamado.ObraDestino.Descricao;
            var assunto = chamadoHistorico.chamado.Assunto;
            var descricao = chamadoHistorico.chamado.Descricao;
            var observacao = chamadoHistorico.chamado.Observacao;
            var obra = chamadoHistorico.chamado.ObraDestino.Descricao;
            var setorDestino = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamadoHistorico.chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamadoHistorico.chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamadoHistorico.chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamadoHistorico.chamado.ResponsavelChamado.Nome;
            var statusChamado = chamadoHistorico.chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            var observacoesInterna = chamadoHistorico.chamado.ObsevacaoInterna == null ? "" : chamadoHistorico.chamado.ObsevacaoInterna;
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
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemCriacaoUsuario(ApplicationUser user)
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
                                + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemReiniciarSenhaUsuario(ApplicationUser user)
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
                                + "Prezado " + user.Nome + " ! <br />"
                                + "<br />"
                                + "Sua senha foi redefinida com sucesso: "
                                + "instruções abaixo: <br />"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. Abra seu navegador de internet e digite o endereço: "
                                + "<a href='http://portal.colegioantoniovieira.com.br/ChamadosBRA/'>http://portal.colegioantoniovieira.com.br/ChamadosBRA/</a>"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. No campo usuário digite seu email: " + user.UserName + "  <br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 3. E utilize a senha: 123456 <br />"
                                + "<br />"
                                + "Pronto! Já poderá voltar a utilizar a ferramenta. Sugerimos que altere imediatamente sua "
                                + "senha."
                                + "<br />"
                                + "<br />"
                                + "Equipe de suporte CAS.<br />"
                                + "&nbsp;</td>"
                                + "</tr>"
                                + "</tbody>"
                                + "</table>"
                                + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemReabertura(ChamadoHistorico chamadoHistorico)
        {
            var mensagem = "Reabertura da Solicitação N. " + chamadoHistorico.chamado.Id;
            var mensagemSetorObra = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamadoHistorico.chamado.ObraDestino.Descricao : chamadoHistorico.chamado.SetorDestino.Descricao + " - " + chamadoHistorico.chamado.ObraDestino.Descricao;
            var assunto = chamadoHistorico.chamado.Assunto;
            var descricao = chamadoHistorico.chamado.Descricao;
            var observacao = chamadoHistorico.chamado.Observacao;
            var obra = chamadoHistorico.chamado.ObraDestino.Descricao;
            var setorDestino = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamadoHistorico.chamado.SetorDestino.Descricao;
            var responsavelAbertura = chamadoHistorico.chamado.ResponsavelAberturaChamado.Nome;
            var responsavelChamado = chamadoHistorico.chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamadoHistorico.chamado.ResponsavelChamado.Nome;
            var statusChamado = chamadoHistorico.chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            var observacoesInterna = chamadoHistorico.chamado.ObsevacaoInterna == null ? "" : chamadoHistorico.chamado.ObsevacaoInterna;
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
                                   + "<p>Por favor não responda essa mensagem. Esse é um e-mail automático do Chamados BRA.</p>"
                                   + "</br><p>&copy;" + @DateTime.Now.Year + " - Chamados BRA</p>"
                                   + "</div>";
            return corpoMensagem;
        }
    }
}