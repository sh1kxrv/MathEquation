namespace MathEquation.CodeAnalysis.Lexer.Tokens
{
    public enum SyntaxKind
    {
        NUMBER,
        LETTER,
        BR_O,
        BR_C,

        MUL,        // *
        DIV,        // /
        SUB,        // -
        ADD,        // +
        POW,        // ^
        FACT,       // !
        EQUALLY,    // =

        //Syntax

        NumberExpression,
        BinaryExpression,





        //Invisible is \0, \n, any whitespaces
        Invisible,
        EOE, //End of Expression
        InvalidToken
    }

    public static class SyntaxKindExpression
    {
        public static string KindToString(this SyntaxKind kind)
        {
            switch (kind)
            {
                case SyntaxKind.BR_O: return "(";
                case SyntaxKind.BR_C: return ")";
                case SyntaxKind.MUL: return "*";
                case SyntaxKind.DIV: return "/";
                case SyntaxKind.SUB: return "-";
                case SyntaxKind.ADD: return "+";
                case SyntaxKind.POW: return "^";
                case SyntaxKind.FACT: return "!";
                case SyntaxKind.EQUALLY: return "=";
                case SyntaxKind.Invisible:
                case SyntaxKind.EOE:
                    return "";
                default: return kind.ToString();
            }
        }
    }
}
