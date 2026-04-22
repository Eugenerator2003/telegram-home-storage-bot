using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ControllerIdAttribute : Attribute
    {
        public int Id { get; }

        public ControllerIdAttribute(int id)
        {
            Id = id;
        }
    }
}
