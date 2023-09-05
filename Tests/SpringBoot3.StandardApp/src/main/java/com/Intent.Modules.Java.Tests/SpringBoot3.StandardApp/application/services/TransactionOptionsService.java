package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.services;

public interface TransactionOptionsService {
    void TransactionDefault();

    void NoTransaction();

    void TransactionReadOnly();

    void TransactionIsolationLevel();

    void TransactionPropagationLevel();

    void TransactionTimeout();

}