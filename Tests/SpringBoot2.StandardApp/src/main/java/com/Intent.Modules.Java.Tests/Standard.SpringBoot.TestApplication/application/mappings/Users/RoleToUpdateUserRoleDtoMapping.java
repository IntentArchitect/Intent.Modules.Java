package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UpdateUserRoleDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;

public class RoleToUpdateUserRoleDtoMapping extends PropertyMap<Role, UpdateUserRoleDto> {
    protected void configure() {
    }
}