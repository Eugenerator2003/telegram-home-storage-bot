using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using StorageBot.Application.Data.Repositories;
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
        public GetEntryQuantityTypesUseCase(
            ILogger<GetEntryQuantityTypesUseCase> logger, 
            IStringLocalizer localizer, 
            UserRepository userRepository, 
            UserOwnershipRepository ownershipRepository) 
            : base(logger, localizer, userRepository, ownershipRepository)
        {
        }

        protected override GetEntryQuantityTypesOutput ExecuteWithRepository(GetEntryQuantityTypesInput input)
        {
            var typeDTOs = QuantityTypeRepository.GetAll()
                                              .Select(t => new EntryQuantityTypeDTO(t.Id, t.IsInteger, t.UnitName));
            _logger.LogInformation("User {userId} get quantity types of personal storage ownership {ownershipId}", input.UserId, input.StorageOwnershipId);
            return new GetEntryQuantityTypesOutput(typeDTOs, Output.OutputResult.Success);
        }

        protected override GetEntryQuantityTypesOutput GenerateNoAccessOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.NoStorageAccess"]);
        }

        protected override GetEntryQuantityTypesOutput GenerateOwnershipNotFoundOutput(GetEntryQuantityTypesInput input)
        {
            return new GetEntryQuantityTypesOutput(Output.OutputResult.FatalFail, _localizer["Case.OwnershipNotFound"]);
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
