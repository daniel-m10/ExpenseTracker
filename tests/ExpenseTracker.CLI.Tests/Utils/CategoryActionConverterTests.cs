using ExpenseTracker.CLI.Utils;
using System.ComponentModel;
using System.Globalization;

namespace ExpenseTracker.CLI.Tests.Utils
{
    [TestFixture]
    public class CategoryActionConverterTests
    {
        [Test]
        public void CategoryActionConverter_Converts_ValidListAction_Success()
        {
            // Act
            var result = GetConvertedAction("list");

            // Assert
            Assert.That(result, Is.EqualTo(CategoryAction.list));
        }

        [Test]
        public void CategoryActionConverter_Converts_ValidAddAction_Success()
        {
            // Act
            var result = GetConvertedAction("add");

            // Assert
            Assert.That(result, Is.EqualTo(CategoryAction.add));
        }

        [Test]
        public void CategoryActionConverter_Converts_ValidDeleteAction_Success()
        {
            // Act
            var result = GetConvertedAction("delete");

            // Assert
            Assert.That(result, Is.EqualTo(CategoryAction.delete));
        }

        [TestCase("List", CategoryAction.list)]
        [TestCase("ADD", CategoryAction.add)]
        [TestCase("DelEtE", CategoryAction.delete)]

        public void CategoryActionConverter_Converts_ValidAction_CaseInsensitive_Success(string action, CategoryAction categoryAction)
        {
            // Act
            var result = GetConvertedAction(action);

            // Assert
            Assert.That(result, Is.EqualTo(categoryAction));
        }

        [Test]
        public void CategoryActionConverter_InvalidAction_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GetConvertedAction("invalid"));
            Assert.That(ex.Message, Does.Contain("Invalid action"));
        }

        [Test]
        public void CategoryActionConverter_NonStringValue_ThrowsArgumentException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() => GetConvertedAction(1));
            Assert.That(ex.Message, Is.EqualTo("Action must be a string."));
        }

        private static object? GetConvertedAction(object action)
        {
            return new CategoryActionConverter().ConvertFrom(null, CultureInfo.InvariantCulture, action);
        }
    }
}
