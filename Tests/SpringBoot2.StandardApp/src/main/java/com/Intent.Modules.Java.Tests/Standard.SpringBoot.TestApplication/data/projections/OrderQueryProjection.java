package com.Intent.Modules.Java.Tests.Standard.SpringBoot.TestApplication.data.projections;

import java.util.UUID;

public interface OrderQueryProjection {
    String getNumber();

    UUID getId();
}