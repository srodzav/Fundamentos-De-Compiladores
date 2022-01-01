using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class Destado
    {
        public List<Estado> lEdosAFN;
        public Boolean bandera;
        public List<TransicionD> listaTransiciones = new List<TransicionD>();
        public int index;
        public bool tipo;

        public Destado(List<Estado> listaEstadosEnAFN, int ix)
        {
            this.lEdosAFN = listaEstadosEnAFN;
            bandera = false;
            index = ix;
        }

        public void check()
        {
            bandera = true;
        }
        public void AddTransicion(Destado d, char simbolo)
        {
            listaTransiciones.Add(new TransicionD(simbolo, d.index));
        }
        public Boolean Contains(int Index)
        {
            foreach (Estado e in this.lEdosAFN)
            {
                if (e.Index == Index)
                {
                    return true;
                }
            }
            return false;
        }
        private String getStringEstadoDestinoTransicion(char transicion)
        {
            foreach (TransicionD t in this.listaTransiciones)
            {
                if (t.Simbolo == transicion)
                {
                    return t.indiceDest.ToString();
                }
            }
            return "Ø";
        }
        public String[] getRowTransiciones(String alfabeto)
        {
            List<String> res = new List<string>();
            res.Add(this.index.ToString());
            foreach (char c in alfabeto)
            {
                res.Add(getStringEstadoDestinoTransicion(c));
            }
            return res.ToArray();
        }
        public int ExisteTransicionSimbolo(char Simbolo)
        {
            int Indice = -1;
            for (int i = 0; i < listaTransiciones.Count; i++)
            {
                if (listaTransiciones[i].Simbolo == Simbolo)
                {
                    Indice = listaTransiciones[i].indiceDest;
                    return Indice;
                }

            }
            if (Indice == -1)
            {
                for (int i = 0; i < listaTransiciones.Count; i++)
                {
                    if (listaTransiciones[i].Simbolo == '.')
                    {
                        Indice = listaTransiciones[i].indiceDest;
                        return Indice;
                    }
                }
            }
            return Indice;
        }

        #region EQUALS

        public Boolean Equals(Destado comparacion)
        {
            List<int> listaEstadosIndexThis = this.lEdosAFN_Index();
            if (comparacion.lEdosAFN.Count() == this.lEdosAFN.Count())
            {
                foreach (int index in comparacion.lEdosAFN_Index())
                {
                    if (!listaEstadosIndexThis.Contains(index))
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean Equals(List<Estado> listaEstados)
        {
            if (listaEstados.Count() == this.lEdosAFN.Count())
            {
                foreach (Estado e in listaEstados)
                {
                    if (!this.Contains(e.Index))
                    {
                        return false;
                    }
                }

                return true;
            }
            else
            {
                return false;
            }
        }
        public Boolean ExistTransicion(int IndiceTransicion)
        {
            foreach (TransicionD t in listaTransiciones)
            {
                if (t.indiceDest == IndiceTransicion)
                {
                    return true;
                }
            }
            return false;
        }
        public List<int> lEdosAFN_Index()
        {
            List<int> res = new List<int>();
            foreach (Estado e in this.lEdosAFN)
            {
                res.Add(e.Index);
            }
            return res;
        }

        #endregion
    }
}
