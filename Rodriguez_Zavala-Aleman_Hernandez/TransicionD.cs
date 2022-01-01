using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class TransicionD
    {
        public char Simbolo { get; set; }

        public int indiceDest { get; set; }

        public string S { get; set; }

        public TransicionD(char transicion, int indiceDestino)//es una transicion para AFD
        {
            Simbolo = transicion;
            indiceDest = indiceDestino;
        }

        public TransicionD(string transicion, int indiceDestino)
        {
            S = transicion;
            indiceDest = indiceDestino;
        }
    }
}

