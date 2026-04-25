using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Routing.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ActionIdAttribute : Attribute
    {
        public int Id { get; }

        public ActionIdAttribute(int id)
        {
            if (id < 0)
                throw new ArgumentException("Id must be non-negative");
            Id = id;
        }
    }
}
