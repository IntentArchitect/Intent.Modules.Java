package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserUpdateDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;

public class UserToUserUpdateDtoMapping extends PropertyMap<User, UserUpdateDto> {
    protected void configure() {
    }
}