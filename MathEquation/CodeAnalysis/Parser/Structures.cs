using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Lexer;

namespace MathEquation.CodeAnalysis.Parser
{
    public class InBracketsExpression
    {
        public int index;
        public int length;
        public TokenCollection inbrackets;
        public bool contin;
    }

    public struct LeftRight
    {
        public TokenCollection left;
        public TokenCollection right;
    }
}
