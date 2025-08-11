using Microsoft.IdentityModel.Tokens;

namespace MasterApp.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new Dictionary<string, string[]>();
    }

    //public ValidationException(IEnumerable<ValidationFailure> failures)
    //    : this()
    //{
    //    Errors = failures
    //        .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
    //        .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    //}
    public class LoginFailedException : Exception
    {
        public LoginFailedException()
        : base("Login failed! Invalid username or password.")
        {

        }
    }
    public class DataValidationException : Exception
    {
        public DataValidationException()
            : base("One or more validation failures have occurred.")
        {
            Failures = new Dictionary<string, string[]>();
        }



        public IDictionary<string, string[]> Failures { get; }
    }
    public class BadRequestException : Exception
    {
        public BadRequestException(string message)
            : base(message)
        {
        }
    }
    public IDictionary<string, string[]> Errors { get; }


}
