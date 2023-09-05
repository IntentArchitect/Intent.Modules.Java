package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.authentication;

import lombok.Data;
import lombok.NoArgsConstructor;
import lombok.AllArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class AuthenticationRequest {
    private String username;
    private String password;

}