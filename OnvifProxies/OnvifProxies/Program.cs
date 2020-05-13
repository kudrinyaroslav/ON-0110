using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Symbols;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace OnvifProxies
{
    class TypeWalker: CSharpSyntaxWalker
    {
        public List<BaseTypeDeclarationSyntax> SharedNodes  = new List<BaseTypeDeclarationSyntax>();
        public List<BaseTypeDeclarationSyntax> Onvif1Nodes  = new List<BaseTypeDeclarationSyntax>();
        public List<BaseTypeDeclarationSyntax> Onvif2Nodes  = new List<BaseTypeDeclarationSyntax>();
        public List<BaseTypeDeclarationSyntax> StorageNodes = new List<BaseTypeDeclarationSyntax>();

        private List<BaseTypeDeclarationSyntax> Last;


        private HashSet<string> Onvif1Namespaces  = new HashSet<string>(new[] 
                                                                        {
                                                                            "http://www.onvif.org/ver10/device/wsdl",
                                                                            "http://www.onvif.org/ver10/media/wsdl",
                                                                            "http://www.onvif.org/ver10/deviceIO/wsdl",
                                                                            "http://www.onvif.org/ver10/advancedsecurity/wsdl",
                                                                            "http://www.onvif.org/ver10/accessrules/wsdl",
                                                                            "http://www.onvif.org/ver10/credential/wsdl",
                                                                            "http://www.onvif.org/ver10/schedule/wsdl",
                                                                            "http://www.onvif.org/ver10/thermal/wsdl"
                                                                        });

        private HashSet<string> Onvif2Namespaces = new HashSet<string>(new[]
                                                                        {
                                                                            "http://www.onvif.org/ver20/imaging/wsdl",
                                                                            "http://www.onvif.org/ver20/ptz/wsdl",
                                                                            "http://www.onvif.org/ver20/media/wsdl",
                                                                            "http://www.onvif.org/ver20/analytics/wsdl",
                                                                            "http://www.onvif.org/ver20/radiometry/wsdl"
                                                                        });

        private HashSet<string> StorageNamespaces = new HashSet<string>(new[] 
                                                                        {
                                                                            "http://www.onvif.org/ver10/replay/wsdl",
                                                                            "http://www.onvif.org/ver10/receiver/wsdl",
                                                                            "http://www.onvif.org/ver10/search/wsdl",
                                                                            "http://www.onvif.org/ver10/recording/wsdl"
                                                                        });

        private HashSet<string> AttributesToLook = new HashSet<string>(new [] { "XmlTypeAttribute", "MessageContractAttribute", "ServiceContractAttribute" });
        private string GetNamespaceLink(SyntaxNode node)
        {
            if (node is AttributeListSyntax)
            {
                var attributeListNode = node as AttributeListSyntax;

                foreach (var attributeName in AttributesToLook)
                {
                    var attribute = attributeListNode.Attributes.FirstOrDefault(e => e.ToString().Contains(attributeName));
                    if (attribute != null && attribute.ArgumentList.Arguments.Any())
                    {
                        var arg = attribute.ArgumentList.Arguments.FirstOrDefault(e => e.ToString().Contains("Namespace"));
                        return arg != null ? arg.Expression.GetText().ToString().Trim('"') : string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        private bool CheckAlreadyExists(BaseTypeDeclarationSyntax node, IEnumerable<BaseTypeDeclarationSyntax> nodes)
        {
            foreach (var T in nodes)
            {
                if (T.IsEquivalentTo(node))
                    return true;
            }

            return false;
        }

        private void _Visit(BaseTypeDeclarationSyntax node)
        {
            var name = node.AttributeLists.FirstOrDefault(e => !string.IsNullOrEmpty(GetNamespaceLink(e)));

            if (null != name)
            {
                var ns = GetNamespaceLink(name);
                //if (ns.StartsWith("http://www.onvif.org/ver10/schema"))
                //    SharedNodes.Add(node);
                //else if (ns.StartsWith("http://www.onvif.org/ver1"))
                //    Onvif1Nodes.Add(node);
                //else if (ns.StartsWith("http://www.onvif.org/ver2"))
                //    Onvif2Nodes.Add(node);
                //else
                //    SharedNodes.Add(node);
                if (Onvif1Namespaces.Contains(ns))
                {
                    Onvif1Nodes.Add(node);
                    Last = Onvif1Nodes;
                }
                else if (Onvif2Namespaces.Contains(ns))
                {
                    Onvif2Nodes.Add(node);
                    Last = Onvif2Nodes;
                }
                else if (StorageNamespaces.Contains(ns))
                {
                    StorageNodes.Add(node);
                    Last = StorageNodes;
                }
                else
                {
                    if (!CheckAlreadyExists(node, SharedNodes))
                    {
                        SharedNodes.Add(node);
                        Last = SharedNodes;
                    }
                }
            }
            else
                Last.Add(node);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            _Visit(node);
        }

        public override void VisitStructDeclaration(StructDeclarationSyntax node)
        {
            _Visit(node);
        }

        public override void VisitEnumDeclaration(EnumDeclarationSyntax node)
        {
            _Visit(node);
        }

        public override void VisitInterfaceDeclaration(InterfaceDeclarationSyntax node)
        {
            _Visit(node);
        }
    }

    class Program
    {
        static CompilationUnitSyntax BuildUnit(IEnumerable<MemberDeclarationSyntax> nodes)
        {
            var unit = SyntaxFactory.CompilationUnit();
            foreach (var T in nodes)
                unit = unit.AddMembers(T);

            return unit;
        }

        static void WriteUnit(string file, CompilationUnitSyntax unit)
        {
            File.WriteAllText(file, unit.GetText().ToString());
        }

        static void Main(string[] args)
        {
            string codeFolderPath = @"C:\Projects\ONVIF\Src\TFSSrc\ON-0110\wsdl\all";
            SyntaxTree tree1 = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(codeFolderPath, "onvif1.cs")));
            SyntaxTree tree2 = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(codeFolderPath, "onvif2.cs")));
            SyntaxTree treeStorage = CSharpSyntaxTree.ParseText(File.ReadAllText(Path.Combine(codeFolderPath, "storage.cs")));

            var walker = new TypeWalker();

            walker.Visit(tree1.GetRoot());
            walker.Visit(tree2.GetRoot());
            walker.Visit(treeStorage.GetRoot());

            WriteUnit(Path.Combine(codeFolderPath, "onvif1_.cs"), BuildUnit(walker.Onvif1Nodes));
            WriteUnit(Path.Combine(codeFolderPath, "onvif2_.cs"), BuildUnit(walker.Onvif2Nodes));
            WriteUnit(Path.Combine(codeFolderPath, "storage_.cs"), BuildUnit(walker.StorageNodes));
            WriteUnit(Path.Combine(codeFolderPath, "onvif_.cs"), BuildUnit(walker.SharedNodes));
        }
    }
}
