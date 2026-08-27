using StorageBot.Application.Tests.TestUtilities;
using StorageBot.Application.Tests.TestUtilities.Factories;
using StorageBot.Application.Tests.TestUtilities.Factories.Storing;
using StorageBot.Application.UseCases;
using StorageBot.Application.UseCases.Storing.Containers;
using StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers.GetOuterContainer
{
    [TestClass]
    public class GetOuterContainerUseCaseTests
    {
        private class GetOuterContainerUseCaseFactory
            : StorageUseCaseFactory<GetOuterContainerUseCase, GetOuterContainerInput, GetOuterContainerOutput>
        {
            public override GetOuterContainerUseCase CreateUseCase()
            {
                return new GetOuterContainerUseCase(Logger, Localizer, UserRepository, OwnershipRepository);
            }
        }

        [TestMethod]
        public void UserNotFoundTest()
        {
            var input = new GetOuterContainerInput(1, 100, 1000);
            var factory = new GetOuterContainerUseCaseFactory();
            UseCaseTester.UserNotFoundTest(factory, input);
        }

        [TestMethod]
        public void UserOwnershipNotFoundTest()
        {
            var input = new GetOuterContainerInput(1, 100, 1000);
            var factory = new GetOuterContainerUseCaseFactory();
            UseCaseTester.UserOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void StorageOwnershipNotFoundTest()
        {
            var input = new GetOuterContainerInput(1, 100, 1000);
            var factory = new GetOuterContainerUseCaseFactory();
            UseCaseTester.StorageOwnershipNotFoundTest(factory, input);
        }

        [TestMethod]
        public void NoStorageAccessTest()
        {
            var input = new GetOuterContainerInput(1, 100, 1000);
            var factory = new GetOuterContainerUseCaseFactory();
            UseCaseTester.NoStorageAccessTest(factory, input);
        }

        [TestMethod]
        public void ExecuteInnerContainerNotFoundTest()
        {
            var ownershipId = 10;
            var input = new GetOuterContainerInput(1, ownershipId, 1000);
            var factory = new GetOuterContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(ownershipId);
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.FatalFail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.InnerContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteNoOuterContainerTest()
        {
            var ownershipId = 10;
            var containerId = 1000;
            var input = new GetOuterContainerInput(1, ownershipId, containerId);
            var factory = new GetOuterContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(ownershipId);
            var container = new Container("name") { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(container);
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            Assert.AreEqual(Output.OutputResult.Fail, output.Result);
            Assert.AreEqual(factory.Localizer["Case.OuterContainerNotFound"], output.Message);
        }

        [TestMethod]
        public void ExecuteSuccessExecuteTest()
        {
            var ownershipId = 10;
            var containerId = 1000;
            var input = new GetOuterContainerInput(1, ownershipId, containerId);
            var factory = new GetOuterContainerUseCaseFactory();
            var context = factory.ConfigureWithTargetOwnership(ownershipId);
            var outerContainer = new Container("outer") { Id = containerId - 1 };
            var container = new Container("name", outerContainer) { Id = containerId };
            context.TargetOwnership.PersonalStorage.AddContainer(outerContainer);
            var useCase = factory.CreateUseCase();
            var output = useCase.Execute(input);
            var dto = new ContainerDTO(outerContainer.Id, outerContainer.Name);
            Assert.AreEqual(Output.OutputResult.Success, output.Result);
            Assert.AreEqual(string.Empty, output.Message);
            Assert.AreEqual(dto, output.OuterContainer);
        }
    }
}
