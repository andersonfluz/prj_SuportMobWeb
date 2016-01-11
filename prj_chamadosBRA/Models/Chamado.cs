using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("Chamado")]
    public class Chamado
    {
        [Key]
        public int Id { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public string Observacao { get; set; }
        public virtual Setor SetorDestino { get; set; }
        public virtual ApplicationUser ResponsavelAberturaChamado { get; set; }
        public virtual ApplicationUser ResponsavelChamado { get; set; }
        public DateTime DataHoraAbertura { get; set; }
        public virtual Obra ObraDestino { get; set; }
        public virtual ICollection<ChamadoAnexo> Anexos { get; set; }
        public Boolean? StatusChamado { get; set; }
        public string ObsevacaoInterna { get; set; }
        public DateTime? DataHoraBaixa { get; set; }
        public string Solucao { get; set; }
        public int? TipoChamado { get; set; }
        public DateTime? DataHoraAtendimento { get; set; }
        public int? Classificacao { get; set; }
        public int? SubClassificacao { get; set; }
        public virtual ApplicationUser ResponsavelCriacaoChamado { get; set; }
        public bool? ErroOperacional { get; set; }
        public bool Cancelado { get; set; }
        public string JustificativaCancelamento { get; set; }
        public DateTime? DataHoraCancelamento { get; set; }
        public virtual ApplicationUser ResponsavelCancelamento { get; set; }
    }
}