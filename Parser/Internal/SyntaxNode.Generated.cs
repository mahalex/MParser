#nullable enable
namespace Parser.Internal
{
    internal class FileSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxList _statementList;
        internal readonly SyntaxToken _endOfFile;
        internal FileSyntaxNode(SyntaxList statementList, SyntaxToken endOfFile): base(TokenKind.File)
        {
            Slots = 2;
            this.AdjustWidth(statementList);
            _statementList = statementList;
            this.AdjustWidth(endOfFile);
            _endOfFile = endOfFile;
        }

        internal FileSyntaxNode(SyntaxList statementList, SyntaxToken endOfFile, TokenDiagnostic[] diagnostics): base(TokenKind.File, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(statementList);
            _statementList = statementList;
            this.AdjustWidth(endOfFile);
            _endOfFile = endOfFile;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.FileSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new FileSyntaxNode(_statementList, _endOfFile, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _statementList, 1 => _endOfFile, _ => null
            }

            ;
        }
    }

    internal class FunctionDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _functionKeyword;
        internal readonly FunctionOutputDescriptionSyntaxNode? _outputDescription;
        internal readonly SyntaxToken _name;
        internal readonly FunctionInputDescriptionSyntaxNode? _inputDescription;
        internal readonly SyntaxList<SyntaxToken> _commas;
        internal readonly SyntaxList _body;
        internal readonly EndKeywordSyntaxNode? _endKeyword;
        internal FunctionDeclarationSyntaxNode(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, SyntaxToken name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, SyntaxList body, EndKeywordSyntaxNode? endKeyword): base(TokenKind.FunctionDeclaration)
        {
            Slots = 7;
            this.AdjustWidth(functionKeyword);
            _functionKeyword = functionKeyword;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
            this.AdjustWidth(commas);
            _commas = commas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal FunctionDeclarationSyntaxNode(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, SyntaxToken name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, SyntaxList body, EndKeywordSyntaxNode? endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.FunctionDeclaration, diagnostics)
        {
            Slots = 7;
            this.AdjustWidth(functionKeyword);
            _functionKeyword = functionKeyword;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
            this.AdjustWidth(commas);
            _commas = commas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.FunctionDeclarationSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new FunctionDeclarationSyntaxNode(_functionKeyword, _outputDescription, _name, _inputDescription, _commas, _body, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _functionKeyword, 1 => _outputDescription, 2 => _name, 3 => _inputDescription, 4 => _commas, 5 => _body, 6 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class FunctionOutputDescriptionSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxList _outputList;
        internal readonly SyntaxToken _assignmentSign;
        internal FunctionOutputDescriptionSyntaxNode(SyntaxList outputList, SyntaxToken assignmentSign): base(TokenKind.FunctionOutputDescription)
        {
            Slots = 2;
            this.AdjustWidth(outputList);
            _outputList = outputList;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
        }

        internal FunctionOutputDescriptionSyntaxNode(SyntaxList outputList, SyntaxToken assignmentSign, TokenDiagnostic[] diagnostics): base(TokenKind.FunctionOutputDescription, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(outputList);
            _outputList = outputList;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.FunctionOutputDescriptionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new FunctionOutputDescriptionSyntaxNode(_outputList, _assignmentSign, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _outputList, 1 => _assignmentSign, _ => null
            }

            ;
        }
    }

    internal class FunctionInputDescriptionSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _parameterList;
        internal readonly SyntaxToken _closingBracket;
        internal FunctionInputDescriptionSyntaxNode(SyntaxToken openingBracket, SyntaxList parameterList, SyntaxToken closingBracket): base(TokenKind.FunctionInputDescription)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(parameterList);
            _parameterList = parameterList;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal FunctionInputDescriptionSyntaxNode(SyntaxToken openingBracket, SyntaxList parameterList, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.FunctionInputDescription, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(parameterList);
            _parameterList = parameterList;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.FunctionInputDescriptionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new FunctionInputDescriptionSyntaxNode(_openingBracket, _parameterList, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBracket, 1 => _parameterList, 2 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class SwitchStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _switchKeyword;
        internal readonly ExpressionSyntaxNode _switchExpression;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList<SwitchCaseSyntaxNode> _cases;
        internal readonly SyntaxToken _endKeyword;
        internal SwitchStatementSyntaxNode(SyntaxToken switchKeyword, ExpressionSyntaxNode switchExpression, SyntaxList<SyntaxToken> optionalCommas, SyntaxList<SwitchCaseSyntaxNode> cases, SyntaxToken endKeyword): base(TokenKind.SwitchStatement)
        {
            Slots = 5;
            this.AdjustWidth(switchKeyword);
            _switchKeyword = switchKeyword;
            this.AdjustWidth(switchExpression);
            _switchExpression = switchExpression;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(cases);
            _cases = cases;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal SwitchStatementSyntaxNode(SyntaxToken switchKeyword, ExpressionSyntaxNode switchExpression, SyntaxList<SyntaxToken> optionalCommas, SyntaxList<SwitchCaseSyntaxNode> cases, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.SwitchStatement, diagnostics)
        {
            Slots = 5;
            this.AdjustWidth(switchKeyword);
            _switchKeyword = switchKeyword;
            this.AdjustWidth(switchExpression);
            _switchExpression = switchExpression;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(cases);
            _cases = cases;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.SwitchStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new SwitchStatementSyntaxNode(_switchKeyword, _switchExpression, _optionalCommas, _cases, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _switchKeyword, 1 => _switchExpression, 2 => _optionalCommas, 3 => _cases, 4 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class SwitchCaseSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _caseKeyword;
        internal readonly ExpressionSyntaxNode _caseIdentifier;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal SwitchCaseSyntaxNode(SyntaxToken caseKeyword, ExpressionSyntaxNode caseIdentifier, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body): base(TokenKind.SwitchCase)
        {
            Slots = 4;
            this.AdjustWidth(caseKeyword);
            _caseKeyword = caseKeyword;
            this.AdjustWidth(caseIdentifier);
            _caseIdentifier = caseIdentifier;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
        }

        internal SwitchCaseSyntaxNode(SyntaxToken caseKeyword, ExpressionSyntaxNode caseIdentifier, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, TokenDiagnostic[] diagnostics): base(TokenKind.SwitchCase, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(caseKeyword);
            _caseKeyword = caseKeyword;
            this.AdjustWidth(caseIdentifier);
            _caseIdentifier = caseIdentifier;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.SwitchCaseSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new SwitchCaseSyntaxNode(_caseKeyword, _caseIdentifier, _optionalCommas, _body, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _caseKeyword, 1 => _caseIdentifier, 2 => _optionalCommas, 3 => _body, _ => null
            }

            ;
        }
    }

    internal class WhileStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _whileKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;
        internal WhileStatementSyntaxNode(SyntaxToken whileKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxToken endKeyword): base(TokenKind.WhileStatement)
        {
            Slots = 5;
            this.AdjustWidth(whileKeyword);
            _whileKeyword = whileKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal WhileStatementSyntaxNode(SyntaxToken whileKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.WhileStatement, diagnostics)
        {
            Slots = 5;
            this.AdjustWidth(whileKeyword);
            _whileKeyword = whileKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.WhileStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new WhileStatementSyntaxNode(_whileKeyword, _condition, _optionalCommas, _body, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _whileKeyword, 1 => _condition, 2 => _optionalCommas, 3 => _body, 4 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class ElseifClause : SyntaxNode
    {
        internal readonly SyntaxToken _elseifKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal ElseifClause(SyntaxToken elseifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body): base(TokenKind.ElseifClause)
        {
            Slots = 4;
            this.AdjustWidth(elseifKeyword);
            _elseifKeyword = elseifKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
        }

        internal ElseifClause(SyntaxToken elseifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, TokenDiagnostic[] diagnostics): base(TokenKind.ElseifClause, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(elseifKeyword);
            _elseifKeyword = elseifKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ElseifClause(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ElseifClause(_elseifKeyword, _condition, _optionalCommas, _body, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _elseifKeyword, 1 => _condition, 2 => _optionalCommas, 3 => _body, _ => null
            }

            ;
        }
    }

    internal class ElseClause : SyntaxNode
    {
        internal readonly SyntaxToken _elseKeyword;
        internal readonly SyntaxList _body;
        internal ElseClause(SyntaxToken elseKeyword, SyntaxList body): base(TokenKind.ElseClause)
        {
            Slots = 2;
            this.AdjustWidth(elseKeyword);
            _elseKeyword = elseKeyword;
            this.AdjustWidth(body);
            _body = body;
        }

        internal ElseClause(SyntaxToken elseKeyword, SyntaxList body, TokenDiagnostic[] diagnostics): base(TokenKind.ElseClause, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(elseKeyword);
            _elseKeyword = elseKeyword;
            this.AdjustWidth(body);
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ElseClause(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ElseClause(_elseKeyword, _body, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _elseKeyword, 1 => _body, _ => null
            }

            ;
        }
    }

    internal class IfStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _ifKeyword;
        internal readonly ExpressionSyntaxNode _condition;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxList<ElseifClause> _elseifClauses;
        internal readonly ElseClause? _elseClause;
        internal readonly SyntaxToken _endKeyword;
        internal IfStatementSyntaxNode(SyntaxToken ifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxList<ElseifClause> elseifClauses, ElseClause? elseClause, SyntaxToken endKeyword): base(TokenKind.IfStatement)
        {
            Slots = 7;
            this.AdjustWidth(ifKeyword);
            _ifKeyword = ifKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(elseifClauses);
            _elseifClauses = elseifClauses;
            this.AdjustWidth(elseClause);
            _elseClause = elseClause;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal IfStatementSyntaxNode(SyntaxToken ifKeyword, ExpressionSyntaxNode condition, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxList<ElseifClause> elseifClauses, ElseClause? elseClause, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.IfStatement, diagnostics)
        {
            Slots = 7;
            this.AdjustWidth(ifKeyword);
            _ifKeyword = ifKeyword;
            this.AdjustWidth(condition);
            _condition = condition;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(elseifClauses);
            _elseifClauses = elseifClauses;
            this.AdjustWidth(elseClause);
            _elseClause = elseClause;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.IfStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new IfStatementSyntaxNode(_ifKeyword, _condition, _optionalCommas, _body, _elseifClauses, _elseClause, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _ifKeyword, 1 => _condition, 2 => _optionalCommas, 3 => _body, 4 => _elseifClauses, 5 => _elseClause, 6 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class ForStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _forKeyword;
        internal readonly AssignmentExpressionSyntaxNode _assignment;
        internal readonly SyntaxList<SyntaxToken> _optionalCommas;
        internal readonly SyntaxList _body;
        internal readonly SyntaxToken _endKeyword;
        internal ForStatementSyntaxNode(SyntaxToken forKeyword, AssignmentExpressionSyntaxNode assignment, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxToken endKeyword): base(TokenKind.ForStatement)
        {
            Slots = 5;
            this.AdjustWidth(forKeyword);
            _forKeyword = forKeyword;
            this.AdjustWidth(assignment);
            _assignment = assignment;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal ForStatementSyntaxNode(SyntaxToken forKeyword, AssignmentExpressionSyntaxNode assignment, SyntaxList<SyntaxToken> optionalCommas, SyntaxList body, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.ForStatement, diagnostics)
        {
            Slots = 5;
            this.AdjustWidth(forKeyword);
            _forKeyword = forKeyword;
            this.AdjustWidth(assignment);
            _assignment = assignment;
            this.AdjustWidth(optionalCommas);
            _optionalCommas = optionalCommas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ForStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ForStatementSyntaxNode(_forKeyword, _assignment, _optionalCommas, _body, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _forKeyword, 1 => _assignment, 2 => _optionalCommas, 3 => _body, 4 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class AssignmentExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _lhs;
        internal readonly SyntaxToken _assignmentSign;
        internal readonly ExpressionSyntaxNode _rhs;
        internal AssignmentExpressionSyntaxNode(ExpressionSyntaxNode lhs, SyntaxToken assignmentSign, ExpressionSyntaxNode rhs): base(TokenKind.AssignmentExpression)
        {
            Slots = 3;
            this.AdjustWidth(lhs);
            _lhs = lhs;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
            this.AdjustWidth(rhs);
            _rhs = rhs;
        }

        internal AssignmentExpressionSyntaxNode(ExpressionSyntaxNode lhs, SyntaxToken assignmentSign, ExpressionSyntaxNode rhs, TokenDiagnostic[] diagnostics): base(TokenKind.AssignmentExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(lhs);
            _lhs = lhs;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
            this.AdjustWidth(rhs);
            _rhs = rhs;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.AssignmentExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new AssignmentExpressionSyntaxNode(_lhs, _assignmentSign, _rhs, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _lhs, 1 => _assignmentSign, 2 => _rhs, _ => null
            }

            ;
        }
    }

    internal class CatchClauseSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _catchKeyword;
        internal readonly SyntaxList _catchBody;
        internal CatchClauseSyntaxNode(SyntaxToken catchKeyword, SyntaxList catchBody): base(TokenKind.CatchClause)
        {
            Slots = 2;
            this.AdjustWidth(catchKeyword);
            _catchKeyword = catchKeyword;
            this.AdjustWidth(catchBody);
            _catchBody = catchBody;
        }

        internal CatchClauseSyntaxNode(SyntaxToken catchKeyword, SyntaxList catchBody, TokenDiagnostic[] diagnostics): base(TokenKind.CatchClause, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(catchKeyword);
            _catchKeyword = catchKeyword;
            this.AdjustWidth(catchBody);
            _catchBody = catchBody;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.CatchClauseSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new CatchClauseSyntaxNode(_catchKeyword, _catchBody, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _catchKeyword, 1 => _catchBody, _ => null
            }

            ;
        }
    }

    internal class TryCatchStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _tryKeyword;
        internal readonly SyntaxList _tryBody;
        internal readonly CatchClauseSyntaxNode? _catchClause;
        internal readonly SyntaxToken _endKeyword;
        internal TryCatchStatementSyntaxNode(SyntaxToken tryKeyword, SyntaxList tryBody, CatchClauseSyntaxNode? catchClause, SyntaxToken endKeyword): base(TokenKind.TryCatchStatement)
        {
            Slots = 4;
            this.AdjustWidth(tryKeyword);
            _tryKeyword = tryKeyword;
            this.AdjustWidth(tryBody);
            _tryBody = tryBody;
            this.AdjustWidth(catchClause);
            _catchClause = catchClause;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal TryCatchStatementSyntaxNode(SyntaxToken tryKeyword, SyntaxList tryBody, CatchClauseSyntaxNode? catchClause, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.TryCatchStatement, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(tryKeyword);
            _tryKeyword = tryKeyword;
            this.AdjustWidth(tryBody);
            _tryBody = tryBody;
            this.AdjustWidth(catchClause);
            _catchClause = catchClause;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.TryCatchStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new TryCatchStatementSyntaxNode(_tryKeyword, _tryBody, _catchClause, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _tryKeyword, 1 => _tryBody, 2 => _catchClause, 3 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class ExpressionStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _expression;
        internal ExpressionStatementSyntaxNode(ExpressionSyntaxNode expression): base(TokenKind.ExpressionStatement)
        {
            Slots = 1;
            this.AdjustWidth(expression);
            _expression = expression;
        }

        internal ExpressionStatementSyntaxNode(ExpressionSyntaxNode expression, TokenDiagnostic[] diagnostics): base(TokenKind.ExpressionStatement, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(expression);
            _expression = expression;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ExpressionStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ExpressionStatementSyntaxNode(_expression, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _expression, _ => null
            }

            ;
        }
    }

    internal class EmptyStatementSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _semicolon;
        internal EmptyStatementSyntaxNode(SyntaxToken semicolon): base(TokenKind.EmptyStatement)
        {
            Slots = 1;
            this.AdjustWidth(semicolon);
            _semicolon = semicolon;
        }

        internal EmptyStatementSyntaxNode(SyntaxToken semicolon, TokenDiagnostic[] diagnostics): base(TokenKind.EmptyStatement, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(semicolon);
            _semicolon = semicolon;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EmptyStatementSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EmptyStatementSyntaxNode(_semicolon, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _semicolon, _ => null
            }

            ;
        }
    }

    internal class EmptyExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal EmptyExpressionSyntaxNode(): base(TokenKind.EmptyExpression)
        {
            Slots = 0;
        }

        internal EmptyExpressionSyntaxNode(TokenDiagnostic[] diagnostics): base(TokenKind.EmptyExpression, diagnostics)
        {
            Slots = 0;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EmptyExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EmptyExpressionSyntaxNode(diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }
    }

    internal class UnaryPrefixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _operation;
        internal readonly ExpressionSyntaxNode _operand;
        internal UnaryPrefixOperationExpressionSyntaxNode(SyntaxToken operation, ExpressionSyntaxNode operand): base(TokenKind.UnaryPrefixOperationExpression)
        {
            Slots = 2;
            this.AdjustWidth(operation);
            _operation = operation;
            this.AdjustWidth(operand);
            _operand = operand;
        }

        internal UnaryPrefixOperationExpressionSyntaxNode(SyntaxToken operation, ExpressionSyntaxNode operand, TokenDiagnostic[] diagnostics): base(TokenKind.UnaryPrefixOperationExpression, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(operation);
            _operation = operation;
            this.AdjustWidth(operand);
            _operand = operand;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.UnaryPrefixOperationExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new UnaryPrefixOperationExpressionSyntaxNode(_operation, _operand, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _operation, 1 => _operand, _ => null
            }

            ;
        }
    }

    internal class CompoundNameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxList _nodes;
        internal CompoundNameExpressionSyntaxNode(SyntaxList nodes): base(TokenKind.CompoundNameExpression)
        {
            Slots = 1;
            this.AdjustWidth(nodes);
            _nodes = nodes;
        }

        internal CompoundNameExpressionSyntaxNode(SyntaxList nodes, TokenDiagnostic[] diagnostics): base(TokenKind.CompoundNameExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(nodes);
            _nodes = nodes;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.CompoundNameExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new CompoundNameExpressionSyntaxNode(_nodes, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _nodes, _ => null
            }

            ;
        }
    }

    internal class NamedFunctionHandleExpressionSyntaxNode : FunctionHandleExpressionSyntaxNode
    {
        internal readonly SyntaxToken _atSign;
        internal readonly CompoundNameExpressionSyntaxNode _functionName;
        internal NamedFunctionHandleExpressionSyntaxNode(SyntaxToken atSign, CompoundNameExpressionSyntaxNode functionName): base(TokenKind.NamedFunctionHandleExpression)
        {
            Slots = 2;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(functionName);
            _functionName = functionName;
        }

        internal NamedFunctionHandleExpressionSyntaxNode(SyntaxToken atSign, CompoundNameExpressionSyntaxNode functionName, TokenDiagnostic[] diagnostics): base(TokenKind.NamedFunctionHandleExpression, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(functionName);
            _functionName = functionName;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.NamedFunctionHandleExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new NamedFunctionHandleExpressionSyntaxNode(_atSign, _functionName, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _atSign, 1 => _functionName, _ => null
            }

            ;
        }
    }

    internal class LambdaExpressionSyntaxNode : FunctionHandleExpressionSyntaxNode
    {
        internal readonly SyntaxToken _atSign;
        internal readonly FunctionInputDescriptionSyntaxNode _input;
        internal readonly ExpressionSyntaxNode _body;
        internal LambdaExpressionSyntaxNode(SyntaxToken atSign, FunctionInputDescriptionSyntaxNode input, ExpressionSyntaxNode body): base(TokenKind.LambdaExpression)
        {
            Slots = 3;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(input);
            _input = input;
            this.AdjustWidth(body);
            _body = body;
        }

        internal LambdaExpressionSyntaxNode(SyntaxToken atSign, FunctionInputDescriptionSyntaxNode input, ExpressionSyntaxNode body, TokenDiagnostic[] diagnostics): base(TokenKind.LambdaExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(input);
            _input = input;
            this.AdjustWidth(body);
            _body = body;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.LambdaExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new LambdaExpressionSyntaxNode(_atSign, _input, _body, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _atSign, 1 => _input, 2 => _body, _ => null
            }

            ;
        }
    }

    internal class BinaryOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _lhs;
        internal readonly SyntaxToken _operation;
        internal readonly ExpressionSyntaxNode _rhs;
        internal BinaryOperationExpressionSyntaxNode(ExpressionSyntaxNode lhs, SyntaxToken operation, ExpressionSyntaxNode rhs): base(TokenKind.BinaryOperationExpression)
        {
            Slots = 3;
            this.AdjustWidth(lhs);
            _lhs = lhs;
            this.AdjustWidth(operation);
            _operation = operation;
            this.AdjustWidth(rhs);
            _rhs = rhs;
        }

        internal BinaryOperationExpressionSyntaxNode(ExpressionSyntaxNode lhs, SyntaxToken operation, ExpressionSyntaxNode rhs, TokenDiagnostic[] diagnostics): base(TokenKind.BinaryOperationExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(lhs);
            _lhs = lhs;
            this.AdjustWidth(operation);
            _operation = operation;
            this.AdjustWidth(rhs);
            _rhs = rhs;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.BinaryOperationExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new BinaryOperationExpressionSyntaxNode(_lhs, _operation, _rhs, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _lhs, 1 => _operation, 2 => _rhs, _ => null
            }

            ;
        }
    }

    internal class IdentifierNameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _name;
        internal IdentifierNameExpressionSyntaxNode(SyntaxToken name): base(TokenKind.IdentifierNameExpression)
        {
            Slots = 1;
            this.AdjustWidth(name);
            _name = name;
        }

        internal IdentifierNameExpressionSyntaxNode(SyntaxToken name, TokenDiagnostic[] diagnostics): base(TokenKind.IdentifierNameExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(name);
            _name = name;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.IdentifierNameExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new IdentifierNameExpressionSyntaxNode(_name, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _name, _ => null
            }

            ;
        }
    }

    internal class NumberLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _number;
        internal NumberLiteralSyntaxNode(SyntaxToken number): base(TokenKind.NumberLiteralExpression)
        {
            Slots = 1;
            this.AdjustWidth(number);
            _number = number;
        }

        internal NumberLiteralSyntaxNode(SyntaxToken number, TokenDiagnostic[] diagnostics): base(TokenKind.NumberLiteralExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(number);
            _number = number;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.NumberLiteralSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new NumberLiteralSyntaxNode(_number, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _number, _ => null
            }

            ;
        }
    }

    internal class StringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;
        internal StringLiteralSyntaxNode(SyntaxToken stringToken): base(TokenKind.StringLiteralExpression)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal StringLiteralSyntaxNode(SyntaxToken stringToken, TokenDiagnostic[] diagnostics): base(TokenKind.StringLiteralExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.StringLiteralSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new StringLiteralSyntaxNode(_stringToken, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _stringToken, _ => null
            }

            ;
        }
    }

    internal class DoubleQuotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;
        internal DoubleQuotedStringLiteralSyntaxNode(SyntaxToken stringToken): base(TokenKind.DoubleQuotedStringLiteralExpression)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal DoubleQuotedStringLiteralSyntaxNode(SyntaxToken stringToken, TokenDiagnostic[] diagnostics): base(TokenKind.DoubleQuotedStringLiteralExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.DoubleQuotedStringLiteralSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new DoubleQuotedStringLiteralSyntaxNode(_stringToken, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _stringToken, _ => null
            }

            ;
        }
    }

    internal class UnquotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _stringToken;
        internal UnquotedStringLiteralSyntaxNode(SyntaxToken stringToken): base(TokenKind.UnquotedStringLiteralExpression)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal UnquotedStringLiteralSyntaxNode(SyntaxToken stringToken, TokenDiagnostic[] diagnostics): base(TokenKind.UnquotedStringLiteralExpression, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(stringToken);
            _stringToken = stringToken;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.UnquotedStringLiteralSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new UnquotedStringLiteralSyntaxNode(_stringToken, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _stringToken, _ => null
            }

            ;
        }
    }

    internal class ArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingSquareBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingSquareBracket;
        internal ArrayLiteralExpressionSyntaxNode(SyntaxToken openingSquareBracket, SyntaxList nodes, SyntaxToken closingSquareBracket): base(TokenKind.ArrayLiteralExpression)
        {
            Slots = 3;
            this.AdjustWidth(openingSquareBracket);
            _openingSquareBracket = openingSquareBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingSquareBracket);
            _closingSquareBracket = closingSquareBracket;
        }

        internal ArrayLiteralExpressionSyntaxNode(SyntaxToken openingSquareBracket, SyntaxList nodes, SyntaxToken closingSquareBracket, TokenDiagnostic[] diagnostics): base(TokenKind.ArrayLiteralExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingSquareBracket);
            _openingSquareBracket = openingSquareBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingSquareBracket);
            _closingSquareBracket = closingSquareBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ArrayLiteralExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ArrayLiteralExpressionSyntaxNode(_openingSquareBracket, _nodes, _closingSquareBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingSquareBracket, 1 => _nodes, 2 => _closingSquareBracket, _ => null
            }

            ;
        }
    }

    internal class CellArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBrace;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBrace;
        internal CellArrayLiteralExpressionSyntaxNode(SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace): base(TokenKind.CellArrayLiteralExpression)
        {
            Slots = 3;
            this.AdjustWidth(openingBrace);
            _openingBrace = openingBrace;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBrace);
            _closingBrace = closingBrace;
        }

        internal CellArrayLiteralExpressionSyntaxNode(SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace, TokenDiagnostic[] diagnostics): base(TokenKind.CellArrayLiteralExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBrace);
            _openingBrace = openingBrace;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBrace);
            _closingBrace = closingBrace;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.CellArrayLiteralExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new CellArrayLiteralExpressionSyntaxNode(_openingBrace, _nodes, _closingBrace, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBrace, 1 => _nodes, 2 => _closingBrace, _ => null
            }

            ;
        }
    }

    internal class ParenthesizedExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _closingBracket;
        internal ParenthesizedExpressionSyntaxNode(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket): base(TokenKind.ParenthesizedExpression)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal ParenthesizedExpressionSyntaxNode(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.ParenthesizedExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ParenthesizedExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ParenthesizedExpressionSyntaxNode(_openingBracket, _expression, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBracket, 1 => _expression, 2 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class CellArrayElementAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _openingBrace;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBrace;
        internal CellArrayElementAccessExpressionSyntaxNode(ExpressionSyntaxNode expression, SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace): base(TokenKind.CellArrayElementAccessExpression)
        {
            Slots = 4;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(openingBrace);
            _openingBrace = openingBrace;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBrace);
            _closingBrace = closingBrace;
        }

        internal CellArrayElementAccessExpressionSyntaxNode(ExpressionSyntaxNode expression, SyntaxToken openingBrace, SyntaxList nodes, SyntaxToken closingBrace, TokenDiagnostic[] diagnostics): base(TokenKind.CellArrayElementAccessExpression, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(openingBrace);
            _openingBrace = openingBrace;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBrace);
            _closingBrace = closingBrace;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.CellArrayElementAccessExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new CellArrayElementAccessExpressionSyntaxNode(_expression, _openingBrace, _nodes, _closingBrace, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _expression, 1 => _openingBrace, 2 => _nodes, 3 => _closingBrace, _ => null
            }

            ;
        }
    }

    internal class FunctionCallExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _functionName;
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBracket;
        internal FunctionCallExpressionSyntaxNode(ExpressionSyntaxNode functionName, SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket): base(TokenKind.FunctionCallExpression)
        {
            Slots = 4;
            this.AdjustWidth(functionName);
            _functionName = functionName;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal FunctionCallExpressionSyntaxNode(ExpressionSyntaxNode functionName, SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.FunctionCallExpression, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(functionName);
            _functionName = functionName;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.FunctionCallExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new FunctionCallExpressionSyntaxNode(_functionName, _openingBracket, _nodes, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _functionName, 1 => _openingBracket, 2 => _nodes, 3 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class MemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxNode _leftOperand;
        internal readonly SyntaxToken _dot;
        internal readonly SyntaxNode _rightOperand;
        internal MemberAccessSyntaxNode(SyntaxNode leftOperand, SyntaxToken dot, SyntaxNode rightOperand): base(TokenKind.MemberAccessExpression)
        {
            Slots = 3;
            this.AdjustWidth(leftOperand);
            _leftOperand = leftOperand;
            this.AdjustWidth(dot);
            _dot = dot;
            this.AdjustWidth(rightOperand);
            _rightOperand = rightOperand;
        }

        internal MemberAccessSyntaxNode(SyntaxNode leftOperand, SyntaxToken dot, SyntaxNode rightOperand, TokenDiagnostic[] diagnostics): base(TokenKind.MemberAccessExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(leftOperand);
            _leftOperand = leftOperand;
            this.AdjustWidth(dot);
            _dot = dot;
            this.AdjustWidth(rightOperand);
            _rightOperand = rightOperand;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.MemberAccessSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new MemberAccessSyntaxNode(_leftOperand, _dot, _rightOperand, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _leftOperand, 1 => _dot, 2 => _rightOperand, _ => null
            }

            ;
        }
    }

    internal class UnaryPostixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _operand;
        internal readonly SyntaxToken _operation;
        internal UnaryPostixOperationExpressionSyntaxNode(ExpressionSyntaxNode operand, SyntaxToken operation): base(TokenKind.UnaryPostfixOperationExpression)
        {
            Slots = 2;
            this.AdjustWidth(operand);
            _operand = operand;
            this.AdjustWidth(operation);
            _operation = operation;
        }

        internal UnaryPostixOperationExpressionSyntaxNode(ExpressionSyntaxNode operand, SyntaxToken operation, TokenDiagnostic[] diagnostics): base(TokenKind.UnaryPostfixOperationExpression, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(operand);
            _operand = operand;
            this.AdjustWidth(operation);
            _operation = operation;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.UnaryPostixOperationExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new UnaryPostixOperationExpressionSyntaxNode(_operand, _operation, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _operand, 1 => _operation, _ => null
            }

            ;
        }
    }

    internal class IndirectMemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly ExpressionSyntaxNode _expression;
        internal readonly SyntaxToken _closingBracket;
        internal IndirectMemberAccessSyntaxNode(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket): base(TokenKind.IndirectMemberAccessExpression)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal IndirectMemberAccessSyntaxNode(SyntaxToken openingBracket, ExpressionSyntaxNode expression, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.IndirectMemberAccessExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(expression);
            _expression = expression;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.IndirectMemberAccessSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new IndirectMemberAccessSyntaxNode(_openingBracket, _expression, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBracket, 1 => _expression, 2 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class CommandExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly IdentifierNameExpressionSyntaxNode _commandName;
        internal readonly SyntaxList<UnquotedStringLiteralSyntaxNode> _arguments;
        internal CommandExpressionSyntaxNode(IdentifierNameExpressionSyntaxNode commandName, SyntaxList<UnquotedStringLiteralSyntaxNode> arguments): base(TokenKind.CommandExpression)
        {
            Slots = 2;
            this.AdjustWidth(commandName);
            _commandName = commandName;
            this.AdjustWidth(arguments);
            _arguments = arguments;
        }

        internal CommandExpressionSyntaxNode(IdentifierNameExpressionSyntaxNode commandName, SyntaxList<UnquotedStringLiteralSyntaxNode> arguments, TokenDiagnostic[] diagnostics): base(TokenKind.CommandExpression, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(commandName);
            _commandName = commandName;
            this.AdjustWidth(arguments);
            _arguments = arguments;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.CommandExpressionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new CommandExpressionSyntaxNode(_commandName, _arguments, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _commandName, 1 => _arguments, _ => null
            }

            ;
        }
    }

    internal class BaseClassInvokationSyntaxNode : ExpressionSyntaxNode
    {
        internal readonly ExpressionSyntaxNode _methodName;
        internal readonly SyntaxToken _atSign;
        internal readonly ExpressionSyntaxNode _baseClassNameAndArguments;
        internal BaseClassInvokationSyntaxNode(ExpressionSyntaxNode methodName, SyntaxToken atSign, ExpressionSyntaxNode baseClassNameAndArguments): base(TokenKind.ClassInvokationExpression)
        {
            Slots = 3;
            this.AdjustWidth(methodName);
            _methodName = methodName;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(baseClassNameAndArguments);
            _baseClassNameAndArguments = baseClassNameAndArguments;
        }

        internal BaseClassInvokationSyntaxNode(ExpressionSyntaxNode methodName, SyntaxToken atSign, ExpressionSyntaxNode baseClassNameAndArguments, TokenDiagnostic[] diagnostics): base(TokenKind.ClassInvokationExpression, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(methodName);
            _methodName = methodName;
            this.AdjustWidth(atSign);
            _atSign = atSign;
            this.AdjustWidth(baseClassNameAndArguments);
            _baseClassNameAndArguments = baseClassNameAndArguments;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.BaseClassInvokationSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new BaseClassInvokationSyntaxNode(_methodName, _atSign, _baseClassNameAndArguments, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _methodName, 1 => _atSign, 2 => _baseClassNameAndArguments, _ => null
            }

            ;
        }
    }

    internal class AttributeAssignmentSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _assignmentSign;
        internal readonly ExpressionSyntaxNode _value;
        internal AttributeAssignmentSyntaxNode(SyntaxToken assignmentSign, ExpressionSyntaxNode value): base(TokenKind.AttributeAssignment)
        {
            Slots = 2;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
            this.AdjustWidth(value);
            _value = value;
        }

        internal AttributeAssignmentSyntaxNode(SyntaxToken assignmentSign, ExpressionSyntaxNode value, TokenDiagnostic[] diagnostics): base(TokenKind.AttributeAssignment, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(assignmentSign);
            _assignmentSign = assignmentSign;
            this.AdjustWidth(value);
            _value = value;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.AttributeAssignmentSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new AttributeAssignmentSyntaxNode(_assignmentSign, _value, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _assignmentSign, 1 => _value, _ => null
            }

            ;
        }
    }

    internal class AttributeSyntaxNode : SyntaxNode
    {
        internal readonly IdentifierNameExpressionSyntaxNode _name;
        internal readonly AttributeAssignmentSyntaxNode? _assignment;
        internal AttributeSyntaxNode(IdentifierNameExpressionSyntaxNode name, AttributeAssignmentSyntaxNode? assignment): base(TokenKind.Attribute)
        {
            Slots = 2;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(assignment);
            _assignment = assignment;
        }

        internal AttributeSyntaxNode(IdentifierNameExpressionSyntaxNode name, AttributeAssignmentSyntaxNode? assignment, TokenDiagnostic[] diagnostics): base(TokenKind.Attribute, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(assignment);
            _assignment = assignment;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.AttributeSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new AttributeSyntaxNode(_name, _assignment, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _name, 1 => _assignment, _ => null
            }

            ;
        }
    }

    internal class AttributeListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _closingBracket;
        internal AttributeListSyntaxNode(SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket): base(TokenKind.AttributeList)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal AttributeListSyntaxNode(SyntaxToken openingBracket, SyntaxList nodes, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.AttributeList, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.AttributeListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new AttributeListSyntaxNode(_openingBracket, _nodes, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBracket, 1 => _nodes, 2 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class MethodDefinitionSyntaxNode : MethodDeclarationSyntaxNode
    {
        internal readonly SyntaxToken _functionKeyword;
        internal readonly FunctionOutputDescriptionSyntaxNode? _outputDescription;
        internal readonly CompoundNameExpressionSyntaxNode _name;
        internal readonly FunctionInputDescriptionSyntaxNode? _inputDescription;
        internal readonly SyntaxList<SyntaxToken> _commas;
        internal readonly SyntaxList _body;
        internal readonly EndKeywordSyntaxNode? _endKeyword;
        internal MethodDefinitionSyntaxNode(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, SyntaxList body, EndKeywordSyntaxNode? endKeyword): base(TokenKind.ConcreteMethodDeclaration)
        {
            Slots = 7;
            this.AdjustWidth(functionKeyword);
            _functionKeyword = functionKeyword;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
            this.AdjustWidth(commas);
            _commas = commas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal MethodDefinitionSyntaxNode(SyntaxToken functionKeyword, FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription, SyntaxList<SyntaxToken> commas, SyntaxList body, EndKeywordSyntaxNode? endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.ConcreteMethodDeclaration, diagnostics)
        {
            Slots = 7;
            this.AdjustWidth(functionKeyword);
            _functionKeyword = functionKeyword;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
            this.AdjustWidth(commas);
            _commas = commas;
            this.AdjustWidth(body);
            _body = body;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.MethodDefinitionSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new MethodDefinitionSyntaxNode(_functionKeyword, _outputDescription, _name, _inputDescription, _commas, _body, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _functionKeyword, 1 => _outputDescription, 2 => _name, 3 => _inputDescription, 4 => _commas, 5 => _body, 6 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class AbstractMethodDeclarationSyntaxNode : MethodDeclarationSyntaxNode
    {
        internal readonly FunctionOutputDescriptionSyntaxNode? _outputDescription;
        internal readonly CompoundNameExpressionSyntaxNode _name;
        internal readonly FunctionInputDescriptionSyntaxNode? _inputDescription;
        internal AbstractMethodDeclarationSyntaxNode(FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription): base(TokenKind.AbstractMethodDeclaration)
        {
            Slots = 3;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
        }

        internal AbstractMethodDeclarationSyntaxNode(FunctionOutputDescriptionSyntaxNode? outputDescription, CompoundNameExpressionSyntaxNode name, FunctionInputDescriptionSyntaxNode? inputDescription, TokenDiagnostic[] diagnostics): base(TokenKind.AbstractMethodDeclaration, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(outputDescription);
            _outputDescription = outputDescription;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(inputDescription);
            _inputDescription = inputDescription;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.AbstractMethodDeclarationSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new AbstractMethodDeclarationSyntaxNode(_outputDescription, _name, _inputDescription, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _outputDescription, 1 => _name, 2 => _inputDescription, _ => null
            }

            ;
        }
    }

    internal class MethodsListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _methodsKeyword;
        internal readonly AttributeListSyntaxNode? _attributes;
        internal readonly SyntaxList _methods;
        internal readonly SyntaxToken _endKeyword;
        internal MethodsListSyntaxNode(SyntaxToken methodsKeyword, AttributeListSyntaxNode? attributes, SyntaxList methods, SyntaxToken endKeyword): base(TokenKind.MethodsList)
        {
            Slots = 4;
            this.AdjustWidth(methodsKeyword);
            _methodsKeyword = methodsKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(methods);
            _methods = methods;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal MethodsListSyntaxNode(SyntaxToken methodsKeyword, AttributeListSyntaxNode? attributes, SyntaxList methods, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.MethodsList, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(methodsKeyword);
            _methodsKeyword = methodsKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(methods);
            _methods = methods;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.MethodsListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new MethodsListSyntaxNode(_methodsKeyword, _attributes, _methods, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _methodsKeyword, 1 => _attributes, 2 => _methods, 3 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class PropertiesListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _propertiesKeyword;
        internal readonly AttributeListSyntaxNode? _attributes;
        internal readonly SyntaxList _properties;
        internal readonly SyntaxToken _endKeyword;
        internal PropertiesListSyntaxNode(SyntaxToken propertiesKeyword, AttributeListSyntaxNode? attributes, SyntaxList properties, SyntaxToken endKeyword): base(TokenKind.PropertiesList)
        {
            Slots = 4;
            this.AdjustWidth(propertiesKeyword);
            _propertiesKeyword = propertiesKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(properties);
            _properties = properties;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal PropertiesListSyntaxNode(SyntaxToken propertiesKeyword, AttributeListSyntaxNode? attributes, SyntaxList properties, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.PropertiesList, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(propertiesKeyword);
            _propertiesKeyword = propertiesKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(properties);
            _properties = properties;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.PropertiesListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new PropertiesListSyntaxNode(_propertiesKeyword, _attributes, _properties, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _propertiesKeyword, 1 => _attributes, 2 => _properties, 3 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class BaseClassListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _lessSign;
        internal readonly SyntaxList _baseClasses;
        internal BaseClassListSyntaxNode(SyntaxToken lessSign, SyntaxList baseClasses): base(TokenKind.BaseClassList)
        {
            Slots = 2;
            this.AdjustWidth(lessSign);
            _lessSign = lessSign;
            this.AdjustWidth(baseClasses);
            _baseClasses = baseClasses;
        }

        internal BaseClassListSyntaxNode(SyntaxToken lessSign, SyntaxList baseClasses, TokenDiagnostic[] diagnostics): base(TokenKind.BaseClassList, diagnostics)
        {
            Slots = 2;
            this.AdjustWidth(lessSign);
            _lessSign = lessSign;
            this.AdjustWidth(baseClasses);
            _baseClasses = baseClasses;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.BaseClassListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new BaseClassListSyntaxNode(_lessSign, _baseClasses, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _lessSign, 1 => _baseClasses, _ => null
            }

            ;
        }
    }

    internal class ClassDeclarationSyntaxNode : StatementSyntaxNode
    {
        internal readonly SyntaxToken _classdefKeyword;
        internal readonly AttributeListSyntaxNode? _attributes;
        internal readonly IdentifierNameExpressionSyntaxNode _className;
        internal readonly BaseClassListSyntaxNode? _baseClassList;
        internal readonly SyntaxList _nodes;
        internal readonly SyntaxToken _endKeyword;
        internal ClassDeclarationSyntaxNode(SyntaxToken classdefKeyword, AttributeListSyntaxNode? attributes, IdentifierNameExpressionSyntaxNode className, BaseClassListSyntaxNode? baseClassList, SyntaxList nodes, SyntaxToken endKeyword): base(TokenKind.ClassDeclaration)
        {
            Slots = 6;
            this.AdjustWidth(classdefKeyword);
            _classdefKeyword = classdefKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(className);
            _className = className;
            this.AdjustWidth(baseClassList);
            _baseClassList = baseClassList;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal ClassDeclarationSyntaxNode(SyntaxToken classdefKeyword, AttributeListSyntaxNode? attributes, IdentifierNameExpressionSyntaxNode className, BaseClassListSyntaxNode? baseClassList, SyntaxList nodes, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.ClassDeclaration, diagnostics)
        {
            Slots = 6;
            this.AdjustWidth(classdefKeyword);
            _classdefKeyword = classdefKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(className);
            _className = className;
            this.AdjustWidth(baseClassList);
            _baseClassList = baseClassList;
            this.AdjustWidth(nodes);
            _nodes = nodes;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.ClassDeclarationSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new ClassDeclarationSyntaxNode(_classdefKeyword, _attributes, _className, _baseClassList, _nodes, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _classdefKeyword, 1 => _attributes, 2 => _className, 3 => _baseClassList, 4 => _nodes, 5 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class EnumerationItemValueSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _openingBracket;
        internal readonly SyntaxList _values;
        internal readonly SyntaxToken _closingBracket;
        internal EnumerationItemValueSyntaxNode(SyntaxToken openingBracket, SyntaxList values, SyntaxToken closingBracket): base(TokenKind.EnumerationItemValue)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(values);
            _values = values;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal EnumerationItemValueSyntaxNode(SyntaxToken openingBracket, SyntaxList values, SyntaxToken closingBracket, TokenDiagnostic[] diagnostics): base(TokenKind.EnumerationItemValue, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(openingBracket);
            _openingBracket = openingBracket;
            this.AdjustWidth(values);
            _values = values;
            this.AdjustWidth(closingBracket);
            _closingBracket = closingBracket;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EnumerationItemValueSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EnumerationItemValueSyntaxNode(_openingBracket, _values, _closingBracket, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _openingBracket, 1 => _values, 2 => _closingBracket, _ => null
            }

            ;
        }
    }

    internal class EnumerationItemSyntaxNode : SyntaxNode
    {
        internal readonly IdentifierNameExpressionSyntaxNode _name;
        internal readonly EnumerationItemValueSyntaxNode? _values;
        internal readonly SyntaxList<SyntaxToken> _commas;
        internal EnumerationItemSyntaxNode(IdentifierNameExpressionSyntaxNode name, EnumerationItemValueSyntaxNode? values, SyntaxList<SyntaxToken> commas): base(TokenKind.EnumerationItem)
        {
            Slots = 3;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(values);
            _values = values;
            this.AdjustWidth(commas);
            _commas = commas;
        }

        internal EnumerationItemSyntaxNode(IdentifierNameExpressionSyntaxNode name, EnumerationItemValueSyntaxNode? values, SyntaxList<SyntaxToken> commas, TokenDiagnostic[] diagnostics): base(TokenKind.EnumerationItem, diagnostics)
        {
            Slots = 3;
            this.AdjustWidth(name);
            _name = name;
            this.AdjustWidth(values);
            _values = values;
            this.AdjustWidth(commas);
            _commas = commas;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EnumerationItemSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EnumerationItemSyntaxNode(_name, _values, _commas, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _name, 1 => _values, 2 => _commas, _ => null
            }

            ;
        }
    }

    internal class EnumerationListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _enumerationKeyword;
        internal readonly AttributeListSyntaxNode? _attributes;
        internal readonly SyntaxList<EnumerationItemSyntaxNode> _items;
        internal readonly SyntaxToken _endKeyword;
        internal EnumerationListSyntaxNode(SyntaxToken enumerationKeyword, AttributeListSyntaxNode? attributes, SyntaxList<EnumerationItemSyntaxNode> items, SyntaxToken endKeyword): base(TokenKind.EnumerationList)
        {
            Slots = 4;
            this.AdjustWidth(enumerationKeyword);
            _enumerationKeyword = enumerationKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(items);
            _items = items;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal EnumerationListSyntaxNode(SyntaxToken enumerationKeyword, AttributeListSyntaxNode? attributes, SyntaxList<EnumerationItemSyntaxNode> items, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.EnumerationList, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(enumerationKeyword);
            _enumerationKeyword = enumerationKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(items);
            _items = items;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EnumerationListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EnumerationListSyntaxNode(_enumerationKeyword, _attributes, _items, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _enumerationKeyword, 1 => _attributes, 2 => _items, 3 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class EventsListSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _eventsKeyword;
        internal readonly AttributeListSyntaxNode? _attributes;
        internal readonly SyntaxList _events;
        internal readonly SyntaxToken _endKeyword;
        internal EventsListSyntaxNode(SyntaxToken eventsKeyword, AttributeListSyntaxNode? attributes, SyntaxList events, SyntaxToken endKeyword): base(TokenKind.EventsList)
        {
            Slots = 4;
            this.AdjustWidth(eventsKeyword);
            _eventsKeyword = eventsKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(events);
            _events = events;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal EventsListSyntaxNode(SyntaxToken eventsKeyword, AttributeListSyntaxNode? attributes, SyntaxList events, SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.EventsList, diagnostics)
        {
            Slots = 4;
            this.AdjustWidth(eventsKeyword);
            _eventsKeyword = eventsKeyword;
            this.AdjustWidth(attributes);
            _attributes = attributes;
            this.AdjustWidth(events);
            _events = events;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EventsListSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EventsListSyntaxNode(_eventsKeyword, _attributes, _events, _endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _eventsKeyword, 1 => _attributes, 2 => _events, 3 => _endKeyword, _ => null
            }

            ;
        }
    }

    internal class EndKeywordSyntaxNode : SyntaxNode
    {
        internal readonly SyntaxToken _endKeyword;
        internal EndKeywordSyntaxNode(SyntaxToken endKeyword): base(TokenKind.EndKeyword)
        {
            Slots = 1;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal EndKeywordSyntaxNode(SyntaxToken endKeyword, TokenDiagnostic[] diagnostics): base(TokenKind.EndKeyword, diagnostics)
        {
            Slots = 1;
            this.AdjustWidth(endKeyword);
            _endKeyword = endKeyword;
        }

        internal override Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position)
        {
            return new Parser.EndKeywordSyntaxNode(parent, this, position);
        }

        public override GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics)
        {
            return new EndKeywordSyntaxNode(_endKeyword, diagnostics);
        }

        public override GreenNode? GetSlot(int i)
        {
            return i switch
            {
            0 => _endKeyword, _ => null
            }

            ;
        }
    }
}