package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.InheritanceAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.InheritanceAssociations.FkBaseClassAssociated;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface FkBaseClassAssociatedRepository extends JpaRepository<FkBaseClassAssociated, UUID> {
}
