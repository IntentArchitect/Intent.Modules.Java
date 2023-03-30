package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.ExplicitKeys;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys.PK_PrimaryKeyLong;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

@IntentIgnoreBody
public interface PK_PrimaryKeyLongRepository extends JpaRepository<PK_PrimaryKeyLong, Long> {
}