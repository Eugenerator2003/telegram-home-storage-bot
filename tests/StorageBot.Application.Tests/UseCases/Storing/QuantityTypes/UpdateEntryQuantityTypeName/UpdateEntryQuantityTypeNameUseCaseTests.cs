using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName
{
    [TestClass]
    public class UpdateEntryQuantityTypeNameUseCaseTests
    {
        private class UpdateEntryQuantityTypeNameUseCaseFactory
            : StorageUseCaseFactory<UpdateEntryQuantityTypeNameUseCase,
                                    UpdateEntryQuantityTypeNameInput,
                                    UpdateEntryQuantityTypeNameOutput>
        {
            public override UpdateEntryQuantityTypeNameUseCase CreateUseCase()
            {
                return new UpdateEntryQuantityTypeNameUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new UpdateEntryQuantityTypeNameInput(1, 1, 1, "2");
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new UpdateEntryQuantityTypeNameInput(1, 1, 1, "2");
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new UpdateEntryQuantityTypeNameInput(1, 1, 1, "2");
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new UpdateEntryQuantityTypeNameInput(1, 1, 1, "2");
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteNotFoundTest()
        {
            var unitName = "new-unit";
            var quantityTypeId = 30000;
            var targetOwnershipId = 18;
            var input = new UpdateEntryQuantityTypeNameInput(1, targetOwnershipId, quantityTypeId, unitName);
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.QuantityTypeNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteEmptyInputStringTest()
        {
            var oldUnitName = "old";
            var newUnitName = string.Empty;
            var quantityTypeId = 30000;
            var targetOwnershipId = 18;
            var input = new UpdateEntryQuantityTypeNameInput(1, targetOwnershipId, quantityTypeId, newUnitName);
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var quantityType = new EntryQuantityType(false, oldUnitName) { Id = quantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var oldUnitName = "old";
            var newUnitName = "new";
            var quantityTypeId = 30000;
            var targetOwnershipId = 18;
            var input = new UpdateEntryQuantityTypeNameInput(1, targetOwnershipId, quantityTypeId, newUnitName);
            var factory = new UpdateEntryQuantityTypeNameUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var quantityType = new EntryQuantityType(false, oldUnitName) { Id = quantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(newUnitName, quantityType.UnitName);
        }
    }
}
