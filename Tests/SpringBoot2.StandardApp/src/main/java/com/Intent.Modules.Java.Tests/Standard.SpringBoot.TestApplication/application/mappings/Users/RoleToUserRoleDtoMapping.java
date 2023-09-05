package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserRoleDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;

public class RoleToUserRoleDtoMapping extends PropertyMap<Role, UserRoleDto> {
    protected void configure() {
    }
}