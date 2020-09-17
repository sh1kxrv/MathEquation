using MathEquation.CodeAnalysis.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer.Tokens
{
    //Add syntax tree and impl. syntax node!
    public class SyntaxToken : SyntaxNode
    {
        public SyntaxKind Kind { get; }
        public int Position { get; }
        public object Value { get; }
        public string Text { get; }
        public SyntaxToken(SyntaxKind kind, string? text, int position, object? value)
        {
            //not finished.
        }
    }
}
