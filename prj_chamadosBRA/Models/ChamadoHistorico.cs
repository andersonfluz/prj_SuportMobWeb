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
        public Chamado chamado { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]    
        public DateTime Data { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh-mm}")]
        public DateTime Hora { get; set; }
        public virtual ApplicationUser Responsavel { get; set; }
        public String Historico { get; set; }
    }
}