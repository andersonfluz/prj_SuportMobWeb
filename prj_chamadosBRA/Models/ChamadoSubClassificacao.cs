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
        public string Descricao { get; set; }
        public virtual ChamadoClassificacao ChamadoClassificacao { get; set; }
    }

}