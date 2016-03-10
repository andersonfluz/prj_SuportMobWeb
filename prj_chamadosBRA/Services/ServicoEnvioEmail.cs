using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Hosting;

namespace prj_chamadosBRA.Service
{
    public class ServicoEnvioEmail : IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _derrubando;

        public ServicoEnvioEmail()
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
                    EmailEnvioSender.EnvioEmailAberturaChamados();
                    EmailEnvioSender.EnvioEmailDirecionamentoChamados();
                    EmailEnvioSender.EnvioEmailAtualizacaoChamados();
                    EmailEnvioSender.EnvioEmailEncerramentoChamados();
                    EmailEnvioSender.EnvioEmailCancelamentoChamados();
                    EmailEnvioSender.EnvioEmailReaberturaChamados();
                    EmailEnvioSender.EnvioEmailCriacaoUsuario();
                    EmailEnvioSender.EnvioEmailRedefinicaoSenhaUsuario();
                    EmailEnvioSender.EnvioEmailAberturaTarefa();
                    EmailEnvioSender.EnvioEmailPrevisaoEntregaTarefa();
                    EmailEnvioSender.EnvioEmailEntregaTarefa();
                    EmailEnvioSender.EnvioEmailAlertaSemResponsavelTrintaMinutos();
                    EmailEnvioSender.EnvioEmailAlertaSemResponsavelUmaHora();
                    EmailEnvioSender.EnvioEmailAlertaSemResponsavelDuasHoras();
                    EmailEnvioSender.EnvioEmailAlertaSemAtualizacaoDoisDiasTrintaMinutos();
                    EmailEnvioSender.EnvioEmailAlertaSemAtualizacaoDoisDiasUmaHora();
                    EmailEnvioSender.EnvioEmailAlertaSemAtualizacaoDoisDiasDuasHoras();
                    EmailEnvioSender.EnvioEmailAlertaSemRetornoSolicitanteUmaOuSeisHoras();
                }
                Thread.Sleep(60000);
            }
        }
    }
}