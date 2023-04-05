package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Attributes;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Attributes.DefaultValues;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface DefaultValuesRepository extends JpaRepository<DefaultValues, UUID> {
}
