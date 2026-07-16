using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetUserOwnership
{
    [TestClass]
    public class GetUserOwnershipUseCaseTests
    {
        private class GetUserOwnershipUseCaseFactory :
            UseCaseFactory<GetUserOwnershipUseCase, GetUserOwnershipInput, GetUserOwnershipOutput>
        {
            public UserRepository UserRepository { get; set; } = new Mock<UserRepository>().Object;

            public UserOwnershipRepository UserOwnershipRepository { get; set; } = new Mock<UserOwnershipRepository>().Object;

            public override GetUserOwnershipUseCase CreateUseCase()
            {
                return new GetUserOwnershipUseCase(Logger, Localizer, UserRepository, UserOwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var userId = 123;
            var factory = new GetUserOwnershipUseCaseFactory();
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(r => r.GetById(userId)).Returns(null as User);
            factory.UserRepository = userRepositoryMock.Object;

            var useCase = factory.CreateUseCase();
            var input = new GetUserOwnershipInput(userId);
            var output = useCase.Execute(input);

            var expectedOwnershipId = -1;
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(expectedOwnershipId, output.OwnershipId);
            Assert.Contains("UserNotFound", output.Message);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var userId = 123;
            var user = new Mock<User>().Object;
            var factory = new GetUserOwnershipUseCaseFactory();
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(r => r.GetByUser(user)).Returns(null as UserOwnership);
            factory.UserRepository = userRepositoryMock.Object;
            factory.UserOwnershipRepository = userOwnershipRepositoryMock.Object;

            var useCase = factory.CreateUseCase();
            var input = new GetUserOwnershipInput(userId);
            var output = useCase.Execute(input);

            var expectedOwnershipId = -1;
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(expectedOwnershipId, output.OwnershipId);
            Assert.Contains("UserOwnershipNotFound", output.Message);
        }

        [TestMethod]
        public void ExecuteTest()
        {
            var userId = 123;
            var ownershipId = 321;
            var user = new Mock<User>().Object;
            var userOwneship = new UserOwnership(user) { Id = ownershipId };
            var factory = new GetUserOwnershipUseCaseFactory();
            var userRepositoryMock = new Mock<UserRepository>();
            userRepositoryMock.Setup(r => r.GetById(userId)).Returns(user);
            var userOwnershipRepositoryMock = new Mock<UserOwnershipRepository>();
            userOwnershipRepositoryMock.Setup(r => r.GetByUser(user)).Returns(userOwneship);
            factory.UserRepository = userRepositoryMock.Object;
            factory.UserOwnershipRepository = userOwnershipRepositoryMock.Object;

            var useCase = factory.CreateUseCase();
            var input = new GetUserOwnershipInput(userId);
            var output = useCase.Execute(input);

            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(userOwneship.Id, output.OwnershipId);
            Assert.IsEmpty(output.Message);
        }
    }
}
