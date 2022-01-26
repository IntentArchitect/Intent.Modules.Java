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
    public class GetAllImplementationStrategy : IImplementationStrategy
    {
        private readonly CrudServiceImplementationDecorator _decorator;

        public GetAllImplementationStrategy(CrudServiceImplementationDecorator decorator)
        {
            _decorator = decorator;
        }

        public bool Match(ClassModel domainModel, OperationModel operationModel)
        {
            if (operationModel.Parameters.Any())
            {
                return false;
            }

            if (!(operationModel?.TypeReference?.IsCollection ?? false))
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

        public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        {
            var dto = operationModel.TypeReference.Element.AsDTOModel();
            return $@"var {domainModel.Name.ToCamelCase().ToPluralName()} = {domainModel.Name.ToCamelCase()}Repository.findAll();
        return {_decorator.Template.GetDataTransferModelName(dto)}.mapFrom{domainModel.Name.ToPluralName()}({domainModel.Name.ToCamelCase().ToPluralName()}, mapper);";
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
