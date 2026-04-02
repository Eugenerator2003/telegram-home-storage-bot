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

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.DeleteEntryQuantityType
{
    public class DeleteEntryQuantityTypeUseCase
        : StorageEntryQuantityTypeUseCase<DeleteEntryQuantityTypeInput, DeleteEntryQuantityTypeOutput>
    {
        public DeleteEntryQuantityTypeUseCase(
            ILogger<DeleteEntryQuantityTypeUseCase> logger,
            IStringLocalizer localizer,
            UserRepository userRepository,
            UserOwnershipRepository ownershipRepository)
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override DeleteEntryQuantityTypeOutput ExecuteWithRepository(DeleteEntryQuantityTypeInput input)
        {
            using (_logger.BeginEntryQuantityTypeScope(input.QuantityTypeId))
            {
                Output.OutputResult result;
                string message = "";
                try
                {
                    QuantityTypeRepository.Delete(input.QuantityTypeId);
                    result = Output.OutputResult.Success;
                    _logger.LogInformation("User delete quantity type");
                }
                catch (EntityNotFoundException ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.QuantityTypeNotFound"];
                    _logger.LogError(ex, "User failed to delete quantity type: quantity type not found");
                }
                catch (Exception ex)
                {
                    result = Output.OutputResult.FatalFail;
                    message = _localizer["UseCase.ServerError"];
                    _logger.LogError(ex, "User failed to delete quantity type");
                }
                return new DeleteEntryQuantityTypeOutput(result, message);
            }
        }

        protected override DeleteEntryQuantityTypeOutput GenerateNoAccessOutput(DeleteEntryQuantityTypeInput input)
        {
            return new DeleteEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override DeleteEntryQuantityTypeOutput GenerateOwnershipNotFoundOutput(DeleteEntryQuantityTypeInput input)
        {
            return new DeleteEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override DeleteEntryQuantityTypeOutput GenerateStorageOwnershipNotFoundOutput(DeleteEntryQuantityTypeInput input)
        {
            return new DeleteEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override DeleteEntryQuantityTypeOutput GenerateUserNotFoundOutput(DeleteEntryQuantityTypeInput input)
        {
            return new DeleteEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
