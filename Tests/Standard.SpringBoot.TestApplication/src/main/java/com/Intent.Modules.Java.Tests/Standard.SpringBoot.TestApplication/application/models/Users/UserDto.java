package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.User;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserDto {
    private UUID Id;
    private String Username;
    private String Email;
    private List<UserRoleDto> Roles;

    public static UserDto mapFromUser(User user, ModelMapper mapper) {
        return mapper.map(user, UserDto.class);
    }

    public static List<UserDto> mapFromUsers(Collection<User> users, ModelMapper mapper) {
        return users
            .stream()
            .map(user -> UserDto.mapFromUser(user, mapper))
            .collect(Collectors.toList());
    }
}