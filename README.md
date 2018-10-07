# MParser

A parser for MATLAB language.

A single solution contains the following projects:

* **Parser** contains the main lexing & parsing code. The main functionality
is taking a piece of text and parsing it into an abstract syntax tree (AST).
You can parse a whole .m file as well as one expression.
* **Parser.Tests** contains some tests for the parsing code. For example,
`ThereAndBackAgain.TestEverything()` parses all the files in MATLAB's
toolbox folder (you'll need a MATLAB installation to do this) and checks
that the text constructed from the resulting ASTs is the same as input text.
* **Semantics** contains some attempts at the next step: inferring semantics
of the code from ASTs. Examples may include tracking variable usage,
external function calls, etc.
* **SyntaxGenerator** is a stand-alone console program that generates a part
of parsing code (mostly class definitions for syntax nodes) from a simple XML.
We use it to avoid writing tons of boilerplate code by hand.
* **ConsoleDemo** is a console project used to play with parsing, print the
resulting ASTs (possibly with syntax highlighting), etc.

The parser architecture is inspired by Microsoft's
[Roslyn](https://github.com/dotnet/roslyn) compiler for C#. Studying Roslyn
source code is a great way to learn about lexers, parsers, and all other
parts of a compiler.
