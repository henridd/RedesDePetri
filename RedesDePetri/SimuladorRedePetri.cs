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
    class SimuladorRedePetri
    {
        public int numCiclo { get; set; }
        public bool terminada { get; set; }
        public List<Lugar> lugares { get; set; } = new List<Lugar>();
        public List<Transicao> transicoes { get; set; } = new List<Transicao>();
        public List<Arco> arcos { get; set; } = new List<Arco>();

        private List<Transicao> filaTransicoes = new List<Transicao>();

        public SimuladorRedePetri()
        {
            numCiclo = 0;
            terminada = false;
        }

        public SimuladorRedePetri(List<Lugar> lugares, List<Transicao> transicoes, List<Arco> arcos) :
            this()
        {
            this.lugares = lugares;
            this.transicoes = transicoes;
            this.arcos = arcos;
        }

        public void ExecutaCiclo()
        {
            filaTransicoes.Clear();
            TransacoesHabilitadas();
            if (filaTransicoes.Count == 0)
            {
                terminada = true;
                Tela.ImprimeSimulacaoFinalizada();
                return;
            }

            foreach (Transicao transicao in filaTransicoes)
            {
                ExecutaTransicao(transicao);
            }

            numCiclo++;
        }

        public void TransacoesHabilitadas()
        {
            foreach (Transicao transicao in transicoes)
            {
                if (transicao.PodeExecutar)
                {
                    filaTransicoes.Add(transicao);
                }
            }
        }

        public void ExecutaTransicao(Transicao transicao)
        {
            if (transicao.PodeExecutar)
            {
                foreach (Lugar lugar in transicao.lugaresEntrada)
                {
                    lugar.RetiraMarca();
                }
                foreach (Lugar lugar in transicao.lugaresSaida)
                {
                    lugar.AdicionaMarca();
                }
            }
        }

        public void RecebeLugares()
        {
            Console.Write("Quantos lugares: ");
            int qtdLugar = int.Parse(Console.ReadLine());

            for (int i = 1; i <= qtdLugar; i++)
            {
                Console.Write($"Quantas marcas em L{i}? ");
                int qtdMarcas = int.Parse(Console.ReadLine());

                lugares.Add(new Lugar($"L{i}", qtdMarcas));
            }
        }

        public void RecebeTransicoes()
        {

            Console.Write("Quantas transicões: ");
            int qtdTransicoes = int.Parse(Console.ReadLine());

            for (int i = 1; i <= qtdTransicoes; i++)
            {
                Transicao transicao = new Transicao($"T{i}");
                transicoes.Add(transicao);

                RecebeLugaresEntrada(transicao);
                RecebeLugaresSaida(transicao);
            }
        }

        private void RecebeLugaresEntrada(Transicao transicao)
        {
            Console.Write($"Quais são os lugares de entrada de {transicao.Nome}? ");
            string[] arrayLugares = Console.ReadLine().Split(',');

            if (arrayLugares.Length > 0)
            {
                foreach (string nome in arrayLugares)
                {
                    Lugar lugar = lugares.Find(obj => obj.Nome == nome);

                    Arco arco = new Arco(lugar, transicao);
                    arcos.Add(arco);

                    lugar.arco = arco;
                    transicao.arcos.Add(arco);
                    transicao.lugaresEntrada.Add(lugar);
                }
            }
        }

        private void RecebeLugaresSaida(Transicao transicao)
        {
            Console.Write($"Quais são os lugares de saida de {transicao.Nome}? ");
            string[] arrayLugares = Console.ReadLine().Split(',');

            if (arrayLugares.Length > 0)
            {
                foreach (string nome in arrayLugares)
                {
                    Lugar lugar = lugares.Find(obj => obj.Nome == nome);

                    Arco arco = new Arco(transicao, lugar);
                    arcos.Add(arco);

                    lugar.arco = arco;
                    transicao.arcos.Add(arco);
                    transicao.lugaresSaida.Add(lugar);
                }
            }
        }

        public void RecebePesoArcos()
        {
            foreach (Transicao transicao in transicoes)
            {
                foreach (Lugar lugar in transicao.lugaresEntrada)
                {
                    Console.Write($"Qual o peso do arco de ${lugar.Nome} para ${transicao.Nome}? ");
                    lugar.arco.peso = int.Parse(Console.ReadLine());
                }
                foreach (Lugar lugar in transicao.lugaresSaida)
                {
                    Console.Write($"Qual o peso do arco de ${transicao.Nome} para ${lugar.Nome}? ");
                    lugar.arco.peso = int.Parse(Console.ReadLine());
                }
            }
        }


        public void SalvarBackup()
        {
            var sb = new StringBuilder();
            // Salvar lugares
            sb.AppendLine("#LUGARES");
            foreach (var lugar in lugares)
                sb.AppendLine(string.Format("{0};{1}", lugar.Nome, lugar.qtdMarcas));

            // Salvar transições            
            sb.AppendLine("#TRANSIÇÕES");
            foreach (var transicao in transicoes)
                sb.AppendLine(string.Format("{0};{1};{2}", transicao.Nome, FormatarStringLugares(transicao.lugaresEntrada), FormatarStringLugares(transicao.lugaresSaida)));

            // Salvar arcos           
            sb.AppendLine("#ARCOS");
            foreach (var arco in arcos)
                sb.AppendLine(string.Format("{0};{1};{2}", arco.origem.Nome, arco.destino.Nome, arco.peso));

            var caminho = Path.Combine(Directory.GetCurrentDirectory(), "bkpSimulacao");
            File.WriteAllText(caminho, sb.ToString());

        }

        private string FormatarStringLugares(List<Lugar> lugares)
        {
            string retorno = "";
            foreach (var entrada in lugares)
            {
                retorno += entrada.Nome;
                if (entrada.Nome != lugares.Last().Nome)
                    retorno += "-";
            }

            return retorno;
        }

        private enum eTipoDado { Nenhum, Lugar, Transicao, Arco }
        public void CarregarSimulacao(string caminho)
        {
            var tipDado = eTipoDado.Nenhum;
            foreach (var linha in File.ReadAllLines(caminho))
            {
                if (linha == "#LUGARES")
                {
                    tipDado = eTipoDado.Lugar;
                    continue;
                }
                if (linha == "#TRANSIÇÕES")
                {
                    tipDado = eTipoDado.Transicao;
                    continue;
                }
                if (linha == "#ARCOS")
                {
                    tipDado = eTipoDado.Arco;
                    continue;
                }

                switch (tipDado)
                {
                    case eTipoDado.Lugar:
                        var dados = linha.Split(';');
                        lugares.Add(new Lugar(dados[0], Int32.Parse(dados[1])));
                        break;

                    case eTipoDado.Transicao:
                        dados = linha.Split(';');
                        var transicao = new Transicao(dados[0]);                        
                        transicoes.Add(transicao);
                        break;                    

                    case eTipoDado.Arco:
                        dados = linha.Split(';');
                        var arco = new Arco();
                        arco.peso = Int32.Parse(dados[2]);
                        if (dados[0].StartsWith("L"))
                        {
                            var origem = lugares.Find(l => l.Nome == dados[0]);
                            arco.origem = origem;

                            var destino = transicoes.Find(l => l.Nome == dados[1]);
                            arco.destino = destino;

                            origem.arco = arco;
                            destino.arcos.Add(arco);
                        }
                        else
                        {
                            var origem = transicoes.Find(l => l.Nome == dados[0]);
                            arco.origem = origem;

                            
                            var destino = lugares.Find(l => l.Nome == dados[1]);
                            arco.destino = destino;

                            origem.arcos.Add(arco);
                            destino.arco = arco;
                        }
                        arcos.Add(arco);
                        break;

                }

            }
        }
    }
}
