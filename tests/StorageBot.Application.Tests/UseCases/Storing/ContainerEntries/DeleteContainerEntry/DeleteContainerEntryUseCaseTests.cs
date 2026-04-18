using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing.ContainerEntries;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.DeleteContainerEntry
{
    [TestClass]
    public class DeleteContainerEntryUseCaseTests
    {
        private class DeleteContainerEntryUseCaseFactory
            : ContainerEntryUseCaseFactory<DeleteContainerEntryUseCase, DeleteContainerEntryInput, DeleteContainerEntryOutput>
        {
            public override DeleteContainerEntryUseCase CreateUseCase()
            {
                return new DeleteContainerEntryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ContainerNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            UseCaseTester.ContainerNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteContainerEntryNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.ContainerEntryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var entryId = 10;
            var input = new DeleteContainerEntryInput(1, targetOwnershipId, containerId, entryId);
            var factory = new DeleteContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var containerEntry = new ContainerEntry(
                "entry",
                new EntryQuantity(
                    new EntryQuantityType(isInteger: false, "unit"),
                    3130
                ))
            { Id = entryId };
            context.TargetContainer.AddEntry(containerEntry);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
        }
    }
}
