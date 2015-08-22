using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoClassificacao")]
    public class ChamadoClassificacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        public virtual Obra Obra { get; set; }

    }
}