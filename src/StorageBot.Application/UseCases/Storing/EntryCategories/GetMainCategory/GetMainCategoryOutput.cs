using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase
{
    public class GetMainCategoryOutput : Output
    {
        public EntryCategoryDTO MainCategory { get; set; }

        public GetMainCategoryOutput(OutputResult result, string message = "") 
            : base(result, message)
        {
            MainCategory = new EntryCategoryDTO(-1, string.Empty);
        }

        public GetMainCategoryOutput(OutputResult result, string message, EntryCategoryDTO mainCategory) 
            : base(result, message)
        {
            MainCategory = mainCategory;
        }
    }
}
