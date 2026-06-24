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
    public class ControllerMapper : IHandlerInfoResolver, INavigation
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
        private readonly Dictionary<string, NavigationInfo> commandIds;

        private NavigationInfo? menuAction;
        private NavigationInfo? unregisteredCommandAction;
        private NavigationInfo? unknownCallbackQueryAction;
        private NavigationInfo? unexpectedMessageAction;

        public ControllerMapper(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            controllerTypes = Array.Empty<Type>();
            actionInvokers = Array.Empty<MethodInvoker[]>();
            controllerIds = new Dictionary<string, int>();
            actionIds = new Dictionary<string, Dictionary<string, int>>();
            commandIds = new Dictionary<string, NavigationInfo>();
            Initialize(assembly);
        }

        public NavigationInfo GetAction<T>(string actionName) where T : BotController
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(actionName);
            if (actionIds.TryGetValue(typeof(T).Name, out var methods))
            {
                if (methods.TryGetValue(actionName, out int id))
                {
                    return new NavigationInfo(controllerIds[actionName], id); 
                }
                else
                {
                    throw new InvalidOperationException($"Action {actionName} ({nameof(T)}) not found");
                }
            }
            else
            {
                throw new InvalidOperationException($"Controller {nameof(T)} not found");
            }
        }

        public NavigationInfo GetMenuAction() => menuAction!;

        public NavigationInfo GetUnregisteredCommandAction() => unexpectedMessageAction!;

        public NavigationInfo GetUnknownCallbackQueryAction() => unknownCallbackQueryAction!;

        public NavigationInfo GetUnexpectedMessageAction() => unexpectedMessageAction!;

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
                return new HandlerInfo(controllerTypes[id.ControllerId], actionInvokers[id.ControllerId][id.ActionId]); 
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

        private void AddIfMenuAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsMenuAction(methodInfo))
            {
                if (menuAction != null)
                    throw new InvalidOperationException(
                        $"Menu action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == menuAction.ActionId)!.Name}");
                menuAction = new NavigationInfo(controllerId, methodId);
            }
        }

        private void AddIfUnregisteredCommandAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsNoCommandAction(methodInfo))
            {
                if (unregisteredCommandAction != null)
                    throw new InvalidOperationException(
                        $"Unregistered command action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == unregisteredCommandAction.ControllerId)!.Name}");
                unregisteredCommandAction = new NavigationInfo(controllerId, methodId);
            }
        }

        private void AddIfUnknownCallbackQueryAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsUnknownCallbackQueryAction(methodInfo))
            {
                if (unknownCallbackQueryAction != null)
                    throw new InvalidOperationException(
                        $"Unknown callback query action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == unknownCallbackQueryAction.ControllerId)!.Name}");
                unknownCallbackQueryAction = new NavigationInfo(controllerId, methodId);
            }
        }

        private void AddIfCommandAction(MethodInfo methodInfo, int methodId, int controllerId)
        {
            var command = GetCommandName(methodInfo);
            if (command is not null)
            {
                if (!commandIds.TryAdd(command, new NavigationInfo(controllerId, methodId)))
                {
                    var existCommandInfo = commandIds[command];
                    throw new InvalidOperationException(
                        $"Command collision for actions {methodInfo.Name} ({methodInfo.DeclaringType!.Name}) and " +
                        $"{controllerTypes[existCommandInfo.ControllerId].GetMethods().First(m => GetActionId(methodInfo) == existCommandInfo.ActionId).Name} " +
                        $"({controllerTypes[existCommandInfo.ControllerId].Name})");
                }
            }
        }

        private void AddIfUnexpectedMessageAction(MethodInfo methodInfo, int methodId, int controllerId, IEnumerable<MethodInfo> controllerActions)
        {
            if (IsUnexpectedMessageAction(methodInfo))
            {
                if (unexpectedMessageAction != null)
                    throw new InvalidOperationException(
                        $"Unepxected message action collision in {controllerTypes[controllerId].Name} controller: {methodInfo.Name}, " +
                        $"{controllerActions.FirstOrDefault(a => GetActionId(methodInfo) == unexpectedMessageAction.ActionId)!.Name}");
                unexpectedMessageAction = new NavigationInfo(controllerId, methodId);
            }
        }

        private void ValidateRequiredActions()
        {
            if (menuAction == null)
                throw new InvalidOperationException("No menu action found in assembly");
            if (unregisteredCommandAction == null)
                throw new InvalidOperationException("No unregistered command action found in assembly");
            if (unknownCallbackQueryAction == null)
                throw new InvalidOperationException("No unknown callback query action found in assembly");
            if (unexpectedMessageAction == null)
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
