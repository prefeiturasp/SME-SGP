using MSTech.GestaoEscolar.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestesConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                while (Console.ReadLine() == "e")
                {
                    Console.WriteLine("Processando fechamento...");
                    GestaoEscolarServicosBO.ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync(limpacache: false);
                    Console.WriteLine("Processando pendências...");
                    GestaoEscolarServicosBO.ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync(limpacache: false);
                    Console.WriteLine("Finalizado.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                Console.ReadLine();
                //Util.GravarErro(ex, context.Scheduler.Context);
            }
        }
    }
}
