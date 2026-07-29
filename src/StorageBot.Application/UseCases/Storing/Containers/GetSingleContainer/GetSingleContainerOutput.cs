using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetSingleContainer
{
    public class GetSingleContainerOutput : Output
    {
        public ContainerDTO Container { get; }

        public GetSingleContainerOutput(OutputResult result, string message, ContainerDTO container) : base(result, message)
        {
            Container = container;
        }

        public GetSingleContainerOutput(OutputResult result, string message = "") : base(result, message)
        {
            Container = new ContainerDTO(-1, string.Empty);
        }
    }
}
