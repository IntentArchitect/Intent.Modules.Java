package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserCreateDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;

public class UserToUserCreateDtoMapping extends PropertyMap<User, UserCreateDto> {
    protected void configure() {
    }
}