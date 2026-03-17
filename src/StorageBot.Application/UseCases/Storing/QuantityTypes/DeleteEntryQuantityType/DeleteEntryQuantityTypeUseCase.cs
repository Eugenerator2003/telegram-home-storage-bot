using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
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
            Output.OutputResult result;
            string message = "";
            try
            {
                QuantityTypeRepository.Delete(input.QuantityTypeId);
                result = Output.OutputResult.Success;
                _logger.LogInformation("User {userId} delete quantity type {quantityTypeId} (ownership {storageOwnershipId})",
                                        input.UserId,
                                        input.QuantityTypeId,
                                        input.StorageOwnershipId);
            }
            catch (EntityNotFoundException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.QuantityTypeNotFound"];
                _logger.LogError(ex, 
                                 "User {userId} failed to delete quantity type {quantityTypeId} (ownership {storageOwnershipId}): quantity type not found",
                                 input.UserId,
                                 input.QuantityTypeId,
                                 input.StorageOwnershipId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.ServerError"];
                _logger.LogError(ex,
                                 "User {userId} failed to delete quantity type {quantityTypeId} (ownership {storageOwnershipId})",
                                 input.UserId,
                                 input.QuantityTypeId,
                                 input.StorageOwnershipId);
            }
            return new DeleteEntryQuantityTypeOutput(result, message);
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
