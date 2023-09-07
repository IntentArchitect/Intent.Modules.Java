using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]

namespace Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV3
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class WebSecurityConfigV3Template    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configurers.AbstractHttpConfigurer;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@Configuration
@EnableWebSecurity
public class {ClassName} {{
    @Bean
    public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {{
        http.cors(AbstractHttpConfigurer::disable)
            .csrf(AbstractHttpConfigurer::disable)
            .authorizeHttpRequests(registry -> registry
                .requestMatchers(""/api/auth/**"", ""/swagger-ui/**"", ""/v3/api-docs/**"", ""/swagger-resources/**"").permitAll()
                .anyRequest().authenticated());

        return http.build();
    }}

    @Bean
    {this.IntentIgnoreBodyAnnotation()}
    public UserDetailsService getUserDetailsService() {{
        // Change the body of this method to use your own implementation of UserDetailsService

        UserDetails user = User.withDefaultPasswordEncoder()
                .username(""user"")
                .password(""password"")
                .roles(""USER"")
                .build();

        return new InMemoryUserDetailsManager(user);
    }}
}}
";
        }
    }
}