package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import java.util.stream.Collectors;
import java.util.stream.Collectors;
import java.util.function.Function;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.CreateUserRoleDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UpdateUserRoleDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserCreateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.Users.UserUpdateDto;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.UsersService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data.UserRepository;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.Role;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.User;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import java.util.List;
import java.util.UUID;
import org.modelmapper.ModelMapper;

@Service
@AllArgsConstructor
@IntentMerge
public class UsersServiceImpl implements UsersService {
    private UserRepository userRepository;
    private ModelMapper mapper;

    @Override
    @Transactional(readOnly = false)
    public UUID Create(UserCreateDto dto) {
        var user = new User();
        user.setUsername(dto.getUsername());
        user.setEmail(dto.getEmail());
        user.setRoles(dto.getRoles().stream().map(x -> createRole(x)).collect(Collectors.toList()));
        userRepository.save(user);
        return user.getId();
    }

    @Override
    @Transactional(readOnly = true)
    public UserDto FindById(UUID id) {
        var user = userRepository.findById(id);
        if (!user.isPresent()) {
            return null;
        }
        return UserDto.mapFromUser(user.get(), mapper);
    }

    @Override
    @Transactional(readOnly = true)
    public List<UserDto> FindAll() {
        var users = userRepository.findAll();
        return UserDto.mapFromUsers(users, mapper);
    }

    @Override
    @Transactional(readOnly = false)
    public void Put(UUID id, UserUpdateDto dto) {
        var user = userRepository.findById(id).get();
        user.setUsername(dto.getUsername());
        user.setEmail(dto.getEmail());
        updateRoles(user.getRoles(), dto.getRoles());
        userRepository.save(user);
    }

    @Override
    @Transactional(readOnly = false)
    public UserDto Delete(UUID id) {
        var user = userRepository.findById(id);
        if (!user.isPresent()) {
            return null;
        }
        var userDto = UserDto.mapFromUser(user.get(), mapper);
        userRepository.delete(user.get());
        return userDto;
    }

    private static Role createRole(CreateUserRoleDto dto) {
        var role = new Role();
        role.setName(dto.getName());
        return role;
    }

    private static void updateRoles(List<Role> existingRoles, List<UpdateUserRoleDto> updatedRoles) {
        var updateRoleMap = updatedRoles.stream().collect(Collectors.toMap(UpdateUserRoleDto::getId, Function.identity()));
        for (Role existRole : existingRoles.stream().toList()) {
            var updateRole = updateRoleMap.get(existRole.getId());
            if (updateRole != null) {
                existRole.setUserId(updateRole.getUserId());
                existRole.setName(updateRole.getName());
                updateRoleMap.remove(existRole.getId());
            }
            else {
                existingRoles.remove(existRole);
            }
        }
        for (var newRole : updateRoleMap.values()) {
            existingRoles.add(createRole(newRole));
        }
    }

    private static Role createRole(UpdateUserRoleDto dto) {
        var role = new Role();
        role.setUserId(dto.getUserId());
        role.setName(dto.getName());
        return role;
    }
}
