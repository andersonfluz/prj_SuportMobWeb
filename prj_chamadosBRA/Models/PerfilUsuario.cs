using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    public class PerfilUsuario
    {
        [Key]
        public Int32 IdPerfil { get; set; }
        public string Descricao { get; set; }
        public Int32? Role { get; set; }
    }
}