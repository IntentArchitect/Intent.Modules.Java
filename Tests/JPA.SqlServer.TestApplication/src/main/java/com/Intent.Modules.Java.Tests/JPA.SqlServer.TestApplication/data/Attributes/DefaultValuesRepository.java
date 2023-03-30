package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Attributes;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Attributes.DefaultValues;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface DefaultValuesRepository extends JpaRepository<DefaultValues, UUID> {
}