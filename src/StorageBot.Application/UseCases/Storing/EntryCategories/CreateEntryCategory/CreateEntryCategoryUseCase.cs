using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory
{
    public class CreateEntryCategoryUseCase :
        StorageEntryCategoryUseCase<CreateEntryCategoryInput, CreateEntryCategoryOutput>
    {
        public CreateEntryCategoryUseCase(
            ILogger<CreateEntryCategoryUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override CreateEntryCategoryOutput ExecuteWithRepository(CreateEntryCategoryInput input)
        {
            if (string.IsNullOrWhiteSpace(input.CategoryName))
            {
                _logger.LogError("User failed to create entry category: empty name input");
                return new CreateEntryCategoryOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
            }

            Output.OutputResult result = Output.OutputResult.Success;
            string message = "";
            try
            {
                if (input.MainCategoryId.HasValue)
                {
                    EntryCategoryRepository.CreateWithMainCategory(input.CategoryName, input.MainCategoryId.Value);
                }
                else
                {
                    EntryCategoryRepository.Create(input.CategoryName);
                }
                _logger.LogInformation("User create entry category");
            }
            catch (EntityNotFoundException ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.MainCategoryNotFound"];
                _logger.LogError(ex, "User failed to create entry category: main category {mainCategoryId} not found",
                                input.MainCategoryId);
            }
            catch (Exception ex)
            {
                result = Output.OutputResult.FatalFail;
                message = _localizer["Case.ServerError"];
                _logger.LogError(ex, "User failed to create entry category");
            }
            return new CreateEntryCategoryOutput(result, message);
        }

        protected override CreateEntryCategoryOutput GenerateNoAccessOutput(CreateEntryCategoryInput input)
        {
            return new CreateEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override CreateEntryCategoryOutput GenerateOwnershipNotFoundOutput(CreateEntryCategoryInput input)
        {
            return new CreateEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override CreateEntryCategoryOutput GenerateStorageOwnershipNotFoundOutput(CreateEntryCategoryInput input)
        {
            return new CreateEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override CreateEntryCategoryOutput GenerateUserNotFoundOutput(CreateEntryCategoryInput input)
        {
            return new CreateEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
