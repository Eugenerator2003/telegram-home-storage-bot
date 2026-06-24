using StorageBot.Host.Controllers;
using StorageBot.Host.Routing.Attributes.Actions.Base;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    public class ControllerMapper : IHandlerInfoResolver, IIdNavigation
    {
        private readonly static Regex CommandRegex = new Regex(@"^[A-Za-z0-9_]+");

        private static int GetControllerId(Type controllerType)
        {
            var attribute = controllerType.GetCustomAttribute<ControllerIdAttribute>()
                ?? throw new InvalidOperationException($"Controller {controllerType.Name} must have an id attribute");
            if (attribute.Id < 0)
                throw new InvalidOperationException($"Controller id ({controllerType.Name}) must be non-negative");
            return attribute.Id;
        }

        private static int GetActionId(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<ActionIdAttribute>()
                ?? throw new InvalidOperationException($"Action method {methodInfo.Name} from {methodInfo.DeclaringType!.Name} must have an id attribute");
            if (attribute.Id < 0)
                throw new InvalidOperationException($"Action id ({methodInfo.Name} from {methodInfo.DeclaringType!.Name}) must be non-negative");
            return attribute.Id;
        }

        private static string? GetCommandName(MethodInfo methodInfo)
        {
            var attribute = methodInfo.GetCustomAttribute<CommandAttribute>();
            if (attribute is not null)
            {
                if (string.IsNullOrWhiteSpace(attribute.Name) || attribute.Name.Length > 32)
                    throw new InvalidOperationException(
                        $"Command name length must be between 1 and 32 characters ({methodInfo.Name} from {methodInfo.DeclaringType!.Name})");
                if (!CommandRegex.IsMatch(attribute.Name))
                    throw new InvalidOperationException(
                        $"Command name must contains only latin letters, numbers and underscores ({methodInfo.Name} from {methodInfo.DeclaringType!.Name})");
            }
            return attribute?.Name;
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

        private static bool IsUnexpectedMessageAction(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<UnexpectedMessageActionAttribute>() is not null;
        }

        private Type[] controllerTypes;
        private MethodInvoker[][] actionInvokers;

        private readonly Dictionary<string, int> controllerIds;
        private readonly Dictionary<string, Dictionary<string, int>> actionIds;
        private readonly Dictionary<string, (int, int)> commandIds;

        private int mainControllerId = -1;
        private int menuActionId = -1;
        private int unregisteredCommandActionId = -1;
        private int unknownCallbackQueryActionId = -1;
        private int unexpectedMessageActionId = -1;

        public ControllerMapper(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            controllerTypes = Array.Empty<Type>();
            actionInvokers = Array.Empty<MethodInvoker[]>();
            controllerIds = new Dictionary<string, int>();
            actionIds = new Dictionary<string, Dictionary<string, int>>();
            commandIds = new Dictionary<string, (int, int)>();
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

        public int GetControllerId<T>() where T : BotController
        {
            return GetControllerId(nameof(T));
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

        public int GetMainControllerId() => mainControllerId;

        public int GetMenuActionId() => menuActionId;

        public int GetUnregisteredCommandActionId() => unregisteredCommandActionId;

        public int GetUnknownCallbackQueryActionId() => unknownCallbackQueryActionId;

        public int GetUnexpectedMessageActionId() => unexpectedMessageActionId;

        public HandlerInfo GetHandlerInfo(int controllerId, int actionId)
        {
            if (controllerId < 0 || actionId < 0)
            {
                throw new ArgumentException("Controller and action IDs must be non-negative");
            }
            if (controllerId > controllerTypes.Length || 
                actionId > actionInvokers[controllerId].Length)
            {
                throw new ArgumentException("Controller or action ID is greater than allowed");
            }
            var controllerType = controllerTypes[controllerId]
                ?? throw new InvalidOperationException("Controller not found");
            var action = actionInvokers[controllerId][actionId]
                ?? throw new InvalidOperationException("Action not found");
            return new HandlerInfo(controllerType, action);
        }

        public HandlerInfo? GetHandlerInfoByCommand(string command)
        {
            if (command.StartsWith('/'))
            {
                command = command.Remove(0, 1);
            }
            if (commandIds.TryGetValue(command, out var id))
            {
                return new HandlerInfo(controllerTypes[id.Item1], actionInvokers[id.Item1][id.Item2]); 
            }
            return null;
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

            int typeCount = types[^1].Id + 1;
            controllerTypes = new Type[typeCount];
            actionInvokers = new MethodInvoker[typeCount][];
            int previous = 0;
            for (int i = 0; i < types.Length; i++)
            {
                var idC = types[i].Id;

                if (i > 0 && idC == previous)
                    throw new InvalidOperationException($"Controller Id collision: {types[i].Type.Name}, {types[i - 1].Type.Name}");

                AddController(types[i].Type, idC);
                AddIfMainController(types[i].Type, idC);

                var methods = types[i].Type
                    .GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance)
                    .Select(m => new { Type = m, Id = GetActionId(m) })
                    .OrderBy(m => m.Id)
                    .ToArray();

                if (methods.Length == 0)
                    throw new InvalidOperationException($"No methods found in controller {types[i].Type.Name}");

                previous = 0;
                actionInvokers[idC] = new MethodInvoker[methods.Length + 1];
                for (int j = 0; j < methods.Length; j++)
                {
                    if (methods[j].Type.ReturnType.IsInstanceOfType(taskType))
                        throw new InvalidOperationException($"Controller action {methods[j].Type.Name} ({types[i].Type.Name}) must return be async");

                    var idM = methods[j].Id;

                    if (j > 0 && idM == previous)
                        throw new InvalidOperationException(
                            $"Action Id collision in {types[i].Type.Name} controller: {methods[j].Type.Name}, {methods[j - 1].Type.Name}");

                    AddAction(types[i].Type, methods[j].Type, idC, idM);
                    AddIfMenuAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfUnregisteredCommandAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfUnknownCallbackQueryAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfUnexpectedMessageAction(methods[j].Type, methods[j].Id, idC, methods.Select(m => m.Type));
                    AddIfCommandAction(methods[j].Type, methods[j].Id, types[i].Id);
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

        private void AddIfCommandAction(MethodInfo methodInfo, int methodId, int controllerId)
        {
            var command = GetCommandName(methodInfo);
            if (command is not null)
            {
                if (!commandIds.TryAdd(command, (controllerId, methodId)))
                {
                    var existCommandId = commandIds[command];
                    throw new InvalidOperationException(
                        $"Command collision for actions {methodInfo.Name} ({methodInfo.DeclaringType!.Name}) and " +
                        $"{controllerTypes[existCommandId.Item1].GetMethods().First(m => GetActionId(methodInfo) == existCommandId.Item2)} ({controllerTypes[existCommandId.Item1]})");
                }
            }
        }

        private void AddIfUnexpectedMessageAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsUnexpectedMessageAction(methodInfo))
            {
                if (controllerId != mainControllerId)
                    throw new InvalidOperationException(
                        $"Unepxected message action {methodInfo.Name} ({controllerTypes[controllerId].Name}) does not belong to main controller {controllerTypes[mainControllerId].Name}");
                if (unknownCallbackQueryActionId != -1)
                    throw new InvalidOperationException(
                        $"Unepxected message action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == menuActionId)!.Name}");
                unexpectedMessageActionId = methodId;
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
            if (unexpectedMessageActionId == -1)
                throw new InvalidOperationException("No unexpected message action found in assembly");
        }

        private void AddController(Type type, int controllerId)
        {
            controllerTypes[controllerId] = type;
            controllerIds.Add(type.Name, controllerId);
            actionIds.Add(type.Name, new Dictionary<string, int>());
        }

        private void AddAction(Type type, MethodInfo method, int controllerId, int actionId)
        {
            var actionInvoker = MethodInvoker.Create(method);
            actionInvokers[controllerId][actionId] = actionInvoker;
            actionIds[type.Name].Add(method.Name, actionId);
        }
    }
}
