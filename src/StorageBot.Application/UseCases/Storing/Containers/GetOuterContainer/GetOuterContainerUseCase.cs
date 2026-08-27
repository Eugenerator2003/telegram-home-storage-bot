using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer
{
    public class GetOuterContainerUseCase
        : StorageContainerUseCase<GetOuterContainerInput, GetOuterContainerOutput>
    {
        public GetOuterContainerUseCase(
            ILogger<StorageContainerUseCase<GetOuterContainerInput, GetOuterContainerOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetOuterContainerOutput ExecuteWithRepository(GetOuterContainerInput input)
        {
            var innerContainer = StorageContainerRepository.GetById(input.InnerContainerId);
            if (innerContainer == null)
            {
                _logger.LogInformation("User can't get inner container: inner container {innerContainer} not found", input.InnerContainerId);
                return new GetOuterContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.InnerContainerNotFound"]);
            }
            GetOuterContainerOutput output = null!;
            if (innerContainer.OuterContainer != null)
            {
                var dto = new ContainerDTO(innerContainer.OuterContainer.Id, innerContainer.OuterContainer.Name);
                output = new GetOuterContainerOutput(Output.OutputResult.Success, string.Empty, dto);
                _logger.LogInformation("User get outer container of container {containerId}", input.InnerContainerId);
            }
            else
            {
                output = new GetOuterContainerOutput(Output.OutputResult.Fail, _localizer["Case.OuterContainerNotFound"]);
                _logger.LogInformation("User can't get outer container: container {containerId} has no outer container", input.InnerContainerId);
            }
            return output;
        }

        protected override GetOuterContainerOutput GenerateNoAccessOutput(GetOuterContainerInput input)
        {
            return new GetOuterContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetOuterContainerOutput GenerateOwnershipNotFoundOutput(GetOuterContainerInput input)
        {
            return new GetOuterContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetOuterContainerOutput GenerateStorageOwnershipNotFoundOutput(GetOuterContainerInput input)
        {
            return new GetOuterContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetOuterContainerOutput GenerateUserNotFoundOutput(GetOuterContainerInput input)
        {
            return new GetOuterContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
