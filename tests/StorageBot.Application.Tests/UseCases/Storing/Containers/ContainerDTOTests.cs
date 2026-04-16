using StorageBot.Application.UseCases.Storing.Containers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Application.Tests.UseCases.Storing.Containers
{
    [TestClass]
    public class ContainerDTOTests
    {
        [TestMethod]
        [DataRow(30, "")]
        [DataRow(2, "daashdksad")]
        public void IntializeTest(int id, string name)
        {
            var dto = new ContainerDTO(id, name);
            Assert.AreEqual(id, dto.Id);
            Assert.AreEqual(name, dto.Name);
        }

        [TestMethod]
        public void EqualsSameTest()
        {
            var id1 = 33;
            var name1 = "name";
            var dto1 = new ContainerDTO(id1, name1);
            var dto2 = new ContainerDTO(id1, name1);
            Assert.IsTrue(dto1.Equals(dto2));  
        }

        [TestMethod]
        public void EqualsDifferentTest()
        {
            var id1 = 33;
            var name1 = "name";
            var dto1 = new ContainerDTO(id1, name1);
            var id2 = 3;
            var name2 = "name";
            var dto2 = new ContainerDTO(id2, name2);
            Assert.IsFalse(dto1.Equals(dto2));
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            var id1 = 33;
            var name1 = "name";
            var dto1 = new ContainerDTO(id1, name1);
            var dto2 = new ContainerDTO(id1, name1);
            Assert.AreEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTest()
        {
            var id1 = 33;
            var name1 = "name";
            var dto1 = new ContainerDTO(id1, name1);
            var id2 = 3;
            var name2 = "name";
            var dto2 = new ContainerDTO(id2, name2);
            Assert.AreNotEqual(dto1.GetHashCode(), dto2.GetHashCode());
        }
    }
}
