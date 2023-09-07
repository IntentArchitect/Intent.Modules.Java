using Intent.Modules.Common.Templates;

namespace Intent.Modules.Java.SpringBoot.Events
{
    public class ApplicationPropertyRequest
    {
        public ApplicationPropertyRequest(string name, string value, string? comment = null)
        {
            Name = name;
            Value = value;
            Comment = comment;
        }

        public string Name { get; }
        public string Value { get; }
        public string? Comment { get; }
    }
}