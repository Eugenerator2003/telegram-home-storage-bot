using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.CreateEntryCategory
{
    public class CreateEntryCategoryOutput : Output
    {
        public CreateEntryCategoryOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
