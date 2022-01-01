using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class AFD
    {
        public Destados dEstados; // Lista de estados
        AFN AFN; // AFN
        public String alfAFD; // Alfabeto del AFD
        List<Estado> listEpsilon; // Lista de Epsilon
        int cAscii = 0; //65 a 90

        public AFD(AFN afn)
        {
            dEstados = new Destados();
            this.AFN = afn;
            this.alfAFD = afn.alfabeto.Trim('ε');
        }

        public void inicializar()
        {
            Destado toAdd = cEpsilon(AFN.Estados[0]);
            dEstados.Add(toAdd);
            // RECORRIDO PARA LOS DESTADOS
            for (int i = 0; i < this.dEstados.Count(); i++)
            {
                Destado dest = this.dEstados.ElementAt(i);
                if (!dest.bandera)
                {
                    dest.check();
                    // RECORRIDO PARA ALFABETO EN EL AFD
                    foreach (char a in alfAFD)
                    {
                        // SELECCIONA Y MUEVE EL DESTADO CON EL CARACTER DEL AFD
                        List<Estado> moverRes = mover(this.dEstados.ElementAt(i), a);
                        if (moverRes.Count() > 0)
                        {
                            Destado U = cEpsilon(moverRes);
                            // SI NO LO CONTIENE LO AGREGA Y HACE TRANSICION
                            if (!dEstados.Contains(U))
                            {
                                dEstados.Add(U); // AGREGA EL ESTADO
                            }
                            if (!dest.ExistTransicion(U.index))
                            {
                                this.dEstados.ElementAt(i).AddTransicion(U, a);
                            }
                        }
                    }
                }
            }
        }

        public bool ValidaLexema(string Lexema)
        {
            bool bandera = true;
            int indiceCaracter = 0;
            int indiceDEstado = 0;
            while (bandera == true && indiceCaracter < Lexema.Length)
            {
                int Aux = dEstados.Lista[indiceDEstado].ExisteTransicionSimbolo(Lexema[indiceCaracter]);
                if (Aux != -1)
                {
                    indiceDEstado = Aux;
                    indiceCaracter++;
                }
                else
                {
                    return false;
                }
            }
            if (dEstados.Lista[indiceDEstado].tipo == true && Lexema != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<Estado> mover(Destado T, char tr)
        {
            List<Estado> resultado = new List<Estado>();
            // RECORRE LOS ESTADOS
            foreach (Estado e in AFN.Estados)
            {
                if (T.Contains(e.Index))
                {
                    foreach (Transicion t in e.Transiciones)
                    {
                        if (t.Simbolo == tr)
                        {
                            // SI EL SIMBOLO ES IGUAL AL CARACTER DEL AFD LO AGREGA
                            resultado.Add(AFN.getEstadoByIndex(t.idEdodes));
                        }
                    }
                }
            }
            return resultado;
        }

        private Destado cEpsilon(Estado ini)
        { // Si no contiene el estado, agrega a la lista de estados del Destado.
            listEpsilon = new List<Estado>();
            listEpsilon.Add(ini);
            List<Estado> listaInterna;

            listaInterna = new List<Estado>();
            foreach (Transicion t in ini.Transiciones)
            {
                if (t.Simbolo == 'ε')
                {
                    Estado estadoDestino = AFN.getEstadoByIndex(t.idEdodes);
                    if (!listEpsilon.Contains(estadoDestino))
                    {
                        listEpsilon.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                        listaInterna.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                    }
                }
            }
            cEpsilonR(listaInterna);

            Destado dAux = dEstados.Exist(listEpsilon);
            if (dAux != null)
            {
                return dAux;
            }
            else
            {
                return new Destado(listEpsilon, cAscii++);
            }

        }

        private Destado cEpsilon(List<Estado> listaEstados)
        {// PARA LISTA DE ESTADOS
            listEpsilon = new List<Estado>();
            List<Estado> listaInterna;

            foreach (Estado e in listaEstados)
            {
                if (!listEpsilon.Contains(e))
                {
                    listEpsilon.Add(e);
                }
                listaInterna = new List<Estado>();
                foreach (Transicion t in e.Transiciones)
                {
                    if (t.Simbolo == 'ε')
                    {
                        Estado estadoDestino = AFN.getEstadoByIndex(t.idEdodes);
                        if (!listEpsilon.Contains(estadoDestino))
                        {
                            listEpsilon.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                            listaInterna.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                        }
                    }
                }
                cEpsilonR(listaInterna);
            }

            Destado dAux = dEstados.Exist(listEpsilon);
            if (dAux != null)
            {
                return dAux;
            }
            else
            {
                return new Destado(listEpsilon, cAscii++);
            }
        }

        private void cEpsilonR(List<Estado> listaEstados)
        {
            List<Estado> listaInterna;

            foreach (Estado e in listaEstados)
            {
                listaInterna = new List<Estado>();
                foreach (Transicion t in e.Transiciones)
                {
                    if (!listEpsilon.Contains(e))
                    {
                        listEpsilon.Add(e); // hasta que ya no encuentra estados
                    }
                    if (t.Simbolo == 'ε')
                    {
                        Estado estadoDestino = AFN.getEstadoByIndex(t.idEdodes);
                        if (!listEpsilon.Contains(estadoDestino))
                        {
                            listEpsilon.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                            listaInterna.Add(estadoDestino);  // AGREGA EL ESTADO DESTINO
                        }
                    }
                }
                cEpsilonR(listaInterna);
            }
        }

    }
}