package com.samples.app.application.services;
import com.samples.app.application.models.UserUpdateDto;

import com.samples.app.application.models.UserDto;

import java.util.List;
import com.samples.app.application.models.UserCreateDto;


public interface TestService {
    UserDto Get(long id);

    void Create(UserCreateDto dto);

    void Update(long id, UserUpdateDto dto);

    void Delete(long id);

    List<UserDto> FindAll();

}