namespace RunGroopWebApp.Exceptions
{
    /*it would be like that:
     var errors = new Dictionary<string, string[]>
{
    { "username", new[] { "Username is required", "Username must be at least 3 characters long" } },
    { "password", new[] { "Password must contain at least one number", "Password must be at least 8 characters long" } }
};
    */
    public class ValidationAppException(IReadOnlyDictionary<string, string[]> errors) : Exception("One or more validation errors occurred")
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; } = errors;
    }
}
