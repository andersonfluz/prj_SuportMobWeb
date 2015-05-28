using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("ChamadoAnexo")]
    public class ChamadoAnexo
    {
        [Key]
        public int idAnexo { get; set; }
        public Chamado Chamado { get; set; }
        public byte[] arquivoAnexo { get; set; }
        public string NomeAnexo { get; set; }
        public string ContentType { get; set; }

    }
}