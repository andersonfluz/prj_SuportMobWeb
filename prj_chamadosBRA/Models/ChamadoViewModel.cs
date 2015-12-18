using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    public class NovoChamadoViewModel
    {
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
        [Required(ErrorMessage = "Informe o Setor")]
        public virtual Setor SetorDestino { get; set; }
        [Required(ErrorMessage = "Informe a Obra")]
        public virtual Obra ObraDestino { get; set; }
        [Display(Name = "Anexo")]
        public virtual ICollection<ChamadoAnexo> Anexos { get; set; }
        [Display(Name = "Tipo do Chamado")]
        [Required(ErrorMessage = "Informe o Tipo do Chamado")]
        public int? TipoChamado { get; set; }
        public virtual ApplicationUser ResponsavelAberturaChamado { get; set; }
    }

    public class EncerramentoChamadoViewModel
    {
        [Key]
        public int Id { get; set; }
        public Boolean? StatusChamado { get; set; }
        [Display(Name = "Solução")]
        [Required(ErrorMessage = "Informe a solução do Chamado")]
        public string Solucao { get; set; }
        [Display(Name = "Data/Hora de Atendimento")]
        [Required(ErrorMessage = "Informe a Data/Hora de Atendimento")]
        public DateTime? DataHoraAtendimento { get; set; }
        [Display(Name = "Classificação do Chamado")]
        [Required(ErrorMessage = "Informe a Classificação do Chamado")]
        public int? Classificacao { get; set; }
        [Display(Name = "SubClassificação do Chamado")]
        [Required(ErrorMessage = "Informe a SubClassificação do Chamado")]
        public int? SubClassificacao { get; set; }
        public virtual ApplicationUser ResponsavelChamado { get; set; }
        public virtual Obra ObraDestino { get; set; }
        public virtual Setor SetorDestino { get; set; }
        [Display(Name = "Erro Operacional")]
        public bool ErroOperacional { get; set; }
    }

    public class ReaberturaChamadoViewModel
    {
        public int Id { get; set; }
        public Boolean? StatusChamado { get; set; }
        [Display(Name = "Responsavel do Chamado")]
        public ApplicationUser ResponsavelChamado { get; set; }
        [Display(Name = "Data/Hora de Atendimento")]
        public DateTime? DataHoraAtendimento { get; set; }
        public string Assunto { get; set; }
        public string Solucao { get; set; }
        [Display(Name = "Justificativa para Reabertura")]
        public string JustificativaReabertura { get; set; }
    }
}