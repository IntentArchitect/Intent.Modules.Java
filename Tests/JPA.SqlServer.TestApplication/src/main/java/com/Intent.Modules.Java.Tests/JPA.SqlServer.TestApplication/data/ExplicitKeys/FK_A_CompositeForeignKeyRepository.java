package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.ExplicitKeys;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys.FK_A_CompositeForeignKey;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface FK_A_CompositeForeignKeyRepository extends JpaRepository<FK_A_CompositeForeignKey, UUID> {
}
