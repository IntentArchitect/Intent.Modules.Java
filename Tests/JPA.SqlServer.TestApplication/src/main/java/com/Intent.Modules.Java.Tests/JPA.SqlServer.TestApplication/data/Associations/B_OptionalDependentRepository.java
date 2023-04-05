package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.B_OptionalDependent;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface B_OptionalDependentRepository extends JpaRepository<B_OptionalDependent, UUID> {
}
