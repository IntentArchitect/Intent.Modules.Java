using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Common.Templates;
using Intent.Modules.Java.Persistence.JPA;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public class DeleteImplementationStrategy : IImplementationStrategy
    {
        public DeleteImplementationStrategy(ServiceImplementationTemplate template, IApplication application)
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
            if (operationModel.Parameters.Count() != 1)
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

        // public string GetImplementation(ClassModel domainModel, OperationModel operationModel)
        // {
        //     var domainType = _decorator.GetDomainTypeName(domainModel);
        //     var domainTypeCamelCased = domainType.ToCamelCase();
        //     var repositoryFieldName = _decorator.GetRepositoryDependency(domainModel).Name;
        //
        //     return $@"var {domainTypeCamelCased} = {repositoryFieldName}.findById({operationModel.Parameters.Single().Name.ToCamelCase()}).get();
        // {repositoryFieldName}.delete({domainTypeCamelCased});";
        // }
        //
        // public IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity)
        // {
        //     yield return _decorator.GetRepositoryDependency(targetEntity);
        // }
    }
}
