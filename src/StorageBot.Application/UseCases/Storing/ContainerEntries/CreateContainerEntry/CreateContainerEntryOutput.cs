using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries.CreateContainerEntry
{
    public class CreateContainerEntryOutput : Output
    {
        public enum InputFailState
        {
            None,
            EmptyName,
            NotPositiveValue,
            NotIntegerValue,
        }

        public InputFailState State { get; set; }

        public CreateContainerEntryOutput(OutputResult result, string message = "", InputFailState failState = InputFailState.None) 
            : base(result, message)
        {
            State = failState;
        }
    }
}
