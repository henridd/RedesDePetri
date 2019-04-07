using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri.Classes
{
    class Transicao : IObjetoSimuladorPetri
    {
        public string Nome { get; private set; }
        public List<Arco> arcos { get; set; } = new List<Arco>();
        public List<Lugar> lugaresEntrada
        {
            get
            {
                // Pega todas as origens
                var orgigensArcos = arcos.Select(a => a.origem);

                // Retorna todas que são lugar
                return orgigensArcos.OfType<Lugar>().ToList();
            }
        }
        public List<Lugar> lugaresSaida
        {
            get
            {
                // Pega todas as origens
                var destinosArcos = arcos.Select(a => a.destino);

                // Retorna todas que são lugar
                return destinosArcos.OfType<Lugar>().ToList();
            }
        }

        public bool PodeExecutar
        {
            get
            {
                foreach (Lugar lugar in lugaresEntrada)
                    if (lugar.qtdMarcas < lugar.arco.peso)
                        return false;
                return true;
            }

        }

        // TODO: Não repetir os lugares na mesma transicao

        public Transicao()
        {
        }

        public Transicao(string nome) : this()
        {
            this.Nome = nome;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
