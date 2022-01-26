using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
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
            var dto = operationModel.TypeReference.Element.AsDTOModel();

            return $@"var {domainModel.Name.ToCamelCase()} = {domainModel.Name.ToCamelCase()}Repository.findById({operationModel.Parameters.First().Name.ToCamelCase()});
        if (!{domainModel.Name.ToCamelCase()}.isPresent()) {{
            return null;
        }}
        return {_decorator.Template.GetDataTransferModelName(dto)}.mapFrom{domainModel.Name.ToPascalCase()}({domainModel.Name.ToCamelCase()}.get(), mapper);";
        }

        public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        {
            var repo = _decorator.Template.GetTypeName(EntityRepositoryTemplate.TemplateId, targetEntity);
            return new[]
            {
                new ClassDependency(repo, repo.ToCamelCase()),
                new ClassDependency("org.modelmapper.ModelMapper", "mapper"), 
            };
        }
    }
}
