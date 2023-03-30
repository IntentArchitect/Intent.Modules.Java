package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPH.InheritanceAssociations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations.TphDerivedClassForConcrete;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TphDerivedClassForConcreteRepository extends JpaRepository<TphDerivedClassForConcrete, UUID> {
}