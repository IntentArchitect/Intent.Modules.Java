package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.CreateUserRoleDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.Role;

public class RoleToCreateUserRoleDtoMapping extends PropertyMap<Role, CreateUserRoleDto> {
    protected void configure() {
    }
}