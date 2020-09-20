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
    public sealed class MathParser
    {
        private readonly MathLexer Lexer;
        private TokenCollection _tokens;
        public HashSet<string> Errors; 
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
            Errors = new HashSet<string>();
            Lexer = new MathLexer();
            _tokens = Lexer.Tokenize(content);
            Errors = Lexer.Errors;
        }
        public MathParser(TokenCollection tokens)
        {
            Errors = new HashSet<string>();
            _tokens = tokens;
        }
        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind)
                return NextToken();
            Errors.Add($"Unexpected token <'{Current.Kind}'> expected <{kind}>");
            return new SyntaxToken(kind, null, Current.Position, null);
        }
        private SyntaxToken NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        public SyntaxTree Parse()
        {
            var expression = ParsePost();
            var eoe = Match(SyntaxKind.EOE);
            return new SyntaxTree(Errors, expression, eoe);
        }
        private ExpressionSyntax ParsePost()
        {
            var left = ParseMain();
            while (Current.Kind == SyntaxKind.ADD || Current.Kind == SyntaxKind.SUB || Current.Kind == SyntaxKind.POW)
            {
                var operatorToken = NextToken();
                var right = ParseMain();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParseMain()
        {
            var left = ParsePrimaryExpression();
            while (Current.Kind == SyntaxKind.MUL || Current.Kind == SyntaxKind.DIV)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }
            return left;
        }
        private ExpressionSyntax ParseExpression()
        {
            return ParsePost();
        }
        private ExpressionSyntax ParsePrimaryExpression()
        {
            if(Current.Kind == SyntaxKind.BR_O)
            {
                var open = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxKind.BR_C);
                return new ParenthizedExpressionSyntax(open, expression, right);
            }
            var numberToken = Match(SyntaxKind.NUMBER);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}
