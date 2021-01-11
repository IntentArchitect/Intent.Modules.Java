package com.samples.app.beans;


import com.samples.app.application.models.UserDto;
import com.samples.app.domain.models.User;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class ModelMapperBean {
    @Bean
    public ModelMapper modelMapper() {
        var modelMapper = new ModelMapper();

        modelMapper.addMappings(new UserToUserDtoMapping());
        return modelMapper;
    }
}
