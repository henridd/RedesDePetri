using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri.Classes
{
    class Arco
    {
        public IObjetoSimuladorPetri origem { get; set; }
        public IObjetoSimuladorPetri destino { get; set; }
        public int peso { get; set; }

        public Arco()
        {
        }

        public Arco(IObjetoSimuladorPetri origem, IObjetoSimuladorPetri destino)
        {
            this.origem = origem;
            this.destino = destino;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", origem.Nome, destino.Nome);
        }
    }
}
