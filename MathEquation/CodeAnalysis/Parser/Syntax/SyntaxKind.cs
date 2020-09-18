namespace MathEquation.CodeAnalysis.Parser.Syntax
{
    public enum SyntaxKind
    {
        NumberToken,
        BR_O,       //(
        BR_C,       //)

        MUL,        // *
        DIV,        // /
        SUB,        // -
        ADD,        // +
        EQUALLY,    // =

        //Syntax

        NumberExpression,
        BinaryExpression,





        //Invisible is \0, \n, any whitespaces
        Invisible,
        EOE, //End of Expression
        InvalidToken,
        ParethizedExpression
    }
}
