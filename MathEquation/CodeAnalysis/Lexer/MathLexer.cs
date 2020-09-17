using MathEquation.CodeAnalysis.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer
{
    public class MathLexer
    {
        private readonly string Content;
        private int ContentLen { get => Content.Length; }
        private LexerPosition LexerPosition;
        public MathLexer(string content)
        {
            Content = content;
        }
        private char Peek(int offset = 0)
        {
            int index = LexerPosition.CurrentPosition + offset;
            if (index >= ContentLen)
                return ' ';
            return Content[index];
        }
        
    }
}
