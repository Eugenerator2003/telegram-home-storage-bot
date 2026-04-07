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
    }
}
