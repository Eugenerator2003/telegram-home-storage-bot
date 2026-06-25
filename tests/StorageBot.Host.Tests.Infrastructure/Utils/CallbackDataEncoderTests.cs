using StorageBot.Host.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Utils
{
    [TestClass]
    public class CallbackDataEncoderTests
    {
        [TestMethod]
        public void EncodeWithoutValuesTest()
        {
            var controllerId = 3;
            var actionId = 5;
            CallbackData data = new CallbackData(controllerId, actionId);
            var resultString = CallbackDataEncoder.Encode(data);
            var bytes = Convert.FromBase64String(resultString);
            Assert.HasCount(2 * sizeof(int), bytes);
            Assert.AreEqual(controllerId, BitConverter.ToInt32(bytes, 0));
            Assert.AreEqual(actionId, BitConverter.ToInt32(bytes, sizeof(int)));
        }

        [TestMethod]
        public void EncodeWithValuesTest()
        {
            var controllerId = 3;
            var actionId = 5;
            int[] values = [1, 78, 3192, 3921300, 3995, 8777886];
            CallbackData data = new CallbackData(controllerId, actionId, values);
            var resultString = CallbackDataEncoder.Encode(data);
            var bytes = Convert.FromBase64String(resultString);
            Assert.HasCount((2 + values.Length) * sizeof(int), bytes);
            Assert.AreEqual(controllerId, BitConverter.ToInt32(bytes, 0));
            Assert.AreEqual(actionId, BitConverter.ToInt32(bytes, sizeof(int)));
            for(int i = 0; i < values.Length; i++)
            {
                Assert.AreEqual(values[i], BitConverter.ToInt32(bytes, (2 + i) * sizeof(int)));
            }
        }

        [TestMethod]
        public void DecodeWithoutValuesTest()
        {
            var controllerId = 99;
            var actionId = 3;
            var bytes = new byte[2 * sizeof(int)];
            Buffer.BlockCopy(BitConverter.GetBytes(controllerId), 0, bytes, 0, sizeof(int));
            Buffer.BlockCopy(BitConverter.GetBytes(actionId), 0, bytes, sizeof(int), sizeof(int));
            var base64 = Convert.ToBase64String(bytes);
            var resultData = CallbackDataEncoder.Decode(base64);
            Assert.AreEqual(controllerId, resultData.ControllerId);
            Assert.AreEqual(actionId, resultData.ActionId);
            Assert.HasCount(0, resultData.Values);
        }

        [TestMethod]
        public void DecodeWithValuesTest()
        {
            var controllerId = 99;
            var actionId = 3;
            int[] values = [1, 78, 3192, 3921300, 3995, 8777886];
            var bytes = new byte[(2 + values.Length) * sizeof(int)];
            Buffer.BlockCopy(BitConverter.GetBytes(controllerId), 0, bytes, 0, sizeof(int));
            Buffer.BlockCopy(BitConverter.GetBytes(actionId), 0, bytes, sizeof(int), sizeof(int));
            for(int i = 0; i < values.Length; i++)
            {
                Buffer.BlockCopy(BitConverter.GetBytes(values[i]), 0, bytes, (2 + i) * sizeof(int), sizeof(int));
            }
            var base64 = Convert.ToBase64String(bytes);
            var resultData = CallbackDataEncoder.Decode(base64);
            Assert.AreEqual(controllerId, resultData.ControllerId);
            Assert.AreEqual(actionId, resultData.ActionId);
            Assert.HasCount(values.Length, resultData.Values);
            CollectionAssert.AreEquivalent(values, resultData.Values);
        }
    }
}
