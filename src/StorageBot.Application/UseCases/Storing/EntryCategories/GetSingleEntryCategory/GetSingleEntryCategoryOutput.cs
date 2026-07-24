using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetSingleEntryCategory
{
    public class GetSingleEntryCategoryOutput : Output
    {
        public EntryCategoryDTO EntryCategory { get; }

        public GetSingleEntryCategoryOutput(OutputResult result, string message = "") : base(result, message)
        {
            EntryCategory = new EntryCategoryDTO(-1, string.Empty);
        }

        public GetSingleEntryCategoryOutput(OutputResult result, string message, EntryCategoryDTO entryCategory) : base(result, message)
        {
            EntryCategory = entryCategory;
        }
    }
}
