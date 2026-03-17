using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType
{
    public class CreateEntryQuantityTypeUseCase
        : StorageEntryQuantityTypeUseCase<CreateEntryQuantityTypeInput, CreateEntryQuantityTypeOutput>
    {
        public CreateEntryQuantityTypeUseCase(
            ILogger<CreateEntryQuantityTypeUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override CreateEntryQuantityTypeOutput ExecuteWithRepository(CreateEntryQuantityTypeInput input)
        {
            if (string.IsNullOrEmpty(input.UnitName))
            {
                _logger.LogError("User {userId} failed to create quantity type: empty unit name", input.UserId);
                return new CreateEntryQuantityTypeOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
            }
            var quantityType = new EntryQuantityType(input.IsInteger, input.UnitName);
            QuantityTypeRepository.Create(quantityType);
            _logger.LogInformation("User {userId} create quantity type {unitName} to storage ownership {storageOwnershipId}", input.UserId, input.UnitName, input.StorageOwnershipId);
            return new CreateEntryQuantityTypeOutput(Output.OutputResult.Success);
        }

        protected override CreateEntryQuantityTypeOutput GenerateNoAccessOutput(CreateEntryQuantityTypeInput input)
        {
            return new CreateEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override CreateEntryQuantityTypeOutput GenerateOwnershipNotFoundOutput(CreateEntryQuantityTypeInput input)
        {
            return new CreateEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override CreateEntryQuantityTypeOutput GenerateStorageOwnershipNotFoundOutput(CreateEntryQuantityTypeInput input)
        {
            return new CreateEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override CreateEntryQuantityTypeOutput GenerateUserNotFoundOutput(CreateEntryQuantityTypeInput input)
        {
            return new CreateEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
