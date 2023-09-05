package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserUpdateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.User;

public class UserToUserUpdateDtoMapping extends PropertyMap<User, UserUpdateDto> {
    protected void configure() {
    }
}