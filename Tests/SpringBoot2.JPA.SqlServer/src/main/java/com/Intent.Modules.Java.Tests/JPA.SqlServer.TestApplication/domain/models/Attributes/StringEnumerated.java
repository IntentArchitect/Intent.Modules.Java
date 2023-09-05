package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Attributes;

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