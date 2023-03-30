package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.B_OptionalAggregate;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface B_OptionalAggregateRepository extends JpaRepository<B_OptionalAggregate, UUID> {
}