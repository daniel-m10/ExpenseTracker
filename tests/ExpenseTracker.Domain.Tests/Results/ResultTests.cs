using ExpenseTracker.Domain.Results;

namespace ExpenseTracker.Domain.Tests.Results
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void ResultT_Should_Create_Success_With_Value()
        {
            // Arrange
            int testValue = 10;

            // Act
            var result = Result<int>.Success(testValue);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.EqualTo(testValue));
                Assert.That(result.Errors, Is.Empty);
            }
        }

        [Test]
        public void ResultT_Should_Create_Failure_With_Single_Error()
        {
            // Arrange
            string errorMessage = "Something went wrong.";

            // Act
            var result = Result<object>.Failure(errorMessage);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain(errorMessage));
                Assert.That(result.Errors, Is.InstanceOf<IReadOnlyList<string>>());
            }
        }

        [Test]
        public void ResultT_Should_Create_Failure_With_Multiple_Errors()
        {
            // Arrange
            var errors = new string[] { "Error 1", "Error 2", "Error 3" };

            // Act
            var result = Result<object>.Failure(errors);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Has.Count.EqualTo(3));
                Assert.That(result.Errors, Does.Contain("Error 1"));
                Assert.That(result.Errors, Does.Contain("Error 2"));
                Assert.That(result.Errors, Does.Contain("Error 3"));
                Assert.That(result.Errors, Is.InstanceOf<IReadOnlyList<string>>());
            }
        }

        [Test]
        public void ResultT_Should_Create_Failure_With_Enumerable_Errors()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2", "Error 3" };

            // Act
            var result = Result<object>.Failure(errors);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Has.Count.EqualTo(3));
                Assert.That(result.Errors, Does.Contain("Error 1"));
                Assert.That(result.Errors, Does.Contain("Error 2"));
                Assert.That(result.Errors, Does.Contain("Error 3"));
                Assert.That(result.Errors, Is.InstanceOf<IReadOnlyList<string>>());
            }
        }

        [Test]
        public void ResultTS_Failure_Should_Handle_Empty_Error_Array()
        {
            // Act
            var result = Result<object>.Failure();

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Is.Empty);
            }
        }

        [Test]
        public void ResultT_Success_Should_Work_With_Reference_Type_Value()
        {
            // Arrange
            var stringReferenceType = "test";

            // Act
            var result = Result<string>.Success(stringReferenceType);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.InstanceOf<string>());
            }
        }

        [Test]
        public void ResultT_Errors_Should_Never_Be_Null_For_Success()
        {
            // Act
            var result = Result<int>.Success(int.MaxValue);

            // Assert
            Assert.That(result.Errors, Is.Not.Null);
        }

        [Test]
        public void ResultT_Errors_Should_Never_Be_Null_For_Failure()
        {
            // Act
            var result = Result<int>.Failure();

            // Assert
            Assert.That(result.Errors, Is.Not.Null);
        }

        [Test]
        public void ResultT_Try_Should_Return_Success_When_No_Exception()
        {
            // Arrange
            static int Operation() => 42;

            // Act
            var result = Result<int>.Try(Operation);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.True);
                Assert.That(result.Value, Is.EqualTo(42));
                Assert.That(result.Errors, Is.Empty);
            }
        }

        [Test]
        public void ResultT_Try_Should_Return_Failure_When_Exception_Thrown()
        {
            // Arrange
            static Exception Operation() => throw new Exception("Something went wrong");

            // Act
            var result = Result<Exception>.Try(Operation);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Value, Is.Null);
                Assert.That(result.Errors, Has.Count.EqualTo(1));
                Assert.That(result.Errors, Does.Contain("Something went wrong"));
            }
        }

        [Test]
        public void Result_Should_Create_Success()
        {
            // Act
            var result = Result.Success();

            // Arrange
            Assert.That(result.IsSuccess, Is.True);
        }

        [Test]
        public void Result_Should_Create_Failure_With_Single_Error()
        {
            // Arrange
            var error = "Error";

            // Act
            var result = Result.Failure(error);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Does.Contain(error));
            }
        }

        [Test]
        public void Result_Should_Create_Failure_With_Multiple_Errors()
        {
            // Arrange
            var errors = new string[] { "Error 1", "Error 2" };

            // Act
            var result = Result.Failure(errors);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(2));
                Assert.That(result.Errors, Does.Contain("Error 1"));
                Assert.That(result.Errors, Does.Contain("Error 2"));
            }
        }

        [Test]
        public void Result_Should_Create_Failure_With_Enumerable_Errors()
        {
            // Arrange
            var errors = new List<string> { "Error 1", "Error 2" };

            // Act
            var result = Result.Failure(errors);

            // Assert
            using (Assert.EnterMultipleScope())
            {
                Assert.That(result.IsSuccess, Is.False);
                Assert.That(result.Errors, Has.Count.EqualTo(2));
                Assert.That(result.Errors, Does.Contain("Error 1"));
                Assert.That(result.Errors, Does.Contain("Error 2"));
            }
        }

        [Test]
        public void Result_Success_Should_Have_Empty_Errors()
        {
            // Act
            var result = Result.Success();

            // Assert
            Assert.That(result.Errors, Is.Empty);
        }

        [Test]
        public void Result_Errors_Should_Never_Be_Null_For_Success()
        {
            // Act
            var result = Result.Success();

            // Assert
            Assert.That(result.Errors, Is.Not.Null);
        }

        [Test]
        public void Result_Errors_Should_Never_Be_Null_For_Failure()
        {
            // Act
            var result = Result.Failure();

            // Assert
            Assert.That(result.Errors, Is.Not.Null);
        }
    }
}
