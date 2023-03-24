package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.ExplicitKeys;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys.PK_A_CompositeKey;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys.PK_A_CompositeKeyId;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

@IntentIgnoreBody
public interface PK_A_CompositeKeyRepository extends JpaRepository<PK_A_CompositeKey, PK_A_CompositeKeyId> {
}