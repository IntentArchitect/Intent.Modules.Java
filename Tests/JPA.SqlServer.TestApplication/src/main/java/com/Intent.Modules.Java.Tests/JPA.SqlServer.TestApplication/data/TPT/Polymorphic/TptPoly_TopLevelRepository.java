package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.Polymorphic;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.Polymorphic.TptPoly_TopLevel;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TptPoly_TopLevelRepository extends JpaRepository<TptPoly_TopLevel, UUID> {
}