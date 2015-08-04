using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("Setor")]
    public class Setor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Nome { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required]
        [Display(Name = "Responsável")]
        public string Responsavel { get; set; }
        [Required]
        [Display(Name = "Email do Responsável")]
        public string EmailResponsavel { get; set; }
        [Required]
        [Display(Name = "Email do Setor")]
        public string EmailSetor { get; set; }
        [Required]
        [Display(Name = "Obra")]
        public virtual Obra obra { get; set; }
    }
}