package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services;

import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationRequest;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationResult;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.exceptions.AuthenticationFailedException;


public interface AuthService {
    AuthenticationResult authenticate(AuthenticationRequest request)
            throws AuthenticationFailedException;

}