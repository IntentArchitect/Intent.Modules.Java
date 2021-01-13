package com.samples.app.application.rest;

import lombok.AllArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
import java.util.Map;
import com.samples.app.application.services.TestService;
import com.samples.app.application.models.UserUpdateDto;
import com.samples.app.application.models.UserDto;
import com.samples.app.application.models.UserCreateDto;
import lombok.extern.slf4j.Slf4j;

@RestController
@RequestMapping("/api/test")
@AllArgsConstructor
@Slf4j
public class TestController {
    private final TestService testService;

    @GetMapping("/{id}")
    public ResponseEntity<UserDto> Get(@PathVariable(value = "id") long id, @RequestHeader Map<String, String> headers) {
        final UserDto result = testService.Get(id);

        return new ResponseEntity<>(result, HttpStatus.OK);
    }

    @PostMapping
    public void Create(@RequestBody UserCreateDto dto, @RequestHeader Map<String, String> headers) {
        testService.Create(dto);
    }

    @PutMapping("/{id}")
    public void Update(@PathVariable(value = "id") long id, @RequestBody UserUpdateDto dto, @RequestHeader Map<String, String> headers) {
        testService.Update(id, dto);
    }

    @DeleteMapping("/{id}")
    public void Delete(@PathVariable(value = "id") long id, @RequestHeader Map<String, String> headers) {
        testService.Delete(id);
    }

    @GetMapping("/findall")
    public ResponseEntity<List<UserDto>> FindAll(@RequestHeader Map<String, String> headers) {
        final List<UserDto> result = testService.FindAll();

        return new ResponseEntity<>(result, HttpStatus.OK);
    }
}