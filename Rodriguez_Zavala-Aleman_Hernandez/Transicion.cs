using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class Transicion
    {
        public char Simbolo { get; set; }

        public int idEdodes { get; set; }

        public Transicion(char S, int id)
        {
            idEdodes = id;
            Simbolo = S;
        }
    }
}
