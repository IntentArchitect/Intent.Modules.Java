package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.Polymorphic;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.Polymorphic.TpcPoly_RootAbstract_Aggr;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface TpcPoly_RootAbstract_AggrRepository extends JpaRepository<TpcPoly_RootAbstract_Aggr, UUID> {
}
