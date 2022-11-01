﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Intent.Metadata.Models;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Domain.Templates.DomainModel;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.Spring.Data.Repositories.Templates.EntityRepository;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;
using ParameterModel = Intent.Modelers.Services.Api.ParameterModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class CreateImplementationStrategy : IImplementationStrategy
    {
        private readonly CrudServiceImplementationDecorator _decorator;

        public CreateImplementationStrategy(CrudServiceImplementationDecorator decorator)
        {
            _decorator = decorator;
        }

        public bool Match(ClassModel domainModel, OperationModel operationModel)
        {
            if (operationModel.Parameters.Count() != 1)
            {
                return false;
            }

            // We seriously need a better way to check for a surrogate key here. This is not a good approach.
            if (operationModel.TypeReference.Element != null
                && !_decorator.Template.GetTypeInfo(operationModel.TypeReference).IsPrimitive
                && operationModel.TypeReference.Element.Name != "guid")
            {
                return false;
            }

            var lowerDomainName = domainModel.Name.ToLower();
            var lowerOperationName = operationModel.Name.ToLower();
            return new[]
                {
                    "post",
                    $"post{lowerDomainName}",
                    "create",
                    $"create{lowerDomainName}",
                    $"add{lowerDomainName}",
                }
                .Contains(lowerOperationName);
        }

        public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        {
            var entityName = _decorator.Template.GetTypeName(DomainModelTemplate.TemplateId, domainModel, new TemplateDiscoveryOptions() { ThrowIfNotFound = false });
            var statements = new List<string>();
            statements.Add($"var {domainModel.Name.ToCamelCase()} = new {entityName ?? domainModel.Name}();");

            statements.AddRange(GetPropertyAssignments(domainModel, operationModel.Parameters.Single()));

            statements.Add($"{domainModel.Name.ToCamelCase()}Repository.save({domainModel.Name.ToCamelCase()});");

            if (operationModel.TypeReference.Element != null)
            {
                statements.Add($"return {domainModel.Name.ToCamelCase()}.getId();");
            }

            return string.Join(@"
        ", statements);
        }

        public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        {
            var repo = _decorator.Template.GetTypeName(EntityRepositoryTemplate.TemplateId, targetEntity);
            return new[]
            {
                new ClassDependency(repo, repo.ToCamelCase()),
            };
        }

        private List<string> GetPropertyAssignments(ClassModel domainModel, ParameterModel operationParameterModel)
        {
            var statements = new List<string>();
            var dto = operationParameterModel.TypeReference.Element.AsDTOModel();
            foreach (var dtoField in dto.Fields)
            {
                var domainAttribute = domainModel.Attributes.FirstOrDefault(p => p.Name.Equals(dtoField.Name, StringComparison.OrdinalIgnoreCase));
                if (domainAttribute == null)
                {
                    statements.Add($"// Warning: No matching field found for {dtoField.Name}");
                    continue;
                }

                if (domainAttribute.Type.Element.Id != dtoField.TypeReference.Element.Id)
                {
                    statements.Add($"// Warning: No matching type for Domain: {domainAttribute.Name} and DTO: {dtoField.Name}");
                    continue;
                }

                statements.Add($"{domainModel.Name.ToCamelCase()}.{domainAttribute.Setter()}({operationParameterModel.Name}.{dtoField.Getter()}());");
            }

            return statements;
        }
    }
}