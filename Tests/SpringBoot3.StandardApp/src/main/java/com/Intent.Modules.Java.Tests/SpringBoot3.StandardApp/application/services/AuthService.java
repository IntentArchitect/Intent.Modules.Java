package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services;

import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.authentication.AuthenticationRequest;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.authentication.AuthenticationResult;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.exceptions.AuthenticationFailedException;


public interface AuthService {
    AuthenticationResult authenticate(AuthenticationRequest request)
            throws AuthenticationFailedException;

}