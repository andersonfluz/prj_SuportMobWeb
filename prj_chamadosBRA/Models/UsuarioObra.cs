using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("UsuarioObra")]
    public class UsuarioObra
    {
        [Key]
        public int idUsuarioObra { get; set; }
        public string Usuario { get; set; }
        public virtual Obra Obra { get; set; }
    }
}