using System.Collections.Generic;
using System.Linq;
using Lexer;

namespace Parser
{
    public class SyntaxNode
    {
        public SyntaxNode Parent { get; set; }
        public List<SyntaxNode> Children { get; }

        public virtual IEnumerable<Token> ChildTokens =>
            Children.SelectMany(c => c.ChildTokens);

        public SyntaxNode(List<SyntaxNode> children)
        {
            Children = children;
            if (children != null)
            {
                foreach (var child in children)
                {
                    child.Parent = this;
                }
            }
        }

        public virtual string FullText =>
            string.Join("", Children.Select(c => c.FullText));

        public List<Trivia> TrailingTrivia
  
        {
            get
            {
                if (ChildTokens.Any())
                {
                    return ChildTokens.Last().TrailingTrivia;
                }
                else
                {
                    return new List<Trivia>();
                }
            }
        }

    }

    public class TokenNode : SyntaxNode
    {
        public Token Token { get; }

        public TokenNode(Token token)
            : base(null)
        {
            Token = token;
        }

        public override string FullText => Token.FullText;

        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }
    }

    public class OutputIdentifierNode : SyntaxNode
    {
        public OutputIdentifierNode(List<SyntaxNode> children) : base(children)
        {
        }
    }

    public class FunctionOutputDescriptionNode : SyntaxNode
    {
        public List<TokenNode> Outputs { get; }
        public TokenNode EqualitySign { get;  }
        
        public FunctionOutputDescriptionNode(
            List<SyntaxNode> children,
            List<TokenNode> outputs,
            TokenNode equalitySign) : base(children)
        {
            Outputs = outputs;
            EqualitySign = equalitySign;
        }
    }

    public class FunctionInputDescriptionNode : SyntaxNode
    {
        public TokenNode OpeningBracket { get; }
        public ParameterListNode Parameters { get; }
        public TokenNode ClosingBracket { get; }
        public FunctionInputDescriptionNode(
            List<SyntaxNode> children,
            TokenNode openingBracket,
            ParameterListNode parameters,
            TokenNode closingBracket) : base(children)
        {
            OpeningBracket = openingBracket;
            Parameters = parameters;
            ClosingBracket = closingBracket;
        }
    }

    public class FunctionDeclarationNode : StatementNode
    {
        public TokenNode Token { get; }
        public FunctionOutputDescriptionNode OutputDescription { get; }
        public TokenNode Name { get; }
        public FunctionInputDescriptionNode InputDescription { get; }
        public StatementListNode Body { get; }
        public TokenNode End { get; }

        public FunctionDeclarationNode(
            List<SyntaxNode> children,
            TokenNode token,
            FunctionOutputDescriptionNode outputDescription,
            TokenNode name,
            FunctionInputDescriptionNode inputDescription,
            StatementListNode body,
            TokenNode end,
            TokenNode semicolonOrComma
            ) : base(children, semicolonOrComma)
        {
            Token = token;
            OutputDescription = outputDescription;
            Name = name;
            InputDescription = inputDescription;
            Body = body;
            End = end;
        }
    }

    public class StatementListNode : SyntaxNode
    {
        public List<SyntaxNode> Statements => Children;
        
        public StatementListNode(List<SyntaxNode> children) : base(children)
        {
        }
    }

    public class ParameterListNode : SyntaxNode
    {
        public List<SyntaxNode> Parameters { get; }

        public ParameterListNode(List<SyntaxNode> children, List<SyntaxNode> parameters) : base(children)
        {
            Parameters = parameters;
        }
    }

    public class ExpressionNode : SyntaxNode
    {
        public ExpressionNode(List<SyntaxNode> children) : base(children)
        {
        }
    }

    public class AssignmentExpressionNode : ExpressionNode
    {
        public ExpressionNode Lhs { get; }
        public TokenNode Assignment { get; }
        public ExpressionNode Rhs { get; }

        public AssignmentExpressionNode(
            List<SyntaxNode> children,
            ExpressionNode lhs,
            TokenNode assignment,
            ExpressionNode rhs) : base(children)
        {
            Lhs = lhs;
            Assignment = assignment;
            Rhs = rhs;
        }
    }

    public class UnaryPrefixOperationExpressionNode : ExpressionNode
    {
        public TokenNode Operation { get; }
        public ExpressionNode Operand { get; }

        public UnaryPrefixOperationExpressionNode(
            List<SyntaxNode> children,
            TokenNode operation,
            ExpressionNode operand) : base(children)
        {
            Operation = operation;
            Operand = operand;
        }
    }

    public class UnaryPostfixOperationExpressionNode : ExpressionNode
    {
        public ExpressionNode Operand { get; }
        public TokenNode Operation { get; }
        public UnaryPostfixOperationExpressionNode(
            List<SyntaxNode> children,
            ExpressionNode operand,
            TokenNode operation) : base(children)
        {
            Operand = operand;
            Operation = operation;
        }
    }

    public class BinaryOperationExpressionNode : ExpressionNode
    {
        public ExpressionNode Lhs { get; }
        public TokenNode Operation { get; }
        public ExpressionNode Rhs { get; }

        public BinaryOperationExpressionNode(
            List<SyntaxNode> children,
            ExpressionNode lhs,
            TokenNode operation,
            ExpressionNode rhs) : base(children)
        {
            Lhs = lhs;
            Operation = operation;
            Rhs = rhs;
        }
    }

    public class SwitchStatementNode : StatementNode
    {
        public TokenNode SwitchKeyword { get; }
        public ExpressionNode SwitchExpression { get; }
        public List<TokenNode> OptionalCommasAfterExpression { get; }
        public List<SwitchCaseNode> Cases { get; }
        public TokenNode EndKeyword { get; }

        public SwitchStatementNode(
            List<SyntaxNode> children,
            TokenNode switchKeyword,
            ExpressionNode switchExpression,
            List<SwitchCaseNode> cases,
            TokenNode endKeyword,
            TokenNode semicolonOrComma,
            List<TokenNode> optionalCommasAfterExpression = null
            ) : base(children, semicolonOrComma)
        {
            SwitchKeyword = switchKeyword;
            SwitchExpression = switchExpression;
            OptionalCommasAfterExpression = optionalCommasAfterExpression;
            Cases = cases;
            EndKeyword = endKeyword;
        }
    }

    public class SwitchCaseNode : SyntaxNode
    {
        public TokenNode CaseKeyword { get; }
        public ExpressionNode CaseIdentifier { get; }
        public List<TokenNode> OptionalCommasAfterIdentifier { get; }
        public StatementListNode StatementList { get; }

        public SwitchCaseNode(
            List<SyntaxNode> children,
            TokenNode caseKeyword,
            ExpressionNode caseIdentifier,
            StatementListNode statementList,
            List<TokenNode> optionalCommasAfterIdentifier = null
            ) : base(children)
        {
            CaseKeyword = caseKeyword;
            CaseIdentifier = caseIdentifier;
            StatementList = statementList;
            OptionalCommasAfterIdentifier = optionalCommasAfterIdentifier;
        }
    }

    public class IdentifierNameNode : ExpressionNode
    {
        public Token Token { get; }

        public IdentifierNameNode(Token token)
            : base(null)
        {
            Token = token;
        }

        public override string FullText => Token.FullText;
        
        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }

    }

    public class NumberLiteralNode : ExpressionNode
    {
        public Token Token { get; }

        public NumberLiteralNode(Token token) : base(null)
        {
            Token = token;
        }

        public override string FullText => Token.FullText;
        
        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }

    }

    public class StringLiteralNode : ExpressionNode
    {
        public Token Token { get; }

        public StringLiteralNode(Token token) : base(null)
        {
            Token = token;
        }
        
        public override string FullText => Token.FullText;
        
        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }

    }

    public class DoubleQuotedStringLiteralNode : ExpressionNode
    {
        public Token Token { get; }

        public DoubleQuotedStringLiteralNode(Token token) : base(null)
        {
            Token = token;
        }

        public override string FullText => Token.FullText;

        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }
    }

    public class UnquotedStringLiteralNode : ExpressionNode
    {
        public Token Token { get; }

        public UnquotedStringLiteralNode(Token token) : base(null)
        {
            Token = token;
        }
        
        public override string FullText => Token.FullText;
        
        public override IEnumerable<Token> ChildTokens
        {
            get { yield return Token; }
        }

    }

    public class StatementNode : SyntaxNode
    {
        public TokenNode SemicolonOrComma { get; set; }
        
        public StatementNode(List<SyntaxNode> children, TokenNode semicolonOrComma = null) : base(children)
        {
            SemicolonOrComma = semicolonOrComma;
        }
    }

    public class ExpressionStatementNode : StatementNode
    {
        public ExpressionNode Expression { get; }

        public ExpressionStatementNode(List<SyntaxNode> children, ExpressionNode expression, TokenNode semicolonOrComma)
            : base(children, semicolonOrComma)
        {
            Expression = expression;
        }
    }

    public class CellArrayElementAccessExpressionNode : ExpressionNode
    {
        public ExpressionNode CellArray { get; }
        public TokenNode OpeningBrace { get; }
        public ArrayElementListNode Indices { get; }
        public TokenNode ClosingBrace { get; }

        public CellArrayElementAccessExpressionNode(
            List<SyntaxNode> children,
            ExpressionNode cellArray,
            TokenNode openingBrace,
            ArrayElementListNode indices,
            TokenNode closingBrace) : base(children)
        {
            CellArray = cellArray;
            OpeningBrace = openingBrace;
            Indices = indices;
            ClosingBrace = closingBrace;
        }
    }

    public class FunctionCallExpressionNode : ExpressionNode
    {
        public ExpressionNode FunctionName { get; }
        public TokenNode OpeningBracket { get; }
        public FunctionCallParameterListNode Parameters { get; }
        public TokenNode ClosingBracket { get; }

        public FunctionCallExpressionNode(
            List<SyntaxNode> children,
            ExpressionNode functionName,
            TokenNode openingBracket,
            FunctionCallParameterListNode parameters,
            TokenNode closingBracket) : base(children)
        {
            FunctionName = functionName;
            OpeningBracket = openingBracket;
            Parameters = parameters;
            ClosingBracket = closingBracket;
        }
    }

    public class FunctionCallParameterListNode : SyntaxNode
    {
        public List<ExpressionNode> Parameters;

        public FunctionCallParameterListNode(
            List<SyntaxNode> children,
            List<ExpressionNode> parameters) : base(children)
        {
            Parameters = parameters;
        }
    }
    
    public class ArrayElementListNode : SyntaxNode
    {
        public List<ExpressionNode> Elements;

        public ArrayElementListNode(
            List<SyntaxNode> children,
            List<ExpressionNode> elements) : base(children)
        {
            Elements = elements;
        }
    }

    public class ArrayLiteralExpressionNode : ExpressionNode
    {
        public TokenNode OpeningSquareBracket { get; }
        public ArrayElementListNode Elements { get; }
        public TokenNode ClosingSquareBracket { get; }

        public ArrayLiteralExpressionNode(
            List<SyntaxNode> children,
            TokenNode openingSquareBracket,
            ArrayElementListNode elements,
            TokenNode closingSquareBracket) : base(children)
        {
            OpeningSquareBracket = openingSquareBracket;
            Elements = elements;
            ClosingSquareBracket = closingSquareBracket;
        }
    }

    public class CellArrayLiteralExpressionNode : ExpressionNode
    {
        public TokenNode OpeningBrace { get; }
        public ArrayElementListNode Elements { get; }
        public TokenNode ClosingBrace { get; }

        public CellArrayLiteralExpressionNode(
            List<SyntaxNode> children,
            TokenNode openingBrace,
            ArrayElementListNode elements,
            TokenNode closingBrace) : base(children)
        {
            OpeningBrace = openingBrace;
            Elements = elements;
            ClosingBrace = closingBrace;
        }
    }

    public class EmptyExpressionNode : ExpressionNode
    {
        public EmptyExpressionNode() : base(null)
        {
        }
        
        public override IEnumerable<Token> ChildTokens
        {
            get { yield break; }
        }

        public override string FullText => "";
    }

    public class CompoundNameNode : ExpressionNode
    {
        public List<IdentifierNameNode> Names;

        public CompoundNameNode(
            List<SyntaxNode> children,
            List<IdentifierNameNode> names
        ) : base(children)
        {
            Names = names;
        }
    }

    public class MemberAccessNode : ExpressionNode
    {
        public SyntaxNode LeftOperand { get; }
        public TokenNode Dot { get; }
        public SyntaxNode RightOperand { get; }

        public MemberAccessNode(
            List<SyntaxNode> children,
            SyntaxNode leftOperand,
            TokenNode dot,
            SyntaxNode rightOperand) : base(children)
        {
            LeftOperand = leftOperand;
            Dot = dot;
            RightOperand = rightOperand;
        }
    }

    public class WhileStatementNode : StatementNode
    {
        public TokenNode WhileKeyword { get; }
        public ExpressionNode Condition { get; }
        public List<TokenNode> OptionalCommasAfterCondition { get; }
        public StatementListNode Body { get; }
        public TokenNode End { get; }

        public WhileStatementNode(
            List<SyntaxNode> children,
            TokenNode whileKeyword,
            ExpressionNode condition,
            List<TokenNode> optionalCommasAfterCondition,
            StatementListNode body,
            TokenNode end,
            TokenNode semicolonOrComma
            ) : base(children, semicolonOrComma)
        {
            WhileKeyword = whileKeyword;
            Condition = condition;
            OptionalCommasAfterCondition = optionalCommasAfterCondition;
            Body = body;
            End = end;
        }
    }

    public class IfStatementNode : StatementNode
    {
        public TokenNode IfKeyword { get; }
        public ExpressionNode Condition { get; }
        public List<TokenNode> OptionalCommasAfterCondition { get; }
        public StatementListNode Body { get; }
        public TokenNode ElseKeyword { get; }
        public StatementListNode ElseBody { get; }
        public TokenNode EndKeyword { get; }

        public IfStatementNode(
            List<SyntaxNode> children,
            TokenNode ifKeyword,
            ExpressionNode condition,
            List<TokenNode> optionalCommasAfterCondition,
            StatementListNode body,
            TokenNode elseKeyword,
            StatementListNode elseBody,
            TokenNode endKeyword,
            TokenNode possibleSemicolonOrComma
            ) : base(children, possibleSemicolonOrComma)
        {
            IfKeyword = ifKeyword;
            Condition = condition;
            OptionalCommasAfterCondition = optionalCommasAfterCondition;
            Body = body;
            ElseKeyword = elseKeyword;
            ElseBody = elseBody;
            EndKeyword = endKeyword;
        }
    }

    public class ParenthesizedExpressionNode : ExpressionNode
    {
        public TokenNode OpenParen { get; }
        public ExpressionNode Expression { get; }
        public TokenNode CloseParen { get; }

        public ParenthesizedExpressionNode(
            List<SyntaxNode> children,
            TokenNode openParen,
            ExpressionNode expression,
            TokenNode closeParen) : base(children)
        {
            OpenParen = openParen;
            Expression = expression;
            CloseParen = closeParen;
        }
    }

    public class ForStatementNode : StatementNode
    {
        public TokenNode ForKeyword { get; }
        public AssignmentExpressionNode ForAssignment { get; }
        public List<TokenNode> OptionalCommasAfterAssignment { get; }
        public StatementListNode Body { get; }
        public TokenNode EndKeyword { get; }

        public ForStatementNode(
            List<SyntaxNode> children,
            TokenNode forKeyword,
            AssignmentExpressionNode forAssignment,
            StatementListNode body,
            TokenNode endKeyword,
            List<TokenNode> optionalCommasAfterAssignment
            ) : base(children)
        {
            ForKeyword = forKeyword;
            ForAssignment = forAssignment;
            Body = body;
            EndKeyword = endKeyword;
            OptionalCommasAfterAssignment = optionalCommasAfterAssignment;
        }
    }

    public class IndirectMemberAccessNode : ExpressionNode
    {
        public TokenNode OpeningBracket { get; }
        public ExpressionNode IndirectMemberName { get; }
        public TokenNode ClosingBracket { get; }

        public IndirectMemberAccessNode(
            List<SyntaxNode> children,
            TokenNode openingBracket,
            ExpressionNode indirectMemberName,
            TokenNode closingBracket) : base(children)
        {
            OpeningBracket = openingBracket;
            IndirectMemberName = indirectMemberName;
            ClosingBracket = closingBracket;
        }
    }
    
    public abstract class FunctionHandleNode : ExpressionNode
    {
        protected FunctionHandleNode(
            List<SyntaxNode> children) : base(children)
        {
        }
    }

    public class NamedFunctionHandleNode : FunctionHandleNode
    {
        public TokenNode AtSign { get; }
        public CompoundNameNode FunctionName { get; }

        public NamedFunctionHandleNode(
            List<SyntaxNode> children,
            TokenNode atSign,
            CompoundNameNode functionName) : base(children)
        {
            AtSign = atSign;
            FunctionName = functionName;
        }
    }

    public class LambdaNode : FunctionHandleNode
    {
        public TokenNode AtSign { get; }
        public FunctionInputDescriptionNode Input { get; }
        public ExpressionNode Body { get; }

        public LambdaNode(
            List<SyntaxNode> children,
            TokenNode atSign,
            FunctionInputDescriptionNode input,
            ExpressionNode body) : base(children)
        {
            AtSign = atSign;
            Input = input;
            Body = body;
        }
    }

    public class TryCatchStatementNode : StatementNode
    {
        public TokenNode TryKeyword { get; }
        public StatementListNode TryBody { get; }
        public TokenNode CatchKeyword { get; }
        public StatementListNode CatchBody { get; }
        public TokenNode EndKeyword { get; }

        public TryCatchStatementNode(
            List<SyntaxNode> children,
            TokenNode tryKeyword,
            StatementListNode tryBody,
            TokenNode catchKeyword,
            StatementListNode catchBody,
            TokenNode endKeyword
        ) : base(children)
        {
            TryKeyword = tryKeyword;
            TryBody = tryBody;
            CatchKeyword = catchKeyword;
            CatchBody = catchBody;
            EndKeyword = endKeyword;
        }
    }

    public class CommandExpressionNode : ExpressionNode
    {
        public IdentifierNameNode CommandName { get; }
        public List<UnquotedStringLiteralNode> Arguments { get; }

        public CommandExpressionNode(
            List<SyntaxNode> children,
            IdentifierNameNode commandName,
            List<UnquotedStringLiteralNode> arguments
        ) : base(children)
        {
            CommandName = commandName;
            Arguments = arguments;
        }
    }

    public class BaseClassInvokationNode : ExpressionNode
    {
        public IdentifierNameNode MethodName { get; }
        public TokenNode AtToken { get; }
        public ExpressionNode BaseClassNameAndArguments { get; }

        public BaseClassInvokationNode(
            List<SyntaxNode> children,
            IdentifierNameNode methodName,
            TokenNode atToken,
            ExpressionNode baseClassNameAndArguments
        ) : base(children)
        {
            MethodName = methodName;
            AtToken = atToken;
            BaseClassNameAndArguments = baseClassNameAndArguments;
        }
    }
}