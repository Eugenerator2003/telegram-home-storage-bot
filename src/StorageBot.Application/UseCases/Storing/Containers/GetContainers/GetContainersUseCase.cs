using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Services;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetContainers
{
    public class GetContainersUseCase 
        : StorageContainerUseCase<GetContainersInput, GetContainersOutput>
    {
        private static ContainerDTO ToDto(Container container)
        {
            return new ContainerDTO(container.Id, container.Name);
        }

        private static ContainerDTO[] TakePage(IEnumerable<Container> containers, int pageSize, int page)
        {
            return containers.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .Select(ToDto)
                             .ToArray();
        }

        private readonly IPageSizeService pageSizeService;

        public GetContainersUseCase(ILogger<GetContainersUseCase> logger, 
                                    IStringLocalizer localizer, 
                                    UserRepository userRepository, 
                                    UserOwnershipRepository ownershipRepository,
                                    IPageSizeService pageSizeService) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
            this.pageSizeService = pageSizeService;
        }

        protected override GetContainersOutput ExecuteWithRepository(GetContainersInput input)
        {
            Container? outerContainer = null;
            if (input.OuterContainerId != null)
            {
                outerContainer = StorageContainerRepository.GetById(input.OuterContainerId.Value);
                if (outerContainer == null)
                {
                    _logger.LogError("User failed to get container: outer container {outerContainerId} not found",
                                     input.OuterContainerId);
                    return new GetContainersOutput(Output.OutputResult.FatalFail, _localizer["Case.OuterContainerNotFound"]);
                }
            }
            ContainerDTO? outerContainerDto = null;
            IEnumerable<Container> innerContainers;
            if (outerContainer == null)
            {
                innerContainers = StorageContainerRepository.GetAll();
            }
            else
            {
                outerContainerDto = ToDto(outerContainer);
                innerContainers = outerContainer.InnerContainers;
            }
            int page = input.Page == 0 ? 1 : input.Page;
            int pageSize = pageSizeService.GetPageSize();
            PaginationDTO pagination = new PaginationDTO(page, pageSize, innerContainers.Count());
            var output = new GetContainersOutput(Output.OutputResult.Success,
                                               string.Empty,
                                               outerContainerDto,
                                               TakePage(innerContainers, pageSize, page),
                                               pagination);
            _logger.LogInformation("User get containers from {containerLocation}",
                                   input.OuterContainerId.HasValue ? $"outer container {input.OuterContainerId.Value}" : "storage");
            return output;
        }

        protected override GetContainersOutput GenerateNoAccessOutput(GetContainersInput input)
        {
            return new GetContainersOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetContainersOutput GenerateOwnershipNotFoundOutput(GetContainersInput input)
        {
            return new GetContainersOutput(Output.OutputResult.FatalFail, _localizer["Case.UserOwnershipNotFound"]);
        }

        protected override GetContainersOutput GenerateStorageOwnershipNotFoundOutput(GetContainersInput input)
        {
            return new GetContainersOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetContainersOutput GenerateUserNotFoundOutput(GetContainersInput input)
        {
            return new GetContainersOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
