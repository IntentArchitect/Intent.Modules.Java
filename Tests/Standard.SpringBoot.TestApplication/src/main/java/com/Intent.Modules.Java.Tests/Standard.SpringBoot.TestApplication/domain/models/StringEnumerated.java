package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.domain.models;

import com.fasterxml.jackson.annotation.JsonValue;

public enum StringEnumerated {
    VALUE_ONE(1),
    VALUE_TWO(2);

    @JsonValue
    public final int value;

    private StringEnumerated(int value) {
        this.value = value;
    }
}