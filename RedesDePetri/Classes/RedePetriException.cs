using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedesDePetri.Classes
{
    public class RedePetriException : Exception
    {
        public RedePetriException(String mensagem) : base(mensagem)
        {

        }
    }
}
