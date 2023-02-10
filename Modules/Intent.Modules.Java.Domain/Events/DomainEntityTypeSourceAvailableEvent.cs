using Intent.Modules.Common.TypeResolution;

namespace Intent.Modules.Java.Domain.Events
{
    public class DomainEntityTypeSourceAvailableEvent
    {
        public DomainEntityTypeSourceAvailableEvent(ITypeSource typeSource)
        {
            TypeSource = typeSource;
        }

        public ITypeSource TypeSource { get; set; }
    }
}
