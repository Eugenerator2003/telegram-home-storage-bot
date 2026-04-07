using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.CreateEntryQuantityType
{
    [TestClass]
    public class CreateEntryQuantityTypeUseCaseTests
    {
        private class CreateEntryQuantityTypeUseCaseFactory
            : StorageUseCaseFactory<CreateEntryQuantityTypeUseCase, CreateEntryQuantityTypeInput, CreateEntryQuantityTypeOutput>
        {
            public override CreateEntryQuantityTypeUseCase CreateUseCase()
            {
                return new CreateEntryQuantityTypeUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new CreateEntryQuantityTypeInput(1, 1, false, "name");
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new CreateEntryQuantityTypeInput(1, 1, false, "name");
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new CreateEntryQuantityTypeInput(1, 1, false, "name");
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new CreateEntryQuantityTypeInput(1, 1, false, "name");
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithUserOwnership();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteEmptyStringTest()
        {
            var targetOwnershipId = 23;
            var input = new CreateEntryQuantityTypeInput(2, targetOwnershipId, false, string.Empty);
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EmptyInputString"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var targetOwnershipId = 23;
            var isInteger = false;
            var unitName = "unit";
            var input = new CreateEntryQuantityTypeInput(2, targetOwnershipId, isInteger, unitName);
            var factory = new CreateEntryQuantityTypeUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            Assert.HasCount(0, context.TargetOwnership.PersonalStorage.EntryQuantityTypes);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.HasCount(1, context.TargetOwnership.PersonalStorage.EntryQuantityTypes);
            var quantityType = context.TargetOwnership.PersonalStorage.EntryQuantityTypes.First();
            Assert.AreEqual(isInteger, quantityType.IsInteger);
            Assert.AreEqual(unitName, quantityType.UnitName);
        }
    }
}
