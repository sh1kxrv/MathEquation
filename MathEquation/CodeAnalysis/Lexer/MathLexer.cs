using MathEquation.CodeAnalysis.Impl;
using MathEquation.CodeAnalysis.Lexer.Tokens;
using MathEquation.CodeAnalysis.Parser.Syntax;
using System;
using System.Collections.Generic;

namespace MathEquation.CodeAnalysis.Lexer
{
    public class MathLexer
    {
        private string Content;
        public HashSet<string> Errors;
        private int ContentLen => Content.Length;
        private LexerPosition LexerPosition;
        private SyntaxKind Kind;
        private object Value;
        public MathLexer()
        {
            LexerPosition = new LexerPosition(0, 0);
        }
        private char Current { get => Peek(0); }
        private char Lookahead { get => Peek(1); }
        private char Peek(int offset = 0)
        {
            int index = LexerPosition.CurrentPosition + offset;
            if (index >= ContentLen)
                return '\0';
            return Content[index];
        }
        public TokenCollection Tokenize(string Content)
        {
            Errors = new HashSet<string>();
            this.Content = Content;
            TokenCollection collection = new TokenCollection();
            SyntaxToken token;
            for (int i = 0; i < ContentLen; i++)
            {
                do
                {
                    token = Get();
                    if (token.Kind == SyntaxKind.InvalidToken)
                        Errors.Add($"Unknown character: <'{Current}'> on pos <{LexerPosition.CurrentPosition}>");
                    if (token.Kind != SyntaxKind.Invisible && token.Kind != SyntaxKind.InvalidToken && token.Kind != SyntaxKind.EOE)
                        collection.Add(token);
                }
                while (token.Kind != SyntaxKind.EOE);
            }
            Kind = SyntaxKind.InvalidToken;
            LexerPosition = new LexerPosition(0, 0);
            Value = null;
            collection.Add(new SyntaxToken(SyntaxKind.EOE, null, ContentLen, null));
            return collection;
        }
        private SyntaxToken Get()
        {
            LexerPosition.StartPosition = LexerPosition.CurrentPosition;
            Kind = SyntaxKind.InvalidToken;
            Value = null;
            switch (Current)
            {
                case '\0':
                    Kind = SyntaxKind.EOE;
                    //LexerPosition.CurrentPosition++;
                    break;
                case '\n':
                case ' ':
                case '\t':
                case '\r':
                    ReadWhiteSpace();
                    break;
                case '+':
                case '-':
                case '/':
                case '*':
                case '=':
                case '^':
                    ReadOperators();
                    break;
                case '1':case '2':
                case '3':case '4':case '5':case '6':
                case '7':case '8':case '9':case '0':
                    ReadNumber();
                    break;
                case '(':
                    Kind = SyntaxKind.BR_O;
                    LexerPosition.CurrentPosition++;
                    break;
                case ')':
                    Kind = SyntaxKind.BR_C;
                    LexerPosition.CurrentPosition++;
                    break;
                default:
                    if (char.IsWhiteSpace(Current))
                        ReadWhiteSpace();
                    else if (char.IsLetter(Current))
                        ReadLetter();
                    else
                    {
                        Errors.Add($"Unknown character: <'{Current}'> on pos <{LexerPosition.CurrentPosition}>");
                        LexerPosition.CurrentPosition++;
                    }
                    break;
            }
            string Text = Current.ToString();
            if (LexerPosition.CurrentPosition - LexerPosition.StartPosition > 0)
                Text = Content.Substring(LexerPosition.StartPosition, LexerPosition.CurrentPosition - LexerPosition.StartPosition);
            return new SyntaxToken(Kind, Text, LexerPosition.CurrentPosition - LexerPosition.StartPosition, Value);
        }

        private void ReadLetter()
        {
            while (char.IsLetter(Current))
                LexerPosition.CurrentPosition++;

            int len = LexerPosition.CurrentPosition - LexerPosition.StartPosition;
            Value = Content.Substring(LexerPosition.StartPosition, len);

            Kind = SyntaxKind.LETTER;
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                LexerPosition.CurrentPosition++;
            Kind = SyntaxKind.Invisible;
        }
        private void ReadNumber()
        {
            bool isDouble = false;
            while (char.IsDigit(Current))
            {
                if (Lookahead is '.' || Lookahead is ',')
                {
                    LexerPosition.CurrentPosition++;
                    isDouble = true;
                }
                LexerPosition.CurrentPosition++;
            }

            int len = LexerPosition.CurrentPosition - LexerPosition.StartPosition;
            string str = Content.Substring(LexerPosition.StartPosition, len);
            if (!isDouble)
            {
                if (!int.TryParse(str, out int value))
                    Errors.Add($"Error with try parse int value. {str}");
                Value = value;
            }
            else {
                //))))))))))
                if (!double.TryParse(str.Replace('.', ','), out double value))
                    Errors.Add($"Error with try parse double value. {str}");
                Value = value;
            }
            Kind = SyntaxKind.NUMBER;
        }
        private void ReadOperators()
        {
            if (Current is '+')
                Kind = SyntaxKind.ADD;
            else if (Current is '-')
                Kind = SyntaxKind.SUB;
            else if (Current is '/')
                Kind = SyntaxKind.DIV;
            else if (Current is '*')
                Kind = SyntaxKind.MUL;
            else if (Current is '=')
                Kind = SyntaxKind.EQUALLY;
            else if (Current is '^')
                Kind = SyntaxKind.POW;
            else
                Errors.Add($"Unknown operator: {Current}");
            LexerPosition.CurrentPosition++;
        }
    }
}
