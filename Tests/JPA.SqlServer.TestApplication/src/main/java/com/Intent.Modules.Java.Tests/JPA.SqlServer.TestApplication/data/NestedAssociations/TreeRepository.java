package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.NestedAssociations;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.NestedAssociations.Tree;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface TreeRepository extends JpaRepository<Tree, UUID> {
}
