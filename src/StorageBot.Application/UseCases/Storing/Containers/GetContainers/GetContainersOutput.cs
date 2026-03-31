using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers.GetContainers
{
    public class GetContainersOutput : Output
    {
        public ContainerDTO? OuterContainer { get; }

        public ContainerDTO[] InnerContainers { get; }

        public PaginationDTO Pagination { get; }

        public GetContainersOutput(OutputResult result, 
                                  string message, 
                                  ContainerDTO? outerContainer, 
                                  ContainerDTO[] innerContainers, 
                                  PaginationDTO pagination)
            : base(result, message)
        {
            OuterContainer = outerContainer;
            InnerContainers = innerContainers;
            Pagination = pagination;
        }

        public GetContainersOutput(OutputResult result, string message)
            : base(result, message)
        {
            InnerContainers = Array.Empty<ContainerDTO>();
            Pagination = new PaginationDTO(0, 0, 0);
        }
    }
}
