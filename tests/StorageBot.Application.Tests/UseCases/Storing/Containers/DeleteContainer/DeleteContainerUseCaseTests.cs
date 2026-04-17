using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers.DeleteContainer;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.DeleteContainer
{
    [TestClass]
    public class DeleteContainerUseCaseTests
    {
        private class DeleteContainerUseCaseFactory
            : StorageUseCaseFactory<DeleteContainerUseCase, DeleteContainerInput, DeleteContainerOutput>
        {
            public override DeleteContainerUseCase CreateUseCase()
            {
                return new DeleteContainerUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipFoundTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteContainerNotFoundTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId); 
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteStorageContainerTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("container") { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, context.TargetOwnership.PersonalStorage.Containers);
        }

        [TestMethod]
        public void ExecuteInnerContainerTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var outerContainer = new Container("outerContainer");
            var container = new Container("container", outerContainer) { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(outerContainer);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(0, outerContainer.InnerContainers);
        }

        [TestMethod]
        public void ExecuteHasInnerContainerTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var factory = new DeleteContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var outerContainer = new Container("outerContainer") { Id = containerId };
            var container = new Container("container", outerContainer) { Id = 9990 };
            context.TargetOwnership.PersonalStorage.AddContainer(outerContainer);
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageContainerHasInnerContainers"], output.Message);
        }

        [TestMethod]
        public void ExecuteContainerNotEmptyTest()
        {
            var targetOwnershipId = 30;
            var containerId = 390;
            var input = new DeleteContainerInput(1, targetOwnershipId, containerId);
            var factory = new DeleteContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("container") { Id = containerId };
            var quantityType = new EntryQuantityType(false, "name");
            container.AddEntry(new ContainerEntry("entry", new EntryQuantity(quantityType)));
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageContainerNotEmpty"], output.Message);
        }
    }
}
