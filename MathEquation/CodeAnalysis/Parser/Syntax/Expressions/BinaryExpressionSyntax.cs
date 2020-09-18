using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer.Syntax.Expressions
{
    public class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxNode token, ExpressionSyntax right)
        {
            Right = right;
            OperatorToken = token;
            Left = left;
        }
        public SyntaxNode OperatorToken { get; }
        public ExpressionSyntax Right { get; }
        public ExpressionSyntax Left { get; }
        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }
}
