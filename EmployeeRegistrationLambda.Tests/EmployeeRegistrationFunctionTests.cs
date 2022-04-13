using System;
using Amazon.Lambda.TestUtilities;
using DevLearningAwsLambda.DTOs;
using DevLearningAwsLambda.Models;
using FluentAssertions;
using NUnit.Framework;

namespace EmployeeRegistrationLambda.Tests
{
    [TestFixture]
    public class EmployeeRegistrationFunctionTests
    {
        [TestCase("", "LastName", 20)]
        [TestCase("FirstName", "", 20)]
        [TestCase("FirstName", "LastName", 0)]
        public void EmployeeRegistrationHandler_Given_Invalid_User_Details_Throw_Exception(string firstName, string lastName, int age)
        {
            //-----------------------------Arrange------------------------------------
            var user = new UserDTO
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };
            var context = new TestLambdaContext();
            var devLearningFunction = new EmployeeRegistrationFunction();

            //-----------------------------Act----------------------------------------
            var exception = Assert.Throws<Exception>(() => devLearningFunction.EmployeeRegistrationHandler(user, context));

            //-----------------------------Assert-------------------------------------
            exception?.Message.Should().Be("Please make sure to enter valid user details");
        }

        [Test]
        public void EmployeeRegistrationHandler_Given_Valid_User_Should_Create_Employee_Details()
        {
            //-----------------------------Arrange------------------------------------
            var user = GetUser();
            var expectedEmployee = GetExpectedEmployee();
            var context = new TestLambdaContext();
            var devLearningFunction = new EmployeeRegistrationFunction();

            //-----------------------------Act----------------------------------------
            var actualEmployee = devLearningFunction.EmployeeRegistrationHandler(user, context);

            //-----------------------------Assert-------------------------------------
            actualEmployee.Should().BeEquivalentTo(expectedEmployee);
        }

        private static UserDTO GetUser()
        {
            return new UserDTO
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Age = 31
            };
        }

        private static User GetExpectedEmployee()
        {
            return new User
            {
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Age = 31,
                YearsToRetirement = 34,
                Salary = 33600.00M
            };
        }
    }
}