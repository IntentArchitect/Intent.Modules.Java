package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.application;

public interface TestInterface {
    @TestAnnotation
    void normalMethod(String value);
    @TestAnnotation
    default String methodWithBody(){
        return "";
    };
}