package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPH.Polymorphic;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPH.Polymorphic.Poly_BaseClassNonAbstract;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface Poly_BaseClassNonAbstractRepository extends JpaRepository<Poly_BaseClassNonAbstract, UUID> {
}
