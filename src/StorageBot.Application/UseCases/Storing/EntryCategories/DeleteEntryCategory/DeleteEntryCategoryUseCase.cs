using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.Exceptions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase
{
    public class DeleteEntryCategoryUseCase :
        StorageEntryCategoryUseCase<DeleteEntryCategoryInput, DeleteEntryCategoryOutput>
    {
        public DeleteEntryCategoryUseCase(
            ILogger<DeleteEntryCategoryUseCase> logger, 
            IStringLocalizer localizer, UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override DeleteEntryCategoryOutput ExecuteWithRepository(DeleteEntryCategoryInput input)
        {
            string message = "";
            Output.OutputResult result = Output.OutputResult.Success;
            try
            {
                EntryCategoryRepository.Delete(input.EntryCategoryId);
                _logger.LogInformation("User {userId} delete entry category {entryCategoryId} (ownership {storageOnwershipId})",
                                        input.UserId,
                                        input.EntryCategoryId,
                                        input.StorageOwnershipId);
            }
            catch (EntityNotFoundException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.EntryCategoryNotFound"];
                _logger.LogError(ex, "User {userId} failed to delete entry category {entryCategoryId} (ownership {storageOnwershipId}): entry category not found",
                                 input.UserId,
                                 input.EntryCategoryId,
                                 input.StorageOwnershipId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.EntryCategoryNotFound"];
                _logger.LogError(ex, "User {userId} failed to delete entry category {entryCategoryId} (ownership {storageOnwershipId})",
                                 input.UserId,
                                 input.EntryCategoryId,
                                 input.StorageOwnershipId);
            }
            return new DeleteEntryCategoryOutput(result, message);
        }

        protected override DeleteEntryCategoryOutput GenerateNoAccessOutput(DeleteEntryCategoryInput input)
        {
            return new DeleteEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override DeleteEntryCategoryOutput GenerateOwnershipNotFoundOutput(DeleteEntryCategoryInput input)
        {
            return new DeleteEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override DeleteEntryCategoryOutput GenerateStorageOwnershipNotFoundOutput(DeleteEntryCategoryInput input)
        {
            return new DeleteEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override DeleteEntryCategoryOutput GenerateUserNotFoundOutput(DeleteEntryCategoryInput input)
        {
            return new DeleteEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
