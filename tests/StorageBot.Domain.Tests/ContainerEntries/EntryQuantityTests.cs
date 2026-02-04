using StorageBot.Domain.ContainerEntries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.ContainerEntries
{
    [TestClass]
    public class EntryQuantityTests
    {
        [TestMethod]
        public void InitializeTest()
        {
            var type = new EntryQuantityType(false, "unit");
            var quantity = new EntryQuantity(type);
        }

        [TestMethod]
        public void InitializeNullTest()
        {
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var quantity = new EntryQuantity(null!);
            });
        }

        [TestMethod]
        public void InitializeIntegerValueTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            decimal value = 2;
            var quantity = new EntryQuantity(type, value);
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InitializeRealWithIntegerValueTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            decimal value = 1m;
            var quantity = new EntryQuantity(type, value);
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InitializeRealWithRealValueTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            decimal value = 1.63m;
            var quantity = new EntryQuantity(type, value);
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InitializeNegativeValueTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            decimal value = -10;
            var exception = Assert.ThrowsExactly<ArgumentOutOfRangeException>(() =>
            {
                var quantity = new EntryQuantity(type, value);
            });
        }

        [TestMethod]
        public void InitializeIntegerWithRealValueTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            decimal value = 2.1m;
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                var quantity = new EntryQuantity(type, value);
            });
        }

        [TestMethod]
        public void InititalizeIntegerWithIntegerValueStringTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            decimal value = 1;
            var quantity = new EntryQuantity(type, value.ToString());
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InititalizeRealWithIntegerValueStringTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            decimal value = 31;
            var quantity = new EntryQuantity(type, value.ToString());
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InititalizeRealWithRealValueStringTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            decimal value = 172.311m;
            var quantity = new EntryQuantity(type, value.ToString());
            Assert.AreEqual(type, quantity.QuantityType);
            Assert.AreEqual(value, quantity.Value);
        }

        [TestMethod]
        public void InitializeIntegerWithRealValueStringTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            decimal value = 2.1m;
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                var quantity = new EntryQuantity(type, value.ToString());
            });
        }

        [TestMethod]
        public void InitializeWithNotValidValueStringTest()
        {
            var type = new EntryQuantityType(isInteger: true, "unit");
            string value = "value";
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                var quantity = new EntryQuantity(type, value);
            });
        }

        [TestMethod]
        public void AddSameTypeTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var quantity1 = new EntryQuantity(type, 23.1m);
            var quantity2 = new EntryQuantity(type, 10.2m);
            var quantity3_12 = quantity1.Add(quantity2);
            var quantity3_21 = quantity2.Add(quantity1);
            var expectedValue = quantity1.Value + quantity2.Value;
            Assert.AreEqual(type, quantity3_12.QuantityType);
            Assert.AreEqual(expectedValue, quantity3_12.Value);
            Assert.AreEqual(type, quantity3_21.QuantityType);
            Assert.AreEqual(expectedValue, quantity3_21.Value);
        }

        [TestMethod]
        public void AddDifferentTypeTest()
        {
            var type1 = new EntryQuantityType(isInteger: false, "unit1");
            var quantity1 = new EntryQuantity(type1, 1m);
            var type2 = new EntryQuantityType(isInteger: false, "unit2");
            var quantity2 = new EntryQuantity(type2, 10.2m);
            Assert.ThrowsExactly<InvalidOperationException>(() => quantity1.Add(quantity2));
        }

        [TestMethod]
        public void AddNullTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var quantity = new EntryQuantity(type, 1m);
            Assert.ThrowsExactly<ArgumentNullException>(() => quantity.Add(null!));
        }

        [TestMethod]
        public void EqualsTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var value = 10.33m;
            var quantity1 = new EntryQuantity(type, value);
            var quantity2 = new EntryQuantity(type, value);
            Assert.AreEqual(quantity1, quantity2);
        }

        [TestMethod]
        public void EqualsDifferentValuesTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var value1= 15.001m;
            var value2= 0.12m;
            var quantity1 = new EntryQuantity(type, value1);
            var quantity2 = new EntryQuantity(type, value2);
            Assert.AreNotEqual(quantity1, quantity2);
        }

        [TestMethod]
        public void EqualsDifferentTypesTest()
        {
            var type1 = new EntryQuantityType(isInteger: false, "unit1");
            var type2 = new EntryQuantityType(isInteger: false, "unit2");
            var value1 = 10.33m;
            var value2 = 10.33m;
            var quantity1 = new EntryQuantity(type1, value1);
            var quantity2 = new EntryQuantity(type2, value2);
            Assert.AreNotEqual(quantity1, quantity2);
        }

        [TestMethod]
        public void GetHashCodeTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var value = 10.33m;
            var quantity1 = new EntryQuantity(type, value);
            var quantity2 = new EntryQuantity(type, value);
            Assert.AreEqual(quantity1.GetHashCode(), quantity2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentValuesTest()
        {
            var type = new EntryQuantityType(isInteger: false, "unit");
            var value1 = 15.001m;
            var value2 = 0.12m;
            var quantity1 = new EntryQuantity(type, value1);
            var quantity2 = new EntryQuantity(type, value2);
            Assert.AreNotEqual(quantity1.GetHashCode(), quantity2.GetHashCode());
        }

        [TestMethod]
        public void GetHashCodeDifferentTypesTest()
        {
            var type1 = new EntryQuantityType(isInteger: false, "unit1");
            var type2 = new EntryQuantityType(isInteger: false, "unit2");
            var value1 = 10.33m;
            var value2 = 10.33m;
            var quantity1 = new EntryQuantity(type1, value1);
            var quantity2 = new EntryQuantity(type2, value2);
            Assert.AreEqual(quantity1.GetHashCode(), quantity2.GetHashCode());
        }

    }
}
