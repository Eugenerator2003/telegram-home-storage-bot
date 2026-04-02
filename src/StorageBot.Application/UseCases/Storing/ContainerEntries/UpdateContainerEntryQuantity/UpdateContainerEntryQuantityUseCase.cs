using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utils;
using StorageBot.Domain.Exceptions.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.UpdateContainerEntryQuantity
{
    public class UpdateContainerEntryQuantityUseCase :
        ContainerEntryUseCase<UpdateContainerEntryQuantityInput, UpdateContainerEntryQuantityOutput>
    {
        public UpdateContainerEntryQuantityUseCase(ILogger<UpdateContainerEntryQuantityUseCase> logger, 
                                                   IStringLocalizer localizer, 
                                                   UserRepository userRepository, 
                                                   UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override UpdateContainerEntryQuantityOutput ExecuteWithContainer(UpdateContainerEntryQuantityInput input)
        {
            using (_logger.BeginContainerEntryScope(input.ContainerEntryId))
            {
                Output.OutputResult result = Output.OutputResult.Success;
                string message = string.Empty;

                var entry = ContainerEntryRepository.GetById(input.ContainerEntryId);
                if (entry == null)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.ContainerEntryNotFound"];
                    _logger.LogError("User failed to update entry quantity: entry not found");
                }
                else if (input.QuantityValue <= 0)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.NotPositiveQuantityValue"];
                    _logger.LogError("User failed to update entry quantity: invalid quantity value (must be positive)");
                }
                else
                {
                    try
                    {
                        entry.SetQuantityValue(input.QuantityValue);
                        _logger.LogInformation("User update entry quantity");
                    }
                    catch (NotIntegerValueException ex)
                    {
                        result = Output.OutputResult.Fail;
                        message = _localizer["UseCase.NotIntegerQuantityValue"];
                        _logger.LogError(ex,
                                         "User failed to update entry quantity: invalid quantity value (must be integer)");
                    }
                    catch (Exception ex)
                    {
                        result = Output.OutputResult.FatalFail;
                        message = _localizer["UseCase.ServerError"];
                        _logger.LogError(ex,
                                         "User failed to update entry quantity");
                    }
                }
                return new UpdateContainerEntryQuantityOutput(result, message);
            }
        }

        protected override UpdateContainerEntryQuantityOutput GenerateContainerNotFoundOutput()
        {
            return new UpdateContainerEntryQuantityOutput(Output.OutputResult.FatalFail, _localizer["UseCase.ContainerNotFound"]);
        }

        protected override UpdateContainerEntryQuantityOutput GenerateNoAccessOutput(UpdateContainerEntryQuantityInput input)
        {
            return new UpdateContainerEntryQuantityOutput(Output.OutputResult.FatalFail, _localizer["UseCase.NoStorageAccess"]);
        }

        protected override UpdateContainerEntryQuantityOutput GenerateOwnershipNotFoundOutput(UpdateContainerEntryQuantityInput input)
        {
            return new UpdateContainerEntryQuantityOutput(Output.OutputResult.FatalFail, _localizer["UseCase.OwnershipNotFound"]);
        }

        protected override UpdateContainerEntryQuantityOutput GenerateStorageOwnershipNotFoundOutput(UpdateContainerEntryQuantityInput input)
        {
            return new UpdateContainerEntryQuantityOutput(Output.OutputResult.FatalFail, _localizer["UseCase.StorageOnwershipNotFound"]);
        }

        protected override UpdateContainerEntryQuantityOutput GenerateUserNotFoundOutput(UpdateContainerEntryQuantityInput input)
        {
            return new UpdateContainerEntryQuantityOutput(Output.OutputResult.FatalFail, _localizer["UseCase.UserNotFound"]);
        }
    }
}
