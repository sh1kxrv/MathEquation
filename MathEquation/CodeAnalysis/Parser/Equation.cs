using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Lexer.Tokens;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Equation
    {
        public double CalculateX(string Content)
        {
            var lexer = new MathLexer();
            var tokens = lexer.Tokenize(Content);

            return CalculateX(tokens);
        }

        public double CalculateX(TokenCollection tokens)
        {
            OpenBrackets(tokens);

            return 0.0;
        }


        private int index = 0,
            length = 0;
        private TokenCollection inbrackets;
        private void OpenBrackets(TokenCollection tokens)
        {
            while (GoToNextBrackets(tokens))
            {
                var value = CalculateX(inbrackets);
                Console.WriteLine("vobla yobla is open");
            }
        }

        private bool GoToNextBrackets(TokenCollection tokens)
        {
            while (index < tokens.Count && tokens[index].Kind != SyntaxKind.BR_O)
                index++;

            if (index >= tokens.Count)
                return false;

            length = 0;

            var brackets = 1;
            inbrackets = new TokenCollection();
            while (brackets != 0)
            {
                length++;

                switch (tokens[index + length].Kind)
                {
                    case SyntaxKind.BR_O:
                        brackets++;
                        break;
                    case SyntaxKind.BR_C:
                        brackets--;
                        break;
                }

                if (brackets != 0)
                    inbrackets.Add(tokens[index + length]);
            }
            length++;

            index = index + length;

            return true;
        }
    }
}
