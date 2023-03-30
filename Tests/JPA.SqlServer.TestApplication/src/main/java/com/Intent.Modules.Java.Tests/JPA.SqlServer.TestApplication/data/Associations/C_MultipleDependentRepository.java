package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.C_MultipleDependent;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.UUID;

@IntentIgnoreBody
public interface C_MultipleDependentRepository extends JpaRepository<C_MultipleDependent, UUID> {
}