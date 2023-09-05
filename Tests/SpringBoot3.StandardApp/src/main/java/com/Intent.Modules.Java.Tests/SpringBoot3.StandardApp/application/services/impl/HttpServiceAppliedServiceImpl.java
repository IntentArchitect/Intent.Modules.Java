package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services.HttpServiceAppliedService;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.intent.IntentMerge;
import org.springframework.transaction.annotation.Transactional;

@Service
@AllArgsConstructor
@IntentMerge
public class HttpServiceAppliedServiceImpl implements HttpServiceAppliedService {
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
