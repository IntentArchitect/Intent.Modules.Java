package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class CreateUserRoleDto {
    private String name;

    public static CreateUserRoleDto mapFromRole(Role role, ModelMapper mapper) {
        return mapper.map(role, CreateUserRoleDto.class);
    }

    public static List<CreateUserRoleDto> mapFromRoles(Collection<Role> roles, ModelMapper mapper) {
        return roles
            .stream()
            .map(role -> CreateUserRoleDto.mapFromRole(role, mapper))
            .collect(Collectors.toList());
    }
}