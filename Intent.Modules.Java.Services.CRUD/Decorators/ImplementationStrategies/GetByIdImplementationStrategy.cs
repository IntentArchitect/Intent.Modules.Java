using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Services.Templates;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class GetByIdImplementationStrategy : IImplementationStrategy
    {
        private readonly CrudServiceImplementationDecorator _decorator;

        public GetByIdImplementationStrategy(CrudServiceImplementationDecorator decorator)
        {
            _decorator = decorator;
        }

        public bool Match(ClassModel domainModel, OperationModel operationModel)
        {
            var entityName = domainModel.Name.ToLower();
            var lowerOperationName = operationModel.Name.ToLower();

            if (operationModel.Parameters.Count() != 1)
            {
                return false;
            }

            if (!operationModel.Parameters.Any(p => string.Equals(p.Name, "id", StringComparison.InvariantCultureIgnoreCase) ||
                                                    string.Equals(p.Name, $"{entityName}Id", StringComparison.InvariantCultureIgnoreCase)))
            {
                return false;
            }

            if (operationModel?.TypeReference?.IsCollection ?? false)
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
                "get",
                $"get{entityName}",
                "find",
                "findbyid",
                $"find{entityName}",
                $"find{entityName}byid",
                entityName
            }
            .Contains(lowerOperationName);
        }

        public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        {
            var domainType = _decorator.GetDomainTypeName(domainModel);
            var domainTypeCamelCased = domainType.ToCamelCase();
            var domainTypePascalCased = domainType.ToPascalCase();
            var repositoryFieldName = _decorator.GetRepositoryDependency(domainModel).Name;
            var dtoType = _decorator.GetDtoTypeName(operationModel.TypeReference.Element);

            return $@"var {domainTypeCamelCased} = {repositoryFieldName}.findById({operationModel.Parameters.First().Name.ToCamelCase()});
        if (!{domainTypeCamelCased}.isPresent()) {{
            return null;
        }}
        return {dtoType}.mapFrom{domainTypePascalCased}({domainTypeCamelCased}.get(), mapper);";
        }

        public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        {
            yield return _decorator.GetRepositoryDependency(targetEntity);
            yield return new ClassDependency("org.modelmapper.ModelMapper", "mapper");
        }
    }
}
