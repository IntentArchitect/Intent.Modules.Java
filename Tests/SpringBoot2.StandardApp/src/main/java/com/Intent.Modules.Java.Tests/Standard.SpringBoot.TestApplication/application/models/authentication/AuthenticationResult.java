package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication;

import lombok.Data;
import lombok.NoArgsConstructor;
import javax.validation.constraints.NotNull;
import lombok.AllArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class AuthenticationResult {
    private String token;

}