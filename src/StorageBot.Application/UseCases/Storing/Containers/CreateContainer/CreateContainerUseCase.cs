using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Exceptions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.CreateContainer
{
    public class CreateContainerUseCase
        : StorageContainerUseCase<CreateContainerInput, CreateContainerOutput>
    {
        public CreateContainerUseCase(
            ILogger<CreateContainerUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override CreateContainerOutput ExecuteWithRepository(CreateContainerInput input)
        {
            if (string.IsNullOrWhiteSpace(input.ContainerName))
            {
                _logger.LogError("User failed to create container: empty input string");
                return new CreateContainerOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
            }

            Output.OutputResult result = Output.OutputResult.Success;
            string message = string.Empty;
            try
            {
                if (input.OuterContainerId.HasValue)
                {
                    StorageContainerRepository.CreateWithOuterContainer(input.ContainerName, input.OuterContainerId.Value);
                }
                else
                {
                    StorageContainerRepository.Create(input.ContainerName);
                }
                _logger.LogInformation("User create container");
            }
            catch (EntityNotFoundException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.OuterContainerNotFound"];
                _logger.LogError(ex, "User failed to create container: outer container {outerContainerId} not found",
                                 input.OuterContainerId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.ServerError"];
                _logger.LogError(ex, "User failed to create container");
            }
            return new CreateContainerOutput(result, message);
        }

        protected override CreateContainerOutput GenerateNoAccessOutput(CreateContainerInput input)
        {
            return new CreateContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override CreateContainerOutput GenerateOwnershipNotFoundOutput(CreateContainerInput input)
        {
            return new CreateContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override CreateContainerOutput GenerateStorageOwnershipNotFoundOutput(CreateContainerInput input)
        {
            return new CreateContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override CreateContainerOutput GenerateUserNotFoundOutput(CreateContainerInput input)
        {
            return new CreateContainerOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
