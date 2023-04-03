package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.InheritanceAssociations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.InheritanceAssociations.TptConcreteBaseClass;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.UUID;

@IntentIgnoreBody
public interface TptConcreteBaseClassRepository extends JpaRepository<TptConcreteBaseClass, UUID> {
}