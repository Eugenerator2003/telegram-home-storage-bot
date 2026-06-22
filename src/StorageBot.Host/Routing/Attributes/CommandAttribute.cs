using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        public string Name { get; }

        public string? Description { get; }

        public CommandAttribute(string name, string? description)
        {
            Description = description;
            Name = name;
        }
    }
}
