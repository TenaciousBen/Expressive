## Purpose

Expressive is an MIT-licensed interpreted expression language in C#. It evaluates Excel-like formulae, with the ability to use user-defined functions and variables such that

`Evaluate("[a] + SUM([b], 2, 3)")`

will evaluate to the correct numerical, date, boolean, enumerable or text value given that the user has specified the values of `[a]`, `[b]` and `SUM`.

## Usage

Install via nuget with the following command:

`Install-Package ExpressiveLang`

The Interpreter exists in Expressive.Core.Language.Interpreter and can either be used statically (directly calling Evaluate) or by instance. If used statically, the functions and variables/values available to the interpreter must be passed into the Evaluate method. If used by instance, the interpreter's variable and function context can be set in its properties, which will then be supplied to all calls to Evaluate. The included Expressive.Console project shows a REPL implementation for the interpreter, showing normal static usage of the Interpreter.

## Syntax

The following literals are understood by Expressive:

| Type | Example | Usage |
| ---- | ------- | ----- |
| Integer | 1 | Creates an Int32 with the specified value. |
| Real Number | 1.5 | Creates either a Decimal or a Float, depending on the NumericPrecision supplied to the interpreter, with the specified value. |
| String | "foo" or 'foo' | Creates a string with the specified value. |
| Scoped expression | (1 + 2) | Works as you'd expect in maths: evaluates preferrentially in an operation, such that (1 + 2) * 2 returns 6 rather than 5. |
| Separated expression | (1, 2, 3) | Creates a List<EvaluationResult>, really only useful as parameters to a function. |
| Function expression | SomeFunction(1, 2, 3) or SomeFunction(1) or SomeFunction() | Invokes a function from the FunctionSource passed into the interpreter, passing the specified parameters. Evaluates parameters prior to invoking the function, such that e.g. SomeFunction(1, 3 + 4, (8 * 2) / 4) can evaluate just fine, with SomeFunction receiving a single List<EvaluationResult> { 1, 7, 4 }. |
| Replacement symbol | [foo] or [foo bar] or [foo123] | A variable to be resolved to a value from the ValueSource passed into the interpreter. If the variable resolves to an EvaluationType.Expression, the variable will be interpreted, otherwise it will be used as is. |

The following function uses every literal understood by expressive:

`SumAndStringify([a], [b], 1.0 * 3) + (" is " + 'too much')`

assuming the following ValueSource definitions:

`[a] = 1
[b] = 2`

and the following FunctionSource definition:

`SumAndStringify = reference to some ExternalFunction which sums its input and returns it as a string`

the result of interpreting the expression will be the following string:

`6 is too much`


## License

This project is licensed under the MIT license.