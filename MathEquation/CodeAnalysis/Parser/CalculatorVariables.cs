using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser
{
    public class CalculatorVariables
    {
        public static bool IsHave(string str) => str.IndexOf(';') != -1;

        public static string CalculateAndReplace(string str)
        {
            var variables = str.Substring(str.IndexOf(';')+1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var expr = str.Substring(0, str.IndexOf(';'));

            var variableindex = 0;
            foreach (var variable in variables)
            {
                var variable_name = variable.Split('=')[0].Trim();
                var variable_expr = variable.Split('=')[1].Trim();
                var calc = new Calculator();

                if (string.IsNullOrEmpty(variable_name) || string.IsNullOrEmpty(variable_expr))
                    throw new Exception("Variable error in '" + variable + "' at position " + variableindex);

                var replace = "";
                if (variable_expr.StartsWith("[") && variable_expr.EndsWith("]"))
                    if (expr.Contains('='))
                    {
                        var split = variable_expr.TrimStart('[').TrimEnd(']').Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).OfType<string>().ToList();

                        if (split.Count == 2)
                            split.Add("1");
                        if (split.Count < 2)
                            split = new List<string>(new string[] { "-1000", "1000", "0,1" });

                        var range = new Range() { From = (float)calc.Calculate(split[0].Replace(".", ",")), 
                                                To = (float)calc.Calculate(split[1].Replace(".", ",")), 
                                                Add = (float)calc.Calculate(split[2].Replace(".", ",")) };

                        replace = Range.CalculateRange(expr + ";" + string.Join("", variables.Select((e, i) => { if (i > variableindex) return e; return ""; }).ToArray()), variable_name, range).ToString();
                    }
                    else
                        throw new Exception("Not is equation");
                else
                    replace = calc.Calculate(variable_expr, $"VariableValueError in '{variable}'").ToString();

                expr = expr.Replace(variable_name, "(" + replace + ")");
                variableindex++;
            }

            return expr;
        }

        public static bool IsHaveUnknownVariables(string str)
        {
            foreach (var ch in new Lexer.MathLexer().Tokenize(str))
            {
                if (ch.Kind == Lexer.Tokens.SyntaxKind.LETTER && ch.IsKnownFunc == false)
                    return true;
            }
            return false;
        }
    }

    public class Range
    {
        public float From;
        public float To;
        public float Add;
        public int trim { get => Add.ToString().Substring(Add.ToString().IndexOf('.') + 1).Length; }

        public static double CalculateRange(string expr, string variable, Range range)
        {
            for (float i = range.From; i <= range.To; i += range.Add)
            {
                i = (float)Math.Round(i, range.trim);
                //if (CalculatorVariables.IsHave(expr))
                //    expr = CalculatorVariables.CalculateAndReplace(expr);
                var exprp = expr.Replace(variable, "(" + i.ToString() + ")");
                var b = Calculator.IsLeftEqualsRight(exprp);
                if (b)
                    return i;
                Calculator.InvokeOnLongTimeOperationReceiveMessage(expr + " is " + b);
            }
            throw new Exception("Value not in range");
        }
    }
}
