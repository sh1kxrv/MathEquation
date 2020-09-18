using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer.Syntax.Expressions
{
    public class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax(SyntaxNode NumberToken)
        {
            this.NumberToken = NumberToken;
        }
        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
        public SyntaxNode NumberToken;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return NumberToken;
        }
    }
}
