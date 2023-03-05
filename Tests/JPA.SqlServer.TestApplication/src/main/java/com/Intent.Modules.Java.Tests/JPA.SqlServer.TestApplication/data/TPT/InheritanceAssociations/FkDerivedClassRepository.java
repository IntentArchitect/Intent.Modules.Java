package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.InheritanceAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.InheritanceAssociations.FkBaseClassId;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.InheritanceAssociations.FkDerivedClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

@IntentIgnoreBody
public interface FkDerivedClassRepository extends JpaRepository<FkDerivedClass, FkBaseClassId> {
}
