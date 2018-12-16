namespace Parser.Internal
{
    internal class FileSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxList _statementList;
        internal readonly SyntaxToken _endOfFile;

        internal FileSyntaxNode(
            SyntaxList statementList,
            SyntaxToken endOfFile) : base(TokenKind.File)
        {

            Slots = 2;
            _statementList = statementList;
            _endOfFile = endOfFile;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.FileSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _statementList;
                case 1: return _endOfFile;
                default: return null;
            }
        }
    }

    internal class FunctionDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _functionKeyword;
        internal readonly FunctionOutputDescriptionSyntaxNode _outputDescription;
        internal readonly SyntaxToken _name;
        internal readonly FunctionInputDescriptionSyntaxNode _inputDescription;
        internal readonly SyntaxList<SyntaxToken> _commas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;

        internal FunctionDeclarationSyntaxNode(
            SyntaxToken functionKeyword,
            FunctionOutputDescriptionSyntaxNode outputDescription,
            SyntaxToken name,
            FunctionInputDescriptionSyntaxNode inputDescription,
            SyntaxList<SyntaxToken> commas,
            SyntaxList body,
            SyntaxToken endKeyword) : base(TokenKind.FunctionDeclaration)
        {

            Slots = 7;
            _functionKeyword = functionKeyword;
            _outputDescription = outputDescription;
            _name = name;
            _inputDescription = inputDescription;
            _commas = commas;
            _body = body;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.FunctionDeclarationSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _functionKeyword;
                case 1: return _outputDescription;
                case 2: return _name;
                case 3: return _inputDescription;
                case 4: return _commas;
                case 5: return _body;
                case 6: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class FunctionOutputDescriptionSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxList _outputList;
        internal readonly SyntaxToken _assignmentSign;

        internal FunctionOutputDescriptionSyntaxNode(
            SyntaxList outputList,
            SyntaxToken assignmentSign) : base(TokenKind.FunctionOutputDescription)
        {

            Slots = 2;
            _outputList = outputList;
            _assignmentSign = assignmentSign;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.FunctionOutputDescriptionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _outputList;
                case 1: return _assignmentSign;
                default: return null;
            }
        }
    }

    internal class FunctionInputDescriptionSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _parameterList;
        internal readonly SyntaxToken _closingBracket;

        internal FunctionInputDescriptionSyntaxNode(
            SyntaxToken openingBracket,
            SyntaxList parameterList,
            SyntaxToken closingBracket) : base(TokenKind.FunctionInputDescription)
        {

            Slots = 3;
            _openingBracket = openingBracket;
            _parameterList = parameterList;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.FunctionInputDescriptionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBracket;
                case 1: return _parameterList;
                case 2: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class SwitchStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _switchKeyword;
        internal readonly ExpressionSyntaxNode _switchExpression;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList<SwitchCaseSyntaxNode> _cases;
        internal readonly SyntaxToken _endKeyword;

        internal SwitchStatementSyntaxNode(
            SyntaxToken switchKeyword,
            ExpressionSyntaxNode switchExpression,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList<SwitchCaseSyntaxNode> cases,
            SyntaxToken endKeyword) : base(TokenKind.SwitchStatement)
        {

            Slots = 5;
            _switchKeyword = switchKeyword;
            _switchExpression = switchExpression;
            _optionalCommas = optionalCommas;
            _cases = cases;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.SwitchStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _switchKeyword;
                case 1: return _switchExpression;
                case 2: return _optionalCommas;
                case 3: return _cases;
                case 4: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class SwitchCaseSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _caseKeyword;
        internal readonly ExpressionSyntaxNode _caseIdentifier;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;

        internal SwitchCaseSyntaxNode(
            SyntaxToken caseKeyword,
            ExpressionSyntaxNode caseIdentifier,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList body) : base(TokenKind.SwitchCase)
        {

            Slots = 4;
            _caseKeyword = caseKeyword;
            _caseIdentifier = caseIdentifier;
            _optionalCommas = optionalCommas;
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.SwitchCaseSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _caseKeyword;
                case 1: return _caseIdentifier;
                case 2: return _optionalCommas;
                case 3: return _body;
                default: return null;
            }
        }
    }

    internal class WhileStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _whileKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;

        internal WhileStatementSyntaxNode(
            SyntaxToken whileKeyword,
            ExpressionSyntaxNode condition,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList body,
            SyntaxToken endKeyword) : base(TokenKind.WhileStatement)
        {

            Slots = 5;
            _whileKeyword = whileKeyword;
            _condition = condition;
            _optionalCommas = optionalCommas;
            _body = body;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.WhileStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _whileKeyword;
                case 1: return _condition;
                case 2: return _optionalCommas;
                case 3: return _body;
                case 4: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class ElseifClause : SyntaxNode
    {
        internal readonly SyntaxToken _elseifKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;

        internal ElseifClause(
            SyntaxToken elseifKeyword,
            ExpressionSyntaxNode condition,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList body) : base(TokenKind.ElseifClause)
        {

            Slots = 4;
            _elseifKeyword = elseifKeyword;
            _condition = condition;
            _optionalCommas = optionalCommas;
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ElseifClause(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _elseifKeyword;
                case 1: return _condition;
                case 2: return _optionalCommas;
                case 3: return _body;
                default: return null;
            }
        }
    }

    internal class ElseClause : SyntaxNode
    {
        internal readonly SyntaxToken _elseKeyword;
        internal readonly SyntaxList _body;

        internal ElseClause(
            SyntaxToken elseKeyword,
            SyntaxList body) : base(TokenKind.ElseClause)
        {

            Slots = 2;
            _elseKeyword = elseKeyword;
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ElseClause(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _elseKeyword;
                case 1: return _body;
                default: return null;
            }
        }
    }

    internal class IfStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _ifKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxList<ElseifClause> _elseifClauses;
        internal readonly ElseClause _elseClause;
        internal readonly SyntaxToken _endKeyword;

        internal IfStatementSyntaxNode(
            SyntaxToken ifKeyword,
            ExpressionSyntaxNode condition,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList body,
            SyntaxList<ElseifClause> elseifClauses,
            ElseClause elseClause,
            SyntaxToken endKeyword) : base(TokenKind.IfStatement)
        {

            Slots = 7;
            _ifKeyword = ifKeyword;
            _condition = condition;
            _optionalCommas = optionalCommas;
            _body = body;
            _elseifClauses = elseifClauses;
            _elseClause = elseClause;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.IfStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _ifKeyword;
                case 1: return _condition;
                case 2: return _optionalCommas;
                case 3: return _body;
                case 4: return _elseifClauses;
                case 5: return _elseClause;
                case 6: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class ForStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _forKeyword;
        internal readonly AssignmentExpressionSyntaxNode _assignment;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;

        internal ForStatementSyntaxNode(
            SyntaxToken forKeyword,
            AssignmentExpressionSyntaxNode assignment,
            SyntaxList<SyntaxToken> optionalCommas,
            SyntaxList body,
            SyntaxToken endKeyword) : base(TokenKind.ForStatement)
        {

            Slots = 5;
            _forKeyword = forKeyword;
            _assignment = assignment;
            _optionalCommas = optionalCommas;
            _body = body;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ForStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _forKeyword;
                case 1: return _assignment;
                case 2: return _optionalCommas;
                case 3: return _body;
                case 4: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class AssignmentExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _lhs;
        internal readonly SyntaxToken _assignmentSign;
        internal readonly ExpressionSyntaxNode _rhs;

        internal AssignmentExpressionSyntaxNode(
            ExpressionSyntaxNode lhs,
            SyntaxToken assignmentSign,
            ExpressionSyntaxNode rhs) : base(TokenKind.AssignmentExpression)
        {

            Slots = 3;
            _lhs = lhs;
            _assignmentSign = assignmentSign;
            _rhs = rhs;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.AssignmentExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _lhs;
                case 1: return _assignmentSign;
                case 2: return _rhs;
                default: return null;
            }
        }
    }

    internal class CatchClauseSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _catchKeyword;
        internal readonly SyntaxList _catchBody;

        internal CatchClauseSyntaxNode(
            SyntaxToken catchKeyword,
            SyntaxList catchBody) : base(TokenKind.CatchClause)
        {

            Slots = 2;
            _catchKeyword = catchKeyword;
            _catchBody = catchBody;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.CatchClauseSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _catchKeyword;
                case 1: return _catchBody;
                default: return null;
            }
        }
    }

    internal class TryCatchStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _tryKeyword;
        internal readonly SyntaxList _tryBody;
        internal readonly CatchClauseSyntaxNode _catchClause;
        internal readonly SyntaxToken _endKeyword;

        internal TryCatchStatementSyntaxNode(
            SyntaxToken tryKeyword,
            SyntaxList tryBody,
            CatchClauseSyntaxNode catchClause,
            SyntaxToken endKeyword) : base(TokenKind.TryCatchStatement)
        {

            Slots = 4;
            _tryKeyword = tryKeyword;
            _tryBody = tryBody;
            _catchClause = catchClause;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.TryCatchStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _tryKeyword;
                case 1: return _tryBody;
                case 2: return _catchClause;
                case 3: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class ExpressionStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _expression;

        internal ExpressionStatementSyntaxNode(
            ExpressionSyntaxNode expression) : base(TokenKind.ExpressionStatement)
        {

            Slots = 1;
            _expression = expression;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ExpressionStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _expression;
                default: return null;
            }
        }
    }

    internal class EmptyStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _semicolon;

        internal EmptyStatementSyntaxNode(
            SyntaxToken semicolon) : base(TokenKind.EmptyStatement)
        {

            Slots = 1;
            _semicolon = semicolon;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EmptyStatementSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _semicolon;
                default: return null;
            }
        }
    }

    internal class EmptyExpressionSyntaxNode : ExpressionSyntaxNode
    {

        internal EmptyExpressionSyntaxNode() : base(TokenKind.EmptyExpression)
        {

            Slots = 0;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EmptyExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }
    }

    internal class UnaryPrefixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _operation;
        internal readonly ExpressionSyntaxNode _operand;

        internal UnaryPrefixOperationExpressionSyntaxNode(
            SyntaxToken operation,
            ExpressionSyntaxNode operand) : base(TokenKind.UnaryPrefixOperationExpression)
        {

            Slots = 2;
            _operation = operation;
            _operand = operand;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.UnaryPrefixOperationExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _operation;
                case 1: return _operand;
                default: return null;
            }
        }
    }

    internal class CompoundNameSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxList _nodes;

        internal CompoundNameSyntaxNode(
            SyntaxList nodes) : base(TokenKind.CompoundName)
        {

            Slots = 1;
            _nodes = nodes;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.CompoundNameSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _nodes;
                default: return null;
            }
        }
    }

    internal class NamedFunctionHandleSyntaxNode : FunctionHandleSyntaxNode
    {
        internal readonly SyntaxToken _atSign;
        internal readonly CompoundNameSyntaxNode _functionName;

        internal NamedFunctionHandleSyntaxNode(
            SyntaxToken atSign,
            CompoundNameSyntaxNode functionName) : base(TokenKind.NamedFunctionHandle)
        {

            Slots = 2;
            _atSign = atSign;
            _functionName = functionName;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.NamedFunctionHandleSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _atSign;
                case 1: return _functionName;
                default: return null;
            }
        }
    }

    internal class LambdaSyntaxNode : FunctionHandleSyntaxNode
    {
        internal readonly SyntaxToken _atSign;
        internal readonly FunctionInputDescriptionSyntaxNode _input;
        internal readonly ExpressionSyntaxNode _body;

        internal LambdaSyntaxNode(
            SyntaxToken atSign,
            FunctionInputDescriptionSyntaxNode input,
            ExpressionSyntaxNode body) : base(TokenKind.Lambda)
        {

            Slots = 3;
            _atSign = atSign;
            _input = input;
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.LambdaSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _atSign;
                case 1: return _input;
                case 2: return _body;
                default: return null;
            }
        }
    }

    internal class BinaryOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _lhs;
        internal readonly SyntaxToken _operation;
        internal readonly ExpressionSyntaxNode _rhs;

        internal BinaryOperationExpressionSyntaxNode(
            ExpressionSyntaxNode lhs,
            SyntaxToken operation,
            ExpressionSyntaxNode rhs) : base(TokenKind.BinaryOperation)
        {

            Slots = 3;
            _lhs = lhs;
            _operation = operation;
            _rhs = rhs;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.BinaryOperationExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _lhs;
                case 1: return _operation;
                case 2: return _rhs;
                default: return null;
            }
        }
    }

    internal class IdentifierNameSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _name;

        internal IdentifierNameSyntaxNode(
            SyntaxToken name) : base(TokenKind.IdentifierName)
        {

            Slots = 1;
            _name = name;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.IdentifierNameSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _name;
                default: return null;
            }
        }
    }

    internal class NumberLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _number;

        internal NumberLiteralSyntaxNode(
            SyntaxToken number) : base(TokenKind.NumberLiteralExpression)
        {

            Slots = 1;
            _number = number;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.NumberLiteralSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _number;
                default: return null;
            }
        }
    }

    internal class StringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;

        internal StringLiteralSyntaxNode(
            SyntaxToken stringToken) : base(TokenKind.StringLiteralExpression)
        {

            Slots = 1;
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.StringLiteralSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _stringToken;
                default: return null;
            }
        }
    }

    internal class DoubleQuotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;

        internal DoubleQuotedStringLiteralSyntaxNode(
            SyntaxToken stringToken) : base(TokenKind.DoubleQuotedStringLiteralExpression)
        {

            Slots = 1;
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.DoubleQuotedStringLiteralSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _stringToken;
                default: return null;
            }
        }
    }

    internal class UnquotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;

        internal UnquotedStringLiteralSyntaxNode(
            SyntaxToken stringToken) : base(TokenKind.UnquotedStringLiteralExpression)
        {

            Slots = 1;
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.UnquotedStringLiteralSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _stringToken;
                default: return null;
            }
        }
    }

    internal class ArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingSquareBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingSquareBracket;

        internal ArrayLiteralExpressionSyntaxNode(
            SyntaxToken openingSquareBracket,
            SyntaxList nodes,
            SyntaxToken closingSquareBracket) : base(TokenKind.ArrayLiteralExpression)
        {

            Slots = 3;
            _openingSquareBracket = openingSquareBracket;
            _nodes = nodes;
            _closingSquareBracket = closingSquareBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ArrayLiteralExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingSquareBracket;
                case 1: return _nodes;
                case 2: return _closingSquareBracket;
                default: return null;
            }
        }
    }

    internal class CellArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBrace;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBrace;

        internal CellArrayLiteralExpressionSyntaxNode(
            SyntaxToken openingBrace,
            SyntaxList nodes,
            SyntaxToken closingBrace) : base(TokenKind.CellArrayLiteralExpression)
        {

            Slots = 3;
            _openingBrace = openingBrace;
            _nodes = nodes;
            _closingBrace = closingBrace;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.CellArrayLiteralExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBrace;
                case 1: return _nodes;
                case 2: return _closingBrace;
                default: return null;
            }
        }
    }

    internal class ParenthesizedExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _closingBracket;

        internal ParenthesizedExpressionSyntaxNode(
            SyntaxToken openingBracket,
            ExpressionSyntaxNode expression,
            SyntaxToken closingBracket) : base(TokenKind.ParenthesizedExpression)
        {

            Slots = 3;
            _openingBracket = openingBracket;
            _expression = expression;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ParenthesizedExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBracket;
                case 1: return _expression;
                case 2: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class CellArrayElementAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _openingBrace;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBrace;

        internal CellArrayElementAccessExpressionSyntaxNode(
            ExpressionSyntaxNode expression,
            SyntaxToken openingBrace,
            SyntaxList nodes,
            SyntaxToken closingBrace) : base(TokenKind.CellArrayElementAccess)
        {

            Slots = 4;
            _expression = expression;
            _openingBrace = openingBrace;
            _nodes = nodes;
            _closingBrace = closingBrace;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.CellArrayElementAccessExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _expression;
                case 1: return _openingBrace;
                case 2: return _nodes;
                case 3: return _closingBrace;
                default: return null;
            }
        }
    }

    internal class FunctionCallExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _functionName;
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBracket;

        internal FunctionCallExpressionSyntaxNode(
            ExpressionSyntaxNode functionName,
            SyntaxToken openingBracket,
            SyntaxList nodes,
            SyntaxToken closingBracket) : base(TokenKind.FunctionCall)
        {

            Slots = 4;
            _functionName = functionName;
            _openingBracket = openingBracket;
            _nodes = nodes;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.FunctionCallExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _functionName;
                case 1: return _openingBracket;
                case 2: return _nodes;
                case 3: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class MemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxNode _leftOperand;
        internal readonly SyntaxToken _dot;
        internal readonly SyntaxNode _rightOperand;

        internal MemberAccessSyntaxNode(
            SyntaxNode leftOperand,
            SyntaxToken dot,
            SyntaxNode rightOperand) : base(TokenKind.MemberAccess)
        {

            Slots = 3;
            _leftOperand = leftOperand;
            _dot = dot;
            _rightOperand = rightOperand;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.MemberAccessSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _leftOperand;
                case 1: return _dot;
                case 2: return _rightOperand;
                default: return null;
            }
        }
    }

    internal class UnaryPostixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _operand;
        internal readonly SyntaxToken _operation;

        internal UnaryPostixOperationExpressionSyntaxNode(
            ExpressionSyntaxNode operand,
            SyntaxToken operation) : base(TokenKind.UnaryPostfixOperationExpression)
        {

            Slots = 2;
            _operand = operand;
            _operation = operation;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.UnaryPostixOperationExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _operand;
                case 1: return _operation;
                default: return null;
            }
        }
    }

    internal class IndirectMemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _closingBracket;

        internal IndirectMemberAccessSyntaxNode(
            SyntaxToken openingBracket,
            ExpressionSyntaxNode expression,
            SyntaxToken closingBracket) : base(TokenKind.IndirectMemberAccess)
        {

            Slots = 3;
            _openingBracket = openingBracket;
            _expression = expression;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.IndirectMemberAccessSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBracket;
                case 1: return _expression;
                case 2: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class CommandExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly IdentifierNameSyntaxNode _commandName;
        internal readonly SyntaxList<UnquotedStringLiteralSyntaxNode> _arguments;

        internal CommandExpressionSyntaxNode(
            IdentifierNameSyntaxNode commandName,
            SyntaxList<UnquotedStringLiteralSyntaxNode> arguments) : base(TokenKind.Command)
        {

            Slots = 2;
            _commandName = commandName;
            _arguments = arguments;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.CommandExpressionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _commandName;
                case 1: return _arguments;
                default: return null;
            }
        }
    }

    internal class BaseClassInvokationSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _methodName;
        internal readonly SyntaxToken _atSign;
        internal readonly ExpressionSyntaxNode _baseClassNameAndArguments;

        internal BaseClassInvokationSyntaxNode(
            ExpressionSyntaxNode methodName,
            SyntaxToken atSign,
            ExpressionSyntaxNode baseClassNameAndArguments) : base(TokenKind.ClassInvokation)
        {

            Slots = 3;
            _methodName = methodName;
            _atSign = atSign;
            _baseClassNameAndArguments = baseClassNameAndArguments;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.BaseClassInvokationSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _methodName;
                case 1: return _atSign;
                case 2: return _baseClassNameAndArguments;
                default: return null;
            }
        }
    }

    internal class AttributeAssignmentSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _assignmentSign;
        internal readonly ExpressionSyntaxNode _value;

        internal AttributeAssignmentSyntaxNode(
            SyntaxToken assignmentSign,
            ExpressionSyntaxNode value) : base(TokenKind.AttributeAssignment)
        {

            Slots = 2;
            _assignmentSign = assignmentSign;
            _value = value;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.AttributeAssignmentSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _assignmentSign;
                case 1: return _value;
                default: return null;
            }
        }
    }

    internal class AttributeSyntaxNode : SyntaxNode
    {
        internal readonly IdentifierNameSyntaxNode _name;
        internal readonly AttributeAssignmentSyntaxNode _assignment;

        internal AttributeSyntaxNode(
            IdentifierNameSyntaxNode name,
            AttributeAssignmentSyntaxNode assignment) : base(TokenKind.Attribute)
        {

            Slots = 2;
            _name = name;
            _assignment = assignment;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.AttributeSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _name;
                case 1: return _assignment;
                default: return null;
            }
        }
    }

    internal class AttributeListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBracket;

        internal AttributeListSyntaxNode(
            SyntaxToken openingBracket,
            SyntaxList nodes,
            SyntaxToken closingBracket) : base(TokenKind.AttributeList)
        {

            Slots = 3;
            _openingBracket = openingBracket;
            _nodes = nodes;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.AttributeListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBracket;
                case 1: return _nodes;
                case 2: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class MethodDefinitionSyntaxNode : MethodDeclarationSyntaxNode
    {
        internal readonly SyntaxToken _functionKeyword;
        internal readonly FunctionOutputDescriptionSyntaxNode _outputDescription;
        internal readonly CompoundNameSyntaxNode _name;
        internal readonly FunctionInputDescriptionSyntaxNode _inputDescription;
        internal readonly SyntaxList<SyntaxToken> _commas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;

        internal MethodDefinitionSyntaxNode(
            SyntaxToken functionKeyword,
            FunctionOutputDescriptionSyntaxNode outputDescription,
            CompoundNameSyntaxNode name,
            FunctionInputDescriptionSyntaxNode inputDescription,
            SyntaxList<SyntaxToken> commas,
            SyntaxList body,
            SyntaxToken endKeyword) : base(TokenKind.MethodDefinition)
        {

            Slots = 7;
            _functionKeyword = functionKeyword;
            _outputDescription = outputDescription;
            _name = name;
            _inputDescription = inputDescription;
            _commas = commas;
            _body = body;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.MethodDefinitionSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _functionKeyword;
                case 1: return _outputDescription;
                case 2: return _name;
                case 3: return _inputDescription;
                case 4: return _commas;
                case 5: return _body;
                case 6: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class AbstractMethodDeclarationSyntaxNode : MethodDeclarationSyntaxNode
    {
        internal readonly FunctionOutputDescriptionSyntaxNode _outputDescription;
        internal readonly CompoundNameSyntaxNode _name;
        internal readonly FunctionInputDescriptionSyntaxNode _inputDescription;

        internal AbstractMethodDeclarationSyntaxNode(
            FunctionOutputDescriptionSyntaxNode outputDescription,
            CompoundNameSyntaxNode name,
            FunctionInputDescriptionSyntaxNode inputDescription) : base(TokenKind.AbstractMethodDeclaration)
        {

            Slots = 3;
            _outputDescription = outputDescription;
            _name = name;
            _inputDescription = inputDescription;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.AbstractMethodDeclarationSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _outputDescription;
                case 1: return _name;
                case 2: return _inputDescription;
                default: return null;
            }
        }
    }

    internal class MethodsListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _methodsKeyword;
        internal readonly AttributeListSyntaxNode _attributes;
        internal readonly SyntaxList _methods;
        internal readonly SyntaxToken _endKeyword;

        internal MethodsListSyntaxNode(
            SyntaxToken methodsKeyword,
            AttributeListSyntaxNode attributes,
            SyntaxList methods,
            SyntaxToken endKeyword) : base(TokenKind.MethodsList)
        {

            Slots = 4;
            _methodsKeyword = methodsKeyword;
            _attributes = attributes;
            _methods = methods;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.MethodsListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _methodsKeyword;
                case 1: return _attributes;
                case 2: return _methods;
                case 3: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class PropertiesListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _propertiesKeyword;
        internal readonly AttributeListSyntaxNode _attributes;
        internal readonly SyntaxList _properties;
        internal readonly SyntaxToken _endKeyword;

        internal PropertiesListSyntaxNode(
            SyntaxToken propertiesKeyword,
            AttributeListSyntaxNode attributes,
            SyntaxList properties,
            SyntaxToken endKeyword) : base(TokenKind.PropertiesList)
        {

            Slots = 4;
            _propertiesKeyword = propertiesKeyword;
            _attributes = attributes;
            _properties = properties;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.PropertiesListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _propertiesKeyword;
                case 1: return _attributes;
                case 2: return _properties;
                case 3: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class BaseClassListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _lessSign;
        internal readonly SyntaxList _baseClasses;

        internal BaseClassListSyntaxNode(
            SyntaxToken lessSign,
            SyntaxList baseClasses) : base(TokenKind.BaseClassList)
        {

            Slots = 2;
            _lessSign = lessSign;
            _baseClasses = baseClasses;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.BaseClassListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _lessSign;
                case 1: return _baseClasses;
                default: return null;
            }
        }
    }

    internal class ClassDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _classdefKeyword;
        internal readonly AttributeListSyntaxNode _attributes;
        internal readonly IdentifierNameSyntaxNode _className;
        internal readonly BaseClassListSyntaxNode _baseClassList;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _endKeyword;

        internal ClassDeclarationSyntaxNode(
            SyntaxToken classdefKeyword,
            AttributeListSyntaxNode attributes,
            IdentifierNameSyntaxNode className,
            BaseClassListSyntaxNode baseClassList,
            SyntaxList nodes,
            SyntaxToken endKeyword) : base(TokenKind.ClassDeclaration)
        {

            Slots = 6;
            _classdefKeyword = classdefKeyword;
            _attributes = attributes;
            _className = className;
            _baseClassList = baseClassList;
            _nodes = nodes;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.ClassDeclarationSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _classdefKeyword;
                case 1: return _attributes;
                case 2: return _className;
                case 3: return _baseClassList;
                case 4: return _nodes;
                case 5: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class EnumerationItemValueSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _values;
        internal readonly SyntaxToken _closingBracket;

        internal EnumerationItemValueSyntaxNode(
            SyntaxToken openingBracket,
            SyntaxList values,
            SyntaxToken closingBracket) : base(TokenKind.EnumerationItemValue)
        {

            Slots = 3;
            _openingBracket = openingBracket;
            _values = values;
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EnumerationItemValueSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _openingBracket;
                case 1: return _values;
                case 2: return _closingBracket;
                default: return null;
            }
        }
    }

    internal class EnumerationItemSyntaxNode : SyntaxNode
    {
        internal readonly IdentifierNameSyntaxNode _name;
        internal readonly EnumerationItemValueSyntaxNode _values;
        internal readonly SyntaxList<SyntaxToken> _commas;

        internal EnumerationItemSyntaxNode(
            IdentifierNameSyntaxNode name,
            EnumerationItemValueSyntaxNode values,
            SyntaxList<SyntaxToken> commas) : base(TokenKind.EnumerationItem)
        {

            Slots = 3;
            _name = name;
            _values = values;
            _commas = commas;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EnumerationItemSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _name;
                case 1: return _values;
                case 2: return _commas;
                default: return null;
            }
        }
    }

    internal class EnumerationListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _enumerationKeyword;
        internal readonly AttributeListSyntaxNode _attributes;
        internal readonly SyntaxList<EnumerationItemSyntaxNode> _items;
        internal readonly SyntaxToken _endKeyword;

        internal EnumerationListSyntaxNode(
            SyntaxToken enumerationKeyword,
            AttributeListSyntaxNode attributes,
            SyntaxList<EnumerationItemSyntaxNode> items,
            SyntaxToken endKeyword) : base(TokenKind.EnumerationList)
        {

            Slots = 4;
            _enumerationKeyword = enumerationKeyword;
            _attributes = attributes;
            _items = items;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EnumerationListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _enumerationKeyword;
                case 1: return _attributes;
                case 2: return _items;
                case 3: return _endKeyword;
                default: return null;
            }
        }
    }

    internal class EventsListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _eventsKeyword;
        internal readonly AttributeListSyntaxNode _attributes;
        internal readonly SyntaxList _events;
        internal readonly SyntaxToken _endKeyword;

        internal EventsListSyntaxNode(
            SyntaxToken eventsKeyword,
            AttributeListSyntaxNode attributes,
            SyntaxList events,
            SyntaxToken endKeyword) : base(TokenKind.EventsList)
        {

            Slots = 4;
            _eventsKeyword = eventsKeyword;
            _attributes = attributes;
            _events = events;
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent)
        {
            return new Parser.EventsListSyntaxNode(parent, this);
        }

        public override GreenNode GetSlot(int i)
        {
            switch (i)
            {
                case 0: return _eventsKeyword;
                case 1: return _attributes;
                case 2: return _events;
                case 3: return _endKeyword;
                default: return null;
            }
        }
    }
}
