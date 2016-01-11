using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("EmailEnvio")]
    public class EmailEnvio
    {
        public int Id { get; set; }
        public string InfoEmail { get; set; }
        public DateTime Data { get; set; }
        public int IdTipoEmail { get; set; }
        public int Tentativas { get; set; }
        public string Erro { get; set; }
    }
}