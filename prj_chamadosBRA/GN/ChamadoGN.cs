using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using prj_chamadosBRA.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace prj_chamadosBRA.GN
{
    public class ChamadoGN
    {
        ApplicationDbContext db;

        public ChamadoGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Setor> RetornarSetoresPorObra(string idObra)
        {
            return new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(idObra));
        }

        public async Task<bool> registrarChamado(Chamado chamado, HttpPostedFileBase upload, String SetorDestino, String ObraDestino, String ResponsavelAberturaChamado, ApplicationUser user)
        {
            try
            {
                ChamadoDAO cDAO = new ChamadoDAO(db);
                ObraDAO oDAO = new ObraDAO(db);
                SetorDAO sDAO = new SetorDAO(db);
                ApplicationUserDAO aDAO = new ApplicationUserDAO(db);
                Setor setor;
                Obra obra;
                chamado.DataHoraAbertura = DateTime.Now;
                chamado.StatusChamado = false;
                if (chamado.TipoChamado == null)
                {
                    chamado.TipoChamado = 2;
                }
                if (user != null)
                {
                    chamado.ResponsavelCriacaoChamado = user;
                    if (ResponsavelAberturaChamado != null)
                    {
                        if (ResponsavelAberturaChamado != "")
                        {
                            chamado.ResponsavelAberturaChamado = (aDAO.retornarUsuario(ResponsavelAberturaChamado));
                        }
                        else
                        {
                            chamado.ResponsavelAberturaChamado = user;
                        }
                    }
                    else
                    {
                        chamado.ResponsavelAberturaChamado = user;
                    }

                }
                if (SetorDestino != null)
                {
                    setor = sDAO.BuscarSetorId(Int32.Parse(SetorDestino));
                    chamado.SetorDestino = setor;
                }

                if (ObraDestino != null)
                {
                    obra = oDAO.BuscarObraId(Int32.Parse(ObraDestino));
                    chamado.ObraDestino = obra;
                }
                else
                {
                    obra = oDAO.BuscarObrasPorUsuario(user.Id)[0];
                    chamado.ObraDestino = obra;
                }

                if (upload != null && upload.ContentLength > 0)
                {
                    ChamadoAnexoDAO caDAO = new ChamadoAnexoDAO(db);
                    var anexo = new ChamadoAnexo
                    {
                        NomeAnexo = System.IO.Path.GetFileName(upload.FileName),
                        ContentType = upload.ContentType
                    };
                    using (var reader = new System.IO.BinaryReader(upload.InputStream))
                    {
                        anexo.arquivoAnexo = reader.ReadBytes(upload.ContentLength);
                    }
                    chamado.Anexos = new List<ChamadoAnexo> { anexo };
                }
                cDAO.salvarChamado(chamado);
                //await Task.Run(() => EmailService.envioEmailAberturaChamado(chamado));
                await EmailServiceUtil.envioEmailAberturaChamado(chamado);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public ChamadoHistorico registrarHistorico(DateTime dataHora, ApplicationUser responsavel, String Historico, Chamado chamado)
        {
            try
            {
                ChamadoHistorico ch = new ChamadoHistorico();
                ch.chamado = chamado;
                ch.Data = dataHora;
                ch.Hora = dataHora;
                ch.Responsavel = responsavel;
                ch.Historico = Historico;
                new ChamadoHistoricoDAO(db).salvarHistorico(ch);
                return ch;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> atualizarChamado(int id, Chamado chamado, String SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento, ApplicationUser responsavel)
        {
            ChamadoDAO cDAO = new ChamadoDAO(db);
            ChamadoGN cGN = new ChamadoGN(db);
            SetorDAO sDAO = new SetorDAO(db);
            ApplicationUserDAO aDAO = new ApplicationUserDAO(db);
            Chamado chamadoOrigem = cDAO.BuscarChamadoId(id);
            ChamadoHistorico chamadoHistorico;


            if (chamadoOrigem.TipoChamado != chamado.TipoChamado)
            {
                chamadoOrigem.TipoChamado = chamado.TipoChamado;
                cDAO.atualizarChamado(id, chamadoOrigem);
                if (chamado.TipoChamado == 1)
                {
                    chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Tipo de Chamado foi alterado para Totvs RM", chamadoOrigem);
                }
                else if (chamado.TipoChamado == 2)
                {
                    chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Tipo de Chamado foi alterado para Outros", chamadoOrigem);
                }
            }

            //Atualização de Setor
            if (chamadoOrigem.SetorDestino != null && SetorDestino != null)
            {
                if (chamadoOrigem.SetorDestino.Id != Convert.ToInt32(SetorDestino))
                {
                    Setor setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                    chamadoOrigem.SetorDestino = setor;
                    chamadoOrigem.ResponsavelChamado = null;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                    await EmailServiceUtil.envioEmailDirecionamentoChamado(chamadoHistorico);
                }
            }
            else if (chamadoOrigem.SetorDestino == null && SetorDestino != null)
            {
                Setor setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                chamadoOrigem.SetorDestino = setor;
                cDAO.atualizarChamado(id, chamadoOrigem);
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                await EmailServiceUtil.envioEmailDirecionamentoChamado(chamadoHistorico);
            }

            //Atualização de Responsavel pelo Chamado
            if (chamadoOrigem.ResponsavelChamado != null && ddlResponsavelChamado != null)
            {
                if (chamadoOrigem.ResponsavelChamado.Id != ddlResponsavelChamado)
                {
                    if (ddlResponsavelChamado == "-1")
                    {
                        chamadoOrigem.ResponsavelChamado = null;
                        cDAO.atualizarChamado(id, chamadoOrigem);
                    }
                    else
                    {
                        ApplicationUser user = aDAO.retornarUsuario(ddlResponsavelChamado);
                        chamadoOrigem.ResponsavelChamado = user;
                        cDAO.atualizarChamado(id, chamadoOrigem);
                        chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                        await EmailServiceUtil.envioEmailDirecionamentoChamado(chamadoHistorico);

                    }
                }
            }
            else if (chamadoOrigem.ResponsavelChamado == null && ddlResponsavelChamado != "")
            {
                ApplicationUser user = aDAO.retornarUsuario(ddlResponsavelChamado);
                chamadoOrigem.ResponsavelChamado = user;
                cDAO.atualizarChamado(id, chamadoOrigem);
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                await EmailServiceUtil.envioEmailDirecionamentoChamado(chamadoHistorico);
            }

            if (informacoesAcompanhamento == null || informacoesAcompanhamento != "")
            {
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, informacoesAcompanhamento, chamadoOrigem);
                await EmailServiceUtil.envioEmailDirecionamentoChamado(chamadoHistorico);
            }
            if (chamadoOrigem.ObsevacaoInterna != chamado.ObsevacaoInterna)
            {
                chamadoOrigem.ObsevacaoInterna = chamado.ObsevacaoInterna;
                cDAO.atualizarChamado(id, chamadoOrigem);
            }
            return true;
        }
    }
}