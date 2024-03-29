﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Parser.Internal
{
    internal abstract class GreenNode
    {
        public TokenKind Kind { get; }
        public int Slots { get; protected set; }
        public abstract GreenNode? GetSlot(int i);
        public bool HasDiagnostics { get; }
        private static Dictionary<GreenNode, TokenDiagnostic[]> diagnosticsTable = new Dictionary<GreenNode, TokenDiagnostic[]>();
        private static TokenDiagnostic[] emptyDiagnostics = Array.Empty<TokenDiagnostic>();

        public GreenNode(TokenKind kind)
        {
            Kind = kind;
        }

        public GreenNode(TokenKind kind, int fullWidth)
        {
            Kind = kind;
            _fullWidth = fullWidth;
        }

        public GreenNode(TokenKind kind, TokenDiagnostic[] diagnostics)
        {
            Kind = kind;
            HasDiagnostics = true;
            diagnosticsTable[this] = diagnostics;
        }

        public GreenNode(TokenKind kind, int fullWidth, TokenDiagnostic[] diagnostics)
        {
            Kind = kind;
            _fullWidth = fullWidth;
            HasDiagnostics = true;
            diagnosticsTable[this] = diagnostics;
        }

        internal abstract Parser.SyntaxNode CreateRed(Parser.SyntaxNode parent, int position);

        public virtual object? GetValue()
        {
            return null;
        }

        protected int _fullWidth;

        public int FullWidth => _fullWidth;

        protected void AdjustWidth(GreenNode? node)
        {
            if (!(node is null))
            {
                _fullWidth += node.FullWidth;
            }
        }

        public virtual string Text
        {
            get
            {
                var sb = new StringBuilder();

                using (var writer = new System.IO.StringWriter(sb, CultureInfo.InvariantCulture))
                {
                    WriteTo(writer, leading: false, trailing: false);
                }

                return sb.ToString();                
            }
        }

        protected bool _isMissing = false;

        internal bool IsMissing => _isMissing;

        private void WriteTo(TextWriter writer, bool leading, bool trailing)
        {
            var stack = new Stack<(GreenNode node, bool leading, bool trailing)>();
            stack.Push((this, leading, trailing));
            WriteStackTo(writer, stack);
        }

        public virtual bool IsToken => false;
        public virtual bool IsTrivia => false;
        public virtual bool IsNode => true;
        public virtual bool IsList => false;

        public virtual void WriteTokenTo(TextWriter writer, bool leading, bool trailing)
        {
            throw new NotImplementedException();
        }

        public virtual void WriteTriviaTo(TextWriter writer)
        {
            throw new NotImplementedException();
        }

        private int GetFirstNonNullChildIndex()
        {
            for (var i = 0; i < Slots; i++)
            {
                if (GetSlot(i) != null)
                {
                    return i;
                }
            }

            return Slots;
        }

        private int GetLastNonNullChildIndex()
        {
            for (var i = Slots - 1; i >= 0; i--)
            {
                if (GetSlot(i) != null)
                {
                    return i;
                }
            }

            return -1;
        }

        private GreenNode? GetFirstTerminal()
        {
            var current = this;
            while (true)
            {
                GreenNode? next = null;
                if (current.Slots == 0)
                {
                    return current;
                }

                for (var i = 0; i < current.Slots; i++)
                {
                    var child = current.GetSlot(i);
                    if (child == null)
                    {
                        continue;
                    }

                    next = child;
                    break;
                }

                if (next == null)
                {
                    return null;
                }

                current = next;
            }
        }

        private GreenNode? GetLastTerminal()
        {
            var current = this;
            while (true)
            {
                GreenNode? next = null;
                if (current.Slots == 0)
                {
                    return current;
                }

                for (var i = current.Slots - 1; i >= 0; i--)
                {
                    var child = current.GetSlot(i);
                    if (child == null)
                    {
                        continue;
                    }

                    next = child;
                    break;
                }

                if (next == null)
                {
                    return null;
                }

                current = next;
            }
        }

        public virtual GreenNode? LeadingTrivia => GetFirstTerminal()?.LeadingTriviaCore;
        public virtual GreenNode? TrailingTrivia => GetLastTerminal()?.TrailingTriviaCore;

        public virtual GreenNode? LeadingTriviaCore => null;
        public virtual GreenNode? TrailingTriviaCore => null;

        public virtual string FullText
        {
            get
            {
                var sb = new StringBuilder();

                using (var writer = new System.IO.StringWriter(sb, CultureInfo.InvariantCulture))
                {
                    WriteTo(writer, leading: true, trailing: true);
                }

                return sb.ToString();
            }
        } 

        private void WriteStackTo(TextWriter writer, Stack<(GreenNode node, bool leading, bool trailing)> stack)
        {
            while (stack.Count > 0)
            {
                var currentTriple = stack.Pop();
                if (currentTriple.node.IsToken)
                {
                    currentTriple.node.WriteTokenTo(writer, currentTriple.leading, currentTriple.trailing);
                } else if (currentTriple.node.IsTrivia)
                {
                    currentTriple.node.WriteTriviaTo(writer);
                }
                else
                {
                    var firstIndex = currentTriple.node.GetFirstNonNullChildIndex();
                    var lastIndex = currentTriple.node.GetLastNonNullChildIndex();
                    for (var i = lastIndex; i >= firstIndex; i--)
                    {
                        var child = currentTriple.node.GetSlot(i);
                        if (child != null)
                        {
                            stack.Push((
                                child,
                                currentTriple.leading || i != firstIndex,
                                currentTriple.trailing || i != lastIndex));
                        }
                    }
                }
            }
        }

        public abstract GreenNode SetDiagnostics(TokenDiagnostic[] diagnostics);

        internal TokenDiagnostic[] GetDiagnostics()
        {
            if (diagnosticsTable.TryGetValue(this, out var diags))
            {
                return diags;
            }

            return emptyDiagnostics;
        }
    }
}