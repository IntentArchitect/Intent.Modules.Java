package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Indexes;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes.DefaultIndex;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;
import java.util.UUID;

@IntentIgnoreBody
public interface DefaultIndexRepository extends JpaRepository<DefaultIndex, UUID> {

    List<DefaultIndex> findByIndexField(String indexField);
}