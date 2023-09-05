package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UserRoleDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.Role;

public class RoleToUserRoleDtoMapping extends PropertyMap<Role, UserRoleDto> {
    protected void configure() {
    }
}