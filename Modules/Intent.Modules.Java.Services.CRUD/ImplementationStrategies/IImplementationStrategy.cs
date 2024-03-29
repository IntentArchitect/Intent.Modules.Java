﻿using System.Collections.Generic;
using Intent.Modelers.Domain.Api;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using OperationModel = Intent.Modelers.Services.Api.OperationModel;

namespace Intent.Modules.Java.Services.CRUD.Decorators.ImplementationStrategies
{
    public interface IImplementationStrategy
    {
        bool IsMatch(OperationModel operationModel);
        void ApplyStrategy(OperationModel operationModel);
    }
}
