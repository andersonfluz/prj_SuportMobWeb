using prj_chamadosBRA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Utils
{
    public class TratamentoListaUtil
    {
        ApplicationDbContext db;

        public TratamentoListaUtil(ApplicationDbContext db)
        {
            this.db = db;
        }
        public List<Chamado> organizarLista(IEnumerable<Chamado> listaChamados, string filtro, string sortOrder)
        {
            if (filtro != null)
            {
                listaChamados = listaChamados.Where(s => s.Id.ToString().Contains(filtro)
                                                           || s.Assunto.ToLower().Contains(filtro.ToLower())
                                                           || s.ObraDestino.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || s.Descricao.ToLower().Contains(filtro.ToLower())
                                                           || (s.ResponsavelAberturaChamado != null && s.ResponsavelAberturaChamado.Nome.ToLower().Contains(filtro.ToLower()))
                                                           || (s.ResponsavelChamado != null && s.ResponsavelChamado.Nome.ToLower().Contains(filtro.ToLower())));
            }

            switch (sortOrder)
            {
                case "id":
                    listaChamados = listaChamados.OrderByDescending(s => s.Id);
                    break;
                case "dataAbertura":
                    listaChamados = listaChamados.OrderByDescending(s => s.DataHoraAbertura);
                    break;
                case "solicitante":
                    listaChamados = listaChamados.OrderBy(s => s.ResponsavelAberturaChamado.Nome);
                    break;
                case "assunto":
                    listaChamados = listaChamados.OrderBy(s => s.Assunto);
                    break;
                case "responsavel":
                    listaChamados = listaChamados.OrderBy(s => s.ResponsavelChamado.Nome);
                    break;
                case "obra":
                    listaChamados = listaChamados.OrderBy(s => s.ObraDestino.Descricao);
                    break;
                case "setor":
                    listaChamados = listaChamados.OrderBy(s => s.SetorDestino.Descricao);
                    break;
                default:
                    listaChamados = listaChamados.OrderBy(s => s.Id);
                    break;
            }
            return listaChamados.ToList();
        }
    }
}