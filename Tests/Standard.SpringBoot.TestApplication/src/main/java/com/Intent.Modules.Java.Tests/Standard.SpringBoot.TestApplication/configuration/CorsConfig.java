package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.configuration;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Bean;
import org.springframework.web.servlet.config.annotation.CorsRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurer;

import org.springframework.beans.factory.annotation.Value;

@Configuration
public class CorsConfig {
    @Value("${cors.origin}")
    private String origin;

    @Bean
    public WebMvcConfigurer corsConfigurer() {
        return new WebMvcConfigurer() {
            @Override
            public void addCorsMappings(CorsRegistry registry) {
                registry.addMapping("/api/**")
                        .allowedOrigins(origin)
                        .allowedMethods("*")
                        .maxAge(3600)
                        .allowedHeaders("*")
                        .exposedHeaders("*");
            }
        };
    }
}