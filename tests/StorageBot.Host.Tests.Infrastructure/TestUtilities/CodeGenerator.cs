using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.TestUtilities
{
    public static class CodeGenerator
    {
        public static StringBuilder GetBaseUsing()
        {
            return new StringBuilder(
                """
                using System;
                using System.Reflection;
                using StorageBot.Host.Controllers; 
                using StorageBot.Host.Routing.Attributes.Actions.Required;
                using StorageBot.Host.Routing.Attributes.Actions.Updates;
                using StorageBot.Host.Routing.Attributes.Controllers;
                """);
        }

        public static StringBuilder AddRequiredAttribute(this StringBuilder builder, string attributeName)
        {
            return builder.Append(
                $$"""
                [AttributeUsage(AttributeTargets.Method)]
                public class {{attributeName}}Attribute : RequiredActionAttribute
                {
                }
                """
                );
        }

        public static StringBuilder AddController(this StringBuilder builder, string controllerName, int id, string[] methods, int[] methodsId, string?[] requiredAttributes)
        {
            if (methods.Length != methodsId.Length || methodsId.Length != requiredAttributes.Length)
            {
                throw new ArgumentException();
            }
            builder.Append(
                $$"""
                [ControllerId({{id}})]
                public class {{controllerName}}
                {
                """);
            for(int i = 0; i < methods.Length; i++)
            {
                if (requiredAttributes[i] != null)
                {
                    builder.Append($"[{requiredAttributes[i]}]");
                }
                builder.Append(
                    $$"""
                    [ActionId({{methodsId[i]}})]
                    public void {{methods[i]}}() {}
                    """);
            }
            builder.Append('}');
            return builder;
        }
    }
}
