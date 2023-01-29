using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class GetAllImplementationStrategy : IImplementationStrategy
    {
        public GetAllImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
        {
            
        }
        
        public bool IsMatch(OperationModel operationModel)
        {
            return false;
        }

        public void ApplyStrategy(OperationModel operationModel)
        {
            throw new NotImplementedException();
        }

        public bool Match(ClassModel domainModel, OperationModel operationModel)
        {
            if (operationModel.Parameters.Any())
            {
                return false;
            }

            if (operationModel.TypeReference?.IsCollection != true)
            {
                return false;
            }

            var lowerDomainName = domainModel.Name.ToLower();
            var pluralLowerDomainName = lowerDomainName.Pluralize();
            var lowerOperationName = operationModel.Name.ToLower();
            return new[]
            {
                $"get",
                $"get{lowerDomainName}",
                $"get{pluralLowerDomainName}",
                $"get{pluralLowerDomainName}list",
                $"getall",
                $"getall{pluralLowerDomainName}",
                $"find",
                $"find{lowerDomainName}",
                $"find{pluralLowerDomainName}",
                "findall"
            }
            .Contains(lowerOperationName);
        }

        // public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        // {
        //     var domainType = _decorator.GetDomainTypeName(domainModel);
        //     var domainTypeCamelCased = domainType.ToCamelCase();
        //     var domainTypePascalCased = domainType.ToPascalCase();
        //     var repositoryFieldName = _decorator.GetRepositoryDependency(domainModel).Name;
        //     var dtoType = _decorator.GetDtoTypeName(operationModel.TypeReference.Element);
        //
        //     return $@"var {domainTypeCamelCased.Pluralize()} = {repositoryFieldName}.findAll();
        // return {dtoType}.mapFrom{domainTypePascalCased.Pluralize()}({domainTypeCamelCased.Pluralize()}, mapper);";
        // }
        //
        // public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        // {
        //     yield return _decorator.GetRepositoryDependency(targetEntity);
        //     yield return new ClassDependency("org.modelmapper.ModelMapper", "mapper");
        // }
    }
}
