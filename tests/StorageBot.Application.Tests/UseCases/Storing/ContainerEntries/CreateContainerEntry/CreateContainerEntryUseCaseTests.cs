using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing.ContainerEntries;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    [TestClass]
    public class CreateContainerEntryUseCaseTests
    {
        private class CreateContainerEntryUseCaseFactory
            : ContainerEntryUseCaseFactory<CreateContainerEntryUseCase, CreateContainerEntryInput, CreateContainerEntryOutput>
        {
            public override CreateContainerEntryUseCase CreateUseCase()
            {
                return new CreateContainerEntryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ContainerNotFoundTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            UseCaseTester.ContainerNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteEntryQuantityTypeNotFoundTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.QuantityTypeNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteEmptyInputStringTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            var entry = context.TargetContainer.Entries.Single();
            Assert.AreEqual(name, entry.Name);
            Assert.AreEqual(1, entry.EntryQuantity.Value);
            Assert.AreEqual(quantityType, entry.EntryQuantity.QuantityType);
        }
    }
}
