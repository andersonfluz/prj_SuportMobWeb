using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("Obra")]
    public class Obra
    {
        [Key]
        public Int32 IDO { get; set; }
        public string Descricao { get; set; }
        public Boolean? Matriz { get; set; }
        public int idCentroAdministrativo { get; set; }
    }
}