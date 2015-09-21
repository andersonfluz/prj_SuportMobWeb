using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;

namespace prj_chamadosBRA.Utils
{
    public static class EmailServiceUtil
    {


        public static async Task envioEmailAberturaChamado(Chamado chamado)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = chamado.ResponsavelAberturaChamado.UserName;
                string copia = "";
                if (chamado.SetorDestino == null)
                {
                    List<Setor> setores = new SetorDAO().BuscarSetoresPorObra(chamado.ObraDestino.IDO);
                    foreach (var setor in setores)
                    {
                        copia = copia + setor.EmailSetor + ";";
                    }
                }
                else
                {
                    copia = chamado.SetorDestino.EmailSetor;
                }
                string assunto = "ChamadosBRA - Notificação Abertura Chamado N. " + chamado.Id;
                string corpoMensagem = montarCorpoMensagemAbertura(chamado);
                WebMail.Send(para, assunto, corpoMensagem, null, copia);
            }
            catch
            {
            }
        }

        public static async Task<bool> envioEmailDirecionamentoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = chamadoHistorico.chamado.ResponsavelAberturaChamado.UserName;
                string copia = "";
                if (chamadoHistorico.chamado.ResponsavelChamado != null)
                {
                    copia = chamadoHistorico.chamado.ResponsavelChamado.UserName;
                }
                else if (chamadoHistorico.chamado.SetorDestino == null)
                {
                    List<Setor> setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.chamado.ObraDestino.IDO);
                    foreach (var setor in setores)
                    {
                        copia = copia + setor.EmailSetor + ";";
                    }
                }
                else
                {
                    copia = chamadoHistorico.chamado.SetorDestino.EmailSetor;
                }
                string assunto = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                string corpoMensagem = montarCorpoMensagemAlteracao(chamadoHistorico);
                WebMail.Send(para, assunto, corpoMensagem, null, copia);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> envioEmailAtualizacaoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = chamadoHistorico.chamado.ResponsavelAberturaChamado.UserName;
                string copia = "";
                if (chamadoHistorico.chamado.ResponsavelChamado != null)
                {
                    copia = chamadoHistorico.chamado.ResponsavelChamado.UserName;
                }
                else if (chamadoHistorico.chamado.SetorDestino == null)
                {
                    List<Setor> setores = new SetorDAO().BuscarSetoresPorObra(chamadoHistorico.chamado.ObraDestino.IDO);
                    foreach (var setor in setores)
                    {
                        copia = copia + setor.EmailSetor + ";";
                    }
                }
                else
                {
                    copia = chamadoHistorico.chamado.SetorDestino.EmailSetor;
                }
                string assunto = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                string corpoMensagem = montarCorpoMensagemAlteracao(chamadoHistorico);
                WebMail.Send(para, assunto, corpoMensagem, null, copia);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> envioEmailEncerramentoChamado(Chamado chamado)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = chamado.ResponsavelAberturaChamado.UserName;
                string copia = null;
                if (chamado.ResponsavelChamado != null)
                {
                    copia = chamado.ResponsavelChamado.UserName;
                }
                string assunto = "ChamadosBRA - Notificação Encerramento do Chamado N. " + chamado.Id;
                string corpoMensagem = montarCorpoMensagemEncerramento(chamado);
                WebMail.Send(para, assunto, corpoMensagem, null, copia);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> envioEmailCriacaoUsuario(ApplicationUser user)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = user.UserName;
                string assunto = "ChamadosBRA - Criação de novo usuario na plataforma";
                string corpoMensagem = montarCorpoMensagemCriacaoUsuario(user);
                WebMail.Send(para, assunto, corpoMensagem, null);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public static async Task<bool> envioEmailRedefinicaoSenhaUsuario(ApplicationUser user)
        {
            try
            {
                WebMail.SmtpServer = "smtp.office365.com";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "notify@cav-ba.com.br";
                WebMail.From = "notify@cav-ba.com.br";
                WebMail.Password = "Notificacoes2013";

                string para = user.UserName;
                string assunto = "ChamadosBRA - Redefinição de senha do usuario na plataforma";
                string corpoMensagem = montarCorpoMensagemReiniciarSenhaUsuario(user);
                WebMail.Send(para, assunto, corpoMensagem, null);
                return true;
            }
            catch
            {
                throw;
            }
        }

        private static String montarCorpoMensagemAbertura(Chamado chamado)
        {
            string mensagem = "Nova Solicitação N. " + chamado.Id;
            string mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            string assunto = chamado.Assunto;
            string descricao = chamado.Descricao;
            string observacao = chamado.Observacao;
            string obra = chamado.ObraDestino.Descricao;
            string setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            string responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            string responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            string statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";


            string corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
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
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemEncerramento(Chamado chamado)
        {
            string mensagem = "Encerramento Solicitação N. " + chamado.Id;
            string mensagemSetorObra = chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamado.ObraDestino.Descricao : chamado.SetorDestino.Descricao + " - " + chamado.ObraDestino.Descricao;
            string assunto = chamado.Assunto;
            string descricao = chamado.Descricao;
            string observacao = chamado.Observacao;
            string obra = chamado.ObraDestino.Descricao;
            string setorDestino = chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamado.SetorDestino.Descricao;
            string responsavelAbertura = chamado.ResponsavelAberturaChamado.Nome;
            string responsavelChamado = chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamado.ResponsavelChamado.Nome;
            string statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            string dataHoraAtendimento = chamado.DataHoraAtendimento.ToString();
            string classificacao = new ChamadoClassificacaoDAO().BuscarClassificacao(chamado.Classificacao.Value).Descricao;
            string subClassificacao = new ChamadoSubClassificacaoDAO().BuscarSubClassificacao(chamado.SubClassificacao.Value).Descricao;

            string corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
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
                                   + "</tbody>"
                                   + "</table>"
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemAlteracao(ChamadoHistorico chamadoHistorico)
        {
            string mensagem = "Alteração da Solicitação N. " + chamadoHistorico.chamado.Id;
            string mensagemSetorObra = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado - " + chamadoHistorico.chamado.ObraDestino.Descricao : chamadoHistorico.chamado.SetorDestino.Descricao + " - " + chamadoHistorico.chamado.ObraDestino.Descricao;
            string assunto = chamadoHistorico.chamado.Assunto;
            string descricao = chamadoHistorico.chamado.Descricao;
            string observacao = chamadoHistorico.chamado.Observacao;
            string obra = chamadoHistorico.chamado.ObraDestino.Descricao;
            string setorDestino = chamadoHistorico.chamado.SetorDestino == null ? "Sem Setor Direcionado" : chamadoHistorico.chamado.SetorDestino.Descricao;
            string responsavelAbertura = chamadoHistorico.chamado.ResponsavelAberturaChamado.Nome;
            string responsavelChamado = chamadoHistorico.chamado.ResponsavelChamado == null ? "Sem Responsavel Direcionado" : chamadoHistorico.chamado.ResponsavelChamado.Nome;
            string statusChamado = chamadoHistorico.chamado.StatusChamado == false ? "Chamado Aberto" : "Chamado Fechado";
            string observacoesInterna = chamadoHistorico.chamado.ObsevacaoInterna == null ? "" : chamadoHistorico.chamado.ObsevacaoInterna;
            string historico = chamadoHistorico.Historico;

            string corpoMensagem = "<div> <table cellspacing='0' cellpadding='0' style='width:100%'>"
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
                                   + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemCriacaoUsuario(ApplicationUser user)
        {
            string corpoMensagem = "<div>"
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
                                + "Parabens "+user.Nome+" ! <br />"
                                + "<br />"
                                + "Seu acesso a ferramenta de chamados já está liberado. Para acessa-la siga as "
                                + "instruções abaixo: <br />"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 1. Abra seu navegador de internet e digite o endereço: "
                                + "<a href='http://portal.colegioantoniovieira.com.br/ChamadosBRA/'>http://portal.colegioantoniovieira.com.br/ChamadosBRA/</a>"
                                + "<br />"
                                + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 2. No campo usuário digite seu email: "+ user.UserName +"  <br />"
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
                                + "</div>";
            return corpoMensagem;
        }

        private static String montarCorpoMensagemReiniciarSenhaUsuario(ApplicationUser user)
        {
            string corpoMensagem = "<div>"
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
                                + "</div>";
            return corpoMensagem;
        }
    }
}