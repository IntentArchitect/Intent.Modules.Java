package com.samples.app.application.models;

import lombok.AllArgsConstructor;
import lombok.Data;
import lombok.NoArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserUpdateDto {
    private String firstName;
    private String lastName;
    private String fullName;
    private String phoneNumber;
}