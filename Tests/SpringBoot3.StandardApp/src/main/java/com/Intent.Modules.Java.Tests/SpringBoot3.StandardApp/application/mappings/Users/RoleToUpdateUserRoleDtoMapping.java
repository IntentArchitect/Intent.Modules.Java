package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users.UpdateUserRoleDto;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.Role;

public class RoleToUpdateUserRoleDtoMapping extends PropertyMap<Role, UpdateUserRoleDto> {
    protected void configure() {
    }
}