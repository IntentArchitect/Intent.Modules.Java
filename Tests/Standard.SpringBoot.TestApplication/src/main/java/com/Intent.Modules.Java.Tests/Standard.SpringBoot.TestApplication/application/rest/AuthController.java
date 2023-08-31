package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationRequest;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.authentication.AuthenticationResult;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.AuthService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.exceptions.AuthenticationFailedException;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import javax.validation.Valid;
import org.springframework.web.server.ResponseStatusException;
import org.springframework.http.HttpStatus;

@RestController
@RequestMapping("/api/auth")
@Tag(name = "AuthService")
@AllArgsConstructor
public class AuthController {
    private final AuthService authService;

    @PostMapping
    @Operation(summary = "authenticate")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public ResponseEntity<AuthenticationResult> authenticate(@Valid @Parameter(required = true) @RequestBody AuthenticationRequest request) {
        try {
            final AuthenticationResult result = authService.authenticate(request);
            if (result == null) {
                return new ResponseEntity<>(HttpStatus.NOT_FOUND);
            }
    
            return new ResponseEntity<>(result, HttpStatus.CREATED);
        } catch (AuthenticationFailedException e) {
            throw new ResponseStatusException(HttpStatus.UNAUTHORIZED);
        }
    }
}
