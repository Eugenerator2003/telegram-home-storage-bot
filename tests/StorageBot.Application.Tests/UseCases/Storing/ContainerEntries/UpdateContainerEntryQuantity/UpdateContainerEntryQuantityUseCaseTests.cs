using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing.ContainerEntries;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity
{
    [TestClass]
    public class UpdateContainerEntryQuantityUseCaseTests
    {
        private class UpdateContainerEntryQuantityUseCaseFactory
            : ContainerEntryUseCaseFactory<
                UpdateContainerEntryQuantityUseCase, 
                UpdateContainerEntryQuantityInput, 
                UpdateContainerEntryQuantityOutput>
        {
            public override UpdateContainerEntryQuantityUseCase CreateUseCase()
            {
                return new UpdateContainerEntryQuantityUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ContainerNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            UseCaseTester.ContainerNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteContainerEntryNotFoundTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.ContainerEntryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteNotPositiveQuantityValueTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = -10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "unit");
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var entry = new ContainerEntry("entry", new EntryQuantity(quantityType)) { Id = containerEntryId };
            context.TargetContainer.AddEntry(entry);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.NotPositiveQuantityValue"], output.Message);
        }

        [TestMethod]
        public void ExecuteNotIntegerQuantityValueTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10.3m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(true, "unit");
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var entry = new ContainerEntry("entry", new EntryQuantity(quantityType)) { Id = containerEntryId };
            context.TargetContainer.AddEntry(entry);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.NotIntegerQuantityValue"], output.Message);
        }

        [TestMethod]
        public void ExecuteIntegerTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 10;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(true, "unit");
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var entry = new ContainerEntry("entry", new EntryQuantity(quantityType)) { Id = containerEntryId };
            context.TargetContainer.AddEntry(entry);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(value, entry.EntryQuantity.Value);
        }

        [TestMethod]
        public void ExecuteRealTest()
        {
            var targetOwnershipId = 2;
            var containerId = 3;
            var containerEntryId = 4;
            var value = 3993.66m;
            var input = new UpdateContainerEntryQuantityInput(1, targetOwnershipId, containerId, containerEntryId, value);
            var factory = new UpdateContainerEntryQuantityUseCaseFactory();
            var context = factory.ConfigureWithTargetContainer(targetOwnershipId, containerId);
            var quantityType = new EntryQuantityType(false, "unit");
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var entry = new ContainerEntry("entry", new EntryQuantity(quantityType)) { Id = containerEntryId };
            context.TargetContainer.AddEntry(entry);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(value, entry.EntryQuantity.Value);
        }
    }
}
