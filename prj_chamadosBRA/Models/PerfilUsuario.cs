using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("PerfilUsuario")]
    public class PerfilUsuario
    {
        [Key]
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
        public int? Role { get; set; }
        public enum Perfis
        {
            Administrador = 1,
            SuperiorBRA = 2,
            Tecnico = 3,
            Usuario = 4,
            Gestor = 5,
            AdministradorObra = 6,
            TecnicoTotvs = 7,
            Terceirizado = 8,
            CentralAtendimentoI = 9
        }

    }
}