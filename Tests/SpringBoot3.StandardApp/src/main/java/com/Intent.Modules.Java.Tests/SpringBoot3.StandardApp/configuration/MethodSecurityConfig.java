package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.configuration;

import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.method.configuration.EnableMethodSecurity;

@Configuration
@EnableMethodSecurity(securedEnabled = true, prePostEnabled = true)
public class MethodSecurityConfig {
}
