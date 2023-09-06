package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.MapFieldService;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.responses.ApiResponse;
import io.swagger.v3.oas.annotations.responses.ApiResponses;
import io.swagger.v3.oas.annotations.tags.Tag;
import java.util.Map;

@RestController
@RequestMapping("/api/map-field")
@Tag(name = "MapFieldService")
@AllArgsConstructor
public class MapFieldController {
    private final MapFieldService mapFieldService;

    @GetMapping(path = "/receive-map-param")
    @Operation(summary = "ReceiveMapParam")
    @ApiResponses(value = {
        @ApiResponse(responseCode = "200", description = "Returns the specified String."),
        @ApiResponse(responseCode = "400", description = "One or more validation errors have occurred."),
        @ApiResponse(responseCode = "404", description = "Can\'t find an String with the parameters provided.") })
    public ResponseEntity<String> ReceiveMapParam(@RequestParam Map<String, String> map) {
        final String result = mapFieldService.ReceiveMapParam(map);

        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}
