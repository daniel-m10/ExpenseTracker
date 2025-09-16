using ExpenseTracker.Domain.ValueObjects;

namespace ExpenseTracker.Domain.Tests.ValueObjects
{
    [TestFixture]
    public class DateRangeTests
    {
        [Test]
        public void DateRange_Should_Create_With_Valid_Start_And_End()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 2);

            // Act
            var dateRange = new DateRange(start, end);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(dateRange, Is.Not.Null);
                Assert.That(dateRange.Start, Is.EqualTo(start));
                Assert.That(dateRange.End, Is.EqualTo(end));
            }
        }

        [Test]
        public void DateRange_Should_Throw_When_Start_Is_After_End()
        {
            // Arrange
            var start = new DateTime(2025, 1, 4);
            var end = new DateTime(2025, 1, 2);

            // Act & Assert
            var ex = Assert.Throws<ArgumentOutOfRangeException>(() => new DateRange(start, end));
            Assert.That(ex.Message, Does.Contain("Start should not be after End."));
        }

        [Test]
        public void DateRange_Contains_Should_Return_True_When_Date_Is_Within_Range()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 4);
            var dateRange = new DateRange(start, end);

            // Act
            var result = dateRange.Contains(new DateTime(2025, 1, 3));

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DateRange_Contains_Should_Return_False_When_Date_Is_Outside_Range()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 4);
            var dateRange = new DateRange(start, end);

            // Act
            var result = dateRange.Contains(new DateTime(2025, 1, 6));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void DateRange_Overlaps_Should_Return_True_When_Ranges_Overlap()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 6);
            var dateRange = new DateRange(start, end);
            var overlapDate = new DateRange(start.AddDays(1), end.AddDays(-1));

            // Act
            var result = dateRange.Overlaps(overlapDate);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void DateRange_Overlaps_Should_Return_False_When_Ranges_Do_Not_Overlap()
        {
            // Arrange
            var start = new DateTime(2025, 1, 3);
            var end = new DateTime(2025, 1, 6);
            var dateRange = new DateRange(start, end);
            var overlapDate = new DateRange(start.AddDays(-3), end.AddDays(-4));

            // Act
            var result = dateRange.Overlaps(overlapDate);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void DateRange_Duration_Should_Return_Correct_TimeSpan()
        {
            // Arrange
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 2);
            var timespan = end - start;
            var dateRange = new DateRange(start, end);

            // Act
            var result = dateRange.Duration();

            // Assert
            Assert.That(result, Is.EqualTo(timespan));
        }

        [Test]
        public void DateRange_Equality_Should_Be_Based_On_Values()
        {
            // Arrange & Act
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 2);
            var dateRange1 = new DateRange(start, end);
            var dateRange2 = new DateRange(start, end);

            // Assert
            Assert.That(dateRange1, Is.EqualTo(dateRange2));
        }

        [Test]
        public void DateRange_ToString_Should_Return_Formatted_String()
        {
            // Arrange & Act
            var start = new DateTime(2025, 1, 1);
            var end = new DateTime(2025, 1, 2);
            var dateRange = new DateRange(start, end);

            // Assert
            Assert.That(dateRange.ToString(), Is.EqualTo("2025-01-01 to 2025-01-02"));
        }
    }
}
