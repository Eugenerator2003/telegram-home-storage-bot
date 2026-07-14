using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetUserOwnership
{
    public class GetUserOwnershipOutput : Output
    {
        public int OwnershipId { get; }

        public GetUserOwnershipOutput(int ownershipId, OutputResult result, string message = "") : base(result, message)
        {
            OwnershipId = ownershipId;
        }
    }
}
