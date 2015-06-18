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
        [Required]
        public string Assunto { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [Display(Name = "Setor")]
        //[Required(ErrorMessage="Informe o Setor")]
        public virtual Setor SetorDestino { get; set; }
        public virtual ApplicationUser ResponsavelAberturaChamado { get; set; }
        [Display(Name = "Responsavel do Chamado")]
        public virtual ApplicationUser ResponsavelChamado { get; set; }
        [Display(Name = "Data Chamado")]
        //[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
        //       ApplyFormatInEditMode = true)]
        public DateTime DataHoraAbertura { get; set; }
        [Required(ErrorMessage = "Informe a Obra")]
        public virtual Obra ObraDestino { get; set; }
        [Display(Name = "Anexo")]
        public virtual ICollection<ChamadoAnexo> Anexos { get; set; }
        public Boolean? StatusChamado { get; set; }
        public string ObsevacaoInterna { get; set; }
        public DateTime? DataHoraBaixa { get; set; }
        [Display(Name = "Solução")]
        public string Solucao { get; set; }
        [Display(Name = "Tipo do Chamado")]
        [Required(ErrorMessage = "Informe o Tipo do Chamado")]
        public int? TipoChamado { get; set; }
        [Display(Name = "Data/Hora de Atendimento")]
        [Required(ErrorMessage = "Informe a Data/Hora de Atendimento")]
        public DateTime? DataHoraAtendimento { get; set; }
        [Display(Name = "Classificação do Chamado")]
        [Required(ErrorMessage = "Informe a Classificação do Chamado")]
        public int? Classificacao { get; set; }
        [Display(Name = "SubClassificação do Chamado")]
        [Required(ErrorMessage = "Informe a SubClassificação do Chamado")]
        public int? SubClassificacao { get; set; }
    }
}