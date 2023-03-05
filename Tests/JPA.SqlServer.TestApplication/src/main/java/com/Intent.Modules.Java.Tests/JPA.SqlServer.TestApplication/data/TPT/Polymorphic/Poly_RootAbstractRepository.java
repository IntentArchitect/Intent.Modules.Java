package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPT.Polymorphic;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPT.Polymorphic.Poly_RootAbstract;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface Poly_RootAbstractRepository extends JpaRepository<Poly_RootAbstract, UUID> {
}
