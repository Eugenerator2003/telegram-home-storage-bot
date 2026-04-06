using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.UpdateEntryCategoryNameUseCase
{
    public class UpdateEntryCategoryNameUseCase :
        StorageEntryCategoryUseCase<UpdateEntryCategoryNameInput, UpdateEntryCategoryNameOutput>
    {
        public UpdateEntryCategoryNameUseCase(
            ILogger<UpdateEntryCategoryNameUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override UpdateEntryCategoryNameOutput ExecuteWithRepository(UpdateEntryCategoryNameInput input)
        {
            using (_logger.BeginEntryCategoryScope(input.EntryCategoryId))
            {
                var entryCategory = EntryCategoryRepository.GetById(input.EntryCategoryId);
                if (entryCategory == null)
                {
                    _logger.LogError("User failed to update entry category: category not found");
                    return new UpdateEntryCategoryNameOutput(Output.OutputResult.FatalFail, _localizer["Case.EntryCategoryNotFound"]);
                }
                if (string.IsNullOrWhiteSpace(input.CategoryName))
                {
                    _logger.LogError("User failed to update entry category: empty name");
                    return new UpdateEntryCategoryNameOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
                }
                entryCategory.Name = input.CategoryName;
                _logger.LogInformation("User update entry category name");
                return new UpdateEntryCategoryNameOutput(Output.OutputResult.Success);
            }
        }

        protected override UpdateEntryCategoryNameOutput GenerateNoAccessOutput(UpdateEntryCategoryNameInput input)
        {
            return new UpdateEntryCategoryNameOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override UpdateEntryCategoryNameOutput GenerateOwnershipNotFoundOutput(UpdateEntryCategoryNameInput input)
        {
            return new UpdateEntryCategoryNameOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override UpdateEntryCategoryNameOutput GenerateStorageOwnershipNotFoundOutput(UpdateEntryCategoryNameInput input)
        {
            return new UpdateEntryCategoryNameOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override UpdateEntryCategoryNameOutput GenerateUserNotFoundOutput(UpdateEntryCategoryNameInput input)
        {
            return new UpdateEntryCategoryNameOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
