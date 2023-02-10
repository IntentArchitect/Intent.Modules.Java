package com.samples.app.application.models;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserCreateDto {
    private String firstName;
    private String lastName;
    private String fullName;
    private String phoneNumber;
    private String username;
    private String email;
    private String password;
}