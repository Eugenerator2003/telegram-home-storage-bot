using StorageBot.Application.UseCases.Storing.QuantityTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.QuantityTypes
{
    [TestClass]
    public class EntryQuantityTypeDTOTests
    {
        [TestMethod]
        [DataRow(1, false, "unit")]
        [DataRow(2, true, "unit2")]
        [DataRow(5, false, "unit1303")]
        public void InitializeTest(int quantityTypeId, bool isInteger, string unitName)
        {
            var dto = new EntryQuantityTypeDTO(quantityTypeId, isInteger, unitName);
            Assert.AreEqual(quantityTypeId, dto.Id);
            Assert.AreEqual(isInteger, dto.IsInteger);
            Assert.AreEqual(unitName, dto.UnitName);
        }

        [TestMethod]
        public void EqualsTrueTest()
        {
            var dto1 = new EntryQuantityTypeDTO(1, false, "i.");
            var dto2 = new EntryQuantityTypeDTO(1, false, "i.");
            Assert.IsTrue(dto1.Equals(dto2));
        }

        [TestMethod]
        public void EqualsFalseTest()
        {
            var dto1 = new EntryQuantityTypeDTO(1, true, "i.");
            var dto2 = new EntryQuantityTypeDTO(1, false, "i.");
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void GetHashCodeSameTest()
        {
            var dto1 = new EntryQuantityTypeDTO(1, false, "i.");
            var dto2 = new EntryQuantityTypeDTO(1, false, "i.");
            Assert.AreEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTest()
        {
            var dto1 = new EntryQuantityTypeDTO(1, true, "i.");
            var dto2 = new EntryQuantityTypeDTO(1, false, "i.");
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }
    }
}
