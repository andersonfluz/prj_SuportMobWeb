using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("vw_ChamadosTICorporativo")]
    public class ChamadoViewBI
    {
        public int Id { get; set; }
        public string Assunto { get; set; }
        public string SetorDestino { get; set; }
        public string Classificacao { get; set; }
        public string ObraDestino { get; set; }
        public string ResponsavelChamado { get; set; }
        public DateTime? DataHoraAbertura { get; set; }
        public int TotalDias { get; set; }
        public int AnoAbertura { get; set; }
        public int MesAbertura { get; set; }
        public int DiaAbertura { get; set; }
        public DateTime? DataHoraBaixa { get; set; }
        public int? AnoBaixa { get; set; }
        public int? MesBaixa { get; set; }
        public int? DiaBaixa { get; set; }
        public string Solicitante { get; set; }
        public string SetorObraSolicitante { get; set; }
        public string TipoChamado { get; set; }
        public string ResponsavelCriacaoChamado { get; set; }
        public string StatusChamado { get; set; }
    }
}