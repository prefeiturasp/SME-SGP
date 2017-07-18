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
                string letra = Console.ReadLine().ToLower();
                while (letra.In("e", "w", "p"))
                {
                    if (letra == "e")
                    {
                        Console.WriteLine("Processando fechamento...");
                        GestaoEscolarServicosBO.ExecJOB_ProcessamentoNotaFrequenciaFechamentoAsync(limpacache: false);
                        Console.WriteLine("Processando pendências...");
                        GestaoEscolarServicosBO.ExecJOB_ProcessamentoRelatorioDisciplinasAlunosPendenciasAsync(limpacache: false);
                        Console.WriteLine("Processando preenchimento de frequência...");
                        GestaoEscolarServicosBO.ExecJOB_ProcessamentoPreenchimentoFrequencia();
                        Console.WriteLine("Processando alunos com baixa frequência e com faltas consecutivas...");
                        GestaoEscolarServicosBO.ExecJOB_ProcessamentoAlunosFrequencia();
                        Console.WriteLine("Finalizado.");
                    }
                    else if (letra == "w")
                    {
                        Console.WriteLine("Processando alerta de início de fechamento...");
                        GestaoEscolarServicosBO.ExecJOB_AlertaInicioFechamento();
                        Console.WriteLine("Processando alerta de fim de fechamento...");
                        GestaoEscolarServicosBO.ExecJOB_AlertaFimFechamento();
                        Console.WriteLine("Finalizado.");
                    }
                    else if (letra == "p")
                    {
                        Console.WriteLine("Processando pendência de aula...");
                        GestaoEscolarServicosBO.ExecJOB_ProcessamentoPendenciaAulas();
                        Console.WriteLine("Finalizado.");
                    }

                    letra = Console.ReadLine().ToLower();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.Message);
                Console.ReadLine();
            }
        }
    }
}
