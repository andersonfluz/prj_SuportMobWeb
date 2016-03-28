using prj_chamadosBRA.Models;
using prj_chamadosBRA.Repositories;
using System;
using System.Collections.Generic;
using System.Web;

namespace prj_chamadosBRA.GN
{
    public class ChamadoGN
    {
        ApplicationDbContext db;

        public ChamadoGN(ApplicationDbContext db)
        {
            this.db = db;
        }

        public Chamado buscarChamadoId(int Id)
        {
            return new ChamadoDAO(db).BuscarChamadoId(Id);
        }

        public static List<Setor> RetornarSetoresPorObra(string idObra)
        {
            return new SetorDAO().BuscarSetoresPorObraAtendimento(Convert.ToInt32(idObra));
        }

        public List<Chamado> GestaoChamados(string tipoChamado, string filtro, string sortOrder, ApplicationUser user)
        {
            //var user = manager.FindById(User.Identity.GetUserId());
            //Usuario com permissão de Gestão
            if (user.PerfilUsuario == 1
                || user.PerfilUsuario == 3
                || user.PerfilUsuario == 5
                || user.PerfilUsuario == 6
                || user.PerfilUsuario == 7
                || user.PerfilUsuario == 9)
            {
                //Usuario Vinculado a Obras
                var obras = new UsuarioObraDAO(db).buscarObrasDoUsuario(user);
                var setoresCorporativos = new UsuarioSetorDAO(db).buscarSetoresCorporativosDoUsuario(user);
                if (setoresCorporativos.Count > 0)
                {
                    obras = new List<Obra>();
                    foreach (var setor in setoresCorporativos)
                    {
                        obras.AddRange(obras = new ObraDAO(db).BuscarObrasSetoresCorporativos(setor));
                    }
                }
                var isMatriz = false;
                foreach (var obra in obras)
                {
                    if (obra.Matriz)
                    {
                        isMatriz = true;
                    }
                }
                if (isMatriz && user.PerfilUsuario == 1)
                {
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamados(filtro, false, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(tipoChamado), filtro, false, sortOrder);
                    }

                }
                else if (isMatriz && user.PerfilUsuario == 7)
                {
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosTecnicoRM(obras, filtro, false, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosTecnicoRMTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, false, sortOrder);
                        //.ToPagedList(pageNumber, pageSize);
                    }
                }
                else if (isMatriz && (user.PerfilUsuario == 3 || user.PerfilUsuario == 5 || user.PerfilUsuario == 9))
                {
                    List<Setor> setores;
                    if (Convert.ToBoolean(HttpContext.Current.Request.Cookies["UsuarioSetorCorporativo"].Value))
                    {
                        setores = new SetorDAO(db).BuscarSetoresCoorporativoPorId(setoresCorporativos[0].SetorCorporativo.Value);
                    }
                    else
                    {
                        setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                    }
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeSetores(setores, filtro, false, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setores, Convert.ToInt32(tipoChamado), filtro, false, sortOrder);
                    }
                }
                else
                {

                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, false, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, false, sortOrder);
                    }
                }

            }
            else
            {
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, false, sortOrder);
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, false, sortOrder);
                }

            }
        }

        public List<Chamado> AcompanhamentoChamados(string tipoChamado, string filtro, bool chamadosEncerrados, string sortOrder, ApplicationUser user)
        {
            //var user = manager.FindById(User.Identity.GetUserId());
            //Usuario para Gestao

            //Usuario Vinculado a Obras
            var obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
            var isMatriz = false;
            foreach (var obra in obras)
            {
                if (obra.Matriz)
                {
                    isMatriz = true;
                }
            }
            if (isMatriz && user.PerfilUsuario == 1)
            {
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    return new ChamadoDAO(db).BuscarChamados(filtro, chamadosEncerrados, sortOrder);
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder);
                }

            }
            else if (user.PerfilUsuario == 2 || user.PerfilUsuario == 3 || user.PerfilUsuario == 4 || user.PerfilUsuario == 7 || user.PerfilUsuario == 9)
            {
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, chamadosEncerrados, sortOrder);
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder);
                }
            }
            else if (user.PerfilUsuario == 5)
            {
                var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    var chamados = new ChamadoDAO(db).BuscarChamadosDeSetores(setores, filtro, chamadosEncerrados, sortOrder);
                    chamados.AddRange(new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, chamadosEncerrados, sortOrder));
                    return chamados;
                }
                else
                {
                    var chamados = new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setores, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder);
                    chamados.AddRange(new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder));
                    return chamados;
                }
            }
            else
            {
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    return new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, chamadosEncerrados, sortOrder);
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, chamadosEncerrados, sortOrder);
                }
            }
        }

        public List<Chamado> MeusChamados(string filtro, string sortOrder, ApplicationUser user)
        {
            return new ChamadoDAO(db).BuscarChamadosDoResponsavel(user, filtro, sortOrder);
        }

        public List<Chamado> TriagemChamados(string tipoChamado, string filtro, string obraSelected, string sortOrder, ApplicationUser user)
        {
            var setores = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
            var isSetorCorporativo = false;
            foreach (var setor in setores)
            {
                if (setor.SetorCorporativo != null)
                {
                    isSetorCorporativo = true;
                }
            }
            if (isSetorCorporativo)
            {
                if (obraSelected == null || obraSelected == "-1")
                {
                    return new ChamadoDAO(db).BuscarChamadosSemResponsaveis(filtro, sortOrder, Convert.ToInt32(tipoChamado));
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosSemResponsaveisPorObra(Convert.ToInt32(obraSelected), Convert.ToInt32(tipoChamado), filtro, sortOrder);
                }

            }
            else
            {
                //lista vazia pois se o usuario sem setorCorporativo chegou aqui é por que teve alguma falha de permissão
                var listEmpty = new List<Chamado>();
                return listEmpty;
            }
        }

        public List<Chamado> ChamadosEncerrados(string tipoChamado, string filtro, string sortOrder, ApplicationUser user)
        {
            //Usuario Administrador
            if (user.PerfilUsuario == 1
                || user.PerfilUsuario == 3
                || user.PerfilUsuario == 5
                || user.PerfilUsuario == 6
                || user.PerfilUsuario == 7)
            {
                //Usuario Vinculado a Obras
                var obras = new UsuarioObraDAO().buscarObrasDoUsuario(user);
                var isMatriz = false;
                foreach (var obra in obras)
                {
                    if (obra.Matriz)
                    {
                        isMatriz = true;
                    }
                }
                if (isMatriz && user.PerfilUsuario == 1)
                {
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamados(filtro, true, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosTipoChamado(Convert.ToInt32(tipoChamado), filtro, true, sortOrder);
                    }

                }
                else if (isMatriz && user.PerfilUsuario == 7)
                {

                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosTecnicoRM(obras, filtro, true, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosTecnicoRMTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, true, sortOrder);
                    }
                }
                else if (isMatriz && user.PerfilUsuario == 3)
                {
                    var setoresUsuario = new UsuarioSetorDAO(db).buscarSetoresDoUsuario(user);
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeSetores(setoresUsuario, filtro, true, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeSetoresTipoChamado(setoresUsuario, Convert.ToInt32(tipoChamado), filtro, true, sortOrder);
                    }
                }
                else
                {
                    if (tipoChamado == null || tipoChamado == "-2")
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeObras(obras, filtro, true, sortOrder);
                    }
                    else
                    {
                        return new ChamadoDAO(db).BuscarChamadosDeObrasTipoChamado(obras, Convert.ToInt32(tipoChamado), filtro, true, sortOrder);
                    }
                }

            }
            else
            {
                if (tipoChamado == null || tipoChamado == "-2")
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuario(user, filtro, true, sortOrder);
                }
                else
                {
                    return new ChamadoDAO(db).BuscarChamadosDoUsuarioTipoChamado(user, Convert.ToInt32(tipoChamado), filtro, true, sortOrder);
                }

            }
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
                    setor = sDAO.BuscarSetorPorIdSetorIdObra(Int32.Parse(SetorDestino));
                    ObraDestino = setor.obra.IDO.ToString();
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
            catch (Exception e)
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

        public bool atualizarChamadoHistorico(int id, string informacoesAcompanhamento, ApplicationUser responsavel)
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

        public bool atualizarChamado(int id, Chamado chamado, String SetorDestino, String ddlResponsavelChamado, string informacoesAcompanhamento, ApplicationUser responsavel)
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

                //Reenviar email de abertura para os responsaveis corretos (tipo do chamado)
                new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                {
                    InfoEmail = chamado.Id.ToString(),
                    Data = DateTime.Now,
                    IdTipoEmail = (int)EmailTipo.EmailTipos.AberturaChamado
                });
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
                        if (cDAO.atualizarChamado(id, chamadoOrigem))
                        {
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
            }
            else if (chamadoOrigem.ResponsavelChamado == null && ddlResponsavelChamado != "")
            {
                var user = aDAO.retornarUsuario(ddlResponsavelChamado);
                chamadoOrigem.ResponsavelChamado = user;
                if (cDAO.atualizarChamado(id, chamadoOrigem))
                {
                    chamadoHistorico = cGN.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Usuario " + user.Nome, chamadoOrigem);
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                    });
                }
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

        public bool redirecionarSetorChamado(int id, string SetorDestino, ApplicationUser responsavel)
        {
            try
            {
                var sDAO = new SetorDAO(db);
                var cDAO = new ChamadoDAO(db);
                var chamadoOrigem = cDAO.BuscarChamadoId(id);
                ChamadoHistorico chamadoHistorico;
                if (chamadoOrigem.SetorDestino.Id != Convert.ToInt32(SetorDestino))
                {
                    var setor = sDAO.BuscarSetorId(Convert.ToInt32(SetorDestino));
                    chamadoOrigem.SetorDestino = setor;
                    chamadoOrigem.ResponsavelChamado = null;
                    cDAO.atualizarChamado(id, chamadoOrigem);
                    chamadoHistorico = this.registrarHistorico(DateTime.Now, responsavel, "O Chamado foi direcionado para o Setor " + setor.Descricao, chamadoOrigem);
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamadoHistorico.Chamado.Id.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.AberturaChamado
                    });
                    new EmailEnvioDAO(db).salvarEmailEnvio(new EmailEnvio
                    {
                        InfoEmail = chamadoHistorico.idChamadoHistorico.ToString(),
                        Data = DateTime.Now,
                        IdTipoEmail = (int)EmailTipo.EmailTipos.DirecionamentoChamado
                    });
                }
                return true;
            }catch(Exception e)
            {
                return false;
            }
        }

        public bool reaberturaChamado(int id, string justificativaReabertura, ApplicationUser responsavel)
        {
            try
            {
                var chamado = new ChamadoDAO(db).BuscarChamadoId(id);
                chamado.StatusChamado = false;
                chamado.Cancelado = false;
                new ChamadoDAO(db).atualizarChamado(id, chamado);
                justificativaReabertura = "O chamado encerrado em: " + chamado.DataHoraBaixa.ToString() + " foi reaberto. Justificativa: " + justificativaReabertura;
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

        public void RegistrarUltimaInteracao(int idChamado)
        {
            new ChamadoDAO(db).ultimaInteracao(idChamado);
        }
    }
}