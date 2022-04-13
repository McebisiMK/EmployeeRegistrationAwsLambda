using System;
using Amazon.Lambda.Core;
using DevLearningAwsLambda.DTOs;
using DevLearningAwsLambda.Models;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace EmployeeRegistrationLambda
{
    public class EmployeeRegistrationFunction
    {
        private const int retirementAge = 65;
        private const decimal BaseSalary = 20000.00M;
        private const decimal PayUnitPercent = 0.02M;

        public User EmployeeRegistrationHandler(UserDTO user, ILambdaContext context)
        {
            ValidUser(user, context);

            context.Logger.Log($"Creating Employee details for: {user.FirstName} {user.LastName}");
            var employee = GetEmployee(user);

            return employee;
        }

        private void ValidUser(UserDTO user, ILambdaContext context)
        {
            var age = user.Age;
            var firstName = user.FirstName;
            var lastName = user.LastName;

            if (InvalidAge(age) || IsEmpty(firstName) || IsEmpty(lastName))
            {
                context.Logger.Log($"Invalid supplied user data: FirstName: {firstName}, LastName: {lastName} and Age: {age}");

                throw new Exception("Please make sure to enter valid user details");
            }
        }

        private bool IsEmpty(string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        private bool InvalidAge(int age)
        {
            return age == 0;
        }

        private User GetEmployee(UserDTO user)
        {
            var age = user.Age;
            var yearsToRetirement = CalculateYearsToRetirement(age);
            var salary = GetSalary(age);
            var employee = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Age = age,
                YearsToRetirement = yearsToRetirement,
                Salary = GetSalary(age)
            };

            return employee;
        }

        private decimal GetSalary(int age)
        {
            var retirementAge = CalculateYearsToRetirement(age);
            var ratedSalary = BaseSalary * retirementAge;
            var actualSalary = ((ratedSalary * PayUnitPercent) + BaseSalary);

            return Math.Round(actualSalary, 2);
        }

        private int CalculateYearsToRetirement(int age)
        {
            return retirementAge - age;
        }
    }
}
