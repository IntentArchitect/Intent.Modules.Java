package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.configuration;

import org.modelmapper.ModelMapper;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.ClassAS.ClassAToClassADTOMapping;
import org.modelmapper.convention.MatchingStrategies;

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
        modelMapper.addMappings(new ClassAToClassADTOMapping());
    }
}
