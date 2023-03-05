package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.NestedAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.NestedAssociations.Inhabitant;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface InhabitantRepository extends JpaRepository<Inhabitant, UUID> {
}
