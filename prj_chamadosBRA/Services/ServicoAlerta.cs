using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace prj_chamadosBRA.Service
{
    public class ServicoAlerta : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _derrubando;

        public ServicoAlerta()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _derrubando = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void Executar()
        {
            while (true)
            {
                lock (_lock)
                {
                    if (_derrubando)
                    {
                        return;
                    }
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Sunday && DateTime.Now.DayOfWeek != DayOfWeek.Saturday && (DateTime.Now.Hour >= 6 && DateTime.Now.Hour <= 18))
                    {
                        //Todos os Tecnicos do setor
                        AlertaSemResponsavel.AlertaTrintaMinutos();
                        AlertaSemAtualizacaoInterno.AlertaDoisDiasTrintaMinutos();

                        //Coordenador do Setor
                        AlertaSemResponsavel.AlertaUmaHora();
                        AlertaSemAtualizacaoInterno.AlertaDoisDiasUmaHora();

                        //Coordenador geral do setor
                        AlertaSemResponsavel.AlertaDuasHoras();              
                        AlertaSemAtualizacaoInterno.AlertaDoisDiasDuasHoras();

                        //Solicitante Chamado
                        AlertaSemRetornoSolicitante.AlertaSemRetornoUmaOuSeisHoras();
                    } 
                }
                Thread.Sleep(60000);
            }
        }

    }
}