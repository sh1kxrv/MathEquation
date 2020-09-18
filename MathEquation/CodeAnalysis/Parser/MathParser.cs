using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Parser.Syntax;
using MathEquation.CodeAnalysis.Parser.Syntax.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser
{
    public class MathParser
    {
        private readonly MathLexer Lexer;
        private TokenCollection _tokens;

        private int _position;
        private SyntaxToken Peek(int offset)
        {
            int index = _position + offset;
            if (index >= _tokens.Count)
                return _tokens[_tokens.Count - 1];
            return _tokens[index];
        }
        private SyntaxToken Current => Peek(0);
        public MathParser(string content)
        {
            Lexer = new MathLexer();
            _tokens = Lexer.Tokenize(content);
        }
        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();
            return new SyntaxToken(kind, null, Current.Position, null);
        }
        private SyntaxToken NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        public ExpressionSyntax Parse()
        {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxKind.ADD || Current.Kind == SyntaxKind.SUB)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParsePrimaryExpression()
        {
            var numberToken = Match(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}
