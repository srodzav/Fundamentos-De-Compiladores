using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class Operando
    {
        public List<Estado> edosOp;

        public Operando(List<Estado> E)
        {
            edosOp = E;
        }

        public Estado GetEstado(int id)
        {
            return edosOp[id];
        }

        public void idEdoinc()
        {
            foreach (Estado c in edosOp)
            {
                c.Index++;
                foreach (Transicion t in c.Transiciones)
                {
                    t.idEdodes++;
                }
            }
        }

        public void idEdosup(int IndexEstado, int IndexEstadoDestino)
        {
            Estado eNew = new Estado(-1, -1);
            foreach (Estado e in edosOp)
            {
                if (e.Index == IndexEstado)
                {
                    eNew = e;
                }
            }

            if (eNew.Index != -1)
            {
                for (int i = 0; i < eNew.Transiciones.Count; i++)
                {
                    if (eNew.Transiciones[i].idEdodes == IndexEstadoDestino)
                    {
                        eNew.Transiciones.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        public void idEdodec()
        {
            foreach (Estado c in edosOp)
            {
                c.Index--;
                foreach (Transicion t in c.Transiciones)
                {
                    t.idEdodes--;
                }
            }
        }

        public char getTransSimbol(int IndexEstado, int IndexEstadoDestino)
        {
            Estado edoInicio = new Estado(-1, -1);
            foreach (Estado e in edosOp)
            {
                if (e.Index == IndexEstado)
                {
                    edoInicio = e;
                }
            }
            if (edoInicio.Index != -1)
            {
                for (int i = 0; i < edoInicio.Transiciones.Count; i++)
                {
                    if ((edoInicio.Transiciones[i].idEdodes) == IndexEstadoDestino)
                    {
                        return edoInicio.Transiciones[i].Simbolo;
                    }
                }
            }
            return '/';
        }
        
        public int edoToString()
        {
            return edosOp.Count;
        }
    }
}
