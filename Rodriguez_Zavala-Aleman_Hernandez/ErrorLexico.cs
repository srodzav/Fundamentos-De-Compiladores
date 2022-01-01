using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class ErrorLexico
    {// CLASE PARA GUARDAR LOS LEXEMAS
        public int Linea { get; set; }
        public string Lexema { get; set; }

        public ErrorLexico(int L, string Lex)
        {
            this.Linea = L;
            this.Lexema = Lex;
        }
    }
}
