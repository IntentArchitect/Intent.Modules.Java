package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.InheritanceAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations.DerivedClassForAbstract;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface DerivedClassForAbstractRepository extends JpaRepository<DerivedClassForAbstract, UUID> {
}
