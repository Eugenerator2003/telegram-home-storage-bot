using StorageBot.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Abstract
{
    [TestClass]
    public class BaseStorageModelTests
    {
        private class TestBaseStorageModel : BaseStorageModel
        {
            public TestBaseStorageModel()
            {
            }

            public TestBaseStorageModel(int id) : base(id)
            {
            }
        }

        [TestMethod]
        public void InitializeTest()
        {
            var model = new TestBaseStorageModel();
            Assert.AreEqual(0, model.Id);
        }

        [TestMethod]
        public void InitializeIdTest()
        {
            int id = 1;
            var model1 = new TestBaseStorageModel(id);
            Assert.AreEqual(id, model1.Id);
        }

        [TestMethod]
        public void GetHashcodeTest()
        {
            int id = 1;
            var model = new TestBaseStorageModel(id);
            Assert.AreEqual(id.GetHashCode(), model.GetHashCode());
        }

        [TestMethod]
        public void EqualsNullTest()
        {
            var model = new TestBaseStorageModel();
            Assert.IsFalse(model.Equals(null));
        }

        [TestMethod]
        public void EqualsObjectTest()
        {
            var model = new TestBaseStorageModel();
            Assert.IsFalse(model.Equals(new object()));
        }

        [TestMethod]
        public void EqualsSameReferenceModelsTest()
        {
            var model = new TestBaseStorageModel();
            Assert.IsTrue(model.Equals(model));
        }

        [TestMethod]
        public void EqualsNotSameReferenceModelsTest()
        {
            var model1 = new TestBaseStorageModel();
            var model2 = new TestBaseStorageModel();
            Assert.IsFalse(model1.Equals(model2));
        }

        [TestMethod]
        public void EqualsSameIdTest()
        {
            int id = 10;
            var model1 = new TestBaseStorageModel(id);
            var model2 = new TestBaseStorageModel(id);
            Assert.IsTrue(model1.Equals(model2));
        }
    }
}
