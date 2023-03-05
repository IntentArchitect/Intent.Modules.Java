package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Indexes;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes.StereotypeIndex;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.List;
import java.util.UUID;

@IntentIgnoreBody
public interface StereotypeIndexRepository extends JpaRepository<StereotypeIndex, UUID> {

    List<StereotypeIndex> findByDefaultIndexField(UUID defaultIndexField);
    List<StereotypeIndex> findByCustomIndexField(UUID customIndexField);
    List<StereotypeIndex> findByGroupedIndexFieldAAndGroupedIndexFieldB(UUID groupedIndexFieldA, UUID groupedIndexFieldB);
}
