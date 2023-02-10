package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassACreateDTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassADTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.models.ClassAS.ClassAUpdateDTO;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.ClassASService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data.ClassARepository;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models.ClassA;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import java.util.List;
import java.util.UUID;
import org.modelmapper.ModelMapper;

@Service
@AllArgsConstructor
@IntentMerge
public class ClassASServiceImpl implements ClassASService {
    private ClassARepository classARepository;
    private ModelMapper mapper;

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Create(ClassACreateDTO dto) {
        var classA = new ClassA();
        classA.setAttribute(dto.getAttribute());
        classARepository.save(classA);
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public ClassADTO FindById(UUID id) {
        var classA = classARepository.findById(id);
        if (!classA.isPresent()) {
            return null;
        }
        return ClassADTO.mapFromClassA(classA.get(), mapper);
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public List<ClassADTO> FindAll() {
        var classAs = classARepository.findAll();
        return ClassADTO.mapFromClassAs(classAs, mapper);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Update(UUID id, ClassAUpdateDTO dto) {
        var classA = classARepository.findById(id).get();
        classA.setId(dto.getId());
        classA.setAttribute(dto.getAttribute());
        classARepository.save(classA);
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Delete(UUID id) {
        var classA = classARepository.findById(id).get();
        classARepository.delete(classA);
    }
}