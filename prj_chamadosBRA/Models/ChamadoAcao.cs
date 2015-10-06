using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoAcao")]
    public class ChamadoAcao
    {
        [Key]
        public int IdAcao { get; set; }
        public string Descricao { get; set; }

    }
}