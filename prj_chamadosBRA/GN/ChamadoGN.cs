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

        public static List<Setor> RetornarSetoresPorObra(string idObra)
        {
            return new SetorDAO().BuscarSetoresPorObra(Convert.ToInt32(idObra));
        }

        public Chamado registrarChamado(Chamado chamado, HttpPostedFileBase upload, String SetorDestino, String ObraDestino, String ResponsavelAberturaChamado, ApplicationUser user)
        {
            try
            {
                var cDAO = new ChamadoDAO(db);
                var oDAO = new ObraDAO(db);
                var sDAO = new SetorDAO(db);
                var aDAO = new ApplicationUserDAO(db);
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
                    var caDAO = new ChamadoAnexoDAO(db);
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
                new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                {
                    IdChamado = chamado.Id,
                    ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(1),
                    Texto = "Chamado Aberto",
                    DataAcao = DateTime.Now,
                    UsuarioAcao = user

                });
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamado.Id.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.AberturaChamado
                });
                return chamado;
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public ChamadoHistorico registrarHistorico(DateTime dataHora, ApplicationUser responsavel, String Historico, Chamado chamado)
        {
            try
            {
                var ch = new ChamadoHistorico
                {
                    Chamado = chamado,
                    Data = dataHora,
                    Hora = dataHora,
                    Responsavel = responsavel,
                    Historico = Historico
                };
                new ChamadoHistoricoDAO(db).salvarHistorico(ch);
                return ch;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> atualizarChamadoHistorico(int id, string informacoesAcompanhamento, ApplicationUser responsavel)
        {
            try
            {
                var chamadoHistorico = new ChamadoGN(db).registrarHistorico(DateTime.Now, responsavel, informacoesAcompanhamento, new ChamadoDAO(db).BuscarChamadoId(id));
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> atualizarChamado(int id, Chamado chamado, String SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento, ApplicationUser responsavel)
        {
            var cDAO = new ChamadoDAO(db);
            var cGN = new ChamadoGN(db);
            var sDAO = new SetorDAO(db);
            var aDAO = new ApplicationUserDAO(db);
            var chamadoOrigem = cDAO.BuscarChamadoId(id);
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
                    var setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                    chamadoOrigem.SetorDestino = setor;
                    chamadoOrigem.ResponsavelChamado = null;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                    });
                }
            }
            else if (chamadoOrigem.SetorDestino == null && SetorDestino != null)
            {
                var setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                chamadoOrigem.SetorDestino = setor;
                cDAO.atualizarChamado(id, chamadoOrigem);
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                });
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
                        var user = aDAO.retornarUsuario(ddlResponsavelChamado);
                        chamadoOrigem.ResponsavelChamado = user;
                        cDAO.atualizarChamado(id, chamadoOrigem);
                        chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                        new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                        {
                            InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                            Data = DateTime.Now,
                            IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                        });

                    }
                }
            }
            else if (chamadoOrigem.ResponsavelChamado == null && ddlResponsavelChamado != "")
            {
                var user = aDAO.retornarUsuario(ddlResponsavelChamado);
                chamadoOrigem.ResponsavelChamado = user;
                cDAO.atualizarChamado(id, chamadoOrigem);
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                });
            }

            if (informacoesAcompanhamento == null || informacoesAcompanhamento != "")
            {
                chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, informacoesAcompanhamento, chamadoOrigem);
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                });
            }
            if (chamadoOrigem.ObsevacaoInterna != chamado.ObsevacaoInterna)
            {
                chamadoOrigem.ObsevacaoInterna = chamado.ObsevacaoInterna;
                cDAO.atualizarChamado(id, chamadoOrigem);
            }
            return true;
        }

        public async Task<bool> reaberturaChamado(int id, string justificativaReabertura, ApplicationUser responsavel)
        {
            try
            {
                var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
                chamado.StatusChamado = false;
                chamado.Cancelado = false;
                new ChamadoDAO(db).atualizarChamado(id, chamado);
                justificativaReabertura = "O chamado encerrado em: "+ chamado.DataHoraBaixa.ToString() + " foi reaberto. Justificativa: " + justificativaReabertura;
                var chamadoHistorico = new ChamadoGN(db).registrarHistorico(DateTime.Now, responsavel, justificativaReabertura, new ChamadoDAO(db).BuscarChamadoId(id));
                new ChamadoLogAcaoDAO(db).salvar(new ChamadoLogAcao
                {
                    IdChamado = chamado.Id,
                    ChamadoAcao = new ChamadoAcaoDAO(db).buscarChamadoAcaoPorId(3),
                    Texto = "Chamado Reaberto",
                    DataAcao = DateTime.Now,
                    UsuarioAcao = responsavel

                });
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.ReaberturaChamado
                });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool registarAnexo(int idChamado, HttpPostedFileBase upload)
        {
            var caDAO = new ChamadoAnexoDAO(db);
            var anexo = new ChamadoAnexo
            {
                NomeAnexo = System.IO.Path.GetFileName(upload.FileName),
                ContentType = upload.ContentType
            };
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                anexo.arquivoAnexo = reader.ReadBytes(upload.ContentLength);
            }
            anexo.Chamado = new ChamadoDAO(db).BuscarChamadoId(idChamado);
            return caDAO.salvarChamadoAnexo(anexo);
        }
    }
}