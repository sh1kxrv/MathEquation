using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Parser.Syntax
{
    public enum SyntaxKind
    {
        NUMBER,
        BR_O,
        BR_C,

        MUL,        // *
        DIV,        // /
        SUB,        // -
        ADD,        // +
        EQUALLY,    // =

        EOE, //End of Expression
        InvalidToken,

        //Invisible is \0, \n, any whitespaces
        Invisible,

        //Syntax

        NumberExpression,
        BinaryExpression
    }
}
