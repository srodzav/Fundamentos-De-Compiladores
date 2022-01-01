using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class Estado
    {
        int id { get; set; }
        public int tipo { get; set; } // 0 inicial, 1 neutro, 2 aceptacion
        public List<Transicion> Transiciones;
        public Estado(int id, int tipo)
        {
            this.id = id;
            this.tipo = tipo;
            Transiciones = new List<Transicion>();
        }

        public void cTipo(int newTipo)
        {
            tipo = newTipo;
        }

        public void generaTransicion(char s, int id)
        {
            Transicion nt = new Transicion(s, id);
            Transiciones.Add(nt);
        }

        public int Index
        {
            get { return id; }
            set { id = value; }
        }

        public List<string> generaTabla(string estado)
        {
            List<string> tTransicion = new List<string>();
            foreach (char c in estado)
            {
                string res = "";
                for (int i = 0; i < Transiciones.Count; i++)
                {
                    if (Transiciones[i].Simbolo == c)
                    {
                        res += Transiciones[i].idEdodes;
                        res += "-";
                    }
                }
                if (res.Length > 1)
                {
                    string Aux = res.Remove(res.Length - 1);
                    tTransicion.Add(Aux);
                }
                else
                {
                    tTransicion.Add(res);
                }
            }
            return tTransicion;
        }
    }
}

