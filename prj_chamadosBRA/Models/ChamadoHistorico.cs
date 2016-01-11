using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoHistorico")]
    public class ChamadoHistorico
    {
        [Key]
        public int idChamadoHistorico { get; set; }
        public virtual Chamado Chamado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]    
        public DateTime Data { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh-mm}")]
        public DateTime Hora { get; set; }
        public virtual ApplicationUser Responsavel { get; set; }
        public string Historico { get; set; }
        public bool Questionamento { get; set; }
        public virtual ChamadoHistorico ReferenciaQuestionamento { get; set; }
        public bool RetornoQuestionamento { get; set; }
    }
}