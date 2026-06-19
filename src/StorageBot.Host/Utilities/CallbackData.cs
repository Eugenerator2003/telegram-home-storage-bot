using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Utilities
{
    public class CallbackData
    {
        public int ControllerId { get; }

        public int ActionId { get; }

        public int[] Values { get; }

        public CallbackData(int controllerId, int actionId, params int[] values)
            : this(controllerId, actionId, values.Length)
        {
            Array.Copy(values, Values, values.Length);
        }

        public CallbackData(int controllerId, int actionId, int valuesCount = 0)
        {
            if (controllerId < 0)
                throw new ArgumentException("Controller id must be non-negative", nameof(controllerId));
            if (actionId < 0)
                throw new ArgumentException("Action id must be non-negative", nameof(actionId));
            if (valuesCount < 0 || valuesCount > 10)
                throw new ArgumentOutOfRangeException("Values count must be from 0 to 10");
            ControllerId = controllerId;
            ActionId = actionId;
            Values = new int[valuesCount];
        }
    }
}
