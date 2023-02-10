package com.samples.app.beans;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import com.samples.app.application.mappings.UserToUserDtoMapping;

@Configuration
public class ModelMapperBean {
    @Bean
    public ModelMapper modelMapper() {
        var modelMapper = new ModelMapper();

        InitializeMappings(modelMapper);

        return modelMapper;
    }

    private void InitializeMappings(ModelMapper modelMapper) {
        modelMapper.addMappings(new UserToUserDtoMapping());
    }
}