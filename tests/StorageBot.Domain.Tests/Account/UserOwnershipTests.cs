using StorageBot.Domain.Account;
using StorageBot.Domain.Storing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Account
{
    [TestClass]
    public class UserOwnershipTests
    {
        private class TestUserOnwership : UserOwnership
        {
            public TestUserOnwership(User user, IEnumerable<Storage> storages) 
                : base(user, storages) { }
        }

        private class TestStorage : Storage
        {
            public TestStorage()
                : base() { }
        }

        private class TestUser : User
        {
            public TestUser()
                : base() { }
        }

        [TestMethod]
        public void InitializeTest()
        {
            var user = new TestUser();
            var ownership = new UserOwnership(user);
            Assert.AreEqual(user, ownership.User);
            Assert.AreEqual(0, ownership.PersonalStorage.Id);
            Assert.IsEmpty(ownership.OtherStorages);
            Assert.AreEqual(ownership, ownership.PersonalStorage.UserOwnership);
        }

        [TestMethod]
        public void InitializeExceptionTest()
        {
            User? user = null;
            var exception = Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                var ownership = new UserOwnership(user!);
            });
            Assert.IsNotNull(exception);
            Assert.Contains(nameof(user), exception.Message);
        }

        [TestMethod]
        public void AddPersonalStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = ownership.PersonalStorage;
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                ownership.AddOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
            Assert.Contains(nameof(storage), exception.Message);
        }

        [TestMethod]
        public void AddAccessibleStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);
            ownership.AddOtherStorage(storage);
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                ownership.AddOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
            Assert.Contains(nameof(storage), exception.Message);
        }

        [TestMethod]
        public void RemovePersonalStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = ownership.PersonalStorage;
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                ownership.RemoveOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
            Assert.Contains(nameof(storage), exception.Message);
        }

        [TestMethod]
        public void RemoveNonAccessibleStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);
            var exception = Assert.ThrowsExactly<ArgumentException>(() =>
            {
                ownership.RemoveOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
            Assert.Contains(nameof(storage), exception.Message);
        }

        [TestMethod]
        public void AddOtherStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);
            ownership.AddOtherStorage(storage);
            Assert.HasCount(1, ownership.OtherStorages);
            Assert.AreEqual(storage, ownership.OtherStorages.ElementAt(0));
        }

        [TestMethod]
        public void AddRemoveOtherStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);

            Assert.IsEmpty(ownership.OtherStorages);
            ownership.AddOtherStorage(storage);
            Assert.HasCount(1, ownership.OtherStorages);
            Assert.AreEqual(storage, ownership.OtherStorages.ElementAt(0));
            ownership.RemoveOtherStorage(storage);
            Assert.IsEmpty(ownership.OtherStorages);
        }

        [TestMethod]
        public void RemoveOtherStorageTest()
        {
            List<Storage> storages = [new TestStorage(), new TestStorage(), new TestStorage()];
            int[] indices = [1, 1, 0];
            var ownership = new TestUserOnwership(new TestUser(), storages);
            for(int i = 0; i < 3; i++)
            {
                int index = indices[i];
                ownership.RemoveOtherStorage(storages[index]);
                storages.Remove(storages[index]);
                Assert.HasCount(3 - i - 1, ownership.OtherStorages);
                CollectionAssert.AreEqual(storages, ownership.OtherStorages.ToArray());
            }
        }
    }
}
