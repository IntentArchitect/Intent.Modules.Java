package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.NonHttpServiceAppliedService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;

@Service
@AllArgsConstructor
@IntentMerge
public class NonHttpServiceAppliedServiceImpl implements NonHttpServiceAppliedService {
    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void Operation1() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}
