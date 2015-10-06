using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoLogAcao")]
    public class ChamadoLogAcao
    {
        [Key]
        public int Id { get; set; }
        public int IdChamado { get; set; }
        public virtual ChamadoAcao ChamadoAcao { get; set; }
        public string Texto { get; set; }
        public DateTime DataAcao { get; set; }
        public virtual ApplicationUser UsuarioAcao { get; set; }


    }
}