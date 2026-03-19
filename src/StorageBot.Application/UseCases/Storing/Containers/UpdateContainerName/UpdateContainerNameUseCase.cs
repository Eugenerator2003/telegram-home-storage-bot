using System;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;

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
            var container = StorageContainerRepository.GetById(input.ContainerId);
            if (container == null) 
            {
                _logger.LogError("User {userId} failed to update container {containerId} (ownership {storageOwnershipId}): container not found",
                                 input.UserId,
                                 input.ContainerId,
                                 input.StorageOwnershipId);
                return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.ContainerNotFound"]);
            }
            if (string.IsNullOrWhiteSpace(input.ContainerName)) 
            {
                _logger.LogError("User {userId} failed to update container {containerId} (ownership {storageOwnershipId}): empty input string",
                                 input.UserId,
                                 input.ContainerId,
                                 input.StorageOwnershipId);
                return new UpdateContainerNameOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);                
            }
            container.Name = input.ContainerName;
            _logger.LogInformation("User {userId} update container {containerId} name (ownership {storageOwnershipId})",
                                    input.UserId,
                                    input.ContainerId,
                                    input.StorageOwnershipId);
            return new UpdateContainerNameOutput(Output.OutputResult.Success);
        }

        protected override UpdateContainerNameOutput GenerateNoAccessOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override UpdateContainerNameOutput GenerateOwnershipNotFoundOutput(UpdateContainerNameInput input)
        {
            return new UpdateContainerNameOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
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
