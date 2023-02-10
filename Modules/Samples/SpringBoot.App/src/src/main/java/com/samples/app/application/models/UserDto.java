package com.samples.app.application.models;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserDto {
    private long id;
    private String firstName;
    private String lastName;
    private String fullName;
    private String phone;
    private String username;
    private String emailAddress;
    private String password;
}