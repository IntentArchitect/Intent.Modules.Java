using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly:IntentTemplate("Intent.ModuleBuilder.Java.Templates.JavaFileStringInterpolationTemplate",Version= "1.0")]
namespace Intent.Modules.Java.SpringBoot.Security.Templates.WebSecurityConfigV2
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class WebSecurityConfigV2Template
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            return $@"
package {Package};

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.provisioning.InMemoryUserDetailsManager;
import org.springframework.security.web.authentication.UsernamePasswordAuthenticationFilter;

@Configuration
@EnableWebSecurity
public class {ClassName} extends WebSecurityConfigurerAdapter {{
    @Override
    protected void configure(HttpSecurity http) throws Exception {{
        http
                .cors().and().csrf().disable()
                .authorizeRequests()
                .antMatchers(""/api/auth/**"", ""/swagger-ui/**"", ""/v3/api-docs/**"", ""/swagger-resources/**"").permitAll()
                .anyRequest().authenticated();

        http.addFilterBefore(authenticationJwtTokenFilter(), UsernamePasswordAuthenticationFilter.class);
    }}

    @Bean
    public {this.GetAuthTokenFilterName()} authenticationJwtTokenFilter() {{
        return new {this.GetAuthTokenFilterName()}();
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