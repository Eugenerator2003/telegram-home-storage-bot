using Moq;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages;
using StorageBot.Domain.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.StorageManipulation.GetAccessibleStorages
{
    [TestClass]
    public class GetAccessibleStoragesUseCaseTests
    {
        private class GetAccessibleStoragesUseCaseFactory :
            UserOwnershipUseCaseFactory<GetAccessibleStoragesUseCase, GetAccessibleStoragesInput, GetAccessibleStoragesOutput>
        {
            public override GetAccessibleStoragesUseCase CreateUseCase()
            {
                return new GetAccessibleStoragesUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetAccessibleStoragesInput(3123);
            var factory = new GetAccessibleStoragesUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetAccessibleStoragesInput(1337);
            var factory = new GetAccessibleStoragesUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void ExecuteSingeTest()
        {
            var input = new GetAccessibleStoragesInput(99000);
            var factory = new GetAccessibleStoragesUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();

            int userId = 1;
            context.UserMock.Object.Id = userId;

            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(userId, output.UserIds.ElementAt(0));
        }

        [TestMethod]
        public void ExecuteManyTest()
        {
            var input = new GetAccessibleStoragesInput(99000);
            var factory = new GetAccessibleStoragesUseCaseFactory();
            var context = factory.ConfigureWithUserOwnership();

            int userId = 1;
            context.UserMock.Object.Id = userId;
            int[] ids = [4, 10, 17];
            foreach(var id in ids)
            {
                var userMock = new Mock<User>();
                userMock.Object.Id = id;
                context.UserOwnership.AddOtherStorage(
                    new Domain.Storing.Storage(
                        new UserOwnership(userMock.Object)
                    ));
            }

            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(userId, output.UserIds.ElementAt(0));
            foreach(var id in ids)
            {
                Assert.Contains(id, output.UserIds);
            }
        }
    }
}
