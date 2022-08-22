namespace Intent.Modules.Java.SpringBoot.Events
{
    public class ApplicationPropertyRequest
    {
        public ApplicationPropertyRequest(string name, string value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; }
        public string Value { get; }
    }
}
