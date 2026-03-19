using System;

namespace StorageBot.Application.UseCases.Storing.Containers.UpdateContainerName
{
    public class UpdateContainerNameOutput : Output
    {
        public UpdateContainerNameOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
