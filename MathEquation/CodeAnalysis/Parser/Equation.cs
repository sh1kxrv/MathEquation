using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathEquation.CodeAnalysis.Lexer;
using MathEquation.CodeAnalysis.Lexer.Tokens;

namespace MathEquation.CodeAnalysis.Parser
{
    public class Equation
    {
        public double CalculateX(string Content)
        {
            var lexer = new MathLexer();
            var tokens = lexer.Tokenize(Content);

            return CalculateX(tokens);
        }

        public double CalculateX(TokenCollection tokens)
        {
            //OpenBrackets(tokens);

            var lr = ToLeftRight(tokens);

            OptimizeX(lr);

            return new Calculator().Calculate(lr.right);
        }
        
        private LeftRight ToLeftRight(TokenCollection tokens)
        {
            var copy = tokens.Copy();
            var lr = new LeftRight();
            var eqindex = 0;

            if (!IsOperator(copy[0].Kind))
                copy.Insert(0, new SyntaxToken(SyntaxKind.ADD, "+", new Impl.ElementPosition(0, 0), null));

            //Get index of EQUALLY
            while (eqindex < tokens.Count)
            {
                if (tokens[eqindex].Kind == SyntaxKind.EQUALLY)
                    break;
                eqindex++;
            }

            //Move all to left
            for (var i = eqindex + 1; i < tokens.Count; i++)
            {
                if (!IsOperator(tokens[i].Kind))
                {
                    copy.Insert(i, tokens[i]);
                    copy.Insert(i, new SyntaxToken(InvertOperator(GetOperator(tokens, i - 1).Kind), null, new Impl.ElementPosition(0, 0), null));
                }
            }

            //Get index of EQUALLY
            eqindex = 0;
            while (eqindex < copy.Count)
            {
                if (copy[eqindex].Kind == SyntaxKind.EQUALLY)
                    break;
                eqindex++;
            }

            //Remove all in left
            var count = copy.Count;
            for (var i = eqindex; i < count; i++)
            {
                copy.RemoveAt(eqindex);
            }

            //Sort
            lr.left = new TokenCollection();
            lr.right = new TokenCollection();
            for (var i = 0; i < copy.Count; i++)
            {
                if (!IsOperator(copy[i].Kind))
                {
                    if (copy[i].Kind == SyntaxKind.LETTER)
                    {
                        lr.left.Add(new SyntaxToken(copy[i - 1].Kind, null, new Impl.ElementPosition(0, 0), null));
                        lr.left.Add(copy[i]);
                    }
                    else
                    {
                        lr.right.Add(new SyntaxToken(InvertOperator(copy[i - 1].Kind), null, new Impl.ElementPosition(0, 0), null));
                        lr.right.Add(copy[i]);
                    }
                }
            }

            return lr;
        }

        private SyntaxToken GetOperator(TokenCollection tokens, int index)
        {
            if (IsOperator(tokens[index].Kind))
                return tokens[index];
            return new SyntaxToken(SyntaxKind.ADD, "+", new Impl.ElementPosition(0, 0), null);
        }

        public bool IsOperator(SyntaxKind kind)
        {
            if (kind == SyntaxKind.ADD ||
                kind == SyntaxKind.DIV ||
                kind == SyntaxKind.MUL ||
                kind == SyntaxKind.SUB)
                return true;
            return false;
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

        private void OptimizeX(LeftRight lr)
        {
            if (lr.left[0].Kind == SyntaxKind.ADD)
                lr.left.RemoveAt(0);

            while (lr.left.Count > 1)
                for (var priority = OperatorPriority.MaxPriority; priority >= 0; priority--)
                    for (var i = 0; i < lr.left.Count; i++)
                        if (OperatorPriority.Get(lr.left[i]) == priority)
                            ReplaceAction(lr.left, lr.right, i);
        }

        private TokenCollection ReplaceAction(TokenCollection tokens, TokenCollection right, int index)
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
                        value.Add(new SyntaxToken(SyntaxKind.LETTER, "x", new Impl.ElementPosition(0, 0), null));

                        //WTF???!!!
                        right.Insert(0, new SyntaxToken(SyntaxKind.BR_O, "(", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.BR_C, ")", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.POW, "^", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.NUMBER, "0.5", new Impl.ElementPosition(0, 0), 0.5));
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
                        value.Add(new SyntaxToken(SyntaxKind.LETTER, "x", new Impl.ElementPosition(0, 0), null));

                        //WTF???!!!
                        right.Insert(0, new SyntaxToken(SyntaxKind.BR_O, "(", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.BR_C, ")", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.POW, "/", new Impl.ElementPosition(0, 0), null));
                        right.Add(new SyntaxToken(SyntaxKind.NUMBER, "2", new Impl.ElementPosition(0, 0), 0.5));
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

            return tokens;
        }


        //Not finalized
        private void OpenBrackets(TokenCollection tokens, int gen = 1)
        {
            InBracketsExpression ex = new InBracketsExpression();
            while ((ex = GoToNextBrackets(tokens, ex.index)).contin)
            {
                OpenBrackets(ex.inbrackets, gen + 1);
                Console.WriteLine("vobla yobla is open, gen: "+gen+", val: "+ex.inbrackets);
            }
        }

        //Not finalized
        private InBracketsExpression GoToNextBrackets(TokenCollection tokens, int index)
        {
            InBracketsExpression ex = new InBracketsExpression();
            ex.index = index;

            while (ex.index < tokens.Count && tokens[ex.index].Kind != SyntaxKind.BR_O)
                ex.index++;

            if (ex.index >= tokens.Count)
            {
                ex.contin = false;
                return ex;
            }

            ex.length = 0;

            var brackets = 1;
            ex.inbrackets = new TokenCollection();
            while (brackets != 0)
            {
                ex.length++;

                switch (tokens[ex.index + ex.length].Kind)
                {
                    case SyntaxKind.BR_O:
                        brackets++;
                        break;
                    case SyntaxKind.BR_C:
                        brackets--;
                        break;
                }

                if (brackets != 0)
                    ex.inbrackets.Add(tokens[ex.index + ex.length]);
            }
            ex.length++;

            ex.index = ex.index + ex.length;

            ex.contin = true;
            return ex;
        }
    }
}
