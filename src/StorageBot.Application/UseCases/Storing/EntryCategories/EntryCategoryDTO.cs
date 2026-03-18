using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.EntryCategories
{
    public class EntryCategoryDTO
    {
        public int Id { get; }

        public string Name { get; }

        public EntryCategoryDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
