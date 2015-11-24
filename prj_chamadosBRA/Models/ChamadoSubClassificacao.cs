using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoSubClassificacao")]
    public class ChamadoSubClassificacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Classificação")]
        public virtual ChamadoClassificacao ChamadoClassificacao { get; set; }
        public bool Ativo { get; set; }
        public string Usuario { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }

}