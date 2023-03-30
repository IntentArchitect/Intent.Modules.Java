package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.E_RequiredCompositeNav;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface E_RequiredCompositeNavRepository extends JpaRepository<E_RequiredCompositeNav, UUID> {
}