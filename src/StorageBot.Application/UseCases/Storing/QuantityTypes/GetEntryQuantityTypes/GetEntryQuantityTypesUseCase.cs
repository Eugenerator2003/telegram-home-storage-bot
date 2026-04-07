using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Services;
using StorageBot.Application.UseCases.StorageManipulation.RelinquishStorageAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetEntryQuantityTypes
{
    public class GetEntryQuantityTypesUseCase
        : StorageEntryQuantityTypeUseCase<GetEntryQuantityTypesInput, GetEntryQuantityTypesOutput>
    {
        private readonly IPageSizeService pageSizeService;

        public GetEntryQuantityTypesUseCase(
            ILogger<GetEntryQuantityTypesUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository,
            IPageSizeService pageSizeService) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
            this.pageSizeService = pageSizeService;
        }

        protected override GetEntryQuantityTypesOutput ExecuteWithRepository(GetEntryQuantityTypesInput input)
        {
            int page = input.Page == 0 ? 1 : input.Page;
            int pageSize = pageSizeService.GetPageSize();
            var types = QuantityTypeRepository.GetAll();
            int count = types.Count();
            var typeDTOs = types.Skip((page - 1) * pageSize)
                                .Take(pageSize)
                                .Select(t => new EntryQuantityTypeDTO(t.Id, t.IsInteger, t.UnitName));
            _logger.LogInformation("User get quantity types of personal storage ownership");
            var pagination = new PaginationDTO(page, pageSize, count);
            return new GetEntryQuantityTypesOutput(typeDTOs, pagination, Output.OutputResult.Success);
        }

        protected override GetEntryQuantityTypesOutput GenerateNoAccessOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetEntryQuantityTypesOutput GenerateOwnershipNotFoundOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetEntryQuantityTypesOutput GenerateStorageOwnershipNotFoundOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetEntryQuantityTypesOutput GenerateUserNotFoundOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
