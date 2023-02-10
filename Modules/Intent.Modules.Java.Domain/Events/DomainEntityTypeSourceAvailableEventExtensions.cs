using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;

namespace Intent.Modules.Java.Domain.Events;

public static class DomainEntityTypeSourceAvailableEventExtensions
{
    public static void AddDomainEntityTypeSource(this IntentTemplateBase template)
    {
        template.AddTypeSource(DomainModelTemplate.TemplateId);
        template.OutputTarget.ExecutionContext.EventDispatcher
            .Subscribe<DomainEntityTypeSourceAvailableEvent>(@event => template.AddTypeSource(@event.TypeSource));
    }
}