using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Merge)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.BeanValidation.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class ValidationDomainModelDecorator : DomainModelDecorator, IDeclareImports
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.BeanValidation.ValidationDomainModelDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly DomainModelTemplate _template;
        private readonly ICollection<string> _imports = new List<string>();
        private readonly IApplication _application;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public ValidationDomainModelDecorator(DomainModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _template.AddDependency(new JavaDependency("javax.validation", "validation-api"));
        }

        private string Use(string fullyQualifiedType, string import = null)
        {
            import = import ?? fullyQualifiedType;
            if (!_imports.Contains(import))
            {
                _imports.Add(import);
            }

            return fullyQualifiedType.Split('.').Last();
        }

        public override string BeforeField(AttributeModel model)
        {
            var annotations = new List<string>();
            if (model.GetTextConstraints()?.MaxLength() != null)
            {
                annotations.Add($"@{("javax.validation.constraints.Size")}(max = {model.GetTextConstraints().MaxLength()})");
            }

            if (model.Name.ToLower().EndsWith("email"))
            {
                annotations.Add($"@{Use("javax.validation.constraints.Email")}");
            }

            if (!model.TypeReference.IsNullable)
            {
                annotations.Add($"@{Use("javax.validation.constraints.NotNull")}");
            }

            return @"
    " + string.Join(@"
    ", annotations);
        }


        public IEnumerable<string> DeclareImports()
        {
            return _imports;

        }

    }
}