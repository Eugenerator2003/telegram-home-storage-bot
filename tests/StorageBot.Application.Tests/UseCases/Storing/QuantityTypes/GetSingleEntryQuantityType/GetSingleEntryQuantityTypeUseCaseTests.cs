using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    [TestClass]
    public class GetSingleEntryQuantityTypeUseCaseTests
    {
        private class GetSingleEntryQuantityTypeUseCaseFactory :
            StorageUseCaseFactory<GetSingleEntryQuantityTypeUseCase, GetSingleEntryQuantityTypeInput, GetSingleEntryQuantityTypeOutput>
        {
            public override GetSingleEntryQuantityTypeUseCase CreateUseCase()
            {
                return new GetSingleEntryQuantityTypeUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetSingleEntryQuantityTypeInput(1, 2, 3);
            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetSingleEntryQuantityTypeInput(1, 2, 3);
            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetSingleEntryQuantityTypeInput(1, 2, 3);
            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new GetSingleEntryQuantityTypeInput(1, 2, 3);
            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var userId = 30;
            var targetOwnershipId = 10; 
            var quantityTypeId = 50;
            var input = new GetSingleEntryQuantityTypeInput(userId, targetOwnershipId, quantityTypeId);

            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var quantityType = new EntryQuantityType(false, "type_type") { Id = quantityTypeId };
            context.TargetOwnership.PersonalStorage.AddEntryQuantityType(quantityType);

            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(quantityType.Id, output.QuantityTypeDTO.Id);
            Assert.AreEqual(quantityType.UnitName, output.QuantityTypeDTO.UnitName);
            Assert.AreEqual(quantityType.IsInteger, output.QuantityTypeDTO.IsInteger);
        }

        [TestMethod]
        public void ExecuteFailTest()
        {
            var userId = 30;
            var targetOwnershipId = 10;
            var quantityTypeId = 50;
            var input = new GetSingleEntryQuantityTypeInput(userId, targetOwnershipId, quantityTypeId);

            var factory = new GetSingleEntryQuantityTypeUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);

            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.QuantityTypeNotFound"], output.Message);
        }
    }
}
