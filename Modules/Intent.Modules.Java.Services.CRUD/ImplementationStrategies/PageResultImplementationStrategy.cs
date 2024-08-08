using System;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies;

public class PageResultImplementationStrategy : IImplementationStrategy
{
    private readonly ServiceImplementationTemplate _template;
    private readonly IApplication _application;

    public PageResultImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
    {
        _template = template;
        _application = application;
    }

    public bool IsMatch(OperationModel operationModel)
    {
        if (operationModel.Parameters.All(param => param.TypeReference?.Element?.Name != "Pageable"))
        {
            return false;
        }

        if (operationModel.ReturnType is null)
        {
            return false;
        }
        
        if (operationModel.ReturnType.IsCollection)
        {
            return false;
        }

        if (operationModel.ReturnType.Element?.Name != "Page")
        {
            return false;
        }

        var dtoModel = operationModel.ReturnType.GenericTypeParameters.FirstOrDefault()?.Element as IElement;
        if (dtoModel is null)
        {
            return false;
        }

        var domainModel = dtoModel.MappedElement?.Element?.AsClassModel();
        if (domainModel is null)
        {
            return false;
        }

        return true;
    }

    public void ApplyStrategy(OperationModel operationModel)
    {
        var @class = _template.JavaFile.Classes.First();
        
        var method = @class.FindMethod(m => m.Name.Equals(operationModel.Name, StringComparison.OrdinalIgnoreCase));
        method.Annotations.Where(p => p.Name.Contains("IntentIgnoreBody")).ToList().ForEach(x => method.Annotations.Remove(x));
        method.Statements.Clear();
        
        var dtoModel = (IElement)operationModel.ReturnType.GenericTypeParameters.First().Element;
        var dtoType = _template.TryGetTypeName(DataTransferModelTemplate.TemplateId, dtoModel, out var dtoName)
            ? dtoName
            : dtoModel.Name.ToPascalCase();
        var domainModel = dtoModel.MappedElement.Element.AsClassModel();
        var domainType = _template.TryGetTypeName(DomainModelTemplate.TemplateId, domainModel, out var result)
            ? result
            : domainModel.Name;
        var domainTypeCamelCased = domainType.ToCamelCase();
        var domainTypePascalCased = domainType.ToPascalCase();
        var repositoryTypeName = _template.GetTypeName(EntityRepositoryTemplate.TemplateId, domainModel);
        var repositoryFieldName = repositoryTypeName.ToCamelCase();

        if (@class.Fields.All(p => p.Type != repositoryTypeName))
        {
            @class.AddField(_template.ImportType(repositoryTypeName), repositoryFieldName);
        }
        
        var pageableVar = operationModel.Parameters.First(param => param.TypeReference.Element.Name == "Pageable");
        
        var codeLines = new JavaStatementAggregator();
        codeLines.Add($@"var {domainTypeCamelCased.Pluralize()} = {repositoryFieldName}.findAll({pageableVar.Name});");
        
        if (@class.Fields.All(p => p.Type != "ModelMapper"))
        {
            @class.AddField(_template.ImportType("org.modelmapper.ModelMapper"), "mapper");
        }

        var pageImplType = _template.ImportType("org.springframework.data.domain.PageImpl");
        var pageRequestType = _template.ImportType("org.springframework.data.domain.PageRequest");
        
        codeLines.Add($"""
                       return new {pageImplType}<>({dtoType}.mapFrom{domainTypePascalCased.Pluralize()}({domainTypeCamelCased.Pluralize()}.getContent(), mapper), 
                                   {pageRequestType}.of({pageableVar.Name}.getPageNumber(), {pageableVar.Name}.getPageSize()), 
                                   {domainTypeCamelCased.Pluralize()}.getTotalElements());
                       """);
        
        method.AddStatements(codeLines.ToList());
    }
}