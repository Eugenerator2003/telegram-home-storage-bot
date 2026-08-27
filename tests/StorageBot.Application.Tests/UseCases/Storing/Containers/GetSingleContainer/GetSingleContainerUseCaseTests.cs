using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetSingleContainer
{
    [TestClass]
    public class GetSingleContainerUseCaseTests
    {
        private class GetSingleContainerUseCaseFactory
            : StorageUseCaseFactory<GetSingleContainerUseCase, GetSingleContainerInput, GetSingleContainerOutput>
        {
            public override GetSingleContainerUseCase CreateUseCase()
            {
                return new GetSingleContainerUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetSingleContainerInput(230, 322, 5);
            var factory = new GetSingleContainerUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetSingleContainerInput(230, 322, 5);
            var factory = new GetSingleContainerUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetSingleContainerInput(230, 322, 5);
            var factory = new GetSingleContainerUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new GetSingleContainerInput(230, 322, 5);
            var factory = new GetSingleContainerUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteStorageContainerNotFoundTest()
        {
            var targetOwnershipId = 322;
            var input = new GetSingleContainerInput(230, targetOwnershipId, 5);
            var factory = new GetSingleContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var output = factory.CreateUseCase().Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.StorageContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessTest()
        {
            var targetOwnershipId = 322;
            var containerId = 1;
            var input = new GetSingleContainerInput(230, targetOwnershipId, containerId);
            var factory = new GetSingleContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(targetOwnershipId);
            var container = new Container("container") { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var output = factory.CreateUseCase().Execute(input);
            var dto = new ContainerDTO(containerId, container.Name);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(dto, output.Container);
        }
    }
}
