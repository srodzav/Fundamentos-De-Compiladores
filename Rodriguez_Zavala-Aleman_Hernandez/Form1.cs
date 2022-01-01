using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    public partial class Form1 : Form
    {
        Posfija p = new Posfija();
        AFD aFD;

        static List<string> PalabrasReservadas = new List<string>()
        {
            "if",
            "then",
            "else",
            "end",
            "repeat",
            "until",
            "read",
            "write"
        };

        static List<string> SimbolosEspeciales = new List<string>()
        {
            "+",
            "-",
            "*",
            "/",
            "=",
            "<",
            ">",
            "(",
            ")",
            ";",
            ":="
        };

        public Form1()
        {
            InitializeComponent();
        }

        #region POSFIJA
        private void button1_Click(object sender, EventArgs e)
        {
            txtPostfija.Text = p.cPosfija(txtInfija.Text);
            txtDebug.Text = p.PosConversion(txtInfija.Text);
        }

        #region POSTFIJA 2

        static String caracteresNumericos = "0123456789";
        static String caracteresAlfabeticos = "abcdefghijklmnñopqrstuvxyz";
        public static String alfabeto = caracteresNumericos + caracteresAlfabeticos + ".";
        static String caracteresOtros2 = "[]- ";
        static String op_Presedecia1 = "*+?";
        static String op_Presedecia2 = "&";
        static String op_Presedecia3 = "|";
        static String op = op_Presedecia1 + op_Presedecia2 + op_Presedecia3;
        static List<string> Operadores = new List<String>() { op_Presedecia3, op_Presedecia2, op_Presedecia1 };

        private string ConversionPosfija(string Infija)
        {
            String posFija = "";
            Stack<char> pila = new Stack<char>();
            String infija = FormateoExR(Infija);
            Boolean error = false;
            int contadorInfija = 0;
            Boolean band = false;

            if (infija == "")
            {
                error = true;
            }

            while (!error && contadorInfija < infija.Length)
            {
                char caracter = infija[contadorInfija];
                switch (caracter)
                {
                    case ' ':
                        break;
                    case '\n':
                        while (pila.Any())
                        {
                            if (pila.Peek() != '(' && pila.Peek() != ')')
                            {
                                posFija += pila.Pop();
                            }
                            else
                            {
                                pila.Pop();
                            }
                        }
                        break;
                    case '(':
                        pila.Push(caracter);
                        break;
                    case ')':
                        while (pila.Peek() != '(')
                        {
                            posFija += pila.Pop();
                        }
                        pila.Pop();
                        break;
                    default:
                        if (alfabeto.Contains(caracter))
                        {
                            posFija += caracter;
                        }
                        else
                        {
                            if (op.Contains(caracter))
                            {
                                band = true;
                                while (band)
                                {
                                    if (!pila.Any() || pila.Peek() == '(' || prioridad(caracter, pila.Peek()))
                                    {
                                        pila.Push(caracter);
                                        band = false;
                                    }
                                    else
                                    {
                                        posFija += pila.Pop();
                                    }
                                }
                            }
                            else
                            {
                                error = true;
                            }
                        }
                        break;
                }
                contadorInfija++;
            }
            if (!error)
            {
                while (pila.Any())
                {
                    posFija += pila.Pop();
                }
            }

            return posFija;
        }

        private string FormateoExR(string ExpresionRegular)
        {
            ExpresionRegular = limpiaExpresion(ExpresionRegular);
            String Resultado = desgloseCorchetes(ExpresionRegular);
            return Resultado;
        }

        private Boolean prioridad(char c1, char c2)
        {
            int indiceMayor = -1;
            int indiceMenor = -1;
            int i = 0;
            foreach (string c in Operadores)
            {

                if (c.Contains(c1))
                {
                    indiceMayor = i;
                    break;
                }
                i++;
            }

            i = 0;
            foreach (string c in Operadores)
            {
                if (c.Contains(c2))
                {
                    indiceMenor = i;
                    break;
                }
                i++;
            }
            if (indiceMayor > indiceMenor)
            { return true; }
            else if (indiceMayor < indiceMenor)
            { return false; }
            else
            { return false; }
        }

        public String desgloseCorchetes(String expresion)
        {
            String resultado = "";
            Stack<int> corchetesIzquierdos = new Stack<int>();
            Stack<String> expresionesResultantes = new Stack<String>();
            List<int> indicesIzquierdos = new List<int>();
            List<int> indicesDerechos = new List<int>();

            for (int i = 0; i < expresion.Length; i++)
            {
                char caracterDeTurno = expresion.ElementAt(i);//caracter de turno dentro de la expresion

                if (caracterDeTurno == '[' || caracterDeTurno == '(')//corchete izquierdo
                {
                    corchetesIzquierdos.Push(i);
                }
                else if (alfabeto.Contains(caracterDeTurno))//es un caracter-> operando 
                {
                    expresionesResultantes.Push(caracterDeTurno.ToString());
                }

                else if (op_Presedecia1.Contains(caracterDeTurno))//es un operador unario
                {
                    String en = "";
                    en = expresionesResultantes.Pop() + caracterDeTurno;
                    expresionesResultantes.Push(en);
                }
                else if (op_Presedecia2.Contains(caracterDeTurno))//es un operador &
                {

                }
                else if (op_Presedecia3.Contains(caracterDeTurno))//es un operador |
                {
                    expresionesResultantes.Push(caracterDeTurno.ToString());
                }
                else if (caracterDeTurno == ']' || caracterDeTurno == ')')//corchete derecho
                {
                    int indiceInicial = corchetesIzquierdos.Pop();

                    if (expresion.Substring(indiceInicial, i - indiceInicial).Contains('-') && !expresion.Substring(indiceInicial + 1, i - indiceInicial - 1).Contains('[') && !expresion.Substring(indiceInicial + 1, i - indiceInicial - 1).Contains(']'))//es una secuencoa de caracteres
                    {
                        String inicial = "";
                        String final = "";
                        int indiceGuion = expresion.IndexOf('-', indiceInicial);

                        for (int j = i - 1; j > indiceInicial; j--)
                        {
                            char car = expresion.ElementAt(j);
                            if (alfabeto.Contains(car))
                            {
                                if (j > indiceGuion)
                                {
                                    final = expresionesResultantes.Pop() + final;
                                }
                                else
                                {
                                    inicial = expresionesResultantes.Pop() + inicial;
                                }
                            }
                        }
                        expresionesResultantes.Push(desgloseSecuencialCorchetes(inicial, final));
                    }
                    else if (caracterDeTurno == ')')
                    {
                        String aux = ")";
                        for (int j = i - 1; j > indiceInicial; j--)
                        {
                            char car = expresion.ElementAt(j);

                            if (alfabeto.Contains(car))
                            {
                                if (op_Presedecia3.Contains(expresion.ElementAt(j - 1)))
                                {
                                    aux = expresionesResultantes.Pop() + aux;
                                }
                                else
                                {
                                    aux = "&" + expresionesResultantes.Pop() + aux;
                                }
                            }
                            else if (op_Presedecia3.Contains(car))
                            {
                                String alaDerecha = expresionesResultantes.Pop();
                                String aLaIzquierda = expresionesResultantes.Pop();
                                expresionesResultantes.Push(aLaIzquierda + alaDerecha);
                            }
                            if (indicesDerechos.Contains(j))
                            {
                                int auxIndex = indicesDerechos.IndexOf(j);
                                int nuevoJ = indicesIzquierdos.ElementAt(auxIndex);
                                indicesDerechos.RemoveAt(auxIndex);
                                indicesIzquierdos.RemoveAt(auxIndex);
                                j = nuevoJ;
                                aux = "&" + expresionesResultantes.Pop() + aux;
                            }
                        }
                        aux = "(" + aux.Substring(1, aux.Length - 1);
                        expresionesResultantes.Push(aux);
                    }
                    else
                    {
                        String aux = ")";
                        for (int j = i - 1; j > indiceInicial; j--)
                        {
                            char car = expresion.ElementAt(j);

                            if (alfabeto.Contains(car))
                            {
                                aux = "|" + expresionesResultantes.Pop() + aux;
                            }
                            else if (op_Presedecia3.Contains(car))
                            {
                                String alaDerecha = expresionesResultantes.Pop();
                                String aLaIzquierda = expresionesResultantes.Pop();
                                expresionesResultantes.Push(aLaIzquierda + alaDerecha);
                            }
                            if (indicesDerechos.Contains(j))
                            {
                                int auxIndex = indicesDerechos.IndexOf(j);
                                int nuevoJ = indicesIzquierdos.ElementAt(auxIndex);
                                indicesDerechos.RemoveAt(auxIndex);
                                indicesIzquierdos.RemoveAt(auxIndex);
                                j = nuevoJ;
                                aux = "|" + expresionesResultantes.Pop() + aux;
                            }
                        }
                        aux = "(" + aux.Substring(1, aux.Length - 1);
                        expresionesResultantes.Push(aux);
                    }
                    indicesIzquierdos.Add(indiceInicial);
                    indicesDerechos.Add(i);
                }
                else
                { }
            }
            while (expresionesResultantes.Any())
            {
                String resultanteTurno = expresionesResultantes.Pop();
                if (expresionesResultantes.Any())
                {
                    if (expresionesResultantes.Peek() == "|")
                    {
                        expresionesResultantes.Pop();
                        expresionesResultantes.Push(expresionesResultantes.Pop() + "|" + resultanteTurno);
                    }
                    else
                    {
                        expresionesResultantes.Push(expresionesResultantes.Pop() + "&" + resultanteTurno);
                    }
                }
                else
                {
                    resultado = resultanteTurno;
                }
            }
            return resultado;
        }
        public String desgloseSecuencialCorchetes(string primerCaracter, string SegundoCaracter)
        {
            String res = "(";
            int aux;
            //Caracteres Alfabeticos.
            if (caracteresAlfabeticos.Contains(primerCaracter) && caracteresAlfabeticos.Contains(SegundoCaracter))
            {
                if (primerCaracter.Length == 1 && SegundoCaracter.Length == 1)
                {
                    if (caracteresAlfabeticos.IndexOf(primerCaracter) < caracteresAlfabeticos.IndexOf(SegundoCaracter))//el orden de caracteres es creciente
                    {
                        for (int z = caracteresAlfabeticos.IndexOf(primerCaracter); z <= caracteresAlfabeticos.IndexOf(SegundoCaracter); z++)
                        {
                            if (z < caracteresAlfabeticos.IndexOf(SegundoCaracter))
                                res += caracteresAlfabeticos.ElementAt(z) + "|";
                            else
                                res += caracteresAlfabeticos.ElementAt(z) + ")";
                        }
                    }
                    else
                    {
                        for (int z = caracteresAlfabeticos.IndexOf(primerCaracter); z >= caracteresAlfabeticos.IndexOf(SegundoCaracter); z--)
                        {
                            if (z > caracteresAlfabeticos.IndexOf(SegundoCaracter))
                                res += caracteresAlfabeticos.ElementAt(z) + "|";
                            else
                                res += caracteresAlfabeticos.ElementAt(z) + ")";
                        }
                    }
                }
                else
                { }
            }
            else if (Int32.TryParse(primerCaracter, out aux) && Int32.TryParse(SegundoCaracter, out aux))
            {
                int primerCaracterDig;
                Int32.TryParse(primerCaracter, out primerCaracterDig);
                int segundoCaracterDig;
                Int32.TryParse(SegundoCaracter, out segundoCaracterDig);

                if (segundoCaracterDig < primerCaracterDig)
                {
                    for (int j = primerCaracterDig; j >= segundoCaracterDig; j--)
                    {
                        res += j.ToString() + "|";
                    }
                }
                else
                {
                    for (int j = primerCaracterDig; j <= segundoCaracterDig; j++)
                    {
                        res += j.ToString() + "|";
                    }
                }

                res = res.Substring(0, res.Length - 1) + ')';
            }
            else
            {
                MessageBox.Show("los caracteres entre corchetes no coinciden en tipo");
            }
            return res;
        }

        String limpiaExpresion(String expresion)
        {
            String res = "";
            expresion = expresion.Trim();
            foreach (char caracter in expresion)
            {
                if (!alfabeto.Contains(caracter) && !op.Contains(caracter) && !caracteresOtros2.Contains(caracter) && caracter != ')' && caracter != '(')
                {
                    MessageBox.Show("Almenos un caracter de la Expresion no corresponde con el alfabeto");
                }
                else if (caracter != ' ')
                {
                    res += caracter;
                }
            }
            return res;
        }

        #endregion

        #endregion

        #region REGEX TO AFN
        private void button2_Click(object sender, EventArgs e)
        {
            txtPostfija1.Text = p.cPosfija(txtInfija2.Text);
            txtDebug2.Text = p.PosConversion(txtInfija2.Text);
        }

        private void btnTABLA_Click(object sender, EventArgs e)
        {
            AFN AFN = new AFN(txtPostfija1.Text);
            Operando afn_res = AFN.automata(txtPostfija1.Text);
            generaTabla(afn_res, AFN.alfabeto);
            AFN.estados = afn_res.edosOp;
        }

        public void generaTabla(Operando AFN, string alfabeto)
        {
            DGV_AFN.Columns.Clear();
            DGV_AFN.Rows.Clear();
            DGV_AFN.Columns.Add("Estado", "Estado");
            foreach (char c in alfabeto)
            {
                DGV_AFN.Columns.Add(c.ToString(), c.ToString());
            }

            for (int i = 0; i < AFN.edosOp.Count; i++)
            {
                DGV_AFN.Rows.Add();
                DGV_AFN.Rows[i].Cells[0].Value = AFN.edosOp[i].Index;
                List<string> TablaTransiciones = AFN.edosOp[i].generaTabla(alfabeto);
                for (int j = 0; j < TablaTransiciones.Count; j++)
                {
                    string[] CadenaSplit = TablaTransiciones[j].Split('-');

                    string Valor = "";
                    string CadenaAux = "{";
                    foreach (string c in CadenaSplit)
                    {
                        CadenaAux += c + ",";
                    }
                    Valor = CadenaAux.Remove(CadenaAux.Length - 1);
                    Valor += "}";
                    if (Valor != "{}")
                    {
                        DGV_AFN.Rows[i].Cells[j + 1].Value = Valor;
                    }
                    else
                    {
                        DGV_AFN.Rows[i].Cells[j + 1].Value = "Ø";
                    }
                }
            }
        }

        #endregion

        #region AFD TO AFN

        private void button4_Click(object sender, EventArgs e)
        {
            txtPostfija3.Text = p.cPosfija(txtInfija3.Text);
            txtDebug3.Text = p.PosConversion(txtInfija3.Text);
        }

        private void btnAFN_Click(object sender, EventArgs e)
        {
            AFN AFN3 = new AFN(txtPostfija3.Text);
            Operando afn_res3 = AFN3.automata(txtPostfija3.Text);
            generaTabla3(afn_res3, AFN3.alfabeto);
            AFN3.estados = afn_res3.edosOp;
        }

        public void generaTabla3(Operando AFN, string alfabeto)
        {
            dataGrid_AFN3.Columns.Clear();
            dataGrid_AFN3.Rows.Clear();
            dataGrid_AFN3.Columns.Add("Estado", "Estado");
            foreach (char c in alfabeto)
            {
                dataGrid_AFN3.Columns.Add(c.ToString(), c.ToString());
            }

            for (int i = 0; i < AFN.edosOp.Count; i++)
            {
                dataGrid_AFN3.Rows.Add();
                dataGrid_AFN3.Rows[i].Cells[0].Value = AFN.edosOp[i].Index;
                List<string> TablaTransiciones = AFN.edosOp[i].generaTabla(alfabeto);
                for (int j = 0; j < TablaTransiciones.Count; j++)
                {
                    string[] CadenaSplit = TablaTransiciones[j].Split('-');

                    string Valor = "";
                    string CadenaAux = "{";
                    foreach (string c in CadenaSplit)
                    {
                        CadenaAux += c + ",";
                    }
                    Valor = CadenaAux.Remove(CadenaAux.Length - 1);
                    Valor += "}";
                    if (Valor != "{}")
                    {
                        dataGrid_AFN3.Rows[i].Cells[j + 1].Value = Valor;
                    }
                    else
                    {
                        dataGrid_AFN3.Rows[i].Cells[j + 1].Value = "Ø";
                    }
                }
            }
        }

        private void btnAFD_Click(object sender, EventArgs e)
        {
            AFN AFN3 = new AFN(txtPostfija3.Text);
            Operando AFNResultante = AFN3.automata(txtPostfija3.Text);
            AFN3.estados = AFNResultante.edosOp;

            AFD afd = new AFD(AFN3);
            afd.inicializar();
            generaTablaAFD(afd);
            aFD = afd;
        }

        public void generaTablaAFD(AFD afd)
        { // GENERA LA TABLA
            dataGrid_AFD3.Columns.Clear();
            dataGrid_AFD3.Rows.Clear();
            dataGrid_AFD3.Columns.Add("Estado", "Estado");
            foreach (char c in afd.alfAFD)
            {
                dataGrid_AFD3.Columns.Add(c.ToString(), c.ToString());
            }

            foreach (Destado d in afd.dEstados.Lista)
            {
                dataGrid_AFD3.Rows.Add(d.getRowTransiciones(afd.alfAFD));
            }
        }

        #endregion

        #region LEXEMA

        private void button6_Click(object sender, EventArgs e)
        {
            txtPostfija4.Text = p.cPosfija(txtInfija4.Text);
            txtDebug4.Text = p.PosConversion(txtInfija4.Text);
        }

        private void btnAFN2_Click(object sender, EventArgs e)
        {
            AFN AFN4 = new AFN(txtPostfija4.Text);
            Operando afn_res4 = AFN4.automata(txtPostfija4.Text);
            generaTabla4(afn_res4, AFN4.alfabeto);
            AFN4.estados = afn_res4.edosOp;
        }

        public void generaTabla4(Operando AFN, string alfabeto)
        {
            dataGrid_AFN4.Columns.Clear();
            dataGrid_AFN4.Rows.Clear();
            dataGrid_AFN4.Columns.Add("Estado", "Estado");
            foreach (char c in alfabeto)
            {
                dataGrid_AFN4.Columns.Add(c.ToString(), c.ToString());
            }

            for (int i = 0; i < AFN.edosOp.Count; i++)
            {
                dataGrid_AFN4.Rows.Add();
                dataGrid_AFN4.Rows[i].Cells[0].Value = AFN.edosOp[i].Index;
                List<string> TablaTransiciones = AFN.edosOp[i].generaTabla(alfabeto);
                for (int j = 0; j < TablaTransiciones.Count; j++)
                {
                    string[] CadenaSplit = TablaTransiciones[j].Split('-');

                    string Valor = "";
                    string CadenaAux = "{";
                    foreach (string c in CadenaSplit)
                    {
                        CadenaAux += c + ",";
                    }
                    Valor = CadenaAux.Remove(CadenaAux.Length - 1);
                    Valor += "}";
                    if (Valor != "{}")
                    {
                        dataGrid_AFN4.Rows[i].Cells[j + 1].Value = Valor;
                    }
                    else
                    {
                        dataGrid_AFN4.Rows[i].Cells[j + 1].Value = "Ø";
                    }
                }
            }
        }

        private void btnAFD2_Click(object sender, EventArgs e)
        {
            AFN AFN4 = new AFN(txtPostfija4.Text);
            Operando AFNResultante = AFN4.automata(txtPostfija4.Text);
            AFN4.estados = AFNResultante.edosOp;

            AFD afd = new AFD(AFN4);
            afd.inicializar();
            generaTablaAFD2(afd);
            aFD = afd;
        }

        public void generaTablaAFD2(AFD afd)
        { // GENERA LA TABLA
            dataGrid_AFD4.Columns.Clear();
            dataGrid_AFD4.Rows.Clear();
            dataGrid_AFD4.Columns.Add("Estado", "Estado");
            foreach (char c in afd.alfAFD)
            {
                dataGrid_AFD4.Columns.Add(c.ToString(), c.ToString());
            }

            foreach (Destado d in afd.dEstados.Lista)
            {
                dataGrid_AFD4.Rows.Add(d.getRowTransiciones(afd.alfAFD));
            }
        }

        private void btn_lexema_Click(object sender, EventArgs e)
        {
            AFN AFN4 = new AFN(txtPostfija4.Text);
            Operando AFNResultante = AFN4.automata(txtPostfija4.Text);
            AFN4.estados = AFNResultante.edosOp;

            AFD afd = new AFD(AFN4);
            afd.inicializar();

            List<int> ListaAceptacion = AFN4.RegresaFinales();
            afd.dEstados.ChecaFinal(ListaAceptacion);
            bool res = afd.ValidaLexema(lexema.Text);
            if (res == true)
            {
                label_lexema.ForeColor = Color.Black;
                label_lexema.Text = "El lexema si pertenece al lenguaje de la expresión regular";

            }
            else
            {
                label_lexema.ForeColor = Color.Red;
                label_lexema.Text = "El lexema no pertenece al lenguaje de la expresión regular";
            }
        }

        #endregion

        #region TINY

        private void BT_ClasificaTokens5oAvance_Click(object sender, EventArgs e)
        {
            try
            {
                if (TB_Numero5oAvance.Text != "" && TB_Identificador5oAvance.Text != "" && TB_LenguajeTiny5oAvance.Text != "")
                {
                    // Se resetea el DataGridView
                    DGV_Tokens5oAvance.Rows.Clear();
                    // Conversion a posfija del identificador y numero
                    string pNum = ConversionPosfija(TB_Numero5oAvance.Text);
                    string pId = ConversionPosfija(TB_Identificador5oAvance.Text);

                    // Se crean los  AFN con su respectivo num e id
                    AFN AFNnum = new AFN(pNum);
                    AFN AFNid = new AFN(pId);

                    // Se guardan los estados
                    Operando AFNres = AFNnum.automata(pNum);
                    AFNnum.estados = AFNres.edosOp;
                    AFNres = AFNid.automata(pId);
                    AFNid.estados = AFNres.edosOp;

                    // Se crean la conversion de AFN a AFD
                    AFD AFDnum = new AFD(AFNnum);
                    AFDnum.inicializar();
                    AFD AFDid = new AFD(AFNid);
                    AFDid.inicializar();

                    // Se crea la lista de aceptacion
                    List<int> ListaAceptacion = AFNnum.RegresaFinales();
                    AFDnum.dEstados.ChecaFinal(ListaAceptacion);
                    ListaAceptacion = AFNid.RegresaFinales();
                    AFDid.dEstados.ChecaFinal(ListaAceptacion);

                    // Se crean las variables para lectura del lenguaje TINY
                    List<String[]> tinyCode = new List<String[]>();
                    List<string> StringClasificadas = new List<string>();
                    int ContadorFila = 0;
                    for (int lineIndex = 0; lineIndex < TB_LenguajeTiny5oAvance.Lines.Length; lineIndex++)
                    {// For para la lectura de linea por linea para el lenguaje TINY
                        string Trimeada = TB_LenguajeTiny5oAvance.Lines[lineIndex].Trim();
                        String[] lineArray = Trimeada.Split(' ');
                        tinyCode.Add(lineArray);
                        foreach (string s in lineArray)
                        {// Se lee cada renglon dentro del lenguaje TINY, se reconoce como palabra reservada, simbolo especial, numero, identificador y error lexico
                            bool Nuevo = !siContiene(s, StringClasificadas);
                            if (siContiene(s, PalabrasReservadas))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens5oAvance.Rows.Add(s, s); // Reconoce if, else, then, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (siContiene(s, SimbolosEspeciales))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens5oAvance.Rows.Add(s, s); // Reconoce (, ), +, *, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (AFDnum.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens5oAvance.Rows.Add("número", s); // Reconoce los numeros
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (AFDid.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens5oAvance.Rows.Add("ídentificador", s); // Reconoce los identificadores
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (s != "")
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens5oAvance.Rows.Add("Error Léxico", s); // Muestra un error lexico
                                    DGV_Tokens5oAvance.Rows[ContadorFila].Cells[0].Style.ForeColor = Color.Red;
                                    DGV_Tokens5oAvance.Rows[ContadorFila].Cells[1].Style.ForeColor = Color.Red;
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Falta uno o mas datos.");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Expecion: \n" + E.Message);
            }
        }

        // Funcion que permite la lectura dentro de una lista, si lo reconcoe regresa true
        public bool siContiene(string Cadena, List<string> Lista)
        {
            bool resultado = false;

            foreach (string c in Lista)
            {
                if (Cadena == c)
                {
                    return true;
                }
            }
            return resultado;
        }

        #endregion

        #region LR(0)

        private void BT_ContruirColeccionLR0Canonica_6_Click(object sender, EventArgs e)
        {
            Gramatica G = new Gramatica(); // Se crea la gramatica ya con los primeros y siguientes
            AFDL AFDG = G.AFD;

            // Se inicializa la tabla AFD
            DGV_AFDCanonica_6.Rows.Clear(); 
            DGV_AFDCanonica_6.Columns.Clear();
            // Se inicializa la tabla de Accion
            DGV_AccionLR0.Columns.Clear();
            // Se inicializa la tabla de ir_A()
            DGV_irALR0.Columns.Clear();
            DGV_AFDCanonica_6.Columns.Add("Estados", "Estados");
            // Se inicializa el textbox de la informacion del estado
            TB_InfoEstadoLR0.Text = "";
            // Se crean las transiciones de los no terminales y terminales
            List<string> todasLasTransiciones = AFDG.getAllTransiciones();

            foreach (string s in todasLasTransiciones) // Se agregan las columnas de transiciones 
            {
                DGV_AFDCanonica_6.Columns.Add(s, s);
            }

            foreach (EstadoAFDL eAFDG in AFDG.Estados) 
            {
                // Por cada estado se crean los estados, con indice y el estado actual
                string EstadoString = eAFDG.getEstadoString();
                TB_InfoEstadoLR0.Text += "Estado: " + eAFDG.IndiceEstado + "(" + eAFDG.ElementosEstado.Count + ")\n" + "{\n";
                TB_InfoEstadoLR0.Text += EstadoString + "} \n";
                List<string> listaDeElementosEnRow = new List<string>();

                listaDeElementosEnRow.Add("I" + eAFDG.IndiceEstado.ToString() + "(" + eAFDG.Transiciones.Count + ")");
                foreach (string s in todasLasTransiciones)
                {
                    TransicionD transicionDAux = eAFDG.getTransicion(s);
                    if (transicionDAux == null) // Si no existe la transicion
                    {
                        listaDeElementosEnRow.Add("ø"); // Elemento vacio
                    }
                    else // Si existe la transicion con ese simbolo
                    {
                        listaDeElementosEnRow.Add(transicionDAux.indiceDest.ToString()); // Estado destino
                    }
                }
                // Se agrega a la lista de elementos de AFD
                DGV_AFDCanonica_6.Rows.Add(listaDeElementosEnRow.ToArray());
            }

            // Se genera la tabla de Accion
            DGV_AccionLR0.Columns.Add("Estado", "Estado");
            for (int i = 0; i < AFDG.T.Count; i++)
            {
                // Se agregan los terminales
                DGV_AccionLR0.Columns.Add(AFDG.T[i], AFDG.T[i]);
            }
            DGV_AccionLR0.Columns.Add("$", "$");

            // Se genera la tabla de no terminales en ir_A()
            for (int i = 0; i < AFDG.NT.Count; i++)
            {
                DGV_irALR0.Columns.Add(AFDG.NT[i], AFDG.NT[i]);
            }

            List<string> Renglon = new List<string>();
            // Por cada estado se agrega dicha accion para terminales
            for (int i = 0; i < AFDG.Estados.Count; i++)
            {
                Renglon.Clear();
                Renglon.Add(i.ToString());
                for (int j = 0; j < AFDG.T.Count + 1; j++)
                {
                    if (AFDG.Accion[i, j] != null)
                    {
                        Renglon.Add(AFDG.Accion[i, j]);
                    }
                    else
                    {
                        Renglon.Add("ø");
                    }
                }
                DGV_AccionLR0.Rows.Add(Renglon.ToArray());
            }
            // Por cada estado se agrega dicha accion para no terminales
            for (int i = 0; i < AFDG.Estados.Count; i++)
            {
                Renglon.Clear();
                for (int j = 0; j < AFDG.NT.Count; j++)
                {
                    if (AFDG.Ir_A[i, j] != null)
                    {
                        Renglon.Add(AFDG.Ir_A[i, j]);
                    }
                    else
                    {
                        Renglon.Add("ø");
                    }
                }
                DGV_irALR0.Rows.Add(Renglon.ToArray());
            }
        }

        #endregion

        #region ARBOL
        // Variable para guardar los errores lexicos que se mostraran
        public List<ErrorLexico> ErroresLexicos; 

        private void BT_ClasificaTokens_6_Click(object sender, EventArgs e)
        { // FUNCION PARA CLASIFICAR LOS TOKENS DE LA GRAMATICA TINY
            try
            {
                if (TB_Numero_6.Text != "" && TB_Identificador_6.Text != "" && TB_ProgramaTiny_6.Text != "")
                {
                    // Se resetea el DataGridView
                    DGV_Tokens_6.Rows.Clear();
                    // Conversion a posfija del identificador y numero
                    string pNum = ConversionPosfija(TB_Numero_6.Text);
                    string pId = ConversionPosfija(TB_Identificador_6.Text);

                    // Se crean los  AFN con su respectivo num e id
                    AFN AFNnum = new AFN(pNum);
                    AFN AFNid = new AFN(pId);

                    // Se guardan los estados
                    Operando AFNres = AFNnum.automata(pNum);
                    AFNnum.estados = AFNres.edosOp;
                    AFNres = AFNid.automata(pId);
                    AFNid.estados = AFNres.edosOp;

                    // Se crean la conversion de AFN a AFD
                    AFD AFDnum = new AFD(AFNnum);
                    AFDnum.inicializar();
                    AFD AFDid = new AFD(AFNid);
                    AFDid.inicializar();

                    // Se crea la lista de aceptacion
                    List<int> ListaAceptacion = AFNnum.RegresaFinales();
                    AFDnum.dEstados.ChecaFinal(ListaAceptacion);
                    ListaAceptacion = AFNid.RegresaFinales();
                    AFDid.dEstados.ChecaFinal(ListaAceptacion);

                    // Se crean las variables para lectura del lenguaje TINY
                    List<String[]> tinyCode = new List<String[]>();
                    List<string> StringClasificadas = new List<string>();
                    int ContadorFila = 0;
                    for (int lineIndex = 0; lineIndex < TB_ProgramaTiny_6.Lines.Length; lineIndex++)
                    {// For para la lectura de linea por linea para el lenguaje TINY
                        string Trimeada = TB_ProgramaTiny_6.Lines[lineIndex].Trim();
                        String[] lineArray = Trimeada.Split(' ');
                        tinyCode.Add(lineArray);
                        foreach (string s in lineArray)
                        {// Se lee cada renglon dentro del lenguaje TINY, se reconoce como palabra reservada, simbolo especial, numero, identificador y error lexico
                            bool Nuevo = !siContiene(s, StringClasificadas);
                            if (siContiene(s, PalabrasReservadas))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add(s, s); // Reconoce if, else, then, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (siContiene(s, SimbolosEspeciales))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add(s, s); // Reconoce (, ), +, *, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (AFDnum.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("número", s); // Reconoce los numeros
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (AFDid.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("ídentificador", s); // Reconoce los identificadores
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                            else if (s != "")
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("Error Léxico", s); // Muestra un error lexico
                                    DGV_Tokens_6.Rows[ContadorFila].Cells[0].Style.ForeColor = Color.Red;
                                    DGV_Tokens_6.Rows[ContadorFila].Cells[1].Style.ForeColor = Color.Red;
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Falta uno o mas datos.");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Expecion: \n" + E.Message);
            }
        }

        private void BT_AnalisisLexicoSintactico_Click(object sender, EventArgs e)
        {
            TB_ErroresLexico.Text = "";
            // Se crea la lista de errores lexicos
            ErroresLexicos = new List<ErrorLexico>();
            // Se crea la cadena de clasificacion de tokens
            List<string> CadenaPrograma = ClasificaTokens(); // (ClasificaTokens = TINY solo que guarda ERRORES LEXICOS)
            TB_ErroresLexico.ForeColor = Color.Red;
            // Si se detecta un error lexico, lo muestra y muestra cuantos.
            if (ErroresLexicos.Count > 0 || CadenaPrograma == null)
            {
                TB_ErroresLexico.Font = new Font(TB_ErroresLexico.Font.FontFamily, 14);
                TB_ErroresLexico.AppendText("n: " + ErroresLexicos.Count + " Errores Léxicos!!");
                TB_ErroresLexico.AppendText(Environment.NewLine);
                foreach (ErrorLexico LEXICO in ErroresLexicos)
                {
                    string Linea = "";
                    if (LEXICO.Linea == -1)
                    {
                        // En caso de que no se llenen los campos
                        Linea = "Por favor llene los campos requeridos";
                        TB_ErroresLexico.AppendText(Linea);
                        TB_ErroresLexico.AppendText(Environment.NewLine);
                        break;
                    }
                    // En caso de que el lenguaje del programa TINY no lo reconozca
                    Linea = "Caracter: " + LEXICO.Linea + "     Lexema: " + LEXICO.Lexema;
                    TB_ErroresLexico.AppendText(Linea);
                    TB_ErroresLexico.AppendText(Environment.NewLine);
                }
            }
            else
            {
                // Caso de que no ocurra ningun error Lexico
                TB_ErroresLexico.Font = new Font(TB_ErroresLexico.Font.FontFamily, 16);
                TB_ErroresLexico.AppendText("Mostrando Arbol de Analisis Sintactico");
                TB_ErroresLexico.AppendText(Environment.NewLine);
                // Se crea la tabla de LR0
                AFDL AFD = CreaTablasLR0();
                // Se llama al algoritmo de evaluacion LR0
                AlgoritmoEvaluacionLR0(CadenaPrograma, AFD); // Recordemos que CadenaPrograma es la cadena clasificada
            }
        }
        
        private List<string> ClasificaTokens()
        {
            try
            {
                if (TB_Numero_6.Text != "" && TB_Identificador_6.Text != "" && TB_ProgramaTiny_6.Text != "")
                {
                    // Se resetea el DataGridView
                    DGV_Tokens_6.Rows.Clear();
                    // Conversion a posfija del identificador y numero
                    string PosfijaNumero = ConversionPosfija(TB_Numero_6.Text);
                    string PosfijaIdentificador = ConversionPosfija(TB_Identificador_6.Text);
                    // Se crean los  AFN con su respectivo num e id
                    AFN AfnNumero = new AFN(PosfijaNumero);
                    AFN AfnIdentificador = new AFN(PosfijaIdentificador);
                    // Se guardan los estados
                    Operando AFNResultante = AfnNumero.automata(PosfijaNumero);
                    AfnNumero.estados = AFNResultante.edosOp;
                    AFNResultante = AfnIdentificador.automata(PosfijaIdentificador);
                    AfnIdentificador.estados = AFNResultante.edosOp;
                    // Se crean la conversion de AFN a AFD
                    AFD AFDNumero = new AFD(AfnNumero);
                    AFDNumero.inicializar();
                    AFD AFDIdentificador = new AFD(AfnIdentificador);
                    AFDIdentificador.inicializar();
                    // Se crea la lista de aceptacion
                    List<int> ListaAceptacion = AfnNumero.RegresaFinales();
                    AFDNumero.dEstados.ChecaFinal(ListaAceptacion);
                    ListaAceptacion = AfnIdentificador.RegresaFinales();
                    AFDIdentificador.dEstados.ChecaFinal(ListaAceptacion);
                    // Se crean las variables para lectura del lenguaje TINY
                    List<string> tinyLines = new List<string>();
                    List<string> StringClasificadas = new List<string>();
                    int ContadorFila = 0;
                    for (int lineIndex = 0; lineIndex < TB_ProgramaTiny_6.Lines.Length; lineIndex++)
                    {// For para la lectura de linea por linea para el lenguaje TINY
                        string Trimeada = TB_ProgramaTiny_6.Lines[lineIndex].Trim();
                        String[] lineArray = Trimeada.Split(' ');
                        foreach (string s in lineArray)
                        {// Se lee cada renglon dentro del lenguaje TINY, se reconoce como palabra reservada, simbolo especial, numero, identificador y error lexico
                            bool Nuevo = !siContiene(s, StringClasificadas);
                            if (siContiene(s, PalabrasReservadas))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add(s, s); // Reconoce if, else, then, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                                tinyLines.Add(s);
                            }
                            else if (siContiene(s, SimbolosEspeciales))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add(s, s); // Reconoce (, ), +, *, etc...
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                                tinyLines.Add(s);

                            }
                            else if (AFDNumero.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("número", s); // Reconoce los numeros
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                                tinyLines.Add("numero");

                            }
                            else if (AFDIdentificador.ValidaLexema(s))
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("ídentificador", s); // Reconoce los identificadores
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                }
                                tinyLines.Add("identificador");
                            }
                            else if (s != "")
                            {
                                if (Nuevo)
                                {
                                    DGV_Tokens_6.Rows.Add("Error Léxico", s); // Muestra un error lexico
                                    DGV_Tokens_6.Rows[ContadorFila].Cells[0].Style.ForeColor = Color.Red;
                                    DGV_Tokens_6.Rows[ContadorFila].Cells[1].Style.ForeColor = Color.Red;
                                    StringClasificadas.Add(s);
                                    ContadorFila++;
                                    // Agrega un error lexico
                                    ErrorLexico NuevoError = new ErrorLexico(ContadorFila, s);
                                    ErroresLexicos.Add(NuevoError);
                                }
                            }
                        }
                    }
                    tinyLines.Add("$");
                    return tinyLines;
                }
                else
                {
                    ErrorLexico NuevoError = new ErrorLexico(-1, "Necesita llenar los campos requeridos");
                    ErroresLexicos.Add(NuevoError);
                    MessageBox.Show("Llenar los campos");
                }
            }
            catch (Exception E)
            {
                MessageBox.Show("Ocurrió una excepción\n" + E.Message);
            }
            return null;
        }

        private AFDL CreaTablasLR0()
        { // REALIZA EL ALGORITMO DE CREACION DE TABLA CANONICA LR(0)
            Gramatica G = new Gramatica(); // Se crea la gramatica ya con los primeros y siguientes
            AFDL AFDG = G.AFD;
            // Se inicializa la tabla AFD
            DGV_AFDCanonica_6.Rows.Clear();
            DGV_AFDCanonica_6.Columns.Clear();
            // Se inicializa la tabla de Accion
            DGV_AccionLR0.Columns.Clear();
            // Se inicializa la tabla de ir_A()
            DGV_irALR0.Columns.Clear();
            DGV_AFDCanonica_6.Columns.Add("Estados", "Estados");
            // Se inicializa el textbox de la informacion del estado
            TB_InfoEstadoLR0.Text = "";
            // Se crean las transiciones de los no terminales y terminales
            List<string> todasLasTransiciones = AFDG.getAllTransiciones();

            foreach (string s in todasLasTransiciones) // Se agregan las columnas de transiciones 
            {
                DGV_AFDCanonica_6.Columns.Add(s, s);
            }

            foreach (EstadoAFDL eAFDG in AFDG.Estados)
            {
                // Por cada estado se crean los estados, con indice y el estado actual
                string EstadoString = eAFDG.getEstadoString();
                TB_InfoEstadoLR0.Text += "Estado: " + eAFDG.IndiceEstado + "(" + eAFDG.ElementosEstado.Count + ")\n" + "{\n";
                TB_InfoEstadoLR0.Text += EstadoString + "} \n";
                List<string> listaDeElementosEnRow = new List<string>();

                listaDeElementosEnRow.Add("I" + eAFDG.IndiceEstado.ToString() + "(" + eAFDG.Transiciones.Count + ")");
                foreach (string s in todasLasTransiciones)
                {
                    TransicionD transicionDAux = eAFDG.getTransicion(s);
                    if (transicionDAux == null) // Si no existe la transicion
                    {
                        listaDeElementosEnRow.Add("ø"); // Elemento vacio
                    }
                    else // Si existe la transicion con ese simbolo
                    {
                        listaDeElementosEnRow.Add(transicionDAux.indiceDest.ToString()); // Estado destino
                    }
                }
                // Se agrega a la lista de elementos de AFD
                DGV_AFDCanonica_6.Rows.Add(listaDeElementosEnRow.ToArray());
            }
            // Se genera la tabla de Accion
            DGV_AccionLR0.Columns.Add("Estado", "Estado");
            for (int i = 0; i < AFDG.T.Count; i++)
            {
                // Se agregan los terminales
                DGV_AccionLR0.Columns.Add(AFDG.T[i], AFDG.T[i]);
            }
            DGV_AccionLR0.Columns.Add("$", "$");

            // Se genera la tabla de no terminales en ir_A()
            for (int i = 0; i < AFDG.NT.Count; i++)
            {
                DGV_irALR0.Columns.Add(AFDG.NT[i], AFDG.NT[i]);
            }

            List<string> Renglon = new List<string>();
            // Por cada estado se agrega dicha accion para terminales
            for (int i = 0; i < AFDG.Estados.Count; i++)
            {
                Renglon.Clear();
                Renglon.Add(i.ToString());
                for (int j = 0; j < AFDG.T.Count + 1; j++)
                {
                    if (AFDG.Accion[i, j] != null)
                    {
                        Renglon.Add(AFDG.Accion[i, j]);
                    }
                    else
                    {
                        Renglon.Add("ø");
                    }
                }
                DGV_AccionLR0.Rows.Add(Renglon.ToArray());
            }
            // Por cada estado se agrega dicha accion para no terminales
            for (int i = 0; i < AFDG.Estados.Count; i++)
            {
                Renglon.Clear();
                for (int j = 0; j < AFDG.NT.Count; j++)
                {
                    if (AFDG.Ir_A[i, j] != null)
                    {
                        Renglon.Add(AFDG.Ir_A[i, j]);
                    }
                    else
                    {
                        Renglon.Add("ø");
                    }
                }
                DGV_irALR0.Rows.Add(Renglon.ToArray());
            }
            return AFDG;
        }

        public bool AlgoritmoEvaluacionLR0(List<string> Programa, AFDL AFD)
        {
            // Se inicializa el ArbolSintactico
            TreeViewArbolSintáctico.Nodes.Clear();
            TreeViewArbolSintáctico.Refresh();
            // Se crea una lista de NodosHijo
            List<TreeNode> NodosHijo = new List<TreeNode>();
            // Iniciamos la pila con estados
            Stack<int> PilaEstados = new Stack<int>();
            // Se crea ACCION e ir_A()
            string[,] Accion = AFD.Accion;
            string[,] Ir_A = AFD.Ir_A;
            PilaEstados.Push(0);
            // IndexA determina el elemento a verificar
            int indexA = 0;
            while (true)
            {
                int s = PilaEstados.Peek(); // Se guarda el estado del tope en s
                string a = Programa[indexA]; // Recordemos que Programa = CadenaPrograma (los tokens ya clasificados)
                // MessageBox.Show(s + ", " + a);
                int indexNodoActual = NodosHijo.Count; // Se guarda el Nodo Actual
                int IndexElementoEvaluando = -1; // Caso base
                if (AFD.T.Contains(a))
                {
                    IndexElementoEvaluando = AFD.T.IndexOf(a); // Si es un Terminal, se agrega al elemento evaluado
                }
                if (AFD.NT.Contains(a))
                {
                    IndexElementoEvaluando = AFD.NT.IndexOf(a); // Si es un no Terminal, se agrega al elemento evaluado
                }
                if (a == "$")
                {
                    IndexElementoEvaluando = AFD.T.Count; // Si es $, se agrega al elemento evaluado
                }
                if (IndexElementoEvaluando != -1) // Si se evaluo
                {
                    if (Accion[s, IndexElementoEvaluando].Contains("d"))
                    { // CASO desplazar
                        // MessageBox.Show("CASO DESPLAZAR");
                        string EstadoDesplazar = Accion[s, IndexElementoEvaluando]; // Realiza ACCION de estado con el elemento evaluado
                        int indexD = EstadoDesplazar.IndexOf("d"); // Clasificacion
                        int NumeroElemento = int.Parse(EstadoDesplazar.Substring(indexD + 1).ToString());
                        PilaEstados.Push(NumeroElemento);
                        // Se añade el nodo HIJO al TREENODE
                        TreeNode NuevoNodo = new TreeNode();
                        NuevoNodo.Text = a;
                        NodosHijo.Add(NuevoNodo);
                        indexA++;
                    }
                    else if (Accion[s, IndexElementoEvaluando].Contains("r"))
                    { // CASO reducir
                        // MessageBox.Show("CASO REDUCIR");
                        string Cadenaarreglo = Accion[s, IndexElementoEvaluando]; // Realiza ACCION de estado con el elemento evaluado
                        int indexR = Cadenaarreglo.IndexOf("r"); // Clasificacion
                        int NumeroElemento = int.Parse(Cadenaarreglo.Substring(indexR + 1).ToString());
                        // GET la cantidad de simbolos gramaticales que tiene la produccion
                        int NumeroCaracteres = AFD.GetCaracteresProduccion(NumeroElemento);

                        for (int i = 0; i < NumeroCaracteres; i++)
                        {
                            PilaEstados.Pop(); // Quita los estados dependiendo del numero de caracteres
                        }
                        s = PilaEstados.Peek(); // Guarda el estado que se encuentra al principio de la pila
                        string Padre = AFD.ObtenPadreProduccion(NumeroElemento);
                        PilaEstados.Push(int.Parse(Ir_A[s, AFD.NT.IndexOf(Padre)])); // PUSH ir_A[t,A] en la PILA
                        // Guardamos los NODOS
                        List<TreeNode> NodosAGuardar = new List<TreeNode>();
                        List<int> IndexEliminaciones = new List<int>();
                        for (int i = indexNodoActual - NumeroCaracteres; i < indexNodoActual; i++)
                        {
                            NodosAGuardar.Add(NodosHijo[i]);
                            IndexEliminaciones.Add(i);
                        }
                        // Sorteamos INDEX para eliminar basura (registros que no nos sirven)
                        IndexEliminaciones.Sort();
                        IndexEliminaciones.Reverse();
                        // Eliminamos NODOS que ya copiamos
                        foreach (int i in IndexEliminaciones)
                        {
                            NodosHijo.RemoveAt(i);
                        }
                        // Creamos NODO PADRE que guardara a los hijos
                        TreeNode NodoPadre = new TreeNode(Padre);
                        // Al NODOPADRE le agregamos NODOSHIJO
                        foreach (TreeNode T in NodosAGuardar)
                        {
                            NodoPadre.Nodes.Add(T);
                        }
                        // Agregamos a la lista de NODOS el NODOPADRE
                        NodosHijo.Add(NodoPadre);
                    }
                    else if (Accion[s, IndexElementoEvaluando] == "ac")
                    { // CASO aceptacion
                        // MessageBox.Show("CASO ACEPTACION");
                        foreach (TreeNode T in NodosHijo)
                        {
                            // Agregamos todos los nodos
                            TreeViewArbolSintáctico.Nodes.Add(T);
                        }
                        break;
                    }
                    else if (Accion[s, IndexElementoEvaluando] == "")
                    {
                        MessageBox.Show("Ocurrió un error sintáctico");
                        break;
                    }
                }
            }
            return false;
        }

        #endregion
    }
}
