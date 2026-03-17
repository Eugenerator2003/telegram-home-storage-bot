using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.DeleteEntryCategoryUseCase
{
    public class DeleteEntryCategoryOutput : Output
    {
        public DeleteEntryCategoryOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
        }
    }
}
