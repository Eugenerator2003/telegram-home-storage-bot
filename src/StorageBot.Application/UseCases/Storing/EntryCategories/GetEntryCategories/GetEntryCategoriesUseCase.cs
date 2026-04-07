using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Services;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetEntryCategories
{
    public class GetEntryCategoriesUseCase
        : StorageEntryCategoryUseCase<GetEntryCategoriesInput, GetEntryCategoriesOutput>
    {
        private static EntryCategoryDTO ToDto(EntryCategory entryCategory)
        {
            return new EntryCategoryDTO(entryCategory.Id, entryCategory.Name);
        }

        private static EntryCategoryDTO[] TakePage(IEnumerable<EntryCategory> entryCategories, int pageSize, int page)
        {
            return entryCategories.Skip((page - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(ToDto)
                                  .ToArray();
        }

        private readonly IPageSizeService pageSizeService;

        public GetEntryCategoriesUseCase(
            ILogger<GetEntryCategoriesUseCase> logger,
            IStringLocalizer localizer,
            UserRepository userRepository,
            UserOwnershipRepository ownershipRepository,
            IPageSizeService pageSizeService)
            : base(logger, localizer, userRepository, ownershipRepository)
        {
            this.pageSizeService = pageSizeService;
        }

        protected override GetEntryCategoriesOutput ExecuteWithRepository(GetEntryCategoriesInput input)
        {
            EntryCategory? mainCategory = null;
            if (input.MainCategoryId.HasValue)
            {
                mainCategory = EntryCategoryRepository.GetById(input.MainCategoryId.Value);
                if (mainCategory == null)
                {
                    _logger.LogError("User failed to get entry categories: main category {mainCategoryId} not found",
                                     input.MainCategoryId);
                    return new GetEntryCategoriesOutput(Output.OutputResult.FatalFail, _localizer["Case.MainCategoryNotFound"]);
                }
            }
            EntryCategoryDTO? mainCategoryDto = null;
            IEnumerable<EntryCategory> categories;
            if (mainCategory != null)
            {
                mainCategoryDto = ToDto(mainCategory);
                categories = mainCategory.Subcategories;
            }
            else
            {
                categories = EntryCategoryRepository.GetAll();
            }
            int pageSize = pageSizeService.GetPageSize();
            int page = input.Page == 0 ? 1 : input.Page;
            int count = categories.Count();
            var output = new GetEntryCategoriesOutput(Output.OutputResult.Success, string.Empty, 
                                                        mainCategoryDto, 
                                                        TakePage(categories, pageSize, page),
                                                        new PaginationDTO(page, pageSize, count));
            _logger.LogInformation("User get entry categories from {categoriesLocation}",
                                   input.MainCategoryId.HasValue ? $"main category {input.MainCategoryId.Value}" : "storage");
            return output;
        }

        protected override GetEntryCategoriesOutput GenerateNoAccessOutput(GetEntryCategoriesInput input)
        {
            return new GetEntryCategoriesOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetEntryCategoriesOutput GenerateOwnershipNotFoundOutput(GetEntryCategoriesInput input)
        {
            return new GetEntryCategoriesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetEntryCategoriesOutput GenerateStorageOwnershipNotFoundOutput(GetEntryCategoriesInput input)
        {
            return new GetEntryCategoriesOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetEntryCategoriesOutput GenerateUserNotFoundOutput(GetEntryCategoriesInput input)
        {
            return new GetEntryCategoriesOutput(Output.OutputResult.FatalFail, "Case.UserNotFound");
        }
    }
}
