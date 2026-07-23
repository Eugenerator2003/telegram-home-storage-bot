using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories.GetMainCategoryUseCase
{
    public class GetMainCategoryInput : StorageInput
    {
        public int SubcategoryId { get; }

        public GetMainCategoryInput(int userId, int storageOwnershipId, int subcategoryId) 
            : base(userId, storageOwnershipId)
        {
            SubcategoryId = subcategoryId;
        }
    }
}
