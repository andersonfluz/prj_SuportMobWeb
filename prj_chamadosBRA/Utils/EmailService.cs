using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace prj_chamadosBRA.Utils
{
    public class EmailService
    {
        public EmailService()
        {
            WebMail.SmtpServer = "smtp.office365.com";
            WebMail.SmtpPort = 587;
            WebMail.EnableSsl = true;
            WebMail.UserName = "notify@cav-ba.com.br";
            WebMail.From = "notify@cav-ba.com.br";
            WebMail.Password = "Notificacoes2013";
        }

        public Boolean envioEmailAberturaChamado(Chamado chamado)
        {
            try
            {
                //string para = chamado.ResponsavelAberturaChamado.UserName;
                string para = "andersonfluz@outlook.com";
                string assunto = "ChamadosBRA - Notificação Abertura Chamado N. " + chamado.Id;
                string corpoMensagem = montarCorpoMensagemAbertura(chamado);
                WebMail.Send(para, assunto, corpoMensagem);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public Boolean envioEmailDirecionamentoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                //string para = chamado.ResponsavelAberturaChamado.UserName;
                string para = "andersonfluz@outlook.com";
                string assunto = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                string corpoMensagem = montarCorpoMensagemAlteracao(chamadoHistorico);
                WebMail.Send(para, assunto, corpoMensagem);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public Boolean envioEmailAtualizacaoChamado(ChamadoHistorico chamadoHistorico)
        {
            try
            {
                //string para = chamado.ResponsavelAberturaChamado.UserName;
                string para = "andersonfluz@outlook.com";
                string assunto = "ChamadosBRA - Notificação Alteracao Chamado N. " + chamadoHistorico.chamado.Id;
                string corpoMensagem = montarCorpoMensagemAlteracao(chamadoHistorico);
                WebMail.Send(para, assunto, corpoMensagem);
                return true;
            }
            catch
            {
                throw;
            }
        }

        public String montarCorpoMensagemAbertura(Chamado chamado)
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
            string statusChamado = chamado.StatusChamado == null || chamado.StatusChamado == false ? "Chamado Fechado" : "Chamado Aberto";

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


        public String montarCorpoMensagemAlteracao(ChamadoHistorico chamadoHistorico)
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
            string statusChamado = chamadoHistorico.chamado.StatusChamado == null || false ? "Chamado Fechado" : "Chamado Aberto";
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
    }
}