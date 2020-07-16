using System;
using System.Collections.Generic;
using System.Linq;
using Parser;

namespace Semantics
{
    public class GetClass
    {
        private static MMethod MethodFromDefinition(ConcreteMethodDeclarationSyntaxNode methodDefinition)
        {
            var name = methodDefinition.Name.Text;
            var description = "";
            description += string.Join("", methodDefinition.LeadingTrivia.Select(x => x.FullText));
            if (methodDefinition.Body == null)
            {
                description += string.Join("", methodDefinition.EndKeyword.LeadingTrivia.Select(x => x.FullText));
            }
            else
            {
                description += string.Join("", methodDefinition.Body.LeadingTrivia.Select(x => x.FullText));
            }

            return new MMethod(name, description);
        }
        
        private static MMethod MethodFromDeclaration(AbstractMethodDeclarationSyntaxNode methodDeclaration)
        {
            var name = methodDeclaration.Name.Text;
            var description = "";
            description += string.Join("", methodDeclaration.LeadingTrivia.Select(x => x.FullText));
            return new MMethod(name, description);
        }
        
        private static List<MMethod> MethodsFromList(MethodsListSyntaxNode methodsList)
        {
            var result = new List<MMethod>();
            foreach (var method in methodsList.Methods)
            {
                if (method.IsToken)
                {
                    continue;
                }

                if (method.AsNode() is ConcreteMethodDeclarationSyntaxNode methodDefinition)
                {
                    result.Add(MethodFromDefinition(methodDefinition));
                }

                if (method.AsNode() is AbstractMethodDeclarationSyntaxNode methodDeclaration)
                {
                    result.Add(MethodFromDeclaration(methodDeclaration));
                }
            }

            return result;
        }
        
        public static MClass FromTree(FileSyntaxNode tree, string fileName)
        {
            var classDeclaration = tree.Body.Statements[0].AsNode() as ClassDeclarationSyntaxNode;
            if (classDeclaration == null)
            {
                return null;
            }
            var name = classDeclaration.ClassName.Text;
            var methods = new List<MMethod>();
            foreach (var s in classDeclaration.Nodes)
            {
                if (s.IsToken)
                {
                    continue;
                }
                if (s.AsNode() is MethodsListSyntaxNode methodsList)
                {
                    methods.AddRange(MethodsFromList(methodsList));
                }
            }
            return new MClass(tree, name, fileName, methods);
        }
    }
}