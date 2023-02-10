package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassACreateDTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassADTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassAUpdateDTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.ClassASService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.Parameter;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import java.util.List;
import java.util.UUID;
import javax.validation.Valid;

@RestController
@RequestMapping("/api/class-as")
@Tag(name = "ClassASService")
@AllArgsConstructor
public class ClassASController {
    private final ClassASService classASService;

    @ResponseStatus(HttpStatus.OK)
    @PostMapping
    @Operation(summary = "Create")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "201", description = "Successfully created."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void Create(@Valid @Parameter(required = true) @RequestBody ClassACreateDTO dto) {
        classASService.Create(dto);
    }

    @GetMapping(path = "/{id}")
    @Operation(summary = "FindById")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified ClassADTO."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an ClassADTO with the parameters provided.") })
    public ResponseEntity<ClassADTO> FindById(@Parameter(required = true) @PathVariable(value = "id") UUID id) {
        final ClassADTO result = classASService.FindById(id);
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @GetMapping
    @Operation(summary = "FindAll")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified List<ClassADTO>.") })
    public ResponseEntity<List<ClassADTO>> FindAll() {
        final List<ClassADTO> result = classASService.FindAll();
        if (result.isEmpty()) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PutMapping(path = "/{id}")
    @Operation(summary = "Update")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "204", description = "Successfully updated."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void Update(@Parameter(required = true) @PathVariable(value = "id") UUID id, @Valid @Parameter(required = true) @RequestBody ClassAUpdateDTO dto) {
        classASService.Update(id, dto);
    }

    @ResponseStatus(HttpStatus.OK)
    @DeleteMapping(path = "/{id}")
    @Operation(summary = "Delete")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Successfully deleted."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred.") })
    public void Delete(@Parameter(required = true) @PathVariable(value = "id") UUID id) {
        classASService.Delete(id);
    }
}
