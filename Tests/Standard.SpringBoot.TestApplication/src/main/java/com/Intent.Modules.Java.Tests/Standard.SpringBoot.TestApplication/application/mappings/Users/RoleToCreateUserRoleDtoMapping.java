package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.CreateUserRoleDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;

public class RoleToCreateUserRoleDtoMapping extends PropertyMap<Role, CreateUserRoleDto> {
    protected void configure() {
    }
}