using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    public class TarefaViewModel
    {
        [Required]
        public Chamado Chamado { get; set; }
        [Required]
        public string Assunto { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Observação")]
        public DateTime DataAbertura { get; set; }
        [Required]
        [Display(Name = "Responsavel")]
        public virtual ApplicationUser Responsavel { get; set; }
        [Required]
        public virtual ChamadoClassificacao Natureza { get; set; }
        [Required]
        public virtual ChamadoSubClassificacao SubNatureza { get; set; }       
    }
}