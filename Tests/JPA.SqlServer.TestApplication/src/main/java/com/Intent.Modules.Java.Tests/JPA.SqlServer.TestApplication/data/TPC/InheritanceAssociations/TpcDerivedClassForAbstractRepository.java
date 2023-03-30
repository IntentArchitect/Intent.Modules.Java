package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.InheritanceAssociations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations.TpcDerivedClassForAbstract;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TpcDerivedClassForAbstractRepository extends JpaRepository<TpcDerivedClassForAbstract, UUID> {
}