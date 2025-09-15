using ExpenseTracker.Domain.Models;

namespace ExpenseTracker.Domain.Tests.Models
{
    [TestFixture]
    public class CategoryTests
    {
        [Test]
        public void Category_Should_SetAndGet_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category = new Category
            {
                Id = id
            };

            // Assert
            Assert.That(category.Id, Is.Not.Default);
            Assert.That(category.Id, Is.EqualTo(id));
        }

        [Test]
        public void Category_Should_SetAndGet_Name()
        {
            // Arrange
            var name = "Category Test";
            var category = new Category
            {
                Name = name
            };

            // Assert
            Assert.That(category.Name, Is.Not.Null);
            Assert.That(category.Name, Is.EqualTo(name));
        }

        [Test]
        public void Category_Should_SetAndGet_Description()
        {
            // Arrange
            var description = "Description Test";
            var category = new Category
            {
                Description = description
            };

            // Assert
            Assert.That(category.Description, Is.Not.Null);
            Assert.That(category.Description, Is.EqualTo(description));
        }

        [Test]
        public void Category_Should_SetAndGet_CreatedAt()
        {
            // Arrange
            var createdAt = new DateTime(2025, 1, 1);
            var category = new Category
            {
                CreatedAt = createdAt
            };

            // Act
            Assert.That(category.CreatedAt, Is.Not.Default);
            Assert.That(category.CreatedAt, Is.EqualTo(createdAt));
        }

        [Test]
        public void Category_Should_Have_NullOrDefault_Values_When_NotSet()
        {
            // Arrange
            var category = new Category();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(category.Id, Is.Default);
                Assert.That(category.Name, Is.EqualTo(string.Empty));
                Assert.That(category.Description, Is.EqualTo(string.Empty));
                Assert.That(category.CreatedAt, Is.Default);
            }
        }

        [Test]
        public void Category_Equality_Should_Be_Based_On_Values()
        {
            // Arrange
            var id = Guid.NewGuid();
            var category1 = new Category { Id = id, Name = "N", Description = "D", CreatedAt = DateTime.Today };
            var category2 = new Category { Id = id, Name = "N", Description = "D", CreatedAt = DateTime.Today };

            //Assert
            Assert.That(category1, Is.EqualTo(category2));
        }

        [Test]
        public void Category_Should_Return_New_Instance_When_Using_With_Expression()
        {
            // Arrange
            var category = new Category { Name = "Old" };
            var updated = category with { Name = "New" };

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(updated.Name, Is.EqualTo("New"));
                Assert.That(category.Name, Is.EqualTo("Old"));
            }
        }
    }
}
