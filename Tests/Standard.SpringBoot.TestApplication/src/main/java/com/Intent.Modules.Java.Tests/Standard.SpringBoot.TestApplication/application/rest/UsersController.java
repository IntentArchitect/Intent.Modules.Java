package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserCreateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserUpdateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.UsersService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import java.util.List;
import java.util.UUID;
import javax.validation.Valid;

@RestController
@RequestMapping("/api/users")
@Tag(name = "UsersService")
@AllArgsConstructor
public class UsersController {
    private final UsersService usersService;

    @PostMapping
    @Operation(summary = "Create")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public ResponseEntity<JsonResponse<UUID>> Create(@Valid @Parameter(required = true) @RequestBody UserCreateDto dto) {
        final UUID result = usersService.Create(dto);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(new JsonResponse<UUID>(result), HttpStatus.OK);
    }

    @GetMapping(path = "/{id}")
    @Operation(summary = "FindById")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified UserDto."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an UserDto with the parameters provided.") })
    public ResponseEntity<UserDto> FindById(@Parameter(required = true) @PathVariable(value = "id") UUID id) {
        final UserDto result = usersService.FindById(id);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping
    @Operation(summary = "FindAll")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified List<UserDto>.") })
    public ResponseEntity<List<UserDto>> FindAll() {
        final List<UserDto> result = usersService.FindAll();
        if (result.isEmpty()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PutMapping(path = "/{id}")
    @Operation(summary = "Put")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "204", description = "Successfully updated."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void Put(@Parameter(required = true) @PathVariable(value = "id") UUID id, @Valid @Parameter(required = true) @RequestBody UserUpdateDto dto) {
        usersService.Put(id, dto);
    }

    @DeleteMapping(path = "/{id}")
    @Operation(summary = "Delete")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Successfully deleted."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public ResponseEntity<UserDto> Delete(@Parameter(required = true) @PathVariable(value = "id") UUID id) {
        final UserDto result = usersService.Delete(id);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}
