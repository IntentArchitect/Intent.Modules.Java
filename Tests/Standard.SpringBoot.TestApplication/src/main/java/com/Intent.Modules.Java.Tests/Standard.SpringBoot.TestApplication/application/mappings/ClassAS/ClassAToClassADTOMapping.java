package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.mappings.ClassAS;

import org.modelmapper.PropertyMap;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassADTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.ClassA;

public class ClassAToClassADTOMapping extends PropertyMap<ClassA, ClassADTO> {
    protected void configure() {
    }
}