using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public class AFN
    {
        public List<Estado> estados;

        // REFERENCIAS PARA OPERADORES
        public String operadores = "*+?&|";
        protected string opUnarios = "*+?";
        protected string opBinarios = "&|C";
        public string alfabeto = "";
        protected int cEstados = 1;

        public List<Estado> Estados { get { return this.estados; } }

        public AFN(String posfija)
        {
            estados = new List<Estado>();
            alfabeto = Alfabeto(posfija) + "ε";
        }

        public Estado getEstadoByIndex(int Index)
        {
            foreach (Estado e in this.estados)
            {
                if (e.Index == Index)
                {
                    return e;
                }
            }
            return null;
        }

        // Funcion que guarda la regex posfija en un string y le concatena epsilon
        public string Alfabeto(String posfija)
        {
            string res = "";
            foreach (char c in posfija)
            {
                if (!operadores.Contains(c) && !res.Contains(c))
                {
                    res += c;
                }
            }
            return res;
        }

        #region AUTOMATA

        // Funcion que permite evaluar la expresion y realizar las funciones
        public Operando automata(String posfija)
        {
            Stack<Operando> PilaOperandos = new Stack<Operando>();
            foreach (char c in posfija)
            { // Por cada caracter en la regex posfija
                Operando Resultado = new Operando(new List<Estado>());
                if (alfabeto.Contains(c))
                { // Se agrega a la pila cada caracter siempre y cuando sea a-z
                    Operando NuevoOperando = AgregaCaracter(c);
                    PilaOperandos.Push(NuevoOperando);
                }
                else
                { // Dependiendo del operador evalua si es unario o binario
                    if (opUnarios.Contains(c))
                    {
                        Operando O = PilaOperandos.Pop();
                        switch (c)
                        {
                            case '*':
                                Resultado = Kleen(O);
                                break;
                            case '+':
                                Resultado = Positiva(O);
                                break;
                            case '?':
                                Resultado = CeroUno(O);
                                break;
                        }
                        PilaOperandos.Push(Resultado);
                    }
                    else if (opBinarios.Contains(c))
                    { // Genera dos operandos los cuales extrae de la pila y realiza concatenacion o union
                        Operando O2 = PilaOperandos.Pop();
                        Operando O1 = PilaOperandos.Pop();
                        switch (c)
                        {
                            case '&':
                                Resultado = Concatenacion(O1, O2);
                                break;
                            case '|':
                                Resultado = Union(O1, O2);
                                break;
                        }
                        PilaOperandos.Push(Resultado);
                    }
                }
            }
            Operando R = PilaOperandos.Pop();
            return R;
        }

        public Operando AgregaCaracter(char C)
        {
            Estado edoInicial = new Estado(cEstados, 0); // Crea un estado inical
            cEstados++;
            Estado edoAceptacion = new Estado(cEstados, 2); // Crea un estado de aceptacion
            cEstados++;
            edoInicial.generaTransicion(C, edoAceptacion.Index);

            List<Estado> liEdos = new List<Estado>();
            liEdos.Add(edoInicial);
            liEdos.Add(edoAceptacion);
            Operando nvOperador = new Operando(liEdos);

            return nvOperador;
        }

        public Operando Concatenacion(Operando Op1, Operando Op2)
        {
            // Crea un operador y se le asigna una lista de estados
            Operando nvOperador = new Operando(new List<Estado>());

            int idEdoaOp1 = Op1.edosOp[Op1.edoToString() - 1].Index;
            char sTrans = Op1.getTransSimbol(Op1.edosOp[Op1.edoToString() - 2].Index, idEdoaOp1);
            Op1.idEdosup(Op1.edosOp[Op1.edoToString() - 2].Index, Op1.edosOp[Op1.edoToString() - 1].Index);
            Op1.edosOp.RemoveAt(Op1.edoToString() - 1);
            Op2.idEdodec();

            int idEdoaOp2 = Op2.edosOp[0].Index;
            Op1.edosOp[Op1.edoToString() - 1].generaTransicion(sTrans, idEdoaOp2);

            foreach (Estado e in Op1.edosOp)
                nvOperador.edosOp.Add(e);
            foreach (Estado e in Op2.edosOp)
                nvOperador.edosOp.Add(e);

            cEstados = Op2.edosOp[Op2.edoToString() - 1].Index + 1;
            return nvOperador;
        }

        public Operando Union(Operando Op1, Operando Op2)
        {
            Operando nvOperador = new Operando(new List<Estado>());

            int idInicio = Op1.edosOp[0].Index;
            Op1.idEdoinc();
            Op2.idEdoinc();
            Op1.edosOp[Op1.edoToString() - 1].cTipo(1);
            Op2.edosOp[Op2.edoToString() - 1].cTipo(1);
            cEstados = Op2.edosOp[Op2.edoToString() - 1].Index + 1;

            Estado nvoInicio = new Estado(idInicio, 0);
            Estado nvoAceptacion = new Estado(cEstados, 2);
            cEstados++;

            nvoInicio.generaTransicion('ε', Op1.GetEstado(0).Index);
            nvoInicio.generaTransicion('ε', Op2.GetEstado(0).Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', nvoAceptacion.Index);
            Op2.GetEstado(Op2.edoToString() - 1).generaTransicion('ε', nvoAceptacion.Index);
            nvOperador.edosOp.Add(nvoInicio);

            foreach (Estado e in Op1.edosOp)
                nvOperador.edosOp.Add(e);
            foreach (Estado e in Op2.edosOp)
                nvOperador.edosOp.Add(e);

            nvOperador.edosOp.Add(nvoAceptacion);
            return nvOperador;
        }

        public Operando Positiva(Operando Op1)
        {
            Operando nvOperador = new Operando(new List<Estado>());

            int idNuevo = Op1.GetEstado(0).Index;
            Op1.idEdoinc();
            Op1.GetEstado(0).cTipo(1);
            Op1.GetEstado(Op1.edoToString() - 1).cTipo(1);

            Estado nvoInicio = new Estado(idNuevo, 0);
            cEstados = Op1.GetEstado(Op1.edoToString() - 1).Index + 1;
            Estado nvoAceptacion = new Estado(cEstados, 2);
            cEstados++;

            nvoInicio.generaTransicion('ε', Op1.GetEstado(0).Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', Op1.GetEstado(0).Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', nvoAceptacion.Index);

            nvOperador.edosOp.Add(nvoInicio);
            foreach (Estado e in Op1.edosOp)
                nvOperador.edosOp.Add(e);

            nvOperador.edosOp.Add(nvoAceptacion);
            return nvOperador;
        }

        public Operando CeroUno(Operando Op1)
        {
            Operando nvOperador = new Operando(new List<Estado>());

            int idNuevo = Op1.GetEstado(0).Index;
            Op1.idEdoinc();
            Op1.GetEstado(0).cTipo(1);
            Op1.GetEstado(Op1.edoToString() - 1).cTipo(1);

            Estado nvoInicio = new Estado(idNuevo, 0);
            cEstados = Op1.GetEstado(Op1.edoToString() - 1).Index + 1;
            Estado nvoAceptacion = new Estado(cEstados, 2);
            cEstados++;

            nvoInicio.generaTransicion('ε', Op1.GetEstado(0).Index);
            nvoInicio.generaTransicion('ε', nvoAceptacion.Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', nvoAceptacion.Index);

            nvOperador.edosOp.Add(nvoInicio);
            foreach (Estado e in Op1.edosOp)
                nvOperador.edosOp.Add(e);

            nvOperador.edosOp.Add(nvoAceptacion);
            return nvOperador;
        }

        public Operando Kleen(Operando Op1)
        {
            Operando nvOperador = new Operando(new List<Estado>());

            int idNuevo = Op1.GetEstado(0).Index;
            Op1.idEdoinc();
            Op1.GetEstado(0).cTipo(1);
            Op1.GetEstado(Op1.edoToString() - 1).cTipo(1);

            Estado nvoInicio = new Estado(idNuevo, 0);
            cEstados = Op1.GetEstado(Op1.edoToString() - 1).Index + 1;
            Estado nvoAceptacion = new Estado(cEstados, 2);
            cEstados++;

            nvoInicio.generaTransicion('ε', Op1.GetEstado(0).Index);
            nvoInicio.generaTransicion('ε', nvoAceptacion.Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', Op1.GetEstado(0).Index);
            Op1.GetEstado(Op1.edoToString() - 1).generaTransicion('ε', nvoAceptacion.Index);

            nvOperador.edosOp.Add(nvoInicio);
            foreach (Estado e in Op1.edosOp)
                nvOperador.edosOp.Add(e);

            nvOperador.edosOp.Add(nvoAceptacion);
            return nvOperador;
        }

        #endregion

        #region TINY

        public List<int> RegresaFinales()
        {
            List<int> Lista = new List<int>();
            foreach (Estado e in estados)
            {
                if (e.tipo == 2)
                {
                    Lista.Add(e.Index);
                }
            }
            return Lista;
        }

        #endregion 
    }
}
