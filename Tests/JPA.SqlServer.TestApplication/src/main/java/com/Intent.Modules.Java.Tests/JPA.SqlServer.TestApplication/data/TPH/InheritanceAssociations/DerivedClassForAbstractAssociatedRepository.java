package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPH.InheritanceAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations.DerivedClassForAbstractAssociated;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface DerivedClassForAbstractAssociatedRepository extends JpaRepository<DerivedClassForAbstractAssociated, UUID> {
}
