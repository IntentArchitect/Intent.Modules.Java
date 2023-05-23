package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.impl;

import lombok.AllArgsConstructor;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services.TransactionOptionsService;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentIgnoreBody;
import com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.intent.IntentMerge;
import org.springframework.transaction.annotation.Isolation;
import org.springframework.transaction.annotation.Propagation;

@Service
@AllArgsConstructor
@IntentMerge
public class TransactionOptionsServiceImpl implements TransactionOptionsService {
    @Override
    @Transactional(readOnly = false)
    @IntentIgnoreBody
    public void TransactionDefault() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @IntentIgnoreBody
    public void NoTransaction() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = true)
    @IntentIgnoreBody
    public void TransactionReadOnly() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false, isolation = Isolation.READ_COMMITTED)
    @IntentIgnoreBody
    public void TransactionIsolationLevel() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false, propagation = Propagation.REQUIRES_NEW)
    @IntentIgnoreBody
    public void TransactionPropagationLevel() {
        throw new UnsupportedOperationException("Your implementation here...");
    }

    @Override
    @Transactional(readOnly = false, timeout = 30)
    @IntentIgnoreBody
    public void TransactionTimeout() {
        throw new UnsupportedOperationException("Your implementation here...");
    }
}