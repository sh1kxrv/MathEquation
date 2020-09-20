using System;
using System.Collections.Generic;
using MathEquation.CodeAnalysis.Parser.Syntax;

namespace MathEquation.CodeAnalysis.Lexer
{
    public class TokenCollection : List<SyntaxToken>
    {
        public TokenCollection(List<SyntaxToken> list) : base(list)
        {

        }
        public TokenCollection()
        {

        }
        public TokenCollection Copy()
        {
            return new TokenCollection(this);
        }
    }
}
