using MathEquation.CodeAnalysis.Parser.Syntax.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Syntax
{
    public sealed class SyntaxTree
    {
        public SyntaxTree(IEnumerable<string> errors, ExpressionSyntax root, SyntaxToken endOfExpression)
        {
            Errors = errors.ToArray();
            Root = root;
            EOE = endOfExpression;
        }
        public IReadOnlyList<string> Errors;
        public ExpressionSyntax Root { get; }
        public SyntaxToken EOE { get; }
    }
}
