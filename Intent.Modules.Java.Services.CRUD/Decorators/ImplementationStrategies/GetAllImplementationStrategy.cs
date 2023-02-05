using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Java.Builder;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Services.Templates.DataTransferModel;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class GetAllImplementationStrategy : IImplementationStrategy
    {
        private readonly ServiceImplementationTemplate _template;
        private readonly IApplication _application;

        public GetAllImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }
        
        public bool IsMatch(OperationModel operationModel)
        {
            if (operationModel.Parameters.Any())
            {
                return false;
            }

            if (operationModel.TypeReference?.IsCollection != true)
            {
                return false;
            }

            var dtoModel = operationModel.TypeReference.Element?.AsDTOModel();
            if (dtoModel == null)
            {
                return false;
            }

            var domainModel = dtoModel.Mapping?.Element?.AsClassModel();
            if (domainModel == null)
            {
                return false;
            }
            
            var lowerOperationName = operationModel.Name.ToLower();
            return new[] { "get", "find" }.Any(x => lowerOperationName.Contains(x));
        }

        public void ApplyStrategy(OperationModel operationModel)
        {
            var dtoModel = operationModel.TypeReference.Element.AsDTOModel();
            var dtoType = _template.TryGetTypeName(DataTransferModelTemplate.TemplateId, dtoModel, out var dtoName)
                ? dtoName
                : dtoModel.Name.ToPascalCase();
            var domainModel = dtoModel.Mapping.Element.AsClassModel();
            var domainType = _template.TryGetTypeName(DomainModelTemplate.TemplateId, domainModel, out var result)
                ? result
                : domainModel.Name;
            var domainTypeCamelCased = domainType.ToCamelCase();
            var domainTypePascalCased = domainType.ToPascalCase();
            var repositoryTypeName = _template.GetTypeName(EntityRepositoryTemplate.TemplateId, domainModel);
            var repositoryFieldName = repositoryTypeName.ToCamelCase();
            
            var codeLines = new JavaStatementAggregator();
            codeLines.Add($@"var {domainTypeCamelCased.Pluralize()} = {repositoryFieldName}.findAll();");
            codeLines.Add($@"return {dtoType}.mapFrom{domainTypePascalCased.Pluralize()}({domainTypeCamelCased.Pluralize()}, mapper);");
            
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
    }
}
