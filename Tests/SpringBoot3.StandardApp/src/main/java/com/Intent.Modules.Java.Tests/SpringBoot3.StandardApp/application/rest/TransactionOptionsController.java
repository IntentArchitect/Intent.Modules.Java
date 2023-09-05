package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.TransactionOptionsService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;

@RestController
@RequestMapping("/api/transaction-options")
@Tag(name = "TransactionOptionsService")
@AllArgsConstructor
public class TransactionOptionsController {
    private final TransactionOptionsService transactionOptionsService;

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/transaction-default")
    @Operation(summary = "TransactionDefault")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void TransactionDefault() {
        transactionOptionsService.TransactionDefault();
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/no-transaction")
    @Operation(summary = "NoTransaction")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void NoTransaction() {
        transactionOptionsService.NoTransaction();
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/transaction-read-only")
    @Operation(summary = "TransactionReadOnly")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void TransactionReadOnly() {
        transactionOptionsService.TransactionReadOnly();
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/transaction-isolation-level")
    @Operation(summary = "TransactionIsolationLevel")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void TransactionIsolationLevel() {
        transactionOptionsService.TransactionIsolationLevel();
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/transaction-propagation-level")
    @Operation(summary = "TransactionPropagationLevel")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void TransactionPropagationLevel() {
        transactionOptionsService.TransactionPropagationLevel();
    }

    @ResponseStatus(HttpStatus.CREATED)
    @PostMapping(path = "/transaction-timeout")
    @Operation(summary = "TransactionTimeout")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created.") })
    public void TransactionTimeout() {
        transactionOptionsService.TransactionTimeout();
    }
}
