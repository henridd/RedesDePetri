using Newtonsoft.Json;
using RedesDePetri.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri
{
    class Program
    {
        public static Tela Tela { get; private set; }

        static void Main(string[] args)
        {
            try
            {
                //args = new string[] { @"E:\Visual Studio\RedesDePetri\RedesDePetri\bin\Debug\bkpSimulacao" };
                SimuladorRedePetri simulador = new SimuladorRedePetri();
                if (args.Length > 0)
                    simulador.CarregarSimulacao(args[0]);
                else {
                    simulador = new SimuladorRedePetri();
                    simulador.RecebeLugares();
                    simulador.RecebeTransicoes();
                    simulador.RecebePesoArcos();
                }

                Console.WriteLine("\n- Simulacao da Rede de Petri -");

                while (!simulador.terminada)
                {
                    simulador.SalvarBackup();
                    simulador.TransacoesHabilitadas();

                    Tela.imprimirSimulacao(simulador);

                    Console.WriteLine();
                    Console.WriteLine("Para executar aperte 'Enter'");
                    if (Console.ReadKey().Key == ConsoleKey.Enter)
                    {
                        simulador.ExecutaCiclo();
                    }
                    else
                    {
                        Tela.ImprimeSimulacaoFinalizada();
                        break;
                    }
                }
            }
            catch (RedePetriException e)
            {
                Console.Write(e.Message);
            }

        }
    }
}
