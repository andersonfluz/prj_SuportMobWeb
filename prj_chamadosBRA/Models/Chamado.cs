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
        public string Descricao { get; set; }
        [DataType(DataType.MultilineText)]
        public string Observacao { get; set; }
        [Required]
        [Display(Name = "Setor")]
        public int IdSetorDestino { get; set; }
        public virtual ApplicationUser ResponsavelAberturaChamado { get; set; }
        [Display(Name = "Responsavel do Chamado")]
        public virtual ApplicationUser ResponsavelChamado { get; set; }
        [Display(Name = "Data Chamado")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
               ApplyFormatInEditMode = true)]
        public DateTime DataHoraAbertura { get; set; }
        public Obra ObraDestino { get; set; }
        [Display(Name = "Anexo")]
        public virtual ICollection<ChamadoAnexo> Anexos { get; set; }
    }
}