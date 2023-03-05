package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.F_OptionalAggregateNav;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface F_OptionalAggregateNavRepository extends JpaRepository<F_OptionalAggregateNav, UUID> {
}
