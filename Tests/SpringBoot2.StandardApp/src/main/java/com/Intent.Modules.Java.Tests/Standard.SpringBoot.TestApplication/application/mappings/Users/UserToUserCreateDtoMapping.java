package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserCreateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.User;

public class UserToUserCreateDtoMapping extends PropertyMap<User, UserCreateDto> {
    protected void configure() {
    }
}