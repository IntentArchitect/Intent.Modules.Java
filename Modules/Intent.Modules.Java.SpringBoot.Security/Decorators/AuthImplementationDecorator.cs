using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Intent.Engine;
using Intent.Modelers.Services.Api;
using Intent.Modules.Java.Services.Templates.ServiceImplementation;
using Intent.Modules.Java.SpringBoot.Security.Templates;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.TemplateDecorator", Version = "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Decorators
{
    [IntentManaged(Mode.Merge)]
    public class AuthImplementationDecorator : ServiceImplementationDecorator
    {
        [IntentManaged(Mode.Fully)]
        public const string DecoratorId = "Intent.Java.SpringBoot.Security.AuthImplementationDecorator";

        [IntentManaged(Mode.Fully)]
        private readonly ServiceImplementationTemplate _template;
        [IntentManaged(Mode.Fully)]
        private readonly IApplication _application;

        [IntentManaged(Mode.Fully, Body = Mode.Fully)]
        public AuthImplementationDecorator(ServiceImplementationTemplate template, IApplication application)
        {
            _template = template;
            _application = application;
        }

        public override string GetImplementation(OperationModel operationModel)
        {
            if (!IsApplicable(operationModel))
            {
                return null;
            }

            return @$"{_template.ImportType("org.springframework.security.core.userdetails.UserDetails")} userDetails = userDetailsService.loadUserByUsername(request.getUsername());
        {_template.ImportType("org.springframework.security.crypto.password.PasswordEncoder")} passwordEncoder = {_template.ImportType("org.springframework.security.crypto.factory.PasswordEncoderFactories")}.createDelegatingPasswordEncoder();
        if (!passwordEncoder.matches(request.getPassword(), userDetails.getPassword())) {{
            throw new AuthenticationFailedException();
        }}

        String token = jwtUtils.generateJwtToken(userDetails);
        return new AuthenticationResult(token);";
        }

        public override IEnumerable<ClassDependency> GetClassDependencies()
        {
            if (!_template.Model.Operations.Any(IsApplicable))
            {
                yield break;
            }

            yield return new ClassDependency(_template.ImportType("org.springframework.security.core.userdetails.UserDetailsService"), "userDetailsService");
            yield return new ClassDependency(_template.GetJwtUtilsName(), "jwtUtils");

            _template.AddDependency(JavaDependencies.SpringBootSecurity);

            foreach (var classDependency in base.GetClassDependencies())
            {
                yield return classDependency;
            }
        }

        private static bool IsApplicable(OperationModel operationModel)
        {
            return operationModel.ParentService.Name == "AuthService" &&
                   operationModel.Name == "authenticate" &&
                   operationModel.Parameters.Count == 1 &&
                   operationModel.Parameters[0].TypeReference.Element.Name == "AuthenticationRequest";
        }
    }
}