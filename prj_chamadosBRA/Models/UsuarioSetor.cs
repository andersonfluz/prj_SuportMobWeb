using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("UsuarioSetor")]
    public class UsuarioSetor
    {
        [Key]
        public int idUsuarioSetor { get; set; }
        public ApplicationUser Usuario { get; set; }
        public int idSetor { get; set; }
    }
}