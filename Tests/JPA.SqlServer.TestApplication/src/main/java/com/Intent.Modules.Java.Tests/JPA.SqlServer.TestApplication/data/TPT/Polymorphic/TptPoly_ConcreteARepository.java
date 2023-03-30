package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.Polymorphic;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.Polymorphic.TptPoly_ConcreteA;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TptPoly_ConcreteARepository extends JpaRepository<TptPoly_ConcreteA, UUID> {
}