using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utils;
using StorageBot.Domain.Exceptions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.DeleteContainer
{
    public class DeleteContainerUseCase
        : StorageContainerUseCase<DeleteContainerInput, DeleteContainerOutput>
    {
        public DeleteContainerUseCase(ILogger<DeleteContainerUseCase> logger, 
                                      IStringLocalizer localizer, 
                                      UserRepository userRepository, 
                                      UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override DeleteContainerOutput ExecuteWithRepository(DeleteContainerInput input)
        {
            using (_logger.BeginContainerScope(input.ContainerId))
            {
                Output.OutputResult result = Output.OutputResult.Success;
                string message = string.Empty;
                try
                {
                    StorageContainerRepository.Delete(input.ContainerId);
                    _logger.LogInformation("User delete container");
                }
                catch (EntityNotFoundException ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["Case.EntryCategoryNotFound"];
                    _logger.LogError(ex, "User failed to delete container: container not found");
                }
                catch (Exception ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["Case.EntryCategoryNotFound"];
                    _logger.LogError(ex, "User failed to delete container");
                }
                return new DeleteContainerOutput(result, message);
            }
        }

        protected override DeleteContainerOutput GenerateNoAccessOutput(DeleteContainerInput input)
        {
            return new DeleteContainerOutput(Output.OutputResult.FatalFail, _localizer["UseCase.NoStorageAccess"]);
        }

        protected override DeleteContainerOutput GenerateOwnershipNotFoundOutput(DeleteContainerInput input)
        {
            return new DeleteContainerOutput(Output.OutputResult.FatalFail, _localizer["UseCase.OwnershipNotFound"]);
        }

        protected override DeleteContainerOutput GenerateStorageOwnershipNotFoundOutput(DeleteContainerInput input)
        {
            return new DeleteContainerOutput(Output.OutputResult.FatalFail, _localizer["UseCase.StorageOwnershipNotFound"]);
        }

        protected override DeleteContainerOutput GenerateUserNotFoundOutput(DeleteContainerInput input)
        {
            return new DeleteContainerOutput(Output.OutputResult.FatalFail, _localizer["UseCase.UseNotFound"]);
        }
    }
}
