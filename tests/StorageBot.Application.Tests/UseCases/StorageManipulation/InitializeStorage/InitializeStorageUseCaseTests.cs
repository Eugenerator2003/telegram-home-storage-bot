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
            var input = new InitializeStorageInput(1);
            var user = new TestUser();
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            var ownership = new UserOwnership(user);
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(m => m.GetByUser(user))
                                       .Returns(ownership);
            var factory = new InitializeStorageUseCaseFactory();
            factory.UserRepository = userRepositoryMock.Object;
            factory.OwnershipRepository = userOwnershipRepositoryMock.Object;
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.UserAlreadyHaveStorage"], output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var input = new InitializeStorageInput(1);
            var user = new TestUser();
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(m => m.GetById(input.UserId))
                              .Returns(user);
            UserOwnership? ownership = null;
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(m => m.GetByUser(user))
                                       .Returns(ownership);
            userOwnershipRepositoryMock.Setup(m => m.Create(It.IsAny<UserOwnership>()));
            var factory = new InitializeStorageUseCaseFactory();
            factory.UserRepository = userRepositoryMock.Object;
            factory.OwnershipRepository = userOwnershipRepositoryMock.Object;
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
        }
    }
}
