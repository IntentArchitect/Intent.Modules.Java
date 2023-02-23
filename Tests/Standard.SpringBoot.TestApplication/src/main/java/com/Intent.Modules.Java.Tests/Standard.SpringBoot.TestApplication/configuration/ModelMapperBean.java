package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.configuration;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.modelmapper.convention.MatchingStrategies;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.RoleToCreateUserRoleDtoMapping;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.RoleToUpdateUserRoleDtoMapping;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.RoleToUserRoleDtoMapping;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.UserToUserCreateDtoMapping;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.UserToUserDtoMapping;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.Users.UserToUserUpdateDtoMapping;

@Configuration
public class ModelMapperBean {
    @Bean
    public ModelMapper modelMapper() {
        var modelMapper = new ModelMapper();

        InitializeMappings(modelMapper);

        return modelMapper;
    }

    private void InitializeMappings(ModelMapper modelMapper) {
        modelMapper.getConfiguration().setMatchingStrategy(MatchingStrategies.STRICT);
        modelMapper.addMappings(new RoleToCreateUserRoleDtoMapping());
        modelMapper.addMappings(new RoleToUpdateUserRoleDtoMapping());
        modelMapper.addMappings(new UserToUserCreateDtoMapping());
        modelMapper.addMappings(new UserToUserDtoMapping());
        modelMapper.addMappings(new RoleToUserRoleDtoMapping());
        modelMapper.addMappings(new UserToUserUpdateDtoMapping());
    }
}