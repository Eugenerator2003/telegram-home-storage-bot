using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.UseCases.Storing.Containers
{
    public class ContainerDTO
    {
        public int Id { get; }
        public string Name { get; }

        public ContainerDTO(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
