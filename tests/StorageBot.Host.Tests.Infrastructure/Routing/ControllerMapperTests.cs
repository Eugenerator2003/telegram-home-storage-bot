using StorageBot.Host.Controllers;
using StorageBot.Host.Routing;
using StorageBot.Host.Routing.Attributes.Actions.Updates;
using StorageBot.Host.Routing.Attributes.Controllers;
using StorageBot.Host.Tests.Infrastructure.TestAttributes;
using StorageBot.Host.Tests.Infrastructure.TestControllers;
using StorageBot.Host.Tests.Infrastructure.TestUtilities;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Routing
{
    [TestClass]
    public class ControllerMapperTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
        }

        [TestMethod]
        public void InitializeControllersNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeControllerIdCollisionTest()
        {
            var collisionId = 1;
            var code = CodeGenerator
                .GetBaseUsing()
                .AddController("controller1", collisionId, ["method1"], [1], [null])
                .AddController("controller2", collisionId, ["method1"], [2], [null]);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeNotAsyncActionTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            code.Append("""
                [ControllerId(3)]
                public class Controller : BotController 
                {
                    public void Action() 
                    {
                        
                    }
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeControllerMethodsNotFoundTest()
        {
            var collisionId = 1;
            var code = CodeGenerator
                .GetBaseUsing()
                .AddController("controller1", collisionId, [], [], []);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeActionIdCollisionTest()
        {
            var collisionId = 1;
            var code = CodeGenerator
                .GetBaseUsing()
                .AddController("controller1", 1, ["method1", "method2"], [collisionId, collisionId], [null, null]);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeValidateFirstRequiredNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            var attribute1Name = "attribute1";
            var attribute2Name = "attribute2";
            code.AddRequiredAttribute(attribute1Name);
            code.AddRequiredAttribute(attribute2Name);
            var controllerId = 1;
            var controllerName = "controller1";
            string[] methods = ["method1", "method2"];
            int[] methodIds = [1, 2];
            string?[] requiredAttributes = [null, null];
            code.AddController(controllerName, controllerId, methods, methodIds, requiredAttributes);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var result = Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
            Assert.Contains("attribute1", result.Message);
        }

        [TestMethod]
        public void InitializeValidateSecondRequiredNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            var attribute1Name = "attribute1";
            var attribute2Name = "attribute2";
            code.AddRequiredAttribute(attribute1Name);
            code.AddRequiredAttribute(attribute2Name);
            var controllerId = 1;
            var controllerName = "controller1";
            string[] methods = ["method1", "method2"];
            int[] methodIds = [1, 2];
            string?[] requiredAttributes = [attribute1Name, null];
            code.AddController(controllerName, controllerId, methods, methodIds, requiredAttributes);

            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var result = Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
            Assert.Contains("attribute2", result.Message);
        }

        [TestMethod]
        public void InitializeControllerIdAttributeNotFoundTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                public class Controller : BotController 
                {
                    [ActionId(3)]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeNegativeControllerIdTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(-30)]
                public class Controller : BotController 
                {
                    [ActionId(3)]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeActionIdAttributeNotFoundTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(30)]
                public class Controller : BotController 
                {
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeActionNegativeIdTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(30)]
                public class Controller : BotController 
                {
                    [ActionId(-10)]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeWhiteSpaceCommandTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(1)]
                public class Controller : BotController 
                {
                    [ActionId(1)]
                    [Command("            ")]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeEmptyCommandTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(1)]
                public class Controller : BotController 
                {
                    [ActionId(1)]
                    [Command("")]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeNullCommandTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(1)]
                public class Controller : BotController 
                {
                    [ActionId(1)]
                    [Command(null)]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void InitializeLargeCommandNameTest()
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append("""
                [ControllerId(1)]
                public class Controller : BotController 
                {
                    [ActionId(1)]
                    [Command("extra_large_command_with_many_symbols")]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        [DataRow("/slash")]
        [DataRow("command!")]
        [DataRow("кулебяка")]
        public void InitializeInvalidCommandNameTest(string commandName)
        {
            var code = CodeGenerator
                .GetBaseUsing();
            code.Append(
                $$"""
                [ControllerId(1)]
                public class Controller : BotController 
                {
                    [ActionId(1)]
                    [Command("{{commandName}}")]
                    public async Task Method() {}
                }
                """);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void GetActionTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method1";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var result = mapper.GetAction<TestController1>(methodName);
            Assert.AreEqual(controllerId, result.ControllerId);
            Assert.AreEqual(methodId, result.ActionId);
        }

        [TestMethod]
        public void GetActionControllerNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing()
                                    .AddRequiredAttribute("attribute1")
                                    .AddController("controller1", 1, ["method1"], [1], ["attribute1"]);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method1";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var result = mapper.GetAction<TestController1>("Method1");
            });
        }

        [TestMethod]
        public void GetActionNotFoundTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method1";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var result = mapper.GetAction<TestController1>(methodName + "2");
            });
        }

        [TestMethod]
        public void GetHandlerInfoNegativeControllerIdTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var controllerId = -1;
            var actionId = 2;
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoNegativeActionIdTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var controllerId = 3;
            var actionId = -2;
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoControllerIdGreaterThanAllowedTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var controllerId = assembly.GetTypes()
                .Where(t => !t.IsAbstract && t.IsAssignableTo(typeof(BaseBotController)))
                .Select(t => t.GetCustomAttribute<ControllerIdAttribute>()!.Id)
                .Max() + 1;
            var actionId = 1;
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoActionIdGreaterThanAllowedTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var actionId = type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Select(m => m.GetCustomAttribute<ActionIdAttribute>()!.Id)
                .Max() + 1;
            Assert.ThrowsExactly<ArgumentException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoActionControllerNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing()
                                    .AddRequiredAttribute("attribute1")
                                    .AddController("controller1", 1, ["method1"], [1], ["attribute1"])
                                    .AddController("controller2", 5, ["method1"], [4], [null]);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var mapper = new ControllerMapper(assembly);
            var type = assembly.GetType("controller2")!;
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id - 1;
            var actionId = type.GetMethod("method1")!.GetCustomAttribute<ActionIdAttribute>()!.Id;
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoActionNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing()
                                    .AddRequiredAttribute("attribute1")
                                    .AddController("controller1", 1, ["method1"], [1], ["attribute1"])
                                    .AddController("controller2", 5, ["method1"], [4], [null]);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var mapper = new ControllerMapper(assembly);
            var type = assembly.GetType("controller2")!;
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var actionId = type.GetMethod("method1")!.GetCustomAttribute<ActionIdAttribute>()!.Id - 1;
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                mapper.GetHandlerInfo(controllerId, actionId);
            });
        }

        [TestMethod]
        public void GetHandlerInfoTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method1";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var result = mapper.GetHandlerInfo(controllerId, methodId);
            var invokerMethod = result.ActionInvoker
                .GetType()
                .GetField("_method", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(result.ActionInvoker);
            Assert.AreEqual(type, result.ControllerType);
            Assert.AreEqual(method, invokerMethod);
        }

        [TestMethod]
        public void GetHandlerByCommandTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method2";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var command = "method2";
            var result = mapper.GetHandlerInfoByCommand(command)!;
            var invokerMethod = result.ActionInvoker
                .GetType()
                .GetField("_method", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(result.ActionInvoker);
            Assert.AreEqual(type, result.ControllerType);
            Assert.AreEqual(method, invokerMethod);
        }

        [TestMethod]
        public void GetHandlerByCommandWithSlashTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var controllerId = type.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var methodName = "Method2";
            var method = type.GetMethod(methodName)!;
            var methodId = method.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var command = '/' + "method2";
            var result = mapper.GetHandlerInfoByCommand(command)!;
            var invokerMethod = result.ActionInvoker
                .GetType()
                .GetField("_method", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(result.ActionInvoker);
            Assert.AreEqual(type, result.ControllerType);
            Assert.AreEqual(method, invokerMethod);
        }

        [TestMethod]
        public void GetHandlerByCommandNotFoundTest()
        {
            var assembly = typeof(ControllerMapperTests).Assembly;
            var mapper = new ControllerMapper(assembly);
            var type = typeof(TestController1);
            var command = "Method654";
            var result = mapper.GetHandlerInfoByCommand(command)!;
            Assert.IsNull(result);
        }

        [TestMethod]
        public void GetRequiredActionTest()
        {
            var assembly = typeof(RequiredNavigationCollectionTests).Assembly;
            var controllerType = typeof(TestController1);
            var controllerId = controllerType.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var action = controllerType.GetMethod("Method1")!;
            var actionId = action.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var mapper = new ControllerMapper(assembly);
            var result = mapper.GetRequiredAction<TestRequiredAttribute1>();
            Assert.AreEqual(controllerId, result.ControllerId);
            Assert.AreEqual(actionId, result.ActionId);
        }

        [TestMethod]
        public void GetRequiredActionAttributeNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            code.AddRequiredAttribute("attribute1");
            code.AddRequiredAttribute("attribute2");
            code.AddRequiredAttribute("attribute3");
            code.AddController(
                "controller",
                0,
                ["method1"],
                [0],
                [null]);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var mapper = new ControllerMapper(assembly);
            });
        }

        [TestMethod]
        public void GetRequiredActionNotFoundTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            code.AddRequiredAttribute("attribute1");
            code.AddController(
                "controller",
                0,
                ["method1"],
                [0],
                ["attribute1"]);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var controllerType = typeof(TestController1);
            var controllerId = controllerType.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var action = controllerType.GetMethod("Method1")!;
            var actionId = action.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var mapper = new ControllerMapper(assembly);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var result = mapper.GetRequiredAction<TestRequiredAttribute1>();
            });
        }
    }
}
