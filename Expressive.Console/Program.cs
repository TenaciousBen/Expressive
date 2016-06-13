using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Expressive.Core.Exceptions;
using Expressive.Core.Language.Interpreter;

namespace Expressive.Console
{
    class Program
    {
        private static ValueSource _values = new ValueSource();
        private static FunctionSource _functions = new FunctionSource();

        static void Main(string[] args)
        {
            System.Console.WriteLine("Expressive REPL interpreter");
            System.Console.WriteLine("Type 'exit' to exit, an assignment of the form 'x = expression' to declare a variable, or an expression to be evaluated");
            do
            {
                var input = GetNextInput();
                if (input?.ToLower()?.Trim() == "exit") break;
                if (TypeResolver.IsAssignment(input))
                {
                    TryRegisterValue(input);
                    continue;
                }
                Interpret(input);
            } while (true);
        }

        private static void Interpret(string expression)
        {
            try
            {
                var result = Interpreter.Evaluate(expression, NumericPrecision.Float, _values, _functions);
                if (result.Type == EvaluationType.Enumerable) System.Console.WriteLine(new PrintableList(result.AsList()));
                else System.Console.WriteLine(result.Result);
            }
            catch (LexerException e)
            {
                System.Console.WriteLine($"Lexer failure at index {e.Index}:");
                System.Console.WriteLine(e.Message);
            }
            catch (ParserException e)
            {
                System.Console.WriteLine("Parser failure:");
                System.Console.WriteLine(e.Message);
            }
            catch (EvaluationException e)
            {
                System.Console.WriteLine("Evaluation exception:");
                System.Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Unknown exception:");
                System.Console.WriteLine(e.GetType().Name);
                System.Console.WriteLine(e.Message);
            }
        }

        private static bool TryRegisterValue(string assignment)
        {
            var value = TypeResolver.Resolve(assignment, _values, _functions);
            if (!value.WasRecognised) return false;
            if (_values.ContainsKey(value.Name)) _values[value.Name] = value.Resolved;
            else _values.TryAddValue(value.Name, value.Resolved);
            return true;
        }

        private static string GetNextInput()
        {
            string read = null;
            do
            {
                System.Console.Write("> ");
                read = System.Console.ReadLine();
            } while (string.IsNullOrWhiteSpace(read));
            return read;
        }
    }
}
