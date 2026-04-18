using StorageBot.Application.UseCases.Storing.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries
{
    [TestClass]
    public class EntryQuantityDTOTests
    {
        [TestMethod]
        [DataRow(1, "231", false, 13.2)]
        [DataRow(299, "", true, 1)]
        [DataRow(98765, "999", true, 123456)]
        public void InitializeTest(int typeId, string unit, bool isInteger, double valueD)
        {
            var value = (decimal)valueD;
            var dto = new EntryQuantityDTO(typeId, unit, isInteger, value);
            Assert.AreEqual(typeId, dto.TypeId);
            Assert.AreEqual(unit, dto.Unit);
            Assert.AreEqual(isInteger, dto.IsInteger);
            Assert.AreEqual(value, dto.Value);
        }

        [TestMethod]
        public void EqualsSameTest()
        {
            var typeId1 = 1;
            var unit1 = "un.";
            var isInteger1 = false;
            var value1 = 30M;
            var dto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            Assert.IsTrue(dto1.Equals(dto2));
        }

        [TestMethod]
        public void EqualsDifferentTest()
        {
            var typeId1 = 1;
            var unit1 = "un.";
            var isInteger1 = false;
            var value1 = 30M;
            var dto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto2 = new EntryQuantityDTO(typeId1, unit1 + "...", isInteger1, value1);
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void GetHashCodeSameTest()
        {
            var typeId1 = 1;
            var unit1 = "un.";
            var isInteger1 = false;
            var value1 = 30M;
            var dto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            Assert.AreEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTest()
        {
            var typeId1 = 1;
            var unit1 = "un.";
            var isInteger1 = false;
            var value1 = 30M;
            var dto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1 || true, value1);
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }
    }
}
