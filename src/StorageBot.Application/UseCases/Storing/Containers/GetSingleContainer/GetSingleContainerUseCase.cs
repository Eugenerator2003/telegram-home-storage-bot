using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer
{
    public class GetSingleContainerUseCase
        : StorageContainerUseCase<GetSingleContainerInput, GetSingleContainerOutput>
    {
        public GetSingleContainerUseCase(
            ILogger<StorageContainerUseCase<GetSingleContainerInput, GetSingleContainerOutput>> logger,
            IStringLocalizer localizer,
            UserRepository userRepository,
            UserOwnershipRepository ownershipRepository)
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetSingleContainerOutput ExecuteWithRepository(GetSingleContainerInput input)
        {
            var container = StorageContainerRepository.GetById(input.ContainerId);
            GetSingleContainerOutput output;
            if (container != null)
            {
                var dto = new ContainerDTO(container.Id, container.Name);
                _logger.LogInformation("User get container {containerId}", container.Id);
                output = new GetSingleContainerOutput(Output.OutputResult.Success, string.Empty, dto);
            }
            else
            {
                _logger.LogInformation("User can't get container: entry category {containerId} not found", input.ContainerId);
                output = new GetSingleContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageContainerNotFound"]);
            }
            return output;
        }

        protected override GetSingleContainerOutput GenerateNoAccessOutput(GetSingleContainerInput input)
        {
            return new GetSingleContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetSingleContainerOutput GenerateOwnershipNotFoundOutput(GetSingleContainerInput input)
        {
            return new GetSingleContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetSingleContainerOutput GenerateStorageOwnershipNotFoundOutput(GetSingleContainerInput input)
        {
            return new GetSingleContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetSingleContainerOutput GenerateUserNotFoundOutput(GetSingleContainerInput input)
        {
            return new GetSingleContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
