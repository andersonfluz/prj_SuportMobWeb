using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("CentroAdministrativo")]
    public class CentroAdministrativo
    {
        [Key]
        public int idCA { get; set; }
        public string Nome { get; set; }
    }
}