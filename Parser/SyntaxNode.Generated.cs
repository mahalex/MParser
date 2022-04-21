#nullable enable
namespace Parser
{
    public class FileSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _body;
        internal FileSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken EndOfFile
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FileSyntaxNode)_green)._endOfFile, this.GetChildPosition(1));
            }
        }

        public BlockStatementSyntaxNode? Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 0);
                return red is null ? default : (BlockStatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _body, 0), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFile(this);
        }
    }

    public class BlockStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _statements;
        internal BlockStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxNodeOrTokenList Statements
        {
            get
            {
                var red = this.GetRed(ref this._statements!, 0);
                return red is null ? throw new System.Exception("statements cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _statements!, 0), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBlockStatement(this);
        }
    }

    public class FunctionDeclarationSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _outputDescription;
        private SyntaxNode? _inputDescription;
        private SyntaxNode? _commas;
        private SyntaxNode? _body;
        private SyntaxNode? _endKeyword;
        internal FunctionDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken FunctionKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionDeclarationSyntaxNode)_green)._functionKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken Name
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionDeclarationSyntaxNode)_green)._name, this.GetChildPosition(2));
            }
        }

        public FunctionOutputDescriptionSyntaxNode? OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 1);
                return red is null ? default : (FunctionOutputDescriptionSyntaxNode)red;
            }
        }

        public FunctionInputDescriptionSyntaxNode? InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 3);
                return red is null ? default : (FunctionInputDescriptionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas!, 4);
                return red is null ? throw new System.Exception("commas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 5);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        public EndKeywordSyntaxNode? EndKeyword
        {
            get
            {
                var red = this.GetRed(ref this._endKeyword, 6);
                return red is null ? default : (EndKeywordSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _outputDescription, 1), 3 => GetRed(ref _inputDescription, 3), 4 => GetRed(ref _commas!, 4), 5 => GetRed(ref _body!, 5), 6 => GetRed(ref _endKeyword, 6), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionDeclaration(this);
        }
    }

    public class FunctionOutputDescriptionSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _outputList;
        internal FunctionOutputDescriptionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionOutputDescriptionSyntaxNode)_green)._assignmentSign, this.GetChildPosition(1));
            }
        }

        public SyntaxNodeOrTokenList OutputList
        {
            get
            {
                var red = this.GetRed(ref this._outputList!, 0);
                return red is null ? throw new System.Exception("outputList cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _outputList!, 0), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionOutputDescription(this);
        }
    }

    public class FunctionInputDescriptionSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _parameterList;
        internal FunctionInputDescriptionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionInputDescriptionSyntaxNode)_green)._openingBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionInputDescriptionSyntaxNode)_green)._closingBracket, this.GetChildPosition(2));
            }
        }

        public SyntaxNodeOrTokenList ParameterList
        {
            get
            {
                var red = this.GetRed(ref this._parameterList!, 1);
                return red is null ? throw new System.Exception("parameterList cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _parameterList!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionInputDescription(this);
        }
    }

    public class SwitchStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _switchExpression;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _cases;
        internal SwitchStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken SwitchKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.SwitchStatementSyntaxNode)_green)._switchKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.SwitchStatementSyntaxNode)_green)._endKeyword, this.GetChildPosition(4));
            }
        }

        public ExpressionSyntaxNode SwitchExpression
        {
            get
            {
                var red = this.GetRed(ref this._switchExpression!, 1);
                return red is null ? throw new System.Exception("switchExpression cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public SyntaxNodeOrTokenList Cases
        {
            get
            {
                var red = this.GetRed(ref this._cases!, 3);
                return red is null ? throw new System.Exception("cases cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _switchExpression!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _cases!, 3), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSwitchStatement(this);
        }
    }

    public class SwitchCaseSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _caseIdentifier;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _body;
        internal SwitchCaseSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken CaseKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.SwitchCaseSyntaxNode)_green)._caseKeyword, this.GetChildPosition(0));
            }
        }

        public ExpressionSyntaxNode CaseIdentifier
        {
            get
            {
                var red = this.GetRed(ref this._caseIdentifier!, 1);
                return red is null ? throw new System.Exception("caseIdentifier cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 3);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _caseIdentifier!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _body!, 3), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSwitchCase(this);
        }
    }

    public class WhileStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _condition;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _body;
        internal WhileStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken WhileKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.WhileStatementSyntaxNode)_green)._whileKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.WhileStatementSyntaxNode)_green)._endKeyword, this.GetChildPosition(4));
            }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition!, 1);
                return red is null ? throw new System.Exception("condition cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 3);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _condition!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _body!, 3), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitWhileStatement(this);
        }
    }

    public class ElseifClause : SyntaxNode
    {
        private SyntaxNode? _condition;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _body;
        internal ElseifClause(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken ElseifKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ElseifClause)_green)._elseifKeyword, this.GetChildPosition(0));
            }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition!, 1);
                return red is null ? throw new System.Exception("condition cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 3);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _condition!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _body!, 3), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitElseifClause(this);
        }
    }

    public class ElseClause : SyntaxNode
    {
        private SyntaxNode? _body;
        internal ElseClause(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken ElseKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ElseClause)_green)._elseKeyword, this.GetChildPosition(0));
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 1);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _body!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitElseClause(this);
        }
    }

    public class IfStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _condition;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _body;
        private SyntaxNode? _elseifClauses;
        private SyntaxNode? _elseClause;
        internal IfStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken IfKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.IfStatementSyntaxNode)_green)._ifKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.IfStatementSyntaxNode)_green)._endKeyword, this.GetChildPosition(6));
            }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition!, 1);
                return red is null ? throw new System.Exception("condition cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 3);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList ElseifClauses
        {
            get
            {
                var red = this.GetRed(ref this._elseifClauses!, 4);
                return red is null ? throw new System.Exception("elseifClauses cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public ElseClause? ElseClause
        {
            get
            {
                var red = this.GetRed(ref this._elseClause, 5);
                return red is null ? default : (ElseClause)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _condition!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _body!, 3), 4 => GetRed(ref _elseifClauses!, 4), 5 => GetRed(ref _elseClause, 5), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIfStatement(this);
        }
    }

    public class ForStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _assignment;
        private SyntaxNode? _optionalCommas;
        private SyntaxNode? _body;
        internal ForStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken ForKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ForStatementSyntaxNode)_green)._forKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ForStatementSyntaxNode)_green)._endKeyword, this.GetChildPosition(4));
            }
        }

        public AssignmentExpressionSyntaxNode Assignment
        {
            get
            {
                var red = this.GetRed(ref this._assignment!, 1);
                return red is null ? throw new System.Exception("assignment cannot be null.") : (AssignmentExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas!, 2);
                return red is null ? throw new System.Exception("optionalCommas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 3);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _assignment!, 1), 2 => GetRed(ref _optionalCommas!, 2), 3 => GetRed(ref _body!, 3), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitForStatement(this);
        }
    }

    public class AssignmentExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _lhs;
        private SyntaxNode? _rhs;
        internal AssignmentExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.AssignmentExpressionSyntaxNode)_green)._assignmentSign, this.GetChildPosition(1));
            }
        }

        public ExpressionSyntaxNode Lhs
        {
            get
            {
                var red = this.GetRed(ref this._lhs!, 0);
                return red is null ? throw new System.Exception("lhs cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public ExpressionSyntaxNode Rhs
        {
            get
            {
                var red = this.GetRed(ref this._rhs!, 2);
                return red is null ? throw new System.Exception("rhs cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _lhs!, 0), 2 => GetRed(ref _rhs!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAssignmentExpression(this);
        }
    }

    public class CatchClauseSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _catchBody;
        internal CatchClauseSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken CatchKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CatchClauseSyntaxNode)_green)._catchKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxNodeOrTokenList CatchBody
        {
            get
            {
                var red = this.GetRed(ref this._catchBody!, 1);
                return red is null ? throw new System.Exception("catchBody cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _catchBody!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCatchClause(this);
        }
    }

    public class TryCatchStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _tryBody;
        private SyntaxNode? _catchClause;
        internal TryCatchStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken TryKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.TryCatchStatementSyntaxNode)_green)._tryKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.TryCatchStatementSyntaxNode)_green)._endKeyword, this.GetChildPosition(3));
            }
        }

        public StatementSyntaxNode TryBody
        {
            get
            {
                var red = this.GetRed(ref this._tryBody!, 1);
                return red is null ? throw new System.Exception("tryBody cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        public CatchClauseSyntaxNode? CatchClause
        {
            get
            {
                var red = this.GetRed(ref this._catchClause, 2);
                return red is null ? default : (CatchClauseSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _tryBody!, 1), 2 => GetRed(ref _catchClause, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitTryCatchStatement(this);
        }
    }

    public class ExpressionStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _expression;
        private SyntaxNode? _semicolon;
        internal ExpressionStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression!, 0);
                return red is null ? throw new System.Exception("expression cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public TrailingSemicolonSyntaxNode? Semicolon
        {
            get
            {
                var red = this.GetRed(ref this._semicolon, 1);
                return red is null ? default : (TrailingSemicolonSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _expression!, 0), 1 => GetRed(ref _semicolon, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }
    }

    public class TrailingSemicolonSyntaxNode : SyntaxNode
    {
        internal TrailingSemicolonSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Semicolon
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.TrailingSemicolonSyntaxNode)_green)._semicolon, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitTrailingSemicolon(this);
        }
    }

    public class EmptyStatementSyntaxNode : StatementSyntaxNode
    {
        internal EmptyStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Semicolon
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EmptyStatementSyntaxNode)_green)._semicolon, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEmptyStatement(this);
        }
    }

    public class EmptyExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal EmptyExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEmptyExpression(this);
        }
    }

    public class UnaryPrefixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _operand;
        internal UnaryPrefixOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Operation
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.UnaryPrefixOperationExpressionSyntaxNode)_green)._operation, this.GetChildPosition(0));
            }
        }

        public ExpressionSyntaxNode Operand
        {
            get
            {
                var red = this.GetRed(ref this._operand!, 1);
                return red is null ? throw new System.Exception("operand cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _operand!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnaryPrefixOperationExpression(this);
        }
    }

    public class CompoundNameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _nodes;
        internal CompoundNameExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 0);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _nodes!, 0), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCompoundNameExpression(this);
        }
    }

    public class NamedFunctionHandleExpressionSyntaxNode : FunctionHandleExpressionSyntaxNode
    {
        private SyntaxNode? _functionName;
        internal NamedFunctionHandleExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AtSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.NamedFunctionHandleExpressionSyntaxNode)_green)._atSign, this.GetChildPosition(0));
            }
        }

        public CompoundNameExpressionSyntaxNode FunctionName
        {
            get
            {
                var red = this.GetRed(ref this._functionName!, 1);
                return red is null ? throw new System.Exception("functionName cannot be null.") : (CompoundNameExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _functionName!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitNamedFunctionHandleExpression(this);
        }
    }

    public class LambdaExpressionSyntaxNode : FunctionHandleExpressionSyntaxNode
    {
        private SyntaxNode? _input;
        private SyntaxNode? _body;
        internal LambdaExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AtSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.LambdaExpressionSyntaxNode)_green)._atSign, this.GetChildPosition(0));
            }
        }

        public FunctionInputDescriptionSyntaxNode Input
        {
            get
            {
                var red = this.GetRed(ref this._input!, 1);
                return red is null ? throw new System.Exception("input cannot be null.") : (FunctionInputDescriptionSyntaxNode)red;
            }
        }

        public ExpressionSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 2);
                return red is null ? throw new System.Exception("body cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _input!, 1), 2 => GetRed(ref _body!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitLambdaExpression(this);
        }
    }

    public class BinaryOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _lhs;
        private SyntaxNode? _rhs;
        internal BinaryOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Operation
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.BinaryOperationExpressionSyntaxNode)_green)._operation, this.GetChildPosition(1));
            }
        }

        public ExpressionSyntaxNode Lhs
        {
            get
            {
                var red = this.GetRed(ref this._lhs!, 0);
                return red is null ? throw new System.Exception("lhs cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public ExpressionSyntaxNode Rhs
        {
            get
            {
                var red = this.GetRed(ref this._rhs!, 2);
                return red is null ? throw new System.Exception("rhs cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _lhs!, 0), 2 => GetRed(ref _rhs!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBinaryOperationExpression(this);
        }
    }

    public class IdentifierNameExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal IdentifierNameExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Name
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.IdentifierNameExpressionSyntaxNode)_green)._name, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIdentifierNameExpression(this);
        }
    }

    public class NumberLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal NumberLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Number
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.NumberLiteralExpressionSyntaxNode)_green)._number, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitNumberLiteralExpression(this);
        }
    }

    public class StringLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal StringLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken StringToken
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.StringLiteralExpressionSyntaxNode)_green)._stringToken, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitStringLiteralExpression(this);
        }
    }

    public class DoubleQuotedStringLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal DoubleQuotedStringLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken StringToken
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.DoubleQuotedStringLiteralExpressionSyntaxNode)_green)._stringToken, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitDoubleQuotedStringLiteralExpression(this);
        }
    }

    public class UnquotedStringLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        internal UnquotedStringLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken StringToken
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.UnquotedStringLiteralExpressionSyntaxNode)_green)._stringToken, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnquotedStringLiteralExpression(this);
        }
    }

    public class ArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _nodes;
        internal ArrayLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningSquareBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ArrayLiteralExpressionSyntaxNode)_green)._openingSquareBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingSquareBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ArrayLiteralExpressionSyntaxNode)_green)._closingSquareBracket, this.GetChildPosition(2));
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 1);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _nodes!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitArrayLiteralExpression(this);
        }
    }

    public class CellArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _nodes;
        internal CellArrayLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBrace
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CellArrayLiteralExpressionSyntaxNode)_green)._openingBrace, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBrace
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CellArrayLiteralExpressionSyntaxNode)_green)._closingBrace, this.GetChildPosition(2));
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 1);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _nodes!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCellArrayLiteralExpression(this);
        }
    }

    public class ParenthesizedExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _expression;
        internal ParenthesizedExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ParenthesizedExpressionSyntaxNode)_green)._openingBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ParenthesizedExpressionSyntaxNode)_green)._closingBracket, this.GetChildPosition(2));
            }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression!, 1);
                return red is null ? throw new System.Exception("expression cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _expression!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitParenthesizedExpression(this);
        }
    }

    public class CellArrayElementAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _expression;
        private SyntaxNode? _nodes;
        internal CellArrayElementAccessExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBrace
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CellArrayElementAccessExpressionSyntaxNode)_green)._openingBrace, this.GetChildPosition(1));
            }
        }

        public SyntaxToken ClosingBrace
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CellArrayElementAccessExpressionSyntaxNode)_green)._closingBrace, this.GetChildPosition(3));
            }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression!, 0);
                return red is null ? throw new System.Exception("expression cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 2);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _expression!, 0), 2 => GetRed(ref _nodes!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCellArrayElementAccessExpression(this);
        }
    }

    public class FunctionCallExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _functionName;
        private SyntaxNode? _nodes;
        internal FunctionCallExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionCallExpressionSyntaxNode)_green)._openingBracket, this.GetChildPosition(1));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.FunctionCallExpressionSyntaxNode)_green)._closingBracket, this.GetChildPosition(3));
            }
        }

        public ExpressionSyntaxNode FunctionName
        {
            get
            {
                var red = this.GetRed(ref this._functionName!, 0);
                return red is null ? throw new System.Exception("functionName cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 2);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _functionName!, 0), 2 => GetRed(ref _nodes!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionCallExpression(this);
        }
    }

    public class MemberAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _leftOperand;
        private SyntaxNode? _rightOperand;
        internal MemberAccessExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Dot
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.MemberAccessExpressionSyntaxNode)_green)._dot, this.GetChildPosition(1));
            }
        }

        public SyntaxNode LeftOperand
        {
            get
            {
                var red = this.GetRed(ref this._leftOperand!, 0);
                return red is null ? throw new System.Exception("leftOperand cannot be null.") : (SyntaxNode)red;
            }
        }

        public SyntaxNode RightOperand
        {
            get
            {
                var red = this.GetRed(ref this._rightOperand!, 2);
                return red is null ? throw new System.Exception("rightOperand cannot be null.") : (SyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _leftOperand!, 0), 2 => GetRed(ref _rightOperand!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMemberAccessExpression(this);
        }
    }

    public class UnaryPostfixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _operand;
        internal UnaryPostfixOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Operation
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.UnaryPostfixOperationExpressionSyntaxNode)_green)._operation, this.GetChildPosition(1));
            }
        }

        public ExpressionSyntaxNode Operand
        {
            get
            {
                var red = this.GetRed(ref this._operand!, 0);
                return red is null ? throw new System.Exception("operand cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _operand!, 0), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnaryPostfixOperationExpression(this);
        }
    }

    public class IndirectMemberAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _expression;
        internal IndirectMemberAccessExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.IndirectMemberAccessExpressionSyntaxNode)_green)._openingBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.IndirectMemberAccessExpressionSyntaxNode)_green)._closingBracket, this.GetChildPosition(2));
            }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression!, 1);
                return red is null ? throw new System.Exception("expression cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _expression!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIndirectMemberAccessExpression(this);
        }
    }

    public class CommandExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _arguments;
        internal CommandExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken CommandName
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.CommandExpressionSyntaxNode)_green)._commandName, this.GetChildPosition(0));
            }
        }

        public SyntaxNodeOrTokenList Arguments
        {
            get
            {
                var red = this.GetRed(ref this._arguments!, 1);
                return red is null ? throw new System.Exception("arguments cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _arguments!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCommandExpression(this);
        }
    }

    public class ClassInvokationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode? _methodName;
        private SyntaxNode? _baseClassNameAndArguments;
        internal ClassInvokationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AtSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ClassInvokationExpressionSyntaxNode)_green)._atSign, this.GetChildPosition(1));
            }
        }

        public ExpressionSyntaxNode MethodName
        {
            get
            {
                var red = this.GetRed(ref this._methodName!, 0);
                return red is null ? throw new System.Exception("methodName cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        public ExpressionSyntaxNode BaseClassNameAndArguments
        {
            get
            {
                var red = this.GetRed(ref this._baseClassNameAndArguments!, 2);
                return red is null ? throw new System.Exception("baseClassNameAndArguments cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _methodName!, 0), 2 => GetRed(ref _baseClassNameAndArguments!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitClassInvokationExpression(this);
        }
    }

    public class AttributeAssignmentSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _value;
        internal AttributeAssignmentSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.AttributeAssignmentSyntaxNode)_green)._assignmentSign, this.GetChildPosition(0));
            }
        }

        public ExpressionSyntaxNode Value
        {
            get
            {
                var red = this.GetRed(ref this._value!, 1);
                return red is null ? throw new System.Exception("value cannot be null.") : (ExpressionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _value!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttributeAssignment(this);
        }
    }

    public class AttributeSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _assignment;
        internal AttributeSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Name
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.AttributeSyntaxNode)_green)._name, this.GetChildPosition(0));
            }
        }

        public AttributeAssignmentSyntaxNode? Assignment
        {
            get
            {
                var red = this.GetRed(ref this._assignment, 1);
                return red is null ? default : (AttributeAssignmentSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _assignment, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttribute(this);
        }
    }

    public class AttributeListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _nodes;
        internal AttributeListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.AttributeListSyntaxNode)_green)._openingBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.AttributeListSyntaxNode)_green)._closingBracket, this.GetChildPosition(2));
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 1);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _nodes!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttributeList(this);
        }
    }

    public class ConcreteMethodDeclarationSyntaxNode : MethodDeclarationSyntaxNode
    {
        private SyntaxNode? _outputDescription;
        private SyntaxNode? _name;
        private SyntaxNode? _inputDescription;
        private SyntaxNode? _commas;
        private SyntaxNode? _body;
        private SyntaxNode? _endKeyword;
        internal ConcreteMethodDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken FunctionKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ConcreteMethodDeclarationSyntaxNode)_green)._functionKeyword, this.GetChildPosition(0));
            }
        }

        public FunctionOutputDescriptionSyntaxNode? OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 1);
                return red is null ? default : (FunctionOutputDescriptionSyntaxNode)red;
            }
        }

        public CompoundNameExpressionSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name!, 2);
                return red is null ? throw new System.Exception("name cannot be null.") : (CompoundNameExpressionSyntaxNode)red;
            }
        }

        public FunctionInputDescriptionSyntaxNode? InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 3);
                return red is null ? default : (FunctionInputDescriptionSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas!, 4);
                return red is null ? throw new System.Exception("commas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        public StatementSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body!, 5);
                return red is null ? throw new System.Exception("body cannot be null.") : (StatementSyntaxNode)red;
            }
        }

        public EndKeywordSyntaxNode? EndKeyword
        {
            get
            {
                var red = this.GetRed(ref this._endKeyword, 6);
                return red is null ? default : (EndKeywordSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _outputDescription, 1), 2 => GetRed(ref _name!, 2), 3 => GetRed(ref _inputDescription, 3), 4 => GetRed(ref _commas!, 4), 5 => GetRed(ref _body!, 5), 6 => GetRed(ref _endKeyword, 6), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitConcreteMethodDeclaration(this);
        }
    }

    public class AbstractMethodDeclarationSyntaxNode : MethodDeclarationSyntaxNode
    {
        private SyntaxNode? _outputDescription;
        private SyntaxNode? _name;
        private SyntaxNode? _inputDescription;
        internal AbstractMethodDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public FunctionOutputDescriptionSyntaxNode? OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 0);
                return red is null ? default : (FunctionOutputDescriptionSyntaxNode)red;
            }
        }

        public CompoundNameExpressionSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name!, 1);
                return red is null ? throw new System.Exception("name cannot be null.") : (CompoundNameExpressionSyntaxNode)red;
            }
        }

        public FunctionInputDescriptionSyntaxNode? InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 2);
                return red is null ? default : (FunctionInputDescriptionSyntaxNode)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            0 => GetRed(ref _outputDescription, 0), 1 => GetRed(ref _name!, 1), 2 => GetRed(ref _inputDescription, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAbstractMethodDeclaration(this);
        }
    }

    public class MethodsListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _attributes;
        private SyntaxNode? _methods;
        internal MethodsListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken MethodsKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.MethodsListSyntaxNode)_green)._methodsKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.MethodsListSyntaxNode)_green)._endKeyword, this.GetChildPosition(3));
            }
        }

        public AttributeListSyntaxNode? Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                return red is null ? default : (AttributeListSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Methods
        {
            get
            {
                var red = this.GetRed(ref this._methods!, 2);
                return red is null ? throw new System.Exception("methods cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _attributes, 1), 2 => GetRed(ref _methods!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMethodsList(this);
        }
    }

    public class PropertiesListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _attributes;
        private SyntaxNode? _properties;
        internal PropertiesListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken PropertiesKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.PropertiesListSyntaxNode)_green)._propertiesKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.PropertiesListSyntaxNode)_green)._endKeyword, this.GetChildPosition(3));
            }
        }

        public AttributeListSyntaxNode? Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                return red is null ? default : (AttributeListSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Properties
        {
            get
            {
                var red = this.GetRed(ref this._properties!, 2);
                return red is null ? throw new System.Exception("properties cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _attributes, 1), 2 => GetRed(ref _properties!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitPropertiesList(this);
        }
    }

    public class BaseClassListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _baseClasses;
        internal BaseClassListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken LessSign
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.BaseClassListSyntaxNode)_green)._lessSign, this.GetChildPosition(0));
            }
        }

        public SyntaxNodeOrTokenList BaseClasses
        {
            get
            {
                var red = this.GetRed(ref this._baseClasses!, 1);
                return red is null ? throw new System.Exception("baseClasses cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _baseClasses!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBaseClassList(this);
        }
    }

    public class ClassDeclarationSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode? _attributes;
        private SyntaxNode? _baseClassList;
        private SyntaxNode? _nodes;
        internal ClassDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken ClassdefKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ClassDeclarationSyntaxNode)_green)._classdefKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClassName
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ClassDeclarationSyntaxNode)_green)._className, this.GetChildPosition(2));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.ClassDeclarationSyntaxNode)_green)._endKeyword, this.GetChildPosition(5));
            }
        }

        public AttributeListSyntaxNode? Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                return red is null ? default : (AttributeListSyntaxNode)red;
            }
        }

        public BaseClassListSyntaxNode? BaseClassList
        {
            get
            {
                var red = this.GetRed(ref this._baseClassList, 3);
                return red is null ? default : (BaseClassListSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes!, 4);
                return red is null ? throw new System.Exception("nodes cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _attributes, 1), 3 => GetRed(ref _baseClassList, 3), 4 => GetRed(ref _nodes!, 4), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitClassDeclaration(this);
        }
    }

    public class EnumerationItemValueSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _values;
        internal EnumerationItemValueSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EnumerationItemValueSyntaxNode)_green)._openingBracket, this.GetChildPosition(0));
            }
        }

        public SyntaxToken ClosingBracket
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EnumerationItemValueSyntaxNode)_green)._closingBracket, this.GetChildPosition(2));
            }
        }

        public SyntaxNodeOrTokenList Values
        {
            get
            {
                var red = this.GetRed(ref this._values!, 1);
                return red is null ? throw new System.Exception("values cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _values!, 1), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationItemValue(this);
        }
    }

    public class EnumerationItemSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _values;
        private SyntaxNode? _commas;
        internal EnumerationItemSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken Name
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EnumerationItemSyntaxNode)_green)._name, this.GetChildPosition(0));
            }
        }

        public EnumerationItemValueSyntaxNode? Values
        {
            get
            {
                var red = this.GetRed(ref this._values, 1);
                return red is null ? default : (EnumerationItemValueSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas!, 2);
                return red is null ? throw new System.Exception("commas cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _values, 1), 2 => GetRed(ref _commas!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationItem(this);
        }
    }

    public class EnumerationListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _attributes;
        private SyntaxNode? _items;
        internal EnumerationListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken EnumerationKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EnumerationListSyntaxNode)_green)._enumerationKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EnumerationListSyntaxNode)_green)._endKeyword, this.GetChildPosition(3));
            }
        }

        public AttributeListSyntaxNode? Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                return red is null ? default : (AttributeListSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Items
        {
            get
            {
                var red = this.GetRed(ref this._items!, 2);
                return red is null ? throw new System.Exception("items cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _attributes, 1), 2 => GetRed(ref _items!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationList(this);
        }
    }

    public class EventsListSyntaxNode : SyntaxNode
    {
        private SyntaxNode? _attributes;
        private SyntaxNode? _events;
        internal EventsListSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken EventsKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EventsListSyntaxNode)_green)._eventsKeyword, this.GetChildPosition(0));
            }
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EventsListSyntaxNode)_green)._endKeyword, this.GetChildPosition(3));
            }
        }

        public AttributeListSyntaxNode? Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                return red is null ? default : (AttributeListSyntaxNode)red;
            }
        }

        public SyntaxNodeOrTokenList Events
        {
            get
            {
                var red = this.GetRed(ref this._events!, 2);
                return red is null ? throw new System.Exception("events cannot be null.") : (SyntaxNodeOrTokenList)red;
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            1 => GetRed(ref _attributes, 1), 2 => GetRed(ref _events!, 2), _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEventsList(this);
        }
    }

    public class EndKeywordSyntaxNode : SyntaxNode
    {
        internal EndKeywordSyntaxNode(SyntaxNode parent, Internal.GreenNode green, int position): base(parent, green, position)
        {
        }

        public SyntaxToken EndKeyword
        {
            get
            {
                return new SyntaxToken(this, ((Parser.Internal.EndKeywordSyntaxNode)_green)._endKeyword, this.GetChildPosition(0));
            }
        }

        internal override SyntaxNode? GetNode(int i)
        {
            return i switch
            {
            _ => null
            }

            ;
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEndKeyword(this);
        }
    }
}