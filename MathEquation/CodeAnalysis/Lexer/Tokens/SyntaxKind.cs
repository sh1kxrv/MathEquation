using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathEquation.CodeAnalysis.Lexer.Tokens
{
    public enum SyntaxKind
    {
        NUMBER,
        OPENBRACKET,
        CLOSEBRACKET,

        MUL,        // *
        DIV,        // /
        SUB,        // -
        ADD,        // +
        EQUALLY,    // =

        EOE, //End of Expression
        InvalidToken,

        //Invisible is \0, \n, any whitespaces
        Invisible
    }
}
