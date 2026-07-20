using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
using StorageBot.Application.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.QuantityTypes.GetSingleEntryQuantityType
{
    public class GetSingleEntryQuantityTypeUseCase
        : StorageEntryQuantityTypeUseCase<GetSingleEntryQuantityTypeInput, GetSingleEntryQuantityTypeOutput>
    {
        public GetSingleEntryQuantityTypeUseCase(
            ILogger<StorageEntryQuantityTypeUseCase<GetSingleEntryQuantityTypeInput, GetSingleEntryQuantityTypeOutput>> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetSingleEntryQuantityTypeOutput ExecuteWithRepository(GetSingleEntryQuantityTypeInput input)
        {
            using (_logger.BeginEntryQuantityTypeScope(input.QuantityTypeId))
            {
                var quantityType = QuantityTypeRepository.GetById(input.QuantityTypeId);
                if (quantityType != null)
                {
                    _logger.LogInformation("User read quantity type");
                    var dto = new EntryQuantityTypeDTO(quantityType.Id, quantityType.IsInteger, quantityType.UnitName);
                    return new GetSingleEntryQuantityTypeOutput(dto, Output.OutputResult.Success, string.Empty);
                }
                else
                {
                    _logger.LogError("User failed to read quantity type: quantity type not found");
                    return new GetSingleEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.QuantityTypeNotFound"]);
                }
            }
        }

        protected override GetSingleEntryQuantityTypeOutput GenerateNoAccessOutput(GetSingleEntryQuantityTypeInput input)
        {
            return new GetSingleEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetSingleEntryQuantityTypeOutput GenerateOwnershipNotFoundOutput(GetSingleEntryQuantityTypeInput input)
        {
            return new GetSingleEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
        }

        protected override GetSingleEntryQuantityTypeOutput GenerateStorageOwnershipNotFoundOutput(GetSingleEntryQuantityTypeInput input)
        {
            return new GetSingleEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.StorageOwnershipNotFound"]);
        }

        protected override GetSingleEntryQuantityTypeOutput GenerateUserNotFoundOutput(GetSingleEntryQuantityTypeInput input)
        {
            return new GetSingleEntryQuantityTypeOutput(Output.OutputResult.FatalFail, _localizer["Case.UserNotFound"]);
        }
    }
}
