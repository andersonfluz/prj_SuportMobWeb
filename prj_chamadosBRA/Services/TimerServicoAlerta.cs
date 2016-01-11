using System;
using System.Threading;

namespace prj_chamadosBRA.Service
{
    public class TimerServicoAlerta
    {
        private static readonly Timer _timer = new Timer(OnTimerElapsed);
        private static readonly ServicoAlerta _servico = new ServicoAlerta();

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