package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.D_MultipleDependent;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface D_MultipleDependentRepository extends JpaRepository<D_MultipleDependent, UUID> {
}
