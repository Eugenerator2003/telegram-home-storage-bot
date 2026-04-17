using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.UpdateContainerName
{
    [TestClass]
    public class UpdateContainerNameUseCaseTests
    {
        private class UpdateContainerNameUseCaseFactory
            : StorageUseCaseFactory<UpdateContainerNameUseCase, UpdateContainerNameInput, UpdateContainerNameOutput>
        {
            public override UpdateContainerNameUseCase CreateUseCase()
            {
                return new UpdateContainerNameUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "0999";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "0999";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipFoundTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "0999";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "0999";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteContainerNotFoundTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "0999";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetStorageId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteEmptyInputStringTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = string.Empty;
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetStorageId);
            var container = new Container("container") { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var targetStorageId = 30;
            var containerId = 99;
            var containerName = "new-container-name";
            var input = new UpdateContainerNameInput(1, targetStorageId, containerId, containerName);
            var factory = new UpdateContainerNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetStorageId);
            var container = new Container("container") { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(containerName, container.Name);
        }
    }
}
