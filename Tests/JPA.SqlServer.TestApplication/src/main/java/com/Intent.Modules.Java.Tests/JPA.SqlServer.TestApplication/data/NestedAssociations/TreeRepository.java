package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.NestedAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.NestedAssociations.Tree;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentMerge;
import java.util.UUID;

@IntentMerge
public interface TreeRepository extends JpaRepository<Tree, UUID> {
}
