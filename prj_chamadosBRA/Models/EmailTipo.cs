using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace prj_chamadosBRA.Models
{
    [Table("EmailTipo")]
    public class EmailTipo
    {
        [Key]
        public int Id { get; set; }
        public string Descricao { get; set; }
        public enum EmailTipos
        {
            AberturaChamado = 1,
            DirecionamentoChamado = 2,
            AtualizacaoChamado = 3,
            EncerramentoChamado = 4,
            CancelamentoChamado = 5,
            ReaberturaChamado = 6,
            CriacaoUsuario = 7,
            RedefinicaoSenhaUsuario = 8,
            AlertaSemResponsavelTrintaMinutos = 9,
            AlertaSemResponsavelUmaHora = 10,
            AlertaSemResponsavelDuasHoras = 11,
            AlertaSemAtualizacaoDoisDiasTrintaMinutos = 12,
            AlertaSemAtualizacaoDoisDiasUmaHora = 13,
            AlertaSemAtualizacaoDoisDiasDuasHoras = 14,
            AlertaSemRetornoSolicitanteUmaOuSeisHoras = 15,
            AberturaTarefa = 16,
            PrevisaoEntregaTarefa = 17,
            EntregaTarefa = 18
        }
    }
}