using System;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;

public class LegacyDeleteImplementationStrategy : IImplementationStrategy
{
    private readonly ServiceImplementationTemplate _template;
    private readonly IApplication _application;

    public LegacyDeleteImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
    {
        _template = template;
        _application = application;
    }
    
    public bool IsMatch(OperationModel operationModel)
    {
        var domainModel = GetDomainForService(operationModel.ParentService);
        if (domainModel == null)
        {
            return false;
        }
        
        var lowerDomainName = domainModel.Name.ToLower();
        var lowerOperationName = operationModel.Name.ToLower();
        if (operationModel.Parameters.Count != 1)
        {
            return false;
        }
        
        if (!operationModel.Parameters.Any(p => string.Equals(p.Name, "id", StringComparison.InvariantCultureIgnoreCase) ||
                                                string.Equals(p.Name, $"{lowerDomainName}Id", StringComparison.InvariantCultureIgnoreCase)))
        {
            return false;
        }
        
        if (operationModel.TypeReference.Element != null)
        {
            return false;
        }
        
        // Support for composite primary keys not implemented:
        if (domainModel.GetPrimaryKeys().PrimaryKeys.Count > 1)
        {
            return false;
        }
        
        return new[]
            {
                "delete",
                $"delete{lowerDomainName}"
            }
            .Contains(lowerOperationName);
    }

    public void ApplyStrategy(OperationModel operationModel)
    {
        var domainModel = GetDomainForService(operationModel.ParentService);
        var domainType = _template.TryGetTypeName(DomainModelTemplate.TemplateId, domainModel, out var result)
            ? result
            : domainModel.Name;
        var domainTypeCamelCased = domainType.ToCamelCase();
        var domainTypePascalCased = domainType.ToPascalCase();
        var repositoryTypeName = _template.GetTypeName(EntityRepositoryTemplate.TemplateId, domainModel);
        var repositoryFieldName = repositoryTypeName.ToCamelCase();

        var codeLines = new JavaStatementAggregator();
        codeLines.Add($@"var {domainTypeCamelCased} = {repositoryFieldName}.findById({operationModel.Parameters.First().Name.ToCamelCase()});");
        codeLines.Add(new JavaStatementBlock($@"if (!{domainTypeCamelCased}.isPresent())")
            .AddStatement($@"return;"));
        codeLines.Add($@"{repositoryFieldName}.delete({domainTypeCamelCased}.get());");

        var @class = _template.JavaFile.Classes.First();
        if (@class.Fields.All(p => p.Type != repositoryTypeName))
        {
            @class.AddField(_template.ImportType(repositoryTypeName), repositoryFieldName);
        }

        if (@class.Fields.All(p => p.Type != "ModelMapper"))
        {
            @class.AddField(_template.ImportType("org.modelmapper.ModelMapper"), "mapper");
        }

        var method = @class.FindMethod(m => m.Name.Equals(operationModel.Name, StringComparison.OrdinalIgnoreCase));
        method.Annotations.Where(p => p.Name.Contains("IntentIgnoreBody")).ToList().ForEach(x => method.Annotations.Remove(x));
        method.Statements.Clear();
        method.AddStatements(codeLines.ToList());
    }
    
    private ClassModel GetDomainForService(ServiceModel service)
    {
        var serviceIdentifier = service.Name.RemoveSuffix("RestController", "Controller", "Service", "Manager").ToLower();
        var entities = _application.MetadataManager.Domain(_application).GetClassModels();
        return entities.FirstOrDefault(e => e.Name.Equals(serviceIdentifier, StringComparison.InvariantCultureIgnoreCase) ||
                                             e.Name.Pluralize().Equals(serviceIdentifier, StringComparison.InvariantCultureIgnoreCase));
    }
}