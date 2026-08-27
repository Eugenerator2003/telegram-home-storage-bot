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
            var quantityValue = 30.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 29.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 11.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 23.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 10.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 1.4m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
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
            var quantityValue = 50m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteNotPositiveValueTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "name1";
            var quantityValue = -1m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(CreateContainerEntryOutput.InputFailState.NotPositiveValue, output.State);
            Assert.AreEqual(factory.Localizer["Case.NotPositiveQuantityValue"], output.Message);
        }

        [TestMethod]
        public void ExecuteNotIntegerValueTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "name2";
            var quantityValue = 33.3m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(true, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(CreateContainerEntryOutput.InputFailState.NotIntegerValue, output.State);
            Assert.AreEqual(factory.Localizer["Case.NotIntegerQuantityValue"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var targetOwnershipId = 1;
            var containerId = 2;
            var entryQuantityTypeId = 3;
            var name = "entry";
            var quantityValue = 1.0m;
            var input = new CreateContainerEntryInput(1, targetOwnershipId, containerId, entryQuantityTypeId, name, quantityValue);
            var factory = new CreateContainerEntryUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "quantity") { Id = entryQuantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(CreateContainerEntryOutput.InputFailState.None, output.State);
            Assert.AreEqual(string.Empty, output.Message);
            var entry = context.TargetContainer.Entries.Single();
            Assert.AreEqual(name, entry.Name);
            Assert.AreEqual(1, entry.EntryQuantity.Value);
            Assert.AreEqual(quantityType, entry.EntryQuantity.QuantityType);
        }
    }
}
