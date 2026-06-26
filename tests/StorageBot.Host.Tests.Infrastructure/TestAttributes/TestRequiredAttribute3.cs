using StorageBot.Host.Routing.Attributes.Actions.Required;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.TestAttributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestRequiredAttribute3 : RequiredActionAttribute
    {
    }
}
