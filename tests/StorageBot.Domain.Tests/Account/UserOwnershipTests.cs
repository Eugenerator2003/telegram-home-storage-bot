using StorageBot.Domain.Account;
using StorageBot.Domain.Exceptions.Account;
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
        }

        [TestMethod]
        public void AddPersonalStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = ownership.PersonalStorage;
            var exception = Assert.ThrowsExactly<AddPersonalStorageAsSharedException>(() =>
            {
                ownership.AddOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void AddAccessibleStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);
            ownership.AddOtherStorage(storage);
            var exception = Assert.ThrowsExactly<UserAlreadyHaveStorageAccessException>(() =>
            {
                ownership.AddOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void RemovePersonalStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = ownership.PersonalStorage;
            var exception = Assert.ThrowsExactly<RemovePersonalStorageException>(() =>
            {
                ownership.RemoveOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
        }

        [TestMethod]
        public void RemoveNonAccessibleStorageTest()
        {
            var ownership = new UserOwnership(new TestUser());
            var storage = new Storage(ownership);
            var exception = Assert.ThrowsExactly<NoStorageToRemoveException>(() =>
            {
                ownership.RemoveOtherStorage(storage);
            });
            Assert.IsNotNull(exception);
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
        public void AddOtherStorageNullTest()
        {
            var ownership = new UserOwnership(new TestUser());
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                ownership.AddOtherStorage(null!);
            });
        }

        [TestMethod]
        public void RemoveOtherStorageNullTest()
        {
            var ownership = new UserOwnership(new TestUser());
            Assert.ThrowsExactly<ArgumentNullException>(() =>
            {
                ownership.RemoveOtherStorage(null!);
            });
        }
    }
}
