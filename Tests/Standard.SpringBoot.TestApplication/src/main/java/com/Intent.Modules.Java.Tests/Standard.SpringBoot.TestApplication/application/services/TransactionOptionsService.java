package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application.services;

public interface TransactionOptionsService {
    void TransactionDefault();

    void NoTransaction();

    void TransactionReadOnly();

    void TransactionIsolationLevel();

    void TransactionPropagationLevel();

    void TransactionTimeout();

}