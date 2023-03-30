package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Indexes;

import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Indexes.CustomIndex;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;
import java.util.UUID;

@IntentIgnoreBody
public interface CustomIndexRepository extends JpaRepository<CustomIndex, UUID> {

    List<CustomIndex> findByIndexField(UUID indexField);
}