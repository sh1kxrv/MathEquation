using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Parser.Syntax.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Syntax.Evaluator
{ 
    public class EquationEvaluator
    {
        struct LeftRight
        {
            public TokenCollection Left;
            public TokenCollection Right;
        }
        const int MaxPriority = 3;

        private static int Get(SyntaxToken token)
        {
            switch (token.Kind)
            {
                case SyntaxKind.BR_O: return 3;
                case SyntaxKind.BR_C: return 3;
                case SyntaxKind.POW: return 2;
                case SyntaxKind.MUL: return 1;
                case SyntaxKind.DIV: return 1;
                case SyntaxKind.SUB: return 0;
                case SyntaxKind.ADD: return 0;
                default: return -1;
            }
        }
        private readonly TokenCollection EquationTokens;
        public HashSet<string> LexerErrors;
        public EquationEvaluator(string Equation)
        {
            var lexer = new MathLexer();
            EquationTokens = lexer.Tokenize(Equation);
            LexerErrors = lexer.Errors;
        }
        public double Calculate()
        {
            if(LexerErrors.Any())
                return -1337;
            var LR = ToLeftRight();
            OptimizeX(LR);
            LR.Right.Add(new SyntaxToken(SyntaxKind.EOE, null, 0, null));
            MathParser parser = new MathParser(LR.Right);
            return new MathEvaluator(parser.Parse().Root).Evaluate();
        }

        private void OptimizeX(LeftRight lr)
        {
            if (lr.Left[0].Kind == SyntaxKind.ADD)
                lr.Left.RemoveAt(0);

            while (lr.Left.Count > 1)
                for (var priority = MaxPriority; priority >= 0; priority--)
                    for (var i = 0; i < lr.Left.Count; i++)
                        if (Get(lr.Left[i]) == priority)
                            ReplaceAction(lr.Left, lr.Right, i);
        }
        private void ReplaceAction(TokenCollection tokens, TokenCollection right, int index)
        {
            int rlength = 0, rindex = 0;
            List<SyntaxToken> value = new List<SyntaxToken>();
            bool iscalc = false;

            switch (tokens[index].Kind)
            {
                case SyntaxKind.MUL:
                    rlength = 3;
                    rindex = index - 1;

                    if (tokens[index - 1].Text == "x" && tokens[index + 1].Text == "x")
                    {
                        value.Add(new SyntaxToken(SyntaxKind.LETTER, "x", 0, null));

                        //WTF???!!!
                        right.Insert(0, new SyntaxToken(SyntaxKind.BR_O, "(", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.BR_C, ")", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.POW, "^", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.NUMBER, "0.5", 0, 0.5));
                    }

                    iscalc = true;
                    break;
                case SyntaxKind.DIV:
                    rlength = 0;
                    rindex = index - 1;

                    //Not finalized

                    iscalc = true;
                    break;
                case SyntaxKind.SUB:
                    rlength = 3;
                    rindex = index - 1;

                    if (tokens[index - 1].Text == "x" && tokens[index + 1].Text == "x")
                    {

                    }

                    iscalc = true;
                    break;
                case SyntaxKind.ADD:
                    rlength = 3;
                    rindex = index - 1;

                    if (tokens[index - 1].Text == "x" && tokens[index + 1].Text == "x")
                    {
                        value.Add(new SyntaxToken(SyntaxKind.LETTER, "x", 0, null));

                        //WTF???!!!
                        right.Insert(0, new SyntaxToken(SyntaxKind.BR_O, "(", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.BR_C, ")", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.POW, "/", 0, null));
                        right.Add(new SyntaxToken(SyntaxKind.NUMBER, "2", 0, 0.5));
                    }

                    iscalc = true;
                    break;
            }

            if (iscalc)
            {
                for (var i = 0; i < rlength; i++)
                    tokens.RemoveAt(rindex);
                tokens.InsertRange(rindex, value);
            }

            //return tokens;
        }
        private LeftRight ToLeftRight()
        {
            var copy = EquationTokens.Copy();
            copy.RemoveAt(copy.Count - 1);
            var LR = new LeftRight();
            if (!IsOperator(copy[0].Kind))
                copy.Insert(0, new SyntaxToken(SyntaxKind.ADD, "+", 0, null));

            int EQ_INDEX = GetEqually(copy);

            int tokensCount = copy.Count;
            int moved = 0;
            for (int index = EQ_INDEX; index < tokensCount; index++)
            {
                if (!IsOperator(copy[index].Kind))
                {
                    if (moved == 0)
                    {
                        copy.Insert(index - 1, copy[index + moved]);
                        copy.Insert(index - 1, new SyntaxToken(InvertOperator(GetOperator(copy, index - 1 + moved).Kind), null, 0, null));
                        moved += 2;
                    }
                    else
                    {
                        copy.Insert(index - 1, copy[index + moved++]);
                        copy.Insert(index - 1, new SyntaxToken(InvertOperator(GetOperator(copy, index - 1 + moved++).Kind), null, 0, null));
                    }
                }
            }

            EQ_INDEX = GetEqually(copy);

            var count = copy.Count;
            for (var i = EQ_INDEX; i < count; i++)
            {
                copy.RemoveAt(EQ_INDEX);
            }

            LR.Left = new TokenCollection();
            LR.Right = new TokenCollection();
            for (var i = 0; i < copy.Count; i++)
            {
                if (!IsOperator(copy[i].Kind))
                {
                    if (copy[i].Kind == SyntaxKind.LETTER)
                    {
                        LR.Left.Add(new SyntaxToken(copy[i - 1].Kind, null, 0, null));
                        LR.Left.Add(copy[i]);
                    }
                    else
                    {
                        LR.Right.Add(new SyntaxToken(InvertOperator(copy[i - 1].Kind), null, 0, null));
                        LR.Right.Add(copy[i]);
                    }
                }
            }

            return LR;
        }
        private SyntaxToken GetOperator(TokenCollection tokens, int index)
        {
            if (IsOperator(tokens[index].Kind))
                return tokens[index];
            return new SyntaxToken(SyntaxKind.ADD, "+", 0, null);
        }
        private int GetEqually(TokenCollection collection)
        {
            int index = 0;
            while (index < collection.Count)
            {
                if (collection[index].Kind == SyntaxKind.EQUALLY)
                    break;
                index++;
            }
            return index;
        }
        private SyntaxKind InvertOperator(SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.ADD: return SyntaxKind.SUB;
                case SyntaxKind.DIV: return SyntaxKind.MUL;
                case SyntaxKind.MUL: return SyntaxKind.DIV;
                case SyntaxKind.SUB: return SyntaxKind.ADD;
                default: return SyntaxKind.Invisible;
            }
        }
        private bool IsOperator(SyntaxKind kind)
        {
            if (kind == SyntaxKind.ADD ||
                kind == SyntaxKind.DIV ||
                kind == SyntaxKind.MUL ||
                kind == SyntaxKind.SUB ||
                kind == SyntaxKind.EQUALLY)
                return true;
            return false;
        }
    }
}
