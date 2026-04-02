using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    public class CreateContainerEntryUseCase
        : ContainerEntryUseCase<CreateContainerEntryInput, CreateContainerEntryOutput>
    {
        public CreateContainerEntryUseCase(ILogger<CreateContainerEntryUseCase> logger, 
                                           IStringLocalizer localizer, 
                                           UserRepository userRepository, 
                                           UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override CreateContainerEntryOutput ExecuteWithContainer(CreateContainerEntryInput input)
        {
            var quantityType = StorageEntryQuantityTypeRepository.GetById(input.EntryQuantityTypeId);
            if (quantityType == null)
            {
                _logger.LogError("User failed to create container entry: quantity type not found");
                return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.QuantityTypeNotFound"]);
            }
            if (string.IsNullOrEmpty(input.EntryName))
            {
                _logger.LogError("User failed to create container entry: empty entry name");
                return new CreateContainerEntryOutput(Output.OutputResult.Fail, _localizer["UseCase.EmptyInputString"]);
            }
            var quantity = new EntryQuantity(quantityType, 1);
            ContainerEntryRepository.Create(input.EntryName, quantity);
            _logger.LogInformation("User create container entry");
            return new CreateContainerEntryOutput(Output.OutputResult.Success);
        }

        protected override CreateContainerEntryOutput GenerateContainerNotFoundOutput()
        {
            return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.ContainerNotFound"]);
        }

        protected override CreateContainerEntryOutput GenerateNoAccessOutput(CreateContainerEntryInput input)
        {
            return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.NoStorageAccess"]);
        }

        protected override CreateContainerEntryOutput GenerateOwnershipNotFoundOutput(CreateContainerEntryInput input)
        {
            return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.OwnershipNotFound"]);
        }

        protected override CreateContainerEntryOutput GenerateStorageOwnershipNotFoundOutput(CreateContainerEntryInput input)
        {
            return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.StorageOwnershipNotFound"]);
        }

        protected override CreateContainerEntryOutput GenerateUserNotFoundOutput(CreateContainerEntryInput input)
        {
            return new CreateContainerEntryOutput(Output.OutputResult.FatalFail, _localizer["UseCase.UserNotFound"]);
        }
    }
}
