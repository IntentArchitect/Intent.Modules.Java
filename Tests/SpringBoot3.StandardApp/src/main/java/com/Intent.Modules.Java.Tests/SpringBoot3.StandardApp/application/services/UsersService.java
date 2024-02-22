package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services;

import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserCreateDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserUpdateDto;
import java.util.List;
import java.util.UUID;

public interface UsersService {
    UUID Create(UserCreateDto dto);

    UserDto FindById(UUID id);

    List<UserDto> FindAll();

    void Put(UUID id, UserUpdateDto dto);

    UserDto Delete(UUID id);

}