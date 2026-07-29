using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetOuterContainer
{
    public class GetOuterContainerOutput : Output
    {
        public ContainerDTO OuterContainer { get; }

        public GetOuterContainerOutput(OutputResult result, string message, ContainerDTO outerContainer) 
            : base(result, message)
        {
            OuterContainer = outerContainer;
        }

        public GetOuterContainerOutput(OutputResult result, string message = "") : base(result, message)
        {
            OuterContainer = new ContainerDTO(-1, string.Empty);
        }
    }
}
