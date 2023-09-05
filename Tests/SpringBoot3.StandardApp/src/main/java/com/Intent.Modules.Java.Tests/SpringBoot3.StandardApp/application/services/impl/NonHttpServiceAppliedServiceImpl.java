package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.NonHttpServiceAppliedService;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;
import org.springframework.transaction.annotation.Transactional;

@Service
@AllArgsConstructor
@IntentMerge
public class NonHttpServiceAppliedServiceImpl implements NonHttpServiceAppliedService {
    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void BasicOperation() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}
