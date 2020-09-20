using MathEquation.CodeAnalysis.Parser.Syntax.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Syntax.Evaluator
{
    public class MathEvaluator
    {
        private readonly ExpressionSyntax Root;
        public MathEvaluator(ExpressionSyntax root)
        {
            Root = root;
        }
        public double Evaluate()
        {
            return EvaluateExpression(Root);
        }

        private double EvaluateExpression(ExpressionSyntax root)
        {
            if (root is NumberExpressionSyntax n)
            {
                string raw = (n.NumberToken.Value ?? "0,0").ToString();
                return double.Parse(raw);
            }
            if(root is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                if (b.OperatorToken.Kind == SyntaxKind.ADD)
                    return left + right;
                else if (b.OperatorToken.Kind == SyntaxKind.SUB)
                    return left - right;
                else if (b.OperatorToken.Kind == SyntaxKind.DIV)
                    return left / right;
                else if (b.OperatorToken.Kind == SyntaxKind.MUL)
                    return left * right;
                else if (b.OperatorToken.Kind == SyntaxKind.POW)
                    return Math.Pow(left, right);
                else
                    throw new Exception($"Unknown operator kind: {b.OperatorToken.Kind}");
            }
            if(root is ParenthizedExpressionSyntax p)
            {
                return EvaluateExpression(p.Expression);
            }
            throw new Exception($"Node not supported {root.Kind}");
        }
    }
}
