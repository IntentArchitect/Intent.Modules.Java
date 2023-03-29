package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.J_RequiredDependent;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.UUID;

@IntentIgnoreBody
public interface J_RequiredDependentRepository extends JpaRepository<J_RequiredDependent, UUID> {
}