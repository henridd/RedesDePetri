using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri.Classes
{
    class Tela
    {
        public static void imprimirSimulacao(SimuladorRedePetri simulador)
        {
            imprimirCabecalho(simulador);
            imprimirCiclo(simulador);
        }

        public static void imprimirCabecalho(SimuladorRedePetri simulador)
        {
            Console.WriteLine();
            Console.Write("Ciclo | ");
            foreach (Lugar lugar in simulador.lugares)
            {
                Console.Write(lugar.Nome + " | ");
            }
            foreach (Transicao transicao in simulador.transicoes)
            {
                Console.Write(transicao.Nome + " | ");
            }
        }

        public static void imprimirCiclo(SimuladorRedePetri simulador)
        {
            Console.WriteLine();
            Console.Write(simulador.numCiclo.ToString("D5") + " | ");

            foreach (Lugar lugar in simulador.lugares)
            {
                Console.Write(lugar.qtdMarcas.ToString("D2") + " | ");
            }

            foreach (Transicao transicao in simulador.transicoes)
            {
                if (transicao.PodeExecutar)
                {
                    Console.Write("S " + " | ");
                }
                else
                {
                    Console.Write("N " + " | ");
                }
            }
        }

        public static void ImprimeSimulacaoFinalizada()
        {
            Console.WriteLine("Simulacao finalizada...");
        }

    }
}
