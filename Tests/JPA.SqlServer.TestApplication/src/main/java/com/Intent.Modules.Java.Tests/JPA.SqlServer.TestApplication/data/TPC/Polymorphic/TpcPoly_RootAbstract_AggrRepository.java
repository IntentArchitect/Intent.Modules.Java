package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.Polymorphic;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.Polymorphic.TpcPoly_RootAbstract_Aggr;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface TpcPoly_RootAbstract_AggrRepository extends JpaRepository<TpcPoly_RootAbstract_Aggr, UUID> {
}