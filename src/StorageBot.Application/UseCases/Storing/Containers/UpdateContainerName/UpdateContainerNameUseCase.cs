using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utilities;

namespace StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName
{
    public class UpdateContainerNameUseCase
        : StorageContainerUseCase<UpdateContainerNameInput, UpdateContainerNameOutput>
    {
        public UpdateContainerNameUseCase(
            ILogger<UpdateContainerNameUseCase> logger,
            IStringLocalizer localizer,
            UserRepository userRepository,
            UserOwnershipRepository ownershipRepository)
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override UpdateContainerNameOutput ExecuteWithRepository(UpdateContainerNameInput input)
        {
            using (_logger.BeginContainerScope(input.ContainerId))
            {
                var container = StorageContainerRepository.GetById(input.ContainerId);
                if (container == null)
                {
                    _logger.LogError("User failed to update container: container not found");
                    return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.ContainerNotFound"]);
                }
                if (string.IsNullOrWhiteSpace(input.ContainerName))
                {
                    _logger.LogError("User failed to update container: empty input string");
                    return new UpdateContainerNameOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
                }
                container.Name = input.ContainerName;
                _logger.LogInformation("User update container name");
                return new UpdateContainerNameOutput(Output.OutputResult.Success);
            }
        }

        protected override UpdateContainerNameOutput GenerateNoAccessOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override UpdateContainerNameOutput GenerateOwnershipNotFoundOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override UpdateContainerNameOutput GenerateStorageOwnershipNotFoundOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override UpdateContainerNameOutput GenerateUserNotFoundOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
