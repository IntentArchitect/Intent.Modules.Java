using System.Collections.Generic;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringDoc.OpenApi.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class OpenApiDtoDecorator : DataTransferModelDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringDoc.OpenApi.OpenApiDtoDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly DataTransferModelTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public OpenApiDtoDecorator(DataTransferModelTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override IEnumerable<string> GetClassAnnotations()
        {
            if (string.IsNullOrWhiteSpace(_template.Model.Comment))
            {
                yield break;
            }

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.media.Schema")}(description = \"{_template.Model.Comment.EscapeJavaString()}\")";
        }

        public override IEnumerable<string> GetFieldAnnotations(DTOFieldModel field)
        {
            if (string.IsNullOrWhiteSpace(field.Comment))
            {
                yield break;
            }

            yield return $"@{_template.ImportType("io.swagger.v3.oas.annotations.media.Schema")}(description = \"{field.Comment.EscapeJavaString()}\")";
        }
    }
}