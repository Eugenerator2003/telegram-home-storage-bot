using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using StorageBot.Host.Controllers;
using StorageBot.Host.Tests.Infrastructure.TestAttributes;
using StorageBot.Host.Tests.Infrastructure.TestControllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.TestUtilities
{
    public class AssemblyGenerator
    {
        public Assembly Generate(string code)
        {
            var tree = CSharpSyntaxTree.ParseText(code);

            var references = new List<MetadataReference>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                    {
                        references.Add(MetadataReference.CreateFromFile(assembly.Location));
                    }
                }
                catch (NotSupportedException)
                {
                    continue;
                }
            }

            //references.Add(MetadataReference.CreateFromFile(typeof(RequiredAttribute)));
            var compilation = CSharpCompilation.Create(
                $"test_assembly_{Random.Shared.Next()}",
                new[] { tree },
                references,
                new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            );

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            if (!result.Success)
            {
                var errors = string.Join(Environment.NewLine, result.Diagnostics);
                throw new InvalidOperationException(errors);
            }

            ms.Seek(0, SeekOrigin.Begin);
            return Assembly.Load(ms.ToArray());
        }
    }
}
