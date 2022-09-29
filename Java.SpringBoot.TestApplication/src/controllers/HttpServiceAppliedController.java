package root.src.controllers;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import root.src.contracts.HttpServiceAppliedService;

@RestController
@RequestMapping("/api/http-service-applied")
@AllArgsConstructor
public class HttpServiceAppliedController {
    private final HttpServiceAppliedService httpServiceAppliedService;

    @GetMapping
    public ResponseEntity<String> GetValue() {
        final String result = httpServiceAppliedService.GetValue();
        if (result == null) {
            return new ResponseEntity<>(HttpStatus.NOT_FOUND);
        }
        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @ResponseStatus(HttpStatus.OK)
    @PostMapping
    public void PostValue(@RequestParam(value = "value") String value) {
        httpServiceAppliedService.PostValue(value);
    }
}
