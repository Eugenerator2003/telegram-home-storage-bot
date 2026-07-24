using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    public class GetSingleEntryCategoryUseCase
        : StorageEntryCategoryUseCase<GetSingleEntryCategoryInput, GetSingleEntryCategoryOutput>
    {
        public GetSingleEntryCategoryUseCase(
            ILogger<StorageEntryCategoryUseCase<GetSingleEntryCategoryInput, GetSingleEntryCategoryOutput>> logger,
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetSingleEntryCategoryOutput ExecuteWithRepository(GetSingleEntryCategoryInput input)
        {
            var category = EntryCategoryRepository.GetById(input.EntryCategoryId);
            GetSingleEntryCategoryOutput output;
            if (category != null)
            {
                var dto = new EntryCategoryDTO(category.Id, category.Name);
                _logger.LogInformation("User get entry category {entryCategoryId}", category.Id);
                output = new GetSingleEntryCategoryOutput(Output.OutputResult.Success, string.Empty, dto);
            }
            else
            {
                _logger.LogInformation("User can't get entry category: entry category {entryCategoryId} not found", input.EntryCategoryId);
                output = new GetSingleEntryCategoryOutput(Output.OutputResult.Fail, _localizer["Case.EntryCategoryNotFound"]);
            }
            return output;
        }

        protected override GetSingleEntryCategoryOutput GenerateNoAccessOutput(GetSingleEntryCategoryInput input)
        {
            return new GetSingleEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetSingleEntryCategoryOutput GenerateOwnershipNotFoundOutput(GetSingleEntryCategoryInput input)
        {
            return new GetSingleEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetSingleEntryCategoryOutput GenerateStorageOwnershipNotFoundOutput(GetSingleEntryCategoryInput input)
        {
            return new GetSingleEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetSingleEntryCategoryOutput GenerateUserNotFoundOutput(GetSingleEntryCategoryInput input)
        {
            return new GetSingleEntryCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
