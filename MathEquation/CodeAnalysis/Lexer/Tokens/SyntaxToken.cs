using MathEquation.CodeAnalysis.Impl;

namespace MathEquation.CodeAnalysis.Lexer.Tokens
{
    //Add syntax tree and impl. syntax node!
    public class SyntaxToken
    {
        public SyntaxKind Kind { get; }
        public ElementPosition Position { get; }
        public object Value { get; }
        public string Text { get; }
        public SyntaxToken(SyntaxKind kind, string? text, ElementPosition position, object? value)
        {
            Kind = kind;
            Position = position;
            Text = text ?? string.Empty;
            Value = value ?? new object();
        }
    }
}
