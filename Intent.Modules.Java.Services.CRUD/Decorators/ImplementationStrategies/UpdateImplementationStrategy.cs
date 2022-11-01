using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;
using ParameterModel = Intent.Modelers.Services.Api.ParameterModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class UpdateImplementationStrategy : IImplementationStrategy
    {
        private readonly CrudServiceImplementationDecorator _decorator;

        public UpdateImplementationStrategy(CrudServiceImplementationDecorator decorator)
        {
            _decorator = decorator;
        }

        public bool Match(ClassModel domainModel, OperationModel operationModel)
        {
            var lowerDomainName = domainModel.Name.ToLower();
            var lowerOperationName = operationModel.Name.ToLower();
            if (operationModel.Parameters.Count() != 2)
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

            return new[]
            {
                "put",
                $"put{lowerDomainName}",
                "update",
                $"update{lowerDomainName}",
            }
            .Contains(lowerOperationName);
        }

        public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        {
            var idParam = operationModel.Parameters.First(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
            var dtoParam = operationModel.Parameters.First(p => !p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));

            return $@"var {domainModel.Name.ToCamelCase()} = {domainModel.Name.ToCamelCase()}Repository.findById({idParam.Name}).get();
        {EmitPropertyAssignments(domainModel, domainModel.Name.ToCamelCase(), dtoParam)}
        { domainModel.Name.ToCamelCase() }Repository.save({domainModel.Name.ToCamelCase()});";
        }

        public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        {
            var repo = _decorator.Template.GetTypeName(EntityRepositoryTemplate.TemplateId, targetEntity);
            return new[]
            {
                new ClassDependency(repo, repo.ToCamelCase()),
            };
        }

        private string EmitPropertyAssignments(ClassModel domainModel, string domainVarName, ParameterModel operationParameterModel)
        {
            var sb = new StringBuilder();
            var dto = operationParameterModel.TypeReference.Element.AsDTOModel();
            foreach (var dtoField in dto.Fields)
            {
                var domainAttribute = domainModel.Attributes.FirstOrDefault(p => p.Name.Equals(dtoField.Name, StringComparison.OrdinalIgnoreCase));
                if (domainAttribute == null)
                {
                    sb.AppendLine($"        // Warning: No matching field found for {dtoField.Name}");
                    continue;
                }
                if (domainAttribute.Type.Element.Id != dtoField.TypeReference.Element.Id)
                {
                    sb.AppendLine($"        // Warning: No matching type for Domain: {domainAttribute.Name} and DTO: {dtoField.Name}");
                    continue;
                }
                sb.AppendLine($"        {domainVarName}.{domainAttribute.Setter()}({operationParameterModel.Name}.{dtoField.Getter()}());");
            }
            return sb.ToString().Trim();
        }
    }
}
