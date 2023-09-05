package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;

public class UserToUserDtoMapping extends PropertyMap<User, UserDto> {
    protected void configure() {
    }
}