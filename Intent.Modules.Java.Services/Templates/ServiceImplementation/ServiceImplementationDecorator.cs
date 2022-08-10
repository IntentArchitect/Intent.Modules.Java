using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Services.Api;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecoratorContract", Version = "1.0")]

namespace Intent.Modules.Java.Services.Templates.ServiceImplementation
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public abstract class ServiceImplementationDecorator : ITemplateDecorator
    {
        public virtual IEnumerable<ClassDependency> GetClassDependencies() => Enumerable.Empty<ClassDependency>();

        public virtual string GetImplementation(OperationModel operationModel) => string.Empty;

        public int Priority { get; protected set; } = 0;
    }

    public class ClassDependency : IEquatable<ClassDependency>
    {
        public ClassDependency(string type, string name)
        {
            Type = type;
            Name = name;
        }

        public string Type { get; }
        public string Name { get; }

        public bool Equals(ClassDependency other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Type == other.Type && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ClassDependency)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Type, Name);
        }
    }
}