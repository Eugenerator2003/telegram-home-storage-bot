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
    public class RequiredNavigationCollectionTests
    {
        [TestMethod]
        public void InitializeWithAttributeTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            code.AddRequiredAttribute("attribute1");
            code.AddRequiredAttribute("attribute2");
            code.AddRequiredAttribute("attribute3");
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var collection = new RequiredNavigationCollection(assembly);
        }

        [TestMethod]
        public void ValidateRequiredBeforeAddTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            var attribute1Name = "attribute1";
            var attribute2Name = "attribute2";
            code.AddRequiredAttribute(attribute1Name);
            code.AddRequiredAttribute(attribute2Name);
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var collection = new RequiredNavigationCollection(assembly);
            var result = Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                collection.ValidateRequired();
            });
            Assert.Contains("attribute1", result.Message);
        }

        [TestMethod]
        public void ValidateRequiredAfterAddTest()
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
            var collection = new RequiredNavigationCollection(assembly);
            collection.AddIfRequired(assembly.GetType(controllerName)!.GetMethod(methods[0])!, controllerId, methodIds[0]);
            var result = Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                collection.ValidateRequired();
            });
            Assert.Contains("attribute2", result.Message);
        }

        [TestMethod]
        public void ValidateRequiredAfterAllAddTest()
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
            string?[] requiredAttributes = [attribute1Name, attribute2Name];
            code.AddController(controllerName, controllerId, methods, methodIds, requiredAttributes);

            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var collection = new RequiredNavigationCollection(assembly);
            var methodInfos = assembly.GetType(controllerName)!.GetMethods();
            collection.AddIfRequired(methodInfos.First(m => m.Name == methods[0]), controllerId, methodIds[0]);
            collection.AddIfRequired(methodInfos.First(m => m.Name == methods[1]), controllerId, methodIds[1]);
            collection.ValidateRequired();
        }

        [TestMethod]
        public void GetRequiredActionTest()
        {
            var assembly = typeof(RequiredNavigationCollectionTests).Assembly;
            var controllerType = typeof(TestController1);
            var controllerId = controllerType.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var action = controllerType.GetMethod("Method1")!;
            var actionId = action.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var collection = new RequiredNavigationCollection(assembly);
            collection.AddIfRequired(action, controllerId, actionId);
            var result = collection.GetRequiredAction<TestRequiredAttribute1>();
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
            var assemblyGenerator = new AssemblyGenerator();
            var assembly = assemblyGenerator.Generate(code.ToString());
            var collection = new RequiredNavigationCollection(assembly);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var result = collection.GetRequiredAction<TestRequiredAttribute1>();
            });
        }

        [TestMethod]
        public void GetRequiredActionNotFoundTest()
        {
            var assembly = typeof(RequiredNavigationCollectionTests).Assembly;
            var controllerType = typeof(TestController1);
            var controllerId = controllerType.GetCustomAttribute<ControllerIdAttribute>()!.Id;
            var action = controllerType.GetMethod("Method1")!;
            var actionId = action.GetCustomAttribute<ActionIdAttribute>()!.Id;
            var collection = new RequiredNavigationCollection(assembly);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                var result = collection.GetRequiredAction<TestRequiredAttribute1>();
            });
        }

        [TestMethod]
        public void AddIfRequiredCollisionTest()
        {
            var code = CodeGenerator.GetBaseUsing();
            var attribute = "attribute1";
            var controller1 = "controller1";
            var controllerId1 = 1;
            var method1 = "method1";
            var method1Id = 1;
            var controller2 = "controller2";
            var controller2Id = 2;
            var method2 = "method2";
            var method2Id = 2;
            code.AddRequiredAttribute(attribute)
                .AddController(controller1, controllerId1, [method1], [method1Id], [attribute])
                .AddController(controller2, controller2Id, [method2], [method2Id], [attribute]);
            var assembly = new AssemblyGenerator().Generate(code.ToString());
            var collection = new RequiredNavigationCollection(assembly);
            var methodInfo1 = assembly.GetType(controller1)!.GetMethod(method1)!;
            var methodInfo2 = assembly.GetType(controller2)!.GetMethod(method2)!;
            collection.AddIfRequired(methodInfo1, controllerId1, method1Id);
            Assert.ThrowsExactly<InvalidOperationException>(() =>
            {
                collection.AddIfRequired(methodInfo2, controller2Id, method2Id);
            });
        }
    }
}
