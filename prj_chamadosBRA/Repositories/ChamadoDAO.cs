using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Repositories
{
    public class ChamadoDAO
    {
        ApplicationDbContext db;

        public ChamadoDAO(ApplicationDbContext db)
        {
            this.db = db;
        }

        public List<Chamado> BuscarChamados(string filtro, bool encerrado, string sortOrder)
        {
            var chamados = (from e in db.Chamado where e.StatusChamado == encerrado select e);
            //return organizarLista(chamados, filtro, sortOrder);
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }

            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }

            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosTipoChamado(int? tipoChamado, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = (from e in db.Chamado where e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e);
            //var ListChamados = organizarLista(chamados, filtro, sortOrder);
            //return ListChamados;
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }

            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosDoUsuario(ApplicationUser user, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado == encerrado select e);
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                case "ultimainteracao":
                    chamados = chamados.OrderByDescending(s => s.UltimaInteracao);
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }

            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosDoUsuarioTipoChamado(ApplicationUser user, int? tipoChamado, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = (from e in db.Chamado where e.ResponsavelAberturaChamado.Id == user.Id && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e);
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }
            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosDeObras(List<Obra> obras, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                var chamadosList = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado == encerrado select e);
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id).ToList();
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome).ToList();
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto).ToList();
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome).ToList();
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao).ToList();
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao).ToList();
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
            }
            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosDeObrasTipoChamado(List<Obra> obras, int? tipoChamado, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = new List<Chamado>();
            foreach (var obra in obras)
            {
                var chamadosList = (from e in db.Chamado where obra.IDO == e.ObraDestino.IDO && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e).ToList();
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id).ToList();
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome).ToList();
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto).ToList();
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome).ToList();
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao).ToList();
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao).ToList();
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeSetores(List<Setor> setores, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = new List<Chamado>();
            foreach (var setor in setores)
            {
                var chamadosList = (from e in db.Chamado where setor.Id == e.SetorDestino.Id && e.StatusChamado == encerrado select e).ToList();
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id).ToList();
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome).ToList();
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto).ToList();
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado != null && s.ResponsavelChamado.Nome != null ? s.ResponsavelChamado.Nome : string.Empty).ToList();
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao).ToList();
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao).ToList();
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosDeSetoresTipoChamado(List<Setor> setores, int? tipoChamado, string filtro, bool encerrado, string sortOrder)
        {
            var chamados = new List<Chamado>();
            foreach (var setor in setores)
            {
                var chamadosList = (from e in db.Chamado where setor.Id == e.SetorDestino.Id && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e).ToList();
                chamados.AddRange(chamadosList);
            }
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower()))).ToList();
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id).ToList();
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome).ToList();
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto).ToList();
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome).ToList();
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao).ToList();
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao).ToList();
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura).ToList();
                    break;
            }
            return chamados;
        }

        public List<Chamado> BuscarChamadosTecnicoRM(List<Obra> obras, string filtro, bool encerrado, string sortOrder)
        {
            var idObra = obras[0].IDO;
            var chamadosList = (from e in db.Chamado where (e.ObraDestino.IDO == idObra || e.TipoChamado == 1) && e.StatusChamado == encerrado select e);

            if (filtro != null)
            {
                chamadosList = chamadosList.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }

            switch (sortOrder)
            {
                case "id":
                    chamadosList = chamadosList.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamadosList = chamadosList.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamadosList = chamadosList.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamadosList = chamadosList.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamadosList = chamadosList.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamadosList = chamadosList.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamadosList = chamadosList.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamadosList = chamadosList.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }
            return chamadosList.ToList();
        }

        public List<Chamado> BuscarChamadosTecnicoRMTipoChamado(List<Obra> obras, int? tipoChamado, string filtro, bool encerrado, string sortOrder)
        {
            var idObra = obras[0].IDO;
            var chamadosList = (from e in db.Chamado where (e.ObraDestino.IDO == idObra || e.TipoChamado == 1) && e.StatusChamado == encerrado && e.TipoChamado == tipoChamado select e);

            if (filtro != null)
            {
                chamadosList = chamadosList.Where(s => s.Id.ToString().ToLower().Contains(filtro.ToLower())
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }
            switch (sortOrder)
            {
                case "id":
                    chamadosList = chamadosList.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamadosList = chamadosList.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamadosList = chamadosList.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamadosList = chamadosList.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamadosList = chamadosList.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamadosList = chamadosList.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    chamadosList = chamadosList.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamadosList = chamadosList.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }
            return chamadosList.ToList();
        }

        public List<Chamado> BuscarChamadosSemResponsaveis(string filtro, string sortOrder, int? tipoChamado)
        {
            if (tipoChamado == -2)
            {
                var chamados = (from e in db.Chamado where e.StatusChamado == false && e.ResponsavelChamado == null select e);
                if (filtro != null)
                {
                    chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                               || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                               || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower())));
                }

                switch (sortOrder)
                {
                    case "id":
                        chamados = chamados.OrderByDescending(s => s.Id);
                        break;
                    case "dataAbertura":
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                    case "solicitante":
                        chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                        break;
                    case "assunto":
                        chamados = chamados.OrderBy(s => s.Assunto);
                        break;
                    case "responsavel":
                        chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                        break;
                    case "obra":
                        chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                        break;
                    case "setor":
                        chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                        break;
                    default:
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                }

                return chamados.ToList();
            }
            else
            {
                var chamados = (from f in db.Chamado where f.StatusChamado == false && f.ResponsavelChamado == null && f.TipoChamado == tipoChamado select f);
                if (filtro != null)
                {
                    chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                               || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                               || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower())));
                }

                switch (sortOrder)
                {
                    case "id":
                        chamados = chamados.OrderByDescending(s => s.Id);
                        break;
                    case "dataAbertura":
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                    case "solicitante":
                        chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                        break;
                    case "assunto":
                        chamados = chamados.OrderBy(s => s.Assunto);
                        break;
                    case "responsavel":
                        chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                        break;
                    case "obra":
                        chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                        break;
                    case "setor":
                        chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                        break;
                    default:
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                }

                return chamados.ToList();
            }
            //return organizarLista(chamados, filtro, sortOrder);

        }

        public List<Chamado> BuscarChamadosSemResponsaveisPorObra(Int32 obra, int? tipoChamado, string filtro, string sortOrder)
        {
            if (tipoChamado == -2)
            {
                var chamados = (from e in db.Chamado where e.StatusChamado == false && e.ResponsavelChamado == null && e.ObraDestino.IDO == obra select e);
                if (filtro != null)
                {
                    chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                               || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                               || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                               || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
                }
                switch (sortOrder)
                {
                    case "id":
                        chamados = chamados.OrderByDescending(s => s.Id);
                        break;
                    case "dataAbertura":
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                    case "solicitante":
                        chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                        break;
                    case "assunto":
                        chamados = chamados.OrderBy(s => s.Assunto);
                        break;
                    case "responsavel":
                        chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                        break;
                    case "obra":
                        chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                        break;
                    case "setor":
                        chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                        break;
                    default:
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                }

                return chamados.ToList();
            }
            else
            {
                var chamados = (from e in db.Chamado where e.StatusChamado == false && e.TipoChamado == tipoChamado && e.ResponsavelChamado == null && e.ObraDestino.IDO == obra select e);
                if (filtro != null)
                {
                    chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                               || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                               || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                               || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                               || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
                }
                switch (sortOrder)
                {
                    case "id":
                        chamados = chamados.OrderByDescending(s => s.Id);
                        break;
                    case "dataAbertura":
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                    case "solicitante":
                        chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                        break;
                    case "assunto":
                        chamados = chamados.OrderBy(s => s.Assunto);
                        break;
                    case "responsavel":
                        chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                        break;
                    case "obra":
                        chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                        break;
                    case "setor":
                        chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                        break;
                    default:
                        chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                        break;
                }

                return chamados.ToList();
            }
        }

        public List<Chamado> BuscarChamadosDoResponsavel(ApplicationUser user, string filtro, string sortOrder)
        {
            var chamados = (from e in db.Chamado where e.ResponsavelChamado.Id == user.Id && e.StatusChamado == false select e);
            if (filtro != null)
            {
                chamados = chamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }
            switch (sortOrder)
            {
                case "id":
                    chamados = chamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    chamados = chamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    chamados = chamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    chamados = chamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    chamados = chamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "ultimainteracao":
                    chamados = chamados.OrderByDescending(s => s.UltimaInteracao);
                    break;
                case "setor":
                    chamados = chamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    chamados = chamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
            }

            return chamados.ToList();
        }

        public List<Chamado> BuscarChamadosSemResponsaveisPorTrintaMinutos()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado == null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 4 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffMinutes(e.DataHoraAbertura, DateTime.Now) >= 30
                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemResponsaveisPorUmaHora()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado == null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 5 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffMinutes(e.DataHoraAbertura, DateTime.Now) >= 60
                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemResponsaveisPorDuasHoras()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado == null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 6 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffMinutes(e.DataHoraAbertura, DateTime.Now) >= 120
                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemAtualizaoPorDoisDiasTrintaMinutos()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            join h in db.ChamadoHistorico on e.Id equals h.Chamado.Id
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado != null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 7 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  h.idChamadoHistorico == db.ChamadoHistorico.OrderByDescending(s => s.Data).FirstOrDefault(s => s.Chamado.Id == h.Chamado.Id).idChamadoHistorico &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffMinutes(h.Data, DateTime.Now) >= 2910

                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemAtualizacaoPorDoisDiasUmaHora()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            join h in db.ChamadoHistorico on e.Id equals h.Chamado.Id
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado != null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 8 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  h.idChamadoHistorico == db.ChamadoHistorico.OrderByDescending(s => s.Data).FirstOrDefault(s => s.Chamado.Id == h.Chamado.Id).idChamadoHistorico &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffDays(h.Data, DateTime.Now) >= 2940
                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemAtualizacaoPorDoisDiasDuasHoras()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            join h in db.ChamadoHistorico on e.Id equals h.Chamado.Id
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado != null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 9 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  h.idChamadoHistorico == db.ChamadoHistorico.OrderByDescending(s => s.Data).FirstOrDefault(s => s.Chamado.Id == h.Chamado.Id).idChamadoHistorico &&
                                  DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 30 &&
                                  DbFunctions.DiffMinutes(h.Data, DateTime.Now) >= 3000
                            select e).ToList();
            return chamados;
        }

        public List<Chamado> BuscarChamadosSemRetornoPorUmaOuSeisHoras()
        {
            var chamados = (from e in db.Chamado
                            join l in db.ChamadoLogAcao on e.Id equals l.IdChamado
                            join h in db.ChamadoHistorico on e.Id equals h.Chamado.Id
                            where e.StatusChamado == false &&
                                  e.ResponsavelChamado != null &&
                                  l.Id == db.ChamadoLogAcao.OrderByDescending(s => s.DataAcao).FirstOrDefault(s => s.IdChamado == l.IdChamado && (s.ChamadoAcao.IdAcao == 10 || s.ChamadoAcao.IdAcao == 1)).Id &&
                                  ((DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 60 && l.ChamadoAcao.IdAcao == 1) ||
                                  (DbFunctions.DiffMinutes(l.DataAcao, DateTime.Now) >= 360 && l.ChamadoAcao.IdAcao == 10))
                            select e).ToList();
            return chamados;
        }

        public Chamado BuscarChamadoId(int id)
        {
            return db.Set<Chamado>().Find(id);
        }

        public bool salvarChamado(Chamado chamado)
        {
            db.Chamado.Add(chamado);
            db.SaveChanges();
            return true;
        }

        public bool atualizarChamado(int id, Chamado chamado)
        {
            try
            {
                var chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
                chamadoUpdate.ObsevacaoInterna = chamado.ObsevacaoInterna;
                chamadoUpdate.SetorDestino = chamado.SetorDestino;
                chamadoUpdate.ObraDestino = chamado.ObraDestino;
                chamadoUpdate.ResponsavelChamado = chamado.ResponsavelChamado;
                chamadoUpdate.TipoChamado = chamado.TipoChamado;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void encerrarChamado(int id, Chamado chamado)
        {
            var chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            chamadoUpdate.DataHoraAtendimento = chamado.DataHoraAtendimento;
            chamadoUpdate.Classificacao = chamado.Classificacao;
            chamadoUpdate.SubClassificacao = chamado.SubClassificacao;
            chamadoUpdate.DataHoraBaixa = chamado.DataHoraBaixa;
            chamadoUpdate.ObraDestino = chamado.ObraDestino;
            chamadoUpdate.Solucao = chamado.Solucao;
            chamadoUpdate.StatusChamado = true;
            chamadoUpdate.ErroOperacional = chamado.ErroOperacional;
            db.SaveChanges();
        }

        public void cancelarChamado(int id, Chamado chamado)
        {
            var chamadoUpdate = (from e in db.Chamado where e.Id == id select e).SingleOrDefault();
            chamadoUpdate.Cancelado = true;
            chamadoUpdate.StatusChamado = true;
            chamadoUpdate.DataHoraCancelamento = DateTime.Now;
            chamadoUpdate.JustificativaCancelamento = chamado.JustificativaCancelamento;
            chamadoUpdate.ResponsavelCancelamento = chamado.ResponsavelCancelamento;
            db.SaveChanges();
        }

        public void ultimaInteracao(int idChamado)
        {
            var chamadoUpdate = (from e in db.Chamado where e.Id == idChamado select e).SingleOrDefault();
            chamadoUpdate.UltimaInteracao = DateTime.Now;
            db.SaveChanges();
        }
    }
}