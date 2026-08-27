using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase
{
    public class GetMainCategoryUseCase : StorageEntryCategoryUseCase<GetMainCategoryInput, GetMainCategoryOutput>
    {
        public GetMainCategoryUseCase(
            ILogger<StorageEntryCategoryUseCase<GetMainCategoryInput, GetMainCategoryOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetMainCategoryOutput ExecuteWithRepository(GetMainCategoryInput input)
        {
            var entryCategory = EntryCategoryRepository.GetById(input.SubcategoryId);
            if (entryCategory == null)
            {
                _logger.LogInformation("User can't get main category: subcategory {subcategoryId} not found", input.SubcategoryId);
                return new GetMainCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.SubcategoryNotFound"]);
            }
            GetMainCategoryOutput output = null!;
            if (entryCategory.MainCategory != null)
            {
                var dto = new EntryCategoryDTO(entryCategory.MainCategory.Id, entryCategory.MainCategory.Name);
                output = new GetMainCategoryOutput(Output.OutputResult.Success, string.Empty, dto);
                _logger.LogInformation("User get main category of subcategory {subcategoryId}", input.SubcategoryId);
            }
            else
            {
                output = new GetMainCategoryOutput(Output.OutputResult.Fail, _localizer["Case.MainCategoryNotFound"]);
                _logger.LogInformation("User can't get main category: subcategory {subcategoryId} has no main category", input.SubcategoryId);
            }
            return output;
        }

        protected override GetMainCategoryOutput GenerateNoAccessOutput(GetMainCategoryInput input)
        {
            return new GetMainCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetMainCategoryOutput GenerateOwnershipNotFoundOutput(GetMainCategoryInput input)
        {
            return new GetMainCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetMainCategoryOutput GenerateStorageOwnershipNotFoundOutput(GetMainCategoryInput input)
        {
            return new GetMainCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetMainCategoryOutput GenerateUserNotFoundOutput(GetMainCategoryInput input)
        {
            return new GetMainCategoryOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
