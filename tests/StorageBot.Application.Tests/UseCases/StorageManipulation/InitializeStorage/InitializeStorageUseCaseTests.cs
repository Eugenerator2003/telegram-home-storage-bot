using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.InitializeStorage;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.InitializeStorage
{
    [TestClass]
    public class InitializeStorageUseCaseTests
    {
        private class InitializeStorageUseCaseFactory
            : UserUseCaseFactory<InitializeStorageUseCase, InitializeStorageInput, InitializeStorageOutput>
        {
            public UserOwnershipRepository OwnershipRepository { get; set; } = new Mock<UserOwnershipRepository>().Object;

            public override InitializeStorageUseCase CreateUseCase()
            {
                return new InitializeStorageUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var factory = new InitializeStorageUseCaseFactory();
            var input = new InitializeStorageInput(10);
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteUserAlreadyHaveOwnershipTest()
        {
            var factory = new InitializeStorageUseCaseFactory();
            var factoryContext = factory.ConfigureWithUser();

            var userOwnership = new UserOwnership(factoryContext.UserMock.Object);
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(m => m.GetByUser(factoryContext.UserMock.Object))
                                       .Returns(userOwnership);
            factory.OwnershipRepository = userOwnershipRepositoryMock.Object;

            var input = new InitializeStorageInput(1);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserAlreadyHaveStorage"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var factory = new InitializeStorageUseCaseFactory();
            var factoryContext = factory.ConfigureWithUser();

            UserOwnership? userOwnership = null;
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(m => m.GetByUser(factoryContext.UserMock.Object))
                                       .Returns(userOwnership);
            factory.OwnershipRepository = userOwnershipRepositoryMock.Object;

            var input = new InitializeStorageInput(1);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
        }
    }
}
