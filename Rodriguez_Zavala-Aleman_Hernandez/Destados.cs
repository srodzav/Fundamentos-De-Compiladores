using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class Destados
    {
        public List<Destado> Lista;

        public Destados()
        {
            Lista = new List<Destado>();
        }

        public Boolean Contains(Destado destadoEntrante)
        {
            foreach (Destado de in this.Lista)
            {
                if (de.Equals(destadoEntrante))
                {
                    return true;
                }
            }
            return false;
        }

        public void Add(Destado destado)
        {
            this.Lista.Add(destado);
        }

        public int Count()
        {
            return this.Lista.Count();
        }

        public Destado ElementAt(int i)
        {
            return this.Lista.ElementAt(i);
        }

        public Destado Exist(List<Estado> lista)
        {
            foreach (Destado d in Lista)
            {
                if (d.Equals(lista))
                {
                    return d;
                }
            }
            return null;
        }

        public void ChecaFinal(List<int> EstadosFinales)
        {
            foreach (Destado d in Lista)
            {
                foreach (int i in EstadosFinales)
                {
                    if (d.Contains(i))
                    {
                        d.tipo = true;
                    }
                    else
                    {
                        d.tipo = false;
                    }
                }
            }
        }
    }
}
