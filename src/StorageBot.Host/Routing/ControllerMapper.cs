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
    public class ControllerMapper : IHandlerInfoResolver, IIdNavigation
    {
        private static int GetControllerId(Type controllerType)
        {
            var attribute = controllerType.GetCustomAttribute<ControllerIdAttribute>()
                                ?? throw new InvalidOperationException($"Controller {controllerType.Name} must have an id attribute");
            return attribute.Id;
        }

        private static int GetActionId(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<ActionIdAttribute>()
                                ?? throw new InvalidOperationException($"Action method {methodInfo.Name} from {methodInfo.DeclaringType!.Name} must have an id attribute");
            return attribute.Id;
        }

        private static bool IsMainController(Type controllerType)
        {
            return controllerType.GetCustomAttribute<MainControllerAttribute>() is not null;
        }

        private static bool IsMenuAction(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<MenuActionAttribute>() is not null;
        }

        private static bool IsNoCommandAction(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<UnregisteredCommandActionAttribute>() is not null;
        }
        
        private static bool IsUnknownCallbackQueryAction(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<UnknownCallbackQueryActionAttribute>() is not null;
        }

        private List<Type> controllerTypes;
        private List<List<MethodInvoker>> actionInvokers;

        private Dictionary<string, int> controllerIds;
        private Dictionary<string, Dictionary<string, int>> actionIds;

        private int mainControllerId = -1;
        private int menuActionId = -1;
        private int unregisteredCommandActionId = -1;
        private int unknownCallbackQueryActionId = -1;

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

        public int GetMainControllerId()
        {
            return mainControllerId;
        }

        public int GetMenuActionId()
        {
            return menuActionId;
        }

        public int GetUnregisteredCommandActionId()
        {
            return unregisteredCommandActionId;
        }

        public int GetUnknownCallbackQueryActionId()
        {
            return unknownCallbackQueryActionId;
        }

        public HandlerInfo GetHandlerInfo(int controllerId, int actionId)
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
            return new HandlerInfo(controllerType, action);
        }

        private void Initialize(Assembly assembly)
        {
            var types = assembly.GetTypes()
                                .Where(c => !c.IsAbstract && c.IsSubclassOf(typeof(BotController)))
                                .Select(c => new { Type = c, Id = GetControllerId(c) })
                                .OrderBy(c => c.Id)
                                .ToArray();
            var taskType = typeof(Task);

            if (types.Length == 0)
                throw new InvalidOperationException("No controllers found in assembly");

            controllerTypes.Capacity = types[types.Length - 1].Id + 1;
            AddBeforeFirstControllers(types[0].Id);
            int previous = 0;
            for (int i = 0; i < types.Length; i++)
            {
                var idC = types[i].Id;

                if (i > 0 && idC == previous)
                    throw new InvalidOperationException($"Controller Id collision: {types[i].Type.Name}, {types[i - 1].Type.Name}");

                for (int j = 0; j < idC - previous - 1; j++)
                {
                    AddEmptyController();
                }
                AddController(types[i].Type, idC);
                AddIfMainController(types[i].Type, idC);

                var methods = types[i].Type
                    .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Select(m => new { Type = m, Id = GetActionId(m) })
                    .OrderBy(m => m.Id)
                    .ToArray();

                if (methods.Length == 0)
                    throw new InvalidOperationException($"No methods found in controller {types[i].Type.Name}");

                AddBeforeFirstActions(idC, methods[0].Id);
                previous = 0;
                for (int j = 0; j < methods.Length; j++)
                {
                    if (methods[j].Type.ReturnType.IsInstanceOfType(taskType))
                        throw new InvalidOperationException($"Controller action {methods[j].Type.Name} ({types[i].Type.Name}) must return a Task");

                    var idM = methods[j].Id;

                    if (j > 0 && idM == previous)
                        throw new InvalidOperationException(
                            $"Action Id collision in {types[i].Type.Name} controller: {methods[j].Type.Name}, {methods[j - 1].Type.Name}");

                    for (int k = 0; k < idM - previous - 1; k++)
                    {
                        AddEmptyAction(idC);
                    }
                    AddAction(types[i].Type, methods[j].Type, idC, idM);
                    AddIfMenuAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfUnregisteredCommandAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfUnknownCallbackQueryAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    previous = idM;
                }
                previous = idC;
            }
            ValidateRequiredActions();
        }

        private void AddIfMainController(Type type, int methodId)
        {
            if (IsMainController(type))
            {
                if (mainControllerId == -1)
                    mainControllerId = methodId;
                else
                    throw new InvalidOperationException($"Main controller collision: {type.Name}, {controllerTypes[mainControllerId].Name}");
            }
        }

        private void AddIfMenuAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsMenuAction(methodInfo))
            {
                if (controllerId != mainControllerId)
                    throw new InvalidOperationException(
                        $"Menu action {methodInfo.Name} ({controllerTypes[controllerId].Name}) does not belong to main controller {controllerTypes[mainControllerId].Name}");
                if (menuActionId != -1)
                    throw new InvalidOperationException(
                        $"Menu action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == menuActionId)!.Name}");
                menuActionId = methodId;
            }
        }

        private void AddIfUnregisteredCommandAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsNoCommandAction(methodInfo))
            {
                if (controllerId != mainControllerId)
                    throw new InvalidOperationException(
                        $"No command action {methodInfo.Name} ({controllerTypes[controllerId].Name}) does not belong to main controller {controllerTypes[mainControllerId].Name}");
                if (unregisteredCommandActionId != -1)
                    throw new InvalidOperationException(
                        $"Unregistered command action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == menuActionId)!.Name}");
                unregisteredCommandActionId = methodId;
            }
        }

        private void AddIfUnknownCallbackQueryAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsUnknownCallbackQueryAction(methodInfo))
            {
                if (controllerId != mainControllerId)
                    throw new InvalidOperationException(
                        $"Unknown callback query action {methodInfo.Name} ({controllerTypes[controllerId].Name}) does not belong to main controller {controllerTypes[mainControllerId].Name}");
                if (unknownCallbackQueryActionId != -1)
                    throw new InvalidOperationException(
                        $"Unknown callback query action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == menuActionId)!.Name}");
                unknownCallbackQueryActionId = methodId;
            }
        }

        private void ValidateRequiredActions()
        {
            if (mainControllerId == -1)
                throw new InvalidOperationException("No main controller found in assembly");
            if (menuActionId == -1)
                throw new InvalidOperationException("No menu action found in assembly");
            if (unregisteredCommandActionId == -1)
                throw new InvalidOperationException("No unregistered command action found in assembly");
            if (unknownCallbackQueryActionId == -1)
                throw new InvalidOperationException("No unknown callback query action found in assembly");
        }

        private void AddBeforeFirstControllers(int id)
        {
            for(int i = 0; i < id; i++)
            {
                AddEmptyController();
            }
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

        private void AddBeforeFirstActions(int controllerId, int actionId)
        {
            for (int i = 0; i < actionId; i++)
            {
                AddEmptyAction(controllerId);
            }
        }
    }
}
