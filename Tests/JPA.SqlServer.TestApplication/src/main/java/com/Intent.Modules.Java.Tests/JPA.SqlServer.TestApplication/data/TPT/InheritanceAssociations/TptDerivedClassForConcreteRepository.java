package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.InheritanceAssociations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.InheritanceAssociations.TptDerivedClassForConcrete;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.UUID;

@IntentIgnoreBody
public interface TptDerivedClassForConcreteRepository extends JpaRepository<TptDerivedClassForConcrete, UUID> {
}