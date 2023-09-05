package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.models.Users;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.domain.models.User;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class UserUpdateDto {
    private UUID id;
    private String username;
    private String email;
    private List<UpdateUserRoleDto> roles;

    public static UserUpdateDto mapFromUser(User user, ModelMapper mapper) {
        return mapper.map(user, UserUpdateDto.class);
    }

    public static List<UserUpdateDto> mapFromUsers(Collection<User> users, ModelMapper mapper) {
        return users
            .stream()
            .map(user -> UserUpdateDto.mapFromUser(user, mapper))
            .collect(Collectors.toList());
    }
}