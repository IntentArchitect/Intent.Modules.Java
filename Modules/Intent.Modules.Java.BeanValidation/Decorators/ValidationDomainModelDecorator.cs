using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.RDBMS.Api;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Java.Templates;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.SpringBoot.Settings;
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
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Merge, Body = Mode.Ignore)]
        public ValidationDomainModelDecorator(DomainModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;

            var validationDependency = JavaDependencies.ValidationApi(application);
            if (validationDependency is not null)
            {
                _template.AddDependency(validationDependency);
            }
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
                annotations.Add($"@{($"{JavaxJakarta()}.validation.constraints.Size")}(max = {model.GetTextConstraints().MaxLength()})");
            }

            if (model.Name.ToLower().EndsWith("email"))
            {
                annotations.Add($"@{Use($"{JavaxJakarta()}.validation.constraints.Email")}");
            }

            if (!model.TypeReference.IsNullable)
            {
                annotations.Add($"@{Use($"{JavaxJakarta()}.validation.constraints.NotNull")}");
            }

            return @"
    " + string.Join(@"
    ", annotations);
        }

        public IEnumerable<string> DeclareImports()
        {
            return _imports;
        }
        
        private string JavaxJakarta()
        {
            return _application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => "javax",
                SpringBoot.Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => "jakarta",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}