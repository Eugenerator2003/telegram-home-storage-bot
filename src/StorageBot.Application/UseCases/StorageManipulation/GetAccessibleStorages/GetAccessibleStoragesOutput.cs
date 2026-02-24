using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.StorageManipulation.GetAccessibleStorages
{
    public class GetAccessibleStoragesOutput : Output
    {
        public IReadOnlyCollection<int> UserIds { get; }

        public GetAccessibleStoragesOutput(OutputResult result, string message = "")
            : base(result, message)
        {
            UserIds = [];
        }

        public GetAccessibleStoragesOutput(OutputResult result, IList<int> userIds, string message = "") 
            : base(result, message)
        {
            ArgumentNullException.ThrowIfNull(userIds);
            UserIds = userIds.AsReadOnly();
        }
    }
}
