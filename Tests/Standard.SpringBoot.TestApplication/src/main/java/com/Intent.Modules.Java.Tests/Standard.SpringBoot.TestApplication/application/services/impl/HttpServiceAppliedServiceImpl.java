package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.HttpServiceAppliedService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;

@Service
@AllArgsConstructor
@IntentMerge
public class HttpServiceAppliedServiceImpl implements HttpServiceAppliedService {

    @Value("${resource.someName}")
    private final String someName;

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public String GetValue() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void PostValue(String value) {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void NonAppliedOperation() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}
