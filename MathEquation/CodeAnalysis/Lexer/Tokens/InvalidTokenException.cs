using System;

namespace MathEquation.CodeAnalysis.Lexer.Tokens
{
    public class InvalidTokenException : Exception
    {
        public InvalidTokenException(int pos) : base($"Unknown element! (Position: {pos})")
        {

        }
    }
}
