using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Services;
using StorageBot.Application.UseCases.Storing.QuantityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.GetContainerEntries
{
    public class GetContainerEntriesUseCase
        : ContainerEntryUseCase<GetContainerEntriesInput, GetContainerEntriesOutput>
    {
        private readonly IPageSizeService pageSizeService;

        public GetContainerEntriesUseCase(
            ILogger<GetContainerEntriesUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository,
            IPageSizeService pageSizeService) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
            this.pageSizeService = pageSizeService;
        }

        protected override GetContainerEntriesOutput ExecuteWithContainer(GetContainerEntriesInput input)
        {
            var page = input.Page == 0 ? 1 : input.Page;
            var items = ContainerEntryRepository.GetAll();
            var count = items.Count();
            var entries = items.Skip((page - 1) * pageSizeService.GetPageSize())
                               .Take(pageSizeService.GetPageSize());
            var pagination = new PaginationDTO(page, pageSizeService.GetPageSize(), count);
            var output = new GetContainerEntriesOutput(
                Output.OutputResult.Success,
                string.Empty,
                entries.Select(e =>
                {
                    var qType = e.EntryQuantity.QuantityType;
                    var quantityDTO = new EntryQuantityDTO(
                        qType.Id, qType.UnitName, qType.IsInteger, e.EntryQuantity.Value);
                    return new ContainerEntryDTO(e.Id, e.Name, quantityDTO);
                }),
                pagination);
            return output;
        }

        protected override GetContainerEntriesOutput GenerateContainerNotFoundOutput()
        {
            return new GetContainerEntriesOutput(Output.OutputResult.FatalFail, _localizer["Case.ContainerNotFound"]);
        }

        protected override GetContainerEntriesOutput GenerateNoAccessOutput(GetContainerEntriesInput input)
        {
            return new GetContainerEntriesOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetContainerEntriesOutput GenerateOwnershipNotFoundOutput(GetContainerEntriesInput input)
        {
            return new GetContainerEntriesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetContainerEntriesOutput GenerateStorageOwnershipNotFoundOutput(GetContainerEntriesInput input)
        {
            return new GetContainerEntriesOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetContainerEntriesOutput GenerateUserNotFoundOutput(GetContainerEntriesInput input)
        {
            return new GetContainerEntriesOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
