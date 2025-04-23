package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.Role;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UpdateUserRoleDto {
    private UUID id;
    private String name;
    private UUID userId;

    public static UpdateUserRoleDto mapFromRole(Role role, ModelMapper mapper) {
        return mapper.map(role, UpdateUserRoleDto.class);
    }

    public static List<UpdateUserRoleDto> mapFromRoles(Collection<Role> roles, ModelMapper mapper) {
        return roles
            .stream()
            .map(role -> UpdateUserRoleDto.mapFromRole(role, mapper))
            .collect(Collectors.toList());
    }
}