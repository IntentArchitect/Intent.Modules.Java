package com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.data.TPC.Polymorphic;

import org.springframework.data.jpa.repository.JpaRepository;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.domain.models.TPC.Polymorphic.Poly_ConcreteB;
import com.Intent.Modules.Java.Tests.JPA.SqlServer.TestApplication.intent.IntentIgnoreBody;
import java.util.UUID;

@IntentIgnoreBody
public interface Poly_ConcreteBRepository extends JpaRepository<Poly_ConcreteB, UUID> {
}
