using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace prj_chamadosBRA.Test
{
    [TestClass]
    public class AlertaSemResponsavelTest
    {
        [TestMethod]
        public void AlertaTrintaMinutos_Test()
        {
            var chamados = Utils.AlertaSemResponsavel.AlertaTrintaMinutos();
            
        }
    }
}
