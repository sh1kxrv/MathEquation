using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Syntax.Expressions
{
    public class ParenthizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthizedExpressionSyntax(SyntaxToken open, ExpressionSyntax expression, SyntaxToken close)
        {
            Open = open;
            Expression = expression;
            Close = close;
        }

        public SyntaxToken Open { get; }
        public SyntaxToken Close { get; }
        public ExpressionSyntax Expression { get; }
        public override SyntaxKind Kind => SyntaxKind.ParethizedExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Open;
            yield return Expression;
            yield return Close;
        }
    }
}
