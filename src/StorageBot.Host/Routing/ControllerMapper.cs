using StorageBot.Host.Controllers;
using StorageBot.Host.Routing.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public class ControllerMapper : IControllerTypeProvider, INavigationIdProvider
    {
        private static int GetControllerId(Type controllerType)
        {
            var attribute = controllerType.GetCustomAttribute<ControllerIdAttribute>()
                                ?? throw new InvalidOperationException("Controller must have an id attribute");
            return attribute.Id;
        }

        private static int GetActionId(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<ControllerIdAttribute>()
                                ?? throw new InvalidOperationException("Action method must have an id attribute");
            return attribute.Id;
        }

        private List<Type> controllerTypes;
        private List<List<MethodInvoker>> actionInvokers;

        private Dictionary<string, int> controllerIds;
        private Dictionary<string, Dictionary<string, int>> actionIds;

        public ControllerMapper(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            controllerTypes = new List<Type>();
            actionInvokers = new List<List<MethodInvoker>>();
            controllerIds = new Dictionary<string, int>();
            actionIds = new Dictionary<string, Dictionary<string, int>>();
            Initialize(assembly);
        }

        public int GetActionId<T>(string actionName) where T : BotController
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(actionName);
            if (actionIds.TryGetValue(typeof(T).Name, out var methods))
            {
                if (methods.TryGetValue(actionName, out int id))
                {
                    return id; 
                }
                else
                {
                    throw new InvalidOperationException("Action not found");
                }
            }
            else
            {
                throw new InvalidOperationException("Controller not found");
            }
        }

        public int GetControllerId(string controllerName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(controllerName);
            if (controllerIds.TryGetValue(controllerName, out int id))
            {
                return id;
            }
            else
            {
                throw new InvalidOperationException("Controller not found");
            }
        }

        public ControllerTypeActionPair GetControllerWithAction(int controllerId, int actionId)
        {
            if (controllerId < 0 || actionId < 0)
            {
                throw new ArgumentException("Controller and action IDs must be non-negative");
            }
            if (controllerId > controllerTypes.Count || 
                actionId > actionInvokers[controllerId].Count)
            {
                throw new ArgumentException("Controller or action ID is greater than allowed");
            }
            var controllerType = controllerTypes[controllerId]
                ?? throw new InvalidOperationException("Controller not found");
            var action = actionInvokers[controllerId][actionId]
                ?? throw new InvalidOperationException("Action not found");
            return new ControllerTypeActionPair(controllerType, action);
        }

        private void Initialize(Assembly assembly)
        {
            var types = assembly.GetTypes()
                                .Where(c => c.IsSubclassOf(typeof(BotController)))
                                .OrderBy(GetControllerId)
                                .ToArray();

            if (types.Length == 0)
            {
                throw new InvalidOperationException("No controllers found in assembly");
            }
            int previous = AddBeforeFirstControllers(types[0]);
            for (int i = 0; i < types.Length; i++)
            {
                var idC = GetControllerId(types[i]);
                if (i > 0 && idC == previous)
                {
                    throw new InvalidOperationException($"Controller Id collision: {types[i].Name}, {types[i - 1].Name}");
                }
                for (int j = 0; j < idC - previous - 1; j++)
                {
                    AddEmptyController();
                }
                AddController(types[i], idC);
                var methods = types[i]
                    .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .OrderBy(GetActionId)
                    .ToArray();
                if (methods.Length == 0)
                {
                    throw new InvalidOperationException($"No methods found in controller {types[i].Name}");
                }
                previous = AddBeforeFirstActions(idC, methods[0]);
                for (int j = 0; j < methods.Length; j++)
                {
                    var idM = GetActionId(methods[j]);
                    if (j > 0 && idM == previous)
                    {
                        throw new InvalidOperationException($"Action Id collision in {types[i].Name} controller: {methods[i].Name}, {methods[i - 1].Name}");
                    }
                    for (int k = 0; k < idM - previous - 1; k++)
                    {
                        AddEmptyAction(idC);
                    }
                    AddAction(types[i], methods[j], idC, idM);
                    previous = idM;
                }
                previous = idC;
            }
        }

        private int AddBeforeFirstControllers(Type type)
        {
            var id = GetControllerId(type);
            for(int i = 0; i < id; i++)
            {
                AddEmptyController();
            }
            return id;
        }

        private void AddEmptyController()
        {
            controllerTypes.Add(null!);
            actionInvokers.Add(null!);
        }

        private void AddController(Type type, int controllerId)
        {
            controllerTypes.Add(type);
            actionInvokers.Add(new List<MethodInvoker>());
            controllerIds.Add(type.Name, controllerId);
            actionIds.Add(type.Name, new Dictionary<string, int>());
        }

        private void AddEmptyAction(int controllerId)
        {
            actionInvokers[controllerId].Add(null!);
        }

        private void AddAction(Type type, MethodInfo method, int controllerId, int actionId)
        {
            var actionInvoker = MethodInvoker.Create(method);
            actionInvokers[controllerId].Add(actionInvoker);
            actionIds[type.Name].Add(method.Name, actionId);
        }

        private int AddBeforeFirstActions(int controllerId, MethodInfo methodInfo)
        {
            var actionId = GetActionId(methodInfo);
            for (int i = 0; i < actionId; i++)
            {
                AddEmptyAction(controllerId);
            }
            return actionId;
        }
    }
}
