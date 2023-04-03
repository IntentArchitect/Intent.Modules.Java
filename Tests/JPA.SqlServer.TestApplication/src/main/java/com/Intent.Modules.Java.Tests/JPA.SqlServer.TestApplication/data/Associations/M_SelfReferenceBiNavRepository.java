package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.Associations;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.Associations.M_SelfReferenceBiNav;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;

import org.springframework.data.jpa.repository.JpaRepository;
import java.util.UUID;

@IntentIgnoreBody
public interface M_SelfReferenceBiNavRepository extends JpaRepository<M_SelfReferenceBiNav, UUID> {
}