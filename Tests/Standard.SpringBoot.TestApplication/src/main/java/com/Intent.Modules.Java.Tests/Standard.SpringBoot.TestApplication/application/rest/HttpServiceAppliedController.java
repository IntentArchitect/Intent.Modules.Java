package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.HttpServiceAppliedService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;

@RestController
@RequestMapping("/api/http-service-applied")
@Tag(name = "HttpServiceAppliedService")
@AllArgsConstructor
public class HttpServiceAppliedController {
    private final HttpServiceAppliedService httpServiceAppliedService;

    @GetMapping
    @Operation(summary = "GetValue")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified String."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an String with the parameters provided.") })
    public ResponseEntity<String> GetValue() {
        final String result = httpServiceAppliedService.GetValue();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping
    @Operation(summary = "PostValue")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void PostValue(@Parameter(required = true) @RequestParam(value = "value") String value) {
        httpServiceAppliedService.PostValue(value);
    }
}