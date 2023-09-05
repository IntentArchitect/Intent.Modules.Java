package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.User;

public class UserToUserDtoMapping extends PropertyMap<User, UserDto> {
    protected void configure() {
    }
}