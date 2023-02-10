package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS;

import lombok.Data;
import lombok.NoArgsConstructor;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.ClassA;
import java.util.Collection;
import java.util.List;
import java.util.stream.Collectors;
import java.util.UUID;
import lombok.AllArgsConstructor;
import org.modelmapper.ModelMapper;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class ClassADTO {
    private UUID id;
    private String attribute;

    public static ClassADTO mapFromClassA(ClassA classA, ModelMapper mapper) {
        return mapper.map(classA, ClassADTO.class);
    }

    public static List<ClassADTO> mapFromClassAs(Collection<ClassA> classAs, ModelMapper mapper) {
        return classAs
            .stream()
            .map(classA -> ClassADTO.mapFromClassA(classA, mapper))
            .collect(Collectors.toList());
    }
}