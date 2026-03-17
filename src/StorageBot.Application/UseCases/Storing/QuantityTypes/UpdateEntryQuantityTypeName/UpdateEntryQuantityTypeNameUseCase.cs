using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.UseCases.Storing.QuantityTypes.AddEntryQuantityType;
using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.UpdateEntryQuantityTypeName
{
    public class UpdateEntryQuantityTypeNameUseCase
        : StorageEntryQuantityTypeUseCase<UpdateEntryQuantityTypeNameInput, UpdateEntryQuantityTypeNameOutput>
    {
        public UpdateEntryQuantityTypeNameUseCase(
            ILogger<UpdateEntryQuantityTypeNameUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override UpdateEntryQuantityTypeNameOutput ExecuteWithRepository(UpdateEntryQuantityTypeNameInput input)
        {
            var quantityType = QuantityTypeRepository.GetById(input.QuantityTypeId);
            if (quantityType == null)
            {
                _logger.LogError("User {userId} failed to update quantity type {quantityTypeId} (ownership {storageOwnershipId}): quantity type not found",
                                 input.UserId, 
                                 input.QuantityTypeId, 
                                 input.StorageOwnershipId);
                return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.FatalFail, _localizer["Case.QuantityTypeNotFound"]);
            }
            if (string.IsNullOrEmpty(input.UnitName))
            {
                _logger.LogError("User {userId} failed to update quantity type {quantityTypeId}: empty unit name", input.QuantityTypeId, input.UserId);
                return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.Fail, _localizer["Case.EmptyInputString"]);
            }
            quantityType.UnitName = input.UnitName;
            _logger.LogInformation("User {userId} update quantity type {quantityTypeId} with name {unitName}", input.UserId, input.QuantityTypeId, input.UnitName);
            return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.Success);
        }

        protected override UpdateEntryQuantityTypeNameOutput GenerateNoAccessOutput(UpdateEntryQuantityTypeNameInput input)
        {
            return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override UpdateEntryQuantityTypeNameOutput GenerateOwnershipNotFoundOutput(UpdateEntryQuantityTypeNameInput input)
        {
            return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override UpdateEntryQuantityTypeNameOutput GenerateStorageOwnershipNotFoundOutput(UpdateEntryQuantityTypeNameInput input)
        {
            return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override UpdateEntryQuantityTypeNameOutput GenerateUserNotFoundOutput(UpdateEntryQuantityTypeNameInput input)
        {
            return new UpdateEntryQuantityTypeNameOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
