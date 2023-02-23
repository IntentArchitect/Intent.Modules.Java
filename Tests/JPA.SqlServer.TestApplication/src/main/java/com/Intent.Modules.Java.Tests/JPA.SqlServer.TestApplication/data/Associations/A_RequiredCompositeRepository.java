package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.A_RequiredComposite;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

/**
 * Spring Data JPA repository for the A_RequiredComposite entity.
 */
@IntentIgnoreBody
public interface A_RequiredCompositeRepository extends JpaRepository<A_RequiredComposite, UUID> {
}