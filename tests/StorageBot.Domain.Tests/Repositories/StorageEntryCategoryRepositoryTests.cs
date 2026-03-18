using StorageBot.Domain.ContainerEntries;
using StorageBot.Domain.Exceptions.Repositories;
using StorageBot.Domain.Repositories;
using StorageBot.Domain.Storing;
using StorageBot.Domain.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageBot.Domain.Tests.Repositories
{
    [TestClass]
    public class StorageEntryCategoryRepositoryTests :
        StorageRepositoryTests<EntryCategory>
    {
        [TestMethod]
        public void CreateWithMainCategoryTest()
        {
            var storage = GetStorage();
            var mainCategory = new EntryCategory("main-category");
            storage.AddEntryCategory(mainCategory);
            var repository = new StorageEntryCategoryRepository(storage);
            string subCategoryName = "sub-category";
            repository.CreateWithMainCategory(subCategoryName, mainCategory.Id);
            var subCategory = mainCategory.Subcategories.FirstOrDefault(c => c.Name.Equals(subCategoryName));
            Assert.IsNotNull(subCategory);
        }

        [TestMethod]
        public void CreateWithMainCategoryExceptionTest()
        {
            var storage = GetStorage();
            var repository = new StorageEntryCategoryRepository(storage);
            Assert.ThrowsExactly<EntityNotFoundException>(() => repository.CreateWithMainCategory("sub-category", 999));
        }

        [TestMethod]
        public void GetByIdDeepTest()
        {
            var category1 = new EntryCategory("category1");
            category1.Id = 1;
            var category2 = new EntryCategory("category2");
            category2.Id = 2;
            var category3 = new EntryCategory("category3");
            category3.Id = 3;
            category1.AddSubcategory(category2);
            category2.AddSubcategory(category3);
            var storage = GetStorage();
            storage.AddEntryCategory(category1);
            var repository = new StorageEntryCategoryRepository(storage);
            var categoryById = repository.GetById(3);
            Assert.AreEqual(category3, categoryById);
        }

        [TestMethod]
        public override void CreateTest()
        {
            var categoryName = "category";
            var storage = GetStorage();
            var repository = new StorageEntryCategoryRepository(storage);
            repository.Create(categoryName);
            Assert.HasCount(1, storage.EntryCategories);
            var category = storage.EntryCategories.First();
            Assert.AreEqual(categoryName, category.Name);
        }

        [TestMethod]
        public override void DeleteExceptionTest()
        {
            base.DeleteExceptionTest();
        }

        [TestMethod]
        public override void DeleteTest()
        {
            base.DeleteTest();
        }

        [TestMethod]
        public override void GetAllTest()
        {
            base.GetAllTest();
        }

        [TestMethod]
        public override void GetByIdNullTest()
        {
            base.GetByIdNullTest();
        }

        [TestMethod]
        public override void GetByIdTest()
        {
            base.GetByIdTest();
        }

        protected override void AddEntityToStorage(Storage storage, EntryCategory entity)
        {
            storage.AddEntryCategory(entity);
        }

        protected override EntryCategory GetEntity()
        {
            return new EntryCategory("category");
        }

        protected override StorageRepository<EntryCategory> GetRepository(Storage storage)
        {
            return new StorageEntryCategoryRepository(storage);
        }

        protected override IEnumerable<EntryCategory> GetStorageTs(Storage storage)
        {
            return storage.EntryCategories;
        }
    }
}
