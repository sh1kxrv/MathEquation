using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Lexer.Tokens;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Calculator
    {
        public static string CalculateUnknownString(string value, string ErrorPrefix = "CalculateError")
        {
            try
            {
                if (CalculatorVariables.IsHave(value))
                    value = CalculatorVariables.CalculateAndReplace(value);
                if (CalculatorVariables.IsHaveUnknownVariables(value))
                {
                    if (value.Contains('='))
                        value = new Equation().CalculateX(value).ToString();
                    else
                        value = "[ERROR] Summary: The expression has variables, but they have no meaning";
                }
                else
                {
                    if (value.Contains('='))
                    {
                        value = value.Replace("=", (IsLeftEqualsRight(value) ? "==" : "!="));
                    }
                    else
                    {
                        value = new Calculator().Calculate(value, ErrorPrefix).ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"[ERROR] Details:\r\n" + ex.ToString());
                return ex.Message.Trim(250) + "...";
            }

            return value;
        }

        public static bool IsLeftEqualsRight(string value)
        {
            if (CalculatorVariables.IsHave(value))
                value = CalculatorVariables.CalculateAndReplace(value);

            var value_left = value.Substring(0, value.IndexOf('='));
            var value_right = value.Substring(value.IndexOf('=') + 1);

            var value_left_d = new Calculator().Calculate(value_left, "CalculateComparisonError(Left)");
            var value_right_d = new Calculator().Calculate(value_right, "CalculateComparisonError(Right)");

            return value_left_d == value_right_d;
        }

        public double Calculate(string Content, string ErrorPrefix = "CalculateError")
        {
            var lexer = new MathLexer();
            var tokens = lexer.Tokenize(Content);

            return Calculate(tokens, ErrorPrefix);
        }

        public int Gen;
        public double Calculate(TokenCollection tokens, string ErrorPrefix, int gen = 0)
        {
            Gen = gen;
            try
            {
                if (tokens[0].Kind == SyntaxKind.SUB || tokens[0].Kind == SyntaxKind.ADD)
                    tokens.Insert(0, new SyntaxToken(SyntaxKind.NUMBER, 0.ToString(), new Impl.ElementPosition(0, 0), 0));

                var trycount = 0;
                start:
                while (tokens.Count > 1)
                {
                    Clear(tokens);
                    for (var priority = OperatorPriority.MaxPriority; priority >= 0; priority--)
                        for (var i = 0; i < tokens.Count; i++)
                        {
                            if (OperatorPriority.Get(tokens[i]) == priority)
                            {
                                i += ReplaceAction(tokens, i);
                                //For safe
                                goto start;
                            }
                            ReplaceMathFunc(tokens, i);
                        }
                    trycount++;
                    if (trycount > 200)
                        throw new Exception("Too many attempts");
                }

                return GetVal(tokens, 0);
            }
            catch (Exception ex)
            {
                throw new Exception($"Summary:\r\n{ErrorPrefix} at position {tokens[1].Position} when calculating '{tokens.ToSimplyString()}' as '{tokens}'\r\nDetails:\r\n" + ex.ToString());
                return -1;
            }
        }

        private void Clear(TokenCollection tokens)
        {
            for (var i = 0; i < tokens.Count; i++)
                if (tokens[i].Kind == SyntaxKind.EOE ||
                    tokens[i].Kind == SyntaxKind.Invisible)
                    tokens.RemoveAt(i);
        }

        public static readonly string[] KnownFunctions = { "sqrt", "cos", "sin", "tg", "acos", "asin", "atg" };
        private int ReplaceMathFunc(TokenCollection tokens, int index)
        {
            int rlength = 0, rindex = 0;
            var value = 0.0;
            bool iscalc = false;

            if (index >= tokens.Count)
                return 0;


            //temp
            rindex = index;
            rlength = 2;
            //end temp

            if (tokens[index].Text == ("sqrt"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Sqrt(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("cos"))
            {
                ReplaceAction(tokens, index + 1);
                
                value = Math.Cos(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("sin"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Sin(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("tg"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Tan(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("acos"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Acos(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("asin"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Asin(GetVal(tokens, index + 1));

                iscalc = true;
            } else if (tokens[index].Text == ("atg"))
            {
                ReplaceAction(tokens, index + 1);

                value = Math.Atan(GetVal(tokens, index + 1));

                iscalc = true;
            }

            if (iscalc)
            {
                OnMessage?.Invoke(tokens[index].Text + tokens[index+1].Text + " = " + value, Gen);

                for (var i = 0; i < rlength; i++)
                    tokens.RemoveAt(rindex);
                tokens.Insert(rindex, new SyntaxToken(SyntaxKind.NUMBER, value.ToString(), new Impl.ElementPosition(rindex, rindex + rlength), value));
            }

            return rlength;
        }

        private int ReplaceAction(TokenCollection tokens, int index)
        {
            int rlength = 0, rindex = 0;
            var value = 0.0;
            bool iscalc = false;

            switch (tokens[index].Kind)
            {
                case SyntaxKind.BR_O:
                    rlength = 0;
                    rindex = index;

                    var brackets = 1;
                    TokenCollection inbrackets = new TokenCollection();
                    while (brackets != 0)
                    {
                        rlength++;

                        switch (tokens[index + rlength].Kind)
                        {
                            case SyntaxKind.BR_O:
                                brackets++;
                                break;
                            case SyntaxKind.BR_C:
                                brackets--;
                                break;
                        }

                        if (brackets != 0)
                            inbrackets.Add(tokens[index + rlength]);
                    }
                    rlength++;

                    //value = Calculate(inbrackets, Gen + 1);
                    value = new Calculator().Calculate(inbrackets, $"CalculateInBracketsError at position {index}", Gen + 1);

                    iscalc = true;
                    break;
                case SyntaxKind.MUL:
                    rlength = 3;
                    rindex = index - 1;

                    value = GetVal(tokens, index - 1) * GetVal(tokens, index + 1);

                    iscalc = true;
                    break;
                case SyntaxKind.DIV:
                    rlength = 3;
                    rindex = index - 1;

                    value = GetVal(tokens, index - 1) / GetVal(tokens, index + 1);

                    iscalc = true;
                    break;
                case SyntaxKind.SUB:
                    rlength = 3;
                    rindex = index - 1;

                    value = GetVal(tokens, index - 1) - GetVal(tokens, index + 1);

                    iscalc = true;
                    break;
                case SyntaxKind.ADD:
                    rlength = 3;
                    rindex = index - 1;

                    value = GetVal(tokens, index - 1) + GetVal(tokens, index + 1);

                    iscalc = true;
                    break;
                case SyntaxKind.POW:
                    rlength = 3;
                    rindex = index - 1;

                    value = Math.Pow(GetVal(tokens, index - 1), GetVal(tokens, index + 1));

                    iscalc = true;
                    break;
                case SyntaxKind.FACT:
                    rlength = 2;
                    rindex = index - 1;

                    value = f((int)GetVal(tokens, index - 1));

                    iscalc = true;
                    break;
            }

            if (iscalc)
            {
                var str = "";
                for (var i = 0; i < rlength; i++)
                    str += string.IsNullOrEmpty(tokens[rindex + i].Text) ? tokens[rindex + i].Value ?? " " + tokens[rindex + i].Kind + " " : tokens[rindex + i].Text;
                OnMessage?.Invoke(str + " = " + value, Gen);

                for (var i = 0; i < rlength; i++)
                    tokens.RemoveAt(rindex);
                tokens.Insert(rindex, new SyntaxToken(SyntaxKind.NUMBER, value.ToString(), new Impl.ElementPosition(rindex, rindex + rlength), value));
            }

            return rindex - index;
        }

        private double GetVal(TokenCollection tokens, int index)
        {
            return Convert.ToDouble(tokens[index].Value);
        }

        public int f(int i) => Enumerable.Range(1, i < 1 ? 1 : i).Aggregate((f, x) => f * x);

        public delegate void OnErrorEventHandler(string msg);
        public static event OnErrorEventHandler OnError;
        public static void InvokeOnError(string msg)
        {
            OnError?.Invoke(msg);
        }

        public delegate void OnMessageEventHandler(string msg, int gen);
        public static event OnMessageEventHandler OnMessage;
    }
}
