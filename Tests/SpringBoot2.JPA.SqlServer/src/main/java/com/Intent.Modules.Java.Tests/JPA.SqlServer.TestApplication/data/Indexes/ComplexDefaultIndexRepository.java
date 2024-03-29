package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Indexes;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes.ComplexDefaultIndex;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.List;
import java.util.UUID;

@IntentMerge
public interface ComplexDefaultIndexRepository extends JpaRepository<ComplexDefaultIndex, UUID> {
    List<ComplexDefaultIndex> findByFieldAAndFieldB(UUID fieldA, UUID fieldB);
}