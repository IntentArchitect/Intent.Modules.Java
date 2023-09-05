package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationRequest;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationResult;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.AuthService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.exceptions.AuthenticationFailedException;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.security.JwtUtils;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.crypto.factory.PasswordEncoderFactories;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.transaction.annotation.Transactional;

@Service
@AllArgsConstructor
@IntentMerge
public class AuthServiceImpl implements AuthService {
    private UserDetailsService userDetailsService;
    private JwtUtils jwtUtils;

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public AuthenticationResult authenticate(AuthenticationRequest request) throws AuthenticationFailedException {
        UserDetails userDetails = userDetailsService.loadUserByUsername(request.getUsername());
        PasswordEncoder passwordEncoder = PasswordEncoderFactories.createDelegatingPasswordEncoder();
        if (!passwordEncoder.matches(request.getPassword(), userDetails.getPassword())) {
        throw new AuthenticationFailedException();
        }
        
        String token = jwtUtils.generateJwtToken(userDetails);
        return new AuthenticationResult(token);
    }
}