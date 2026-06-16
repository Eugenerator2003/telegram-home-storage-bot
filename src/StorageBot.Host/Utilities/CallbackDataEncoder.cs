using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Host.Utilities
{
    public static class CallbackDataEncoder
    {
        public static string Encode(CallbackData data)
        {
            byte[] strBytes = new byte[64];
            Array.Copy(BitConverter.GetBytes(data.ControllerId), strBytes, 4);
            Array.Copy(BitConverter.GetBytes(data.ActionId), 0, strBytes, 4, 4);
            for(int i = 0; i < data.Values.Length; i++)
            {
                Array.Copy(BitConverter.GetBytes(data.Values[i]), 0, strBytes, (2 + i) * 4, 4);
            }
            return Encoding.UTF8.GetString(strBytes);
        }

        public static CallbackData Decode(string data)
        {
            byte[] strBytes = Encoding.UTF8.GetBytes(data);
            if (strBytes.Length > 64 || strBytes.Length < 8)
            {
                throw new ArgumentException("Bytes length of callback string must be from 8 to 64", nameof(data));
            }
            if (strBytes.Length % 4 != 0)
            {
                throw new ArgumentException("Bytes length of callback string must be a multiple of 4", nameof(data));
            }
            int controllerId = BitConverter.ToInt32(strBytes, 0);
            int actionId = BitConverter.ToInt32(strBytes, 4);
            int[] values = new int[strBytes.Length / 4 - 2];
            for(int i = 0; i < values.Length; i++)
            {
                values[i] = BitConverter.ToInt32(strBytes, (i + 2) * 4);
            }
            return new CallbackData(controllerId, actionId, values);
        }
    }
}
