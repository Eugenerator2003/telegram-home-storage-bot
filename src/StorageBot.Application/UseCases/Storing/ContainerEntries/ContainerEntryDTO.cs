using StorageBot.Application.UseCases.Storing.QuantityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.ContainerEntries
{
    public class ContainerEntryDTO
    {
        public int Id { get; }

        public string Name { get; }

        public EntryQuantityDTO EntryQuantity { get; }

        public ContainerEntryDTO(int id, string name, EntryQuantityDTO quantityDto)
        {
            Id = id;
            Name = name;
            EntryQuantity = quantityDto;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not ContainerEntryDTO dto)
            {
                return false;
            }
            return Id == dto.Id &&
                   Name == dto.Name &&
                   EntryQuantity.Equals(dto.EntryQuantity);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name, EntryQuantity);
        }
    }
}
