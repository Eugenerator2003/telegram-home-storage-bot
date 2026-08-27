using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.EntryCategories;
using StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    [TestClass]
    public class GetSingleEntryCategoryUseCaseTests
    {
        private class GetSingleEntryCategoryUseCaseFactory :
            StorageUseCaseFactory<GetSingleEntryCategoryUseCase, GetSingleEntryCategoryInput, GetSingleEntryCategoryOutput>
        {
            public override GetSingleEntryCategoryUseCase CreateUseCase()
            {
                return new GetSingleEntryCategoryUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetSingleEntryCategoryInput(76, 322, 17);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetSingleEntryCategoryInput(76, 322, 17);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetSingleEntryCategoryInput(76, 322, 17);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new GetSingleEntryCategoryInput(76, 322, 17);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteEntryCategoryNotFoundTest()
        {
            var targetOwnershipId = 322;
            var input = new GetSingleEntryCategoryInput(76, targetOwnershipId, 17);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.EntryCategoryNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var categoryId = 17;
            var targetOwnershipId = 322;
            var input = new GetSingleEntryCategoryInput(76, targetOwnershipId, categoryId);
            var factory = new GetSingleEntryCategoryUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var category = new EntryCategory("categoooooory") { Id = categoryId };
            context.TargetOwnership.PersonalStorage.AddEntryCategory(category);
            var output = factory.CreateUseCase().Execute(input);
            var dto = new EntryCategoryDTO(category.Id, category.Name);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(dto, output.EntryCategory);
        }
    }
}
