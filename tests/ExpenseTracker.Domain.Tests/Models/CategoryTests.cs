using ExpenseTracker.Domain.Models;
using System.Xml.Linq;

namespace ExpenseTracker.Domain.Tests.Models
{
    [TestFixture]
    public class CategoryTests
    {
        private Category _category;

        [SetUp]
        public void SetUp()
        {
            _category = new Category();
        }

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
            _category.Name = name;

            // Assert
            Assert.That(_category.Name, Is.Not.Null);
            Assert.That(_category.Name, Is.EqualTo(name));
        }

        [Test]
        public void Category_Should_SetAndGet_Description()
        {
            // Arrange
            var description = "Description Test";
            _category.Description = description;

            // Assert
            Assert.That(_category.Description, Is.Not.Null);
            Assert.That(_category.Description, Is.EqualTo(description));
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
            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(_category.Id, Is.Default);
                Assert.That(_category.Name, Is.EqualTo(string.Empty));
                Assert.That(_category.Description, Is.EqualTo(string.Empty));
                Assert.That(_category.CreatedAt, Is.Default);
            }
        }
    }
}
