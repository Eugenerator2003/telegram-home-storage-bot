using StorageBot.Host.Routing.Attributes.Actions.Required;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public class RequiredNavigationCollection : IRequiredNavigation
    {
        private class RequiredNavigationContext
        {
            public required MethodInfo Method { get; set; }

            public required NavigationInfo NavigationInfo { get; set; }
        }

        private static void ThrowCollision(MethodInfo exists, MethodInfo detected, string attributeName)
        {
            throw new InvalidOperationException($"Collision for actions with attribute \"{attributeName}\":" +
                $"{exists.Name} ({exists.DeclaringType?.Name}), {detected.Name} ({detected.DeclaringType?.Name})");
        }

        private static void ThrowNotFound(string attributeName)
        {
            throw new InvalidOperationException($"Action with attribute {attributeName} not found");
        }

        private readonly Type[] RequiredAttributes;
        private readonly Dictionary<Type, RequiredNavigationContext> contexts;

        public RequiredNavigationCollection(Assembly assembly)
        {
            contexts = new Dictionary<Type, RequiredNavigationContext>();
            RequiredAttributes = assembly
                .GetTypes()
                .Where(t => t.IsAssignableTo(typeof(RequiredActionAttribute)) && !t.IsAbstract)
                .ToArray();
        }

        public NavigationInfo GetRequiredAction<TAttribute>() where TAttribute : RequiredActionAttribute
        {
            var type = typeof(TAttribute);
            if (!RequiredAttributes.Contains(type))
            {
                throw new InvalidOperationException($"Attribute {type.Name} not registered as required");
            }
            if (!contexts.TryGetValue(type, out var context))
            {
                throw new InvalidOperationException($"Action with attribute {type.Name} not registered yet");
            }
            return context.NavigationInfo;
        }

        public void AddIfRequired(MethodInfo methodInfo, int controllerId, int methodId)
        {
            for(int i = 0; i < RequiredAttributes.Length; i++)
            {
                if (methodInfo.GetCustomAttribute(RequiredAttributes[i]) != null)
                {
                    var navigationContext = new RequiredNavigationContext()
                    {
                        Method = methodInfo,
                        NavigationInfo = new NavigationInfo(controllerId, methodId)
                    };
                    if (!contexts.TryAdd(RequiredAttributes[i], navigationContext))
                    {
                        ThrowCollision(contexts[RequiredAttributes[i]]!.Method, methodInfo, RequiredAttributes[i].Name);
                    }
                }
            }
        }

        public void ValidateRequired()
        {
            for(int i = 0; i < RequiredAttributes.Length; i++)
            {
                if (!contexts.TryGetValue(RequiredAttributes[i], out var _))
                {
                    ThrowNotFound(RequiredAttributes[i].Name);
                }
            }
        }
    }
}
