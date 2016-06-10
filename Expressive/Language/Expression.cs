using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressive.Core.Extensions;

namespace Expressive.Core.Compiler
{
    internal class Expression
    {
        //    public static object Evaluate(string expression)
        //    {
        //        if (string.IsNullOrEmpty(expression))
        //            return null;
        //        var sanitizedExpression = expression.Replace(" ", "");
        //        var statements = new Queue<Statement>();
        //        var current = "";
        //        for (int i = 0; i < sanitizedExpression.Length; i++)
        //        {
        //            var character = sanitizedExpression[i].ToString();
        //        }
        //        //SUM((1*2)+3)
        //    }

        //    private static Statement GetStatement(string expression)
        //    {
        //        var statement = new Statement();
        //        var current = "";
        //        for (int i = 0; i < expression.Length; i++)
        //        {
        //            var character = expression[i].ToString();
        //            if (character == Constants.OpenScope)
        //            {
        //                current = "";
        //            }
        //        }
        //    }
        //}

        //internal class Statement
        //{
        //    public Queue<Statement> Statements { get; set; }
        //    public string EncloseLeft { get; set; }
        //    public string EncloseRight { get; set; }

        //    public Statement()
        //    {
        //        Statements = new Queue<Statement>();
        //    }
        //}

        //internal enum TokenType
        //{
        //    Symbol,
        //    Comparison,
        //    Math
        //}
    }
}
