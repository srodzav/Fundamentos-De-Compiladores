using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class EstadoAFDL
    {
        public List<Elemento> ElementosEstado;
        public List<TransicionD> Transiciones;
        public int IndiceEstado;

        public EstadoAFDL(List<Elemento> E, int indice)
        {
            ElementosEstado = E;
            Transiciones = new List<TransicionD>();
            IndiceEstado = indice;
        }

        public List<Elemento> DevuelveCadenas(string SimboloABuscar)
        {
            List<Elemento> Resultado = new List<Elemento>();

            foreach (Elemento e in ElementosEstado)
            {
                string c = e.CuerpoProduccion.TrimEnd(' ');
                c.TrimEnd(' ');
                string[] Split = c.Split(' ');
                String ve = c;

                int indexPunto = -1;
                for (int i = 0; i < Split.Length; i++)
                {
                    if (Split[i] == ".")
                    {
                        indexPunto = i;
                        break;
                    }
                }
                if (indexPunto != -1 && indexPunto != Split.Length - 1) //[.][Valor] ---> . a 
                {
                    if (Split[indexPunto + 1] == SimboloABuscar) // X está después del .  ---> [a][P][a][.]
                    {
                        char Aux2 = Split[indexPunto + 1][0];
                        int aux = indexPunto + 1;
                        //Crear cadena nueva, luego mover el punto 1 lugar a la derecha.
                        string R = MoverPuntoDerecha(Split, indexPunto);
                        R.TrimEnd(' ');
                        Resultado.Add(new Elemento(R, e.EncabezadoProduccion));
                    }
                }
            }
            return Resultado;
        }

        public string MoverPuntoDerecha(string[] Cadena, int indicePunto)
        {
            string[] Resultado = new string[Cadena.Length];
            string R = "";
            for (int i = 0; i < Cadena.Length; i++)
            {
                Resultado[i] = Cadena[i];
            }

            Resultado[indicePunto] = Cadena[indicePunto + 1];
            Resultado[indicePunto + 1] = Cadena[indicePunto];

            foreach (string c in Resultado)
            {
                R += c + " ";
            }
            string Aux = R.TrimEnd(' ');
            return (Aux);
        }

        public bool EsIgual(List<Elemento> Candidato)
        {
            int Contador = 0;
            foreach (Elemento e in Candidato)
            {
                string c = e.CuerpoProduccion;
                if (Contiene(c))
                {
                    Contador++;
                }
            }

            if (Contador == ElementosEstado.Count)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Contiene(string Cadena)
        {
            foreach (Elemento e in ElementosEstado)
            {
                if (Cadena == e.CuerpoProduccion)
                {
                    return true;
                }
            }
            return false;
        }
        public void AgregaTransicion(string Simbolo, int Id)
        {
            TransicionD NuevaTransicion = new TransicionD(Simbolo, Id);
            Transiciones.Add(NuevaTransicion);
        }

        public TransicionD getTransicion(string trans)
        {
            foreach (TransicionD tD in this.Transiciones)
            {
                if (tD.S == trans)
                {
                    return tD;
                }
            }
            return null;
        }

        public string getEstadoString()
        {
            String res = "";

            foreach (Elemento e in ElementosEstado)
            {
                string s = e.CuerpoProduccion;
                string Encabezado = e.EncabezadoProduccion;
                res += "\t" + Encabezado + "  -->  " + s + "\n";
            }
            return res;
        }
    }
}
