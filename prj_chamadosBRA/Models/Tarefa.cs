using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("Tarefa")]
    public class Tarefa
    {
        [Key]
        public int Id { get; set; }
        public virtual Chamado Chamado { get; set; }
        public string Assunto { get; set; }
        public string Descricao { get; set; }
        public virtual ApplicationUser Responsavel { get; set; }
        public virtual ChamadoClassificacao Natureza { get; set; }
        public virtual ChamadoSubClassificacao SubNatureza { get; set; }
        public bool Especialista { get; set; }
        public bool StatusTarefa { get; set; }
        public string Solucao { get; set; }
        public bool Cancelado { get; set; }
        public string JustificativaCancelamento { get; set; }
    }
}