using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    [TestClass]
    public class DeleteEntryQuantityTypeUseCaseTests
    {
        private class DeleteEntryQuantityTypeUseCaseFactory
            : StorageUseCaseFactory<DeleteEntryQuantityTypeUseCase, DeleteEntryQuantityTypeInput, DeleteEntryQuantityTypeOutput>
        {
            public override DeleteEntryQuantityTypeUseCase CreateUseCase()
            {
                return new DeleteEntryQuantityTypeUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new DeleteEntryQuantityTypeInput(1, 1, 6);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new DeleteEntryQuantityTypeInput(1, 1, 6);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new DeleteEntryQuantityTypeInput(1, 1, 6);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new DeleteEntryQuantityTypeInput(1, 1, 6);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var targetOwnershipId = 99;
            var quantityTypeId = 20;
            var input = new DeleteEntryQuantityTypeInput(1, targetOwnershipId, quantityTypeId);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var storage = context.TargetOwnership.PersonalStorage;
            var type1 = new EntryQuantityType(false, "unit1") { Id = 1 };
            var type2 = new EntryQuantityType(false, "unit2") { Id = quantityTypeId };
            var type3 = new EntryQuantityType(true, "unit54") { Id = 3 };
            storage.AddEntryQuantityType(type1);
            storage.AddEntryQuantityType(type2);
            storage.AddEntryQuantityType(type3);
            Assert.HasCount(3, storage.EntryQuantityTypes);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(2, storage.EntryQuantityTypes);
            Assert.IsNull(storage.EntryQuantityTypes.FirstOrDefault(t => t.Id == quantityTypeId));
        }

        [TestMethod]
        public void ExecuteNotFoundTest()
        {
            var targetOwnershipId = 99;
            var quantityTypeId = 20;
            var input = new DeleteEntryQuantityTypeInput(1, targetOwnershipId, quantityTypeId);
            var factory = new DeleteEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.QuantityTypeNotFound"], output.Message);
        }
    }
}
