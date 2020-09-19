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
        public double Calculate(string Content)
        {
            var lexer = new MathLexer();
            var tokens = lexer.Tokenize(Content);

            return Calculate(tokens);
        }

        public double Calculate(TokenCollection tokens)
        {
            if (tokens[0].Kind == SyntaxKind.SUB || tokens[0].Kind == SyntaxKind.ADD)
                tokens.Insert(0, new SyntaxToken(SyntaxKind.NUMBER, 0.ToString(), new Impl.ElementPosition(0, 0), 0));

            while (tokens.Count > 1)
                for (var priority = OperatorPriority.MaxPriority; priority >= 0; priority--)
                    for (var i = 0; i < tokens.Count; i++)
                    {
                        if (OperatorPriority.Get(tokens[i]) == priority)
                            ReplaceAction(tokens, i);
                    }

            return GetVal(tokens, 0);
        }

        private TokenCollection ReplaceAction(TokenCollection tokens, int index)
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

                    value = Calculate(inbrackets);

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
            }

            if (iscalc)
            {
                for (var i = 0; i < rlength; i++)
                    tokens.RemoveAt(rindex);
                tokens.Insert(rindex, new SyntaxToken(SyntaxKind.NUMBER, value.ToString(), new Impl.ElementPosition(0, 0), value));
            }

            return tokens;
        }

        private double GetVal(TokenCollection tokens, int index)
        {
            return Convert.ToDouble(tokens[index].Value);
        }
    }
}
