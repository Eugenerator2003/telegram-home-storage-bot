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

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.DeleteContainerEntry
{
    public class DeleteContainerEntryUseCase
        : ContainerEntryUseCase<DeleteContainerEntryInput, DeleteContainerEntryOutput>
    {
        public DeleteContainerEntryUseCase(ILogger<DeleteContainerEntryUseCase> logger, 
                                           IStringLocalizer localizer, 
                                           UserRepository userRepository, 
                                           UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override DeleteContainerEntryOutput ExecuteWithContainer(DeleteContainerEntryInput input)
        {
            using (_logger.BeginContainerEntryScope(input.ContainerEntryId))
            {
                Output.OutputResult result = Output.OutputResult.Success;
                string message = string.Empty;
                try
                {
                    ContainerEntryRepository.Delete(input.ContainerEntryId);
                    _logger.LogInformation("User delete container entry");
                }
                catch (EntityNotFoundException ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.ContainerEnryNotFound"];
                    _logger.LogError(ex,
                                     "User failed to delete container entry: container not found");
                }
                catch (Exception ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.ServerError"];
                    _logger.LogError(ex,
                                     "User failed to delete container entry: container not found");
                }
                return new DeleteContainerEntryOutput(result, message);
            }
        }

        protected override DeleteContainerEntryOutput GenerateContainerNotFoundOutput()
        {
            return new DeleteContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.ContainerNotFound"]);
        }

        protected override DeleteContainerEntryOutput GenerateNoAccessOutput(DeleteContainerEntryInput input)
        {
            return new DeleteContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.NoStorageAccess"]);
        }

        protected override DeleteContainerEntryOutput GenerateOwnershipNotFoundOutput(DeleteContainerEntryInput input)
        {
            return new DeleteContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.OwnershipNotFound"]);
        }

        protected override DeleteContainerEntryOutput GenerateStorageOwnershipNotFoundOutput(DeleteContainerEntryInput input)
        {
            return new DeleteContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.StorageOwnershipNotFound"]);
        }

        protected override DeleteContainerEntryOutput GenerateUserNotFoundOutput(DeleteContainerEntryInput input)
        {
            return new DeleteContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.UserNotFound"]);
        }
    }
}
