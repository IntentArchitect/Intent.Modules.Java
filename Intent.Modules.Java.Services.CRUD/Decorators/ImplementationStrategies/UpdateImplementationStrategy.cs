using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modelers.Services.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;
using ParameterModel = Intent.Modelers.Services.Api.ParameterModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class UpdateImplementationStrategy : IImplementationStrategy
    {
        public UpdateImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
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

            // Support for composite primary keys not implemented:
            if (domainModel.GetPrimaryKeys().PrimaryKeys.Count > 1)
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

        // public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        //          {
        //              var domainType = _decorator.GetDomainTypeName(domainModel);
        //              var domainTypeCamelCased = domainType.ToCamelCase();
        //              var repositoryFieldName = _decorator.GetRepositoryDependency(domainModel).Name;
        //              var idParam = operationModel.Parameters.First(p => p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
        //              var dtoParam = operationModel.Parameters.First(p => !p.Name.EndsWith("id", StringComparison.OrdinalIgnoreCase));
        //              var assignments = string.Join(@"
        //          ", GetPropertyAssignments(domainModel, dtoParam, domainTypeCamelCased));
        //  
        //              return $@"var {domainTypeCamelCased} = {repositoryFieldName}.findById({idParam.Name.ToCamelCase()}).get();
        //          {assignments}
        //          {repositoryFieldName}.save({domainTypeCamelCased});";
        //          }
        //  
        //          public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        //          {
        //              yield return _decorator.GetRepositoryDependency(targetEntity);
        //          }
        //  
        //          private IEnumerable<string> GetPropertyAssignments(
        //              ClassModel domainModel,
        //              ParameterModel operationParameterModel,
        //              string variableName)
        //          {
        //              var dto = operationParameterModel.TypeReference.Element.AsDTOModel();
        //              foreach (var dtoField in dto.Fields)
        //              {
        //                  var domainAttribute = domainModel.Attributes.FirstOrDefault(p => p.Name.Equals(dtoField.Name, StringComparison.OrdinalIgnoreCase));
        //                  if (domainAttribute == null)
        //                  {
        //                      yield return $"// Warning: No matching field found for {dtoField.Name}";
        //                      continue;
        //                  }
        //  
        //                  if (domainAttribute.Type.Element.Id != dtoField.TypeReference.Element.Id)
        //                  {
        //                      yield return $"// Warning: No matching type for Domain: {domainAttribute.Name} and DTO: {dtoField.Name}";
        //                      continue;
        //                  }
        //  
        //                  yield return $"{variableName}.{domainAttribute.Setter()}({operationParameterModel.Name}.{dtoField.Getter()}());";
        //              }
        //          }
    }
}
