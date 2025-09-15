using ExpenseTracker.Domain.Abstractions;
using ExpenseTracker.Domain.Extensions;
using ExpenseTracker.Domain.Models;
using NSubstitute;

namespace ExpenseTracker.Domain.Tests.Extensions
{
    [TestFixture]
    public class CategoryExtensionsTest
    {
        private ICategoryRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = Substitute.For<ICategoryRepository>();
        }

        [Test]
        public void CategoryExtensions_UpdateDetails_Should_Return_Success_For_Valid_Update()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category { Id = id, Name = "Old Name", Description = "Old Description", CreatedAt = DateTime.Today };

            // Act
            var result = category.UpdateDetails("New Name", "New Description");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
                Assert.That(result.Value?.Id, Is.EqualTo(id));
                Assert.That(result.Value?.Name, Is.EqualTo("New Name"));
                Assert.That(result.Value?.Description, Is.EqualTo("New Description"));
                Assert.That(result.Value?.CreatedAt, Is.EqualTo(DateTime.Today));
            }
        }

        [Test]
        public void CategoryExtensions_UpdateDetails_Should_Return_Failure_For_Invalid_Update()
        {
            // Arrange
            var category = new Category { Name = "N", Description = "D" };

            // Act
            var result = category.UpdateDetails(name: string.Empty, "New Description");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Does.Contain("Name is required."));
            }
        }

        [Test]
        public void CategoryExtensions_UpdateDetails_Should_Return_Failure_When_Category_Is_Null()
        {
            // Arrange
            Category category = null!;

            // Act
            var result = category.UpdateDetails("New Name", "New Description");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Does.Contain("Category cannot be null."));
            }
        }

        [Test]
        public void CategoryExtensions_UpdateDetails_Should_Not_Mutate_Original_Category()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category { Id = id, Name = "Old Name", Description = "Old Description", CreatedAt = DateTime.Today };

            // Act
            var result = category.UpdateDetails("New Name", "New Description");

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Errors, Is.Empty);
                Assert.That(result.Value, Is.Not.EqualTo(category));
            }
        }

        [Test]
        public async Task CategoryExtensions_IsNameUnique_Should_Return_True_When_Name_Is_Unique()
        {
            // Arrange
            var categories = new List<Category> { new() { Name = "A" }, new() { Name = "B" } };
            _repository.GetAllAsync().Returns(categories);
            var category = new Category();

            // Act
            var result = await category.IsNameUnique("C", _repository);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task CategoryExtensions_IsNameUnique_Should_Return_False_When_Name_Is_Not_Unique()
        {
            // Arrange
            var categories = new List<Category> { new() { Name = "A" }, new() { Name = "B" } };
            _repository.GetAllAsync().Returns(categories);
            var category = new Category();

            // Act
            var result = await category.IsNameUnique("A", _repository);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CategoryExtensions_IsNameUnique_Should_Be_Case_Insensitive()
        {
            // Arrange
            var categories = new List<Category> { new() { Name = "a" }, new() { Name = "B" } };
            _repository.GetAllAsync().Returns(categories);
            var category = new Category();

            // Act
            var result = await category.IsNameUnique("A", _repository);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public async Task CategoryExtensions_IsNameUnique_Should_Handle_Empty_Repository()
        {
            // Arrange
            _repository.GetAllAsync().Returns([]);
            var category = new Category();

            // Act
            var result = await category.IsNameUnique("A", _repository);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
