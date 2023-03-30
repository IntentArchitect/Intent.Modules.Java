package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.L_SelfReferenceMultiple;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface L_SelfReferenceMultipleRepository extends JpaRepository<L_SelfReferenceMultiple, UUID> {
}