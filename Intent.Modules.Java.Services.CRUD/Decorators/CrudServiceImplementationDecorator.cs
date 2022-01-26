using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.RoslynWeaver.Attributes;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

[assembly: DefaultIntentManaged(Mode.Ignore)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.Services.CRUD.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class CrudServiceImplementationDecorator : ServiceImplementationDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.Services.CRUD.CrudServiceImplementationDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly ServiceImplementationTemplate _template;
        private readonly IApplication _application;
        private readonly List<IImplementationStrategy> _strategies;
        private readonly ClassModel _targetEntity;
        private readonly IMetadataManager _metadataManager;

        [IntentManaged(Mode.Merge, Body = Mode.Fully)]
        public CrudServiceImplementationDecorator(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
            _metadataManager = application.MetadataManager;
            _strategies = new List<IImplementationStrategy>
            {
                new GetAllImplementationStrategy(this),
                new GetByIdImplementationStrategy(this),
                new CreateImplementationStrategy(this),
                new UpdateImplementationStrategy(this),
                new DeleteImplementationStrategy(this)
            };
            _targetEntity = GetDomainForService(Template.Model);
        }

        public ServiceImplementationTemplate Template => _template;

        public override IEnumerable<ClassDependency> GetClassDependencies()
        {
            var services = new List<ClassDependency>();
            if (_targetEntity == null)
            {
                return services;
            }

            foreach (var operationModel in Template.Model.Operations)
            {
                foreach (var strategy in _strategies)
                {
                    if (strategy.Match(_targetEntity, operationModel))
                    {
                        services.AddRange(strategy.GetRequiredServices(_targetEntity));
                    }
                }
            }
            return services.Distinct();
        }

        public override string GetImplementation(OperationModel operationModel)
        {
            if (_targetEntity == null)
            {
                return string.Empty;
            }

            foreach (var strategy in _strategies)
            {
                if (strategy.Match(_targetEntity, operationModel))
                {
                    return strategy.GetImplementation(_targetEntity, operationModel);
                }
            }

            return string.Empty;
        }

        private ClassModel GetDomainForService(ServiceModel service)
        {
            var serviceIdentifier = service.Name.RemoveSuffix("RestController", "Controller", "Service", "Manager").ToLower();
            var entities = _metadataManager.Domain(_application).GetClassModels();
            return entities.SingleOrDefault(e => e.Name.Equals(serviceIdentifier, StringComparison.InvariantCultureIgnoreCase) ||
                                                 e.Name.Pluralize().Equals(serviceIdentifier, StringComparison.InvariantCultureIgnoreCase));
        }
    }
}