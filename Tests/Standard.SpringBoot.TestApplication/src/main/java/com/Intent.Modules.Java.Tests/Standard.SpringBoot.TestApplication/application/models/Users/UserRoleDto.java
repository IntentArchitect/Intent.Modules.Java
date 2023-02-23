package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserRoleDto {
    private UUID userId;
    private String name;
    private UUID Id;

    public static UserRoleDto mapFromRole(Role role, ModelMapper mapper) {
        return mapper.map(role, UserRoleDto.class);
    }

    public static List<UserRoleDto> mapFromRoles(Collection<Role> roles, ModelMapper mapper) {
        return roles
            .stream()
            .map(role -> UserRoleDto.mapFromRole(role, mapper))
            .collect(Collectors.toList());
    }
}