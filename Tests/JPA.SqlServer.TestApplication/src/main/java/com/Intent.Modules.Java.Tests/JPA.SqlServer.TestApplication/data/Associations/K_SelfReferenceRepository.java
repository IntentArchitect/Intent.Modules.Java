package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.K_SelfReference;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.UUID;

@IntentIgnoreBody
public interface K_SelfReferenceRepository extends JpaRepository<K_SelfReference, UUID> {
}