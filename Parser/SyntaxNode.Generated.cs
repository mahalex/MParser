namespace Parser
{
    public class FileSyntaxNode : SyntaxNode
    {
        private SyntaxNode _statementList;

        internal FileSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken EndOfFile
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FileSyntaxNode)_green)._endOfFile); }
        }

        public SyntaxNodeOrTokenList StatementList
        {
            get
            {
                var red = this.GetRed(ref this._statementList, 0);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _statementList, 0);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFile(this);
        }

    }

    public class FunctionDeclarationSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _outputDescription;
        private SyntaxNode _inputDescription;
        private SyntaxNode _commas;
        private SyntaxNode _body;

        internal FunctionDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken FunctionKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionDeclarationSyntaxNode)_green)._functionKeyword); }
        }

        public SyntaxToken Name
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionDeclarationSyntaxNode)_green)._name); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionDeclarationSyntaxNode)_green)._endKeyword); }
        }

        public FunctionOutputDescriptionSyntaxNode OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 1);
                if (red != null)
                    return (FunctionOutputDescriptionSyntaxNode)red;

                return default(FunctionOutputDescriptionSyntaxNode);
            }
        }

        public FunctionInputDescriptionSyntaxNode InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 3);
                if (red != null)
                    return (FunctionInputDescriptionSyntaxNode)red;

                return default(FunctionInputDescriptionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas, 4);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 5);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _outputDescription, 1);
                case 3: return GetRed(ref _inputDescription, 3);
                case 4: return GetRed(ref _commas, 4);
                case 5: return GetRed(ref _body, 5);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionDeclaration(this);
        }

    }

    public class FunctionOutputDescriptionSyntaxNode : SyntaxNode
    {
        private SyntaxNode _outputList;

        internal FunctionOutputDescriptionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionOutputDescriptionSyntaxNode)_green)._assignmentSign); }
        }

        public SyntaxNodeOrTokenList OutputList
        {
            get
            {
                var red = this.GetRed(ref this._outputList, 0);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _outputList, 0);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionOutputDescription(this);
        }

    }

    public class FunctionInputDescriptionSyntaxNode : SyntaxNode
    {
        private SyntaxNode _parameterList;

        internal FunctionInputDescriptionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionInputDescriptionSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionInputDescriptionSyntaxNode)_green)._closingBracket); }
        }

        public SyntaxNodeOrTokenList ParameterList
        {
            get
            {
                var red = this.GetRed(ref this._parameterList, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _parameterList, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionInputDescription(this);
        }

    }

    public class SwitchStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _switchExpression;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _cases;

        internal SwitchStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken SwitchKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.SwitchStatementSyntaxNode)_green)._switchKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.SwitchStatementSyntaxNode)_green)._endKeyword); }
        }

        public ExpressionSyntaxNode SwitchExpression
        {
            get
            {
                var red = this.GetRed(ref this._switchExpression, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Cases
        {
            get
            {
                var red = this.GetRed(ref this._cases, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _switchExpression, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _cases, 3);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSwitchStatement(this);
        }

    }

    public class SwitchCaseSyntaxNode : SyntaxNode
    {
        private SyntaxNode _caseIdentifier;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _body;

        internal SwitchCaseSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken CaseKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.SwitchCaseSyntaxNode)_green)._caseKeyword); }
        }

        public ExpressionSyntaxNode CaseIdentifier
        {
            get
            {
                var red = this.GetRed(ref this._caseIdentifier, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _caseIdentifier, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _body, 3);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitSwitchCase(this);
        }

    }

    public class WhileStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _condition;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _body;

        internal WhileStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken WhileKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.WhileStatementSyntaxNode)_green)._whileKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.WhileStatementSyntaxNode)_green)._endKeyword); }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _condition, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _body, 3);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitWhileStatement(this);
        }

    }

    public class ElseifClause : SyntaxNode
    {
        private SyntaxNode _condition;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _body;

        internal ElseifClause(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken ElseifKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ElseifClause)_green)._elseifKeyword); }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _condition, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _body, 3);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitElseifClause(this);
        }

    }

    public class ElseClause : SyntaxNode
    {
        private SyntaxNode _body;

        internal ElseClause(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken ElseKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ElseClause)_green)._elseKeyword); }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _body, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitElseClause(this);
        }

    }

    public class IfStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _condition;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _body;
        private SyntaxNode _elseifClauses;
        private SyntaxNode _elseClause;

        internal IfStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken IfKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.IfStatementSyntaxNode)_green)._ifKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.IfStatementSyntaxNode)_green)._endKeyword); }
        }

        public ExpressionSyntaxNode Condition
        {
            get
            {
                var red = this.GetRed(ref this._condition, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList ElseifClauses
        {
            get
            {
                var red = this.GetRed(ref this._elseifClauses, 4);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public ElseClause ElseClause
        {
            get
            {
                var red = this.GetRed(ref this._elseClause, 5);
                if (red != null)
                    return (ElseClause)red;

                return default(ElseClause);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _condition, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _body, 3);
                case 4: return GetRed(ref _elseifClauses, 4);
                case 5: return GetRed(ref _elseClause, 5);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIfStatement(this);
        }

    }

    public class ForStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _assignment;
        private SyntaxNode _optionalCommas;
        private SyntaxNode _body;

        internal ForStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken ForKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ForStatementSyntaxNode)_green)._forKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ForStatementSyntaxNode)_green)._endKeyword); }
        }

        public AssignmentExpressionSyntaxNode Assignment
        {
            get
            {
                var red = this.GetRed(ref this._assignment, 1);
                if (red != null)
                    return (AssignmentExpressionSyntaxNode)red;

                return default(AssignmentExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList OptionalCommas
        {
            get
            {
                var red = this.GetRed(ref this._optionalCommas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 3);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _assignment, 1);
                case 2: return GetRed(ref _optionalCommas, 2);
                case 3: return GetRed(ref _body, 3);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitForStatement(this);
        }

    }

    public class AssignmentExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _lhs;
        private SyntaxNode _rhs;

        internal AssignmentExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.AssignmentExpressionSyntaxNode)_green)._assignmentSign); }
        }

        public ExpressionSyntaxNode Lhs
        {
            get
            {
                var red = this.GetRed(ref this._lhs, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public ExpressionSyntaxNode Rhs
        {
            get
            {
                var red = this.GetRed(ref this._rhs, 2);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _lhs, 0);
                case 2: return GetRed(ref _rhs, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAssignmentExpression(this);
        }

    }

    public class CatchClauseSyntaxNode : SyntaxNode
    {
        private SyntaxNode _catchBody;

        internal CatchClauseSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken CatchKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.CatchClauseSyntaxNode)_green)._catchKeyword); }
        }

        public SyntaxNodeOrTokenList CatchBody
        {
            get
            {
                var red = this.GetRed(ref this._catchBody, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _catchBody, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCatchClause(this);
        }

    }

    public class TryCatchStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _tryBody;
        private SyntaxNode _catchClause;

        internal TryCatchStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken TryKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.TryCatchStatementSyntaxNode)_green)._tryKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.TryCatchStatementSyntaxNode)_green)._endKeyword); }
        }

        public SyntaxNodeOrTokenList TryBody
        {
            get
            {
                var red = this.GetRed(ref this._tryBody, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public CatchClauseSyntaxNode CatchClause
        {
            get
            {
                var red = this.GetRed(ref this._catchClause, 2);
                if (red != null)
                    return (CatchClauseSyntaxNode)red;

                return default(CatchClauseSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _tryBody, 1);
                case 2: return GetRed(ref _catchClause, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitTryCatchStatement(this);
        }

    }

    public class ExpressionStatementSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _expression;

        internal ExpressionStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _expression, 0);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitExpressionStatement(this);
        }

    }

    public class EmptyStatementSyntaxNode : StatementSyntaxNode
    {

        internal EmptyStatementSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Semicolon
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EmptyStatementSyntaxNode)_green)._semicolon); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEmptyStatement(this);
        }

    }

    public class EmptyExpressionSyntaxNode : ExpressionSyntaxNode
    {

        internal EmptyExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }



        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEmptyExpression(this);
        }

    }

    public class UnaryPrefixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _operand;

        internal UnaryPrefixOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Operation
        {
            get { return new SyntaxToken(this, ((Parser.Internal.UnaryPrefixOperationExpressionSyntaxNode)_green)._operation); }
        }

        public ExpressionSyntaxNode Operand
        {
            get
            {
                var red = this.GetRed(ref this._operand, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _operand, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnaryPrefixOperationExpression(this);
        }

    }

    public class CompoundNameSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _nodes;

        internal CompoundNameSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 0);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _nodes, 0);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCompoundName(this);
        }

    }

    public class NamedFunctionHandleSyntaxNode : FunctionHandleSyntaxNode
    {
        private SyntaxNode _functionName;

        internal NamedFunctionHandleSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AtSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.NamedFunctionHandleSyntaxNode)_green)._atSign); }
        }

        public CompoundNameSyntaxNode FunctionName
        {
            get
            {
                var red = this.GetRed(ref this._functionName, 1);
                if (red != null)
                    return (CompoundNameSyntaxNode)red;

                return default(CompoundNameSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _functionName, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitNamedFunctionHandle(this);
        }

    }

    public class LambdaSyntaxNode : FunctionHandleSyntaxNode
    {
        private SyntaxNode _input;
        private SyntaxNode _body;

        internal LambdaSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AtSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.LambdaSyntaxNode)_green)._atSign); }
        }

        public FunctionInputDescriptionSyntaxNode Input
        {
            get
            {
                var red = this.GetRed(ref this._input, 1);
                if (red != null)
                    return (FunctionInputDescriptionSyntaxNode)red;

                return default(FunctionInputDescriptionSyntaxNode);
            }
        }

        public ExpressionSyntaxNode Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 2);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _input, 1);
                case 2: return GetRed(ref _body, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitLambda(this);
        }

    }

    public class BinaryOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _lhs;
        private SyntaxNode _rhs;

        internal BinaryOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Operation
        {
            get { return new SyntaxToken(this, ((Parser.Internal.BinaryOperationExpressionSyntaxNode)_green)._operation); }
        }

        public ExpressionSyntaxNode Lhs
        {
            get
            {
                var red = this.GetRed(ref this._lhs, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public ExpressionSyntaxNode Rhs
        {
            get
            {
                var red = this.GetRed(ref this._rhs, 2);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _lhs, 0);
                case 2: return GetRed(ref _rhs, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBinaryOperationExpression(this);
        }

    }

    public class IdentifierNameSyntaxNode : ExpressionSyntaxNode
    {

        internal IdentifierNameSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Name
        {
            get { return new SyntaxToken(this, ((Parser.Internal.IdentifierNameSyntaxNode)_green)._name); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIdentifierName(this);
        }

    }

    public class NumberLiteralSyntaxNode : ExpressionSyntaxNode
    {

        internal NumberLiteralSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Number
        {
            get { return new SyntaxToken(this, ((Parser.Internal.NumberLiteralSyntaxNode)_green)._number); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitNumberLiteral(this);
        }

    }

    public class StringLiteralSyntaxNode : ExpressionSyntaxNode
    {

        internal StringLiteralSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken StringToken
        {
            get { return new SyntaxToken(this, ((Parser.Internal.StringLiteralSyntaxNode)_green)._stringToken); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitStringLiteral(this);
        }

    }

    public class DoubleQuotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {

        internal DoubleQuotedStringLiteralSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken StringToken
        {
            get { return new SyntaxToken(this, ((Parser.Internal.DoubleQuotedStringLiteralSyntaxNode)_green)._stringToken); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitDoubleQuotedStringLiteral(this);
        }

    }

    public class UnquotedStringLiteralSyntaxNode : ExpressionSyntaxNode
    {

        internal UnquotedStringLiteralSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken StringToken
        {
            get { return new SyntaxToken(this, ((Parser.Internal.UnquotedStringLiteralSyntaxNode)_green)._stringToken); }
        }


        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnquotedStringLiteral(this);
        }

    }

    public class ArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _nodes;

        internal ArrayLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningSquareBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ArrayLiteralExpressionSyntaxNode)_green)._openingSquareBracket); }
        }

        public SyntaxToken ClosingSquareBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ArrayLiteralExpressionSyntaxNode)_green)._closingSquareBracket); }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _nodes, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitArrayLiteralExpression(this);
        }

    }

    public class CellArrayLiteralExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _nodes;

        internal CellArrayLiteralExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBrace
        {
            get { return new SyntaxToken(this, ((Parser.Internal.CellArrayLiteralExpressionSyntaxNode)_green)._openingBrace); }
        }

        public SyntaxToken ClosingBrace
        {
            get { return new SyntaxToken(this, ((Parser.Internal.CellArrayLiteralExpressionSyntaxNode)_green)._closingBrace); }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _nodes, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCellArrayLiteralExpression(this);
        }

    }

    public class ParenthesizedExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _expression;

        internal ParenthesizedExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ParenthesizedExpressionSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ParenthesizedExpressionSyntaxNode)_green)._closingBracket); }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _expression, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitParenthesizedExpression(this);
        }

    }

    public class CellArrayElementAccessExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _expression;
        private SyntaxNode _nodes;

        internal CellArrayElementAccessExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBrace
        {
            get { return new SyntaxToken(this, ((Parser.Internal.CellArrayElementAccessExpressionSyntaxNode)_green)._openingBrace); }
        }

        public SyntaxToken ClosingBrace
        {
            get { return new SyntaxToken(this, ((Parser.Internal.CellArrayElementAccessExpressionSyntaxNode)_green)._closingBrace); }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _expression, 0);
                case 2: return GetRed(ref _nodes, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCellArrayElementAccessExpression(this);
        }

    }

    public class FunctionCallExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _functionName;
        private SyntaxNode _nodes;

        internal FunctionCallExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionCallExpressionSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.FunctionCallExpressionSyntaxNode)_green)._closingBracket); }
        }

        public ExpressionSyntaxNode FunctionName
        {
            get
            {
                var red = this.GetRed(ref this._functionName, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _functionName, 0);
                case 2: return GetRed(ref _nodes, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitFunctionCallExpression(this);
        }

    }

    public class MemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _leftOperand;
        private SyntaxNode _rightOperand;

        internal MemberAccessSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Dot
        {
            get { return new SyntaxToken(this, ((Parser.Internal.MemberAccessSyntaxNode)_green)._dot); }
        }

        public SyntaxNode LeftOperand
        {
            get
            {
                var red = this.GetRed(ref this._leftOperand, 0);
                if (red != null)
                    return (SyntaxNode)red;

                return default(SyntaxNode);
            }
        }

        public SyntaxNode RightOperand
        {
            get
            {
                var red = this.GetRed(ref this._rightOperand, 2);
                if (red != null)
                    return (SyntaxNode)red;

                return default(SyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _leftOperand, 0);
                case 2: return GetRed(ref _rightOperand, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMemberAccess(this);
        }

    }

    public class UnaryPostixOperationExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _operand;

        internal UnaryPostixOperationExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken Operation
        {
            get { return new SyntaxToken(this, ((Parser.Internal.UnaryPostixOperationExpressionSyntaxNode)_green)._operation); }
        }

        public ExpressionSyntaxNode Operand
        {
            get
            {
                var red = this.GetRed(ref this._operand, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _operand, 0);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitUnaryPostixOperationExpression(this);
        }

    }

    public class IndirectMemberAccessSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _expression;

        internal IndirectMemberAccessSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.IndirectMemberAccessSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.IndirectMemberAccessSyntaxNode)_green)._closingBracket); }
        }

        public ExpressionSyntaxNode Expression
        {
            get
            {
                var red = this.GetRed(ref this._expression, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _expression, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitIndirectMemberAccess(this);
        }

    }

    public class CommandExpressionSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _commandName;
        private SyntaxNode _arguments;

        internal CommandExpressionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public IdentifierNameSyntaxNode CommandName
        {
            get
            {
                var red = this.GetRed(ref this._commandName, 0);
                if (red != null)
                    return (IdentifierNameSyntaxNode)red;

                return default(IdentifierNameSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Arguments
        {
            get
            {
                var red = this.GetRed(ref this._arguments, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _commandName, 0);
                case 1: return GetRed(ref _arguments, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitCommandExpression(this);
        }

    }

    public class BaseClassInvokationSyntaxNode : ExpressionSyntaxNode
    {
        private SyntaxNode _methodName;
        private SyntaxNode _baseClassNameAndArguments;

        internal BaseClassInvokationSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AtSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.BaseClassInvokationSyntaxNode)_green)._atSign); }
        }

        public ExpressionSyntaxNode MethodName
        {
            get
            {
                var red = this.GetRed(ref this._methodName, 0);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        public ExpressionSyntaxNode BaseClassNameAndArguments
        {
            get
            {
                var red = this.GetRed(ref this._baseClassNameAndArguments, 2);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _methodName, 0);
                case 2: return GetRed(ref _baseClassNameAndArguments, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBaseClassInvokation(this);
        }

    }

    public class AttributeAssignmentSyntaxNode : SyntaxNode
    {
        private SyntaxNode _value;

        internal AttributeAssignmentSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken AssignmentSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.AttributeAssignmentSyntaxNode)_green)._assignmentSign); }
        }

        public ExpressionSyntaxNode Value
        {
            get
            {
                var red = this.GetRed(ref this._value, 1);
                if (red != null)
                    return (ExpressionSyntaxNode)red;

                return default(ExpressionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _value, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttributeAssignment(this);
        }

    }

    public class AttributeSyntaxNode : SyntaxNode
    {
        private SyntaxNode _name;
        private SyntaxNode _assignment;

        internal AttributeSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public IdentifierNameSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name, 0);
                if (red != null)
                    return (IdentifierNameSyntaxNode)red;

                return default(IdentifierNameSyntaxNode);
            }
        }

        public AttributeAssignmentSyntaxNode Assignment
        {
            get
            {
                var red = this.GetRed(ref this._assignment, 1);
                if (red != null)
                    return (AttributeAssignmentSyntaxNode)red;

                return default(AttributeAssignmentSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _name, 0);
                case 1: return GetRed(ref _assignment, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttribute(this);
        }

    }

    public class AttributeListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _nodes;

        internal AttributeListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.AttributeListSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.AttributeListSyntaxNode)_green)._closingBracket); }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _nodes, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAttributeList(this);
        }

    }

    public class MethodDefinitionSyntaxNode : MethodDeclarationSyntaxNode
    {
        private SyntaxNode _outputDescription;
        private SyntaxNode _name;
        private SyntaxNode _inputDescription;
        private SyntaxNode _commas;
        private SyntaxNode _body;

        internal MethodDefinitionSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken FunctionKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.MethodDefinitionSyntaxNode)_green)._functionKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.MethodDefinitionSyntaxNode)_green)._endKeyword); }
        }

        public FunctionOutputDescriptionSyntaxNode OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 1);
                if (red != null)
                    return (FunctionOutputDescriptionSyntaxNode)red;

                return default(FunctionOutputDescriptionSyntaxNode);
            }
        }

        public CompoundNameSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name, 2);
                if (red != null)
                    return (CompoundNameSyntaxNode)red;

                return default(CompoundNameSyntaxNode);
            }
        }

        public FunctionInputDescriptionSyntaxNode InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 3);
                if (red != null)
                    return (FunctionInputDescriptionSyntaxNode)red;

                return default(FunctionInputDescriptionSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas, 4);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        public SyntaxNodeOrTokenList Body
        {
            get
            {
                var red = this.GetRed(ref this._body, 5);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _outputDescription, 1);
                case 2: return GetRed(ref _name, 2);
                case 3: return GetRed(ref _inputDescription, 3);
                case 4: return GetRed(ref _commas, 4);
                case 5: return GetRed(ref _body, 5);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMethodDefinition(this);
        }

    }

    public class AbstractMethodDeclarationSyntaxNode : MethodDeclarationSyntaxNode
    {
        private SyntaxNode _outputDescription;
        private SyntaxNode _name;
        private SyntaxNode _inputDescription;

        internal AbstractMethodDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public FunctionOutputDescriptionSyntaxNode OutputDescription
        {
            get
            {
                var red = this.GetRed(ref this._outputDescription, 0);
                if (red != null)
                    return (FunctionOutputDescriptionSyntaxNode)red;

                return default(FunctionOutputDescriptionSyntaxNode);
            }
        }

        public CompoundNameSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name, 1);
                if (red != null)
                    return (CompoundNameSyntaxNode)red;

                return default(CompoundNameSyntaxNode);
            }
        }

        public FunctionInputDescriptionSyntaxNode InputDescription
        {
            get
            {
                var red = this.GetRed(ref this._inputDescription, 2);
                if (red != null)
                    return (FunctionInputDescriptionSyntaxNode)red;

                return default(FunctionInputDescriptionSyntaxNode);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _outputDescription, 0);
                case 1: return GetRed(ref _name, 1);
                case 2: return GetRed(ref _inputDescription, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitAbstractMethodDeclaration(this);
        }

    }

    public class MethodsListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _attributes;
        private SyntaxNode _methods;

        internal MethodsListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken MethodsKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.MethodsListSyntaxNode)_green)._methodsKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.MethodsListSyntaxNode)_green)._endKeyword); }
        }

        public AttributeListSyntaxNode Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                if (red != null)
                    return (AttributeListSyntaxNode)red;

                return default(AttributeListSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Methods
        {
            get
            {
                var red = this.GetRed(ref this._methods, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _attributes, 1);
                case 2: return GetRed(ref _methods, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitMethodsList(this);
        }

    }

    public class PropertiesListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _attributes;
        private SyntaxNode _properties;

        internal PropertiesListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken PropertiesKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.PropertiesListSyntaxNode)_green)._propertiesKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.PropertiesListSyntaxNode)_green)._endKeyword); }
        }

        public AttributeListSyntaxNode Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                if (red != null)
                    return (AttributeListSyntaxNode)red;

                return default(AttributeListSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Properties
        {
            get
            {
                var red = this.GetRed(ref this._properties, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _attributes, 1);
                case 2: return GetRed(ref _properties, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitPropertiesList(this);
        }

    }

    public class BaseClassListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _baseClasses;

        internal BaseClassListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken LessSign
        {
            get { return new SyntaxToken(this, ((Parser.Internal.BaseClassListSyntaxNode)_green)._lessSign); }
        }

        public SyntaxNodeOrTokenList BaseClasses
        {
            get
            {
                var red = this.GetRed(ref this._baseClasses, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _baseClasses, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitBaseClassList(this);
        }

    }

    public class ClassDeclarationSyntaxNode : StatementSyntaxNode
    {
        private SyntaxNode _attributes;
        private SyntaxNode _className;
        private SyntaxNode _baseClassList;
        private SyntaxNode _nodes;

        internal ClassDeclarationSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken ClassdefKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ClassDeclarationSyntaxNode)_green)._classdefKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.ClassDeclarationSyntaxNode)_green)._endKeyword); }
        }

        public AttributeListSyntaxNode Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                if (red != null)
                    return (AttributeListSyntaxNode)red;

                return default(AttributeListSyntaxNode);
            }
        }

        public IdentifierNameSyntaxNode ClassName
        {
            get
            {
                var red = this.GetRed(ref this._className, 2);
                if (red != null)
                    return (IdentifierNameSyntaxNode)red;

                return default(IdentifierNameSyntaxNode);
            }
        }

        public BaseClassListSyntaxNode BaseClassList
        {
            get
            {
                var red = this.GetRed(ref this._baseClassList, 3);
                if (red != null)
                    return (BaseClassListSyntaxNode)red;

                return default(BaseClassListSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Nodes
        {
            get
            {
                var red = this.GetRed(ref this._nodes, 4);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _attributes, 1);
                case 2: return GetRed(ref _className, 2);
                case 3: return GetRed(ref _baseClassList, 3);
                case 4: return GetRed(ref _nodes, 4);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitClassDeclaration(this);
        }

    }

    public class EnumerationItemValueSyntaxNode : SyntaxNode
    {
        private SyntaxNode _values;

        internal EnumerationItemValueSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken OpeningBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EnumerationItemValueSyntaxNode)_green)._openingBracket); }
        }

        public SyntaxToken ClosingBracket
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EnumerationItemValueSyntaxNode)_green)._closingBracket); }
        }

        public SyntaxNodeOrTokenList Values
        {
            get
            {
                var red = this.GetRed(ref this._values, 1);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _values, 1);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationItemValue(this);
        }

    }

    public class EnumerationItemSyntaxNode : SyntaxNode
    {
        private SyntaxNode _name;
        private SyntaxNode _values;
        private SyntaxNode _commas;

        internal EnumerationItemSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }


        public IdentifierNameSyntaxNode Name
        {
            get
            {
                var red = this.GetRed(ref this._name, 0);
                if (red != null)
                    return (IdentifierNameSyntaxNode)red;

                return default(IdentifierNameSyntaxNode);
            }
        }

        public EnumerationItemValueSyntaxNode Values
        {
            get
            {
                var red = this.GetRed(ref this._values, 1);
                if (red != null)
                    return (EnumerationItemValueSyntaxNode)red;

                return default(EnumerationItemValueSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Commas
        {
            get
            {
                var red = this.GetRed(ref this._commas, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 0: return GetRed(ref _name, 0);
                case 1: return GetRed(ref _values, 1);
                case 2: return GetRed(ref _commas, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationItem(this);
        }

    }

    public class EnumerationListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _attributes;
        private SyntaxNode _items;

        internal EnumerationListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken EnumerationKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EnumerationListSyntaxNode)_green)._enumerationKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EnumerationListSyntaxNode)_green)._endKeyword); }
        }

        public AttributeListSyntaxNode Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                if (red != null)
                    return (AttributeListSyntaxNode)red;

                return default(AttributeListSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Items
        {
            get
            {
                var red = this.GetRed(ref this._items, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _attributes, 1);
                case 2: return GetRed(ref _items, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEnumerationList(this);
        }

    }

    public class EventsListSyntaxNode : SyntaxNode
    {
        private SyntaxNode _attributes;
        private SyntaxNode _events;

        internal EventsListSyntaxNode(SyntaxNode parent, Internal.GreenNode green) : base(parent, green)
        {
        }

        public SyntaxToken EventsKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EventsListSyntaxNode)_green)._eventsKeyword); }
        }

        public SyntaxToken EndKeyword
        {
            get { return new SyntaxToken(this, ((Parser.Internal.EventsListSyntaxNode)_green)._endKeyword); }
        }

        public AttributeListSyntaxNode Attributes
        {
            get
            {
                var red = this.GetRed(ref this._attributes, 1);
                if (red != null)
                    return (AttributeListSyntaxNode)red;

                return default(AttributeListSyntaxNode);
            }
        }

        public SyntaxNodeOrTokenList Events
        {
            get
            {
                var red = this.GetRed(ref this._events, 2);
                if (red != null)
                    return (SyntaxNodeOrTokenList)red;

                return default(SyntaxNodeOrTokenList);
            }
        }

        internal override SyntaxNode GetNode(int i)
        {
            switch (i)
            {
                case 1: return GetRed(ref _attributes, 1);
                case 2: return GetRed(ref _events, 2);
                default: return null;
            }
        }

        public override void Accept(SyntaxVisitor visitor)
        {
            visitor.VisitEventsList(this);
        }

    }
}