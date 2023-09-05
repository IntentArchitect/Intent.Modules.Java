package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserCreateDto {
    private String username;
    private String email;
    private List<CreateUserRoleDto> roles;

    public static UserCreateDto mapFromUser(User user, ModelMapper mapper) {
        return mapper.map(user, UserCreateDto.class);
    }

    public static List<UserCreateDto> mapFromUsers(Collection<User> users, ModelMapper mapper) {
        return users
            .stream()
            .map(user -> UserCreateDto.mapFromUser(user, mapper))
            .collect(Collectors.toList());
    }
}