using prj_chamadosBRA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace prj_chamadosBRA.Services
{
    public class TimerServicoEnvioEmail
    {
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly ServicoEnvioEmail _servico = new ServicoEnvioEmail();

        public static void Start()
        {
            _timer.Change(TimeSpan.Zero, TimeSpan.FromMilliseconds(60000));
        }

        private static void OnTimerElapsed(object sender)
        {
            _servico.Executar();
        }
    }
}