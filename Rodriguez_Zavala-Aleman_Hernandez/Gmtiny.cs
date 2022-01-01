using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    class Gmtiny
    {
        public static HashSet<string> PReservados = new HashSet<string>();
        public static HashSet<string> SEspeciales = new HashSet<string>();
        public static HashSet<string> noterminales = new HashSet<string>();
        public static HashSet<string> terminales;
        
        public AFD identificador;
        public AFD numero;
        
      
        static Gmtiny()
        {
            //Palabras reservadas
            PReservados.Add("if");
            PReservados.Add("then");
            PReservados.Add("else");
            PReservados.Add("end");
            PReservados.Add("repeat");
            PReservados.Add("until");
            PReservados.Add("read");
            PReservados.Add("write");

            //SImbolos Especiales
            SEspeciales.Add("+");
            SEspeciales.Add("-");
            SEspeciales.Add("*");
            SEspeciales.Add("/");
            SEspeciales.Add("=");
            SEspeciales.Add("<");
            SEspeciales.Add(">");
            SEspeciales.Add("(");
            SEspeciales.Add(")");
            SEspeciales.Add(";");
            SEspeciales.Add(":=");

            //Terminales
            terminales = new HashSet<string>(PReservados);
            terminales.UnionWith(SEspeciales);

            //No terminales
            noterminales.Add("programa");
            noterminales.Add("secuencia-sent");
            noterminales.Add("sentencia");
            noterminales.Add("sent-if");
            noterminales.Add("sent-repeat");
            noterminales.Add("sent-assign");
            noterminales.Add("sent-read");
            noterminales.Add("sent-write");
            noterminales.Add("exp");
            noterminales.Add("op-comp");
            noterminales.Add("exp-simple");
            noterminales.Add("opsuma");
            noterminales.Add("term");
            noterminales.Add("opmult");
            noterminales.Add("factor");
        }

        public Gmtiny(AFD identificador, AFD numero)
        {
            this.identificador = identificador;
            this.numero = numero;
        }
    }
}
