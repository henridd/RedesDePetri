using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri.Classes
{
    class Lugar : IObjetoSimuladorPetri
    {
        public string Nome { get; private set; }
        public int qtdMarcas { get; set; }
        public Arco arco { get; set; }

        public Lugar()
        {
        }

        public Lugar(string nome, int qtdMarcas)
        {
            this.Nome = nome;
            this.qtdMarcas = qtdMarcas;
        }

        public void RetiraMarca()
        {
            if (arco.peso > qtdMarcas)
            {
                throw new RedePetriException("Quantidade de marcas indisponivel");
            }
            qtdMarcas -= arco.peso;
        }

        public void AdicionaMarca()
        {
            qtdMarcas += arco.peso;
        }

        public override string ToString()
        {
            return Nome;
        }
    }
}
