package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS;

import lombok.Data;
import lombok.NoArgsConstructor;
import java.util.UUID;
import lombok.AllArgsConstructor;

@AllArgsConstructor
@NoArgsConstructor
@Data
public class ClassAUpdateDTO {
    private UUID id;
    private String attribute;

}