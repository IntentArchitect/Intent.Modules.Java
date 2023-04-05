package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.ExplicitKeys;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.ExplicitKeys.PK_PrimaryKeyInt;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;

@IntentMerge
public interface PK_PrimaryKeyIntRepository extends JpaRepository<PK_PrimaryKeyInt, Integer> {
}
