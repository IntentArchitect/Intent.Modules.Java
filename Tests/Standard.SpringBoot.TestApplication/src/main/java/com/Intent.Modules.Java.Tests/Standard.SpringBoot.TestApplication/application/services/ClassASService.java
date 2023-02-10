package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services;

import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassACreateDTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassADTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassAUpdateDTO;
import java.util.List;
import java.util.UUID;


public interface ClassASService {
    void Create(ClassACreateDTO dto);

    ClassADTO FindById(UUID id);

    List<ClassADTO> FindAll();

    void Update(UUID id, ClassAUpdateDTO dto);

    void Delete(UUID id);

}