package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPH.InheritanceAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations.FkBaseClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.InheritanceAssociations.FkBaseClassId;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

@IntentIgnoreBody
public interface FkBaseClassRepository extends JpaRepository<FkBaseClass, FkBaseClassId> {
}
