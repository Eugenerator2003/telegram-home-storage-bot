using Newtonsoft.Json.Linq;
using StorageBot.Application.UseCases.Storing.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static Mono.CompilerServices.SymbolWriter.CodeBlockEntry;

namespace StorageBot.Application.Tests.UseCases.Storing.ContainerEntries
{
    [TestClass]
    public class ContainerEntryDTOTests
    {
        [TestMethod]
        [DataRow(1, "entry", 2, "un", false, 30)]
        [DataRow(2, "cont. entry", 10, "kg", true, 304399)]
        [DataRow(239, "", 999, "sht", false, 30.9999)]
        public void InitializeTest(int id, string name, int typeId, string unit, bool isInteger, double valueD)
        {
            var value = (decimal)valueD;
            var quantityDto = new EntryQuantityDTO(typeId, unit, isInteger, value);
            var dto = new ContainerEntryDTO(id, name, quantityDto);
            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(name, dto.Name);
            Assert.AreEqual(quantityDto, dto.EntryQuantity);
        }

        [TestMethod]
        public void EqualsSameTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1, quantityDto2);
            Assert.IsTrue(dto1.Equals(dto2));
        }

        [TestMethod]
        public void EqualsDifferentPropertyTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1 + " entry", quantityDto2);
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void EqualsDifferentDtoTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1 + " unit", isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1 + " entry", quantityDto2);
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void GetHashCodeSameTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1, quantityDto2);
            Assert.AreEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentPropertyTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1 + " entry", quantityDto2);
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentDtoTest()
        {
            var id1 = 20;
            var name1 = "entry";
            var typeId1 = 3;
            var unit1 = "unit";
            var isInteger1 = false;
            var value1 = 300m;
            var quantityDto1 = new EntryQuantityDTO(typeId1, unit1, isInteger1, value1);
            var quantityDto2 = new EntryQuantityDTO(typeId1, unit1 + " unit", isInteger1, value1);
            var dto1 = new ContainerEntryDTO(id1, name1, quantityDto1);
            var dto2 = new ContainerEntryDTO(id1, name1 + " entry", quantityDto2);
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }
    }
}
