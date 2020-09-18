using MathEquation.CodeAnalysis.Impl;
using System.Collections.Generic;
using System.Linq;

namespace MathEquation.CodeAnalysis.Parser.Syntax
{
#nullable enable
    //Add syntax tree and impl. syntax node!
    public class SyntaxToken : SyntaxNode
    {
        public override SyntaxKind Kind { get; }
        public int Position { get; }
        public object Value { get; }
        public string Text { get; }
        public SyntaxToken(SyntaxKind kind, string? text, int pos, object? value)
        {
            Kind = kind;
            Position = pos;
            Text = text ?? string.Empty;
#pragma warning disable CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
            Value = value;
#pragma warning restore CS8601 // Возможно, назначение-ссылка, допускающее значение NULL.
        }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
