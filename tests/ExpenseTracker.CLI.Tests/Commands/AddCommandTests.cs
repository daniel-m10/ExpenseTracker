using CommandLine;
using ExpenseTracker.CLI.Commands;
using NSubstitute;

namespace ExpenseTracker.CLI.Tests.Commands
{
    [TestFixture]
    public class AddCommandTests
    {
        [Test]
        public void AddCommand_Parses_RequiredArguments_WithLongOptions_Success()
        {
            // Arrange
            var longRequiredOptions = new string[] { "add", "--description", "Lunch", "--amount", "20" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(longRequiredOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Amount, Is.EqualTo(20));
                Assert.That(result.Value.Category, Is.Null);
                Assert.That(result.Value.Date, Is.Null);
            }
        }

        [Test]
        public void AddCommand_Parses_RequiredArguments_WithShortOptions_Success()
        {
            // Arrange
            var shortRequiredOptions = new string[] { "add", "-d", "Lunch", "-a", "20" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(shortRequiredOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Amount, Is.EqualTo(20));
                Assert.That(result.Value.Category, Is.Null);
                Assert.That(result.Value.Date, Is.Null);
            }
        }

        [Test]
        public void AddCommand_Parses_AllArguments_IncludingOptional_Success()
        {
            // Arrange
            var allOptions = new string[] { "add", "--description", "Lunch", "--amount", "20", "--category", "Food", "--date", "2025-08-09" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(allOptions);

            // Assert
            Assert.That(result, Is.InstanceOf<Parsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Amount, Is.EqualTo(20));
                Assert.That(result.Value.Category, Is.EqualTo("Food"));
                Assert.That(result.Value.Date, Is.EqualTo("2025-08-09"));
            }
        }

        [Test]
        public void AddCommand_Parses_ShortAndLongOptions_Equivalently()
        {
            // Arrange
            var allOptions = new string[]
            {
                "add",
                "--description",
                "Lunch",
                "-a",
                "100",
                "--category",
                "Food",
                "-t",
                "2025/09/09"
            };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(allOptions);

            // Assert
            Assert.That(result.Errors, Is.Empty);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Amount, Is.EqualTo(100));
                Assert.That(result.Value.Category, Is.EqualTo("Food"));
                Assert.That(result.Value.Date, Is.EqualTo("2025/09/09"));
            }
        }

        [Test]
        public void AddCommand_Parses_ExtraWhitespace_Success()
        {
            // Assert
            var optionsWithExtraSpaces = new string[]
            { "add",
              "   -d  ",
              "  Lunch",
              "-a    ",
              "100"
            };

            var normalizeString = optionsWithExtraSpaces
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s.Trim())
                .ToArray();

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(normalizeString);

            // Assert
            Assert.That(result.Errors, Is.Empty);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Amount, Is.EqualTo(100));
            }
        }

        [Test]
        public void AddCommand_Parses_MixedOrderOfOptions_Success()
        {
            // Arrange
            var optionsMixed = new string[]
            {
                "add",
                "-t",
                "2025/09/09",
                "-d",
                "Lunch",
                "--category",
                "Food",
                "-a",
                "20"
            };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(optionsMixed);

            // Assert
            Assert.That(result.Errors, Is.Empty);
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Value.Amount, Is.EqualTo(20));
                Assert.That(result.Value.Description, Is.EqualTo("Lunch"));
                Assert.That(result.Value.Date, Is.EqualTo("2025/09/09"));
                Assert.That(result.Value.Category, Is.EqualTo("Food"));
            }
        }

        [Test]
        public void AddCommand_MissingDescription_Fails()
        {
            // Arrange
            var descriptionMissing = new string[] { "add", "-a", "20" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(descriptionMissing);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(
                    result.Errors.OfType<MissingRequiredOptionError>()
                        .Any(e => e.NameInfo.LongName == "description" &&
                        e.NameInfo.ShortName == "d"), Is.True);
            }
        }

        [Test]
        public void AddCommand_MissingAmount_Fails()
        {
            // Arrange
            var amountMissing = new string[] { "add", "-d", "Lunch" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(amountMissing);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(
                    result.Errors.OfType<MissingRequiredOptionError>()
                    .Any(e => e.NameInfo.LongName == "amount" &&
                    e.NameInfo.ShortName == "a"), Is.True);
            }
        }

        [Test]
        public void AddCommand_InvalidAmountType_Fails()
        {
            // Arrange
            var invalidAmountType = new string[] { "add", "-d", "Lunch", "-a", "twenty" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(invalidAmountType);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(
                    result.Errors.OfType<BadFormatConversionError>()
                    .Any(e => e.NameInfo.LongName == "amount" &&
                    e.NameInfo.ShortName == "a"), Is.True);
            }
        }

        [Test]
        public void AddCommand_UnknownOption_Fails()
        {
            // Arrange
            var unknownOption = new string[] { "add", "--desc", "Lunch", "-a", "20" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(unknownOption);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
                Assert.That(
                    result.Errors.OfType<UnknownOptionError>()
                    .Any(e => e.Token == "desc"), Is.True);
            }
        }

        [Test]
        public void AddCommand_EmptyArguments_Fails()
        {
            // Arrange
            var emptyArguments = new string[] { "add", "-d", "", "-a", "" };

            // Act
            var result = Parser.Default.ParseArguments<AddCommand>(emptyArguments);

            // Assert
            Assert.That(result, Is.InstanceOf<NotParsed<AddCommand>>());
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.Errors, Is.Not.Empty);
            }
        }
    }
}
