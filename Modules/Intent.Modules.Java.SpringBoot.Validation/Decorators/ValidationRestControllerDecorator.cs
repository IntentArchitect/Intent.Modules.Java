using System;
using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.SpringBoot.Settings;
using Intent.Modules.Java.SpringBoot.Templates.RestController;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Validation.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class ValidationRestControllerDecorator : RestControllerDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringBoot.Validation.ValidationRestControllerDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly RestControllerTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public ValidationRestControllerDecorator(RestControllerTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _template.AddDependency(JavaDependencies.SpringBootValidation);
        }

        public override IEnumerable<string> ParameterAnnotations(ParameterModel parameter)
        {
            if (parameter.TypeReference?.Element.IsDTOModel() == true)
            {
                yield return $"@{_template.ImportType($"{JavaxJakarta()}.validation.Valid")}";
            }

            foreach (var annotation in base.ParameterAnnotations(parameter))
            {
                yield return annotation;
            }
        }
        
        private string JavaxJakarta()
        {
            return _application.Settings.GetSpringBoot().TargetVersion().AsEnum() switch
            {
                Settings.SpringBoot.TargetVersionOptionsEnum.V2_7_5 => "javax",
                Settings.SpringBoot.TargetVersionOptionsEnum.V3_1_3 => "jakarta",
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}