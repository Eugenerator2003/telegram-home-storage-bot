using StorageBot.Host.Presentation;
using StorageBot.Host.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace StorageBot.Host.Tests.Infrastructure.Utilities
{
    [TestClass]
    public class CallbackDataTests
    {
        [TestMethod]
        public void InitializeWithZeroValuesCountTest()
        {
            var controllerId = 3;
            var actionId = 33;
            var valuesCount = 0;
            var callbackData = new CallbackData(controllerId, actionId, valuesCount);
            Assert.AreEqual(controllerId, callbackData.ControllerId);
            Assert.AreEqual(actionId, callbackData.ActionId);
            Assert.HasCount(valuesCount, callbackData.Values);
        }

        [TestMethod]
        public void InitializeWithValuesCountTest()
        {
            var controllerId = 3;
            var actionId = 33;
            var valuesCount = 6;
            var callbackData = new CallbackData(controllerId, actionId, valuesCount);
            Assert.AreEqual(controllerId, callbackData.ControllerId);
            Assert.AreEqual(actionId, callbackData.ActionId);
            Assert.HasCount(valuesCount, callbackData.Values);
            Assert.IsTrue(callbackData.Values.All(a => a == 0));
        }

        [TestMethod]
        public void InitializeWithValuesTest()
        {
            var controllerId = 3;
            var actionId = 33;
            int[] values = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
            var callbackData = new CallbackData(controllerId, actionId, values);
            Assert.AreEqual(controllerId, callbackData.ControllerId);
            Assert.AreEqual(actionId, callbackData.ActionId);
            Assert.HasCount(values.Length, callbackData.Values);
            CollectionAssert.AreEquivalent(values, callbackData.Values);
        }

        [TestMethod]
        public void InitializeNegativeControllerIdTest()
        {
            var controllerId = -10;
            var actionId = 33;
            int valuesCount = 0;
            var exception = Assert.ThrowsExactly<ArgumentException>(() => {
                var data = new CallbackData(controllerId, actionId, valuesCount);
            });
            Assert.Contains("controllerId", exception.Message);
        }

        [TestMethod]
        public void InitializeNegativeActionIdTest()
        {
            var controllerId = 30;
            var actionId = -33;
            var valuesCount = 0;
            var exception = Assert.ThrowsExactly<ArgumentException>(() => {
                var data = new CallbackData(controllerId, actionId, valuesCount);
            });
            Assert.Contains("actionId", exception.Message);
        }

        [TestMethod]
        public void InitializeNegativeValuesCountTest()
        {
            var controllerId = 1;
            var actionId = 0;
            var valuesCount = -3;
            var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                var data = new CallbackData(controllerId, actionId, valuesCount);
            });
        }

        [TestMethod]
        public void InitializeOutOfRangeValuesCountTest()
        {
            var controllerId = 1;
            var actionId = 0;
            var valuesCount = 12;
            var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                var data = new CallbackData(controllerId, actionId, valuesCount);
            });
        }

        [TestMethod]
        public void InitializeOutOfRangeValuesTest()
        {
            var controllerId = 1;
            var actionId = 0;
            int[] values = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11];
            var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => {
                var data = new CallbackData(controllerId, actionId, values);
            });
        }
    }
}
