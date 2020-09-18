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
        EQUALLY,    // =

        //Syntax

        NumberExpression,
        BinaryExpression,





        //Invisible is \0, \n, any whitespaces
        Invisible,
        EOE, //End of Expression
        InvalidToken
    }
}
