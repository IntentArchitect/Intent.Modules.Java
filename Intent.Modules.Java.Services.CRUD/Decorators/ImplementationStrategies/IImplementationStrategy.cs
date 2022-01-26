using System.Collections.Generic;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public interface IImplementationStrategy
    {
        bool Match(ClassModel domainModel, OperationModel operationModel);
        string GetImplementation(ClassModel domainModel, OperationModel operationModel);
        IEnumerable<ClassDependency> GetRequiredServices(ClassModel targetEntity);
    }
}
