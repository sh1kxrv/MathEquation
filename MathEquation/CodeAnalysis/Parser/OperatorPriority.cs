using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Lexer.Tokens;

namespace MathEquation.CodeAnalysis.Parser
{
    class OperatorPriority
    {
        public const int MaxPriority = 3;

        public static int Get(SyntaxToken token)
        {
            switch (token.Kind)
            {
                case SyntaxKind.BR_O: return 3;
                case SyntaxKind.BR_C: return 3;
                case SyntaxKind.POW: return 2;
                case SyntaxKind.FACT: return 2;
                case SyntaxKind.MUL: return 1;
                case SyntaxKind.DIV: return 1;
                case SyntaxKind.SUB: return 0;
                case SyntaxKind.ADD: return 0;
                default: return -1;
            }
        }
    }
}
