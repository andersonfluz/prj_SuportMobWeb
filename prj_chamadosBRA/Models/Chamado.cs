using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
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
        public string ResponsavelChamado { get; set; }
        public DateTime DataHoraAbertura { get; set; }

        public ICollection<ChamadoAnexo> Anexos { get; set; }
    }
}