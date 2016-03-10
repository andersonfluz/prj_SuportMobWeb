using System;
using System.ComponentModel.DataAnnotations;

namespace prj_chamadosBRA.Models
{
    public class NovaTarefaViewModel
    {
        [Required]
        public virtual Chamado Chamado { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required]
        [Display(Name = "Tipo do Chamado")]
        public int TipoTarefa { get; set; }
        [Display(Name = "Data de Abertura")]
        public DateTime DataAbertura { get; set; }
        [Display(Name = "Tipo de Responsavel")]
        [Required]
        public string TipoResponsavel { get; set; }
        [Display(Name = "Responsavel")]
        public string Responsavel { get; set; }
        [Display(Name = "Responsavel Terceiros")]
        public string ResponsavelTerceiros { get; set; }
        [Required]
        public int? Natureza { get; set; }
        [Required]
        public int? SubNatureza { get; set; }
        public bool Terceirizado { get; set; }
        public bool Especialista { get; set; }
    }


    public class ListaTarefaViewModel
    {
        public int Id { get; set; }
        public string Assunto { get; set; }
        [Display(Name = "Abertura")]
        public DateTime DataAbertura { get; set; }
        [Display(Name = "Previsão de Entrega")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataPrevisaoEntrega { get; set; }
        [Display(Name = "Tipo do Chamado")]
        public int TipoTarefa { get; set; }
        [Display(Name = "Solicitante")]
        public ApplicationUser Solicitante { get; set; }
        public DateTime DataEntrega { get; set; } 
    }

    public class EditarTarefaViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public virtual Chamado Chamado { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Observação")]
        public DateTime DataAbertura { get; set; }
        public DateTime DataPrevisaoEntrega { get; set; }
        [Required]
        [Display(Name = "Responsavel")]
        public string Responsavel { get; set; }
        [Required]
        public int? Natureza { get; set; }
        [Required]
        public int? SubNatureza { get; set; }
    }

    public class DetalhesTarefaViewModel
    {
        public int Id { get; set; }
        public virtual Chamado Chamado { get; set; }
        public string Assunto { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public ApplicationUser Solicitante { get; set; }
        [Display(Name ="Abertura")]
        public DateTime DataAbertura { get; set; }
        [Display(Name ="Previsão de Entrega")]
        public DateTime? DataPrevisaoEntrega { get; set; }
        [Display(Name = "Responsavel")]
        public ApplicationUser Responsavel { get; set; }
        public ChamadoClassificacao Natureza { get; set; }
        public ChamadoSubClassificacao SubNatureza { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Solucao { get; set; }
        public bool? Aprovado { get; set; }
        public string JustificativaAprovacao { get; set; }
    }

    public class PrevisaoTarefaViewModel
    {
        public int Id { get; set; }
        public virtual Chamado Chamado { get; set; }
        public string Assunto { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public ApplicationUser Solicitante { get; set; }
        [Display(Name = "Abertura")]
        public DateTime DataAbertura { get; set; }
        [Required]
        [Display(Name = "Previsão de Entrega")]
        public DateTime? DataPrevisaoEntrega { get; set; }
        [Display(Name = "Responsavel")]
        public ApplicationUser Responsavel { get; set; }
        public ChamadoClassificacao Natureza { get; set; }
        public ChamadoSubClassificacao SubNatureza { get; set; }
    }

    public class EncerramentoTarefaViewModel
    {
        public int Id { get; set; }
        public virtual Chamado Chamado { get; set; }
        public string Assunto { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public ApplicationUser Solicitante { get; set; }
        [Display(Name = "Abertura")]
        public DateTime DataAbertura { get; set; }        
        [Required]
        [Display(Name = "Data da Entrega")]
        public DateTime DataEntrega { get; set; }        
        [Required]
        [Display(Name = "Solução")]
        [DataType(DataType.MultilineText)]
        public string Solucao { get; set; }
        [Display(Name = "Responsável")]
        public ApplicationUser Responsavel { get; set; }        
    }

    public class AprovacaoTarefaViewModel
    {
        public int Id { get; set; }
        public virtual Chamado Chamado { get; set; }
        public string Assunto { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public ApplicationUser Solicitante { get; set; }
        [Display(Name = "Abertura")]
        public DateTime DataAbertura { get; set; }
        [Required]
        public bool? Aprovado { get; set; }
        [Required]
        [Display(Name = "Justificativa/Observação")]
        public string Justificativa { get; set; }
        [Display(Name = "Responsável")]
        public ApplicationUser Responsavel { get; set; }
    }

}