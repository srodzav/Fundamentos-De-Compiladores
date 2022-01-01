using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections;

namespace Rodriguez_Zavala_Aleman_Hernandez
{
    class Posfija
    {
        /**
         * @param exp cadena que representa una expresion regular
         * La funcion se encarga de agregar elemento a la expresion, para que posteriormente
         * se pueda convertir en una expresion posfija
         * @return devuelve una cadena vacia si el parametro de expresion es incorrecto
         */

        public string cPosfija(string str)
        {
            str = PosConversion(str);
            Stack<char> stk = new Stack<char>();
            int i = 0; int k = 0;
            char[] r = new char[str.Length];
            char[] arr = str.ToArray<char>();

            string s;
            while (i < arr.Length)
            {
                s = arr[i].ToString();
                switch (s)
                {
                    case "(":
                        stk.Push(arr[i]);
                        break;
                    case ")":
                        while (stk.Peek() != '(')
                        {
                            r[k++] = stk.Pop();
                        }
                        stk.Pop();
                        break;
                    case var c when new Regex(@"^[a-z]+$").IsMatch(c):
                    case var c1 when new Regex(@"^[A-Z]+$").IsMatch(c1):
                    case var c2 when new Regex(@"^[0-9]+$").IsMatch(c2):
                        r[k++] = arr[i];
                        break;
                    case "*":
                    case "+":
                    case "?":
                    case "|":
                    case "&":
                        bool b = true;
                        while (b)
                        {
                            if (stk.Count == 0 || stk.Peek().Equals('(') || p(arr[i], stk.Peek()))
                            {
                                stk.Push(arr[i]);
                                b = false;
                            }
                            else
                            {
                                r[k++] = stk.Pop();
                            }
                        }
                        break;
                }
                i++;
            }
            while (stk.Count > 0)
            {
                if (stk.Peek() != '(')
                    r[k++] = stk.Pop();
                else
                    stk.Pop();
            }
            return new String(r);
        }

        private bool p(char a, char b)
        {
            return pp(a) > pp(b);
        }

        private int pp(char c)
        {
            switch (c)
            {
                case '*':
                case '+':
                case '?':
                    return 2;
                case '|':
                    return 0;
                case '&':
                    return 1;
            }
            return -1;
        }

        public string PosConversion(string exp)
        {
            if (!CheckExpression(exp))
                return "";
            Stack<char> stack = new Stack<char>();
            exp = exp.Replace(" ", String.Empty);
            char[] charArray = exp.ToCharArray();
            int i = 0;

            if (charArray[i] != '[')
            {
                stack.Push(charArray[i]);
                i++;
            }
            while (i < charArray.Length)
            {
                if (charArray[i] == '[')
                {
                    if (stack.Count() > 0 && stack.Peek() != '|' && stack.Peek() != '(')
                        stack.Push('&');
                    i = ChangeCharacterInterval(stack, charArray, i);
                }
                else if (IsConcatenation(charArray[i]) && stack.Peek() != '(' && stack.Peek() != '|')
                {
                    stack.Push('&');
                    stack.Push(charArray[i]);
                }
                else
                {
                    stack.Push(charArray[i]);
                    if (charArray[i].Equals('|') && (!charArray[i + 1].Equals('[') && !charArray[i + 1].Equals('(')))
                    {
                        stack.Push(charArray[++i]);
                    }
                }
                i++;
            }
            return ChangeExpression(stack);
        }

        /**
         * @param stk una pila de caracteres 
         * los careacteres dela pila se convierten en una cadena
         */
        private string ChangeExpression(Stack<char> stk)
        {
            char[] obj = stk.ToArray();
            IQueryable<char> reversed = obj.AsQueryable().Reverse();
            return new String(reversed.ToArray());
        }

        private bool IsConcatenation(char chacracter)
        {
            string character = chacracter.ToString();
            switch (character)
            {
                case var c when new Regex(@"^[a-z]+$").IsMatch(c):
                case var c1 when new Regex(@"^[A-Z]+$").IsMatch(c1):
                case var c2 when new Regex(@"^[0-9]+$").IsMatch(c2):
                case "(":
                case "[":
                    return true;
                    // break;
            }
            return false;
        }

        #region CheckExpression
        /**
         * @param exp expresion regular
         * Revisa si la expresion esta escrita correctamente
         * 
         */
        private bool CheckExpression(string exp)
        {

            exp = exp.Replace(" ", String.Empty);
            string c = exp.Substring(0, 1);
            string c1 = exp.Substring(exp.Length - 1, 1);
            if (
                c != "*" && c != "+" && c != "?" && c != "|"
                && c1 != "|"
                && CheckCorrespondence(exp, '(', ')')
                && Empty(exp, '(', ')')
                && CheckCorrespondence(exp, '[', ']')
                && Empty(exp, '[', ']')
                )
            {
                return true;
            }


            return false;
        }

        /**
         * @param exp expresion regular 
         * @param left caracrer puede representar un (, [ o {
         * @param right caracter puede representar ), ] o }
         * rvisa si existe una correspondencia () [] {}
         * 
         */
        public bool CheckCorrespondence(string exp, char left, char right)
        {
            int g = 0;
            foreach (char c in exp)
            {
                g += c.Equals(left) ? 1 : c.Equals(right) ? -1 : 0;
                if (g < 0)
                    break;
            }
            return g == 0;
        }

        /**
         * Revisa si no existen parentesis vacios
         */
        private bool Empty(string exp, char left, char right)
        {
            char var = ' ';
            foreach (char s in exp)
            {
                if (var.Equals(left) && s.Equals(right))
                    return false;
                var = s;

            }
            return true;
        }

        #endregion CheckExpression

        #region ChangeCharacterInterval
        /**
         * Hace la conversion de un intervalo de caracteres
         */
        private int ChangeCharacterInterval(Stack<char> stk, char[] charArray, int i)
        {
            int j = i;
            stk.Push('(');
            j++;

            Stack<char> stkAux = new Stack<char>();
            while (charArray[j] != ']')
            {
                if (charArray[j] != '-')
                {
                    stkAux.Push(charArray[j]);
                }
                else
                {
                    j++;
                    ChangeInterval(stkAux.Pop(), charArray[j], stkAux);
                }
                j++;
            }
            IQueryable<char> reversed = stkAux.AsQueryable().Reverse();
            char[] arr = reversed.ToArray<char>();
            int it;
            for (it = 0; it < arr.Length - 1; it++)
            {
                stk.Push(arr[it]);
                stk.Push('|');
            }
            stk.Push(arr[it]);
            stk.Push(')');

            return j;
        }

        private void ChangeInterval(char a, char b, Stack<char> stk)
        {
            for (char c = a; c <= b; c++)
            {
                stk.Push(c);
            }
        }

        #endregion ChangeCharacterInterval

    }
}
