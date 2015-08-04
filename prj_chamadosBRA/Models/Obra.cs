using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("Obra")]
    public class Obra
    {
        [Key]
        public Int32 IDO { get; set; }
        [Required]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Required]
        public Boolean Matriz { get; set; }
        [Required]
        [Display(Name = "Centro Administrativo")]
        public virtual CentroAdministrativo CentroAdministrativo { get; set; }
    }
}