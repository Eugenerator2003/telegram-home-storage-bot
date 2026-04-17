using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers.CreateContainer;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.CreateContainer
{
    [TestClass]
    public class CreateContainerUseCaseTests
    {
        private class CreateContainerUseCaseFactory
            : StorageUseCaseFactory<CreateContainerUseCase, CreateContainerInput, CreateContainerOutput>
        {
            public override CreateContainerUseCase CreateUseCase()
            {
                return new CreateContainerUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteInputStringTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = string.Empty;
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteOuterContainerNotFoundTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = 30;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.OuterContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteNoOuterContainerTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = null;
            var name = "container";
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var factory = new CreateContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            var container = context.TargetOwnership.PersonalStorage.Containers.Single();
            Assert.AreEqual(name, container.Name);
        }

        [TestMethod]
        public void ExecuteWithOuterContainerTest()
        {
            var targetOwnershipId = 30;
            int? outerContainerId = 30;
            var name = "container";
            var factory = new CreateContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var outerContainer = new Container("outer-container") { Id = outerContainerId.Value };
            context.TargetOwnership.PersonalStorage.AddContainer(outerContainer);
            var input = new CreateContainerInput(1, targetOwnershipId, outerContainerId, name);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            var container = outerContainer.InnerContainers.Single();
            Assert.AreEqual(name, container.Name);
        }
    }
}
