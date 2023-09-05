package com.Intent.Modules.Java.Tests.SpringBoot3.StandardApp.application.rest;

public class JsonResponse<T> {
    public JsonResponse(T value) {
        this.value = value;
    }

    private T value;
    public T getValue() {
        return this.value;
    }
    public void setValue(T value) {
        this.value = value;
    }
}